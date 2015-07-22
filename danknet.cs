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

    #region SpawnMenus
    private void OpenSuperSpawnMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn T20", "");
        button.Activated += (sender, args) => this.SpawnCar(1663218586, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Osiris", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Osiris, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Bullet", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Bullet, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Adder", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Adder, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Vacca", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Vacca, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Infernus", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Infernus, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Zentorno", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Zentorno, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Entity XF", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.EntityXF, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Cheetah", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Cheetah, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Turismo R", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Turismor, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Voltic", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Voltic, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Spawn Menu (Super)", menuItems.ToArray()));
    }

    private void OpenSportSpawnMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Sentinel", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Sentinel, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Sentinel XS", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Sentinel2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Oracle", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Oracle, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Oracle XS", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Oracle2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Fusilade", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Fusilade, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Comet", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Comet2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Penumbra", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Penumbra, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn 9F", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Ninef, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn 9F Cabrio", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Ninef2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Page 2", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Ninef2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Spawn Menu (Sport 1)", menuItems.ToArray()));
    }

    private void OpenSportSpawnMenu2()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Furore GT", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Furoregt, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Sultan", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Sultan, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Futo", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Futo, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Coquette", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Coquette, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Coquette 2", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Coquette2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Carbonizzare", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Carbonizzare, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Jester", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Jester, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Rapid GT", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.RapidGT, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Rapid GT2", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.RapidGT2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Buffalo", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Buffalo, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Buffalo 2", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Buffalo2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Buffalo 3", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Buffalo3, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Banshee", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Banshee, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Page 3", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Ninef2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Spawn Menu (Sport 2)", menuItems.ToArray()));
    }

    private void OpenSportSpawnMenu3()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Surano", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Surano, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Schwartzer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Schwarzer, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Feltzer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Feltzer2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Feltzer 2", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Feltzer3, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Elegy RH8", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Elegy2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Khamelion", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Khamelion, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Spawn Menu (Sport 3)", menuItems.ToArray()));
    }

    private void OpenSportClassicSpawnMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Peyote (Vehicle)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Peyote, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Z-Type", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Ztype, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Monroe", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Monroe, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Pigalle", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Pigalle, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Coquette Classic", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Coquette2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Stinger GT", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.StingerGT, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn JB 700", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.JB700, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Tornado (Vehicle)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Tornado, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Tornado 2", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Tornado2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Tornado 3", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Tornado3, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Tornado 4", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Tornado4, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Manana", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Manana, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Roosevelt", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.BType, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Spawn Menu (Sport Classics)", menuItems.ToArray()));
    }

    private void OpenCoupesSpawnMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Zion", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Zion, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Zion Cabrio", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Zion2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Jackal", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Jackal, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn F620", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.F620, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Felon", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Felon, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Felon GT", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Felon2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Cognoscenti Cabrio", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.CogCabrio, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Exemplar", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Exemplar, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Prairie", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Prairie, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);
        
        this.View.AddMenu(new Menu("Spawn Menu (Coupes)", menuItems.ToArray()));
    }

    #endregion

    private void OpenSpawnMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Super Cars", "");
        button.Activated += (sender, args) => this.OpenSuperSpawnMenu();
        menuItems.Add(button);

        button = new MenuButton("Sport Cars", "");
        button.Activated += (sender, args) => this.OpenSportSpawnMenu();
        menuItems.Add(button);

        button = new MenuButton("Sports Classics Cars", "");
        button.Activated += (sender, args) => this.OpenSportClassicSpawnMenu();
        menuItems.Add(button);

        button = new MenuButton("Coupe Cars", "");
        button.Activated += (sender, args) => this.OpenSportClassicSpawnMenu();
        menuItems.Add(button);
        
        //TODO: Muscle, Sedan, Compacts, SUV, Off-Road, Motorcycles, Vans, Military, Emergency, Utility, Service, Industrial, Commercial, Cycles, Helicopters, Planes, Boats, Specials (Dukes, Space Docker, cable car, liberator (0xCD93A7DB), sovereign (0x2C509634))
        this.View.AddMenu(new Menu("About & How to use", menuItems.ToArray()));
    }

    //{ "NORTH YANKTON", 3360.19f, -4849.67f, 111.8f } https://github.com/moocowmaniak/GTA-V-Improved-Trainer/blob/master/samples/NativeTrainer/script.cpp ln1842

    //TODO: Spawn Ramps and stuff. Ability to load from txt.

    //TODO: Player Models
    //LPCSTR animalModels[] = { "a_c_boar", "a_c_chimp", "a_c_cow", "a_c_coyote", "a_c_deer", "a_c_fish", "a_c_hen", "a_c_cat_01", "a_c_chickenhawk","a_c_cormorant", "a_c_crow", "a_c_dolphin", "a_c_humpback", "a_c_killerwhale", "a_c_pigeon", "a_c_seagull", "a_c_sharkhammer","a_c_pig", "a_c_rat", "a_c_rhesus", "a_c_chop", "a_c_husky", "a_c_mtlion", "a_c_retriever", "a_c_sharktiger", "a_c_shepherd" };
