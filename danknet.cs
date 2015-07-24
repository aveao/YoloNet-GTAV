using System;
using GTA;
using GTA.Math;
using GTA.Native;
using System.Windows.Forms;
using System.Collections.Generic;
using Menu = GTA.Menu;
using System.IO;
using System.Drawing;
using System.Text;

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
    bool markgunped = false;
    bool vehiclegun = false;
    Vehicle markedvehicle;
    Vehicle markedvehicle1;
    Vehicle markedvehicle2;
    Vehicle markedvehicle3;
    Ped markedped;
    Ped markedped1;
    Ped markedped2;
    Ped markedped3;
    int curmark = 1;
    int curmarkped = 1;
    Menu Markmenu;
    Vehicle curveh;
    bool featurePlayerUnlimitedAbility = false;
    bool featurePlayerNoNoiseUpdated = false;
    bool featurePlayerNoNoise = false;
    bool featurePlayerSuperJump = false;
    bool featureWeaponFireAmmo = false;
    bool featureWeaponExplosiveAmmo = false;
    bool featureWeaponExplosiveMelee = false;
    Vector3 tpfactor = new Vector3(0f, 0f, 3f);
    string configfile = "scripts\\danknetmenu.txt";
    private ScriptSettings settings;
    private Dictionary<Vector3, string> tplist = new Dictionary<Vector3, string>();
    private Dictionary<string, int> mdllist = new Dictionary<string, int>();
    private Dictionary<string, int> pedlist = new Dictionary<string, int>();
    string sectionname = "DANKNETMENU";
    string tpfilename = "scripts\\danknettplist.txt";
    string mdlfilename = "scripts\\danknetmdllist.txt";
    string pedfilename = "scripts\\danknetpedlist.txt";
    bool showfps = false;
    string version = "v0.5";
    string versionlink = "http://ardaozkal.github.io/danknetversion.txt";

    public danknet()
    {
        settings = ScriptSettings.Load(configfile);
        this.View.MenuTransitions = true;
        Tick += OnTick;
        this.KeyDown += this.OnKeyDown;
        CheckForUpdate();
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

        if (File.Exists(mdlfilename))
        {
            List<string> readen = new List<string>();
            readen.AddRange(File.ReadAllLines(mdlfilename)); //name newline int of model

            string nagme = ""; //name
            int mdlno;
            int no = 0;
            foreach (string readed in readen)
            {
                no++;
                try
                {
                    if (no == 1)
                    {
                        nagme = readed;
                    }
                    else if (no == 2)
                    {
                        mdlno = int.Parse(readed);
                        no = 0;
                        mdllist.Add(nagme, mdlno);
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
            File.Create(mdlfilename).Close();
        }


        if (File.Exists(pedfilename))
        {
            List<string> readen = new List<string>();
            readen.AddRange(File.ReadAllLines(pedfilename)); //name newline int of model

            string nagme = ""; //name
            int mdlno;
            int no = 0;
            foreach (string readed in readen)
            {
                no++;
                try
                {
                    if (no == 1)
                    {
                        nagme = readed;
                    }
                    else if (no == 2)
                    {
                        mdlno = int.Parse(readed);
                        no = 0;
                        pedlist.Add(nagme, mdlno);
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
            File.Create(pedfilename).Close();
        }
    }
    #region
    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == this.settings.GetValue<Keys>(sectionname, "Enable/Disable_Menu", Keys.F6))
        {
            if (this.View.ActiveMenus == 0)
            {
                this.OpenTrainerMenu();
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

        button = new MenuButton("(Vehicle) Spawn Menu", "TODO:EDIT THIS");
        button.Activated += (sender, args) => this.OpenSpawnMenuVehicle();
        menuItems.Add(button);

        button = new MenuButton("(Object) Spawn Menu", "TODO:EDIT THIS");
        button.Activated += (sender, args) => this.OpenSpawnMenuObject();
        menuItems.Add(button);

        button = new MenuButton("(Ped) Spawn Menu", "TODO:EDIT THIS");
        button.Activated += (sender, args) => this.OpenSpawnMenuPed();
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

        button = new MenuButton("(Ped) Mark Menu", "TP, kill etc.");
        button.Activated += (sender, args) => this.OpenPedMarkMenu();
        menuItems.Add(button);

        button = new MenuButton("(Vehicle) Mark Menu", "TP to car etc.");
        button.Activated += (sender, args) => this.OpenVehicleMarkMenu();
        menuItems.Add(button);

        button = new MenuButton("North Yankton Menu", "TODO:EDIT THIS");
        button.Activated += (sender, args) => this.OpenYanktonMenu();
        menuItems.Add(button);

        button = new MenuButton("About and how to use", "TODO:EDIT THIS");
        button.Activated += (sender, args) => this.OpenAbout();
        menuItems.Add(button);

        this.View.AddMenu(new Menu(("Danknet Menu " + version), menuItems.ToArray()));
    }

    void CheckForUpdate()
    {
        try
        {
            System.Net.WebClient wc = new System.Net.WebClient();
            string downloadedstring = wc.DownloadString(versionlink);
            if (!downloadedstring.StartsWith(version)) //so we won't have windows 9, we will have windows 10.
            {
                UI.Notify("A new update is available, new version: " + downloadedstring);
            }
            else
            {
                UI.Notify("DankNet Menu is up to date");
            }
        }
        catch
        {
            UI.Notify("Failed checking updates");
        }
    }

    private void OpenPedMarkMenu()
    {
        var menuItems = new List<IMenuItem>();
        //TODO: Shoot with explosions of the selected ped
        var toggle = new MenuToggle("Use markgun", "Just aim at a ped", markgunped);
        toggle.Changed += (sender, args) =>
        {
            var tg = sender as MenuToggle;
            if (tg == null)
            {
                return;
            }
            markgunped = tg.Value;
        };
        menuItems.Add(toggle);


        if (markedped != null && markedped.Exists() && markedped.IsAlive)
        {
            var button = new MenuButton("Unmark", "");
            button.Activated += (sender, args) => this.UnMark();
            menuItems.Add(button);
        }
        else
        {
            markedped = null;
            if (curmarkped == 1)
            {
                markedped1 = markedped;
            }
            else if (curmarkped == 2)
            {
                markedped2 = markedped;
            }
            else if (curmarkped == 3)
            {
                markedped3 = markedped;
            }
        }
        if (markedped1 != null && markedped1.Exists() && markedped1.IsAlive)
        {
            var button = new MenuButton("Open Marked Menu 1", "");
            button.Activated += (sender, args) => this.OpenPlayerMenu(markedped1);
            menuItems.Add(button);
            Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, (Entity)markedped1, true);
        }
        else
        {
            markedped1 = null;
        }

        if (markedped2 != null && markedped2.Exists() && markedped2.IsAlive)
        {
            var button = new MenuButton("Open Marked Menu 2", "");
            button.Activated += (sender, args) => this.OpenPlayerMenu(markedped2);
            menuItems.Add(button);
            Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, (Entity)markedped2, true);
        }
        else
        {
            markedped2 = null;
        }

        if (markedped3 != null && markedped3.Exists() && markedped3.IsAlive)
        {
            var button = new MenuButton("Open Marked Menu 3", "");
            button.Activated += (sender, args) => this.OpenPlayerMenu(markedped3);
            menuItems.Add(button);
            Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, (Entity)markedped3, true);
        }
        else
        {
            markedped3 = null;
        }

        var numerog = new MenuNumericScroller(("Switch Marked Ped"), "", 1, 3, 1, (curmarkped - 1));
        numerog.Changed += numerog_Changed;
        menuItems.Add(numerog);

        Markmenu = new Menu("(Ped) Mark Menu", menuItems.ToArray());
        this.View.AddMenu(Markmenu);
    }

    private void OpenVehicleMarkMenu()
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
        if (markedvehicle1 != null && markedvehicle1.FriendlyName != "NULL" && markedvehicle1.Exists())
        {
            var button = new MenuButton("Open Marked Menu 1", "");
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
            var button = new MenuButton("Open Marked Menu 2", "");
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
            var button = new MenuButton("Open Marked Menu 3", "");
            button.Activated += (sender, args) => this.OpenVehicleMenu(markedvehicle3);
            menuItems.Add(button);
            Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, (Entity)markedvehicle3, true);
        }
        else
        {
            markedvehicle3 = null;
        }

        var numero = new MenuNumericScroller(("Switch Marked Vehicle"), "", 1, 3, 1, (curmark - 1));
        numero.Changed += numero_Activated;
        menuItems.Add(numero);

        Markmenu = new Menu("(Vehicle) Mark Menu", menuItems.ToArray());
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

        toggle = new MenuToggle("Vehicle gun", "Get Ready 4 lag", vehiclegun);
        toggle.Changed += (sender, args) =>
        {
            var tg = sender as MenuToggle;
            if (tg == null)
            {
                return;
            }
            vehiclegun = tg.Value;
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

        var button = new MenuButton("RGB car primary color", "First r, then g and then b");
        button.Activated += (sender, args) => this.rgbcarprimcolor(veh);
        menuItems.Add(button);

        button = new MenuButton("RGB car secondary color", "First r, then g and then b");
        button.Activated += (sender, args) => this.rgbcarseccolor(veh);
        menuItems.Add(button);

        button = new MenuButton("HEX car primary color", "Just hex like ff00ff");
        button.Activated += (sender, args) => this.hexcarprimcolor(veh);
        menuItems.Add(button);

        button = new MenuButton("HEX car secondary color", "Just hex like ff00ff");
        button.Activated += (sender, args) => this.hexcarseccolor(veh);
        menuItems.Add(button);

        button = new MenuButton("Clear car custom colors", "");
        button.Activated += (sender, args) => this.rgbcarclean(veh);
        menuItems.Add(button);

        button = new MenuButton("Open All Doors", "titan ftw");
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

    private void OpenYanktonMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Load North Yankton (Prologue)", "");
        button.Activated += (sender, args) => this.loadnyankton(false);
        menuItems.Add(button);
        button = new MenuButton("Load North Yankton (Burying the hatchet)", "");
        button.Activated += (sender, args) => this.loadnyankton(true);
        menuItems.Add(button);
        button = new MenuButton("Teleport me into it", "");
        button.Activated += (sender, args) => this.tptony(Game.Player.Character);
        menuItems.Add(button); 
        this.View.AddMenu(new Menu(("North Yankton Menu"), menuItems.ToArray()));
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

        text = new MenuLabel(("version " + version), false);
        menuItems.Add(text);

        text = new MenuLabel("Num 2 and 8 to scroll", false);
        menuItems.Add(text);

        text = new MenuLabel("5 to select", false);
        menuItems.Add(text);

        text = new MenuLabel("Num 0 to go to prev menu", false);
        menuItems.Add(text);

        var button = new MenuButton("Check for updates", "");
        button.Activated += (sender, args) => this.CheckForUpdate();
        menuItems.Add(button);

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
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Sentinel2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Sentinel XS", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Sentinel, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Oracle", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Oracle2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Oracle XS", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Oracle, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
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

        button = new MenuButton("Spawn Surano", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Surano, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Schwartzer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Schwarzer, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Feltzer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Feltzer2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Elegy RH8", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Elegy2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Khamelion", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Khamelion, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Page 2", "");
        button.Activated += (sender, args) => this.OpenSportSpawnMenu2();
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

        button = new MenuButton("Spawn Carbonizzare", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Carbonizzare, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Jester", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Jester, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Rapid GT", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.RapidGT, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Rapid GT (Convertible)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.RapidGT2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Buffalo", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Buffalo, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Buffalo S", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Buffalo2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Sprunk Buffalo", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Buffalo3, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Banshee", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Banshee, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Spawn Menu (Sport 2)", menuItems.ToArray()));
    }

    private void OpenSportClassicSpawnMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Peyote", "");
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

        button = new MenuButton("Spawn Tornado", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Tornado, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Tornado (Convertible)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Tornado2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Tornado (Rusty)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Tornado3, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Tornado (Rusty Topless)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Tornado4, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Manana", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Manana, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Roosevelt", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.BType, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Stirling GT", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Feltzer3, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
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

    private void OpenMuscleSpawnMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Dominator", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Dominator, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Blade", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Blade, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Rat-Loader (Rusty)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.RatLoader, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Rat-Truck (Clean)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.RatLoader2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Ruiner", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Ruiner, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Phoenix", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Phoenix, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Hotknife", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Hotknife, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Voodoo", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Voodoo2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Vigero", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Vigero, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Sabre Turbo", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.SabreGT, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Picador", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Picador, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Gauntlet", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Gauntlet, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Buccaneer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Buccaneer, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Spawn Menu (Muscle)", menuItems.ToArray()));
    }

    private void OpenSedanSpawnMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Stratum", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Stratum, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Ingot", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Ingot, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Stanier", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Stanier, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Tailgater", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Tailgater, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Intruder", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Intruder, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Asterope", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Asterope, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Super Diamond", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Superd, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Stretch", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Stretch, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Regina", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Regina, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Premier", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Premier, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Asea", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Asea, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Open Page 2", "");
        button.Activated += (sender, args) => this.OpenSedanSpawnMenu2();
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Spawn Menu (Sedans)", menuItems.ToArray()));
    }

    private void OpenSedanSpawnMenu2()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Surge", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Surge, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Fugitive", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Fugitive, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Romero Hearse", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Romero, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Schafter", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Schafter2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Washington", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Washington, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Primo", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Primo, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Emperor", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Emperor, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Emperor (Rusty)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Emperor2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Spawn Menu (Sedans)", menuItems.ToArray()));
    }

    private void OpenCompactsSpawnMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Issi", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Issi2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Dilettante", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Dilettante, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Dilettante (Patrol)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Dilettante2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Blista", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Blista, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Blista Compact", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Blista2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Rhapsody", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Rhapsody, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Panto", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Panto, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Spawn Menu (Sedans)", menuItems.ToArray()));
    }

    private void OpenSUVSpawnMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Radius", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Radi, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Rocoto", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Rocoto, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Patriot", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Patriot, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn BeeJay XL", "Insert lennyface");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.BJXL, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Baller", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Baller, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Baller 2", "Diff Back, metal in doors");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Baller2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn FQ2", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Fq2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Huntley", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Huntley, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Habanero", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Habanero, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Page 2", "");
        button.Activated += (sender, args) => this.OpenSUVSpawnMenu2();
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Spawn Menu (SUV)", menuItems.ToArray()));
    }

    private void OpenSUVSpawnMenu2()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Landstalker", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Landstalker, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Granger", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Granger, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Seminole", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Seminole, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Gresley", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Gresley, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Serrano", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Serrano, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Dubsta", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Dubsta, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Cavalcade", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Cavalcade, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Spawn Menu (SUV) 2", menuItems.ToArray()));
    }

    private void OpenOffRoadSpawnMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Dubsta", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Dubsta, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Dubsta 2", "Tire in back");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Dubsta2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Dubsta 6x6", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Dubsta3, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Sandking XL", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Sandking, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Sandking SWB", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Sandking2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Blazer (ATV)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Blazer, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Blazer Lifeguard (ATV)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Blazer2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Hot Rod Blazer (ATV)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Blazer3, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Sanchez (Livery)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Sanchez, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Sanchez 2", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Sanchez2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Page 2", "");
        button.Activated += (sender, args) => this.OpenOffRoadSpawnMenu2();
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Spawn Menu (Off-Road)", menuItems.ToArray()));
    }

    private void OpenOffRoadSpawnMenu2()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Enduro", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Enduro, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Rebel", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Rebel, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Rusty Rebel", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Rebel2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Rancher XL", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.RancherXL, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn (Topless) Mesa", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Mesa, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Mesa (Merryweather)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Mesa3, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Bodhi", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Bodhi2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Dune (Buggy)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Dune, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn BF Injection", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.BfInjection, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Bifta", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Bifta, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Spawn Menu (Off-Road) 2", menuItems.ToArray()));
    }

    private void OpenMotorcyclesSpawnMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Lectro", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Lectro, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Hot Rod Blazer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Blazer3, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Sovereign", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Sovereign, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Daemon", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Daemon, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Bagger", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Bagger, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Vader", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Vader, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn PCJ 600", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.PCJ, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Hakuchou", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Hakuchou, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Nemesis", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Nemesis, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Ruffian", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Ruffian, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Faggio", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Faggio2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Bati 801", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Bati, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Bati 801RR", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Bati2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Carbon RS", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.CarbonRS, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Innovation", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Innovation, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Hexer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Hexer, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Thrust", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Thrust, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Double-T(!)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Double, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Akuma", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Akuma, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Spawn Menu (Motorcycles)", menuItems.ToArray()));
    }

    private void OpenSpecialsSpawnMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Space Docker", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Dune2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Duke", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Dukes, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Duke'o'Death", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Dukes2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Train", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Freight, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Monkey Blista", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Blista3, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Liberator", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Monster, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Sovereign", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Sovereign, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Cable Car", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.CableCar, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Towtruck", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.TowTruck, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Towtruck (old)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.TowTruck2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Clown Van", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Speedo2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Weed Pony", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Pony2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("North Yankton Vehicles", "");
        button.Activated += (sender, args) => this.OpenSpecialsSpawnMenu2();
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Spawn Menu (Specials)", menuItems.ToArray()));
    }

    private void OpenSpecialsSpawnMenu2()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Asea (North Yankton)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Asea2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Rancher XL (North Yankton)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.RancherXL2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Mesa (North Yankton)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Mesa2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Emperor (North Yankton)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Emperor3, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Burrito (North Yankton)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Burrito5, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Police 4x4 (North Yankton)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.PoliceOld1, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Police Cruiser (North Yankton)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.PoliceOld2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Sadler (North Yankton)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Sadler2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Fieldmaster (North Yankton)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Tractor3, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Stockade (North Yankton)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Stockade3, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);
        //Want a special car here? Send me a message!
        this.View.AddMenu(new Menu("Spawn Menu (North Yankton)", menuItems.ToArray()));
    }

    private void OpenVanSpawnMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Journey", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Journey, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Speedo", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Speedo, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Minivan", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Minivan, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Bobcat XL", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.BobcatXL, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Taco Van", "dev note:\nI NEVER EVER ate taco \nin my life. they're nowhere\nto be found");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Taco, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Burrito", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Burrito, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Burrito2", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Burrito2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Burrito3", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Burrito3, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Burrito4", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Burrito4, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Pony", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Pony, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Camper", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Camper, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Boxville", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Boxville, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Boxville2", "one is brute");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Boxville2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Youga", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Youga, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Rumpo", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Rumpo, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Bison", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Bison, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Bison2", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Bison2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Surfer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Surfer, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Rusty Surfer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Surfer2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Spawn Menu (Vans)", menuItems.ToArray()));
    }

    private void OpenMilitarySpawnMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Rhino", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Rhino, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Barracks", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Barracks, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Barracks 2", "No vis diff \n(hash barracks3)");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Barracks3, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Semi Barracks", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Barracks2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Crusader", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Crusader, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        var txt = new MenuLabel("Emergency", true);
        menuItems.Add(txt);

        button = new MenuButton("Spawn Police", "Old Look\nNorth Yankton ones are in Special");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Police, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Police 2", "Newer Look");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Police2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Police 3", "New Look");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Police3, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Unmarked Cruiser", "Looks like agent car");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Police4, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Police Bike", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Policeb, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Police Transporter", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.PoliceT, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Police Riot Car", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Riot, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Prison Bus", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.PBus, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);
        //PARK RANGER
        //LIFEGUARD
        button = new MenuButton("Spawn Fire Truck", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.FireTruck, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn FIB", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.FBI, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn FIB SUV", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.FBI2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Ambulance", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Ambulance, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Spawn Menu (Military&Emergency)", menuItems.ToArray()));
    }
    
    private void OpenUtilitySpawnMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Sadler", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Sadler, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Utility Truck (B)", "Big Size");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.UtilityTruck, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);
        button = new MenuButton("Spawn Utility Truck (M)", "Medium Size");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.UtilityTruck2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);
        button = new MenuButton("Spawn Utility Truck (S)", "Small Size");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.UtilityTruck3, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Tractor", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Tractor, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Fieldmaster", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Tractor2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Scrap Truck", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Scrap, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Ripley", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Ripley, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Lawn Mower", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Mower, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Docktug", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Docktug, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Caddy", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Caddy, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn (Topless) Caddy", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Caddy2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Airtug", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Airtug, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Forklift", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Forklift, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Spawn Menu (Utility)", menuItems.ToArray()));
    }

    private void OpenServiceSpawnMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Trashmaster", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Trash, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Trashmaster 2", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Trash2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Tourbus", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Tourbus, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Taxi", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Taxi, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Rental Shuttle Bus", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.RentalBus, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);
        //Dashhound
        button = new MenuButton("Spawn Bus", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Bus, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);
        //Airport Bus

        var txt = new MenuLabel("Industrial:", true);
        menuItems.Add(txt);

        button = new MenuButton("Spawn Tipper", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.TipTruck, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Rusty Tipper", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.TipTruck2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Flatbed", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Flatbed, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Rubble", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Rubble, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Mixer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Mixer, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Mixer2", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Mixer2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Dump", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Dump, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Cutter", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Cutter, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);
        //Dozer

        this.View.AddMenu(new Menu("Spawn Menu (Service&Industrial)", menuItems.ToArray()));
    }

    private void OpenCommercialSpawnMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Benson", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Benson, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Pounder", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Pounder, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Packer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Packer, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Mule", "Horizontal Door");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Mule, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Mule2", "Vertical Door, good for cars");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Mule2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Mule3", "Horizontal Door");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Mule3, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Phantom", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Phantom, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Hauler", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Hauler, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Biff", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Biff, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Stockade", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Stockade, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Spawn Menu (Commercial)", menuItems.ToArray()));
    }

    private void OpenCyclesSpawnMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Whippet Race Bike", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.TriBike, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Endurex Race Bike", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.TriBike2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Tri-Cycles Race Bike", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.TriBike3, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Scorcher", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Scorcher, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Fixter", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Fixter, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Cruiser", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Cruiser, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn BMX", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Bmx, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Spawn Menu (Cycles)", menuItems.ToArray()));
    }

    private void OpenHelisSpawnMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Valkyrie", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Valkyrie, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Savage", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Savage, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Swift", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Swift, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Swift Deluxe", "aka Golden");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Swift2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Annihilator", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Annihilator, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Buzzard", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Buzzard, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Buzzard Attack Chopper", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Buzzard2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Cargobob", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Cargobob, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Jetsam Cargobob", "Little bit more colored");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Cargobob2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn TPI Cargobob", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Cargobob3, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Frogger", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Frogger, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn FIB Frogger", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Frogger2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Maverick", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Maverick, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Police Maverick", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Polmav, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Skylift", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Skylift, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Spawn Menu (Helicopters)", menuItems.ToArray()));
    }

    private void OpenPlaneSpawnMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Vestra", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Vestra, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Savage", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Savage, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);
        //besra

        button = new MenuButton("Spawn Miljet", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Miljet, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Blimp", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Blimp, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Xero Blimp", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Blimp2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Luxor", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Luxor, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Luxor Deluxe", "aka Golden");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Luxor2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Shamal", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Shamal, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Cargo Plane", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.CargoPlane, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Cuban 800", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Cuban800, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Duster", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Duster, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Dodo", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Dodo, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);
        //mallard
        button = new MenuButton("Spawn Mammatus", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Mammatus, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn P-996 Lazer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Lazer, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Titan", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Titan, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Velum", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Velum, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Velum2", "Idk difference\nbut always spawns yellow");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Velum2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Spawn Menu (Planes)", menuItems.ToArray()));
    }

    private void OpenBoatSpawnMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Marquis", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Marquis, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Dinghy", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Dinghy, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Dinghy2", "Always black");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Dinghy2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Dinghy3", "Always black");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Dinghy3, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Police Predator", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Predator, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Jetmax", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Jetmax, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Squalo", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Squalo, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Suntrap", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Suntrap, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Tropic", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Tropic, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Seashark", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Seashark, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Lifegoard Seashark", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Seashark2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Submersible", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Submersible, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Kraken (Sub)", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Submersible2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Spawn Menu (Boats)", menuItems.ToArray()));
    }

    private void OpenTrailerSpawnMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Spawn Army Trailer", "Empty");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.ArmyTrailer, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Army Trailer 2", "Not Empty");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.ArmyTrailer2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Empty Vehicle Trailer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.TR2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Full Vehicle Trailer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.TR4, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Empty Trailer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.TR3, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn TRFlat", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.TRFlat, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Bale Trailer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.BaleTrailer, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Boat Trailer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.BoatTrailer, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Dock Trailer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.DockTrailer, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Freight Trailer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.FreightTrailer, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Grain Trailer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.GrainTrailer, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Prop Trailer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.PropTrailer, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Rake Trailer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.RakeTrailer, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Tree Log Trailer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.TrailerLogs, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Blue Trailer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Trailers, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Trailer with Ad", "Clucking Bell, Pisswasser etc.");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Trailers2, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Big Goods Trailer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.Trailers3, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Small Trailer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.TrailerSmall, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("Spawn Fame or Shame Trailer", "");
        button.Activated += (sender, args) => this.SpawnCar(VehicleHash.TVTrailer, (Game.Player.Character.Position + new Vector3(0f, 0f, 1f)), Game.Player.Character.Heading);
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Spawn Menu (Trailers)", menuItems.ToArray()));
    }

    #endregion
    //TODO: if BackButton Pressed: Goto last page as option.
    private void OpenSpawnMenuVehicle()
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
        button.Activated += (sender, args) => this.OpenCoupesSpawnMenu();
        menuItems.Add(button);

        button = new MenuButton("Muscle Cars", "");
        button.Activated += (sender, args) => this.OpenMuscleSpawnMenu();
        menuItems.Add(button);

        button = new MenuButton("Sedan Cars", "");
        button.Activated += (sender, args) => this.OpenSedanSpawnMenu();
        menuItems.Add(button);

        button = new MenuButton("Compact Cars", "");
        button.Activated += (sender, args) => this.OpenCompactsSpawnMenu();
        menuItems.Add(button);

        button = new MenuButton("SUV Cars", "");
        button.Activated += (sender, args) => this.OpenSUVSpawnMenu();
        menuItems.Add(button);

        button = new MenuButton("Off-Road Cars", "");
        button.Activated += (sender, args) => this.OpenOffRoadSpawnMenu();
        menuItems.Add(button);

        button = new MenuButton("Motorcycles", "");
        button.Activated += (sender, args) => this.OpenMotorcyclesSpawnMenu();
        menuItems.Add(button);

        button = new MenuButton("Vans", "");
        button.Activated += (sender, args) => this.OpenVanSpawnMenu();
        menuItems.Add(button);

        button = new MenuButton("Military & Emergency Vehicles", "");
        button.Activated += (sender, args) => this.OpenMilitarySpawnMenu();
        menuItems.Add(button);

        button = new MenuButton("Utility Vehicles", "");
        button.Activated += (sender, args) => this.OpenUtilitySpawnMenu();
        menuItems.Add(button);

        button = new MenuButton("Service & Industrial Vehicles", "");
        button.Activated += (sender, args) => this.OpenServiceSpawnMenu();
        menuItems.Add(button);

        button = new MenuButton("Commercial Vehicles", "");
        button.Activated += (sender, args) => this.OpenCommercialSpawnMenu();
        menuItems.Add(button);

        button = new MenuButton("Cycles", "");
        button.Activated += (sender, args) => this.OpenCyclesSpawnMenu();
        menuItems.Add(button);

        button = new MenuButton("Helicopters", "");
        button.Activated += (sender, args) => this.OpenHelisSpawnMenu();
        menuItems.Add(button);

        button = new MenuButton("Planes", "");
        button.Activated += (sender, args) => this.OpenPlaneSpawnMenu();
        menuItems.Add(button);

        button = new MenuButton("Boats", "");
        button.Activated += (sender, args) => this.OpenBoatSpawnMenu();
        menuItems.Add(button);

        button = new MenuButton("Trailers", "");
        button.Activated += (sender, args) => this.OpenTrailerSpawnMenu();
        menuItems.Add(button);

        button = new MenuButton("Special Vehicles", "");
        button.Activated += (sender, args) => this.OpenSpecialsSpawnMenu();
        menuItems.Add(button);

        this.View.AddMenu(new Menu("(Vehicle) Spawn Menu", menuItems.ToArray()));
    }

    void fromfileobject(string name)
    {
        if (File.Exists("scripts//" + name))
        {
            int no = 0;
            int propname = 0;
            Vector3 position = new Vector3();
            Vector3 rotation = new Vector3();
            bool dynamicc = false;

            foreach (string str in File.ReadAllLines("scripts//" + name))
            {
                no++;
                if (no == 1)
                {
                    propname = int.Parse(str);
                }
                else if (no == 2)
                {
                    position.X = float.Parse(str);
                }
                else if (no == 3)
                {
                    position.Y = float.Parse(str);
                }
                else if (no == 4)
                {
                    position.Z = float.Parse(str);
                }
                else if (no == 5)
                {
                    rotation.X = float.Parse(str);
                }
                else if (no == 6)
                {
                    rotation.Y = float.Parse(str);
                }
                else if (no == 7)
                {
                    rotation.Z = float.Parse(str);
                }
                else if (no == 8)
                {
                    if (str.ToLower() == "true")
                    {
                        dynamicc = true;
                    }
                    else
                    {
                        dynamicc = false;
                    }

                    UI.Notify("8!");
                    Prop idk = World.CreateProp(propname, position, true, false);
                    idk.Rotation = rotation;
                    idk.FreezePosition = !dynamicc;
                    lastprop = idk;

                    no = 0;
                }
            }

            UI.Notify("Done!");
        }
        else
        {
            UI.Notify("That file wasn't found.");
        }
    }

    Prop lastprop;

    private void savelastprop(string name)
    {
        if (lastprop != null)
        {
            string content = "";

            content += lastprop.Model.GetHashCode();
            content += Environment.NewLine;
            content += lastprop.Position.X;
            content += Environment.NewLine;
            content += lastprop.Position.Y;
            content += Environment.NewLine;
            content += lastprop.Position.Z - 0.8f;
            content += Environment.NewLine;
            content += lastprop.Rotation.X;
            content += Environment.NewLine;
            content += lastprop.Rotation.Y;
            content += Environment.NewLine;
            content += lastprop.Rotation.Z;
            content += Environment.NewLine;
            content += isdynamic;

            File.WriteAllText(("scripts//" + name), content);
        }
    }

    private void lastpropposset(Vector3 vec)
    {
        lastprop.Position = vec;
    }

    private void lastproprotset(Vector3 vec)
    {
        lastprop.Rotation = vec;
    }

    private void OpenLastPropMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Save Last Prop", "Example Input:\nblabla.txt");
        button.Activated += (sender, args) => this.savelastprop(Game.GetUserInput(25));
        menuItems.Add(button);

        var text = new MenuLabel("Position Set:", true);
        menuItems.Add(text);

        button = new MenuButton("+X", "");
        button.Activated += (sender, args) => this.lastpropposset(lastprop.Position + new Vector3(0.1f, 0f, 0f));
        menuItems.Add(button);

        button = new MenuButton("-X", "");
        button.Activated += (sender, args) => this.lastpropposset(lastprop.Position - new Vector3(0.1f, 0f, 0f));
        menuItems.Add(button);

        button = new MenuButton("+Y", "");
        button.Activated += (sender, args) => this.lastpropposset(lastprop.Position + new Vector3(0f, 0.1f, 0f));
        menuItems.Add(button);

        button = new MenuButton("-Y", "");
        button.Activated += (sender, args) => this.lastpropposset(lastprop.Position - new Vector3(0f, 0.1f, 0f));
        menuItems.Add(button);

        button = new MenuButton("+Z", "");
        button.Activated += (sender, args) => this.lastpropposset(lastprop.Position + new Vector3(0f, 0f, 0.1f));
        menuItems.Add(button);

        button = new MenuButton("-Z", "");
        button.Activated += (sender, args) => this.lastpropposset(lastprop.Position - new Vector3(0f, 0f, 0.1f));
        menuItems.Add(button);

        text = new MenuLabel("Rotation Set:", true);
        menuItems.Add(text);

        button = new MenuButton("+X", "");
        button.Activated += (sender, args) => this.lastproprotset(lastprop.Rotation + new Vector3(0.1f, 0f, 0f));
        menuItems.Add(button);

        button = new MenuButton("-X", "");
        button.Activated += (sender, args) => this.lastproprotset(lastprop.Rotation - new Vector3(0.1f, 0f, 0f));
        menuItems.Add(button);

        button = new MenuButton("+Y", "");
        button.Activated += (sender, args) => this.lastproprotset(lastprop.Rotation + new Vector3(0f, 0.1f, 0f));
        menuItems.Add(button);

        button = new MenuButton("-Y", "");
        button.Activated += (sender, args) => this.lastproprotset(lastprop.Rotation - new Vector3(0f, 0.1f, 0f));
        menuItems.Add(button);

        button = new MenuButton("+Z", "");
        button.Activated += (sender, args) => this.lastproprotset(lastprop.Rotation + new Vector3(0f, 0f, 0.1f));
        menuItems.Add(button);

        button = new MenuButton("-Z", "");
        button.Activated += (sender, args) => this.lastproprotset(lastprop.Rotation - new Vector3(0f, 0f, 0.1f));
        menuItems.Add(button);

        this.View.AddMenu(new Menu("Last Prop Menu", menuItems.ToArray()));
    }

    private void OpenSpawnMenuObject()
    {
        var menuItems = new List<IMenuItem>();
        
        var button = new MenuButton("Custom Input", "Example Input:\n-1818980770");
        button.Activated += (sender, args) => this.spawnprop(((Model)(int.Parse(Game.GetUserInput(20)))), Game.Player.Character.Position, false, true, Game.Player.Character.Heading);
        menuItems.Add(button);

        button = new MenuButton("From File", "Example Input:\nblabla.txt");
        button.Activated += (sender, args) => this.fromfileobject(Game.GetUserInput(25));
        menuItems.Add(button);

        button = new MenuButton("From Objects.ini File", "Example Input:\nObjects.ini\nThis was added to help you get more creations");
        button.Activated += (sender, args) => this.readobjectsini(Game.GetUserInput(25));
        menuItems.Add(button);

        button = new MenuButton("Last Prop", "");
        button.Activated += (sender, args) => this.OpenLastPropMenu();
        menuItems.Add(button);

        int numbr = 0;
        foreach (string str in mdllist.Keys)
        {
            if (!(numbr == 15))
            {
                numbr++;
                int blabla;
                mdllist.TryGetValue(str, out blabla);

                button = new MenuButton(str, blabla.ToString());
                button.Activated += (sender, args) => this.spawnprop(((Model)blabla), Game.Player.Character.Position, false, true, Game.Player.Character.Heading);
                menuItems.Add(button);
            }
        }
        if (numbr == 15)
        {
            button = new MenuButton("Page 2", "");
            button.Activated += (sender, args) => this.openspawnprop2(2);
            menuItems.Add(button);
        }
        lastmenu2 = new Menu("(Object) Spawn Menu", menuItems.ToArray());
        this.View.AddMenu(lastmenu2);
    }

    private void openspawnprop2(int page)
    {
        if (page == 2)
        {
            View.RemoveMenu(lastmenu2);
        }
        OpenSpawnMenuObject2(page);
    }

    Menu lastmenu2;
    private void OpenSpawnMenuObject2(int curpage)
    {
        var menuItems = new List<IMenuItem>();

        if (curpage > 2) //3 or bigger
        {
            View.RemoveMenu(lastmenu2);
            var buttonm = new MenuButton(("Page " + (curpage - 1)), ("See page " + (curpage - 1)));
            buttonm.Activated += (sender, args) => this.openspawnprop2(curpage - 1);
            menuItems.Add(buttonm);
        }

        int currentno = 0;
        int skipno = 0;
        foreach (string name in mdllist.Keys)
        {
            if (skipno == (15 * (curpage - 1)))
            {
                if (currentno == 15)
                {
                    var buttonm = new MenuButton(("Page " + (curpage + 1)), ("See page " + (curpage + 1)));
                    buttonm.Activated += (sender, args) => this.OpenSpawnMenuObject2(curpage + 1);
                    menuItems.Add(buttonm);
                    break;
                }
                currentno++;
                int mdll;
                mdllist.TryGetValue(name, out mdll);

                var button = new MenuButton(name, (mdll.ToString()));
                button.Activated += (sender, args) => this.spawnprop(((Model)mdll), Game.Player.Character.Position, false, true, Game.Player.Character.Heading);
                menuItems.Add(button);
            }
            else
            {
                skipno++;
            }
        }
        Menu thismenu = new Menu(("(Object) Spawn Menu " + curpage), menuItems.ToArray());
        lastmenu2 = thismenu;
        this.View.AddMenu(thismenu);
    }

    private void OpenSpawnMenuPed()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Custom Input", "Example Input:\n-1818980770");
        button.Activated += (sender, args) => this.spawnped(((Model)(int.Parse(Game.GetUserInput(26)))), Game.Player.Character.Position, Game.Player.Character.Heading);
        menuItems.Add(button);

        int numbr = 0;
        foreach (string str in pedlist.Keys)
        {
            if (!(numbr == 15))
            {
                numbr++;
                int blabla;
                pedlist.TryGetValue(str, out blabla);

                button = new MenuButton(str, blabla.ToString());
                button.Activated += (sender, args) => this.spawnped(((Model)blabla), Game.Player.Character.Position, Game.Player.Character.Heading);
                menuItems.Add(button);
            }
        }
        if (numbr == 15)
        {
            button = new MenuButton("Page 2", "");
            button.Activated += (sender, args) => this.openspawnped2(2);
            menuItems.Add(button);
        }
        lastmenu3 = new Menu("(Ped) Spawn Menu", menuItems.ToArray());
        this.View.AddMenu(lastmenu3);
    }

    private void OpenChangeModelMenu()
    {
        var menuItems = new List<IMenuItem>();

        var button = new MenuButton("Custom Input", "Example Input:\n-1818980770");
        button.Activated += (sender, args) => Function.Call(Hash.SET_PLAYER_MODEL, Game.Player, Game.GetUserInput(26));
        menuItems.Add(button);

        int numbr = 0;
        foreach (string str in pedlist.Keys)
        {
            if (!(numbr == 15))
            {
                numbr++;
                int blabla;
                pedlist.TryGetValue(str, out blabla);

                button = new MenuButton(str, blabla.ToString());
                button.Activated += (sender, args) => Function.Call(Hash.SET_PLAYER_MODEL, Game.Player, blabla);
                menuItems.Add(button);
            }
        }
        if (numbr == 15)
        {
            button = new MenuButton("Page 2", "");
            button.Activated += (sender, args) => this.openspawnped2(2);
            menuItems.Add(button);
        }
        lastmenu4 = new Menu("Change Model Menu", menuItems.ToArray());
        this.View.AddMenu(lastmenu4);
    }
    
    private void openchangemodel2(int page)
    {
        if (page == 2)
        {
            View.RemoveMenu(lastmenu4);
        }
        OpenChangeModelMenu2(page);
    }

    Menu lastmenu4;
    private void OpenChangeModelMenu2(int curpage)
    {
        var menuItems = new List<IMenuItem>();

        if (curpage > 2) //3 or bigger
        {
            View.RemoveMenu(lastmenu4);
            var buttonm = new MenuButton(("Page " + (curpage - 1)), ("See page " + (curpage - 1)));
            buttonm.Activated += (sender, args) => this.openchangemodel2(curpage - 1);
            menuItems.Add(buttonm);
        }

        int currentno = 0;
        int skipno = 0;
        foreach (string name in pedlist.Keys)
        {
            if (skipno == (15 * (curpage - 1)))
            {
                if (currentno == 15)
                {
                    var buttonm = new MenuButton(("Page " + (curpage + 1)), ("See page " + (curpage + 1)));
                    buttonm.Activated += (sender, args) => this.openchangemodel2(curpage + 1);
                    menuItems.Add(buttonm);
                    break;
                }
                currentno++;
                int mdll;
                pedlist.TryGetValue(name, out mdll);

                var button = new MenuButton(name, (mdll.ToString()));
                button.Activated += (sender, args) => Function.Call(Hash.SET_PLAYER_MODEL, Game.Player, mdll);
                menuItems.Add(button);
            }
            else
            {
                skipno++;
            }
        }
        Menu thismenu = new Menu(("Change Model Menu " + curpage), menuItems.ToArray());
        lastmenu4 = thismenu;
        this.View.AddMenu(thismenu);
    }
    private void openspawnped2(int page)
    {
        if (page == 2)
        {
            View.RemoveMenu(lastmenu3);
        }
        OpenSpawnMenuPed2(page);
    }

    Menu lastmenu3;
    private void OpenSpawnMenuPed2(int curpage)
    {
        var menuItems = new List<IMenuItem>();

        if (curpage > 2) //3 or bigger
        {
            View.RemoveMenu(lastmenu3);
            var buttonm = new MenuButton(("Page " + (curpage - 1)), ("See page " + (curpage - 1)));
            buttonm.Activated += (sender, args) => this.openspawnped2(curpage - 1);
            menuItems.Add(buttonm);
        }

        int currentno = 0;
        int skipno = 0;
        foreach (string name in pedlist.Keys)
        {
            if (skipno == (15 * (curpage - 1)))
            {
                if (currentno == 15)
                {
                    var buttonm = new MenuButton(("Page " + (curpage + 1)), ("See page " + (curpage + 1)));
                    buttonm.Activated += (sender, args) => this.OpenSpawnMenuPed2(curpage + 1);
                    menuItems.Add(buttonm);
                    break;
                }
                currentno++;
                int mdll;
                pedlist.TryGetValue(name, out mdll);

                var button = new MenuButton(name, (mdll.ToString()));
                button.Activated += (sender, args) => this.spawnped(((Model)mdll), Game.Player.Character.Position, Game.Player.Character.Heading);
                menuItems.Add(button);
            }
            else
            {
                skipno++;
            }
        }
        Menu thismenu = new Menu(("(Ped) Spawn Menu " + curpage), menuItems.ToArray());
        lastmenu3 = thismenu;
        this.View.AddMenu(thismenu);
    }

    private void spawnped(Model modelname, Vector3 pos)
    {
        World.CreatePed(modelname, pos);
    }

    private void spawnped(Model modelname, Vector3 pos, float heading)
    {
        World.CreatePed(modelname, pos, heading);
    }
    bool isdynamic;
    private void spawnprop(Model modelname, Vector3 pos, bool dynamic, bool placeonground)
    {
        isdynamic = dynamic;
        Prop idk = World.CreateProp(modelname, pos, dynamic, placeonground);
        idk.FreezePosition = !dynamic;
        lastprop = idk;
    }

    private void spawnprop(Model modelname, Vector3 pos, bool dynamic, bool placeonground, float heading)
    {
        isdynamic = dynamic;
        Prop idk = World.CreateProp(modelname, pos, true, placeonground);
        idk.Heading = heading;
        idk.FreezePosition = !dynamic;
        lastprop = idk;
    }


    //TODO: Spawn Ramps and stuff. Ability to load from txt.
    //^Do not do until further notice

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

        button = new MenuButton("Burn", "*fire sound*");
        button.Activated += (sender, args) => this.BreatheFire(playa);
        menuItems.Add(button);

        button = new MenuButton("Freeze", "");
        button.Activated += (sender, args) => this.Freeze(playa);
        menuItems.Add(button);

        button = new MenuButton("UnFreeze", "");
        button.Activated += (sender, args) => this.Unfreeze(playa);
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
            toggle = new MenuToggle("Show FPS", "60fps ftw", showfps);
            toggle.Changed += (sender, args) =>
            {
                var tg = sender as MenuToggle;
                if (tg == null)
                {
                    return;
                }
                showfps = tg.Value;
            };
            menuItems.Add(toggle);
            
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

            toggle = new MenuToggle("Unlimited Ability", "gooota beee slooooow\nor gata be stronk", featurePlayerUnlimitedAbility);
            toggle.Changed += (sender, args) =>
            {
                var tg = sender as MenuToggle;
                if (tg == null)
                {
                    return;
                }
                featurePlayerUnlimitedAbility = tg.Value;
            };
            menuItems.Add(toggle);

            toggle = new MenuToggle("No Noise", "*empty, cuz u cant hear m8*", featurePlayerNoNoise);
            toggle.Changed += (sender, args) =>
            {
                var tg = sender as MenuToggle;
                if (tg == null)
                {
                    return;
                }
                featurePlayerNoNoiseUpdated = tg.Value;
                featurePlayerNoNoise = tg.Value;
            };
            menuItems.Add(toggle);

            toggle = new MenuToggle("Super Jump", "aka KANGAROO in GTA:SA", featurePlayerSuperJump);
            toggle.Changed += (sender, args) =>
            {
                var tg = sender as MenuToggle;
                if (tg == null)
                {
                    return;
                }
                featurePlayerSuperJump = tg.Value;
            };
            menuItems.Add(toggle);

            toggle = new MenuToggle("Fire Ammo", "Glorious Helicopters <3", featureWeaponFireAmmo);
            toggle.Changed += (sender, args) =>
            {
                var tg = sender as MenuToggle;
                if (tg == null)
                {
                    return;
                }
                featureWeaponFireAmmo = tg.Value;
            };
            menuItems.Add(toggle);

            toggle = new MenuToggle("Explosive Ammo", "Wait didn't I already code this?", featureWeaponExplosiveAmmo);
            toggle.Changed += (sender, args) =>
            {
                var tg = sender as MenuToggle;
                if (tg == null)
                {
                    return;
                }
                featureWeaponExplosiveAmmo = tg.Value;
            };
            menuItems.Add(toggle);

            toggle = new MenuToggle("Explosive Melee", "Wait didn't I already code this?", featureWeaponExplosiveMelee);
            toggle.Changed += (sender, args) =>
            {
                var tg = sender as MenuToggle;
                if (tg == null)
                {
                    return;
                }
                featureWeaponExplosiveMelee = tg.Value;
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

        button = new MenuButton("Weather: ThunderStorm", "Watch dem clouds");
        button.Activated += (sender, args) => this.setweather(Weather.ThunderStorm);
        menuItems.Add(button);

        var numerogg = new MenuNumericScroller(("Gravity Level"), "0 is normal, 3 is fly", 0, 3, 1, grav);
        numerogg.Changed += numerogg_Changed;
        menuItems.Add(numerogg);
        //button = new MenuButton("Goto Mission Marker" + GTA.World.GetActiveBlips().Length, "ezpz races");
        //button.Activated += (sender, args) => this.GotoMissionMarker2();
        //menuItems.Add(button);

        this.View.AddMenu(new Menu("World Menu", menuItems.ToArray()));
    }
    int grav = 0;
    void numerogg_Changed(object sender, MenuItemDoubleValueArgs e)
    {
        grav = (int)((MenuNumericScroller)sender).Value;
        World.GravityLevel = grav;
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
        //Doesnt work
        //toggle = new MenuToggle("Max 6 Star Wanted Level", "5 stars r 2ez4me", six_star);
        //toggle.Changed += (sender, args) =>
        //{
        //    var tggg = sender as MenuToggle;
        //    if (tggg == null)
        //    {
        //        return;
        //    }
        //    six_star = tggg.Value;
        //    if (tggg.Value)
        //    {
        //        Function.Call(Hash.SET_MAX_WANTED_LEVEL, 6);
        //    }
        //    else
        //    {
        //        Function.Call(Hash.SET_MAX_WANTED_LEVEL, 5);
        //    }
        //};
        //menuItems.Add(toggle);

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

    void readobjectsini(string fileloc)
    {
        bool donetp = false;
        int current = 0;
        Vector3 position = new Vector3();
        int mdlhash = 0;
        Quaternion rotation = new Quaternion();
        Vector3 plctotpplayer = new Vector3();
        bool dyn = false;

        foreach (string str in File.ReadAllLines("scripts//" + fileloc))
        {
            current++;
            if (!donetp)
            {
                if (current == 1)
                {
                    if (str != "[Player]")
                    {
                        donetp = true;
                    }
                    //do nothing, reads "[Player]"
                }
                else if (current == 2)
                {
                    if (str != "Teleport=1")
                    {
                        position.X = float.Parse(str.Replace("x=", "").Replace(".", ","));
                        donetp = true;
                    }
                }
                else if (current == 3)
                {
                    plctotpplayer.X = float.Parse(str.Replace("x=", "").Replace(".", ","));
                }
                else if (current == 4)
                {
                    plctotpplayer.Y = float.Parse(str.Replace("y=", "").Replace(".", ","));
                }
                else if (current == 5)
                {
                    plctotpplayer.Z = float.Parse(str.Replace("z=", "").Replace(".", ","));
                    current = 0;
                    Game.Player.Character.Position = plctotpplayer;
                    donetp = true;
                }
            }
            else
            {
                if (current == 1)
                {
                    //empty [1] etc
                }
                else if (current == 2)
                {
                    position.X = float.Parse(str.Replace("x=", "").Replace(".", ","));
                }
                else if (current == 3)
                {
                    position.Y = float.Parse(str.Replace("y=", "").Replace(".", ","));
                }
                else if (current == 4)
                {
                    position.Z = float.Parse(str.Replace("z=", "").Replace(".", ","));
                }
                else if (current == 5)
                {
                    //height
                }
                else if (current == 6)
                {
                    mdlhash = int.Parse(str.Replace("Model=", ""));
                }
                else if (current == 7)
                {
                    rotation.X = float.Parse(str.Replace("qx=", "").Replace(".", ","));
                }
                else if (current == 8)
                {
                    rotation.Y = float.Parse(str.Replace("qy=", "").Replace(".", ","));
                }
                else if (current == 9)
                {
                    rotation.Z = float.Parse(str.Replace("qz=", "").Replace(".", ","));
                }
                else if (current == 10)
                {
                    rotation.W = float.Parse(str.Replace("qw=", "").Replace(".", ","));
                }
                else if (current == 11)
                {
                    //offset z?
                }
                else if (current == 12)
                {
                    if (str == "Dynamic=0")
                    {
                        dyn = false;
                    }
                    else
                    {
                        dyn = true;
                    }
                    current = 0;
                    Prop idkm8 = World.CreateProp(mdlhash, position, dyn, false);

                    idkm8.Rotation = QuaternionToEulerAngles(rotation);
                }
            }
        }
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

    // https://github.com/crosire/scripthookvdotnet/issues/263#event-364495345
    //Source: http://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToEuler/index.htm
    public Vector3 QuaternionToEulerAngles(Quaternion q)
    {
        var t = q.X * q.Y + q.Z * q.W;
        if (t > 0.499)
        {
            return new Vector3((float)(2 * System.Math.Atan2(q.X, q.W)), (float)(System.Math.PI / 2), 0);
        }
        else if (t < -0.499)
        {
            return new Vector3((float)(-2 * System.Math.Atan2(q.X, q.W)), (float)(-System.Math.PI / 2), 0);
        }
        else
        {
            return new Vector3((float)(System.Math.Atan2(2 * q.Y * q.W - 2 * q.X * q.Z, 1 - 2 * q.Y * q.Y - 2 * q.Z * q.Z)), (float)(System.Math.Asin(2 * t)), (float)(System.Math.Atan2(2 * q.X * q.W - 2 * q.Y * q.Z, 1 - 2 * q.X * q.X - 2 * q.Z * q.Z)));
        }
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
        OpenVehicleMarkMenu();
    }

    void numerog_Changed(object sender, MenuItemDoubleValueArgs e)
    {
        curmarkped = (int)((MenuNumericScroller)sender).Value + 1;

        if (curmarkped == 1)
        {
            markedped = markedped1;
        }
        else if (curmarkped == 2)
        {
            markedped = markedped2;
        }
        else if (curmarkped == 3)
        {
            markedped = markedped3;
        }
        else //error
        {
            curmarkped = 1;
        }
        View.RemoveMenu(Markmenu);
        OpenPedMarkMenu();
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
        OpenVehicleMarkMenu();
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


    void loadnyankton(bool bth)
    {
        //bth bury the hatchet
        Function.Call(Hash.REQUEST_IPL, "plg_01");
        Function.Call(Hash.REQUEST_IPL, "prologue01");
        Function.Call(Hash.REQUEST_IPL, "prologue01_lod");
        Function.Call(Hash.REQUEST_IPL, "prologue01c");
        Function.Call(Hash.REQUEST_IPL, "prologue01c_lod");
        Function.Call(Hash.REQUEST_IPL, "prologue01d");
        Function.Call(Hash.REQUEST_IPL, "prologue01d_lod");
        Function.Call(Hash.REQUEST_IPL, "prologue01e");
        Function.Call(Hash.REQUEST_IPL, "prologue01e_lod");
        Function.Call(Hash.REQUEST_IPL, "prologue01f");
        Function.Call(Hash.REQUEST_IPL, "prologue01f_lod");
        Function.Call(Hash.REQUEST_IPL, "prologue01g");
        Function.Call(Hash.REQUEST_IPL, "prologue01h");
        Function.Call(Hash.REQUEST_IPL, "prologue01h_lod");
        Function.Call(Hash.REQUEST_IPL, "prologue01i");
        Function.Call(Hash.REQUEST_IPL, "prologue01i_lod");
        Function.Call(Hash.REQUEST_IPL, "prologue01j");
        Function.Call(Hash.REQUEST_IPL, "prologue01j_lod");
        Function.Call(Hash.REQUEST_IPL, "prologue01k");
        Function.Call(Hash.REQUEST_IPL, "prologue01k_lod");
        Function.Call(Hash.REQUEST_IPL, "prologue01z");
        Function.Call(Hash.REQUEST_IPL, "prologue01z_lod");
        Function.Call(Hash.REQUEST_IPL, "plg_02");
        Function.Call(Hash.REQUEST_IPL, "prologue02");
        Function.Call(Hash.REQUEST_IPL, "prologue02_lod");
        Function.Call(Hash.REQUEST_IPL, "plg_03");
        Function.Call(Hash.REQUEST_IPL, "prologue03");
        Function.Call(Hash.REQUEST_IPL, "prologue03_lod");
        Function.Call(Hash.REQUEST_IPL, "prologue03b");
        Function.Call(Hash.REQUEST_IPL, "prologue03b_lod");
        if (bth)
        {
            Function.Call(Hash.REQUEST_IPL, "prologue03_grv_cov");
            Function.Call(Hash.REQUEST_IPL, "prologue03_grv_cov_lod");
            Function.Call(Hash.REQUEST_IPL, "prologue03_grv_fun");
        }
        else
        {
            Function.Call(Hash.REQUEST_IPL, "prologue03_grv_dug");
            Function.Call(Hash.REQUEST_IPL, "prologue03_grv_dug_lod");
            Function.Call(Hash.REQUEST_IPL, "prologue_grv_torch");
        }
        Function.Call(Hash.REQUEST_IPL, "plg_04");
        Function.Call(Hash.REQUEST_IPL, "prologue04");
        Function.Call(Hash.REQUEST_IPL, "prologue04_lod");
        Function.Call(Hash.REQUEST_IPL, "prologue04b");
        Function.Call(Hash.REQUEST_IPL, "prologue04b_lod");
        Function.Call(Hash.REQUEST_IPL, "prologue04_cover");
        Function.Call(Hash.REQUEST_IPL, "des_protree_end");
        Function.Call(Hash.REQUEST_IPL, "des_protree_start");
        Function.Call(Hash.REQUEST_IPL, "des_protree_start_lod");
        Function.Call(Hash.REQUEST_IPL, "plg_05");
        Function.Call(Hash.REQUEST_IPL, "prologue05");
        Function.Call(Hash.REQUEST_IPL, "prologue05_lod");
        Function.Call(Hash.REQUEST_IPL, "prologue05b");
        Function.Call(Hash.REQUEST_IPL, "prologue05b_lod");
        Function.Call(Hash.REQUEST_IPL, "plg_06");
        Function.Call(Hash.REQUEST_IPL, "prologue06");
        Function.Call(Hash.REQUEST_IPL, "prologue06_lod");
        Function.Call(Hash.REQUEST_IPL, "prologue06b");
        Function.Call(Hash.REQUEST_IPL, "prologue06b_lod");
        Function.Call(Hash.REQUEST_IPL, "prologue06_int");
        Function.Call(Hash.REQUEST_IPL, "prologue06_int_lod");
        Function.Call(Hash.REQUEST_IPL, "prologue06_pannel");
        Function.Call(Hash.REQUEST_IPL, "prologue06_pannel_lod");
        Function.Call(Hash.REQUEST_IPL, "prologue_m2_door");
        Function.Call(Hash.REQUEST_IPL, "prologue_m2_door_lod");
        Function.Call(Hash.REQUEST_IPL, "plg_occl_00");
        Function.Call(Hash.REQUEST_IPL, "prologue_occl");
        Function.Call(Hash.REQUEST_IPL, "plg_rd");
        Function.Call(Hash.REQUEST_IPL, "prologuerd");
        Function.Call(Hash.REQUEST_IPL, "prologuerdb");
        Function.Call(Hash.REQUEST_IPL, "prologuerd_lod");
    }

    void tptony(Ped name)
    {
        if (name.IsInVehicle())
        {
            name.CurrentVehicle.Position = new Vector3(3360.19f, -4849.67f, 111.8f);
        }
        else
        {
            name.Position = new Vector3(3360.19f, -4849.67f, 111.8f);
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
    //System.Drawing.Color.FromArgb(0xFF0000);  

    private void hexcarprimcolor(Vehicle veh)
    {
        string got = Game.GetUserInput(7);
        veh.CustomPrimaryColor = Color.FromArgb(int.Parse(got.Substring(0, 2), System.Globalization.NumberStyles.HexNumber), int.Parse(got.Substring(2, 2), System.Globalization.NumberStyles.HexNumber), int.Parse(got.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
    }

    private void hexcarseccolor(Vehicle veh)
    {
        string got = Game.GetUserInput(7);
        veh.CustomSecondaryColor = Color.FromArgb(int.Parse(got.Substring(0, 2), System.Globalization.NumberStyles.HexNumber), int.Parse(got.Substring(2, 2), System.Globalization.NumberStyles.HexNumber), int.Parse(got.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
    }

    private void rgbcarprimcolor(Vehicle veh)
    {
        veh.CustomPrimaryColor = Color.FromArgb(int.Parse(Game.GetUserInput(4)), int.Parse(Game.GetUserInput(4)), int.Parse(Game.GetUserInput(4)));
    }

    private void rgbcarseccolor(Vehicle veh)
    {
        veh.CustomSecondaryColor = Color.FromArgb(int.Parse(Game.GetUserInput(4)), int.Parse(Game.GetUserInput(4)), int.Parse(Game.GetUserInput(4)));
    }

    private void rgbcarclean(Vehicle veh)
    {
        veh.ClearCustomPrimaryColor();
        veh.ClearCustomSecondaryColor();
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
            OpenVehicleMarkMenu();
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
            OpenVehicleMarkMenu();
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
            Function.Call(Hash.SET_VEHICLE_DOOR_OPEN, veh, i, true, true);
        }
    }
    private void Doorclose(Vehicle veh)
    {
        for (int i = 0; i <= 7; i++)
        {
            Function.Call(Hash.SET_VEHICLE_DOOR_SHUT, veh, i, true);
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
        playa.Kill();
    }

    void Freeze(Ped playa)
    {
        playa.FreezePosition = true;
        if (playa.IsInVehicle())
        {
            playa.CurrentVehicle.FreezePosition = true;
        }
    }

    void Unfreeze(Ped playa)
    {
        playa.FreezePosition = false;
        if (playa.IsInVehicle())
        {
            playa.CurrentVehicle.FreezePosition = false;
        }
    }

    void OnTick(object sender, EventArgs e)
    {
        if (never_wanted)
        {
            Game.Player.WantedLevel = 0;
        }

        if (showfps)
        {
            UIText uIText = new UIText((Game.FPS.ToString()), new Point(10, 10), 0.4f, Color.Red);
            uIText.Draw();
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

            if (vehiclegun && Game.Player.Character.IsShooting)
            {
                Game.Player.Character.FreezePosition = true;

                //http://stackoverflow.com/questions/3132126/how-do-i-select-a-random-value-from-an-enumeration

                Array values = Enum.GetValues(typeof(VehicleHash));
                Random random = new Random();
                VehicleHash randomVeh = (VehicleHash)values.GetValue(random.Next(values.Length));
                //Zentorno only cuz randoms usually crash game
                Vehicle createdveh = World.CreateVehicle(VehicleHash.Zentorno, Game.Player.Character.Position, Game.Player.Character.Heading);
                createdveh.Speed = 1000;
            }
            else if (vehiclegun)
            {
                Game.Player.Character.FreezePosition = false;
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
                    OpenVehicleMarkMenu();
                }
            }

            if (markgunped)
            {
                if (Game.Player.GetTargetedEntity().Exists() && Game.Player.GetTargetedEntity().Model.IsPed)
                {
                    if (curmarkped == 1)
                    {
                        if (markedped1 != null)
                        {
                            Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, (Entity)markedped1, false);
                        }
                        markedped1 = (Ped)Game.Player.GetTargetedEntity();
                        markedped = markedped1;
                    }
                    else if (curmarkped == 2)
                    {
                        if (markedped2 != null)
                        {
                            Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, (Entity)markedped2, false);
                        }
                        markedped2 = (Ped)Game.Player.GetTargetedEntity();
                        markedped = markedped2;
                    }
                    else if (curmarkped == 3)
                    {
                        if (markedped3 != null)
                        {
                            Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, (Entity)markedped3, false);
                        }
                        markedped3 = (Ped)Game.Player.GetTargetedEntity();
                        markedped = markedped3;
                    }
                    markgunped = false;
                    View.RemoveMenu(Markmenu);
                    OpenPedMarkMenu();
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

            // player special ability
            if (featurePlayerUnlimitedAbility)
            {
                Function.Call(Hash._RECHARGE_SPECIAL_ABILITY, Game.Player, 1);
            }

            // player no noise
            if (featurePlayerNoNoiseUpdated)
            {
                if (!featurePlayerNoNoise)
                    Function.Call(Hash.SET_PLAYER_NOISE_MULTIPLIER, Game.Player, 1.0);
                featurePlayerNoNoiseUpdated = false;
            }

            if (featurePlayerNoNoise)
            {
                Function.Call(Hash.SET_PLAYER_NOISE_MULTIPLIER, Game.Player, 0.0);
            }

            if (featurePlayerSuperJump)
            {
                Function.Call(Hash.SET_SUPER_JUMP_THIS_FRAME, Game.Player);
            }

            if (featureWeaponFireAmmo)
            {
                Function.Call(Hash.SET_FIRE_AMMO_THIS_FRAME, Game.Player);
            }
            if (featureWeaponExplosiveAmmo)
            {
                Function.Call(Hash.SET_EXPLOSIVE_AMMO_THIS_FRAME, Game.Player);
            }
            if (featureWeaponExplosiveMelee)
            {
                Function.Call(Hash.SET_EXPLOSIVE_MELEE_THIS_FRAME, Game.Player);
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
            int iint32 = this.speed - 1;
            UIText uIText = new UIText(string.Concat("Speed: ", iint32.ToString()), new Point(500, 50), 0.4f, Color.White);
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

    void rainmoney()
    {
        UI.Notify("WARNING! DO NOT USE THIS FUNCTION IN ONLINE.", true);
        Hash moneypickup = Function.Call<Hash>(Hash.GET_HASH_KEY, "PICKUP_MONEY_CASE");
        //http://ecb2.biz/releases/GTAV/lists/pickups.txt
        InputArgument[] stuff = { moneypickup.ToString(), Game.Player.Character.Position.X, Game.Player.Character.Position.Y, (Game.Player.Character.Position.Z + 0.5f), 0, 40000, 289396019, false, true };
        Function.Call(Hash.CREATE_AMBIENT_PICKUP, stuff);
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
