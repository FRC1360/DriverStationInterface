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

namespace Frc1360.DriverStation.CustomControls
{
    public sealed class LineGraph : Control
    {
        public static readonly DependencyProperty DataSourceProperty = DependencyProperty.Register("DataSource", typeof(IEnumerable), typeof(LineGraph));
        private static readonly DependencyPropertyKey GeometryPropertyKey = DependencyProperty.RegisterReadOnly("Geometry", typeof(Geometry), typeof(LineGraph), new PropertyMetadata());
        public static readonly DependencyProperty GeometryProperty = GeometryPropertyKey.DependencyProperty;

        public LineGraph()
        {
            
        }

        static LineGraph()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LineGraph), new FrameworkPropertyMetadata(typeof(LineGraph)));
        }

        public IEnumerable DataSource
        {
            get => GetValue(DataSourceProperty) as IEnumerable;
            set => SetValue(DataSourceProperty, value);
        }

        public Geometry Geometry
        {
            get => GetValue(GeometryProperty) as Geometry;
            private set => SetValue(GeometryPropertyKey, value);
        }

        private IEnumerable<double> Enumerate()
        {
            foreach (var x in DataSource)
            {
                double r;
                try
                {
                    r = Convert.ToDouble(x);
                }
                catch
                {
                    continue;
                }
                yield return r;
            }
        }

        private void Redraw()
        {
            var src = Enumerate().ToArray();
            var max = src.Max();
            Geometry = src.Length > 0 ? new PathGeometry(new[] { new PathFigure(new Point(0, src[0]), src.Skip(1).Select((y, x) => new LineSegment(new Point(x + 1, y * (src.Length - 1) / max), true)), false) }) : new PathGeometry();
            InvalidateMeasure();
        }

        private void DataSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => Redraw();

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == DataSourceProperty)
            {
                if (e.OldValue is INotifyCollectionChanged ncc_old)
                    ncc_old.CollectionChanged -= DataSourceCollectionChanged;
                if (e.NewValue is INotifyCollectionChanged ncc_new)
                    ncc_new.CollectionChanged += DataSourceCollectionChanged;
                Redraw();
            }
        }
        /*
        protected override Size MeasureOverride(Size constraint)
        {
            constraint = base.MeasureOverride(constraint);
            double value = Math.Min(constraint.Width, constraint.Height);
            return new Size(value, value);
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            arrangeBounds = base.ArrangeOverride(arrangeBounds);
            double value = Math.Min(arrangeBounds.Width, arrangeBounds.Height);
            return new Size(value, value);
        }*/
    }
}
