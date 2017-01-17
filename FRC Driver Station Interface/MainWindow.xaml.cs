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

namespace FRC_Driver_Station_Interface
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
    }
}
