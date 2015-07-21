using System;
using GTA;
using GTA.Math;
using GTA.Native;
using System.Windows.Forms;
using System.Collections.Generic;
using Menu = GTA.Menu;
using System.IO;
using System.Drawing;

public class danknet : Script
{
    private bool noclip = false;
    private int speed = 10;
    private int helpup = 0;
    private int helpdown = 0;
    bool lasersight = false;
    bool isinv = false;
    bool huddisabled = false;
    bool mobileradio = false;
    bool sonicmode = false;
    bool isonblackout = false;
    bool timestopped = false;
    bool isforeveralone = false;
    bool iliekitinside = false;
    bool six_star = false;
    bool radarblips = false;
    bool never_wanted = false;
    bool unlimited_ammo = false;
    bool abletogesture = true;
    bool shootexp = false;
    bool shootexpbyme = false;
    bool deletegun = false;
    bool onehitkillgun = false;
    bool _100shotgun = false;
    bool tpgun = false;
    bool godgun = false;
    bool healgun = false;
    bool opendoorgun = false;
    bool markgun = false;
    Vehicle markedvehicle;
    Vehicle markedvehicle1;
    Vehicle markedvehicle2;
    Vehicle markedvehicle3;
    int curmark = 1;
    Menu Markmenu;
    Vehicle curveh;
    Vector3 tpfactor = new Vector3(0f, 0f, 3f);
    string configfile = "scripts\\danknetmenu.txt";
    private ScriptSettings settings;
    private Dictionary<Vector3, string> tplist = new Dictionary<Vector3, string>();
    string sectionname = "DANKNETMENU";
    string tpfilename = "scripts\\danknettplist.txt";

