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

namespace _20250412
{
    /// <summary>
    /// このカスタム コントロールを XAML ファイルで使用するには、手順 1a または 1b の後、手順 2 に従います。
    ///
    /// 手順 1a) 現在のプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:_20250412"
    ///
    ///
    /// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:_20250412;assembly=_20250412"
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
    public class CustomControl1 : Control
    {
        static CustomControl1()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomControl1), new FrameworkPropertyMetadata(typeof(CustomControl1)));
        }
    }

    public class GeometryThumb : Thumb
    {
        public GeoShapeWithAnchorHandle MyGeoShape { get; set; } = null!;
        public Panel MyPanel { get; set; } = null!;

        static GeometryThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GeometryThumb), new FrameworkPropertyMetadata(typeof(GeometryThumb)));
        }
        public GeometryThumb()
        {
            Loaded += GeometryThumb_Loaded;
            DragDelta += GeometryThumb_DragDelta;
        }


        private void GeometryThumb_Loaded(object sender, RoutedEventArgs e)
        {
            MyBindOrigin();
            MyBindTransform();
            MyBindTranslateXY();
            MyBindSetGeoShapeBounds();

            //MyPanel.SetBinding(WidthProperty, new Binding() { Source = MyGeoShape, Path = new PropertyPath(GeoShape.MyTransformedBoundsProperty), Converter = new MyConvRectWidth() });
            //MyPanel.SetBinding(HeightProperty, new Binding() { Source = MyGeoShape, Path = new PropertyPath(GeoShape.MyTransformedBoundsProperty), Converter = new MyConvRectHeight() });


            MyPanel.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(MyGeoShapeBoundsProperty), Converter = new MyConvRectWidth() });
            MyPanel.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(MyGeoShapeBoundsProperty), Converter = new MyConvRectHeight() });


        }

        private void MyBindSetGeoShapeBounds()
        {
            var bind = new MultiBinding() { Converter = new MyConvGeoShapeBounds2() };
            bind.Bindings.Add(new Binding() { Source = MyGeoShape, Path = new PropertyPath(GeoShape.MyGeometryProperty) });
            bind.Bindings.Add(new Binding() { Source = MyGeoShape, Path = new PropertyPath(GeoShape.MyRotateTransformProperty) });
            bind.Bindings.Add(new Binding() { Source = MyGeoShape, Path = new PropertyPath(GeoShape.MyScaleTransformProperty) });
            bind.Bindings.Add(new Binding() { Source = MyGeoShape, Path = new PropertyPath(GeoShape.MyPenProperty) });
            SetBinding(MyGeoShapeBoundsProperty, bind);
        }

        private void MyBindTranslateXY()
        {
            SetBinding(MyTranslateXProperty, new Binding() { Source = MyGeoShape, Path = new PropertyPath(GeoShape.MyGeometryRenderBoundsWithPenProperty), Converter = new MyConvTranslateX() });
            SetBinding(MyTranslateYProperty, new Binding() { Source = MyGeoShape, Path = new PropertyPath(GeoShape.MyGeometryRenderBoundsWithPenProperty), Converter = new MyConvTranslateY() });
        }

        private void MyBindTransform()
        {
            var bind = new MultiBinding() { Converter = new MyConvTransform() };
            bind.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyScaleXProperty) });
            bind.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyScaleYProperty) });
            bind.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyAngleProperty) });
            bind.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyTranslateXProperty) });
            bind.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyTranslateYProperty) });
            //MyPanel.SetBinding(RenderTransformProperty, bind);
            MyGeoShape.SetBinding(RenderTransformProperty, bind);
        }

        private void MyBindOrigin()
        {
            var bind = new MultiBinding() { Converter = new MyConvPoint() };
            bind.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyCenterXProperty) });
            bind.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyCenterYProperty) });
            SetBinding(MyOriginProperty, bind);
            //MyPanel.SetBinding(RenderTransformOriginProperty, bind);
            //MyGeoShape.SetBinding(RenderTransformOriginProperty, bind);
        }

        #region 依存関係プロパティ


        public Rect MyGeoShapeBounds
        {
            get { return (Rect)GetValue(MyGeoShapeBoundsProperty); }
            set { SetValue(MyGeoShapeBoundsProperty, value); }
        }
        public static readonly DependencyProperty MyGeoShapeBoundsProperty =
            DependencyProperty.Register(nameof(MyGeoShapeBounds), typeof(Rect), typeof(GeometryThumb), new PropertyMetadata(new Rect()));

        public double MyTranslateX
        {
            get { return (double)GetValue(MyTranslateXProperty); }
            set { SetValue(MyTranslateXProperty, value); }
        }
        public static readonly DependencyProperty MyTranslateXProperty =
            DependencyProperty.Register(nameof(MyTranslateX), typeof(double), typeof(GeometryThumb), new PropertyMetadata(0.0));

        public double MyTranslateY
        {
            get { return (double)GetValue(MyTranslateYProperty); }
            set { SetValue(MyTranslateYProperty, value); }
        }
        public static readonly DependencyProperty MyTranslateYProperty =
            DependencyProperty.Register(nameof(MyTranslateY), typeof(double), typeof(GeometryThumb), new PropertyMetadata(0.0));

        public Point MyOrigin
        {
            get { return (Point)GetValue(MyOriginProperty); }
            protected set { SetValue(MyOriginProperty, value); }
        }
        public static readonly DependencyProperty MyOriginProperty =
            DependencyProperty.Register(nameof(MyOrigin), typeof(Point), typeof(GeometryThumb), new PropertyMetadata(new Point()));

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(GeometryThumb), new PropertyMetadata(null));

        public double MyAngle
        {
            get { return (double)GetValue(MyAngleProperty); }
            set { SetValue(MyAngleProperty, value); }
        }
        public static readonly DependencyProperty MyAngleProperty =
            DependencyProperty.Register(nameof(MyAngle), typeof(double), typeof(GeometryThumb), new PropertyMetadata(0.0));

        public double MyScaleX
        {
            get { return (double)GetValue(MyScaleXProperty); }
            set { SetValue(MyScaleXProperty, value); }
        }
        public static readonly DependencyProperty MyScaleXProperty =
            DependencyProperty.Register(nameof(MyScaleX), typeof(double), typeof(GeometryThumb), new PropertyMetadata(1.0));

        public double MyScaleY
        {
            get { return (double)GetValue(MyScaleYProperty); }
            set { SetValue(MyScaleYProperty, value); }
        }
        public static readonly DependencyProperty MyScaleYProperty =
            DependencyProperty.Register(nameof(MyScaleY), typeof(double), typeof(GeometryThumb), new PropertyMetadata(1.0));

        public double MyCenterX
        {
            get { return (double)GetValue(MyCenterXProperty); }
            set { SetValue(MyCenterXProperty, value); }
        }
        public static readonly DependencyProperty MyCenterXProperty =
            DependencyProperty.Register(nameof(MyCenterX), typeof(double), typeof(GeometryThumb), new PropertyMetadata(0.0));

        public double MyCenterY
        {
            get { return (double)GetValue(MyCenterYProperty); }
            set { SetValue(MyCenterYProperty, value); }
        }
        public static readonly DependencyProperty MyCenterYProperty =
            DependencyProperty.Register(nameof(MyCenterY), typeof(double), typeof(GeometryThumb), new PropertyMetadata(0.0));

        #endregion 依存関係プロパティ


        #region 初期設定

        private void GeometryThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
            Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("geo") is GeoShapeWithAnchorHandle geo)
            {
                MyGeoShape = geo;
                MyGeoShape.OnAnchorHandleDragCompleted += MyGeoShape_OnAnchorHandleDragCompleted;
            }
            if (GetTemplateChild("canvas") is Panel panel)
            {
                MyPanel = panel;
            }
        }

        //アンカーハンドルのドラッグ移動終了時
        private void MyGeoShape_OnAnchorHandleDragCompleted(DragCompletedEventArgs obj)
        {


        }

        public void AnchorSwitch()
        {
            MyGeoShape?.AnchorHandleSwitch();
        }
        #endregion 初期設定

        public void OffsetShape()
        {
            //図形オフセット
            Canvas.SetLeft(MyGeoShape, -MyGeoShapeBounds.Left);
            Canvas.SetTop(MyGeoShape, -MyGeoShapeBounds.Top);

        }

        public void OffsetThumb()
        {
            //Thumbオフセット
            var imaX = Canvas.GetLeft(this);
            var imaY = Canvas.GetTop(this);
            Canvas.SetLeft(this, imaX + MyGeoShape.MyTransformedBounds.Left);
            Canvas.SetTop(this, imaY + MyGeoShape.MyTransformedBounds.Top);
        }
        public void Offset()
        {
            OffsetShape();
            Canvas.SetLeft(this, MyGeoShape.MyTransformedBounds.Left);
            Canvas.SetTop(this, MyGeoShape.MyTransformedBounds.Top);
        }

        public void GetTransformedBounds()
        {
            var ro = MyGeoShape.MyRotateTransform; var sc = MyGeoShape.MyScaleTransform;
            var pen = MyGeoShape.MyPen;
            var geo2 = MyGeoShape.MyGeometry.Clone(); var bou = MyGeoShape.MyGeometry.Clone().Bounds;

            var bou1 = sc.TransformBounds(bou);
            var bou2 = ro.TransformBounds(bou1);
            var bou3 = ro.TransformBounds(bou);
            var bou4 = sc.TransformBounds(bou3);
            var bou50 = geo2.GetRenderBounds(pen);
            var bou51 = sc.TransformBounds(bou50);
            var bou52 = ro.TransformBounds(bou51);


            geo2.Transform = ro;
            var ropen = geo2.GetRenderBounds(pen);
            var rosc = sc.TransformBounds(ropen);

            geo2.Transform = sc;
            var scpen = geo2.GetRenderBounds(pen);
            var scro = ro.TransformBounds(scpen);

            geo2.Transform = MyGeoShape.MyRenderTransform;
            var repen = geo2.GetRenderBounds(pen);


        }
    }



    //public class MyConvGeoShapeBounds5 : IMultiValueConverter
    //{
    //    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        var geo = (Geometry)values[0];
    //        var ro = (Transform)values[1];
    //        var sc = (Transform)values[2];
    //        var pen = (Pen)values[3];
    //        var clone = geo.Clone();
    //        var rr = clone.GetRenderBounds(pen);
    //        var cb= clone.Bounds;
    //        var r = clone.GetRenderBounds(pen);
    //        return r;
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public class MyConvGeoShapeBounds4 : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var geo = (Geometry)values[0];
            var ro = (Transform)values[1];
            var sc = (Transform)values[2];
            var pen = (Pen)values[3];
            var clone = geo.Clone();
            TransformGroup tra = new();
            clone.Transform = tra;
            tra.Children.Add(sc);
            tra.Children.Add(ro);
            var r = clone.GetRenderBounds(pen);
            return r;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConvGeoShapeBounds3 : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var geo = (Geometry)values[0];
            var ro = (Transform)values[1];
            var sc = (Transform)values[2];
            var pen = (Pen)values[3];
            var clone = geo.Clone();
            clone.Transform = sc;
            var r = clone.GetRenderBounds(pen);
            r = ro.TransformBounds(r);
            return r;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConvGeoShapeBounds2 : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var geo = (Geometry)values[0];
            var ro = (Transform)values[1];
            var sc = (Transform)values[2];
            var pen = (Pen)values[3];
            var clone = geo.Clone();
            clone.Transform = ro;
            var r = clone.GetRenderBounds(pen);
            r = sc.TransformBounds(r);
            return r;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    //public class MyConvGeoShapeBounds : IMultiValueConverter
    //{
    //    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        var geo = (Geometry)values[0];
    //        var tra = (Transform)values[1];
    //        var pen = (Pen)values[2];
    //        var clone = geo.Clone();
    //        clone.Transform = tra;
    //        return clone.GetRenderBounds(pen);
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public class MyConvTranslateX : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var geo = (Rect)value;
            return -geo.X;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConvTranslateY : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var geo = (Rect)value;
            return -geo.Y;
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

    public class MyConvTransform : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var scalex = (double)values[0];
            var scaley = (double)values[1];
            var angle = (double)values[2];
            var tx = (double)values[3];
            var ty = (double)values[4];
            TransformGroup transform = new();
            //transform.Children.Add(new TranslateTransform(tx, ty));
            transform.Children.Add(new ScaleTransform(scalex, scaley));
            transform.Children.Add(new RotateTransform(angle));
            return transform;
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

}
