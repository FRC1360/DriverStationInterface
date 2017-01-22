﻿using System;
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
