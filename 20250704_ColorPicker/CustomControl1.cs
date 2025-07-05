using _20250704_ColorPicker;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace _20250704_ColorPicker
{



    // PikkaにRGBをバインドしたブラシを追加したもの
    public class PikkaBrush : Pikka
    {
        public PikkaBrush() { MyBind(); }
        private void MyBind()
        {
            MultiBinding mb;
            mb = new() { Converter = new MyConvRGBSolidColorBrush() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyRProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyGProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyBProperty) });
            _ = SetBinding(MySolidColorBrushProperty, mb);
        }


        public SolidColorBrush MySolidColorBrush
        {
            get { return (SolidColorBrush)GetValue(MySolidColorBrushProperty); }
            set { SetValue(MySolidColorBrushProperty, value); }
        }
        public static readonly DependencyProperty MySolidColorBrushProperty =
            DependencyProperty.Register(nameof(MySolidColorBrush), typeof(SolidColorBrush), typeof(Pikka), new PropertyMetadata(null));
    }




    // カラーピッカー
    public class Pikka : Control
    {
        public enum MyCurrentChangingColorType { None = 0, Rgb, Hsv, }

        public MyCurrentChangingColorType CurrentChangingColorType { get; private set; }

        static Pikka()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Pikka), new FrameworkPropertyMetadata(typeof(Pikka)));
        }

        public Pikka()
        {
            
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_Viewbox") is Viewbox vb)
            {
                if (AdornerLayer.GetAdornerLayer(vb) is AdornerLayer layer)
                {
                    MyMarkerAdorner = new MarkerAdorner(vb);
                    layer.Add(MyMarkerAdorner);
                    _ = MyMarkerAdorner.SetBinding(MarkerAdorner.MyXRateProperty, new Binding() { Source = this, Path = new PropertyPath(MySatProperty), Mode = BindingMode.TwoWay });
                    _ = MyMarkerAdorner.SetBinding(MarkerAdorner.MyYRateProperty, new Binding() { Source = this, Path = new PropertyPath(MyValProperty), Mode = BindingMode.TwoWay });
                    _ = MyMarkerAdorner.SetBinding(MarkerAdorner.MyMarkerSizeProperty, new Binding() { Source = this, Path = new PropertyPath(MyMarkerSizeProperty), Mode = BindingMode.TwoWay });
                    
                }
            }

            if (GetTemplateChild("PART_HSVImage") is HSVImage svi)
            {
                _ = svi.SetBinding(HSVImage.HueProperty, new Binding() { Source = this, Path = new PropertyPath(MyHueProperty) });
            }
        }


        #region 依存関係プロパティ

        // 画像サイズ、拡大表示されるので16でも十分。全色表示するには256を指定して、Pikkaのサイズも256にする
        public double MyHSVImageSize
        {
            get { return (double)GetValue(MyHSVImageSizeProperty); }
            set { SetValue(MyHSVImageSizeProperty, value); }
        }
        public static readonly DependencyProperty MyHSVImageSizeProperty =
            DependencyProperty.Register(nameof(MyHSVImageSize), typeof(double), typeof(Pikka), new PropertyMetadata(16.0));

        // Markerのサイズ
        public double MyMarkerSize
        {
            get { return (double)GetValue(MyMarkerSizeProperty); }
            set { SetValue(MyMarkerSizeProperty, value); }
        }
        public static readonly DependencyProperty MyMarkerSizeProperty =
            DependencyProperty.Register(nameof(MyMarkerSize), typeof(double), typeof(Pikka), new PropertyMetadata(20.0));


        public MarkerAdorner MyMarkerAdorner
        {
            get { return (MarkerAdorner)GetValue(MyMarkerAdornerProperty); }
            private set { SetValue(MyMarkerAdornerProperty, value); }
        }
        public static readonly DependencyProperty MyMarkerAdornerProperty =
            DependencyProperty.Register(nameof(MyMarkerAdorner), typeof(MarkerAdorner), typeof(Pikka), new PropertyMetadata(null));



        #region HSV

        public double MyHue
        {
            get { return (double)GetValue(MyHueProperty); }
            set { SetValue(MyHueProperty, value); }
        }
        public static readonly DependencyProperty MyHueProperty =
            DependencyProperty.Register(nameof(MyHue), typeof(double), typeof(Pikka), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnChangedHSV)));


        public double MySat
        {
            get { return (double)GetValue(MySatProperty); }
            set { SetValue(MySatProperty, value); }
        }
        public static readonly DependencyProperty MySatProperty =
            DependencyProperty.Register(nameof(MySat), typeof(double), typeof(Pikka), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnChangedHSV)));


        public double MyVal
        {
            get { return (double)GetValue(MyValProperty); }
            set { SetValue(MyValProperty, value); }
        }
        public static readonly DependencyProperty MyValProperty =
            DependencyProperty.Register(nameof(MyVal), typeof(double), typeof(Pikka), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnChangedHSV)));
        #endregion HSV

        #region RGB

        public byte MyR
        {
            get { return (byte)GetValue(MyRProperty); }
            set { SetValue(MyRProperty, value); }
        }
        public static readonly DependencyProperty MyRProperty =
            DependencyProperty.Register(nameof(MyR), typeof(byte), typeof(Pikka), new FrameworkPropertyMetadata((byte)0, new PropertyChangedCallback(OnChangedRGB)));

        public byte MyG
        {
            get { return (byte)GetValue(MyGProperty); }
            set { SetValue(MyGProperty, value); }
        }
        public static readonly DependencyProperty MyGProperty =
            DependencyProperty.Register(nameof(MyG), typeof(byte), typeof(Pikka), new FrameworkPropertyMetadata((byte)0, new PropertyChangedCallback(OnChangedRGB)));

        public byte MyB
        {
            get { return (byte)GetValue(MyBProperty); }
            set { SetValue(MyBProperty, value); }
        }
        public static readonly DependencyProperty MyBProperty =
            DependencyProperty.Register(nameof(MyB), typeof(byte), typeof(Pikka), new FrameworkPropertyMetadata((byte)0, new PropertyChangedCallback(OnChangedRGB)));
        #endregion RGB

        //#region HSV

        //public double MyHue
        //{
        //    get { return (double)GetValue(MyHueProperty); }
        //    set { SetValue(MyHueProperty, value); }
        //}
        //public static readonly DependencyProperty MyHueProperty =
        //    DependencyProperty.Register(nameof(MyHue), typeof(double), typeof(Pikka), new PropertyMetadata(0.0));


        //public double MySat
        //{
        //    get { return (double)GetValue(MySatProperty); }
        //    set { SetValue(MySatProperty, value); }
        //}
        //public static readonly DependencyProperty MySatProperty =
        //    DependencyProperty.Register(nameof(MySat), typeof(double), typeof(Pikka), new PropertyMetadata(0.0));


        //public double MyVal
        //{
        //    get { return (double)GetValue(MyValProperty); }
        //    set { SetValue(MyValProperty, value); }
        //}
        //public static readonly DependencyProperty MyValProperty =
        //    DependencyProperty.Register(nameof(MyVal), typeof(double), typeof(Pikka), new PropertyMetadata(0.0));
        //#endregion HSV

        //#region RGB

        //public byte MyR
        //{
        //    get { return (byte)GetValue(MyRProperty); }
        //    set { SetValue(MyRProperty, value); }
        //}
        //public static readonly DependencyProperty MyRProperty =
        //    DependencyProperty.Register(nameof(MyR), typeof(byte), typeof(Pikka), new PropertyMetadata((byte)0));

        //public byte MyG
        //{
        //    get { return (byte)GetValue(MyGProperty); }
        //    set { SetValue(MyGProperty, value); }
        //}
        //public static readonly DependencyProperty MyGProperty =
        //    DependencyProperty.Register(nameof(MyG), typeof(byte), typeof(Pikka), new PropertyMetadata((byte)0));

        //public byte MyB
        //{
        //    get { return (byte)GetValue(MyBProperty); }
        //    set { SetValue(MyBProperty, value); }
        //}
        //public static readonly DependencyProperty MyBProperty =
        //    DependencyProperty.Register(nameof(MyB), typeof(byte), typeof(Pikka), new PropertyMetadata((byte)0));
        //#endregion RGB

        #endregion 依存関係プロパティ

        #region 依存関係プロパティの変更後に実行する関数
        
        private static void OnChangedRGB(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Pikka pk && pk.CurrentChangingColorType == MyCurrentChangingColorType.None)
            {
                pk.CurrentChangingColorType = MyCurrentChangingColorType.Rgb;
                (pk.MyHue, pk.MySat, pk.MyVal) = MathHSV.Rgb2hsv(pk.MyR, pk.MyG, pk.MyB);
                pk.CurrentChangingColorType = MyCurrentChangingColorType.None;
            }
        }

        private static void OnChangedHSV(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Pikka pk && pk.CurrentChangingColorType == MyCurrentChangingColorType.None)
            {
                pk.CurrentChangingColorType = MyCurrentChangingColorType.Hsv;
                (pk.MyR, pk.MyG, pk.MyB) = MathHSV.Hsv2rgb(pk.MyHue, pk.MySat, pk.MyVal);
                pk.CurrentChangingColorType = MyCurrentChangingColorType.None;
            }
        }
        #endregion 依存関係プロパティの変更後に実行する関数

    }





    //public class HSVImageWithMarker : Control
    //{
    //    public HSVImage MyHSVImage { get; set; } = null!;
    //    static HSVImageWithMarker()
    //    {
    //        DefaultStyleKeyProperty.OverrideMetadata(typeof(HSVImageWithMarker), new FrameworkPropertyMetadata(typeof(HSVImageWithMarker)));
    //    }
    //    public HSVImageWithMarker()
    //    {
    //        MySikisai = new SikisaiBrush();
    //        Loaded += HSVImageWithMarker_Loaded;
    //    }

    //    private void HSVImageWithMarker_Loaded(object sender, RoutedEventArgs e)
    //    {
    //        // Adorner  Layer   結果
    //        // Viewbox  Viewbox おｋ
    //        // Viewbox  Image   おｋ
    //        // Image    Viewbox クソデカMarker
    //        // Image    Image   クソデカMarker

    //        if (GetTemplateChild("PART_Viewbox") is Viewbox viewbox)
    //        {
    //            MyMarkerAdorner = new MarkerAdorner(viewbox);

    //            // AdornerLayerを取得して、MarkerAdornerを設置
    //            if (AdornerLayer.GetAdornerLayer(viewbox) is AdornerLayer layer)
    //            {
    //                layer.Add(MyMarkerAdorner);
    //            }
    //        }
    //        if (GetTemplateChild("PART_HSVImage") is HSVImage HSVImage)
    //        {
    //            MyHSVImage = HSVImage;
    //        }

    //        MyBind();
    //    }

    //    private void MyBind()
    //    {
    //        MyMarkerAdorner.SetBinding(MarkerAdorner.MyXRateProperty, new Binding() { Source = MySikisai, Path = new PropertyPath(Sikisai.SProperty), Mode = BindingMode.TwoWay });
    //        MyMarkerAdorner.SetBinding(MarkerAdorner.MyYRateProperty, new Binding() { Source = MySikisai, Path = new PropertyPath(Sikisai.VProperty), Mode = BindingMode.TwoWay });

    //        //BindingOperations.SetBinding(MySikisai, Sikisai.SProperty, new Binding() { Source = MyMarkerAdorner, Path = new PropertyPath(MarkerAdorner.MySaturationProperty) });
    //        //BindingOperations.SetBinding(MySikisai, Sikisai.VProperty, new Binding() { Source = MyMarkerAdorner, Path = new PropertyPath(MarkerAdorner.MyValueProperty) });
    //        //BindingOperations.SetBinding(MySikisai, Sikisai.HProperty, new Binding() { Source = MyHSVImage, Path = new PropertyPath(HSVImage.HueProperty) });


    //    }





    //    #region 依存関係プロパティ

    //    public SikisaiBrush MySikisai
    //    {
    //        get { return (SikisaiBrush)GetValue(MySikisaiProperty); }
    //        set { SetValue(MySikisaiProperty, value); }
    //    }
    //    public static readonly DependencyProperty MySikisaiProperty =
    //        DependencyProperty.Register(nameof(MySikisai), typeof(SikisaiBrush), typeof(HSVImageWithMarker), new PropertyMetadata(null));


    //    public MarkerAdorner MyMarkerAdorner
    //    {
    //        get { return (MarkerAdorner)GetValue(MyMarkerAdornerProperty); }
    //        set { SetValue(MyMarkerAdornerProperty, value); }
    //    }
    //    public static readonly DependencyProperty MyMarkerAdornerProperty =
    //        DependencyProperty.Register(nameof(MyMarkerAdorner), typeof(MarkerAdorner), typeof(HSVImageWithMarker), new PropertyMetadata(null));
    //    #endregion 依存関係プロパティ

    //}







    public class EllipseThumb : Thumb
    {
        static EllipseThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EllipseThumb), new FrameworkPropertyMetadata(typeof(EllipseThumb)));
        }
        public EllipseThumb()
        {

        }

        #region 依存関係プロパティ

        public Brush MyInsideStroke
        {
            get { return (Brush)GetValue(MyInsideStrokeProperty); }
            set { SetValue(MyInsideStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyInsideStrokeProperty =
            DependencyProperty.Register(nameof(MyInsideStroke), typeof(Brush), typeof(EllipseThumb), new PropertyMetadata(Brushes.Black));

        public Brush MyOutsideStroke
        {
            get { return (Brush)GetValue(MyOutsideStrokeProperty); }
            set { SetValue(MyOutsideStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyOutsideStrokeProperty =
            DependencyProperty.Register(nameof(MyOutsideStroke), typeof(Brush), typeof(EllipseThumb), new PropertyMetadata(Brushes.White));


        #endregion 依存関係プロパティ
    }


    // RGBからブラシ作成
    public class MyConvRGBSolidColorBrush : IMultiValueConverter
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
            throw new NotImplementedException();
        }
    }


    // -2するだけ
    public class MyConvInsideEllipseSize : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double size = ((double)value) - 2.0;
            if (size < 0) { size = 0; }
            return size;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}