using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using Frc1360.DriverStation.RobotComm;

namespace Frc1360.DriverStation.Views
{
    /// <summary>
    /// Interaction logic for Connecting.xaml
    /// </summary>
    public partial class Connecting : Page
    {
        public Connecting()
        {
            InitializeComponent();
        }

        private void loaded(object sender, RoutedEventArgs e) => Task.Run(async () =>
            {
            restart:
                try
                {
                    App.Connection = new Connection(new IPEndPoint(new IPAddress(new byte[] { 10, 13, 60, 2 }), 5800));
                    foreach (var c in Components.ComponentControllers)
                        await c.Value.InitializeAsync(App.Connection);
                }
                catch (Exception ex)
                {
                    if (Dispatcher.Invoke(() =>
                    {
                        App.SetStatus(this, "Failed to connect");
                        App.SetProgress(this, null);
                        return MessageBox.Show(Application.Current.MainWindow, "An error occured while while connecting to the robot; would you like to try again?\n\n" + ex.ToString(), "An error occured", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes;
                    }))
                        goto restart;
                    Dispatcher.Invoke(Application.Current.Shutdown);
                }
                Dispatcher.Invoke(() => NavigationService.Navigate(new Uri("/Views/Dashboard.xaml", UriKind.Relative)));
            });
    }
}
