using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using DynamicDataDisplay.Markers.DataSources;

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
            //chart.DataSource = new DataSource(new float[] { 0.0f, 1.0f, 0.5f });
            //chart.DataSource = new PointArrayDataSource(new Point[] { new Point(0.0, 1.0), new Point(1.0, 0.0) });
        }

        private class DataSource : PointDataSourceBase
        {
            IEnumerable<float> data;

            public DataSource(IEnumerable<float> data)
            {
                this.data = data;
                if (data is INotifyCollectionChanged ncc)
                    ncc.CollectionChanged += OnCollectionChanged;
            }

            protected override IEnumerable GetDataCore(DataSourceEnvironment environment) => data.Select((y, x) => new Point(x, y));
        }
    }
}
