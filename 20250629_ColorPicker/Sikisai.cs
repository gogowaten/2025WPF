using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace _20250629_ColorPicker
{
    public class Sikisai : DependencyObject
    {
        private bool IsChangingRGB;// RGBを変更中
        private bool IsChangingHSV;

        public Sikisai()
        {
            HSV = new();
            RGB = new();
        }

        public Sikisai(Color color)
        {
            
        }




        #region 依存関係プロパティ

        public RGB RGB
        {
            get { return (RGB)GetValue(RGBProperty); }
            set { SetValue(RGBProperty, value); }
        }
        public static readonly DependencyProperty RGBProperty =
            DependencyProperty.Register(nameof(RGB), typeof(RGB), typeof(Sikisai), new PropertyMetadata(null));

        public HSV HSV
        {
            get { return (HSV)GetValue(HSVProperty); }
            set { SetValue(HSVProperty, value); }
        }
        public static readonly DependencyProperty HSVProperty =
            DependencyProperty.Register(nameof(HSV), typeof(HSV), typeof(Sikisai), new PropertyMetadata(null));

        public byte R
        {
            get { return (byte)GetValue(RProperty); }
            set { SetValue(RProperty, value); }
        }
        public static readonly DependencyProperty RProperty =
            DependencyProperty.Register(nameof(R), typeof(byte), typeof(Sikisai), new PropertyMetadata(0));

        public byte G
        {
            get { return (byte)GetValue(GProperty); }
            set { SetValue(GProperty, value); }
        }
        public static readonly DependencyProperty GProperty =
            DependencyProperty.Register(nameof(G), typeof(byte), typeof(Sikisai), new PropertyMetadata(0));

        public Byte B
        {
            get { return (Byte)GetValue(BProperty); }
            set { SetValue(BProperty, value); }
        }
        public static readonly DependencyProperty BProperty =
            DependencyProperty.Register(nameof(B), typeof(Byte), typeof(Sikisai), new PropertyMetadata(0));

        public double H
        {
            get { return (double)GetValue(HProperty); }
            set { SetValue(HProperty, value); }
        }
        public static readonly DependencyProperty HProperty =
            DependencyProperty.Register(nameof(H), typeof(double), typeof(Sikisai), new PropertyMetadata(0.0));

        public double S
        {
            get { return (double)GetValue(SProperty); }
            set { SetValue(SProperty, value); }
        }
        public static readonly DependencyProperty SProperty =
            DependencyProperty.Register(nameof(S), typeof(double), typeof(Sikisai), new PropertyMetadata(0.0));

        public double V
        {
            get { return (double)GetValue(VProperty); }
            set { SetValue(VProperty, value); }
        }
        public static readonly DependencyProperty VProperty =
            DependencyProperty.Register(nameof(V), typeof(double), typeof(Sikisai), new PropertyMetadata(0.0));

        #endregion 依存関係プロパティ


        private static void OnHSVChenged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Sikisai)
            {
                if (e.NewValue is HSV)
                {

                }
            }
        }
    }
}
