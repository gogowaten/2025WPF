﻿using _20250713_Lav;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace _20250713_Lab
{
    public class Sikisai : DependencyObject
    {
        private bool IsChanging;

        public Sikisai()
        {
            MyBind();
        }

        private void MyBind()
        {
            MultiBinding mb = new() { Converter = new MyConvColorBrush() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(RProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(GProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(BProperty) });
            BindingOperations.SetBinding(this, MyBrushProperty, mb);
        }


        #region 依存関係プロパティ


        public SolidColorBrush MyBrush
        {
            get { return (SolidColorBrush)GetValue(MyBrushProperty); }
            set { SetValue(MyBrushProperty, value); }
        }
        public static readonly DependencyProperty MyBrushProperty =
            DependencyProperty.Register(nameof(MyBrush), typeof(SolidColorBrush), typeof(Sikisai), new PropertyMetadata(null));



        #region RGB


        public byte R
        {
            get { return (byte)GetValue(RProperty); }
            set { SetValue(RProperty, value); }
        }
        public static readonly DependencyProperty RProperty =
            DependencyProperty.Register(nameof(R), typeof(byte), typeof(Sikisai), new FrameworkPropertyMetadata((byte)0, new PropertyChangedCallback(OnRGBChanged)));

        public byte G
        {
            get { return (byte)GetValue(GProperty); }
            set { SetValue(GProperty, value); }
        }
        public static readonly DependencyProperty GProperty =
            DependencyProperty.Register(nameof(G), typeof(byte), typeof(Sikisai), new FrameworkPropertyMetadata((byte)0, new PropertyChangedCallback(OnRGBChanged)));

        public byte B
        {
            get { return (byte)GetValue(BProperty); }
            set { SetValue(BProperty, value); }
        }
        public static readonly DependencyProperty BProperty =
            DependencyProperty.Register(nameof(B), typeof(byte), typeof(Sikisai), new FrameworkPropertyMetadata((byte)0, new PropertyChangedCallback(OnRGBChanged)));

        #endregion RGB

        #region LinearRGB


        public double LinearR
        {
            get { return (double)GetValue(LinearRProperty); }
            set { SetValue(LinearRProperty, value); }
        }
        public static readonly DependencyProperty LinearRProperty =
            DependencyProperty.Register(nameof(LinearR), typeof(double), typeof(Sikisai), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnLinearRGBChanged)));

        public double LinearG
        {
            get { return (double)GetValue(LinearGProperty); }
            set { SetValue(LinearGProperty, value); }
        }
        public static readonly DependencyProperty LinearGProperty =
            DependencyProperty.Register(nameof(LinearG), typeof(double), typeof(Sikisai), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnLinearRGBChanged)));

        public double LinearB
        {
            get { return (double)GetValue(LinearBProperty); }
            set { SetValue(LinearBProperty, value); }
        }
        public static readonly DependencyProperty LinearBProperty =
            DependencyProperty.Register(nameof(LinearB), typeof(double), typeof(Sikisai), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnLinearRGBChanged)));

        #endregion LinearRGB

        #region XYZD65

        public double XD65
        {
            get { return (double)GetValue(XD65Property); }
            set { SetValue(XD65Property, value); }
        }
        public static readonly DependencyProperty XD65Property =
            DependencyProperty.Register(nameof(XD65), typeof(double), typeof(Sikisai), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnXYZD65Changed)));

        public double YD65
        {
            get { return (double)GetValue(YD65Property); }
            set { SetValue(YD65Property, value); }
        }
        public static readonly DependencyProperty YD65Property =
            DependencyProperty.Register(nameof(YD65), typeof(double), typeof(Sikisai), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnXYZD65Changed)));

        public double ZD65
        {
            get { return (double)GetValue(ZD65Property); }
            set { SetValue(ZD65Property, value); }
        }
        public static readonly DependencyProperty ZD65Property =
            DependencyProperty.Register(nameof(ZD65), typeof(double), typeof(Sikisai), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnXYZD65Changed)));

        #endregion XYZD65



        #region XYZD50

        public double XD50
        {
            get { return (double)GetValue(XD50Property); }
            set { SetValue(XD50Property, value); }
        }
        public static readonly DependencyProperty XD50Property =
            DependencyProperty.Register(nameof(XD50), typeof(double), typeof(Sikisai), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnXYZD50Changed)));

        public double YD50
        {
            get { return (double)GetValue(YD50Property); }
            set { SetValue(YD50Property, value); }
        }
        public static readonly DependencyProperty YD50Property =
            DependencyProperty.Register(nameof(YD50), typeof(double), typeof(Sikisai), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnXYZD50Changed)));

        public double ZD50
        {
            get { return (double)GetValue(ZD50Property); }
            set { SetValue(ZD50Property, value); }
        }
        public static readonly DependencyProperty ZD50Property =
            DependencyProperty.Register(nameof(ZD50), typeof(double), typeof(Sikisai),
                new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnXYZD50Changed)));

        #endregion XYZD50

        #region Lab

        public double LabL
        {
            get { return (double)GetValue(LabLProperty); }
            set { SetValue(LabLProperty, value); }
        }
        public static readonly DependencyProperty LabLProperty =
            DependencyProperty.Register(nameof(LabL), typeof(double), typeof(Sikisai), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnLabChanged)));

        public double Laba
        {
            get { return (double)GetValue(LabaProperty); }
            set { SetValue(LabaProperty, value); }
        }
        public static readonly DependencyProperty LabaProperty =
            DependencyProperty.Register(nameof(Laba), typeof(double), typeof(Sikisai), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnLabChanged)));

        public double Labb
        {
            get { return (double)GetValue(LabbProperty); }
            set { SetValue(LabbProperty, value); }
        }
        public static readonly DependencyProperty LabbProperty =
            DependencyProperty.Register(nameof(Labb), typeof(double), typeof(Sikisai), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnLabChanged)));

        #endregion Lab

        #region LCh

        public double LChL
        {
            get { return (double)GetValue(LChLProperty); }
            set { SetValue(LChLProperty, value); }
        }
        public static readonly DependencyProperty LChLProperty =
            DependencyProperty.Register(nameof(LChL), typeof(double), typeof(Sikisai), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnLChChanged)));

        public double LChC
        {
            get { return (double)GetValue(LChCProperty); }
            set { SetValue(LChCProperty, value); }
        }
        public static readonly DependencyProperty LChCProperty =
            DependencyProperty.Register(nameof(LChC), typeof(double), typeof(Sikisai), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnLChChanged)));

        public double LChh
        {
            get { return (double)GetValue(LChhProperty); }
            set { SetValue(LChhProperty, value); }
        }
        public static readonly DependencyProperty LChhProperty =
            DependencyProperty.Register(nameof(LChh), typeof(double), typeof(Sikisai), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnLChChanged)));




        #endregion LCh



        #endregion 依存関係プロパティ


        #region PropertyChangedCallback

        private static void OnRGBChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Sikisai me && !me.IsChanging)
            {
                me.IsChanging = true;
                (me.LinearR, me.LinearG, me.LinearB) = MathIro.Rgb2LinearRGB(me.R, me.G, me.B);
                (me.XD65, me.YD65, me.ZD65) = MathIro.LinearRGBToXYZD65v0(me.LinearR, me.LinearG, me.LinearB);
                (me.XD50, me.YD50, me.ZD50) = MathIro.LinearRGBToXYZD50v2(me.LinearR, me.LinearG, me.LinearB);
                (me.LabL, me.Laba, me.Labb) = MathIro.XyzD50ToLab(me.XD50, me.YD50, me.ZD50);
                (me.LChL, me.LChC, me.LChh) = MathIro.LabToLch(me.LabL, me.Laba, me.Labb);
                
                me.IsChanging = false;
            }
        }

        private static void OnLinearRGBChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Sikisai me && !me.IsChanging)
            {
                me.IsChanging = true;
                (me.R, me.G, me.B) = MathIro.LinearRgb2Rgb(me.LinearR, me.LinearG, me.LinearB);
                (me.XD65, me.YD65, me.ZD65) = MathIro.LinearRGBToXYZD65v0(me.LinearR, me.LinearG, me.LinearB);
                (me.XD50, me.YD50, me.ZD50) = MathIro.LinearRGBToXYZD50v2(me.LinearR, me.LinearG, me.LinearB);
                (me.LabL, me.Laba, me.Labb) = MathIro.XyzD50ToLab(me.XD50, me.YD50, me.ZD50);
                me.IsChanging = false;
            }
        }

        private static void OnXYZD50Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Sikisai me && !me.IsChanging)
            {
                me.IsChanging = true;
                (me.LinearR, me.LinearG, me.LinearB) = MathIro.XyzD50ToLinearRgbV2(me.XD50, me.YD50, me.ZD50);
                (me.LabL, me.Laba, me.Labb) = MathIro.XyzD50ToLab(me.XD50, me.YD50, me.ZD50);
                (me.R, me.G, me.B) = MathIro.LinearRgb2Rgb(me.LinearR, me.LinearG, me.LinearB);
                (me.XD65, me.YD65, me.ZD65) = MathIro.LinearRGBToXYZD65v0(me.LinearR, me.LinearG, me.LinearB);
                me.IsChanging = false;
            }
        }


        private static void OnXYZD65Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Sikisai me && !me.IsChanging)
            {
                me.IsChanging = true;
                (me.LinearR, me.LinearG, me.LinearB) = MathIro.XyzD65ToLinearRgbv2(me.XD65, me.YD65, me.ZD65);
                (me.XD50, me.YD50, me.ZD50) = MathIro.LinearRGBToXYZD50v2(me.LinearR, me.LinearG, me.LinearB);
                (me.LabL, me.Laba, me.Labb) = MathIro.XyzD50ToLab(me.XD50, me.YD50, me.ZD50);
                (me.R, me.G, me.B) = MathIro.LinearRgb2Rgb(me.LinearR, me.LinearG, me.LinearB);
                me.IsChanging = false;
            }
        }


        private static void OnLabChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Sikisai me && !me.IsChanging)
            {
                me.IsChanging = true;
                (me.XD50, me.YD50, me.ZD50) = MathIro.LabToXYZD50(me.LabL, me.Laba, me.Labb);
                (me.LinearR, me.LinearG, me.LinearB) = MathIro.XyzD50ToLinearRgbV2(me.XD50, me.YD50, me.ZD50);
                (me.R, me.G, me.B) = MathIro.LinearRgb2Rgb(me.LinearR, me.LinearG, me.LinearB);
                (me.XD65, me.YD65, me.ZD65) = MathIro.LinearRGBToXYZD65v0(me.LinearR, me.LinearG, me.LinearB);
                me.IsChanging = false;
            }
        }

        private static void OnLChChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Sikisai me && !me.IsChanging)
            {
                me.IsChanging = true;
                (me.LabL, me.Laba, me.Labb) = MathIro.XyzD50ToLab(me.XD50, me.YD50, me.ZD50);
                (me.XD50, me.YD50, me.ZD50) = MathIro.LabToXYZD50(me.LabL, me.Laba, me.Labb);
                (me.LinearR, me.LinearG, me.LinearB) = MathIro.XyzD50ToLinearRgbV2(me.XD50, me.YD50, me.ZD50);
                (me.XD65, me.YD65, me.ZD65) = MathIro.LinearRGBToXYZD65v0(me.LinearR, me.LinearG, me.LinearB);
                (me.R, me.G, me.B) = MathIro.LinearRgb2Rgb(me.LinearR, me.LinearG, me.LinearB);
                me.IsChanging = false;
            }
        }


        #endregion PropertyChangedCallback
    }






    //public class LinearRGB : DependencyObject
    //{
    //    public LinearRGB() { }
    //    public LinearRGB(RGB rgb)
    //    {
    //        this.R = MathIro.Rgb2LinearRGB(rgb.R);
    //        this.G = MathIro.Rgb2LinearRGB(rgb.G);
    //        this.B = MathIro.Rgb2LinearRGB(rgb.B);
    //    }
    //    public LinearRGB(byte r, byte g, byte b)
    //    {
    //        this.R = MathIro.Rgb2LinearRGB(r);
    //        this.G = MathIro.Rgb2LinearRGB(g);
    //        this.B = MathIro.Rgb2LinearRGB(b);
    //    }


    //    public double R
    //    {
    //        get { return (double)GetValue(RProperty); }
    //        set { SetValue(RProperty, value); }
    //    }
    //    public static readonly DependencyProperty RProperty =
    //        DependencyProperty.Register(nameof(R), typeof(double), typeof(LinearRGB), new PropertyMetadata(0.0));

    //    public double G
    //    {
    //        get { return (double)GetValue(GProperty); }
    //        set { SetValue(GProperty, value); }
    //    }
    //    public static readonly DependencyProperty GProperty =
    //        DependencyProperty.Register(nameof(G), typeof(double), typeof(LinearRGB), new PropertyMetadata(0.0));

    //    public double B
    //    {
    //        get { return (double)GetValue(BProperty); }
    //        set { SetValue(BProperty, value); }
    //    }
    //    public static readonly DependencyProperty BProperty =
    //        DependencyProperty.Register(nameof(B), typeof(double), typeof(LinearRGB), new PropertyMetadata(0.0));

    //}



    //public class RGB : DependencyObject
    //{
    //    public RGB() { }


    //    public byte R
    //    {
    //        get { return (byte)GetValue(RProperty); }
    //        set { SetValue(RProperty, value); }
    //    }
    //    public static readonly DependencyProperty RProperty =
    //        DependencyProperty.Register(nameof(R), typeof(byte), typeof(RGB), new PropertyMetadata((byte)0));

    //    public byte G
    //    {
    //        get { return (byte)GetValue(GProperty); }
    //        set { SetValue(GProperty, value); }
    //    }
    //    public static readonly DependencyProperty GProperty =
    //        DependencyProperty.Register(nameof(G), typeof(byte), typeof(RGB), new PropertyMetadata((byte)0));

    //    public byte B
    //    {
    //        get { return (byte)GetValue(BProperty); }
    //        set { SetValue(BProperty, value); }
    //    }
    //    public static readonly DependencyProperty BProperty =
    //        DependencyProperty.Register(nameof(B), typeof(byte), typeof(RGB), new PropertyMetadata((byte)0));

    //}



    //public class XYZ : DependencyObject
    //{
    //    public XYZ() { }


    //    public double X
    //    {
    //        get { return (double)GetValue(XProperty); }
    //        set { SetValue(XProperty, value); }
    //    }
    //    public static readonly DependencyProperty XProperty =
    //        DependencyProperty.Register(nameof(X), typeof(double), typeof(XYZ), new PropertyMetadata(0.0));

    //    public double Y
    //    {
    //        get { return (double)GetValue(YProperty); }
    //        set { SetValue(YProperty, value); }
    //    }
    //    public static readonly DependencyProperty YProperty =
    //        DependencyProperty.Register(nameof(Y), typeof(double), typeof(XYZ), new PropertyMetadata(0.0));

    //    public double Z
    //    {
    //        get { return (double)GetValue(ZProperty); }
    //        set { SetValue(ZProperty, value); }
    //    }
    //    public static readonly DependencyProperty ZProperty =
    //        DependencyProperty.Register(nameof(Z), typeof(double), typeof(XYZ), new PropertyMetadata(0.0));

    //}


    //public class XYZD50 : DependencyObject
    //{
    //    public XYZD50() { }


    //    public double X
    //    {
    //        get { return (double)GetValue(XProperty); }
    //        set { SetValue(XProperty, value); }
    //    }
    //    public static readonly DependencyProperty XProperty =
    //        DependencyProperty.Register(nameof(X), typeof(double), typeof(XYZD50), new PropertyMetadata(0.0));

    //    public double Y
    //    {
    //        get { return (double)GetValue(YProperty); }
    //        set { SetValue(YProperty, value); }
    //    }
    //    public static readonly DependencyProperty YProperty =
    //        DependencyProperty.Register(nameof(Y), typeof(double), typeof(XYZD50), new PropertyMetadata(0.0));

    //    public double Z
    //    {
    //        get { return (double)GetValue(ZProperty); }
    //        set { SetValue(ZProperty, value); }
    //    }
    //    public static readonly DependencyProperty ZProperty =
    //        DependencyProperty.Register(nameof(Z), typeof(double), typeof(XYZD50), new PropertyMetadata(0.0));

    //}









    public class MyConvColorBrush : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (byte)values[0];
            var g = (byte)values[1];
            var b = (byte)values[2];
            return new SolidColorBrush(Color.FromArgb(255, r, g, b));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
