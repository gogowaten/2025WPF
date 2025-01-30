using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace _20250130_CenterRotateEzLine
{
    class Class1
    {
    }

    public class EzLine : Shape
    {

        public EzLine()
        {

            MultiBinding mb = new() { Converter = new MyConverterRotateTF() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyIsRotateCenterAverageProperty), Mode = BindingMode.OneWay });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyAngleProperty), Mode = BindingMode.OneWay });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyRectProperty), Mode = BindingMode.OneWay });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty), Mode = BindingMode.OneWay });
            SetBinding(RenderTransformProperty, mb);
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                if (MyPoints.Count == 0)
                {
                    return Geometry.Empty;
                }
                StreamGeometry geo = new();
                using (var context = geo.Open())
                {
                    context.BeginFigure(MyPoints[0], isFilled: MyIsFilled, isClosed: MyIsClosed);
                    PointCollection pc = new(MyPoints.Clone());
                    pc.RemoveAt(0);
                    context.PolyLineTo(pc, isStroked: MyIsStroked, isSmoothJoin: MyIsSmoothJoin);
                }
                geo.Freeze();

                return geo;
            }
        }

        //描画直後に
        //全頂点が収まる矩形を取得
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            MyBounds = RenderedGeometry.Bounds;
        }

        #region 依存関係プロパティ


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

        public bool MyIsFilled
        {
            get { return (bool)GetValue(MyIsFilledProperty); }
            set { SetValue(MyIsFilledProperty, value); }
        }
        public static readonly DependencyProperty MyIsFilledProperty =
            DependencyProperty.Register(nameof(MyIsFilled), typeof(bool), typeof(EzLine),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public bool MyIsClosed
        {
            get { return (bool)GetValue(MyIsClosedProperty); }
            set { SetValue(MyIsClosedProperty, value); }
        }
        public static readonly DependencyProperty MyIsClosedProperty =
            DependencyProperty.Register(nameof(MyIsClosed), typeof(bool), typeof(EzLine),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool MyIsStroked
        {
            get { return (bool)GetValue(MyIsStrokedProperty); }
            set { SetValue(MyIsStrokedProperty, value); }
        }
        public static readonly DependencyProperty MyIsStrokedProperty =
            DependencyProperty.Register(nameof(MyIsStroked), typeof(bool), typeof(EzLine),
                new FrameworkPropertyMetadata(true,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 依存関係プロパティ


        #region 必須依存関係プロパティ

        //回転の中心の指定
        //trueで平均座標
        //falseで全頂点が収まる矩形の中心
        public bool MyIsRotateCenterAverage
        {
            get { return (bool)GetValue(MyIsRotateCenterAverageProperty); }
            set { SetValue(MyIsRotateCenterAverageProperty, value); }
        }
        public static readonly DependencyProperty MyIsRotateCenterAverageProperty =
            DependencyProperty.Register(nameof(MyIsRotateCenterAverage), typeof(bool), typeof(EzLine),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public double MyAngle
        {
            get { return (double)GetValue(MyAngleProperty); }
            set { SetValue(MyAngleProperty, value); }
        }
        public static readonly DependencyProperty MyAngleProperty =
            DependencyProperty.Register(nameof(MyAngle), typeof(double), typeof(EzLine),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //全頂点が収まる矩形を保持
        private static readonly DependencyPropertyKey MyBoundsPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyBounds), typeof(Rect), typeof(EzLine), new PropertyMetadata(new Rect()));
        public static readonly DependencyProperty MyRectProperty = MyBoundsPropertyKey.DependencyProperty;
        public Rect MyBounds
        {
            get { return (Rect)GetValue(MyBoundsPropertyKey.DependencyProperty); }
            internal set { SetValue(MyBoundsPropertyKey, value); }
        }


        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(EzLine),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 必須依存関係プロパティ

    }





    //回転の中心座標を計算して
    //RotateTransformに変換
    public class MyConverterRotateTF : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var isCenterAverage = (bool)values[0];
            var angle = (double)values[1];
            var r = (Rect)values[2];
            var pc = (PointCollection)values[3];

            Point centerP;

            if (isCenterAverage)
            {
                //平均座標
                double x = 0, y = 0;
                foreach (var item in pc)
                {
                    x += item.X;
                    y += item.Y;
                }
                centerP = new(x / pc.Count, y / pc.Count);
            }
            else
            {
                //全頂点が収まる矩形の中心
                centerP = new Point(r.Width / 2.0, r.Height / 2.0);
            }
            return new RotateTransform(angle, centerP.X, centerP.Y);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
