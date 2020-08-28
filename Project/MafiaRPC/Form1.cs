using MafiaRPC.DiscordRpcDemo;
using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace MafiaRPC
{
    public partial class Form1 : Form
    {
        private DiscordRpc.EventHandlers handlers;
        private DiscordRpc.RichPresence presence;
        private long time = DateTimeOffset.Now.ToUnixTimeSeconds();

        private int hp;
        private int gun;
        private string mission;
        private int CarcyclopediaID;
        private string MAFIA_VERSION;

        public Form1()
        {
            InitializeComponent();
        }

        //1.0--------------------------------------------------
        private IntPtr baseAddressMission = (IntPtr)0x0065115C;
        private IntPtr baseAddressGun = (IntPtr)0x006F9464;
        private IntPtr baseAddressCar = (IntPtr)0x0067A4D8;
        //-----------------------------------------------------

        //1.1-------------------------------------------------
        private IntPtr baseAddressMission11 = (IntPtr)0x00646D90;
        private IntPtr baseAddressGun11 = (IntPtr)0x00646D4C;
        private IntPtr baseAddressCar11 = (IntPtr)0x006BC7C0;
        //----------------------------------------------------

        //1.2----------------------------------------------------
        private IntPtr baseAddressMission12 = (IntPtr)0x0063788C;
        private IntPtr baseAddressGun12 = (IntPtr)0x00647E1C;
        private IntPtr baseAddressCar12 = (IntPtr)0x006BD890;
        //-------------------------------------------------------

        private void AfterTimer(object sender, EventArgs e)
        {
            ReadMafia();
        }

        private void ReadMafia()
        {
            Process[] processlist = Process.GetProcessesByName("Game");

            if (processlist.Length != 0)
            {

                String result = ":(";
                foreach (Process p in processlist)
                {
                    result = p.MainModule.FileName;
                    break;
                }

                if (result.Contains("Game.exe"))
                {
                    result = result.Remove(result.IndexOf("Game.exe"));
                    result = result + "log.txt";
                    string log = File.ReadAllText(result);

                    if (log.Contains("GetVer:384"))
                    {
                        MAFIA_VERSION = "1.0";
                        Mafia10();
                    }

                    if (log.Contains("GetVer:393"))
                    {
                        MAFIA_VERSION = "1.1";
                        Mafia11();
                    }

                    if (log.Contains("GetVer:395"))
                    {
                        MAFIA_VERSION = "1.2";
                        Mafia12();
                    }
                }

                else
                {
                    timer1.Stop();
                    MessageBox.Show("Wrong path to the game", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    label1.Text = string.Empty;
                    button1.Text = "Activate";
                    DiscordRpc.Shutdown();
                }
            }

            else
            {
                timer1.Stop();
                MessageBox.Show("Wrong path to the game", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                label1.Text = string.Empty;
                button1.Text = "Activate";
                DiscordRpc.Shutdown();
            }
        }

        private void Mafia10()
        {
            Process[] proc = Process.GetProcessesByName("Game");

            if (proc.Length != 0)
            {
                VAMemory memory = new VAMemory("Game");

                this.handlers = default(DiscordRpc.EventHandlers);
                DiscordRpc.Initialize("747679968389234783", ref this.handlers, true, null);
                this.handlers = default(DiscordRpc.EventHandlers);
                DiscordRpc.Initialize("747679968389234783", ref this.handlers, true, null);
                this.presence.largeImageKey = "mafia";
                this.presence.largeImageText = "Mafia";

                // READ ADDRESSES WITH POINTERS
                //-----------------------------------------------------------------------------------

                baseAddressMission = (IntPtr)0x0065115C;
                baseAddressMission = IntPtr.Add((IntPtr)memory.ReadInt32(baseAddressMission), 0x68);
                baseAddressMission = IntPtr.Add((IntPtr)memory.ReadInt32(baseAddressMission), 0x0);

                baseAddressGun = (IntPtr)0x006F9464;
                baseAddressGun = IntPtr.Add((IntPtr)memory.ReadInt32(baseAddressGun), 0xE4);
                baseAddressGun = IntPtr.Add((IntPtr)memory.ReadInt32(baseAddressGun), 0xAF0);

                baseAddressCar = (IntPtr)0x0067A4D8;
                baseAddressCar = IntPtr.Add((IntPtr)memory.ReadInt32(baseAddressCar), 0x48);

                hp = memory.ReadInt32((IntPtr)0x00661538);
                mission = memory.ReadStringASCII(baseAddressMission, 20);
                gun = memory.ReadInt32((IntPtr)baseAddressGun);
                CarcyclopediaID = memory.ReadInt32((IntPtr)baseAddressCar);

                //-----------------------------------------------------------------------------------

                mission = mission.Remove(mission.IndexOf("\0"));
                this.presence.state = hp.ToString() + " HP";
                this.presence.startTimestamp = time;
                this.presence.endTimestamp = 0;
                switches();
            }
        }

        private void Mafia11()
        {
            Process[] proc = Process.GetProcessesByName("Game");

            if (proc.Length != 0)
            {
                VAMemory memory = new VAMemory("Game");

                this.handlers = default(DiscordRpc.EventHandlers);
                DiscordRpc.Initialize("747679968389234783", ref this.handlers, true, null);
                this.handlers = default(DiscordRpc.EventHandlers);
                DiscordRpc.Initialize("747679968389234783", ref this.handlers, true, null);
                this.presence.largeImageKey = "mafia";
                this.presence.largeImageText = "Mafia";

                // READ ADDRESSES WITH POINTERS
                //-----------------------------------------------------------------------------------

                baseAddressMission11 = (IntPtr)0x00646D90;
                baseAddressMission11 = IntPtr.Add((IntPtr)memory.ReadInt32(baseAddressMission11), 0x0);

                baseAddressGun11 = (IntPtr)0x00646D4C;
                baseAddressGun11 = IntPtr.Add((IntPtr)memory.ReadInt32(baseAddressGun11), 0xE4);
                baseAddressGun11 = IntPtr.Add((IntPtr)memory.ReadInt32(baseAddressGun11), 0xAF0);

                baseAddressCar11 = (IntPtr)0x006BC7C0;
                baseAddressCar11 = IntPtr.Add((IntPtr)memory.ReadInt32(baseAddressCar11), 0x30);

                hp = memory.ReadInt32((IntPtr)0x006C2AC0);
                mission = memory.ReadStringASCII(baseAddressMission11, 20);
                gun = memory.ReadInt32((IntPtr)baseAddressGun11);
                CarcyclopediaID = memory.ReadInt32((IntPtr)baseAddressCar11);

                //-----------------------------------------------------------------------------------

                mission = mission.Remove(mission.IndexOf("\0"));
                this.presence.state = hp.ToString() + " HP";
                this.presence.startTimestamp = time;
                this.presence.endTimestamp = 0;
                switches();
            }
        }

        private void Mafia12()
        {
            Process[] proc = Process.GetProcessesByName("Game");

            if (proc.Length != 0)
            {
                VAMemory memory = new VAMemory("Game");

                this.handlers = default(DiscordRpc.EventHandlers);
                DiscordRpc.Initialize("747679968389234783", ref this.handlers, true, null);
                this.handlers = default(DiscordRpc.EventHandlers);
                DiscordRpc.Initialize("747679968389234783", ref this.handlers, true, null);
                this.presence.largeImageKey = "mafia";
                this.presence.largeImageText = "Mafia";

                // READ ADDRESSES WITH POINTERS
                //-----------------------------------------------------------------------------------

                baseAddressMission12 = (IntPtr)0x0063788C;
                baseAddressMission12 = IntPtr.Add((IntPtr)memory.ReadInt32(baseAddressMission12), 0x68);
                baseAddressMission12 = IntPtr.Add((IntPtr)memory.ReadInt32(baseAddressMission12), 0x0);

                baseAddressGun12 = (IntPtr)0x00647E1C;
                baseAddressGun12 = IntPtr.Add((IntPtr)memory.ReadInt32(baseAddressGun12), 0xE4);
                baseAddressGun12 = IntPtr.Add((IntPtr)memory.ReadInt32(baseAddressGun12), 0xAF0);

                baseAddressCar12 = (IntPtr)0x006BD890;
                baseAddressCar12 = IntPtr.Add((IntPtr)memory.ReadInt32(baseAddressCar12), 0x30);

                hp = memory.ReadInt32((IntPtr)0x006C3B90);
                mission = memory.ReadStringASCII(baseAddressMission12, 20);
                gun = memory.ReadInt32((IntPtr)baseAddressGun12);
                CarcyclopediaID = memory.ReadInt32((IntPtr)baseAddressCar12);

                //-----------------------------------------------------------------------------------

                mission = mission.Remove(mission.IndexOf("\0"));
                this.presence.state = hp.ToString() + " HP";
                this.presence.startTimestamp = time;
                this.presence.endTimestamp = 0;
                switches();
            }
        }

        private void switches()
        {
            Process[] proc = Process.GetProcessesByName("Game");

            if (proc.Length != 0)
            {
                VAMemory memory = new VAMemory("Game");

                // CHECK GUN
                //-----------------------------------------------------------------------------------
                switch (gun)
                {
                    case 0:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Empty hands";
                        break;

                    case 1:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Action";
                        break;

                    case 2:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Knuckle Duster";
                        break;

                    case 3:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Knife";
                        break;

                    case 4:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Baseball bat";
                        break;

                    case 5:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Molotov";
                        break;

                    case 6:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Colt Detective Special";
                        break;

                    case 7:
                        this.presence.state = hp.ToString() + " HP" + ", " + "S&W 27 Magnum";
                        break;

                    case 8:
                        this.presence.state = hp.ToString() + " HP" + ", " + "S&W 10";
                        break;

                    case 9:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Colt 1911";
                        break;

                    case 10:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Thompson 1928";
                        break;

                    case 11:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Pump-action shotgun";
                        break;

                    case 12:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Sawed-off shotgun";
                        break;

                    case 13:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Springfield M1903";
                        break;

                    case 14:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Mosin-Nagant 1891/30";
                        break;

                    case 15:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Grenade";
                        break;

                    case 16:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Key";
                        break;

                    case 17:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Bucket";
                        break;

                    case 18:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Flashlight";
                        break;

                    case 19:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Book/Docs";
                        break;

                    case 20:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Bar";
                        break;

                    case 21:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Papers";
                        break;

                    case 22:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Bomb";
                        break;

                    case 23:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Door keys";
                        break;

                    case 24:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Safe key";
                        break;

                    case 25:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Crowbar";
                        break;

                    case 26:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Fly tickets";
                        break;

                    case 27:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Box";
                        break;

                    case 28:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Plank";
                        break;

                    case 29:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Bottle";
                        break;

                    case 30:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Small key";
                        break;

                    case 31:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Sword";
                        break;

                    case 32:
                        this.presence.state = hp.ToString() + " HP" + ", " + "Dog's head";
                        break;

                    default:
                        this.presence.state = hp.ToString() + " HP";
                        break;
                }

                // CHECK MISSION      
                //-----------------------------------------------------------------------------------

                switch (mission)
                {
                    case "":
                        this.presence.details = string.Empty;
                        this.presence.state = string.Empty;
                        break;

                    case "00menu":
                        this.presence.details = "In Main Menu";
                        this.presence.largeImageKey = "00menu";
                        this.presence.largeImageText = "In Main Menu";
                        this.presence.smallImageKey = "mafia";
                        this.presence.state = string.Empty;
                        break;

                    case "autosalon":
                        this.presence.details = "Carcyclopedia";
                        this.presence.largeImageKey = "carcyklopedia";
                        this.presence.largeImageText = "Carcyclopedia";
                        this.presence.smallImageKey = "mafia";
                        this.presence.state = string.Empty;
                        break;

                    case "carcyclopedia":
                        this.presence.details = "Carcyclopedia";
                        this.presence.largeImageKey = "carcyklopedia";
                        this.presence.largeImageText = "Carcyclopedia";
                        this.presence.smallImageKey = "mafia";
                        this.presence.state = string.Empty;
                        break;

                    case "extreme":
                        this.presence.details = "Free Ride Extreme";
                        this.presence.largeImageKey = "freeride";
                        this.presence.largeImageText = "Free Ride Extreme";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "freeitaly":
                        this.presence.details = "Free Ride City - Small";
                        this.presence.largeImageKey = "freeride";
                        this.presence.largeImageText = "Free Ride City - Small";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "freekrajina":
                        this.presence.details = "Free Ride Country Side - Daytime";
                        this.presence.largeImageKey = "freeride";
                        this.presence.largeImageText = "Free Ride Country Side - Daytime";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "freekrajinanoc":
                        this.presence.details = "Free Ride Country Side - Night";
                        this.presence.largeImageKey = "freeride";
                        this.presence.largeImageText = "Free Ride Country Side - Night";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "freeride":
                        this.presence.details = "Free Ride City - Daytime";
                        this.presence.largeImageKey = "freeride";
                        this.presence.largeImageText = "Free Ride City - Daytime";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "freeridenoc":
                        this.presence.details = "Free Ride City - Night";
                        this.presence.largeImageKey = "freeride";
                        this.presence.largeImageText = "Free Ride City - Night";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "intro":
                        this.presence.details = "Intro";
                        this.presence.largeImageKey = "intermezzo";
                        this.presence.largeImageText = "Intro";
                        this.presence.smallImageKey = "mafia";
                        this.presence.state = string.Empty;
                        break;

                    case "intermezzo02":
                        this.presence.details = "Intermezzo-1";
                        this.presence.largeImageKey = "intermezzo";
                        this.presence.largeImageText = "Intermezzo-1";
                        this.presence.smallImageKey = "mafia";
                        this.presence.state = string.Empty;
                        break;

                    case "intermezzo03":
                        this.presence.details = "Intermezzo-2";
                        this.presence.largeImageKey = "intermezzo";
                        this.presence.largeImageText = "Intermezzo-2";
                        this.presence.smallImageKey = "mafia";
                        this.presence.state = string.Empty;
                        break;

                    case "intermezzo4":
                        this.presence.details = "Intermezzo-3";
                        this.presence.largeImageKey = "intermezzo";
                        this.presence.largeImageText = "Intermezzo-3";
                        this.presence.smallImageKey = "mafia";
                        this.presence.state = string.Empty;
                        break;

                    case "fmv intermezzo05":
                        this.presence.details = "Intermezzo-4";
                        this.presence.largeImageKey = "intermezzo";
                        this.presence.largeImageText = "Intermezzo-4";
                        this.presence.smallImageKey = "mafia";
                        this.presence.state = string.Empty;
                        break;

                    case "fmv konec":
                        this.presence.details = "Epilogue";
                        this.presence.largeImageKey = "intermezzo";
                        this.presence.largeImageText = "Epilogue";
                        this.presence.smallImageKey = "mafia";
                        this.presence.state = string.Empty;
                        break;

                    case "FMV KONEC":
                        this.presence.details = "Epilogue";
                        this.presence.largeImageKey = "intermezzo";
                        this.presence.largeImageText = "Epilogue";
                        this.presence.smallImageKey = "mafia";
                        this.presence.state = string.Empty;
                        break;

                    case "mise01":
                        this.presence.details = "An Offer You Can't Refuse";
                        this.presence.largeImageKey = "m01";
                        this.presence.largeImageText = "An Offer You Can't Refuse";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise02a-taxi":
                        this.presence.details = "The Running Man - Taxi Rank";
                        this.presence.largeImageKey = "m02a";
                        this.presence.largeImageText = "The Running Man - Taxi Rank";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise02-saliery":
                        this.presence.details = "The Running Man - Salieri Bar";
                        this.presence.largeImageKey = "m02";
                        this.presence.largeImageText = "The Running Man - Salieri Bar";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise02-ulicka":
                        this.presence.details = "The Running Man - Coffee Break";
                        this.presence.largeImageKey = "m02";
                        this.presence.largeImageText = "The Running Man - Coffee Break";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise03-morello":
                        this.presence.details = "Molotov Party - City";
                        this.presence.largeImageKey = "m03";
                        this.presence.largeImageText = "Molotov Party - City";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise03-saliery":
                        this.presence.details = "Molotov Party - Salieri Bar";
                        this.presence.largeImageKey = "m03";
                        this.presence.largeImageText = "Molotov Party - Salieri Bar";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise03-salierykonec":
                        this.presence.details = "Molotov Party - Return To Salieri's Bar";
                        this.presence.largeImageKey = "m03";
                        this.presence.largeImageText = "Return To Salieri's Bar";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise04-krajina":
                        this.presence.details = "Ordinary Routine - Get Him!";
                        this.presence.largeImageKey = "m04";
                        this.presence.largeImageText = "Ordinary Routine - Get Him!";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise04-mesto":
                        this.presence.details = "Ordinary Routine - After The Briefing";
                        this.presence.largeImageKey = "m04";
                        this.presence.largeImageText = "Ordinary Routine - After The Briefing";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise04-saliery":
                        this.presence.details = "Ordinary Routine - Salieri Bar";
                        this.presence.largeImageKey = "m04";
                        this.presence.largeImageText = "Ordinary Routine - Salieri Bar";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise04-motorest":
                        this.presence.details = "Ordinary Routine - Clark's Motel";
                        this.presence.largeImageKey = "m04";
                        this.presence.largeImageText = "Ordinary Routine - Clark's Motel";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise05-mesto":
                        this.presence.details = "Fairplay - City";
                        this.presence.largeImageKey = "m06";
                        this.presence.largeImageText = "Fairplay - City";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise05-saliery":
                        this.presence.details = "Fairplay - Salieri Bar";
                        this.presence.largeImageKey = "m06";
                        this.presence.largeImageText = "Fairplay - Salieri Bar";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise06-autodrom":
                        this.presence.details = "Fairplay - Race";
                        this.presence.largeImageKey = "m06";
                        this.presence.largeImageText = "Fairplay - Race";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise06-mesto":
                        this.presence.details = "Fairplay - City";
                        this.presence.largeImageKey = "m06";
                        this.presence.largeImageText = "Fairplay - City";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise06-oslava":
                        this.presence.details = "Fairplay - City";
                        this.presence.largeImageKey = "m06";
                        this.presence.largeImageText = "Fairplay - City";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise06-saliery":
                        this.presence.details = "Fairplay - Salieri's Bar";
                        this.presence.largeImageKey = "m06";
                        this.presence.largeImageText = "Fairplay - Salieri's Bar";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise07b-chuligani":
                        this.presence.details = "Better Get Used To It - City";
                        this.presence.largeImageKey = "m07b";
                        this.presence.largeImageText = "Better Get Used To It - City";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise07b-saliery":
                        this.presence.details = "Better Get Used To It - Salieri Bar";
                        this.presence.largeImageKey = "m07b";
                        this.presence.largeImageText = "Better Get Used To It - Salieri Bar";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise07-saliery":
                        this.presence.details = "Sarah - Salieri Bar";
                        this.presence.largeImageKey = "m07";
                        this.presence.largeImageText = "Sarah - Salieri Bar";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise07-sara":
                        this.presence.details = "Sarah - Moonlight Stroll";
                        this.presence.largeImageKey = "m07";
                        this.presence.largeImageText = "Sarah - Moonlight Stroll";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise08-hotel":
                        this.presence.details = "The Whore - Hotel Corleone";
                        this.presence.largeImageKey = "m08a";
                        this.presence.largeImageText = "The Whore - Hotel Corleone";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise08-kostel":
                        this.presence.details = "The Priest - Suprise";
                        this.presence.largeImageKey = "m08b";
                        this.presence.largeImageText = "The Priest - Suprise";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise08-mesto":
                        this.presence.details = "The Whore - City";
                        this.presence.largeImageKey = "m08a";
                        this.presence.largeImageText = "The Whore - City";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise09-krajina":
                        this.presence.details = "A Trip To The Country - Return Journey";
                        this.presence.largeImageKey = "m09";
                        this.presence.largeImageText = "A Trip To The Country - Return Journey";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise09-mesto":
                        this.presence.details = "A Trip To The Country - City";
                        this.presence.largeImageKey = "m09";
                        this.presence.largeImageText = "A Trip To The Country - City";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise09-prejimka":
                        this.presence.details = "A Trip To The Country - The Farm";
                        this.presence.largeImageKey = "m09";
                        this.presence.largeImageText = "A Trip To The Country - The Farm";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise09-saliery":
                        this.presence.details = "A Trip To The Country - Salieri Bar";
                        this.presence.largeImageKey = "m09";
                        this.presence.largeImageText = "A Trip To The Country - Salieri Bar";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise10-letiste":
                        this.presence.details = "Omerta - The Airport";
                        this.presence.largeImageKey = "m10";
                        this.presence.largeImageText = "Omerta - The Airport";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise10-mesto":
                        this.presence.details = "Omerta - City";
                        this.presence.largeImageKey = "m10";
                        this.presence.largeImageText = "Omerta - City";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise10-saliery":
                        this.presence.details = "Omerta - Salieri Bar";
                        this.presence.largeImageKey = "m10";
                        this.presence.largeImageText = "Omerta - Salieri Bar";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise11-mesto":
                        this.presence.details = "Visiting Rich People - City";
                        this.presence.largeImageKey = "m11";
                        this.presence.largeImageText = "Visiting Rich People - City";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise11-saliery":
                        this.presence.details = "Visiting Rich People - Salieri Bar";
                        this.presence.largeImageKey = "m11";
                        this.presence.largeImageText = "Visiting Rich People - Salieri Bar";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise11-vila":
                        this.presence.details = "Visiting Rich People - The Villa";
                        this.presence.largeImageKey = "m11";
                        this.presence.largeImageText = "Visiting Rich People - The Villa";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise12-garage":
                        this.presence.details = "A Great Deal! - The Parking Lot";
                        this.presence.largeImageKey = "m12";
                        this.presence.largeImageText = "A Great Deal! - The Parking Lot";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise12-mesto":
                        this.presence.details = "A Great Deal! - Car Chase";
                        this.presence.largeImageKey = "m12";
                        this.presence.largeImageText = "A Great Deal! - Car Chase";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise12-saliery":
                        this.presence.details = "A Great Deal! - Salieri Bar";
                        this.presence.largeImageKey = "m12";
                        this.presence.largeImageText = "A Great Deal! - Salieri Bar";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise13-mesto":
                        this.presence.details = "Bon Appétit! - City";
                        this.presence.largeImageKey = "m13";
                        this.presence.largeImageText = "Bon Appétit! - City";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise13-mesto2":
                        this.presence.details = "Bon Appétit! - After The Attack";
                        this.presence.largeImageKey = "m13";
                        this.presence.largeImageText = "Bon Appétit! - After The Attack";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise13-restaurace":
                        this.presence.details = "Bon Appétit! - Pepe's Restaurant";
                        this.presence.largeImageKey = "m13";
                        this.presence.largeImageText = "Bon Appétit! - Pepe's Restaurant";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise13-zradce":
                        this.presence.details = "Bon Appétit! - Carlo";
                        this.presence.largeImageKey = "m13";
                        this.presence.largeImageText = "Bon Appétit! - Carlo";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise14-mesto":
                        this.presence.details = "Happy Birthday! - City";
                        this.presence.largeImageKey = "m14";
                        this.presence.largeImageText = "Happy Birthday! - City";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise14-parnik":
                        this.presence.details = "Happy Birthday! - At The Party";
                        this.presence.largeImageKey = "m14";
                        this.presence.largeImageText = "Happy Birthday! - At The Party";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise14-saliery":
                        this.presence.details = "Happy Birthday! - Vincenzo's Workshop";
                        this.presence.largeImageKey = "m14";
                        this.presence.largeImageText = "Happy Birthday! - Vincenzo's Workshop";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise15-mesto":
                        this.presence.details = "You Lucky Bastard! - City";
                        this.presence.largeImageKey = "m15";
                        this.presence.largeImageText = "You Lucky Bastard! - City";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise15-pristav":
                        this.presence.details = "You Lucky Bastard! - The Harbor";
                        this.presence.largeImageKey = "m15";
                        this.presence.largeImageText = "You Lucky Bastard! - The Harbor";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise15-saliery":
                        this.presence.details = "You Lucky Bastard! - Salieri Bar";
                        this.presence.largeImageKey = "m15";
                        this.presence.largeImageText = "You Lucky Bastard! - Salieri Bar";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise16-krajina":
                        this.presence.details = "Creme De La Creme - Manhunt";
                        this.presence.largeImageKey = "m16";
                        this.presence.largeImageText = "Creme De La Creme - Manhunt";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise16-letiste":
                        this.presence.details = "Creme De La Creme - The Airport";
                        this.presence.largeImageKey = "m16";
                        this.presence.largeImageText = "Creme De La Creme - The Airport";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise16-mesto":
                        this.presence.details = "Creme De La Creme - City";
                        this.presence.largeImageKey = "m16";
                        this.presence.largeImageText = "Creme De La Creme - City";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise16-saliery":
                        this.presence.details = "Creme De La Creme - Salieri Bar";
                        this.presence.largeImageKey = "m16";
                        this.presence.largeImageText = "Creme De La Creme - Salieri Bar";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise17-mesto":
                        this.presence.details = "Election Campaign - City";
                        this.presence.largeImageKey = "m17";
                        this.presence.largeImageText = "Election Campaign - City";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise17-saliery":
                        this.presence.details = "Election Campaign - Salieri Bar";
                        this.presence.largeImageKey = "m17";
                        this.presence.largeImageText = "Election Campaign - Salieri Bar";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise17-vezeni":
                        this.presence.details = "Election Campaign - Old Prison";
                        this.presence.largeImageKey = "m17";
                        this.presence.largeImageText = "Election Campaign - Old Prison";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise18-mesto":
                        this.presence.details = "Just For Relaxation - City";
                        this.presence.largeImageKey = "m18";
                        this.presence.largeImageText = "Just For Relaxation - City";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise18-pristav":
                        this.presence.details = "Just For Relaxation - The Harbor";
                        this.presence.largeImageKey = "m18";
                        this.presence.largeImageText = "Just For Relaxation - The Harbor";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise18-saliery":
                        this.presence.details = "Just For Relaxation - Salieri Bar";
                        this.presence.largeImageKey = "m18";
                        this.presence.largeImageText = "Just For Relaxation - Salieri Bar";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise19-banka":
                        this.presence.details = "Moonlightning - The Bank";
                        this.presence.largeImageKey = "m19";
                        this.presence.largeImageText = "Moonlightning - The Bank";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise19-mesto":
                        this.presence.details = "Moonlightning - City";
                        this.presence.largeImageKey = "m19";
                        this.presence.largeImageText = "Moonlightning - City";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise19-pauli":
                        this.presence.details = "Moonlightning - Paulie's Flat";
                        this.presence.largeImageKey = "m19";
                        this.presence.largeImageText = "Moonlightning - Paulie's Flat";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise20-galery":
                        this.presence.details = "The Death Of Art - Grand Finale";
                        this.presence.largeImageKey = "m20";
                        this.presence.largeImageText = "The Death Of Art - Grand Finale";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise20-mesto":
                        this.presence.details = "The Death Of Art - City";
                        this.presence.largeImageKey = "m20";
                        this.presence.largeImageText = "The Death Of Art - City";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "mise20-pauli":
                        this.presence.details = "The Death Of Art - Paulie's Flat";
                        this.presence.largeImageKey = "m20";
                        this.presence.largeImageText = "The Death Of Art - Paulie's Flat";
                        this.presence.smallImageKey = "mafia";
                        break;

                    case "tutorial":
                        this.presence.details = "Tutorial";
                        this.presence.largeImageKey = "tutorial";
                        this.presence.largeImageText = "Tutorial";
                        this.presence.smallImageKey = "mafia";
                        break;
                }

                //CARCYCLOPEDIA CARS

                    if (mission == "carcyclopedia" || mission == "autosalon")
                    {
                        switch (CarcyclopediaID)
                        {
                        case 0:
                            this.presence.state = "Lookin' at Bolt Ace Tudor";
                            break;

                        case 1:
                            this.presence.state = "Lookin' at Bolt Ace Touring";
                            break;

                        case 2:
                            this.presence.state = "Lookin' at Bolt Ace Runabout";
                            break;

                        case 3:
                            this.presence.state = "Lookin' at Bolt Ace Pickup";
                            break;

                        case 4:
                            this.presence.state = "Lookin' at Bolt Ace Fordor";
                            break;

                        case 5:
                            this.presence.state = "Lookin' at Bolt Ace Coupe";
                            break;

                        case 6:
                            this.presence.state = "Lookin' at Bolt Model B Tudor";
                            break;

                        case 7:
                            this.presence.state = "Lookin' at Bolt Model B Roadster";
                            break;

                        case 8:
                            this.presence.state = "Lookin' at Bolt Model B Pickup";
                            break;

                        case 9:
                            this.presence.state = "Lookin' at Bolt Model B Fordor";
                            break;

                        case 10:
                            this.presence.state = "Lookin' at Bolt Model B Deliviry";
                            break;

                        case 11:
                            this.presence.state = "Lookin' at Bolt Model B Coupe";
                            break;

                        case 12:
                            this.presence.state = "Lookin' at Bolt Model B Cabriolet";
                            break;

                        case 13:
                            this.presence.state = "Lookin' at Schubert Six";
                            break;

                        case 14:
                            this.presence.state = "Lookin' at Bolt V8 Coupe";
                            break;

                        case 15:
                            this.presence.state = "Lookin' at Bolt V8 Fordor";
                            break;

                        case 16:
                            this.presence.state = "Lookin' at Bolt V8 Roadster";
                            break;

                        case 17:
                            this.presence.state = "Lookin' at Bolt V8 Touring";
                            break;

                        case 18:
                            this.presence.state = "Lookin' at Bolt V8 Tudor";
                            break;

                        case 19:
                            this.presence.state = "Lookin' at Schubert Extra Six Fordor";
                            break;

                        case 20:
                            this.presence.state = "Lookin' at Schubert Extra Six Tudor";
                            break;

                        case 21:
                            this.presence.state = "Lookin' at Falconer";
                            break;

                        case 22:
                            this.presence.state = "Lookin' at Falconer Yellowcar";
                            break;

                        case 23:
                            this.presence.state = "Lookin' at Crusader Chromium Fordor";
                            break;

                        case 24:
                            this.presence.state = "Lookin' at Crusader Chromium Tudor";
                            break;

                        case 25:
                            this.presence.state = "Lookin' at Guardian Terraplane Coupe";
                            break;

                        case 26:
                            this.presence.state = "Lookin' at Guardian Terraplane Fordor";
                            break;

                        case 27:
                            this.presence.state = "Lookin' at Guardian Terraplane Tudor";
                            break;

                        case 28:
                            this.presence.state = "Lookin' at Thor 812 Cabriolet FWD";
                            break;

                        case 29:
                            this.presence.state = "Lookin' at Thor 812 Phaeton FWD";
                            break;

                        case 30:
                            this.presence.state = "Lookin' at Thor 810 Sedan FWD";
                            break;

                        case 31:
                            this.presence.state = "Lookin' at Wright Coupe";
                            break;

                        case 32:
                            this.presence.state = "Lookin' at Wright Fordor";
                            break;

                        case 33:
                            this.presence.state = "Lookin' at Bruno Speedster 851";
                            break;

                        case 34:
                            this.presence.state = "Lookin' at Celeste Marque 500";
                            break;

                        case 35:
                            this.presence.state = "Lookin' at Lassiter V16 Fordor";
                            break;

                        case 36:
                            this.presence.state = "Lookin' at Lassiter V16 Phaeton";
                            break;

                        case 37:
                            this.presence.state = "Lookin' at Lassiter V16 Roadster";
                            break;

                        case 38:
                            this.presence.state = "Lookin' at Silver Fletcher";
                            break;

                        case 39:
                            this.presence.state = "Lookin' at Lassiter V16 Appolyon";
                            break;

                        case 40:
                            this.presence.state = "Lookin' at Trautenberg Model J";
                            break;

                        case 41:
                            this.presence.state = "Lookin' at Carrozella C-Otto 4WD";
                            break;

                        case 42:
                            this.presence.state = "Lookin' at Brubaker 4WD";
                            break;

                        case 43:
                            this.presence.state = "Lookin' at Trautenberg Racer 4WD";
                            break;

                        case 44:
                            this.presence.state = "Lookin' at Caesar 8C Monstro";
                            break;

                        case 45:
                            this.presence.state = "Lookin' at Bolt Ambulance";
                            break;

                        case 46:
                            this.presence.state = "Lookin' at Bolt Firetruck";
                            break;

                        case 47:
                            this.presence.state = "Lookin' at Bolt Hearse";
                            break;

                        case 48:
                            this.presence.state = "Lookin' at Lassiter V16 Charon";
                            break;

                        case 49:
                            this.presence.state = "Lookin' at Ulver Airstream Fordor";
                            break;

                        case 50:
                            this.presence.state = "Lookin' at Ulver Airstream Tudor";
                            break;

                        case 51:
                            this.presence.state = "Lookin' at Lassiter V16 Police";
                            break;

                        case 52:
                            this.presence.state = "Lookin' at Schubert Six Police";
                            break;

                        case 53:
                            this.presence.state = "Lookin' at Schubert Extra 6 Police Fordor";
                            break;

                        case 54:
                            this.presence.state = "Lookin' at Schubert Extra 6 Police Tudor";
                            break;

                        case 55:
                            this.presence.state = "Lookin' at Caesar 8C 2300 Racing";
                            break;

                        default:
                            this.presence.state = "Lookin' at unknown car, huh";
                            break;
                        }
                    }

                //-----------------------------------------------------------------------------------

                if (hp == 0)
                {
                    this.presence.state = "Dead";
                }

                this.presence.smallImageText = "Mafia " + MAFIA_VERSION;
                label1.Text = "Mafia process found! Your game version is " + MAFIA_VERSION;
                DiscordRpc.UpdatePresence(ref this.presence);
            }

            else
            {
                timer1.Stop();
                label1.Text = "Mafia process not found!";
                button1.Text = "Activate";
                DiscordRpc.Shutdown();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button();
        }

        private void Button()
        {
            Process[] proc = Process.GetProcessesByName("Game");
            if (proc.Length != 0)
            {
                button1.Text = "Refresh";
                time = DateTimeOffset.Now.ToUnixTimeSeconds();
                timer1.Interval = 150;
                timer1.Tick += new EventHandler(AfterTimer);
                timer1.Start();
            }

            else
            {
                label1.Text = "Mafia process not found!";
                button1.Text = "Activate";
                timer1.Stop();
                DiscordRpc.Shutdown();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiscordRpc.Shutdown();
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Powered by Smelson and Legion (C) 2020! From Russia With Love", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}