//    LPCSTR pedModels[69][10] = {
//    { "player_zero", "player_one", "player_two", "a_c_boar", "a_c_chimp", "a_c_cow", "a_c_coyote", "a_c_deer", "a_c_fish", "a_c_hen" },
//    { "a_c_cat_01", "a_c_chickenhawk", "a_c_cormorant", "a_c_crow", "a_c_dolphin", "a_c_humpback", "a_c_killerwhale", "a_c_pigeon", "a_c_seagull", "a_c_sharkhammer" },
//    { "a_c_pig", "a_c_rat", "a_c_rhesus", "a_c_chop", "a_c_husky", "a_c_mtlion", "a_c_retriever", "a_c_sharktiger", "a_c_shepherd", "s_m_m_movalien_01" },
//    { "a_f_m_beach_01", "a_f_m_bevhills_01", "a_f_m_bevhills_02", "a_f_m_bodybuild_01", "a_f_m_business_02", "a_f_m_downtown_01", "a_f_m_eastsa_01", "a_f_m_eastsa_02", "a_f_m_fatbla_01", "a_f_m_fatcult_01" },
//    { "a_f_m_fatwhite_01", "a_f_m_ktown_01", "a_f_m_ktown_02", "a_f_m_prolhost_01", "a_f_m_salton_01", "a_f_m_skidrow_01", "a_f_m_soucentmc_01", "a_f_m_soucent_01", "a_f_m_soucent_02", "a_f_m_tourist_01" },
//    { "a_f_m_trampbeac_01", "a_f_m_tramp_01", "a_f_o_genstreet_01", "a_f_o_indian_01", "a_f_o_ktown_01", "a_f_o_salton_01", "a_f_o_soucent_01", "a_f_o_soucent_02", "a_f_y_beach_01", "a_f_y_bevhills_01" },
//    { "a_f_y_bevhills_02", "a_f_y_bevhills_03", "a_f_y_bevhills_04", "a_f_y_business_01", "a_f_y_business_02", "a_f_y_business_03", "a_f_y_business_04", "a_f_y_eastsa_01", "a_f_y_eastsa_02", "a_f_y_eastsa_03" },
//    { "a_f_y_epsilon_01", "a_f_y_fitness_01", "a_f_y_fitness_02", "a_f_y_genhot_01", "a_f_y_golfer_01", "a_f_y_hiker_01", "a_f_y_hippie_01", "a_f_y_hipster_01", "a_f_y_hipster_02", "a_f_y_hipster_03" },
//    { "a_f_y_hipster_04", "a_f_y_indian_01", "a_f_y_juggalo_01", "a_f_y_runner_01", "a_f_y_rurmeth_01", "a_f_y_scdressy_01", "a_f_y_skater_01", "a_f_y_soucent_01", "a_f_y_soucent_02", "a_f_y_soucent_03" },
//    { "a_f_y_tennis_01", "a_f_y_topless_01", "a_f_y_tourist_01", "a_f_y_tourist_02", "a_f_y_vinewood_01", "a_f_y_vinewood_02", "a_f_y_vinewood_03", "a_f_y_vinewood_04", "a_f_y_yoga_01", "a_m_m_acult_01" },
//    { "a_m_m_afriamer_01", "a_m_m_beach_01", "a_m_m_beach_02", "a_m_m_bevhills_01", "a_m_m_bevhills_02", "a_m_m_business_01", "a_m_m_eastsa_01", "a_m_m_eastsa_02", "a_m_m_farmer_01", "a_m_m_fatlatin_01" },
//    { "a_m_m_genfat_01", "a_m_m_genfat_02", "a_m_m_golfer_01", "a_m_m_hasjew_01", "a_m_m_hillbilly_01", "a_m_m_hillbilly_02", "a_m_m_indian_01", "a_m_m_ktown_01", "a_m_m_malibu_01", "a_m_m_mexcntry_01" },
//    { "a_m_m_mexlabor_01", "a_m_m_og_boss_01", "a_m_m_paparazzi_01", "a_m_m_polynesian_01", "a_m_m_prolhost_01", "a_m_m_rurmeth_01", "a_m_m_salton_01", "a_m_m_salton_02", "a_m_m_salton_03", "a_m_m_salton_04" },
//    { "a_m_m_skater_01", "a_m_m_skidrow_01", "a_m_m_socenlat_01", "a_m_m_soucent_01", "a_m_m_soucent_02", "a_m_m_soucent_03", "a_m_m_soucent_04", "a_m_m_stlat_02", "a_m_m_tennis_01", "a_m_m_tourist_01" },
//    { "a_m_m_trampbeac_01", "a_m_m_tramp_01", "a_m_m_tranvest_01", "a_m_m_tranvest_02", "a_m_o_acult_01", "a_m_o_acult_02", "a_m_o_beach_01", "a_m_o_genstreet_01", "a_m_o_ktown_01", "a_m_o_salton_01" },
//    { "a_m_o_soucent_01", "a_m_o_soucent_02", "a_m_o_soucent_03", "a_m_o_tramp_01", "a_m_y_acult_01", "a_m_y_acult_02", "a_m_y_beachvesp_01", "a_m_y_beachvesp_02", "a_m_y_beach_01", "a_m_y_beach_02" },
//    { "a_m_y_beach_03", "a_m_y_bevhills_01", "a_m_y_bevhills_02", "a_m_y_breakdance_01", "a_m_y_busicas_01", "a_m_y_business_01", "a_m_y_business_02", "a_m_y_business_03", "a_m_y_cyclist_01", "a_m_y_dhill_01" },
//    { "a_m_y_downtown_01", "a_m_y_eastsa_01", "a_m_y_eastsa_02", "a_m_y_epsilon_01", "a_m_y_epsilon_02", "a_m_y_gay_01", "a_m_y_gay_02", "a_m_y_genstreet_01", "a_m_y_genstreet_02", "a_m_y_golfer_01" },
//    { "a_m_y_hasjew_01", "a_m_y_hiker_01", "a_m_y_hippy_01", "a_m_y_hipster_01", "a_m_y_hipster_02", "a_m_y_hipster_03", "a_m_y_indian_01", "a_m_y_jetski_01", "a_m_y_juggalo_01", "a_m_y_ktown_01" },
//    { "a_m_y_ktown_02", "a_m_y_latino_01", "a_m_y_methhead_01", "a_m_y_mexthug_01", "a_m_y_motox_01", "a_m_y_motox_02", "a_m_y_musclbeac_01", "a_m_y_musclbeac_02", "a_m_y_polynesian_01", "a_m_y_roadcyc_01" },
//    { "a_m_y_runner_01", "a_m_y_runner_02", "a_m_y_salton_01", "a_m_y_skater_01", "a_m_y_skater_02", "a_m_y_soucent_01", "a_m_y_soucent_02", "a_m_y_soucent_03", "a_m_y_soucent_04", "a_m_y_stbla_01" },
//    { "a_m_y_stbla_02", "a_m_y_stlat_01", "a_m_y_stwhi_01", "a_m_y_stwhi_02", "a_m_y_sunbathe_01", "a_m_y_surfer_01", "a_m_y_vindouche_01", "a_m_y_vinewood_01", "a_m_y_vinewood_02", "a_m_y_vinewood_03" },
//    { "a_m_y_vinewood_04", "a_m_y_yoga_01", "u_m_y_proldriver_01", "u_m_y_rsranger_01", "u_m_y_sbike", "u_m_y_staggrm_01", "u_m_y_tattoo_01", "csb_abigail", "csb_anita", "csb_anton" },
//    { "csb_ballasog", "csb_bride", "csb_burgerdrug", "csb_car3guy1", "csb_car3guy2", "csb_chef", "csb_chin_goon", "csb_cletus", "csb_cop", "csb_customer" },
//    { "csb_denise_friend", "csb_fos_rep", "csb_g", "csb_groom", "csb_grove_str_dlr", "csb_hao", "csb_hugh", "csb_imran", "csb_janitor", "csb_maude" },
//    { "csb_mweather", "csb_ortega", "csb_oscar", "csb_porndudes", "csb_porndudes_p", "csb_prologuedriver", "csb_prolsec", "csb_ramp_gang", "csb_ramp_hic", "csb_ramp_hipster" },
//    { "csb_ramp_marine", "csb_ramp_mex", "csb_reporter", "csb_roccopelosi", "csb_screen_writer", "csb_stripper_01", "csb_stripper_02", "csb_tonya", "csb_trafficwarden", "cs_amandatownley" },
//    { "cs_andreas", "cs_ashley", "cs_bankman", "cs_barry", "cs_barry_p", "cs_beverly", "cs_beverly_p", "cs_brad", "cs_bradcadaver", "cs_carbuyer" },
//    { "cs_casey", "cs_chengsr", "cs_chrisformage", "cs_clay", "cs_dale", "cs_davenorton", "cs_debra", "cs_denise", "cs_devin", "cs_dom" },
//    { "cs_dreyfuss", "cs_drfriedlander", "cs_fabien", "cs_fbisuit_01", "cs_floyd", "cs_guadalope", "cs_gurk", "cs_hunter", "cs_janet", "cs_jewelass" },
//    { "cs_jimmyboston", "cs_jimmydisanto", "cs_joeminuteman", "cs_johnnyklebitz", "cs_josef", "cs_josh", "cs_lamardavis", "cs_lazlow", "cs_lestercrest", "cs_lifeinvad_01" },
//    { "cs_magenta", "cs_manuel", "cs_marnie", "cs_martinmadrazo", "cs_maryann", "cs_michelle", "cs_milton", "cs_molly", "cs_movpremf_01", "cs_movpremmale" },
//    { "cs_mrk", "cs_mrsphillips", "cs_mrs_thornhill", "cs_natalia", "cs_nervousron", "cs_nigel", "cs_old_man1a", "cs_old_man2", "cs_omega", "cs_orleans" },
//    { "cs_paper", "cs_paper_p", "cs_patricia", "cs_priest", "cs_prolsec_02", "cs_russiandrunk", "cs_siemonyetarian", "cs_solomon", "cs_stevehains", "cs_stretch" },
//    { "cs_tanisha", "cs_taocheng", "cs_taostranslator", "cs_tenniscoach", "cs_terry", "cs_tom", "cs_tomepsilon", "cs_tracydisanto", "cs_wade", "cs_zimbor" },
//    { "g_f_y_ballas_01", "g_f_y_families_01", "g_f_y_lost_01", "g_f_y_vagos_01", "g_m_m_armboss_01", "g_m_m_armgoon_01", "g_m_m_armlieut_01", "g_m_m_chemwork_01", "g_m_m_chemwork_01_p", "g_m_m_chiboss_01" },
//    { "g_m_m_chiboss_01_p", "g_m_m_chicold_01", "g_m_m_chicold_01_p", "g_m_m_chigoon_01", "g_m_m_chigoon_01_p", "g_m_m_chigoon_02", "g_m_m_korboss_01", "g_m_m_mexboss_01", "g_m_m_mexboss_02", "g_m_y_armgoon_02" },
//    { "g_m_y_azteca_01", "g_m_y_ballaeast_01", "g_m_y_ballaorig_01", "g_m_y_ballasout_01", "g_m_y_famca_01", "g_m_y_famdnf_01", "g_m_y_famfor_01", "g_m_y_korean_01", "g_m_y_korean_02", "g_m_y_korlieut_01" },
//    { "g_m_y_lost_01", "g_m_y_lost_02", "g_m_y_lost_03", "g_m_y_mexgang_01", "g_m_y_mexgoon_01", "g_m_y_mexgoon_02", "g_m_y_mexgoon_03", "g_m_y_mexgoon_03_p", "g_m_y_pologoon_01", "g_m_y_pologoon_01_p" },
//    { "g_m_y_pologoon_02", "g_m_y_pologoon_02_p", "g_m_y_salvaboss_01", "g_m_y_salvagoon_01", "g_m_y_salvagoon_02", "g_m_y_salvagoon_03", "g_m_y_salvagoon_03_p", "g_m_y_strpunk_01", "g_m_y_strpunk_02", "hc_driver" },
//    { "hc_gunman", "hc_hacker", "ig_abigail", "ig_amandatownley", "ig_andreas", "ig_ashley", "ig_ballasog", "ig_bankman", "ig_barry", "ig_barry_p" },
//    { "ig_bestmen", "ig_beverly", "ig_beverly_p", "ig_brad", "ig_bride", "ig_car3guy1", "ig_car3guy2", "ig_casey", "ig_chef", "ig_chengsr" },
//    { "ig_chrisformage", "ig_clay", "ig_claypain", "ig_cletus", "ig_dale", "ig_davenorton", "ig_denise", "ig_devin", "ig_dom", "ig_dreyfuss" },
//    { "ig_drfriedlander", "ig_fabien", "ig_fbisuit_01", "ig_floyd", "ig_groom", "ig_hao", "ig_hunter", "ig_janet", "ig_jay_norris", "ig_jewelass" },
//    { "ig_jimmyboston", "ig_jimmydisanto", "ig_joeminuteman", "ig_johnnyklebitz", "ig_josef", "ig_josh", "ig_kerrymcintosh", "ig_lamardavis", "ig_lazlow", "ig_lestercrest" },
//    { "ig_lifeinvad_01", "ig_lifeinvad_02", "ig_magenta", "ig_manuel", "ig_marnie", "ig_maryann", "ig_maude", "ig_michelle", "ig_milton", "ig_molly" },
//    { "ig_mrk", "ig_mrsphillips", "ig_mrs_thornhill", "ig_natalia", "ig_nervousron", "ig_nigel", "ig_old_man1a", "ig_old_man2", "ig_omega", "ig_oneil" },
//    { "ig_orleans", "ig_ortega", "ig_paper", "ig_patricia", "ig_priest", "ig_prolsec_02", "ig_ramp_gang", "ig_ramp_hic", "ig_ramp_hipster", "ig_ramp_mex" },
//    { "ig_roccopelosi", "ig_russiandrunk", "ig_screen_writer", "ig_siemonyetarian", "ig_solomon", "ig_stevehains", "ig_stretch", "ig_talina", "ig_tanisha", "ig_taocheng" },
//    { "ig_taostranslator", "ig_taostranslator_p", "ig_tenniscoach", "ig_terry", "ig_tomepsilon", "ig_tonya", "ig_tracydisanto", "ig_trafficwarden", "ig_tylerdix", "ig_wade" },
//    { "ig_zimbor", "mp_f_deadhooker", "mp_f_freemode_01", "mp_f_misty_01", "mp_f_stripperlite", "mp_g_m_pros_01", "mp_headtargets", "mp_m_claude_01", "mp_m_exarmy_01", "mp_m_famdd_01" },
//    { "mp_m_fibsec_01", "mp_m_freemode_01", "mp_m_marston_01", "mp_m_niko_01", "mp_m_shopkeep_01", "mp_s_m_armoured_01", "", "", "", "" },
//    { "", "s_f_m_fembarber", "s_f_m_maid_01", "s_f_m_shop_high", "s_f_m_sweatshop_01", "s_f_y_airhostess_01", "s_f_y_bartender_01", "s_f_y_baywatch_01", "s_f_y_cop_01", "s_f_y_factory_01" },
//    { "s_f_y_hooker_01", "s_f_y_hooker_02", "s_f_y_hooker_03", "s_f_y_migrant_01", "s_f_y_movprem_01", "s_f_y_ranger_01", "s_f_y_scrubs_01", "s_f_y_sheriff_01", "s_f_y_shop_low", "s_f_y_shop_mid" },
//    { "s_f_y_stripperlite", "s_f_y_stripper_01", "s_f_y_stripper_02", "s_f_y_sweatshop_01", "s_m_m_ammucountry", "s_m_m_armoured_01", "s_m_m_armoured_02", "s_m_m_autoshop_01", "s_m_m_autoshop_02", "s_m_m_bouncer_01" },
//    { "s_m_m_chemsec_01", "s_m_m_ciasec_01", "s_m_m_cntrybar_01", "s_m_m_dockwork_01", "s_m_m_doctor_01", "s_m_m_fiboffice_01", "s_m_m_fiboffice_02", "s_m_m_gaffer_01", "s_m_m_gardener_01", "s_m_m_gentransport" },
//    { "s_m_m_hairdress_01", "s_m_m_highsec_01", "s_m_m_highsec_02", "s_m_m_janitor", "s_m_m_lathandy_01", "s_m_m_lifeinvad_01", "s_m_m_linecook", "s_m_m_lsmetro_01", "s_m_m_mariachi_01", "s_m_m_marine_01" },
//    { "s_m_m_marine_02", "s_m_m_migrant_01", "u_m_y_zombie_01", "s_m_m_movprem_01", "s_m_m_movspace_01", "s_m_m_paramedic_01", "s_m_m_pilot_01", "s_m_m_pilot_02", "s_m_m_postal_01", "s_m_m_postal_02" },
//    { "s_m_m_prisguard_01", "s_m_m_scientist_01", "s_m_m_security_01", "s_m_m_snowcop_01", "s_m_m_strperf_01", "s_m_m_strpreach_01", "s_m_m_strvend_01", "s_m_m_trucker_01", "s_m_m_ups_01", "s_m_m_ups_02" },
//    { "s_m_o_busker_01", "s_m_y_airworker", "s_m_y_ammucity_01", "s_m_y_armymech_01", "s_m_y_autopsy_01", "s_m_y_barman_01", "s_m_y_baywatch_01", "s_m_y_blackops_01", "s_m_y_blackops_02", "s_m_y_busboy_01" },
//    { "s_m_y_chef_01", "s_m_y_clown_01", "s_m_y_construct_01", "s_m_y_construct_02", "s_m_y_cop_01", "s_m_y_dealer_01", "s_m_y_devinsec_01", "s_m_y_dockwork_01", "s_m_y_doorman_01", "s_m_y_dwservice_01" },
//    { "s_m_y_dwservice_02", "s_m_y_factory_01", "s_m_y_fireman_01", "s_m_y_garbage", "s_m_y_grip_01", "s_m_y_hwaycop_01", "s_m_y_marine_01", "s_m_y_marine_02", "s_m_y_marine_03", "s_m_y_mime" },
//    { "s_m_y_pestcont_01", "s_m_y_pilot_01", "s_m_y_prismuscl_01", "s_m_y_prisoner_01", "s_m_y_ranger_01", "s_m_y_robber_01", "s_m_y_sheriff_01", "s_m_y_shop_mask", "s_m_y_strvend_01", "s_m_y_swat_01" },
//    { "s_m_y_uscg_01", "s_m_y_valet_01", "s_m_y_waiter_01", "s_m_y_winclean_01", "s_m_y_xmech_01", "s_m_y_xmech_02", "u_f_m_corpse_01", "u_f_m_miranda", "u_f_m_promourn_01", "u_f_o_moviestar" },
//    { "u_f_o_prolhost_01", "u_f_y_bikerchic", "u_f_y_comjane", "u_f_y_corpse_01", "u_f_y_corpse_02", "u_f_y_hotposh_01", "u_f_y_jewelass_01", "u_f_y_mistress", "u_f_y_poppymich", "u_f_y_princess" },
//    { "u_f_y_spyactress", "u_m_m_aldinapoli", "u_m_m_bankman", "u_m_m_bikehire_01", "u_m_m_fibarchitect", "u_m_m_filmdirector", "u_m_m_glenstank_01", "u_m_m_griff_01", "u_m_m_jesus_01", "u_m_m_jewelsec_01" },
//    { "u_m_m_jewelthief", "u_m_m_markfost", "u_m_m_partytarget", "u_m_m_prolsec_01", "u_m_m_promourn_01", "u_m_m_rivalpap", "u_m_m_spyactor", "u_m_m_willyfist", "u_m_o_finguru_01", "u_m_o_taphillbilly" },
//    { "u_m_o_tramp_01", "u_m_y_abner", "u_m_y_antonb", "u_m_y_babyd", "u_m_y_baygor", "u_m_y_burgerdrug_01", "u_m_y_chip", "u_m_y_cyclist_01", "u_m_y_fibmugger_01", "u_m_y_guido_01" },
//    { "u_m_y_gunvend_01", "u_m_y_hippie_01", "u_m_y_imporage", "u_m_y_justin", "u_m_y_mani", "u_m_y_militarybum", "u_m_y_paparazzi", "u_m_y_party_01", "u_m_y_pogo_01", "u_m_y_prisoner_01" }
//};

