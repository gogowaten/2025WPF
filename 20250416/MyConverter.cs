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

namespace _20250416
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

    public class MyConvRenderTransform2 : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var x = (double)values[0] * (double)values[5];
            var y = (double)values[1] * (double)values[6];
            var scaleX = (double)values[2];
            var scaleY = (double)values[3];
            var angle = (double)values[4];

            TransformGroup transformGroup = new();
            transformGroup.Children.Add(new ScaleTransform(scaleX, scaleY, x, y));
            transformGroup.Children.Add(new RotateTransform(angle, x, y));
            return transformGroup;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //public class MyConvRenderTransform : IMultiValueConverter
    //{
    //    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        var width = ((double)values[0]) / 2.0;
    //        var height = ((double)values[1]) / 2.0;
    //        var scaleX = (double)values[2];
    //        var scaleY = (double)values[3];
    //        ScaleTransform scale = new(scaleX, scaleY, width, height);
    //        var angle = (double)values[4];
    //        RotateTransform rotate = new(angle, width, height);
    //        TransformGroup transformGroup = new TransformGroup();
    //        transformGroup.Children.Add(scale);
    //        transformGroup.Children.Add(rotate);
    //        return transformGroup;
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

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