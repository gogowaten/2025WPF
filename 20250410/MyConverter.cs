﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Provider;
using System.Windows.Data;
using System.Windows.Media;

namespace _20250410
{

    public class MyConvRectLeft : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bounds = (Rect)value;
            return bounds.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MyConvRectTop : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bounds = (Rect)value;
            return bounds.Top;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
    public class MyConvRectLeftReverse : IValueConverter
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
    public class MyConvRectTopReverse : IValueConverter
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
            return left - bounds.Left;
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
            return top - bounds.Top;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConvPoint : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var x = (double)values[0];
            var y = (double)values[1];
            return new Point(x, y);
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
            var angle = (double)values[0];
            var scaleX = (double)values[1];
            var scaleY = (double)values[2];
            ScaleTransform scale = new(scaleX, scaleY);
            RotateTransform rotate = new(angle);
            TransformGroup transformGroup = new();
            transformGroup.Children.Add(scale);
            transformGroup.Children.Add(rotate);
            return transformGroup;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConvPanelRenderTransform : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var angle = (double)values[0];
            var scaleX = (double)values[1];
            var scaleY = (double)values[2];
            var cx = (double)values[3];
            var cy = (double)values[4];
            var width = (double)values[5];
            var height = (double)values[6];
            width *= cx;
            height *= cy;
            TransformGroup transform = new();
            transform.Children.Add(new ScaleTransform(scaleX, scaleY, width, height));
            transform.Children.Add(new RotateTransform(angle, width, height));
            return transform;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConvPanelRenderTransformGeo : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var angle = (double)values[0];
            var scaleX = (double)values[1];
            var scaleY = (double)values[2];
            var cx = (double)values[3];
            var cy = (double)values[4];
            var bounds = (Rect)values[5];
            double x = bounds.Left + bounds.Width * cx;
            double y = bounds.Top + bounds.Height * cy;
            TransformGroup transform = new();
            transform.Children.Add(new ScaleTransform(scaleX, scaleY, x, y));
            transform.Children.Add(new RotateTransform(angle, x, y));
            return transform;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #region Bounds

    public class MyConvInsideTransformedBound11 : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var bounds = (Rect)values[0];
            var tf = (Transform)values[1];
            return tf.TransformBounds(bounds);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //GeometryをTransformしてPenを使ったGetRenderBounds
    public class MyConvInsideTransformedBounds2 : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var geo = (Geometry)values[0];
            var clone = geo.Clone();
            var tf = (Transform)values[1];
            var myPen = (Pen)values[2];
            clone.Transform = tf;
            return clone.GetRenderBounds(myPen);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConvInsideTransformedBounds : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double angle = (double)values[0];
            double scaleX = (double)values[1];
            double scaleY = (double)values[2];
            double x = (double)values[3];
            double y = (double)values[4];
            double width = (double)values[5];
            double height = (double)values[6];
            x *= width;
            y *= height;

            //TransformGroup transformGroup = new();
            //transformGroup.Children.Add(new ScaleTransform(scaleX, scaleY, x, y));
            //transformGroup.Children.Add(new RotateTransform(angle, x, y));

            //Rect result = transformGroup.TransformBounds(new Rect(0, 0, width, height));
            //return result;

            ScaleTransform sc = new(scaleX, scaleY, x, y);
            RotateTransform ro = new(angle, x, y);
            Rect rr = new(0, 0, width, height);
            rr = sc.TransformBounds(rr);
            rr = ro.TransformBounds(rr);

            return rr;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConvInsideGeoShapeTransformedBounds : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Rect bounds = (Rect)values[0];
            double angle = (double)values[1];
            double scaleX = (double)values[2];
            double scaleY = (double)values[3];
            double x = (double)values[4];
            double y = (double)values[5];
            double width = bounds.Width;
            double height = bounds.Height;
            x *= width;
            y *= height;

            ScaleTransform sc = new(scaleX, scaleY, x, y);
            RotateTransform ro = new(angle, x, y);
            Rect rr = new(0, 0, width, height);
            rr = sc.TransformBounds(rr);
            rr = ro.TransformBounds(rr);
            //rr.Offset(bounds.Left, bounds.Top);
            return rr;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConvInsideGeoShapeTransformedBounds2 : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Geometry geo = (Geometry)values[0];
            Pen myPen = (Pen)values[1];
            double angle = (double)values[2];
            double scaleX = (double)values[3];
            double scaleY = (double)values[4];
            double x = (double)values[5];
            double y = (double)values[6];

            Rect bounds = geo.GetRenderBounds(myPen);
            var cx = bounds.Left * x + (bounds.Width * x);//0,100,200
            var cy = bounds.Top * y + (bounds.Height * y);//-20,0,20
            ScaleTransform sc = new(scaleX, scaleY, cx, cy);
            RotateTransform ro = new(angle, cx, cy);

            TransformGroup group = new();
            group.Children.Add(sc);
            group.Children.Add(ro);
            var clone = geo.Clone();
            clone.Transform = group;
            Rect rrr = clone.GetRenderBounds(myPen);

            return rrr;
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

    public class MyConvWidthHeightRect : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var w = (double)values[0];
            var h = (double)values[1];
            return new Rect(0, 0, w, h);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class MyConvRectWidth : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (Rect)value;
            return r.Width;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class MyConvRectHeight : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (Rect)value;
            return r.Height;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion Bounds
}