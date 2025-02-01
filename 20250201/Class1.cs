using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Globalization;

namespace _20250201
{
    internal class Class1
    {

    }


    public class EzLine : Shape
    {
        #region 依存関係プロパティ
        // FrameworkPropertyMetadataOptions.AffectsRender // デザイン画面上での更新で必要
        // FrameworkPropertyMetadataOptions.AffectsMeasure // 必要ないかも？


        //線の描画を考慮したBoundsを使ってオフセット表示をする
        public bool MyIsOffset
        {
            get { return (bool)GetValue(MyIsOffsetProperty); }
            set { SetValue(MyIsOffsetProperty, value); }
        }
        public static readonly DependencyProperty MyIsOffsetProperty =
            DependencyProperty.Register(nameof(MyIsOffset), typeof(bool), typeof(EzLine), new FrameworkPropertyMetadata(false,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));

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

        public double MyStrokeThickness
        {
            get { return (double)GetValue(MyStrokeThicknessProperty); }
            set { SetValue(MyStrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeThicknessProperty =
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(EzLine), new FrameworkPropertyMetadata(1.0,
                FrameworkPropertyMetadataOptions.AffectsRender |// デザイン画面上での更新で必要
                FrameworkPropertyMetadataOptions.AffectsMeasure)); // 必要ないかも？

        public Brush MyStroke
        {
            get { return (Brush)GetValue(MyStrokeProperty); }
            set { SetValue(MyStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeProperty =
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(EzLine), new FrameworkPropertyMetadata(Brushes.Magenta,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        public Brush MyFill
        {
            get { return (Brush)GetValue(MyFillProperty); }
            set { SetValue(MyFillProperty, value); }
        }
        public static readonly DependencyProperty MyFillProperty =
            DependencyProperty.Register(nameof(MyFill), typeof(Brush), typeof(EzLine), new FrameworkPropertyMetadata(Brushes.Pink,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));

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



        #endregion 依存関係プロパティ

        #region 読み取り専用依存関係プロパティ

        //図形本体のPath
        private static readonly DependencyPropertyKey MyPathPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyPath), typeof(Path), typeof(EzLine), new PropertyMetadata(null));
        public static readonly DependencyProperty MyPathProperty = MyPathPropertyKey.DependencyProperty;
        public Path MyPath
        {
            get { return (Path)GetValue(MyPathPropertyKey.DependencyProperty); }
            internal set { SetValue(MyPathPropertyKey, value); }
        }

        /// <summary>
        /// Pen(Stroke)の太さを考慮した位置とサイズ
        /// </summary>
        private static readonly DependencyPropertyKey MyBoundsWithPenPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyBoundsWithPen), typeof(Rect), typeof(EzLine), new PropertyMetadata(new Rect()));
        public static readonly DependencyProperty MyBoundsWithPenProperty = MyBoundsWithPenPropertyKey.DependencyProperty;
        public Rect MyBoundsWithPen
        {
            get { return (Rect)GetValue(MyBoundsWithPenPropertyKey.DependencyProperty); }
            internal set { SetValue(MyBoundsWithPenPropertyKey, value); }
        }


        public PointCollection MySegmentPoints
        {
            get { return (PointCollection)GetValue(MySegmentPointsProperty); }
            set { SetValue(MySegmentPointsProperty, value); }
        }
        public static readonly DependencyProperty MySegmentPointsProperty =
            DependencyProperty.Register(nameof(MySegmentPoints), typeof(PointCollection), typeof(EzLine), new PropertyMetadata(new PointCollection()));

        //public PointCollection MySegmentPoints
        //{
        //    get { return (PointCollection)GetValue(MySegmentPointsProperty); }
        //    set { SetValue(MySegmentPointsProperty, value); }
        //}
        //public static readonly DependencyProperty MySegmentPointsProperty =
        //    DependencyProperty.Register(nameof(MySegmentPoints), typeof(PointCollection), typeof(EzLine),
        //        new FrameworkPropertyMetadata(null,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



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
                return geo;
            }
        }


        #endregion 読み取り専用依存関係プロパティ

        static EzLine()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EzLine), new FrameworkPropertyMetadata(typeof(EzLine)));
        }
        public EzLine()
        {
            DataContext = this;
            Loaded += EzLine_Loaded;
        }

        private void EzLine_Loaded(object sender, RoutedEventArgs e)
        {
            var neko = MyPoints;
            BindingOperations.SetBinding(this, MySegmentPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty), Converter = new MyConverterMakeSegmentPoints(), Mode = BindingMode.OneWay });
            BindingOperations.SetBinding(this, MyPenProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeThicknessProperty), Converter = new MyConverterPen(), Mode = BindingMode.OneWay });
            //RelativeSource source = new(RelativeSourceMode.Self);
            
            //RelativeSource source = new(RelativeSourceMode.TemplatedParent);
            
            
            SetBinding(StrokeProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeProperty) });
            SetBinding(StrokeThicknessProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeThicknessProperty) });
            SetBinding(FillProperty, new Binding() { Source = this, Path = new PropertyPath(MyFillProperty) });

        }

        //起動中のTemplate構築時に中のPathを取得
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //if (GetTemplateChild("PART_Path") is Path path)
            //{
            //    MyPath = path;
            //}
        }

        //描画直後に線(Pen)の描画を考慮した位置とサイズの取得
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            Rect r=DefiningGeometry.GetRenderBounds(MyPen);
            if (r.Width > 0) { MyBoundsWithPen = r; }            
        }
    }


    public class MyConverterMakeSegmentPoints : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var pc = (PointCollection)value;
            if (pc.Count == 0) { return DependencyProperty.UnsetValue; }
            var clone = pc.Clone();
            clone.RemoveAt(0);
            return clone;
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



}
