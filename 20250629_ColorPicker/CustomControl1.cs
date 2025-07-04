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

namespace _20250629_ColorPicker
{

  




    public class Pikka : Control
    {
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
                    MyMarkerAdorner.SetBinding(MarkerAdorner.MyXRateProperty, new Binding() { Source = this, Path = new PropertyPath(MySatProperty), Mode = BindingMode.TwoWay });
                    MyMarkerAdorner.SetBinding(MarkerAdorner.MyYRateProperty, new Binding() { Source = this, Path = new PropertyPath(MyValProperty), Mode = BindingMode.TwoWay });
                    MyMarkerAdorner.SetBinding(MarkerAdorner.MyMarkerSizeProperty, new Binding() { Source = this, Path = new PropertyPath(MyMarkerSizeProperty), Mode = BindingMode.TwoWay });

                }
            }

            if (GetTemplateChild("PART_SVImage") is SVImage svi)
            {
                svi.SetBinding(SVImage.HueProperty, new Binding() { Source = this, Path = new PropertyPath(MyHueProperty) });
            }
        }


        #region 依存関係プロパティ


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


        public double MyHue
        {
            get { return (double)GetValue(MyHueProperty); }
            set { SetValue(MyHueProperty, value); }
        }
        public static readonly DependencyProperty MyHueProperty =
            DependencyProperty.Register(nameof(MyHue), typeof(double), typeof(Pikka), new PropertyMetadata(0.0));


        public double MySat
        {
            get { return (double)GetValue(MySatProperty); }
            set { SetValue(MySatProperty, value); }
        }
        public static readonly DependencyProperty MySatProperty =
            DependencyProperty.Register(nameof(MySat), typeof(double), typeof(Pikka), new PropertyMetadata(0.0));


        public double MyVal
        {
            get { return (double)GetValue(MyValProperty); }
            set { SetValue(MyValProperty, value); }
        }
        public static readonly DependencyProperty MyValProperty =
            DependencyProperty.Register(nameof(MyVal), typeof(double), typeof(Pikka), new PropertyMetadata(0.0));

        #endregion 依存関係プロパティ

    }





    public class SVImageWithMarker : Control
    {
        public SVImage MySVImage { get; set; } = null!;
        static SVImageWithMarker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SVImageWithMarker), new FrameworkPropertyMetadata(typeof(SVImageWithMarker)));
        }
        public SVImageWithMarker()
        {
            MySikisai = new SikisaiBrush();
            Loaded += SVImageWithMarker_Loaded;
        }

        private void SVImageWithMarker_Loaded(object sender, RoutedEventArgs e)
        {
            // Adorner  Layer   結果
            // Viewbox  Viewbox おｋ
            // Viewbox  Image   おｋ
            // Image    Viewbox クソデカMarker
            // Image    Image   クソデカMarker

            if (GetTemplateChild("PART_Viewbox") is Viewbox viewbox)
            {
                MyMarkerAdorner = new MarkerAdorner(viewbox);

                // AdornerLayerを取得して、MarkerAdornerを設置
                if (AdornerLayer.GetAdornerLayer(viewbox) is AdornerLayer layer)
                {
                    layer.Add(MyMarkerAdorner);
                }
            }
            if (GetTemplateChild("PART_SVImage") is SVImage sVImage)
            {
                MySVImage = sVImage;
            }

            MyBind();
        }

        private void MyBind()
        {
            MyMarkerAdorner.SetBinding(MarkerAdorner.MyXRateProperty, new Binding() { Source = MySikisai, Path = new PropertyPath(Sikisai.SProperty), Mode = BindingMode.TwoWay });
            MyMarkerAdorner.SetBinding(MarkerAdorner.MyYRateProperty, new Binding() { Source = MySikisai, Path = new PropertyPath(Sikisai.VProperty), Mode = BindingMode.TwoWay });

            //BindingOperations.SetBinding(MySikisai, Sikisai.SProperty, new Binding() { Source = MyMarkerAdorner, Path = new PropertyPath(MarkerAdorner.MySaturationProperty) });
            //BindingOperations.SetBinding(MySikisai, Sikisai.VProperty, new Binding() { Source = MyMarkerAdorner, Path = new PropertyPath(MarkerAdorner.MyValueProperty) });
            //BindingOperations.SetBinding(MySikisai, Sikisai.HProperty, new Binding() { Source = MySVImage, Path = new PropertyPath(SVImage.HueProperty) });


        }





        #region 依存関係プロパティ

        public SikisaiBrush MySikisai
        {
            get { return (SikisaiBrush)GetValue(MySikisaiProperty); }
            set { SetValue(MySikisaiProperty, value); }
        }
        public static readonly DependencyProperty MySikisaiProperty =
            DependencyProperty.Register(nameof(MySikisai), typeof(SikisaiBrush), typeof(SVImageWithMarker), new PropertyMetadata(null));


        public MarkerAdorner MyMarkerAdorner
        {
            get { return (MarkerAdorner)GetValue(MyMarkerAdornerProperty); }
            set { SetValue(MyMarkerAdornerProperty, value); }
        }
        public static readonly DependencyProperty MyMarkerAdornerProperty =
            DependencyProperty.Register(nameof(MyMarkerAdorner), typeof(MarkerAdorner), typeof(SVImageWithMarker), new PropertyMetadata(null));
        #endregion 依存関係プロパティ

    }







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
