using System.Windows.Controls;

namespace Frc1360.DriverStation.Components.AutonomousSelector
{
    /// <summary>
    /// Interaction logic for Selector.xaml
    /// </summary>
    public partial class Selector : UserControl
    {
        public Selector(AutonController controller)
        {
            InitializeComponent();
            list.ItemsSource = controller.AutonomousSelection;
        }
    }
}
