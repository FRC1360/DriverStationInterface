using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Frc1360.DriverStation.CustomControls
{
    public sealed class LineGraph : Control
    {
        public static readonly DependencyProperty DataSourceProperty = DependencyProperty.Register("DataSource", typeof(IEnumerable), typeof(LineGraph));
        private static readonly DependencyPropertyKey GeometryPropertyKey = DependencyProperty.RegisterReadOnly("Geometry", typeof(Geometry), typeof(LineGraph), new PropertyMetadata());
        public static readonly DependencyProperty GeometryProperty = GeometryPropertyKey.DependencyProperty;

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
            var max = src.Length == 0 ? 0 : src.Max();
            Geometry = src.Length > 0 ? new PathGeometry(new[] { new PathFigure(new Point(0, src[0] * (src.Length - 1) / max), src.Skip(1).Select((y, x) => new LineSegment(new Point(x + 1, y * (src.Length - 1) / max), true)), false) }) : new PathGeometry();
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
    }
}
