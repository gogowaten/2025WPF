using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace _20250203
{
    internal class Class1
    {
    }


    public class EzLine : Shape
    {
        #region 依存関係プロパティ
        // FrameworkPropertyMetadataOptions.AffectsRender // デザイン画面上での更新で必要
        // FrameworkPropertyMetadataOptions.AffectsMeasure // 必要ないかも？


        #region 基本


        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(EzLine),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool MyIsFilled
        {
            get { return (bool)GetValue(MyIsFilledProperty); }
            set { SetValue(MyIsFilledProperty, value); }
        }
        public static readonly DependencyProperty MyIsFilledProperty =
            DependencyProperty.Register(nameof(MyIsFilled), typeof(bool), typeof(EzLine), new FrameworkPropertyMetadata(true,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        public bool MyIsStroked
        {
            get { return (bool)GetValue(MyIsStrokedProperty); }
            set { SetValue(MyIsStrokedProperty, value); }
        }
        public static readonly DependencyProperty MyIsStrokedProperty =
            DependencyProperty.Register(nameof(MyIsStroked), typeof(bool), typeof(EzLine), new FrameworkPropertyMetadata(true,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        public bool MyIsClosed
        {
            get { return (bool)GetValue(MyIsClosedProperty); }
            set { SetValue(MyIsClosedProperty, value); }
        }
        public static readonly DependencyProperty MyIsClosedProperty =
            DependencyProperty.Register(nameof(MyIsClosed), typeof(bool), typeof(EzLine), new FrameworkPropertyMetadata(false,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        public bool MyIsSmoothJoin
        {
            get { return (bool)GetValue(MyIsSmoothJoinProperty); }
            set { SetValue(MyIsSmoothJoinProperty, value); }
        }
        public static readonly DependencyProperty MyIsSmoothJoinProperty =
            DependencyProperty.Register(nameof(MyIsSmoothJoin), typeof(bool), typeof(EzLine),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public FillRule MyFillRule
        {
            get { return (FillRule)GetValue(MyFillRuleProperty); }
            set { SetValue(MyFillRuleProperty, value); }
        }
        public static readonly DependencyProperty MyFillRuleProperty =
            DependencyProperty.Register(nameof(MyFillRule), typeof(FillRule), typeof(EzLine),
                new FrameworkPropertyMetadata(FillRule.Nonzero,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public Pen MyPen
        {
            get { return (Pen)GetValue(MyPenProperty); }
            set { SetValue(MyPenProperty, value); }
        }
        public static readonly DependencyProperty MyPenProperty =
            DependencyProperty.Register(nameof(MyPen), typeof(Pen), typeof(EzLine), new FrameworkPropertyMetadata(new Pen(),
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        public double MyAngle
        {
            get { return (double)GetValue(MyAngleProperty); }
            set { SetValue(MyAngleProperty, value); }
        }
        public static readonly DependencyProperty MyAngleProperty =
            DependencyProperty.Register(nameof(MyAngle), typeof(double), typeof(EzLine),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(OnMyAnglePropertyChanged)));

        private static void OnMyAnglePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EzLine ez)
            {
                var tf = ez.RenderTransform;
                if (ez.RenderTransform is RotateTransform ro)
                {
                    ro.Angle = (double)e.NewValue;
                }
            }
        }


        public RotateTransform MyRotateTransform
        {
            get { return (RotateTransform)GetValue(MyRotateTransformProperty); }
            set { SetValue(MyRotateTransformProperty, value); }
        }
        public static readonly DependencyProperty MyRotateTransformProperty =
            DependencyProperty.Register(nameof(MyRotateTransform), typeof(RotateTransform), typeof(EzLine), new PropertyMetadata(new RotateTransform()));





        #endregion 基本

        #endregion 依存関係プロパティ

        #region 読み取り専用依存関係プロパティ


        public Geometry MyGeometry
        {
            get { return (Geometry)GetValue(MyGeometryProperty); }
            set { SetValue(MyGeometryProperty, value); }
        }
        public static readonly DependencyProperty MyGeometryProperty =
            DependencyProperty.Register(nameof(MyGeometry), typeof(Geometry), typeof(EzLine), new PropertyMetadata(Geometry.Empty));

        //Geometryを回転してからのPen付きRect
        public Rect MyGeometryRotatePenBounds
        {
            get { return (Rect)GetValue(MyGeometryRotatePenBoundsProperty); }
            set { SetValue(MyGeometryRotatePenBoundsProperty, value); }
        }
        public static readonly DependencyProperty MyGeometryRotatePenBoundsProperty =
            DependencyProperty.Register(nameof(MyGeometryRotatePenBounds), typeof(Rect), typeof(EzLine), new PropertyMetadata(Rect.Empty));

        //未使用、GeometryのPen付きRectを回転したRect
        public Rect MyGeometryRenderRotateBounds
        {
            get { return (Rect)GetValue(MyGeometryRenderRotateBoundsProperty); }
            private set { SetValue(MyGeometryRenderRotateBoundsProperty, value); }
        }
        public static readonly DependencyProperty MyGeometryRenderRotateBoundsProperty =
            DependencyProperty.Register(nameof(MyGeometryRenderRotateBounds), typeof(Rect), typeof(EzLine), new PropertyMetadata(Rect.Empty));


        //GeometryのPen付きRect
        public Rect MyGeometryRenderBounds
        {
            get { return (Rect)GetValue(MyGeometryRenderBoundsProperty); }
            private set { SetValue(MyGeometryRenderBoundsProperty, value); }
        }
        public static readonly DependencyProperty MyGeometryRenderBoundsProperty =
            DependencyProperty.Register(nameof(MyGeometryRenderBounds), typeof(Rect), typeof(EzLine), new PropertyMetadata(Rect.Empty));

        //GeometryのPenなしRect
        public Rect MyGeometryBounds
        {
            get { return (Rect)GetValue(MyGeometryBoundsProperty); }
            private set { SetValue(MyGeometryBoundsProperty, value); }
        }
        public static readonly DependencyProperty MyGeometryBoundsProperty =
            DependencyProperty.Register(nameof(MyGeometryBounds), typeof(Rect), typeof(EzLine), new PropertyMetadata(Rect.Empty));


        public PointCollection MySegmentPoints
        {
            get { return (PointCollection)GetValue(MySegmentPointsProperty); }
            private set { SetValue(MySegmentPointsProperty, value); }
        }
        public static readonly DependencyProperty MySegmentPointsProperty =
            DependencyProperty.Register(nameof(MySegmentPoints), typeof(PointCollection), typeof(EzLine), new PropertyMetadata(new PointCollection()));

        #endregion 読み取り専用依存関係プロパティ


        protected override Geometry DefiningGeometry
        {
            get
            {
                if (MyPoints == null || MyPoints.Count == 0) { return Geometry.Empty; }
                StreamGeometry geo = new();
                using (var context = geo.Open())
                {
                    context.BeginFigure(MyPoints[0], MyIsFilled, MyIsClosed);
                    context.PolyLineTo(MySegmentPoints, MyIsStroked, MyIsSmoothJoin);
                }
                geo.Freeze();
                MyGeometry = geo;
                MyGeometryBounds = geo.Bounds;
                MyGeometryRenderBounds = geo.GetRenderBounds(MyPen);
                //MyGeometryRenderRotateBounds = RenderTransform.TransformBounds(MyGeometryRenderBounds);
                return geo;
            }
        }




        static EzLine()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EzLine), new FrameworkPropertyMetadata(typeof(EzLine)));
        }
        public EzLine()
        {
            DataContext = this;

            Loaded += EzLine_Loaded;
            MyBindSet();
        }

        private void EzLine_Loaded(object sender, RoutedEventArgs e)
        {

            var x = MyGeometryRotatePenBounds.Width / 2.0;
            var y = MyGeometryRotatePenBounds.Height / 2.0;
            
            //var x = MyGeometryRenderBounds.Width / 2.0;
            //var y = MyGeometryRenderBounds.Height / 2.0;
            //var x = MyGeometryBounds.Width / 2.0;
            //var y = MyGeometryBounds.Height / 2.0;

            RenderTransform = new RotateTransform(MyAngle, x, y);
        }

        private void MyBindSet()
        {

            SetBinding(MyPenProperty, new Binding() { Source = this, Path = new PropertyPath(StrokeThicknessProperty), Mode = BindingMode.OneWay, Converter = new MyConverterPen() });
            SetBinding(MySegmentPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty), Mode = BindingMode.OneWay, Converter = new MyConverterMakeSegmentPoints() });

            //MyGeometryRotatePenBooundsProperty
            //Geometryを回転してからPen付きのRect
            MultiBinding multi = new() { Converter = new MyConverterGeometryRotatePenBounds() };
            multi.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyGeometryProperty), Mode = BindingMode.OneWay });
            multi.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyPenProperty), Mode = BindingMode.OneWay });
            multi.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(RenderTransformProperty), Mode = BindingMode.OneWay });
            SetBinding(MyGeometryRotatePenBoundsProperty, multi);


            //回転角度

            ////3
            //MultiBinding mb = new() { Converter = new MyConverterAngle3() };
            //mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyAngleProperty), Mode = BindingMode.OneWay });
            //mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty), Mode = BindingMode.OneWay });
            //SetBinding(RenderTransformProperty, mb);

            ////2-2
            //MultiBinding mb = new() { Converter = new MyConverterAngle2() };
            //mb.Bindings.Add(new Binding() { Source=this,Path=new PropertyPath(MyAngleProperty), Mode = BindingMode.OneWay });
            //mb.Bindings.Add(new Binding() { Source=this,Path=new PropertyPath(MyGeometryRenderBoundsProperty), Mode = BindingMode.OneWay });
            //SetBinding(RenderTransformProperty, mb);

            ////2-1
            //MultiBinding mb = new() { Converter = new MyConverterAngle2() };
            //mb.Bindings.Add(new Binding() { Source=this,Path=new PropertyPath(MyAngleProperty), Mode = BindingMode.OneWay });
            //mb.Bindings.Add(new Binding() { Source=this,Path=new PropertyPath(MyGeometryBoundsProperty), Mode = BindingMode.OneWay });
            //SetBinding(RenderTransformProperty, mb);

            //1
            //SetBinding(RenderTransformProperty, new Binding() { Source = this, Path = new PropertyPath(MyAngleProperty), Mode = BindingMode.OneWay, Converter = new MyConverterAngle() });

            //MultiBinding mb = new() { Converter = new MyConverterRotateTF() };
            //mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyIsRotateCenterAverageProperty), Mode = BindingMode.OneWay });
            //mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyAngleProperty), Mode = BindingMode.OneWay });
            //mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyRectProperty), Mode = BindingMode.OneWay });
            //mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty), Mode = BindingMode.OneWay });
            //SetBinding(RenderTransformProperty, mb);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

        }
    }





    #region 図形用コンバーター

    public class MyConverterAngle3 : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var angle = (double)values[0];
            if (values[1] is PointCollection pc)
            {
                double x = 0, y = 0;
                foreach (var item in pc)
                {
                    x += item.X;
                    y += item.Y;
                }
                return new RotateTransform(angle, x / pc.Count, y / pc.Count);
            }
            else { return Transform.Identity; }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConverterAngle2 : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var angle = (double)values[0];
            var re = (Rect)values[1];
            return new RotateTransform(angle, re.Width / 2.0, re.Height / 2.0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class MyConverterAngle : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var angle = (double)value;
            return new RotateTransform(angle);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConverterGeometryRotatePenBounds : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var geo = (Geometry)values[0];
            var p = (Pen)values[1];
            if (values[2] is RotateTransform rotate)
            {
                var clone = geo.Clone();
                clone.Transform = rotate;
                return clone.GetRenderBounds(p);

            }
            else
            {
                return Rect.Empty;
            }

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MyConverterMakeSegmentPoints : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PointCollection pc)
            {

                if (pc.Count == 0) { return DependencyProperty.UnsetValue; }
                var clone = pc.Clone();
                clone.RemoveAt(0);
                return clone;
            }
            else { return new PointCollection(); }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 太さから透明色のPenを作って返す
    /// </summary>
    public class MyConverterPen : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double thickness = (double)value;
            return new Pen(Brushes.Transparent, thickness);
        }

        //こっちは未使用になるはず        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Pen pen = (Pen)value;
            return pen.Thickness;
            //throw new NotImplementedException();
        }
    }
    #endregion 図形用コンバーター

}
