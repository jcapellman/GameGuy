using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using GameGUY.ViewModels.Base;

namespace GameGUY.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private SortedDictionary<string, GameGUY.GGInterpreter> appDICT =
            new SortedDictionary<string, GameGUY.GGInterpreter>();

        private MainMenu menu = new MainMenu();

        private static string desKey = "45^^!209";
        private GGInterpreter currentApp;

        private string _appWindowTitle;

        public string AppWindowTitle
        {
            get => _appWindowTitle;

            set
            {
                _appWindowTitle = value;
                OnPropertyChanged();
            }
        }

        private bool _romRunning = false;

        private string _outputWindowText = string.Empty;

        public string OutputWindowText
        {
            get => _outputWindowText;

            set
            {
                _outputWindowText = value;
                OnPropertyChanged();
            }
        }

        private ContentAlignment _outputWindowAlignment;

        public ContentAlignment OutputWindowAlignment
        {
            get => _outputWindowAlignment;

            set
            {
                _outputWindowAlignment = value;
                OnPropertyChanged();
            }
        }

        private void AddOutput(string strOutput)
        {
            OutputWindowText += $"{strOutput}{System.Environment.NewLine}";
        }

        private void SetOutputWindow(string strOutput, ContentAlignment contentAlignment = ContentAlignment.TopLeft)
        {
            OutputWindowText = strOutput;
            OutputWindowAlignment = contentAlignment;
        }

        public void Initialize()
        {
            SetOutputWindow(string.Empty);

            AppWindowTitle = "gameGUY";

            AddOutput("gameGUY - Version 1.0 - (C)1998-2019 Jarred Capellman\nSelect a game from the menu to play");

            ReadRoms();

            InitMenu();
        }

        private void InitMenu()
        {
            var systemMnu = menu.MenuItems.Add("System");
            var startMnu = systemMnu.MenuItems.Add("Games");

            foreach (string romName in appDICT.Keys)
            {
                startMnu.MenuItems.Add(romName, runRom);
            }

           //systemMnu.MenuItems.Add("Exit", exit_Click);
        }

        private void ReadRoms()
        {
            var path = Application.StartupPath + "\\ROMS";

            if (!Directory.Exists(path))
            {
                AddOutput($"{path} does not exist, no games will be loaded...");

                return;
            }

            var dInfo = new DirectoryInfo(path);

            var files = dInfo.GetFiles();

            if (!files.Any())
            {
                AddOutput($"{path} contains no games...");

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

            AddOutput($"Loaded {appDICT.Count}...");
        }

        private void runRom(object sender, EventArgs e)
        {
            SetOutputWindow(string.Empty);

            if (appDICT.ContainsKey(sender.ToString().Substring(53)))
            {
                currentApp = appDICT[sender.ToString().Substring(53)];

                currentApp.reset();

                AppWindowTitle = "gameGuy - " + currentApp.getProgramName();

                _romRunning = true;

                UpdateDisplay(currentApp.render());
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }

        private void UpdateDisplay(string toRenderStr)
        {
            if (!currentApp.getGameState())
            {
                SetOutputWindow(toRenderStr);                
            }
            else
            {
                SetOutputWindow("GAME OVER", ContentAlignment.MiddleCenter);

                _romRunning = false;
            }
        }

        public void HandleInput(string input)
        {
            if (!_romRunning)
            {
                return;
            }

            currentApp.sendInput(input);
            SetOutputWindow(currentApp.render());
        }
    }
}