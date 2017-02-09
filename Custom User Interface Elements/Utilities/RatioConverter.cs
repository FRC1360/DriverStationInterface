using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Frc1360.DriverStation.CustomControls.Utilities
{
    public class RatioConverter : IMultiValueConverter
    {
        public double Scalar { get; set; } = 1;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => values.Cast<object>().Select(System.Convert.ToDouble).Aggregate((x, y) => x / y) * Scalar;

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
