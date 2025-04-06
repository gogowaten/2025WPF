using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Provider;
using System.Windows.Data;
using System.Windows.Media;

namespace _20250405
{
    public class MyConvInsideElementLeft : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bounds = (Rect)value;
            return -bounds.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MyConvInsideElementTop : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bounds = (Rect)value;
            return -bounds.Top;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConvActualLeft : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var left = (double)values[0];
            var bounds = (Rect)values[1];
            return left + bounds.Left;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConvActualTop : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var top = (double)values[0];
            var bounds = (Rect)values[1];
            return top + bounds.Top;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConvRenderTransform : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var origin = (double)values[0];
            var width = (double)values[1];
            var height = (double)values[2];
            var scaleX = (double)values[3];
            var scaleY = (double)values[4];
            var angle = (double)values[5];

            var centerX = width * origin;
            var centerY = height * origin;

            TransformGroup transformGroup = new();
            transformGroup.Children.Add(new RotateTransform(angle, centerX, centerY));
            transformGroup.Children.Add(new ScaleTransform(scaleX, scaleY, centerX, centerY));
            return transformGroup;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConvRenderTransform2 : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var origin = (double)values[0];
            var bounds = (Rect)values[1];
            var scaleX = (double)values[3];
            var scaleY = (double)values[4];
            var angle = (double)values[5];

            var centerX = bounds.Width * origin;
            var centerY = bounds.Height * origin;

            TransformGroup transformGroup = new();
            transformGroup.Children.Add(new RotateTransform(angle, centerX, centerY));
            transformGroup.Children.Add(new ScaleTransform(scaleX, scaleY, centerX, centerY));
            return transformGroup;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class MyConvRenderTransormBoundsForGeoShape : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            var geo = (Geometry)values[0];
            var angle = (double)values[1];
            var scaleX = (double)values[2];
            var scaleY = (double)values[3];
            var origin = (double)values[4];
            var pen = (Pen)values[5];
            var bounds = (Rect)values[6];
            var centerX = bounds.Width * origin;
            var centerY = bounds.Height * origin;
            //var centerX = (bounds.Left + bounds.Width) * origin;
            //var centerY = (bounds.Top + bounds.Height) * origin;

            TransformGroup transformGroup = new();
            transformGroup.Children.Add(new RotateTransform(angle, centerX, centerY));
            transformGroup.Children.Add(new ScaleTransform(scaleX, scaleY));
            geo.Transform = transformGroup;
            var result = geo.GetRenderBounds(pen);
            result.Offset(-bounds.Left, -bounds.Top);
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConvRenderTransormBounds : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var RTF = (Transform)values[0];
            var actW = (double)values[1];
            var actH = (double)values[2];


            return RTF.TransformBounds(new Rect(0, 0, actW, actH));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}