using System.Windows;
using System.Diagnostics;

namespace Frc1360.DriverStation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void minimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void maximize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void restore(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
        }

        private void close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ComponentsFolder(object sender, RoutedEventArgs e)
        {
            Process.Start(App.ComponentsDirectory);
        }
    }
}