    public danknet()
    {
        settings = ScriptSettings.Load(configfile);
        this.View.MenuTransitions = true;
        Tick += OnTick;
        this.KeyDown += this.OnKeyDown;

        if (this.settings == null)
        {
            File.Create(configfile).Close();
            File.WriteAllText(configfile, ("[" + sectionname + "]"+ Environment.NewLine + "Enable/Disable_Menu = F6" + Environment.NewLine + "Forward_on_noclip = W" + Environment.NewLine + "Back_on_noclip = S" + Environment.NewLine + "Left_on_noclip = A" + Environment.NewLine + "Right_on_noclip = D" + Environment.NewLine + "Increase_speed_on_noclip = Add" + Environment.NewLine + "Increase_speed_on_noclip = Subtract"));
            this.settings = ScriptSettings.Load(configfile);
            this.settings.SetValue<Keys>(sectionname, "Enable/Disable_Menu", Keys.F6);
            this.settings.SetValue<Keys>(sectionname, "Forward_on_noclip", Keys.W);
            this.settings.SetValue<Keys>(sectionname, "Back_on_noclip", Keys.S);
            this.settings.SetValue<Keys>(sectionname, "Left_on_noclip", Keys.A);
            this.settings.SetValue<Keys>(sectionname, "Right_on_noclip", Keys.D);
            this.settings.SetValue<Keys>(sectionname, "Increase_speed_on_noclip", Keys.Add);
            this.settings.SetValue<Keys>(sectionname, "Decrease_speed_on_noclip", Keys.Subtract);
        }

        if (File.Exists(tpfilename))
        {
            List<string> readen = new List<string>();
            readen.AddRange(File.ReadAllLines(tpfilename)); //x newline y newline z newline name

            string nagme; //name
            Vector3 vec = new Vector3();
            int no = 0;
            foreach (string readed in readen)
            {
                no++;
                try
                {
                    if (no == 1)
                    {
                        vec.X = float.Parse(readed);
                    }
                    else if (no == 2)
                    {
                        vec.Y = float.Parse(readed);
                    }
                    else if (no == 3)
                    {
                        vec.Z = float.Parse(readed);
                    }
                    else if (no == 4)
                    {
                        nagme = readed;
                        no = 0;
                        tplist.Add(vec, nagme);
                    }
                }
                catch
                {
                    //this one is dangerous, file with bad format can crash script.
                }
            }
        }
        else
        {
            File.Create(tpfilename).Close();
        }

    }
    #region
    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == this.settings.GetValue<Keys>(sectionname, "Enable/Disable_Menu", Keys.F6))
        {
            if (this.View.ActiveMenus == 0)
            {
                if (!Function.Call<bool>(Hash.IS_DLC_PRESENT, -1762644250)) //checks if rockstar developer and doesn't open if developer
                {
                    this.OpenTrainerMenu();
                }
            }
            else
            {
                this.CloseTrainerMenu();
            }
        }
    }

    private void CloseTrainerMenu()
    {
        this.View.CloseAllMenus();
    }

    private void OpenTrainerMenu()
    {
        var menuItems = new List<IMenuItem> { };
        //add stuff here
        var button = new MenuButton("Player Menu", "TODO:EDIT THIS");
        button.Activated += (sender, args) => this.OpenPlayerMenu(Game.Player.Character);
        menuItems.Add(button);

        button = new MenuButton("Spawn Menu", "TODO:EDIT THIS");
        button.Activated += (sender, args) => this.OpenSpawnMenu();
        menuItems.Add(button);

        button = new MenuButton("Weapon Menu", "TODO:EDIT THIS");
        button.Activated += (sender, args) => this.OpenWeaponMenu();
        menuItems.Add(button);

        button = new MenuButton("World Menu", "TODO:EDIT THIS");
        button.Activated += (sender, args) => this.OpenWorldMenu();
        menuItems.Add(button);

        button = new MenuButton("Cop Menu", "TODO:EDIT THIS");
        button.Activated += (sender, args) => this.OpenCopsMenu();
        menuItems.Add(button);

        button = new MenuButton("Vehicle Menu", "TODO:EDIT THIS");
        button.Activated += (sender, args) => this.OpenVehicleMenu(Game.Player.Character.CurrentVehicle);
        menuItems.Add(button);

        button = new MenuButton("Teleport Menu", "Beam me up Dank-ty!");
        button.Activated += (sender, args) => this.OpenTeleportMenu();
        menuItems.Add(button);

        button = new MenuButton("(Ped) Mark Menu", "TP to car etc.");
        button.Activated += (sender, args) => this.OpenMarkMenu(); //TODO: Actually implement this
        menuItems.Add(button);

        button = new MenuButton("(Vehicle) Mark Menu", "TP to car etc.");
        button.Activated += (sender, args) => this.OpenMarkMenu();
        menuItems.Add(button);

        button = new MenuButton("About and how to use", "TODO:EDIT THIS");
        button.Activated += (sender, args) => this.OpenAbout();
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Danknet Menu v0.1", menuItems.ToArray()));
    }

    private void OpenMarkMenu()
    {
        var menuItems = new List<IMenuItem>();
        var text = (markedvehicle != null) ? new MenuLabel(("Marked Vehicle: " + markedvehicle.FriendlyName), true) : new MenuLabel(("Marked Vehicle: None"), true);
        menuItems.Add(text);

        if (Game.Player.Character.IsInVehicle())
        {
            var button = new MenuButton("Mark the vehicle I'm in", (Game.Player.Character.CurrentVehicle.FriendlyName));
            button.Activated += (sender, args) => this.MarkVehicleImIn();
            menuItems.Add(button);
        }
        else
        {
            text = new MenuLabel("You are not in a Vehicle");
            menuItems.Add(text);
        }

        var toggle = new MenuToggle("Use markgun", "Just aim at a vehicle", markgun);
        toggle.Changed += (sender, args) =>
        {
            var tg = sender as MenuToggle;
            if (tg == null)
            {
                return;
            }
            markgun = tg.Value;
        };
        menuItems.Add(toggle);

        if (Game.Player.LastVehicle != null || Game.Player.LastVehicle.FriendlyName != "NULL")
        {
            var button = new MenuButton("Mark my last vehicle", Game.Player.LastVehicle.FriendlyName);
            button.Activated += (sender, args) => this.MarkLastVehicle();
            menuItems.Add(button);
        }
        else
        {
            text = new MenuLabel("You weren't in a Vehicle");
            menuItems.Add(text);
        }

        if (markedvehicle != null && markedvehicle.Exists())
        {
            var button = new MenuButton("Unmark", "");
            button.Activated += (sender, args) => this.UnMark();
            menuItems.Add(button);
            if (markedvehicle1 != null && markedvehicle1.FriendlyName != "NULL" && markedvehicle1.Exists())
            {
                button = new MenuButton("Open Marked Menu 1", "");
                button.Activated += (sender, args) => this.OpenVehicleMenu(markedvehicle1);
                menuItems.Add(button);
                Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, (Entity)markedvehicle1, true);
            }
            else
            {
                markedvehicle1 = null;
            }

            if (markedvehicle2 != null && markedvehicle2.FriendlyName != "NULL" && markedvehicle2.Exists())
            {
                button = new MenuButton("Open Marked Menu 2", "");
                button.Activated += (sender, args) => this.OpenVehicleMenu(markedvehicle2);
                menuItems.Add(button);
                Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, (Entity)markedvehicle2, true);
            }
            else
            {
                markedvehicle2 = null;
            }

            if (markedvehicle3 != null && markedvehicle3.FriendlyName != "NULL" && markedvehicle3.Exists())
            {
                button = new MenuButton("Open Marked Menu 3", "");
                button.Activated += (sender, args) => this.OpenVehicleMenu(markedvehicle3);
                menuItems.Add(button);
                Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, (Entity)markedvehicle3, true);
            }
            else
            {
                markedvehicle3 = null;
            }
        }
        else
        {
            markedvehicle = null;
            if (curmark == 1)
            {
                markedvehicle1 = markedvehicle;
            }
            else if (curmark == 2)
            {
                markedvehicle2 = markedvehicle;
            }
            else if (curmark == 3)
            {
                markedvehicle3 = markedvehicle;
            }
        }

        var numero = new MenuNumericScroller(("Switch Marked Vehicle"), "", 1, 3, 1, (curmark - 1));
        numero.Changed += numero_Activated;
        menuItems.Add(numero);

        Markmenu = new Menu("Mark Menu", menuItems.ToArray());
        this.View.AddMenu(Markmenu);
    }

    private void gotowaypoint()
    {
        int teleportIteratrions = -1;
        if (teleportIteratrions >= -1)
        {
            teleportIteratrions = tptowaypoint(Game.Player.Character, teleportIteratrions);
        }
    }

    private void OpenTeleportMenu()
    {
        var menuItems = new List<IMenuItem>();

        //double n cuz can be removed
        var buttonn = new MenuButton("Go to waypoint", "Te-le-pooort: \ngotta save them all!");
        buttonn.Activated += (sender, args) => this.gotowaypoint();
        menuItems.Add(buttonn);

        var button = new MenuButton("Add current location to list", "Te-le-pooort: \ngotta save them all!");
        button.Activated += (sender, args) => this.addcurrenttotplist2();
        menuItems.Add(button);

        button = new MenuButton("Go to Custom Location", "First x, then y and z.");
        button.Activated += (sender, args) => this.Telep(new Vector3(float.Parse(Game.GetUserInput(8).Replace(".", ",")), float.Parse(Game.GetUserInput(8).Replace(".", ",")), float.Parse(Game.GetUserInput(8).Replace(".", ","))));
        menuItems.Add(button);

        int currentno = 0;
        foreach (Vector3 loc in tplist.Keys)
        {
            if (currentno == 10)
            {
                button = new MenuButton("Page 2", "See page 2");
                button.Activated += (sender, args) => this.OpenTeleportMenu2(2);
                menuItems.Add(button);
                break;
            }
            currentno++;
            string nagme;
            tplist.TryGetValue(loc, out nagme);

            button = new MenuButton(nagme, (loc.X + " " + loc.Y + " " + loc.Z));
            button.Activated += (sender, args) => this.Telep(loc);
            menuItems.Add(button);
        }

        this.View.AddMenu(new Menu("Teleport Menu", menuItems.ToArray()));
    }

    private void opentp2(int page)
    {
        if (page == 2)
        {
            View.RemoveMenu(lastmenu);
        }
        OpenTeleportMenu2(page);
    }

    Menu lastmenu;
    private void OpenTeleportMenu2(int curpage)
    {
        var menuItems = new List<IMenuItem>();

        if (curpage > 2) //3 or bigger
        {
            View.RemoveMenu(lastmenu);
            var buttonm = new MenuButton(("Page " + (curpage - 1)), ("See page " + (curpage - 1)));
            buttonm.Activated += (sender, args) => this.opentp2(curpage - 1);
            menuItems.Add(buttonm);
        }

        Dictionary<Vector3, string> locationlist = new Dictionary<Vector3, string>();

        int currentno = 0;
        int skipno = 0;
        foreach (Vector3 loc in tplist.Keys)
        {
            if (skipno == (10 * (curpage - 1)))
            {
                if (currentno == 10)
                {
                    var buttonm = new MenuButton(("Page " + (curpage + 1)), ("See page " + (curpage + 1)));
                    buttonm.Activated += (sender, args) => this.OpenTeleportMenu2(curpage + 1);
                    menuItems.Add(buttonm);
                    break;
                }
                currentno++;
                string nagme;
                tplist.TryGetValue(loc, out nagme);

                var button = new MenuButton(nagme, (loc.X + " " + loc.Y + " " + loc.Z));
                button.Activated += (sender, args) => this.Telep(loc);
                menuItems.Add(button);
            }
            else
            {
                skipno++;
            }
        }
        Menu thismenu = new Menu(("Teleport Menu " + curpage), menuItems.ToArray());
        lastmenu = thismenu;
        this.View.AddMenu(thismenu);
    }

    private void OpenWeaponMenu()
    {
        var menuItems = new List<IMenuItem>();
        var toggle = new MenuToggle("Laser Sight", "BOOM HEADSHOT", lasersight);
        toggle.Changed += (sender, args) =>
        {
            var tg = sender as MenuToggle;
            if (tg == null)
            {
                return;
            }
            lasersight = tg.Value;
            this.lazergun(tg.Value);
        };
        menuItems.Add(toggle);

        toggle = new MenuToggle("Unlimited ammo", "Rek dem all", unlimited_ammo);
        toggle.Changed += (sender, args) =>
        {
            var tg = sender as MenuToggle;
            if (tg == null)
            {
                return;
            }
            unlimited_ammo = tg.Value;
        };
        menuItems.Add(toggle);

        toggle = new MenuToggle("Shoot explosion", "Rek dem all", shootexp);
        toggle.Changed += (sender, args) =>
        {
            var tg = sender as MenuToggle;
            if (tg == null)
            {
                return;
            }
            shootexp = tg.Value;
        };
        menuItems.Add(toggle);

        toggle = new MenuToggle("Shoot explosion by me", "Rek dem all", shootexpbyme);
        toggle.Changed += (sender, args) =>
        {
            var tg = sender as MenuToggle;
            if (tg == null)
            {
                return;
            }
            shootexpbyme = tg.Value;
        };
        menuItems.Add(toggle);

        toggle = new MenuToggle("Delete gun", "get ready for crash", deletegun);
        toggle.Changed += (sender, args) =>
        {
            var tg = sender as MenuToggle;
            if (tg == null)
            {
                return;
            }
            deletegun = tg.Value;
        };
        menuItems.Add(toggle);

        toggle = new MenuToggle("One hit kill gun", "get ready for crash", onehitkillgun);
        toggle.Changed += (sender, args) =>
        {
            var tg = sender as MenuToggle;
            if (tg == null)
            {
                return;
            }
            onehitkillgun = tg.Value;
        };
        menuItems.Add(toggle);

        toggle = new MenuToggle("100 shot gun", "get ready for lag", _100shotgun);
        toggle.Changed += (sender, args) =>
        {
            var tg = sender as MenuToggle;
            if (tg == null)
            {
                return;
            }
            _100shotgun = tg.Value;
        };
        menuItems.Add(toggle);

        toggle = new MenuToggle("Teleport gun", "get ready for crash", tpgun);
        toggle.Changed += (sender, args) =>
        {
            var tg = sender as MenuToggle;
            if (tg == null)
            {
                return;
            }
            tpgun = tg.Value;
        };
        menuItems.Add(toggle);

        toggle = new MenuToggle("Godmode gun", "ezpz heists", godgun);
        toggle.Changed += (sender, args) =>
        {
            var tg = sender as MenuToggle;
            if (tg == null)
            {
                return;
            }
            godgun = tg.Value;
        };
        menuItems.Add(toggle);

        toggle = new MenuToggle("Open Door gun", "titan made easier", opendoorgun);
        toggle.Changed += (sender, args) =>
        {
            var tg = sender as MenuToggle;
            if (tg == null)
            {
                return;
            }
            opendoorgun = tg.Value;
        };
        menuItems.Add(toggle);

        //opendoorgun
        toggle = new MenuToggle("Heal gun", "ezpz heists", healgun);
        toggle.Changed += (sender, args) =>
        {
            var tg = sender as MenuToggle;
            if (tg == null)
            {
                return;
            }
            healgun = tg.Value;
        };
        menuItems.Add(toggle);

        var button = new MenuButton("Unlock all guns", "stun em all");
        button.Activated += (sender, args) => this.UnlockAllWeapons();
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Weapon Stuff", menuItems.ToArray()));
    }
    private void OpenVehicleMenu(Vehicle veh)
    {
        var menuItems = new List<IMenuItem>();
        curveh = veh;
        List<VehicleColor> colorlist = new List<VehicleColor>();
        colorlist.AddRange((IEnumerable<VehicleColor>)Enum.GetValues(typeof(VehicleColor)));
        List<string> colornamelist = new List<string>();
        int clrno = 0;
        int crntno = 0;
        foreach (VehicleColor clr in colorlist)
        {
            crntno++;
            if (veh.PrimaryColor.ToString() == clr.ToString())
            {
                clrno = crntno;
            }
            colornamelist.Add(clr.ToString());
        }

        var enumm = new MenuEnumScroller("Color", "Chrome, Epsilon, we have all", colornamelist.ToArray(), (clrno - 1));
        enumm.Activated += enumm_Activated;
        menuItems.Add(enumm);

        var button = new MenuButton("Open All Doors", "titan ftw");
        button.Activated += (sender, args) => this.Dooropen(veh);
        menuItems.Add(button);

        button = new MenuButton("Close All Doors", "titan ftw");
        button.Activated += (sender, args) => this.Doorclose(veh);
        menuItems.Add(button);

        button = new MenuButton("Change Plate", "SWAGYOLO ftw");
        button.Activated += (sender, args) => this.changeplate(veh);
        menuItems.Add(button);

        var numero = new MenuNumericScroller("Light multiplier", "", 1, 1001, 10);
        numero.Changed += numero_Changed;
        menuItems.Add(numero);

        var numero2 = new MenuNumericScroller("Speed multiplier", "", 1, 1001, 10);
        numero2.Changed += numero2_Changed;
        menuItems.Add(numero2);

        button = new MenuButton("Pimp My Ride", "Nice Wheels m8");
        button.Activated += (sender, args) => this.pimpmyride(veh);
        menuItems.Add(button);

        button = new MenuButton("Depimp My Ride", "n00b Wheels m8");
        button.Activated += (sender, args) => this.depimpmyride(veh);
        menuItems.Add(button);

        button = new MenuButton("Fix Dem Ride", "nice n clean");
        button.Activated += (sender, args) => this.FixRide(veh);
        menuItems.Add(button);

        button = new MenuButton("Fix & God Dem Ride", "Dem wheels is broken");
        button.Activated += (sender, args) => this.FixGodRide(veh);
        menuItems.Add(button);

        button = new MenuButton("RC Menu", "BTTF");
        button.Activated += (sender, args) => this.OpenVehicleRCMenu(veh);
        menuItems.Add(button);

        button = new MenuButton("TP Menu", "Bzzzt");
        button.Activated += (sender, args) => this.OpenVehicleTPMenu(veh);
        menuItems.Add(button);

        if (veh.FriendlyName.Length > 7)
        {
            this.View.AddMenu(new Menu(("Vehicle Menu (" + veh.FriendlyName.Substring(0,7) + ")"), menuItems.ToArray()));
        }
        else
        {
            this.View.AddMenu(new Menu(("Vehicle Menu (" + veh.FriendlyName + ")"), menuItems.ToArray()));
        }
    }

    private void OpenVehicleTPMenu(Vehicle veh)
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Teleport me to it", "");
        button.Activated += (sender, args) => this.TpToVehicle(veh);
        menuItems.Add(button);
        button = new MenuButton("Teleport me into it", "");
        button.Activated += (sender, args) => this.TpIntoVehicle(veh);
        menuItems.Add(button);
        button = new MenuButton("Teleport it to me", "");
        button.Activated += (sender, args) => this.TpVehicleToMe(veh);
        menuItems.Add(button);

        if (veh.FriendlyName.Length > 6)
        {
            this.View.AddMenu(new Menu(("Vehicle TP Menu (" + veh.FriendlyName.Substring(0, 6) + ")"), menuItems.ToArray()));
        }
        else
        {
            this.View.AddMenu(new Menu(("Vehicle TP Menu (" + veh.FriendlyName + ")"), menuItems.ToArray()));
        }
    }

    private void OpenVehicleRCMenu(Vehicle veh)
    {
        var menuItems = new List<IMenuItem>();
        var button = new MenuButton("Explode", "BOOOM");
        button.Activated += (sender, args) => this.ExplodeVehicle(veh);
        menuItems.Add(button);

        var numero3 = new MenuNumericScroller("RC: Forward", "BTTF IS REAL", 1, 1001, 10);
        numero3.Activated += numero3_Changed;
        menuItems.Add(numero3);

        button = new MenuButton("RC: Stop", "BTTF IS REAL");
        button.Activated += (sender, args) => car_go(veh, 0f);
        menuItems.Add(button);

        var numero4 = new MenuNumericScroller("RC: Use Horn", "DOOOOOOOOOOOOOOOT", 1000, 100000, 1000);
        numero4.Activated += numero4_Changed;
        menuItems.Add(numero4);


        var togglee = new MenuToggle("RC: Control Engine", "FTTF FTW", veh.EngineRunning);
        togglee.Changed += (sender, args) =>
        {
            var tg = sender as MenuToggle;
            if (tg == null)
            {
                return;
            }
            veh.EngineRunning = tg.Value;
            veh.LightsOn = tg.Value;
        };
        menuItems.Add(togglee);

        if (veh.FriendlyName.Length > 6)
        {
            this.View.AddMenu(new Menu(("Vehicle RC Menu (" + veh.FriendlyName.Substring(0,6) + ")"), menuItems.ToArray()));
        }
        else
        {
            this.View.AddMenu(new Menu(("Vehicle RC Menu (" + veh.FriendlyName + ")"), menuItems.ToArray()));
        }
    }

    void numero4_Changed(object sender, MenuItemDoubleValueArgs e)
    {
        int duration = (int)(((MenuNumericScroller)sender).Value + 1);
        curveh.SoundHorn(duration);
        //veh.SoundHorn(x * 1000)
    }

    private void OpenAbout()
    {
        var menuItems = new List<IMenuItem>();

        var text = new MenuLabel("Made by Ardaozkal", true);
        menuItems.Add(text);

        text = new MenuLabel("version 0.1", false);
        menuItems.Add(text);

        text = new MenuLabel("Num 2 and 8 to scroll", false);
        menuItems.Add(text);

        text = new MenuLabel("5 to select", false);
        menuItems.Add(text);

        text = new MenuLabel("Num 0 to go to prev menu", false);
        menuItems.Add(text);

        this.View.AddMenu(new Menu("About & How to use", menuItems.ToArray()));
    }

    private void OpenSpawnMenu()
    {
        var menuItems = new List<IMenuItem>();
        //TODO: Add all vehicles here, preferably by categories.
        var button = new MenuButton("Spawn Vacca", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Vacca, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Surano", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Surano, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Tornado", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Tornado, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Super Diamond", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Superd, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Double-T", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Double, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        this.View.AddMenu(new Menu("About & How to use", menuItems.ToArray()));
    }

    private void OpenPlayerMenu(Ped playa)
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Heal fully", "Gives the player \n100% health");
        button.Activated += (sender, args) => this.HealPlayer(playa);
        menuItems.Add(button);

        button = new MenuButton("Force Suicide", "Ded");
        button.Activated += (sender, args) => this.ForceSuicide(playa);
        menuItems.Add(button);

        button = new MenuButton("Breathe Fire", "*fire sound*");
        button.Activated += (sender, args) => this.BreatheFire(playa);
        menuItems.Add(button);

        button = new MenuButton("Freeze", "");
        button.Activated += (sender, args) => this.Freeze(playa);
        menuItems.Add(button);

        var toggle = new MenuToggle("Godmode", "U cant die m8", isinv);
        toggle.Changed += (sender, args) =>
        {
            var tg = sender as MenuToggle;
            if (tg == null)
            {
                return;
            }
            playa.IsInvincible = tg.Value;
            isinv = tg.Value;
        };
        menuItems.Add(toggle);

        toggle = new MenuToggle("Able to Ragdoll", "TODO: Edit this", Game.Player.Character.CanRagdoll);
        toggle.Changed += (sender, args) =>
        {
            var tg = sender as MenuToggle;
            if (tg == null)
            {
                return;
            }
            playa.CanRagdoll = tg.Value;
        };
        menuItems.Add(toggle);

        toggle = new MenuToggle("Able to Gesture", "Dock ftw", abletogesture);
        toggle.Changed += (sender, args) =>
        {
            var tg = sender as MenuToggle;
            if (tg == null)
            {
                return;
            }
            abletogesture = tg.Value;
            playa.CanPlayGestures = tg.Value;
        };
        menuItems.Add(toggle);

        if (playa == Game.Player.Character)
        {
            button = new MenuButton("Add SP money", "get rich m8");
            button.Activated += (sender, args) => this.GetRich();
            menuItems.Add(button);

            button = new MenuButton("Unlock Achievements", "b careful m8");
            button.Activated += (sender, args) => this.Achihack();
            menuItems.Add(button);

        toggle = new MenuToggle("Disable HUD", "bo2 calls hardcore,\n insurgent calls casual", huddisabled);
        toggle.Changed += (sender, args) =>
        {
            var tg = sender as MenuToggle;
            if (tg == null)
            {
                return;
            }
            Function.Call(Hash.DISPLAY_HUD, !tg.Value);
            Function.Call(Hash.DISPLAY_RADAR, !tg.Value);
            huddisabled = tg.Value;
        };
        menuItems.Add(toggle);

        toggle = new MenuToggle("No fall/drag outta car", "TODO:Add smth here", iliekitinside);
        toggle.Changed += (sender, args) =>
        {
            var tgg = sender as MenuToggle;
            if (tgg == null)
            {
                return;
            }
            iliekitinside = tgg.Value;
        };
        menuItems.Add(toggle);

        toggle = new MenuToggle("Mobile Radio", "SR4 Style", mobileradio);
        toggle.Changed += (sender, args) =>
        {
            var tg = sender as MenuToggle;
            if (tg == null)
            {
                return;
            }
            Function.Call(Hash._0x2F7CEB6520288061, tg.Value);
            Function.Call(Hash.SET_AUDIO_FLAG, "AllowRadioDuringSwitch", tg.Value);
            Function.Call(Hash.SET_MOBILE_PHONE_RADIO_STATE, tg.Value);
            Function.Call(Hash.SET_AUDIO_FLAG, "MobileRadioInGame", tg.Value);
            mobileradio = tg.Value;
        };
        menuItems.Add(toggle);

        button = new MenuButton("Skip Radio Song", "Skips Song");
        button.Activated += (sender, args) => Function.Call(Hash.SKIP_RADIO_FORWARD);
        menuItems.Add(button);

        toggle = new MenuToggle("Faster Move", "Gata go fast", sonicmode);
        toggle.Changed += (sender, args) =>
        {
            var tg = sender as MenuToggle;
            if (tg == null)
            {
                return;
            }
            sonicmode = tg.Value;
            if (tg.Value)
            {
                Function.Call(Hash._SET_SWIM_SPEED_MULTIPLIER, Game.Player, 1.49f);
                Function.Call(Hash._SET_MOVE_SPEED_MULTIPLIER, Game.Player, 1.49f);
            }
            else
            {
                Function.Call(Hash._SET_SWIM_SPEED_MULTIPLIER, Game.Player, 1f);
                Function.Call(Hash._SET_MOVE_SPEED_MULTIPLIER, Game.Player, 1f);
            }
        };
        menuItems.Add(toggle);

        button = new MenuButton("Noclip", "use first person \n to change facing way \n num+/- to change speed");
        button.Activated += (sender, args) => this.togglenoclip();
        menuItems.Add(button);
        }

        this.View.AddMenu(new Menu("Player Stuff", menuItems.ToArray()));
    }

    private void OpenWorldMenu()
    {
        var menuItems = new List<IMenuItem>();

        var toggle = new MenuToggle("Blackout", "GTAV_DAWGS", isonblackout);
        toggle.Changed += (sender, args) =>
        {
            var tg = sender as MenuToggle;
            if (tg == null)
            {
                return;
            }
            isonblackout = tg.Value;
            Blackout(tg.Value);
        };
        menuItems.Add(toggle);

        toggle = new MenuToggle("Stop Time", "TODO: Add smth here", timestopped);
        toggle.Changed += (sender, args) =>
        {
            var tg = sender as MenuToggle;
            if (tg == null)
            {
                return;
            }
            timestopped = tg.Value;
            Stoptime(tg.Value);
        };
        menuItems.Add(toggle);
        //MenuNumericScroller

        var text = new MenuLabel("Time: " + World.CurrentDayTime.Hours + ":" + World.CurrentDayTime.Minutes, false);
        menuItems.Add(text); //TODO: MAKE THIS UPDATE

        var button = new MenuButton("+1 hour", "time flies yo");
        button.Activated += (sender, args) => Function.Call(Hash.ADD_TO_CLOCK_TIME, 1, 0, 0);
        menuItems.Add(button);

        button = new MenuButton("-1 hour", "time flies yo");
        button.Activated += (sender, args) => Function.Call(Hash.ADD_TO_CLOCK_TIME, -1, 0, 0);
        menuItems.Add(button);

        button = new MenuButton("Destroy Cameras", "ezpz heists (untested)");
        button.Activated += (sender, args) => GTA.World.DestroyAllCameras();
        menuItems.Add(button);


        button = new MenuButton("Weather: Clean", "Watch dem clouds");
        button.Activated += (sender, args) => this.setweather(Weather.Clear);
        menuItems.Add(button);

        button = new MenuButton("Weather: ExtraSunny", "Watch dem clouds");
        button.Activated += (sender, args) => this.setweather(Weather.ExtraSunny);
        menuItems.Add(button);

        button = new MenuButton("Weather: Christmas", "Watch dem clouds");
        button.Activated += (sender, args) => this.setweather(Weather.Christmas);
        menuItems.Add(button);

        button = new MenuButton("Weather: Snow", "Watch dem clouds");
        button.Activated += (sender, args) => this.setweather(Weather.Snowing);
        menuItems.Add(button);

        button = new MenuButton("Weather: Rain", "Watch dem clouds");
        button.Activated += (sender, args) => this.setweather(Weather.Raining);
        menuItems.Add(button);

        button = new MenuButton("Weather: Blizzard", "Watch dem clouds");
        button.Activated += (sender, args) => this.setweather(Weather.Blizzard);
        menuItems.Add(button);

        //button = new MenuButton("Goto Mission Marker" + GTA.World.GetActiveBlips().Length, "ezpz races");
        //button.Activated += (sender, args) => this.GotoMissionMarker2();
        //menuItems.Add(button);

        this.View.AddMenu(new Menu("World Stuff", menuItems.ToArray()));
    }

    private void OpenCopsMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Increase Heat", "B careful m8");
        button.Activated += (sender, args) => this.MoreCops();
        menuItems.Add(button);
        button = new MenuButton("Lower Heat", "Better Call Saul");
        button.Activated += (sender, args) => this.LessCops();
        menuItems.Add(button);
        button = new MenuButton("Clear Heat", "Shake 'em off");
        button.Activated += (sender, args) => this.ClearCops();
        menuItems.Add(button);

        var toggle = new MenuToggle("Never wanted", "dem cops make me cri everytiem", never_wanted);
        toggle.Changed += (sender, args) =>
        {
            var tggg = sender as MenuToggle;
            if (tggg == null)
            {
                return;
            }
            never_wanted = tggg.Value;
        };
        menuItems.Add(toggle);

        toggle = new MenuToggle("Get Ignored", "Forever Alone", isforeveralone);
        toggle.Changed += (sender, args) =>
        {
            var tggg = sender as MenuToggle;
            if (tggg == null)
            {
                return;
            }
            isforeveralone = tggg.Value;
            Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, Game.Player, tggg.Value);
        };
        menuItems.Add(toggle);

        toggle = new MenuToggle("Invisible Police Radar Blips", "where r u", radarblips);
        toggle.Changed += (sender, args) =>
        {
            var tggg = sender as MenuToggle;
            if (tggg == null)
            {
                return;
            }
            radarblips = tggg.Value;
            Function.Call(Hash.SET_POLICE_RADAR_BLIPS, !tggg.Value);
        };
        menuItems.Add(toggle);

        toggle = new MenuToggle("Max 6 Star Wanted Level", "5 stars r 2ez4me", six_star);
        toggle.Changed += (sender, args) =>
        {
            var tggg = sender as MenuToggle;
            if (tggg == null)
            {
                return;
            }
            six_star = tggg.Value;
            if (tggg.Value)
            {
                Function.Call(Hash.SET_MAX_WANTED_LEVEL, 6);
            }
            else
            {
                Function.Call(Hash.SET_MAX_WANTED_LEVEL, 5);
            }
        };
        menuItems.Add(toggle);

        this.View.AddMenu(new Menu("Cop Stuff", menuItems.ToArray()));
    }
    #endregion

    #region Commands

    private Vehicle SpawnCar(VehicleHash vehname, Vector3 pos, float heading)
    {
        return World.CreateVehicle(((Model)vehname), pos, heading);
    }

    int tptowaypoint(Ped playerPed, int iterationno)
    {
        Entity e = playerPed;
        Vehicle playerVeh = null;
        if (Game.Player.Character.IsInVehicle())
        {
            playerVeh = Game.Player.Character.CurrentVehicle;
        }
        Vector3 coords = new Vector3(0f,0f,0f);
        Vector3 oldLocation = new Vector3(0f, 0f, 0f);
        if (iterationno == -1)
        {
            int blipIterator = Function.Call<int>(Hash._GET_BLIP_INFO_ID_ITERATOR);
		for (int i = Function.Call<int>(Hash.GET_FIRST_BLIP_INFO_ID, blipIterator); Function.Call<int>(Hash.DOES_BLIP_EXIST, i) != 0; i = Function.Call<int>(Hash.GET_NEXT_BLIP_INFO_ID, blipIterator))
		{
			if (Function.Call<int>(Hash.GET_BLIP_INFO_ID_TYPE, i) == 4)
            {
                coords = Function.Call<Vector3>(Hash.GET_BLIP_INFO_ID_COORD, i);
                if (playerVeh != null)
                {
                    playerVeh.Position = new Vector3(coords.X, coords.Y, 10000f);
                    playerVeh.PlaceOnGround();
                }
                else
                {
                    int type = 1;
                    if (type == 1)
                    {
                        Game.Player.Character.Position = new Vector3(coords.X, coords.Y, (World.GetGroundHeight(coords) + 4));
                    }
                    else
                    {
                        playerVeh = SpawnCar(VehicleHash.Zentorno, Game.Player.Character.Position, Game.Player.Character.Heading);
                        Game.Player.Character.SetIntoVehicle(playerVeh, VehicleSeat.Driver);
                        playerVeh.Position = new Vector3(coords.X, coords.Y, 10000f);
                        playerVeh.PlaceOnGround();
                        playerPed.Position += new Vector3(0f, 0f, 0.1f); //aka not in car
                        playerVeh.Delete();
                    }
                }
                iterationno = 19;
				oldLocation = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, e, false);
				break;
			}
		}
	}
	if (iterationno > -1)
	{
		bool groundFound = false;
        
		Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, e, coords.X, coords.Y, 800 - (iterationno*50), false, false, true);
		if (Function.Call<bool>(Hash.GET_GROUND_Z_FOR_3D_COORD, coords.X, coords.Y, 800 - (iterationno*50), coords.Z) == true)
		{
			groundFound = true;
			Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, e, coords.X, coords.Y, coords.Z + 3, false, false, true);
			iterationno = -1;
		}
		if (iterationno > -1)
			iterationno--;
		if (!groundFound && iterationno < 0)
		{
			Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, e, oldLocation.X, oldLocation.Y, oldLocation.Z, false, false, true);
		}
	}
	return iterationno;
    }

    void BreatheFire(Ped selectedPed)
    {
        Vector3 Mouth = Function.Call<Vector3>(Hash.GET_PED_BONE_COORDS, selectedPed, (int)Bone.SKEL_ROOT, 0.1f, 0.0f, 0.0f);
        Function.Call(Hash._ADD_SPECFX_EXPLOSION, Mouth.X, Mouth.Y, Mouth.Z, 12, 12, 1.0f, true, true, 0.0f);
    }

    void enumm_Activated(object sender, MenuItemIndexArgs e)
    {
        VehicleColor selcolor = (VehicleColor)Enum.GetValues(typeof(VehicleColor)).GetValue(e.Index);
        this.changecolor(curveh, selcolor);
    }

    void car_go(Vehicle veh, float speed)
    {
        veh.Speed = speed;
    }
    
    void numero_Changed(object sender, MenuItemDoubleValueArgs e)
    {
        brightnessmultiplier(curveh, (float)(((MenuNumericScroller)sender).Value + 1));
    }

    void numero2_Changed(object sender, MenuItemDoubleValueArgs e)
    {
        carspeedmultiplier(curveh, (float)(((MenuNumericScroller)sender).Value + 1));
    }

    void numero3_Changed(object sender, MenuItemDoubleValueArgs e)
    {
        float speed = (float)(((MenuNumericScroller)sender).Value + 1);
        car_go(curveh, speed);
    }

    void numero_Activated(object sender, MenuItemDoubleValueArgs e)
    {
        curmark = (int)((MenuNumericScroller)sender).Value + 1;

        if (curmark == 1)
        {
            markedvehicle = markedvehicle1;
        }
        else if (curmark == 2)
        {
            markedvehicle = markedvehicle2;
        }
        else if (curmark == 3)
        {
            markedvehicle = markedvehicle3;
        }
        else //error
        {
            curmark = 1;
        }
        View.RemoveMenu(Markmenu);
        OpenMarkMenu();
    }

    private void UnMark()
    {
        markedvehicle = null;
        if (curmark == 1)
        {
            markedvehicle1 = markedvehicle;
            Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, (Entity)markedvehicle1, false);
        }
        else if (curmark == 2)
        {
            markedvehicle2 = markedvehicle;
            Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, (Entity)markedvehicle2, false);
        }
        else if (curmark == 3)
        {
            markedvehicle3 = markedvehicle;
            Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, (Entity)markedvehicle3, false);
        }
        View.RemoveMenu(Markmenu);
        OpenMarkMenu();
    }

    private void TpVehicleToMe(Vehicle veh)
    {
        if (veh.Exists())
        {
            if (Game.Player.Character.IsInVehicle())
            {
                Game.Player.Character.CurrentVehicle.Position += tpfactor;
                veh.Position = (Game.Player.Character.CurrentVehicle.Position - tpfactor);
            }
            else
            {
                Game.Player.Character.Position += tpfactor;
                veh.Position = (Game.Player.Character.Position - tpfactor);
            }
        }
    }

    private void TpIntoVehicle(Vehicle veh)
    {
        if (veh.Exists())
        {
            if (veh.IsSeatFree(VehicleSeat.Driver))
            {
                Game.Player.Character.SetIntoVehicle(veh, VehicleSeat.Driver);
            }
            else
            {
                Game.Player.Character.SetIntoVehicle(veh, VehicleSeat.Any);
            }
        }
    }

    private void TpToVehicle(Vehicle veh)
    {
        if (veh.Exists())
        {
            if (Game.Player.Character.IsInVehicle())
            {
                Game.Player.Character.CurrentVehicle.Position = (veh.Position + tpfactor);
            }
            else
            {
                Game.Player.Character.Position = (veh.Position + tpfactor);
            }
        }
    }
    private void MarkVehicleImIn()
    {
        if (Game.Player.Character.IsInVehicle())
        {
            markedvehicle = Game.Player.Character.CurrentVehicle;
            if (curmark == 1)
            {
                if (markedvehicle1 != null)
                {
                    Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, (Entity)markedvehicle1, false);
                }
                markedvehicle1 = markedvehicle;
            }
            else if (curmark == 2)
            {
                if (markedvehicle2 != null)
                {
                    Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, (Entity)markedvehicle2, false);
                }
                markedvehicle2 = markedvehicle;
            }
            else if (curmark == 3)
            {
                if (markedvehicle1 != null)
                {
                    Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, (Entity)markedvehicle3, false);
                }
                markedvehicle3 = markedvehicle;
            }
            View.RemoveMenu(Markmenu);
            OpenMarkMenu();
        }
    }

    private void ExplodeVehicle(Vehicle veh)
    {
        if (veh.Exists())
        {
            World.AddExplosion(veh.Position, ExplosionType.BigExplosion1, 10f, 1f);
            //veh.Explode();
        }
    }

    private void MarkLastVehicle()
    {
        if (Game.Player.LastVehicle != null)
        {
            markedvehicle = Game.Player.LastVehicle;
            if (curmark == 1)
            {
                if (markedvehicle1 != null)
                {
                    Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, (Entity)markedvehicle1, false);
                }
                markedvehicle1 = markedvehicle;
            }
            else if (curmark == 2)
            {
                if (markedvehicle2 != null)
                {
                    Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, (Entity)markedvehicle2, false);
                }
                markedvehicle2 = markedvehicle;
            }
            else if (curmark == 3)
            {
                if (markedvehicle3 != null)
                {
                    Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, (Entity)markedvehicle3, false);
                }
                markedvehicle3 = markedvehicle;
            }
            View.RemoveMenu(Markmenu);
            OpenMarkMenu();
        }
    }

    private void ClearCops()
    {
        Function.Call(Hash.CLEAR_PLAYER_WANTED_LEVEL, Game.Player);
    }

    private void Telep(Vector3 loc)
    {
        if (Game.Player.Character.IsInVehicle())
        {
            Game.Player.Character.CurrentVehicle.Position = loc;
        }
        else
        {
            Game.Player.Character.Position = loc;
        }
    }

    private void addcurrenttotplist()
    {
        tplist.Add(Game.Player.Character.Position, (Game.GetUserInput(21) + " "));
        updatetplist();
    }

    private void addcurrenttotplist2()
    {
        addcurrenttotplist();
        CloseTrainerMenu();
        OpenTrainerMenu();
        OpenTeleportMenu();
    }

    private void updatetplist()
    {
        string textfile = "";
        foreach (Vector3 loc in tplist.Keys)
        {
            string nagme = "ERROR WITH NAME";
            tplist.TryGetValue(loc, out nagme);
            if (textfile != "")
            {
                textfile += Environment.NewLine;
            }
            textfile += loc.X + Environment.NewLine + loc.Y + Environment.NewLine + loc.Z + Environment.NewLine + nagme;
        }
        File.WriteAllText(tpfilename, textfile);
    }

    private void togglenoclip()
    {
        this.noclip = !this.noclip;
        if (!this.noclip)
        {
            Game.Player.Character.FreezePosition = false;
            if (Game.Player.Character.IsInVehicle())
            {
                Game.Player.Character.CurrentVehicle.FreezePosition = false;
            }
        }
    }
    private void setweather(Weather weatherr)
    {
        World.Weather = weatherr;
    }

    private void lazergun(bool on)
    {
        Function.Call(Hash.ENABLE_LASER_SIGHT_RENDERING, on);
    }

    private void pimpmyride(Vehicle veh)
    {
        veh.CanTiresBurst = false;
        veh.IsStolen = false;
        veh.DirtLevel = 0f;
        veh.SetMod(VehicleMod.Armor, 5, false);
        veh.SetMod(VehicleMod.Brakes, 3, false);
        veh.SetMod(VehicleMod.Engine, 4, false);
        veh.SetMod(VehicleMod.Suspension, 4, false);
        veh.SetMod(VehicleMod.Transmission, 3, false);
        veh.ToggleMod(VehicleToggleMod.Turbo, true);
        veh.ToggleMod(VehicleToggleMod.XenonHeadlights, true);
        veh.ToggleMod(VehicleToggleMod.TireSmoke, true);
    }

    private void depimpmyride(Vehicle veh)
    {
        //out of range = stock
        veh.CanTiresBurst = true;
        veh.SetMod(VehicleMod.Armor, 999, false);
        veh.SetMod(VehicleMod.Brakes, 999, false);
        veh.SetMod(VehicleMod.Engine, 999, false);
        veh.SetMod(VehicleMod.Suspension, 999, false);
        veh.SetMod(VehicleMod.Transmission, 999, false);
        veh.ToggleMod(VehicleToggleMod.Turbo, false);
        veh.ToggleMod(VehicleToggleMod.XenonHeadlights, false);
        veh.ToggleMod(VehicleToggleMod.TireSmoke, false);
    }

    private void carspeedmultiplier(Vehicle veh, float multiplier)
    {
        Function.Call(Hash._SET_VEHICLE_ENGINE_POWER_MULTIPLIER, veh, (multiplier * 1));
        Function.Call(Hash._SET_VEHICLE_ENGINE_TORQUE_MULTIPLIER, veh, (multiplier * 1));
    }

    private void brightnessmultiplier(Vehicle veh, float multiplier)
    {
        Function.Call(Hash.SET_VEHICLE_LIGHT_MULTIPLIER, veh, (multiplier * 10)); //*10 cuz default is 10
    }

    private void UnlockAllWeapons()
    {
        var msgBox = new GTA.MessageBox("U wanna \nunlock all dem gunz?");
        msgBox.Yes += (sender, args) => this.DoUnlockAllWeapons();
        this.View.AddMenu(msgBox);
    }

    private void changecolor(Vehicle veh, VehicleColor vehcolor)
    {
        veh.PrimaryColor = vehcolor;
        veh.SecondaryColor = vehcolor;
    }

    private void changeplate(Vehicle veh)
    {
        veh.NumberPlate = Game.GetUserInput(9); //8+1
    }

    private void Achihack()
    {
        for (int i = 1; i <= 57; i++)
        {
            Function.Call(Hash.GIVE_ACHIEVEMENT_TO_PLAYER, i); //TODO: check this
        }
    }

    private void Dooropen(Vehicle veh)
    {
        for (int i = 0; i <= 7; i++)
        {
            Function.Call(Hash.SET_VEHICLE_DOOR_OPEN, veh, i, true, true); //check this
        }
    }
    private void Doorclose(Vehicle veh)
    {
        for (int i = 0; i <= 7; i++)
        {
            Function.Call(Hash.SET_VEHICLE_DOOR_SHUT, veh, i, true); //check this
        }
    }

    private void GetRich()
    {
        Game.Player.Money = 2147483647; //sp money, 32bit int limit. Volv--- Rockstar increase to 64bit (decimal) plz.
    }

    private void HealPlayer(Ped player)
    {
        player.Health = player.MaxHealth;
        player.Armor = player.MaxHealth;
    }

    private void Blackout(bool value)
    {
        Function.Call(Hash._SET_BLACKOUT, value);
    }

    private void Stoptime(bool value)
    {
        Function.Call(Hash.PAUSE_CLOCK, value);
    }

    void ForceSuicide(Ped playa)
    {
        playa.AlwaysDiesOnLowHealth = true;
        playa.IsInvincible = false;
        playa.Armor = 0;
        playa.Health = 0;
    }

    void Freeze(Ped playa)
    {
        playa.FreezePosition = true;
        if (playa.IsInVehicle())
        {
            playa.CurrentVehicle.FreezePosition = true;
        }
    }

    void OnTick(object sender, EventArgs e)
    {
        if (never_wanted)
        {
            Game.Player.WantedLevel = 0;
        }

        Game.Player.Character.Weapons.Current.InfiniteAmmo = unlimited_ammo;
        Game.Player.Character.Weapons.Current.InfiniteAmmoClip = unlimited_ammo;

        Game.Player.Character.CanBeDraggedOutOfVehicle = iliekitinside;
        Game.Player.Character.CanBeKnockedOffBike = iliekitinside;

        lazergun(lasersight);

        Game.Player.Character.IsInvincible = isinv;

        Function.Call(Hash.DISPLAY_HUD, !huddisabled);
        Function.Call(Hash.DISPLAY_RADAR, !huddisabled);

        Function.Call(Hash._0x2F7CEB6520288061, mobileradio);
        Function.Call(Hash.SET_AUDIO_FLAG, "AllowRadioDuringSwitch", mobileradio);
        Function.Call(Hash.SET_MOBILE_PHONE_RADIO_STATE, mobileradio);
        Function.Call(Hash.SET_AUDIO_FLAG, "MobileRadioInGame", mobileradio);

        if (sonicmode)
        {
            Function.Call(Hash._SET_SWIM_SPEED_MULTIPLIER, Game.Player, 1.49f);
            Function.Call(Hash._SET_MOVE_SPEED_MULTIPLIER, Game.Player, 1.49f);
        }
        else
        {
            Function.Call(Hash._SET_SWIM_SPEED_MULTIPLIER, Game.Player, 1f);
            Function.Call(Hash._SET_MOVE_SPEED_MULTIPLIER, Game.Player, 1f);
        }

        Blackout(isonblackout);

        Stoptime(timestopped);

        Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, Game.Player, isforeveralone);

        if (six_star)
        {
            Function.Call(Hash.SET_MAX_WANTED_LEVEL, 6);
        }
        else
        {
            Function.Call(Hash.SET_MAX_WANTED_LEVEL, 5);
        }

        Function.Call(Hash.SET_POLICE_RADAR_BLIPS, !radarblips);

        Game.Player.Character.CanPlayGestures = abletogesture;

        try
        {
            if (shootexp && Game.Player.Character.IsShooting)
            {
                if (Game.Player.GetTargetedEntity().Exists())
                {
                    World.AddExplosion(Game.Player.GetTargetedEntity().Position, ExplosionType.BigExplosion1, 1.0f, 1.0f);
                }
            }

            if (tpgun && Game.Player.Character.IsShooting)
            {
                if (Game.Player.GetTargetedEntity().Exists())
                {
                    if (Game.Player.Character.IsInVehicle())
                    {
                        Game.Player.Character.CurrentVehicle.Position = Game.Player.GetTargetedEntity().Position;
                    }
                    else
                    {
                        Game.Player.Character.Position = Game.Player.GetTargetedEntity().Position;
                    }
                }
            }

            if (shootexpbyme && Game.Player.Character.IsShooting)
            {
                shootexp = false;
                if (Game.Player.GetTargetedEntity().Exists())
                {
                    World.AddOwnedExplosion(Game.Player.Character, Game.Player.GetTargetedEntity().Position, ExplosionType.BigExplosion1, 1.0f, 1.0f);
                }
            }


            if (_100shotgun && Game.Player.Character.IsShooting)
            {
                if (Game.Player.GetTargetedEntity().Exists())
                {
                    for (int a = 0; a <= 100; a++)
                    {
                        World.ShootBullet(Game.Player.GetTargetedEntity().Position, Game.Player.GetTargetedEntity().Position, Game.Player.Character, new Model(WeaponHash.Knife), 999);
                    }
                }
                //World.ShootBullet
            }

            if (markgun)
            {
                if (Game.Player.GetTargetedEntity().Exists() && Game.Player.GetTargetedEntity().Model.IsVehicle)
                {
                    if (curmark == 1)
                    {
                        if (markedvehicle1 != null)
                        {
                            Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, (Entity)markedvehicle1, false);
                        }
                        markedvehicle1 = (Vehicle)Game.Player.GetTargetedEntity();
                        markedvehicle = markedvehicle1;
                    }
                    else if (curmark == 2)
                    {
                        if (markedvehicle != null)
                        {
                            Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, (Entity)markedvehicle2, false);
                        }
                        markedvehicle2 = (Vehicle)Game.Player.GetTargetedEntity();
                        markedvehicle = markedvehicle2;
                    }
                    else if (curmark == 3)
                    {
                        if (markedvehicle3 != null)
                        {
                            Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, (Entity)markedvehicle3, false);
                        }
                        markedvehicle3 = (Vehicle)Game.Player.GetTargetedEntity();
                        markedvehicle = markedvehicle3;
                    }
                    markgun = false;
                    View.RemoveMenu(Markmenu);
                    OpenMarkMenu();
                }
            }

            if (opendoorgun && Game.Player.Character.IsShooting)
            {
                if (Game.Player.GetTargetedEntity().Exists() && Game.Player.GetTargetedEntity().Model.IsVehicle)
                {
                    Vehicle targetveh = (Vehicle)Game.Player.GetTargetedEntity();
                    for (int b = 0; b <= 7; b++)
                    {
                        Function.Call(Hash.SET_VEHICLE_DOOR_OPEN, targetveh, b, true, true); //check this
                    }
                }
            }

            if (healgun)
            {
                if (Game.Player.GetTargetedEntity().Exists() && Game.Player.GetTargetedEntity().Model.IsPed)
                {
                    Ped targetped = (Ped)Game.Player.GetTargetedEntity();
                    targetped.Health = targetped.MaxHealth;
                    targetped.Armor = targetped.MaxHealth;
                }
                else if (Game.Player.GetTargetedEntity().Exists() && Game.Player.GetTargetedEntity().Model.IsVehicle)
                {
                    Vehicle targetveh = (Vehicle)Game.Player.GetTargetedEntity();
                    targetveh.Repair();
                }
            }
            //Game.Player.Character.IsInvincible = tg.Value;

            if (deletegun && Game.Player.Character.IsShooting)
            {
                if (Game.Player.GetTargetedEntity().Exists())
                    Game.Player.GetTargetedEntity().Delete();
            }

            if (onehitkillgun && Game.Player.Character.IsShooting)
            {
                if (Game.Player.GetTargetedEntity().Exists())
                    Game.Player.GetTargetedEntity().Health = 0;
            }
        }
        catch
        {

        }

        #region noclip
        Vehicle[] nearbyVehicles;
        int i;
        Ped character = Game.Player.Character;
        if (this.noclip)
        {
            int handle = character.Handle;
            int ınt32 = this.speed - 1;
            UIText uIText = new UIText(string.Concat("Speed: ", ınt32.ToString()), new Point(500, 50), 0.4f, Color.White);
            uIText.Draw();
            Vector3 offsetInWorldCoords = character.GetOffsetInWorldCoords(new Vector3(0f, (float)(this.speed + this.helpup), 0f));
            Vector3 vector3 = character.GetOffsetInWorldCoords(new Vector3(0f, (float)(-this.speed - this.helpdown), 0f));
            Vector3 offsetInWorldCoords1 = character.GetOffsetInWorldCoords(new Vector3((float)(-this.speed - this.helpdown), 0f, 0f));
            Vector3 vector31 = character.GetOffsetInWorldCoords(new Vector3((float)(this.speed + this.helpup), 0f, 0f));
            Vector3 offsetInWorldCoords2 = character.GetOffsetInWorldCoords(new Vector3(0f, 0f, (float)(this.speed + this.helpup)));
            Vector3 vector32 = character.GetOffsetInWorldCoords(new Vector3(0f, 0f, (float)(-this.speed - this.helpdown)));
            Vector3 vector33 = character.Position;
            if (Game.IsKeyPressed(this.settings.GetValue<Keys>(sectionname, "Increase_speed_on_noclip", Keys.Add)))
            {
                this.speed = this.speed + 1;
            }
            else if (Game.IsKeyPressed(this.settings.GetValue<Keys>(sectionname, "Decrease_speed_on_noclip", Keys.Subtract)))
            {
                if (this.speed > 2)
                {
                    this.speed = this.speed - 1;
                }
            }
            if (character.IsInVehicle())
            {
                int handle1 = character.CurrentVehicle.Handle;
                character.CurrentVehicle.Position = vector33;
                character.CurrentVehicle.FreezePosition = true;
                nearbyVehicles = World.GetNearbyVehicles(character, 10f);
                if ((nearbyVehicles == null ? false : (int)nearbyVehicles.Length > 0))
                {
                    for (i = 0; i < (int)nearbyVehicles.Length; i++)
                    {
                        if (Entity.Exists(nearbyVehicles[i]))
                        {
                            if (nearbyVehicles[i] != character.CurrentVehicle)
                            {
                                nearbyVehicles[i].FreezePosition = false;
                            }
                        }
                    }
                }
                if ((character.CurrentVehicle.HeightAboveGround > 3f || character.CurrentVehicle.HeightAboveGround < 0f ? true : this.speed > 3))
                {
                    this.helpup = 0;
                    this.helpdown = 0;
                }
                else
                {
                    this.helpup = 3;
                    this.helpdown = 5;
                }
                if (Game.IsKeyPressed(this.settings.GetValue<Keys>(sectionname, "Forward_on_noclip", Keys.W)))
                {
                    character.CurrentVehicle.Position = offsetInWorldCoords;
                }
                if (Game.IsKeyPressed(this.settings.GetValue<Keys>(sectionname, "Back_on_noclip", Keys.S)))
                {
                    character.CurrentVehicle.Position = vector3;
                }
                if (Game.IsKeyPressed(this.settings.GetValue<Keys>(sectionname, "Left_on_noclip", Keys.A)))
                {
                    character.CurrentVehicle.Position = offsetInWorldCoords1;
                }
                if (Game.IsKeyPressed(this.settings.GetValue<Keys>(sectionname, "Right_on_noclip", Keys.D)))
                {
                    character.CurrentVehicle.Position = vector31;
                }
                if (Game.IsKeyPressed(this.settings.GetValue<Keys>(sectionname, "Up_on_noclip", Keys.Z)))
                {
                    character.CurrentVehicle.Position = offsetInWorldCoords2;
                }
                if (Game.IsKeyPressed(this.settings.GetValue<Keys>(sectionname, "Down_on_noclip", Keys.X)))
                {
                    character.CurrentVehicle.Position = vector32;
                }
            }
            else
            {
                character.FreezePosition = true;
                nearbyVehicles = World.GetNearbyVehicles(character, 10f);
                this.helpup = 0;
                this.helpdown = 0;
                if ((nearbyVehicles == null ? false : (int)nearbyVehicles.Length > 0))
                {
                    for (i = 0; i < (int)nearbyVehicles.Length; i++)
                    {
                        if (Entity.Exists(nearbyVehicles[i]))
                        {
                            nearbyVehicles[i].FreezePosition = false;
                        }
                    }
                }
                if (Game.IsKeyPressed(this.settings.GetValue<Keys>(sectionname, "Forward_on_noclip", Keys.W)))
                {
                    character.Position = offsetInWorldCoords;
                }
                if (Game.IsKeyPressed(this.settings.GetValue<Keys>(sectionname, "Back_on_noclip", Keys.S)))
                {
                    character.Position = vector3;
                }
                if (Game.IsKeyPressed(this.settings.GetValue<Keys>(sectionname, "Left_on_noclip", Keys.A)))
                {
                    character.Position = offsetInWorldCoords1;
                }
                if (Game.IsKeyPressed(this.settings.GetValue<Keys>(sectionname, "Right_on_noclip", Keys.D)))
                {
                    character.Position = vector31;
                }
                if (Game.IsKeyPressed(this.settings.GetValue<Keys>(sectionname, "Up_on_noclip", Keys.Z)))
                {
                    character.Position = offsetInWorldCoords2;
                }
                if (Game.IsKeyPressed(this.settings.GetValue<Keys>(sectionname, "Down_on_noclip", Keys.X)))
                {
                    character.Position = vector32;
                }
            }
        }
        #endregion
    }

    void rainmoney(int amount)
    {
        UI.Notify("WARNING! DO NOT USE THIS FUNCTION IN ONLINE.", true);
        for (int i = 0; i <= 1000; i++)
        {
            Hash moneypickup = Function.Call<Hash>(Hash.GET_HASH_KEY, "PICKUP_MONEY_CASE");
            InputArgument[] stuff = { moneypickup.ToString(), Game.Player.Character.Position.X, Game.Player.Character.Position.Y, (Game.Player.Character.Position.Z + 0.5f), 0, 40000, 0x113FD533, false, true };
            Function.Call(Hash.CREATE_AMBIENT_PICKUP, stuff);
        }
    }

    private void DoUnlockAllWeapons()
    {
        foreach (WeaponHash wpn in Enum.GetValues(typeof(WeaponHash)))
        {
            Game.Player.Character.Weapons.Give(wpn, 9999, false, true);
        }
    }
    private void MoreCops()
    {
        if (six_star && (Game.Player.WantedLevel != 6) || !six_star && Game.Player.WantedLevel != 5)
        {
            Game.Player.WantedLevel++;
        }
    }

    private void LessCops()
    {
        if (Game.Player.WantedLevel != 0)
        {
            Game.Player.WantedLevel--;
        }
    }

    private void FixRide(Vehicle vehicle)
    {
        vehicle.Repair();
    }

    private void FixGodRide(Vehicle vehicle)
    {
        vehicle.CanBeVisiblyDamaged = false;
        vehicle.CanTiresBurst = false;
        vehicle.EngineRunning = true;
        vehicle.IsDriveable = true;
        vehicle.IsStolen = false;
        vehicle.BodyHealth = vehicle.MaxHealth;
        vehicle.EngineHealth = vehicle.MaxHealth;
        vehicle.Repair();
        vehicle.IsInvincible = true;
    }
    #endregion
}
