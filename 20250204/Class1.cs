using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _20250204
{
    class Class1
    {
    }
    public class EzLine : Shape
    {

        protected override Geometry DefiningGeometry
        {
            get
            {
                if (MyPoints == null) { return Geometry.Empty; }
                StreamGeometry geo = new() { FillRule = MyFillRule };
                using (var context = geo.Open())
                {
                    context.BeginFigure(MyPoints[0], MyIsFilled, MyIsClosed);
                    context.PolyLineTo(MySegmentPoints, MyIsStroked, MyIsSmoothJoin);
                }
                geo.Freeze();
                MyBounds = geo.Bounds;
                MyBounds2 = geo.GetRenderBounds(MyPen);
                var TFGeo = geo.Clone();
                TFGeo.Transform = RenderTransform;                
                MyBounds3 = TFGeo.Bounds;
                MyBounds4 = TFGeo.GetRenderBounds(MyPen);
                var clonetf = TFGeo.Transform;
                if(TFGeo.Transform is TransformGroup tg)
                {
                    foreach (var item in tg.Children)
                    {
                        if(item is RotateTransform ro)
                        {
                            var angle = ro.Angle;
                        }
                    }
                }
                return geo;
            }
        }
        public EzLine()
        {
            DataContext = this;
            SetBinding(MySegmentPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty), Mode = BindingMode.OneWay, Converter = new MyConverterSegmentPoints() });

            MyPenBind();
        }

        //Penのバインド、Penは図形のBoundsを計測するために必要
        private void MyPenBind()
        {
            MultiBinding mb = new() { Converter = new MyConverterPen() };
            mb.Bindings.Add(MakeOneWayBind(StrokeThicknessProperty));
            mb.Bindings.Add(MakeOneWayBind(StrokeMiterLimitProperty));
            mb.Bindings.Add(MakeOneWayBind(StrokeEndLineCapProperty));
            mb.Bindings.Add(MakeOneWayBind(StrokeStartLineCapProperty));
            mb.Bindings.Add(MakeOneWayBind(StrokeLineJoinProperty));
            SetBinding(MyPenProperty, mb);
        }
        private Binding MakeOneWayBind(DependencyProperty property)
        {
            return new Binding() { Source = this, Path = new PropertyPath(property), Mode = BindingMode.OneWay };
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            MyBounds5 = this.RenderedGeometry.GetRenderBounds(MyPen);

        }

        #region 依存関係プロパティ

        #region 通常

        public FillRule MyFillRule
        {
            get { return (FillRule)GetValue(MyFillRuleProperty); }
            set { SetValue(MyFillRuleProperty, value); }
        }
        public static readonly DependencyProperty MyFillRuleProperty =
            DependencyProperty.Register(nameof(MyFillRule), typeof(FillRule), typeof(EzLine),
                new FrameworkPropertyMetadata(FillRule.EvenOdd,
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
        #endregion 通常

        #region 読み取り専用

        //線描画のBoundsをOnRenderで取得したもの、MyBounds3と同じ値
        //this.RenderedGeometry.GetRenderBounds(MyPen);
        public Rect MyBounds5
        {
            get { return (Rect)GetValue(MyBounds5Property); }
            private set { SetValue(MyBounds5Property, value); }
        }
        public static readonly DependencyProperty MyBounds5Property =
            DependencyProperty.Register(nameof(MyBounds5), typeof(Rect), typeof(EzLine), new PropertyMetadata(new Rect()));

        //変形後の線描画のBounds
        //TFGeo.GetRenderBounds(MyPen);
        public Rect MyBounds4
        {
            get { return (Rect)GetValue(MyBounds4Property); }
            private set { SetValue(MyBounds4Property, value); }
        }
        public static readonly DependencyProperty MyBounds4Property =
            DependencyProperty.Register(nameof(MyBounds4), typeof(Rect), typeof(EzLine), new PropertyMetadata(new Rect()));

        //変形後のGeometryBounds
        //TFGeo.Bounds;
        public Rect MyBounds3
        {
            get { return (Rect)GetValue(MyBounds3Property); }
            private set { SetValue(MyBounds3Property, value); }
        }
        public static readonly DependencyProperty MyBounds3Property =
            DependencyProperty.Register(nameof(MyBounds3), typeof(Rect), typeof(EzLine), new PropertyMetadata(new Rect()));

        public Pen MyPen
        {
            get { return (Pen)GetValue(MyPenProperty); }
            private set { SetValue(MyPenProperty, value); }
        }
        public static readonly DependencyProperty MyPenProperty =
            DependencyProperty.Register(nameof(MyPen), typeof(Pen), typeof(EzLine), new PropertyMetadata(new Pen()));

        //線描画のBounds
        //GetRenderBounds(MyPen);
        public Rect MyBounds2
        {
            get { return (Rect)GetValue(MyBounds2Property); }
            private set { SetValue(MyBounds2Property, value); }
        }
        public static readonly DependencyProperty MyBounds2Property =
            DependencyProperty.Register(nameof(MyBounds2), typeof(Rect), typeof(EzLine), new PropertyMetadata(new Rect()));

        //GeometryBounds
        public Rect MyBounds
        {
            get { return (Rect)GetValue(MyBoundsProperty); }
            private set { SetValue(MyBoundsProperty, value); }
        }
        public static readonly DependencyProperty MyBoundsProperty =
            DependencyProperty.Register(nameof(MyBounds), typeof(Rect), typeof(EzLine), new PropertyMetadata(new Rect()));

        public PointCollection MySegmentPoints
        {
            get { return (PointCollection)GetValue(MySegmentPointsProperty); }
            private set { SetValue(MySegmentPointsProperty, value); }
        }


        public static readonly DependencyProperty MySegmentPointsProperty =
            DependencyProperty.Register(nameof(MySegmentPoints), typeof(PointCollection), typeof(EzLine), new PropertyMetadata(null));

        #endregion 読み取り専用

        #endregion 依存関係プロパティ



    }


    #region コンバーター
    public class MyConverterPen : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var thick = (double)values[0];
            var miter = (double)values[1];
            var end = (PenLineCap)values[2];
            var sta = (PenLineCap)values[3];
            var join = (PenLineJoin)values[4];
            Pen result = new Pen(Brushes.Transparent, thick);
            result.EndLineCap = end;
            result.StartLineCap = sta;
            result.LineJoin = join;
            result.MiterLimit = miter;
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConverterSegmentPoints : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PointCollection pc)
            {
                var clone = pc.Clone();
                clone.RemoveAt(0);
                return clone;
            }
            else
            {
                return new PointCollection();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion コンバーター


}
