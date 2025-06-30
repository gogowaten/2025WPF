using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace _20250629_ColorPicker
{
    public class SikisaiBrush : Sikisai
    {
        public SikisaiBrush()
        {
            MyBind();
        }

        public SikisaiBrush(SolidColorBrush myBrush) : this()
        {
            MyBrush = myBrush;
        }

        public SolidColorBrush MyBrush
        {
            get { return (SolidColorBrush)GetValue(MyBrushProperty); }
            set { SetValue(MyBrushProperty, value); }
        }
        public static readonly DependencyProperty MyBrushProperty =
            DependencyProperty.Register(nameof(MyBrush), typeof(SolidColorBrush), typeof(SikisaiBrush),
                new FrameworkPropertyMetadata(Brushes.Black,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        private void MyBind()
        {
            MultiBinding mb = new() { Converter = new MyConvRGBtoBrush() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(RProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(GProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(BProperty) });
            BindingOperations.SetBinding(this, MyBrushProperty, mb);
        }



    }





    public class Sikisai : DependencyObject
    {
        private bool IsChangingRGB { get; set; }// RGBを変更中
        private bool IsChangingHSV;

        public Sikisai() : this(Colors.Black)
        {

        }

        public Sikisai(Color color)
        {
            R = color.R; G = color.G; B = color.B;
            HSV hsv = MathHSV.Rgb2HSV(R, G, B);
            H = hsv.H; S = hsv.S; V = hsv.V;
        }



        #region 依存関係プロパティ

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

        public double H
        {
            get { return (double)GetValue(HProperty); }
            set { SetValue(HProperty, value); }
        }
        public static readonly DependencyProperty HProperty =
            DependencyProperty.Register(nameof(H), typeof(double), typeof(Sikisai), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnHSVChenged)));

        public double S
        {
            get { return (double)GetValue(SProperty); }
            set { SetValue(SProperty, value); }
        }
        public static readonly DependencyProperty SProperty =
            DependencyProperty.Register(nameof(S), typeof(double), typeof(Sikisai), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnHSVChenged)));

        public double V
        {
            get { return (double)GetValue(VProperty); }
            set { SetValue(VProperty, value); }
        }
        public static readonly DependencyProperty VProperty =
            DependencyProperty.Register(nameof(V), typeof(double), typeof(Sikisai), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnHSVChenged)));


        #endregion 依存関係プロパティ

        /// <summary>
        /// <see cref="Sikisai"/> オブジェクトの HSV (色相、彩度、明度) プロパティの変更を処理します。
        /// </summary>
        /// <remarks>このメソッドは、<see cref="Sikisai"/> オブジェクトの RGB プロパティを更新された HSV 値と同期します。オブジェクトが更新中であることを示すフラグを一時的に設定することで、HSV プロパティの変更によって再帰的な更新がトリガーされないようにします。</remarks>
        /// <param name="d">HSV プロパティが変更された <see cref="DependencyObject"/>。</param>
        /// <param name="e">プロパティの変更に関する情報を含むイベントデータ。</param>
        private static void OnHSVChenged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Sikisai sikisai)
            {
                if (sikisai.IsChangingHSV) { return; }
                sikisai.IsChangingHSV = true;
                var rgb = MathHSV.Hsv2RGB(sikisai.H, sikisai.S, sikisai.V);
                sikisai.R = rgb.R; sikisai.G = rgb.G; sikisai.B = rgb.B;
                sikisai.IsChangingHSV = false;
            }
        }

        private static void OnRGBChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Sikisai sikisai)
            {
                if (sikisai.IsChangingRGB) { return; }
                sikisai.IsChangingRGB = true;
                var hsv = MathHSV.Rgb2HSV(sikisai.R, sikisai.G, sikisai.B);
                sikisai.H = hsv.H; sikisai.V = hsv.V; sikisai.S = hsv.S;
                sikisai.IsChangingRGB = false;
            }
        }




    }






    //public class MyConvHSV : IMultiValueConverter
    //{
    //    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        var h = (double)values[0];
    //        var s = (double)values[1];
    //        var v = (double)values[2];
    //        return new HSV(h, s, v);
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        var v = (HSV)value;
    //        return [v.H, v.S, v.V];
    //    }
    //}

    //public class MyConvRGB : IMultiValueConverter
    //{
    //    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        var r = (byte)values[0];
    //        var g = (byte)values[1];
    //        var b = (byte)values[2];
    //        //var a = (byte)values[3];
    //        return new RGB(r, g, b);
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        RGB rgb = (RGB)value;
    //        return [rgb.R, rgb.G, rgb.B];
    //    }
    //}





    public class MyConvRGBtoBrush : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (byte)values[0];
            var g = (byte)values[1];
            var b = (byte)values[2];
            return new SolidColorBrush(Color.FromRgb(r, g, b));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var brush = (SolidColorBrush)value;
            return [brush.Color.R, brush.Color.G, brush.Color.B];
        }
    }







}
