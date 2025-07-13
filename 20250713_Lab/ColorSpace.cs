using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _20250713_Lab
{
    public class ColorSpace : DependencyObject
    {
        public ColorSpace() { }


        public RGB MyRGB
        {
            get { return (RGB)GetValue(MyRGBProperty); }
            set { SetValue(MyRGBProperty, value); }
        }
        public static readonly DependencyProperty MyRGBProperty =
            DependencyProperty.Register(nameof(MyRGB), typeof(RGB), typeof(ColorSpace), new PropertyMetadata(new RGB()));


        public LinearRGB MyLinearRGB
        {
            get { return (LinearRGB)GetValue(MyLinearRGBProperty); }
            set { SetValue(MyLinearRGBProperty, value); }
        }
        public static readonly DependencyProperty MyLinearRGBProperty =
            DependencyProperty.Register(nameof(MyLinearRGB), typeof(LinearRGB), typeof(ColorSpace), new PropertyMetadata(new LinearRGB()));


    }


    public class Lav : DependencyObject
    {
        public Lav() { }


        public double L
        {
            get { return (double)GetValue(LProperty); }
            set { SetValue(LProperty, value); }
        }
        public static readonly DependencyProperty LProperty =
            DependencyProperty.Register(nameof(L), typeof(double), typeof(Lav), new PropertyMetadata(0.0));

        public double A
        {
            get { return (double)GetValue(AProperty); }
            set { SetValue(AProperty, value); }
        }
        public static readonly DependencyProperty AProperty =
            DependencyProperty.Register(nameof(A), typeof(double), typeof(Lav), new PropertyMetadata(0.0));

        public double B
        {
            get { return (double)GetValue(BProperty); }
            set { SetValue(BProperty, value); }
        }
        public static readonly DependencyProperty BProperty =
            DependencyProperty.Register(nameof(B), typeof(double), typeof(Lav), new PropertyMetadata(0.0));
        XYZD50
    }



    public class LinearRGB : DependencyObject
    {
        public LinearRGB() { }



        public double LR
        {
            get { return (double)GetValue(LRProperty); }
            set { SetValue(LRProperty, value); }
        }
        public static readonly DependencyProperty LRProperty =
            DependencyProperty.Register(nameof(LR), typeof(double), typeof(LinearRGB), new PropertyMetadata(0.0));

        public double LG
        {
            get { return (double)GetValue(LGProperty); }
            set { SetValue(LGProperty, value); }
        }
        public static readonly DependencyProperty LGProperty =
            DependencyProperty.Register(nameof(LG), typeof(double), typeof(LinearRGB), new PropertyMetadata(0.0));

        public double LB
        {
            get { return (double)GetValue(LBProperty); }
            set { SetValue(LBProperty, value); }
        }
        public static readonly DependencyProperty LBProperty =
            DependencyProperty.Register(nameof(LB), typeof(double), typeof(LinearRGB), new PropertyMetadata(0.0));

    }

    public class RGB : DependencyObject
    {
        public RGB() { }


        public byte R
        {
            get { return (byte)GetValue(RProperty); }
            set { SetValue(RProperty, value); }
        }
        public static readonly DependencyProperty RProperty =
            DependencyProperty.Register(nameof(R), typeof(byte), typeof(RGB), new PropertyMetadata((byte)0));

        public byte G
        {
            get { return (byte)GetValue(GProperty); }
            set { SetValue(GProperty, value); }
        }
        public static readonly DependencyProperty GProperty =
            DependencyProperty.Register(nameof(G), typeof(byte), typeof(RGB), new PropertyMetadata((byte)0));

        public byte B
        {
            get { return (byte)GetValue(BProperty); }
            set { SetValue(BProperty, value); }
        }
        public static readonly DependencyProperty BProperty =
            DependencyProperty.Register(nameof(B), typeof(byte), typeof(RGB), new PropertyMetadata((byte)0));

    }



    public class XYZ : DependencyObject
    {
        public XYZ() { }


        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register(nameof(X), typeof(double), typeof(XYZ), new PropertyMetadata(0.0));

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register(nameof(Y), typeof(double), typeof(XYZ), new PropertyMetadata(0.0));

        public double Z
        {
            get { return (double)GetValue(ZProperty); }
            set { SetValue(ZProperty, value); }
        }
        public static readonly DependencyProperty ZProperty =
            DependencyProperty.Register(nameof(Z), typeof(double), typeof(XYZ), new PropertyMetadata(0.0));

    }


    public class XYZD50 : DependencyObject
    {
        public XYZD50() { }


        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register(nameof(X), typeof(double), typeof(XYZD50), new PropertyMetadata(0.0));

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register(nameof(Y), typeof(double), typeof(XYZD50), new PropertyMetadata(0.0));

        public double Z
        {
            get { return (double)GetValue(ZProperty); }
            set { SetValue(ZProperty, value); }
        }
        public static readonly DependencyProperty ZProperty =
            DependencyProperty.Register(nameof(Z), typeof(double), typeof(XYZD50), new PropertyMetadata(0.0));

    }




}
