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
        }

        private void Changed(object sender, SelectionChangedEventArgs e)
        {
            controller.Update(selector.SelectedIndex);
        }
    }
}
