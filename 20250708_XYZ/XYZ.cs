using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _20250708_XYZ
{
    //public enum CurrentCCC { None = 0, RGB, LinearRGB, XYZ }


    // sRGB
    public class XYZ : DependencyObject
    {
        //private CurrentCCC MyCurrentCCC;
        public bool MyIsPropertyChanging;
        public XYZ() { }

        public XYZ(byte r, byte g, byte b)
        {
            R = r; G = g; B = b;
        }


        private static void OnPropertyChangedRGB(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is XYZ me && !me.MyIsPropertyChanging)
            {
                me.MyIsPropertyChanging = true;
                var (lr, lg, lb) = MathIro.Rgb2LinearRGB(me.R, me.G, me.B);
                me.LR = lr; me.LG = lg; me.LB = lb;                
                (me.XD50, me.YD50, me.ZD50) = MathIro.ToXYZD50(lr, lg, lb);// D50のXYZ
                (me.X, me.Y, me.Z) = MathIro.ToXYZ(lr, lg, lb);// D65のXYZ
                me.MyIsPropertyChanging = false;
            }
        }

        private static void OnPropertyChangedXYZ(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is XYZ me && !me.MyIsPropertyChanging)
            {
                me.MyIsPropertyChanging = true;
                var (r, g, b) = MathIro.LinearRgb2Rgb(MathIro.Xyz2LinearRgb(me.X, me.Y, me.Z));
                me.R = r; me.G = g; me.B = b;
                me.MyIsPropertyChanging = false;
            }
        }

        //private static void OnPropertyChangedLinearRGB(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    if (d is XYZ me && !me.MyIsPropertyChanging)
        //    {
        //        me.MyIsPropertyChanging = true;
        //        var (r, g, b) = MathIro.LinearRgb2Rgb(MathIro.Xyz2Rgb(me.X, me.Y, me.Z));
        //        me.R = r; me.G = g; me.B = b;
        //        me.MyIsPropertyChanging = false;
        //    }
        //}


        public double XD50
        {
            get { return (double)GetValue(XD50Property); }
            set { SetValue(XD50Property, value); }
        }
        public static readonly DependencyProperty XD50Property =
            DependencyProperty.Register(nameof(XD50), typeof(double), typeof(XYZ), new PropertyMetadata(0.0));

        public double YD50
        {
            get { return (double)GetValue(YD50Property); }
            set { SetValue(YD50Property, value); }
        }
        public static readonly DependencyProperty YD50Property =
            DependencyProperty.Register(nameof(YD50), typeof(double), typeof(XYZ), new PropertyMetadata(0.0));

        public double ZD50
        {
            get { return (double)GetValue(ZD50Property); }
            set { SetValue(ZD50Property, value); }
        }
        public static readonly DependencyProperty ZD50Property =
            DependencyProperty.Register(nameof(ZD50), typeof(double), typeof(XYZ), new PropertyMetadata(0.0));


        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register(nameof(X), typeof(double), typeof(XYZ), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnPropertyChangedXYZ)));

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register(nameof(Y), typeof(double), typeof(XYZ), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnPropertyChangedXYZ)));

        public double Z
        {
            get { return (double)GetValue(ZProperty); }
            set { SetValue(ZProperty, value); }
        }
        public static readonly DependencyProperty ZProperty =
            DependencyProperty.Register(nameof(Z), typeof(double), typeof(XYZ), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnPropertyChangedXYZ)));


        public byte R
        {
            get { return (byte)GetValue(RProperty); }
            set { SetValue(RProperty, value); }
        }
        public static readonly DependencyProperty RProperty =
            DependencyProperty.Register(nameof(R), typeof(byte), typeof(XYZ), new FrameworkPropertyMetadata((byte)0, new PropertyChangedCallback(OnPropertyChangedRGB)));

        public byte G
        {
            get { return (byte)GetValue(GProperty); }
            set { SetValue(GProperty, value); }
        }
        public static readonly DependencyProperty GProperty =
            DependencyProperty.Register(nameof(G), typeof(byte), typeof(XYZ), new FrameworkPropertyMetadata((byte)0, new PropertyChangedCallback(OnPropertyChangedRGB)));

        public byte B
        {
            get { return (byte)GetValue(BProperty); }
            set { SetValue(BProperty, value); }
        }
        public static readonly DependencyProperty BProperty =
            DependencyProperty.Register(nameof(B), typeof(byte), typeof(XYZ), new FrameworkPropertyMetadata((byte)0, new PropertyChangedCallback(OnPropertyChangedRGB)));


        public double LR
        {
            get { return (double)GetValue(LRProperty); }
            set { SetValue(LRProperty, value); }
        }
        public static readonly DependencyProperty LRProperty =
            DependencyProperty.Register(nameof(LR), typeof(double), typeof(XYZ), new PropertyMetadata(0.0));

        public double LG
        {
            get { return (double)GetValue(LGProperty); }
            set { SetValue(LGProperty, value); }
        }
        public static readonly DependencyProperty LGProperty =
            DependencyProperty.Register(nameof(LG), typeof(double), typeof(XYZ), new PropertyMetadata(0.0));

        public double LB
        {
            get { return (double)GetValue(LBProperty); }
            set { SetValue(LBProperty, value); }
        }
        public static readonly DependencyProperty LBProperty =
            DependencyProperty.Register(nameof(LB), typeof(double), typeof(XYZ), new PropertyMetadata(0.0));

    }
}
