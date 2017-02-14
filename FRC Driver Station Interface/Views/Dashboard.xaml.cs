using System.Windows.Controls;

namespace Frc1360.DriverStation.Views
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
            list.ItemsSource = Components.ComponentControllers;
        }
    }
}
