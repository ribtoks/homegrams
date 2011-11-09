using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace HierarchyInterface
{
    class DoubleToThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double d = (double)value;
            return new Thickness(0.0, d, 0.0, d);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Thickness th = (Thickness)value;
            return th.Top;
        }
    }
}
