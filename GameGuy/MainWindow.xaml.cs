using System.Windows;

using GameGUY.ViewModels;

namespace GameGUY
{
    public partial class MainWindow : Window
    {
        private MainViewModel ViewModel => (MainViewModel)DataContext;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();

            ViewModel.Initialize();

            KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            ViewModel.HandleInput(e.Key.ToString());            
        }

        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void txtBxOutput_TextChanged(object sender, System.Windows.Input.KeyEventArgs e)
        {
            ViewModel.HandleInput(e.Key.ToString());
        }

        private void mnuRestart_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Restart();
        }
    }
}