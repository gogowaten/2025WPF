using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace _20250706
{
    public class HSV : DependencyObject
    {
        public HSV() { MyHSV = new(0, 0, 0); MyBind(); }
        public HSV(double myH, double myS, double myV) : this()
        {
            MyH = myH; MyS = myS; MyV = myV;
        }
        public HSV((double h, double s, double v) a) : this()
        {
            MyH = a.h; MyS = a.s; MyV = a.v;
        }

        private void MyBind()
        {
            MultiBinding mb;
            mb = new() { Converter = new MyConvHSV(), Mode = BindingMode.TwoWay };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyHProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MySProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyVProperty) });
            BindingOperations.SetBinding(this, MyHSVProperty, mb);
        }

        class MyConvHSV : IMultiValueConverter
        {
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                var h = (double)values[0];
                var s = (double)values[1];
                var v = (double)values[2];
                return (h, s, v);
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        #region 依存関係プロパティ

        public (double h, double s, double v) MyHSV
        {
            get { return ((double h, double s, double v))GetValue(MyHSVProperty); }
            set { SetValue(MyHSVProperty, value); }
        }
        public static readonly DependencyProperty MyHSVProperty =
            DependencyProperty.Register(nameof(MyHSV), typeof((double h, double s, double v)), typeof(HSV), new PropertyMetadata(null));


        public double MyH
        {
            get { return (double)GetValue(MyHProperty); }
            set { SetValue(MyHProperty, value); }
        }
        public static readonly DependencyProperty MyHProperty =
            DependencyProperty.Register(nameof(MyH), typeof(double), typeof(HSV), new PropertyMetadata(0.0));

        public double MyS
        {
            get { return (double)GetValue(MySProperty); }
            set { SetValue(MySProperty, value); }
        }
        public static readonly DependencyProperty MySProperty =
            DependencyProperty.Register(nameof(MyS), typeof(double), typeof(HSV), new PropertyMetadata(0.0));

        public double MyV
        {
            get { return (double)GetValue(MyVProperty); }
            set { SetValue(MyVProperty, value); }
        }
        public static readonly DependencyProperty MyVProperty =
            DependencyProperty.Register(nameof(MyV), typeof(double), typeof(HSV), new PropertyMetadata(0.0));
        #endregion 依存関係プロパティ

    }


    public class RGB : DependencyObject
    {
        public RGB() { MyRGB = new(0, 0, 0); MyBind(); }
        public RGB(byte myR, byte myG, byte myB) : this() { MyR = myR; MyG = myG; MyB = myB; }
        public RGB((byte r, byte g, byte b) a) : this() { MyR = a.r; MyG = a.g; MyB = a.b; }

        private void MyBind()
        {
            MultiBinding mb = new() { Converter = new MyConvRGB(), Mode = BindingMode.TwoWay };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyRProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyGProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyBProperty) });
            BindingOperations.SetBinding(this, MyRGBProperty, mb);
        }


        class MyConvRGB : IMultiValueConverter
        {
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                var r = (byte)values[0];
                var g = (byte)values[1];
                var b = (byte)values[2];
                return (r, g, b);
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        #region 依存関係プロパティ
        public (byte r, byte g, byte b) MyRGB
        {
            get { return ((byte r, byte g, byte b))GetValue(MyRGBProperty); }
            set { SetValue(MyRGBProperty, value); }
        }
        public static readonly DependencyProperty MyRGBProperty =
            DependencyProperty.Register(nameof(MyRGB), typeof((byte r, byte g, byte b)), typeof(RGB), new PropertyMetadata(null));

        public byte MyR
        {
            get { return (byte)GetValue(MyRProperty); }
            set { SetValue(MyRProperty, value); }
        }
        public static readonly DependencyProperty MyRProperty =
            DependencyProperty.Register(nameof(MyR), typeof(byte), typeof(RGB), new PropertyMetadata((byte)0));

        public byte MyG
        {
            get { return (byte)GetValue(MyGProperty); }
            set { SetValue(MyGProperty, value); }
        }
        public static readonly DependencyProperty MyGProperty =
            DependencyProperty.Register(nameof(MyG), typeof(byte), typeof(RGB), new PropertyMetadata((byte)0));

        public byte MyB
        {
            get { return (byte)GetValue(MyBProperty); }
            set { SetValue(MyBProperty, value); }
        }
        public static readonly DependencyProperty MyBProperty =
            DependencyProperty.Register(nameof(MyB), typeof(byte), typeof(RGB), new PropertyMetadata((byte)0));
        #endregion 依存関係プロパティ

    }


    public class Iro : DependencyObject
    {
        public Iro()
        {
            MyHSV = new HSV();
            MyRGB = new RGB();
            MyBind();

        }

        private void MyBind()
        {
            BindingOperations.SetBinding(this, MyRGBProperty, new Binding() { Source = this, Path = new PropertyPath(MyHSVProperty), Mode = BindingMode.TwoWay, Converter = new MyConvHSVRGB() });
            //BindingOperations.SetBinding(MyRGB, RGB.MyRGBProperty, new Binding() { Source = MyHSV, Path = new PropertyPath(HSV.MyHSVProperty), Converter = new MyConv(), Mode = BindingMode.TwoWay });

        }

        class MyConv : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                (double h, double s, double v) = ((double h, double s, double v))value;
                return MathHSV.Hsv2rgb(h, s, v);
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                (byte r, byte g, byte b) = ((byte r, byte g, byte b))value;
                return MathHSV.Rgb2hsv(r, g, b);
            }
        }

        public HSV MyHSV
        {
            get { return (HSV)GetValue(MyHSVProperty); }
            set { SetValue(MyHSVProperty, value); }
        }
        public static readonly DependencyProperty MyHSVProperty =
            DependencyProperty.Register(nameof(MyHSV), typeof(HSV), typeof(Iro), new PropertyMetadata(null));

        public RGB MyRGB
        {
            get { return (RGB)GetValue(MyRGBProperty); }
            set { SetValue(MyRGBProperty, value); }
        }
        public static readonly DependencyProperty MyRGBProperty =
            DependencyProperty.Register(nameof(MyRGB), typeof(RGB), typeof(Iro), new PropertyMetadata(null));


    }




    public class MyConvHSVRGB : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hsv = (HSV)value;
            var (r, g, b) = MathHSV.Hsv2rgb(hsv.MyH, hsv.MyS, hsv.MyV);
            return new RGB(r, g, b);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var rgb = (RGB)value;
            return new HSV(MathHSV.Rgb2hsv(rgb.MyR, rgb.MyG, rgb.MyB));
        }
    }





}
