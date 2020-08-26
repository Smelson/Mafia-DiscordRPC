using MafiaRPC.DiscordRpcDemo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MafiaRPC
{
    public partial class Form1 : Form
    {
        private DiscordRpc.EventHandlers handlers;
        private DiscordRpc.RichPresence presence;
        private long time = DateTimeOffset.Now.ToUnixTimeSeconds();

        public Form1()
        {
            InitializeComponent();
        }

        int pID;
        int address = 0x0065115C;
        byte[] offsets = new byte[] { 0x68, 0x0 };

        private void AfterTimer(object sender, EventArgs e)
        {
            Process[] proc = Process.GetProcessesByName("Game");

            if (proc.Length != 0)
            {

                VAMemory memory = new VAMemory("Game");

                this.handlers = default(DiscordRpc.EventHandlers);
                DiscordRpc.Initialize("747679968389234783", ref this.handlers, true, null);
                this.handlers = default(DiscordRpc.EventHandlers);
                DiscordRpc.Initialize("747679968389234783", ref this.handlers, true, null);
                this.presence.details = "The Punks Mod Development";
                this.presence.largeImageKey = "mafia";
                this.presence.largeImageText = "Mafia";
                int hp = memory.ReadInt32((IntPtr)0x00661538);
                int bullets = memory.ReadInt32((IntPtr)0x0066153C);
                this.presence.state = hp.ToString() + " HP" + ", " + bullets.ToString() + " ammo";
                this.presence.startTimestamp = time;
                this.presence.endTimestamp = 0;
                if (hp == 0)
                {
                    this.presence.state = "Dead";
                }

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
                label1.Text = "Mafia process found!";
                button1.Text = "Refresh";
                time = DateTimeOffset.Now.ToUnixTimeSeconds();
                timer1.Interval = 1;
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
    }
}
