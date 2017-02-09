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
        public static DependencyProperty DataSourceProperty = DependencyProperty.Register("DataSource", typeof(IEnumerable), typeof(LineGraph));
        public static DependencyPropertyKey GeometryPropertyKey = DependencyProperty.RegisterReadOnly("Geometry", typeof(Geometry), typeof(LineGraph), new PropertyMetadata());
        public static DependencyProperty GeometryProperty = GeometryPropertyKey.DependencyProperty;

        public LineGraph()
        {
            this.ApplyTemplate();
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
            var src = Enumerate();
            Geometry = src.GetEnumerator().MoveNext() ? new PathGeometry(new[] { new PathFigure(new Point(0, src.First()), src.Select((y, x) => new LineSegment(new Point(x + 1, y), true)), false) }, FillRule.EvenOdd, new ScaleTransform(1.0 / Math.Min(1, src.Count() - 1), src.Max(), 0, 0)) : new PathGeometry();
            InvalidateMeasure();
        }

        private void DataSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => Redraw();

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == DataSourceProperty)
            {
                if (e.OldValue is INotifyCollectionChanged ncc_old)
                    ncc_old.CollectionChanged -= DataSourceCollectionChanged;
                if (e.NewValue is INotifyCollectionChanged ncc_new)
                    ncc_new.CollectionChanged += DataSourceCollectionChanged;
                Redraw();
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            double value = Math.Min(constraint.Width, constraint.Height);
            return new Size(value, value);
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            double value = Math.Min(arrangeBounds.Width, arrangeBounds.Height);
            return new Size(value, value);
        }
    }
}
