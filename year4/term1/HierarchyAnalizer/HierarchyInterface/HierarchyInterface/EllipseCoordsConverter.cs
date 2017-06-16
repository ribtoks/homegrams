using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Shapes;
using System.Windows;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Controls;

namespace HierarchyInterface
{
    class EllipseCoordsXConverter : IMultiValueConverter
    {
        #region Ivalueconverter members
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            object[] arr = (object[])parameter;

            var ellipse = arr[0] as Ellipse;
            var wrapperGrid = arr[1] as Grid;
            // calculate ellipse center point
            var point = ellipse.TransformToAncestor(wrapperGrid).Transform(new Point(0, 0));
            return point.X + (ellipse.ActualWidth / 2.0);
        }

        public object[] ConvertBack(object values, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
        #endregion
    }

    class EllipseCoordsYConverter : IValueConverter
    {
        #region Ivalueconverter members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object[] arr = (object[])parameter;

            var ellipse = arr[0] as Ellipse;
            var wrapperGrid = arr[1] as Grid;
            // calculate ellipse center point
            var point = ellipse.TransformToAncestor(wrapperGrid).Transform(new Point(0, 0));
            return point.Y + (ellipse.ActualHeight / 2.0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
        #endregion
    }
}