//LPCSTR pedModelNames[69][10] = {
//    { "MICHAEL", "FRANKLIN", "TREVOR", "BOAR", "CHIMP", "COW", "COYOTE", "DEER", "FISH", "HEN" },
//    { "CAT", "HAWK", "CORMORANT", "CROW", "DOLPHIN", "HUMPBACK", "WHALE", "PIGEON", "SEAGULL", "SHARKHAMMER" },
//    { "PIG", "RAT", "RHESUS", "CHOP", "HUSKY", "MTLION", "RETRIEVER", "SHARKTIGER", "SHEPHERD", "ALIEN" },
//    { "BEACH", "BEVHILLS", "BEVHILLS", "BODYBUILD", "BUSINESS", "DOWNTOWN", "EASTSA", "EASTSA", "FATBLA", "FATCULT" },
//    { "FATWHITE", "KTOWN", "KTOWN", "PROLHOST", "SALTON", "SKIDROW", "SOUCENTMC", "SOUCENT", "SOUCENT", "TOURIST" },
//    { "TRAMPBEAC", "TRAMP", "GENSTREET", "INDIAN", "KTOWN", "SALTON", "SOUCENT", "SOUCENT", "BEACH", "BEVHILLS" },
//    { "BEVHILLS", "BEVHILLS", "BEVHILLS", "BUSINESS", "BUSINESS", "BUSINESS", "BUSINESS", "EASTSA", "EASTSA", "EASTSA" },
//    { "EPSILON", "FITNESS", "FITNESS", "GENHOT", "GOLFER", "HIKER", "HIPPIE", "HIPSTER", "HIPSTER", "HIPSTER" },
//    { "HIPSTER", "INDIAN", "JUGGALO", "RUNNER", "RURMETH", "SCDRESSY", "SKATER", "SOUCENT", "SOUCENT", "SOUCENT" },
//    { "TENNIS", "TOPLESS", "TOURIST", "TOURIST", "VINEWOOD", "VINEWOOD", "VINEWOOD", "VINEWOOD", "YOGA", "ACULT" },
//    { "AFRIAMER", "BEACH", "BEACH", "BEVHILLS", "BEVHILLS", "BUSINESS", "EASTSA", "EASTSA", "FARMER", "FATLATIN" },
//    { "GENFAT", "GENFAT", "GOLFER", "HASJEW", "HILLBILLY", "HILLBILLY", "INDIAN", "KTOWN", "MALIBU", "MEXCNTRY" },
//    { "MEXLABOR", "OG_BOSS", "PAPARAZZI", "POLYNESIAN", "PROLHOST", "RURMETH", "SALTON", "SALTON", "SALTON", "SALTON" },
//    { "SKATER", "SKIDROW", "SOCENLAT", "SOUCENT", "SOUCENT", "SOUCENT", "SOUCENT", "STLAT", "TENNIS", "TOURIST" },
//    { "TRAMPBEAC", "TRAMP", "TRANVEST", "TRANVEST", "ACULT", "ACULT", "BEACH", "GENSTREET", "KTOWN", "SALTON" },
//    { "SOUCENT", "SOUCENT", "SOUCENT", "TRAMP", "ACULT", "ACULT", "BEACHVESP", "BEACHVESP", "BEACH", "BEACH" },
//    { "BEACH", "BEVHILLS", "BEVHILLS", "BREAKDANCE", "BUSICAS", "BUSINESS", "BUSINESS", "BUSINESS", "CYCLIST", "DHILL" },
//    { "DOWNTOWN", "EASTSA", "EASTSA", "EPSILON", "EPSILON", "GAY", "GAY", "GENSTREET", "GENSTREET", "GOLFER" },
//    { "HASJEW", "HIKER", "HIPPY", "HIPSTER", "HIPSTER", "HIPSTER", "INDIAN", "JETSKI", "JUGGALO", "KTOWN" },
//    { "KTOWN", "LATINO", "METHHEAD", "MEXTHUG", "MOTOX", "MOTOX", "MUSCLBEAC", "MUSCLBEAC", "POLYNESIAN", "ROADCYC" },
//    { "RUNNER", "RUNNER", "SALTON", "SKATER", "SKATER", "SOUCENT", "SOUCENT", "SOUCENT", "SOUCENT", "STBLA" },
//    { "STBLA", "STLAT", "STWHI", "STWHI", "SUNBATHE", "SURFER", "VINDOUCHE", "VINEWOOD", "VINEWOOD", "VINEWOOD" },
//    { "VINEWOOD", "YOGA", "PROLDRIVER", "RSRANGER", "SBIKE", "STAGGRM", "TATTOO", "ABIGAIL", "ANITA", "ANTON" },
//    { "BALLASOG", "BRIDE", "BURGERDRUG", "CAR3GUY1", "CAR3GUY2", "CHEF", "CHIN_GOON", "CLETUS", "COP", "CUSTOMER" },
//    { "DENISE_FRIEND", "FOS_REP", "G", "GROOM", "DLR", "HAO", "HUGH", "IMRAN", "JANITOR", "MAUDE" },
//    { "MWEATHER", "ORTEGA", "OSCAR", "PORNDUDES", "PORNDUDES_P", "PROLOGUEDRIVER", "PROLSEC", "GANG", "HIC", "HIPSTER" },
//    { "MARINE", "MEX", "REPORTER", "ROCCOPELOSI", "SCREEN_WRITER", "STRIPPER", "STRIPPER", "TONYA", "TRAFFICWARDEN", "AMANDATOWNLEY" },
//    { "ANDREAS", "ASHLEY", "BANKMAN", "BARRY", "BARRY_P", "BEVERLY", "BEVERLY_P", "BRAD", "BRADCADAVER", "CARBUYER" },
//    { "CASEY", "CHENGSR", "CHRISFORMAGE", "CLAY", "DALE", "DAVENORTON", "DEBRA", "DENISE", "DEVIN", "DOM" },
//    { "DREYFUSS", "DRFRIEDLANDER", "FABIEN", "FBISUIT", "FLOYD", "GUADALOPE", "GURK", "HUNTER", "JANET", "JEWELASS" },
//    { "JIMMYBOSTON", "JIMMYDISANTO", "JOEMINUTEMAN", "JOHNNYKLEBITZ", "JOSEF", "JOSH", "LAMARDAVIS", "LAZLOW", "LESTERCREST", "LIFEINVAD" },
//    { "MAGENTA", "MANUEL", "MARNIE", "MARTINMADRAZO", "MARYANN", "MICHELLE", "MILTON", "MOLLY", "MOVPREMF", "MOVPREMMALE" },
//    { "MRK", "MRSPHILLIPS", "MRS_THORNHILL", "NATALIA", "NERVOUSRON", "NIGEL", "OLD_MAN1A", "OLD_MAN2", "OMEGA", "ORLEANS" },
//    { "PAPER", "PAPER_P", "PATRICIA", "PRIEST", "PROLSEC", "RUSSIANDRUNK", "SIEMONYETARIAN", "SOLOMON", "STEVEHAINS", "STRETCH" },
//    { "TANISHA", "TAOCHENG", "TAOSTRANSLATOR", "TENNISCOACH", "TERRY", "TOM", "TOMEPSILON", "TRACYDISANTO", "WADE", "ZIMBOR" },
//    { "BALLAS", "FAMILIES", "LOST", "VAGOS", "ARMBOSS", "ARMGOON", "ARMLIEUT", "CHEMWORK", "CHEMWORK_P", "CHIBOSS" },
//    { "CHIBOSS_P", "CHICOLD", "CHICOLD_P", "CHIGOON", "CHIGOON_P", "CHIGOON", "KORBOSS", "MEXBOSS", "MEXBOSS", "ARMGOON" },
//    { "AZTECA", "BALLAEAST", "BALLAORIG", "BALLASOUT", "FAMCA", "FAMDNF", "FAMFOR", "KOREAN", "KOREAN", "KORLIEUT" },
//    { "LOST", "LOST", "LOST", "MEXGANG", "MEXGOON", "MEXGOON", "MEXGOON", "MEXGOON_P", "POLOGOON", "POLOGOON_P" },
//    { "POLOGOON", "POLOGOON_P", "SALVABOSS", "SALVAGOON", "SALVAGOON", "SALVAGOON", "SALVAGOON_P", "STRPUNK", "STRPUNK", "HC_DRIVER" },
//    { "HC_GUNMAN", "HC_HACKER", "ABIGAIL", "AMANDATOWNLEY", "ANDREAS", "ASHLEY", "BALLASOG", "BANKMAN", "BARRY", "BARRY_P" },
//    { "BESTMEN", "BEVERLY", "BEVERLY_P", "BRAD", "BRIDE", "CAR3GUY1", "CAR3GUY2", "CASEY", "CHEF", "CHENGSR" },
//    { "CHRISFORMAGE", "CLAY", "CLAYPAIN", "CLETUS", "DALE", "DAVENORTON", "DENISE", "DEVIN", "DOM", "DREYFUSS" },
//    { "DRFRIEDLANDER", "FABIEN", "FBISUIT", "FLOYD", "GROOM", "HAO", "HUNTER", "JANET", "JAY_NORRIS", "JEWELASS" },
//    { "JIMMYBOSTON", "JIMMYDISANTO", "JOEMINUTEMAN", "JOHNNYKLEBITZ", "JOSEF", "JOSH", "KERRYMCINTOSH", "LAMARDAVIS", "LAZLOW", "LESTERCREST" },
//    { "LIFEINVAD", "LIFEINVAD", "MAGENTA", "MANUEL", "MARNIE", "MARYANN", "MAUDE", "MICHELLE", "MILTON", "MOLLY" },
//    { "MRK", "MRSPHILLIPS", "MRS_THORNHILL", "NATALIA", "NERVOUSRON", "NIGEL", "OLD_MAN1A", "OLD_MAN2", "OMEGA", "ONEIL" },
//    { "ORLEANS", "ORTEGA", "PAPER", "PATRICIA", "PRIEST", "PROLSEC", "GANG", "HIC", "HIPSTER", "MEX" },
//    { "ROCCOPELOSI", "RUSSIANDRUNK", "SCREEN_WRITER", "SIEMONYETARIAN", "SOLOMON", "STEVEHAINS", "STRETCH", "TALINA", "TANISHA", "TAOCHENG" },
//    { "TAOSTRANSLATOR", "TAOSTRANSLATOR_P", "TENNISCOACH", "TERRY", "TOMEPSILON", "TONYA", "TRACYDISANTO", "TRAFFICWARDEN", "TYLERDIX", "WADE" },
//    { "ZIMBOR", "DEADHOOKER", "FREEMODE", "MISTY", "STRIPPERLITE", "PROS", "MP_HEADTARGETS", "CLAUDE", "EXARMY", "FAMDD" },
//    { "FIBSEC", "FREEMODE", "MARSTON", "NIKO", "SHOPKEEP", "ARMOURED", "NONE", "NONE", "NONE", "NONE" },
//    { "NONE", "FEMBARBER", "MAID", "SHOP_HIGH", "SWEATSHOP", "AIRHOSTESS", "BARTENDER", "BAYWATCH", "COP", "FACTORY" },
//    { "HOOKER", "HOOKER", "HOOKER", "MIGRANT", "MOVPREM", "RANGER", "SCRUBS", "SHERIFF", "SHOP_LOW", "SHOP_MID" },
//    { "STRIPPERLITE", "STRIPPER", "STRIPPER", "SWEATSHOP", "AMMUCOUNTRY", "ARMOURED", "ARMOURED", "AUTOSHOP", "AUTOSHOP", "BOUNCER" },
//    { "CHEMSEC", "CIASEC", "CNTRYBAR", "DOCKWORK", "DOCTOR", "FIBOFFICE", "FIBOFFICE", "GAFFER", "GARDENER", "GENTRANSPORT" },
//    { "HAIRDRESS", "HIGHSEC", "HIGHSEC", "JANITOR", "LATHANDY", "LIFEINVAD", "LINECOOK", "LSMETRO", "MARIACHI", "MARINE" },
//    { "MARINE", "MIGRANT", "ZOMBIE", "MOVPREM", "MOVSPACE", "PARAMEDIC", "PILOT", "PILOT", "POSTAL", "POSTAL" },
//    { "PRISGUARD", "SCIENTIST", "SECURITY", "SNOWCOP", "STRPERF", "STRPREACH", "STRVEND", "TRUCKER", "UPS", "UPS" },
//    { "BUSKER", "AIRWORKER", "AMMUCITY", "ARMYMECH", "AUTOPSY", "BARMAN", "BAYWATCH", "BLACKOPS", "BLACKOPS", "BUSBOY" },
//    { "CHEF", "CLOWN", "CONSTRUCT", "CONSTRUCT", "COP", "DEALER", "DEVINSEC", "DOCKWORK", "DOORMAN", "DWSERVICE" },
//    { "DWSERVICE", "FACTORY", "FIREMAN", "GARBAGE", "GRIP", "HWAYCOP", "MARINE", "MARINE", "MARINE", "MIME" },
//    { "PESTCONT", "PILOT", "PRISMUSCL", "PRISONER", "RANGER", "ROBBER", "SHERIFF", "SHOP_MASK", "STRVEND", "SWAT" },
//    { "USCG", "VALET", "WAITER", "WINCLEAN", "XMECH", "XMECH", "CORPSE", "MIRANDA", "PROMOURN", "MOVIESTAR" },
//    { "PROLHOST", "BIKERCHIC", "COMJANE", "CORPSE", "CORPSE", "HOTPOSH", "JEWELASS", "MISTRESS", "POPPYMICH", "PRINCESS" },
//    { "SPYACTRESS", "ALDINAPOLI", "BANKMAN", "BIKEHIRE", "FIBARCHITECT", "FILMDIRECTOR", "GLENSTANK", "GRIFF", "JESUS", "JEWELSEC" },
//    { "JEWELTHIEF", "MARKFOST", "PARTYTARGET", "PROLSEC", "PROMOURN", "RIVALPAP", "SPYACTOR", "WILLYFIST", "FINGURU", "TAPHILLBILLY" },
//    { "TRAMP", "ABNER", "ANTONB", "BABYD", "BAYGOR", "BURGERDRUG", "CHIP", "CYCLIST", "FIBMUGGER", "GUIDO" },
//    { "GUNVEND", "HIPPIE", "IMPORAGE", "JUSTIN", "MANI", "MILITARYBUM", "PAPARAZZI", "PARTY", "POGO", "PRISONER" }
//};
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

        this.View.AddMenu(new Menu("Player Menu", menuItems.ToArray()));
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

    private Vehicle SpawnCar(int vehname, Vector3 pos, float heading)
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
                    int type = 2;
                    if (type == 1)
                    {
                        Game.Player.Character.Position = new Vector3(coords.X, coords.Y, (World.GetGroundHeight(coords) + 4));
                    }
                    else if (type == 2)
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
