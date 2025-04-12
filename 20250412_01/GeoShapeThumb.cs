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

namespace _20250412_01
{
    /// <summary>
    /// このカスタム コントロールを XAML ファイルで使用するには、手順 1a または 1b の後、手順 2 に従います。
    ///
    /// 手順 1a) 現在のプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:_20250412_01"
    ///
    ///
    /// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:_20250412_01;assembly=_20250412_01"
    ///
    /// また、XAML ファイルのあるプロジェクトからこのプロジェクトへのプロジェクト参照を追加し、
    /// リビルドして、コンパイル エラーを防ぐ必要があります:
    ///
    ///     ソリューション エクスプローラーで対象のプロジェクトを右クリックし、
    ///     [参照の追加] の [プロジェクト] を選択してから、このプロジェクトを参照し、選択します。
    ///
    ///
    /// 手順 2)
    /// コントロールを XAML ファイルで使用します。
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class GeoShapeThumb : Thumb
    {
        public GeoShapeWithAnchorHandle MyGeoShape { get; private set; } = null!;
        public Grid MyGrid { get; private set; } = null!;

        static GeoShapeThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GeoShapeThumb), new FrameworkPropertyMetadata(typeof(GeoShapeThumb)));
        }
        public GeoShapeThumb()
        {
            Canvas.SetLeft(this, 0);
            Canvas.SetTop(this, 0);
            DragDelta += GeoShapeThumb_DragDelta;
            Loaded += GeoShapeThumb_Loaded;
        }

        private void GeoShapeThumb_Loaded(object sender, RoutedEventArgs e)
        {
            var bind = new MultiBinding() { Converter = new MyConvTransform() };
            bind.Bindings.Add(new Binding() { Source = MyGeoShape, Path = new PropertyPath(GeoShape.MyGeometryRenderBoundsWithPenProperty) });
            bind.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyAngleProperty) });
            bind.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyScaleXProperty) });
            bind.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyScaleYProperty) });
            bind.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyCenterXProperty) });
            bind.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyCenterYProperty) });

            //SetBinding(MyTransformProperty, bind);
            SetBinding(RenderTransformProperty, bind);
            //MyGeoShape.SetBinding(RenderTransformProperty, bind);
            //MyGrid.SetBinding(RenderTransformProperty, bind);
        }

        private void GeoShapeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
            Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("geoshape") is GeoShapeWithAnchorHandle shape)
            {
                MyGeoShape = shape;
            }
            if (GetTemplateChild("grid") is Grid panel) { MyGrid = panel; }
        }

        public void AnchorHandleSwitch() => MyGeoShape?.AnchorHandleSwitch();

        #region shape

        public double MyAngle
        {
            get { return (double)GetValue(MyAngleProperty); }
            set { SetValue(MyAngleProperty, value); }
        }
        public static readonly DependencyProperty MyAngleProperty =
            DependencyProperty.Register(nameof(MyAngle), typeof(double), typeof(GeoShapeThumb), new PropertyMetadata(0.0));

        public double MyScaleX
        {
            get { return (double)GetValue(MyScaleXProperty); }
            set { SetValue(MyScaleXProperty, value); }
        }
        public static readonly DependencyProperty MyScaleXProperty =
            DependencyProperty.Register(nameof(MyScaleX), typeof(double), typeof(GeoShapeThumb), new PropertyMetadata(1.0));

        public double MyScaleY
        {
            get { return (double)GetValue(MyScaleYProperty); }
            set { SetValue(MyScaleYProperty, value); }
        }
        public static readonly DependencyProperty MyScaleYProperty =
            DependencyProperty.Register(nameof(MyScaleY), typeof(double), typeof(GeoShapeThumb), new PropertyMetadata(1.0));

        public double MyCenterX
        {
            get { return (double)GetValue(MyCenterXProperty); }
            set { SetValue(MyCenterXProperty, value); }
        }
        public static readonly DependencyProperty MyCenterXProperty =
            DependencyProperty.Register(nameof(MyCenterX), typeof(double), typeof(GeoShapeThumb), new PropertyMetadata(0.0));

        public double MyCenterY
        {
            get { return (double)GetValue(MyCenterYProperty); }
            set { SetValue(MyCenterYProperty, value); }
        }
        public static readonly DependencyProperty MyCenterYProperty =
            DependencyProperty.Register(nameof(MyCenterY), typeof(double), typeof(GeoShapeThumb), new PropertyMetadata(0.0));

        #endregion shape


        public Transform MyTransform
        {
            get { return (Transform)GetValue(MyTransformProperty); }
            set { SetValue(MyTransformProperty, value); }
        }
        public static readonly DependencyProperty MyTransformProperty =
            DependencyProperty.Register(nameof(MyTransform), typeof(Transform), typeof(GeoShapeThumb), new PropertyMetadata(null));

    }

    public class MyConvTransform : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var bounds = (Rect)values[0];
            var angle = (double)values[1];
            var scalex = (double)values[2];
            var scaley = (double)values[3];
            var cx = (double)values[4];
            var cy = (double)values[5];
            var x = cx * bounds.Width + bounds.Left;
            var y = cy * bounds.Height + bounds.Top;
            TransformGroup transform = new();
            //transform.Children.Add(new ScaleTransform(scalex, scaley, x, y));
            //transform.Children.Add(new RotateTransform(angle, x, y));//これだと図形が移動してしまう
            transform.Children.Add(new RotateTransform(angle));
            return transform;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
