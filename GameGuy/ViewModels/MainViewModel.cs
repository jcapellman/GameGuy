using System;
using System.Drawing;
using System.Reflection;

using GameGUY.Common;
using GameGUY.ViewModels.Base;

namespace GameGUY.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
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

            AppWindowTitle = Constants.APP_NAME;

            AddOutput($"{Constants.APP_NAME} - Version {Assembly.GetExecutingAssembly().GetName().Version} - {Constants.COPYRIGHT_STR}");
        }

        private void runRom(object sender, EventArgs e)
        {
            SetOutputWindow(string.Empty);

            currentApp.reset();

            AppWindowTitle = $"{Constants.APP_NAME} - {currentApp.getProgramName()}";

            _romRunning = true;

            UpdateDisplay(currentApp.render());
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

        public void Restart()
        {

        }
    }
}