using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GameGUY
{
    public partial class frmMain : Form
    {
        private bool appRunning = false;

        private SortedDictionary<string, GameGUY.GGInterpreter> appDICT =
            new SortedDictionary<string, GameGUY.GGInterpreter>();

        private MainMenu mnu = new MainMenu();
        private static string desKey = "45^^!209";
        private GGInterpreter currentApp;
        private string output = "";

        public frmMain()
        {
            InitializeComponent();

            lblOutput.Text =
                "gameGUY - Version 1.0 - (C)1998-2019 Jarred Capellman\nSelect a game from the menu to play";

            ReadRoms();
            InitMenu();

            this.KeyPress += new KeyPressEventHandler(frmMain_KeyPress);
        }

        private void InitMenu()
        {
            MenuItem systemMnu = mnu.MenuItems.Add("System");
            MenuItem startMnu = systemMnu.MenuItems.Add("Games");

            foreach (string romName in appDICT.Keys)
            {
                startMnu.MenuItems.Add(romName, runRom);
            }

            systemMnu.MenuItems.Add("Exit", exit_Click);

            this.Menu = mnu;
        }

        private void ReadRoms()
        {
            var path = Application.StartupPath + "\\ROMS";

            if (!Directory.Exists(path))
            {
                MessageBox.Show($"{path} does not exist, no games will be loaded", Application.ProductName);

                return;
            }

            var dInfo = new DirectoryInfo(path);

            var files = dInfo.GetFiles();

            if (!files.Any())
            {
                MessageBox.Show($"{path} contains no games", Application.ProductName);

                return;
            }

            foreach (var file in files)
            {
                if (file.Extension.ToUpper() != ".GG")
                {
                    continue;
                }

                var tmpApp = new GGInterpreter(file.FullName, desKey);

                appDICT.Add(tmpApp.getProgramName(), tmpApp);
            }
        }

        private void runRom(object sender, EventArgs e)
        {
            output = "";

            if (appDICT.ContainsKey(sender.ToString().Substring(53)))
            {
                currentApp = appDICT[sender.ToString().Substring(53)];

                currentApp.reset();

                Text = "gameGuy - " + currentApp.getProgramName();

                lblOutput.TextAlign = ContentAlignment.TopLeft;
                this.lblOutput.Enabled = true;
                this.appRunning = true;

                updateDisplay(currentApp.render());
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }

        void updateDisplay(string toRenderStr)
        {
            if (!currentApp.getGameState())
            {
                this.lblOutput.Text = toRenderStr;
            }
            else
            {
                this.lblOutput.Text = "GAME OVER";
                this.lblOutput.TextAlign = ContentAlignment.MiddleCenter;

                this.appRunning = false;
            }
        }

        void frmMain_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.appRunning)
            {
                currentApp.sendInput(e.KeyChar.ToString());
                updateDisplay(currentApp.render());
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtBxOutput_TextChanged(object sender, EventArgs e)
        {
            if (this.appRunning)
            {
                currentApp.sendInput(e.ToString());
            }
        }
    }
}