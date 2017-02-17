using System.Windows.Controls;

namespace Frc1360.DriverStation.Components.ConfigSelector
{
    /// <summary>
    /// Interaction logic for Selector.xaml
    /// </summary>
    public partial class Selector : UserControl
    {
        private ConfigController controller;

        public Selector(ConfigController controller)
        {
            this.controller = controller;
            InitializeComponent();
            selector.ItemsSource = controller.Options;
            selector.SelectedIndex = controller.Index;
        }

        private void Changed(object sender, SelectionChangedEventArgs e)
        {
            controller.Update(selector.SelectedIndex);
        }
    }
}
