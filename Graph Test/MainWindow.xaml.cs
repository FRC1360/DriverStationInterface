using System.Windows;

namespace Graph_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            chart.DataSource = new float[] { 0.0f, 1.0f, 0.5f };
        }
    }
}
