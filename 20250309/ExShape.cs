using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Security.Cryptography.Xml;


namespace _20250309
{
    public abstract class EzShape : Shape
    {
        public EzShape()
        {
            DataContext = this;
            SetBinding(MySegmentPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty), Converter = new MyConverterSegmentPoints() });
            MyPenBind();
        }

        public abstract void AddPoint(Point point);


        private void MyPenBind()
        {
            //Penのバインド、Penは図形のBoundsを計測するために必要
            MultiBinding mb = new() { Converter = new MyConverterPen() };
            mb.Bindings.Add(MakeOneWayBind(StrokeThicknessProperty));
            mb.Bindings.Add(MakeOneWayBind(StrokeMiterLimitProperty));
            mb.Bindings.Add(MakeOneWayBind(StrokeEndLineCapProperty));
            mb.Bindings.Add(MakeOneWayBind(StrokeStartLineCapProperty));
            mb.Bindings.Add(MakeOneWayBind(StrokeLineJoinProperty));
            SetBinding(MyPenProperty, mb);

            //回転、RotateTransform
            SetBinding(RenderTransformProperty, new Binding() { Source = this, Path = new PropertyPath(MyAngleProperty), Converter = new MyConvRotateTransform() });
        }
        private Binding MakeOneWayBind(DependencyProperty property)
        {
            return new Binding() { Source = this, Path = new PropertyPath(property), Mode = BindingMode.OneWay };
        }



        #region 依存関係プロパティ

        ////図形のアンカーハンドルThumb表示用のAdorner
        //public EzShapeAdorner? MyEzShapeAdorner
        //{
        //    get { return (EzShapeAdorner)GetValue(MyEzShapeAdornerProperty); }
        //    protected set { SetValue(MyEzShapeAdornerProperty, value); }
        //}
        //public static readonly DependencyProperty MyEzShapeAdornerProperty =
        //    DependencyProperty.Register(nameof(MyEzShapeAdorner), typeof(EzShapeAdorner), typeof(EzShape), new PropertyMetadata(null));

        #region 必須

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(EzShape),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion 必須


        #region 通常

        #region 回転

        public double MyAngle
        {
            get { return (double)GetValue(MyAngleProperty); }
            set { SetValue(MyAngleProperty, value); }
        }
        public static readonly DependencyProperty MyAngleProperty =
            DependencyProperty.Register(nameof(MyAngle), typeof(double), typeof(EzShape),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        #endregion 回転

        public FillRule MyFillRule
        {
            get { return (FillRule)GetValue(MyFillRuleProperty); }
            set { SetValue(MyFillRuleProperty, value); }
        }
        public static readonly DependencyProperty MyFillRuleProperty =
            DependencyProperty.Register(nameof(MyFillRule), typeof(FillRule), typeof(EzShape),
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
            DependencyProperty.Register(nameof(MyIsSmoothJoin), typeof(bool), typeof(EzShape),
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
            DependencyProperty.Register(nameof(MyIsStroked), typeof(bool), typeof(EzShape),
                new FrameworkPropertyMetadata(true,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public bool MyIsFilled
        {
            get { return (bool)GetValue(MyIsFilledProperty); }
            set { SetValue(MyIsFilledProperty, value); }
        }
        public static readonly DependencyProperty MyIsFilledProperty =
            DependencyProperty.Register(nameof(MyIsFilled), typeof(bool), typeof(EzShape),
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
            DependencyProperty.Register(nameof(MyIsClosed), typeof(bool), typeof(EzShape),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion 通常

        #region protected setな依存関係プロパティ


        //変形後の線描画のBounds
        //TFGeo.GetRenderBounds(MyPen);
        public Rect MyBounds4
        {
            get { return (Rect)GetValue(MyBounds4Property); }
            protected set { SetValue(MyBounds4Property, value); }
        }
        public static readonly DependencyProperty MyBounds4Property =
            DependencyProperty.Register(nameof(MyBounds4), typeof(Rect), typeof(EzShape), new PropertyMetadata(new Rect()));

        //変形後のGeometryBounds
        //TFGeo.Bounds;
        public Rect MyBounds3
        {
            get { return (Rect)GetValue(MyBounds3Property); }
            protected set { SetValue(MyBounds3Property, value); }
        }
        public static readonly DependencyProperty MyBounds3Property =
            DependencyProperty.Register(nameof(MyBounds3), typeof(Rect), typeof(EzShape), new PropertyMetadata(new Rect()));


        //線描画のBounds
        //GetRenderBounds(MyPen);
        public Rect MyBounds2
        {
            get { return (Rect)GetValue(MyBounds2Property); }
            protected set { SetValue(MyBounds2Property, value); }
        }
        public static readonly DependencyProperty MyBounds2Property =
            DependencyProperty.Register(nameof(MyBounds2), typeof(Rect), typeof(EzShape), new PropertyMetadata(new Rect()));

        //GeometryBounds
        public Rect MyBounds1
        {
            get { return (Rect)GetValue(MyBounds1Property); }
            protected set { SetValue(MyBounds1Property, value); }
        }
        public static readonly DependencyProperty MyBounds1Property =
            DependencyProperty.Register(nameof(MyBounds1), typeof(Rect), typeof(EzShape), new PropertyMetadata(new Rect()));

        public Pen MyPen
        {
            get { return (Pen)GetValue(MyPenProperty); }
            protected set { SetValue(MyPenProperty, value); }
        }
        public static readonly DependencyProperty MyPenProperty =
            DependencyProperty.Register(nameof(MyPen), typeof(Pen), typeof(EzShape), new PropertyMetadata(new Pen()));

        public PointCollection MySegmentPoints
        {
            get { return (PointCollection)GetValue(MySegmentPointsProperty); }
            protected set { SetValue(MySegmentPointsProperty, value); }
        }
        public static readonly DependencyProperty MySegmentPointsProperty =
            DependencyProperty.Register(nameof(MySegmentPoints), typeof(PointCollection), typeof(EzShape), new PropertyMetadata(null));

        #endregion privete setな依存関係プロパティ


        #endregion 依存関係プロパティ

        /// <summary>
        /// アンカーハンドルの表示切替
        /// </summary>
        public void AdornerSwitch()
        {
            if (AdornerLayer.GetAdornerLayer(this) is AdornerLayer layer)
            {
                Adorner[] ados = layer.GetAdorners(this);
                //無ければ追加(表示)
                if (ados is null)
                {
                    layer.Add(new EzShapeAdorner(this));
                }
                //在れば削除
                else
                {
                    foreach (var item in ados.OfType<EzShapeAdorner>())
                    {
                        layer.Remove(item);
                    }
                }
            }
        }


    }



    public class EzBezier : EzShape
    {
        protected override Geometry DefiningGeometry
        {
            get
            {
                if (MyPoints == null) { return Geometry.Empty; }
                StreamGeometry geo = new() { FillRule = FillRule.Nonzero };
                using (var context = geo.Open())
                {
                    context.BeginFigure(MyPoints[0], isFilled: false, isClosed: false);
                    context.PolyBezierTo(MySegmentPoints, isStroked: true, isSmoothJoin: false);
                }
                geo.Freeze();


                //Boundsの更新はここで行う必要がある。OnRenderではなんか違う
                MyBounds1 = geo.Bounds;
                MyBounds2 = geo.GetRenderBounds(MyPen);
                //回転後のBounds
                var clone = geo.Clone();
                clone.Transform = RenderTransform;
                MyBounds3 = clone.Bounds;
                MyBounds4 = clone.GetRenderBounds(MyPen);

                return geo;
            }
        }

        public EzBezier()
        {
            DataContext = this;
            //MyPoints = [new Point(), new Point(100, 0), new Point(100, 100), new Point(0, 100)];
            Stroke = Brushes.DodgerBlue;
            StrokeThickness = 60.0;


        }

        public override void AddPoint(Point point)
        {
            MyPoints.Add(MyPoints[^1]);
            MyPoints.Add(point);
            MyPoints.Add(point);

        }
    }


    #region 専用Adorner

    public class EzShapeAdorner : Adorner
    {
        private readonly TwoColorDashLine MyControlLine;//制御線
        public readonly List<Thumb> MyAnchorThumbsList;//アンカーハンドルThumb
        private readonly Canvas MyCanvas;
        private readonly VisualCollection MyVisualCollection;
        private readonly EzShape MyTarget;//装飾ターゲット

        #region VisualCollectionで必要        
        protected override int VisualChildrenCount => MyVisualCollection.Count;

        protected override Visual GetVisualChild(int index) => MyVisualCollection[index];
        #endregion VisualCollectionで必要

        public EzShapeAdorner(EzShape adornedElement) : base(adornedElement)
        {
            MyCanvas = new();
            MyAnchorThumbsList = [];
            MyTarget = adornedElement;
            MyVisualCollection = new(this) { MyCanvas };
            //制御線追加、アンカーハンドルより下側に表示したいのでzindexを-1指定
            MyControlLine = new()
            {
                MyPoints = MyTarget.MyPoints,
            };
            Panel.SetZIndex(MyControlLine, -1);
            MyCanvas.Children.Add(MyControlLine);

            //アンカーハンドルの作成追加
            InitAnchorThumbs();

            //アンカーハンドルの位置をMyPointsに合わせる
            //ResetAnchorLocate();

        }


        private void InitAnchorThumbs()
        {
            for (int i = 0; i < MyTarget.MyPoints.Count; i++)
            {
                AddAnchorThumb(MyTarget.MyPoints[i], i);
            }
        }
        private void AddAnchorThumb(Point point, int id)
        {
            Thumb anchor = new()
            {
                Cursor = Cursors.Hand,
                Height = AnchorSize,
                Width = AnchorSize,
                Opacity = 0.3,
                Background = Brushes.Red,
                Tag = id
            };
            Canvas.SetLeft(anchor, point.X - AnchorSize / 2.0);
            Canvas.SetTop(anchor, point.Y - AnchorSize / 2.0);
            MyAnchorThumbsList.Insert(id, anchor);
            MyCanvas.Children.Insert(id, anchor);
        }

        //アンカーハンドルの位置をMyPointsに合わせる
        public void ResetAnchorLocate()
        {
            if (MyTarget.MyPoints.Count == MyAnchorThumbsList.Count)
            {
                for (int i = 0; i < MyTarget.MyPoints.Count; i++)
                {
                    Point p = MyTarget.MyPoints[i];
                    Canvas.SetLeft(MyAnchorThumbsList[i], p.X - AnchorSize / 2.0);
                    Canvas.SetTop(MyAnchorThumbsList[i], p.Y - AnchorSize / 2.0);
                }
            }
            else
            {
                MyAnchorThumbsList.Clear();
                for (int i = 0; i < MyTarget.MyPoints.Count; i++)
                {
                    AddAnchorThumb(MyTarget.MyPoints[i], i);
                }
            }
        }

        public double AnchorSize
        {
            get { return (double)GetValue(AnchorSizeProperty); }
            set { SetValue(AnchorSizeProperty, value); }
        }
        public static readonly DependencyProperty AnchorSizeProperty =
            DependencyProperty.Register(nameof(AnchorSize), typeof(double), typeof(EzShapeAdorner), new PropertyMetadata(20.0));


        protected override Size ArrangeOverride(Size finalSize)
        {
            MyCanvas.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            return base.ArrangeOverride(finalSize);
        }



        public Rect GetAnchorThumbBounds()
        {
            //Pointsを変形
            PointCollection tempPc = [];
            foreach (var item in MyTarget.MyPoints)
            {
                tempPc.Add(MyTarget.RenderTransform.Transform(item));
            }

            //各アンカーハンドルのRectを作成して
            //RectのUnionメソッドを利用すれば、
            //すべてのアンカーハンドルが収まるRectが作成できる
            double halfHandle = AnchorSize / 2.0;//アンカーポイントの中心位置
            Point p = tempPc[0];
            Rect r = new(p.X - halfHandle, p.Y - halfHandle, AnchorSize, AnchorSize);
            foreach (var item in tempPc)
            {
                Rect pr = new(item.X - halfHandle, item.Y - halfHandle, AnchorSize, AnchorSize);
                r.Union(pr);
            }
            return r;
        }
    }




    #endregion 専用Adorner


    //[028722]ベジエ曲線の各部の名称
    //https://support.justsystems.com/faq/1032/app/servlet/qadoc?QID=028722

    //ベジェ曲線の方向線とアンカーポイント、制御点を表示してみた - 午後わてんのブログ
    //https://gogowaten.hatenablog.com/entry/15547295
    //WPF、ベジェ曲線で直線表示、アンカー点の追加と削除 - 午後わてんのブログ
    //https://gogowaten.hatenablog.com/entry/2022/06/14/132217

    /// <summary>
    /// ベジェ曲線の方向線表示用、2色破線
    /// OnRenderで直線描画、その上にDefiningGeometryで破線描画
    /// </summary>

    class TwoColorDashLine : Shape
    {
        public TwoColorDashLine()
        {
            Stroke = Brushes.White;
            StrokeThickness = 1.0;
            StrokeDashArray = [15.0];
            //UseLayoutRounding = true;//効果がない感じ
            SetMyBind();
        }

        //実線のPenのバインド
        private void SetMyBind()
        {
            var mb = new MultiBinding() { Converter = new MyConvLinePen() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyStrokeBaseProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(StrokeThicknessProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(StrokeDashArrayProperty) });
            SetBinding(MyPenProperty, mb);
        }

        //実線を描画
        protected override void OnRender(DrawingContext drawingContext)
        {
            for (int i = 0; i < MyPoints.Count - 1; i++)
            {
                if ((i - 1) % 3 != 0)
                {
                    drawingContext.DrawLine(MyPen, MyPoints[i], MyPoints[i + 1]);
                }
            }
            base.OnRender(drawingContext);
        }

        //破線を描画
        protected override Geometry DefiningGeometry
        {
            get
            {
                if (MyPoints == null) { return Geometry.Empty; }
                StreamGeometry geo = new();
                using var context = geo.Open();
                for (int i = 0; i < MyPoints.Count - 1; i++)
                {
                    if ((i - 1) % 3 != 0)
                    {
                        context.BeginFigure(MyPoints[i], isFilled: false, isClosed: false);
                        context.LineTo(MyPoints[i + 1], isStroked: true, isSmoothJoin: false);
                    }
                }
                geo.Freeze();
                return geo;
            }
        }


        #region 依存関係プロパティ


        public Pen MyPen
        {
            get { return (Pen)GetValue(MyPenProperty); }
            set { SetValue(MyPenProperty, value); }
        }
        public static readonly DependencyProperty MyPenProperty =
            DependencyProperty.Register(nameof(MyPen), typeof(Pen), typeof(TwoColorDashLine), new PropertyMetadata(null));

        //AffectRender必須
        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(TwoColorDashLine),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public SolidColorBrush MyStrokeBase
        {
            get { return (SolidColorBrush)GetValue(MyStrokeBaseProperty); }
            set { SetValue(MyStrokeBaseProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeBaseProperty =
            DependencyProperty.Register(nameof(MyStrokeBase), typeof(SolidColorBrush), typeof(TwoColorDashLine), new PropertyMetadata(Brushes.Black));
        #endregion 依存関係プロパティ

    }



    #region コンバーター


    /// <summary>
    /// 制御線に使うPen
    /// </summary>
    public class MyConvLinePen : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var brush = (SolidColorBrush)values[0];
            var thickness = (double)values[1];
            var dashArray = (DoubleCollection)values[2];
            Pen pen = new(brush, thickness);
            DashStyle dash = new()
            {
                Offset = dashArray[0],
                Dashes = dashArray
            };
            pen.DashStyle = dash;
            pen.Freeze();
            return pen;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //回転軸のY座標、見た目通りの矩形(Bounds2)の中央にしている
    public class MyConverterCenterY : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (Rect)value;
            return (r.Y * 2 + r.Height) / 2.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MyConverterCenterX : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (Rect)value;
            return (r.X * 2 + r.Width) / 2.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConvRotateTransform : IValueConverter
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
    //RenderTransformはRotateTransformに決め打ちしている
    public class MyConverterRenderTransform : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var angle = (double)values[0];
            var x = (double)values[1];
            var y = (double)values[2];
            return new RotateTransform(angle, x, y);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //Penの生成、各種プロパティも反映
    public class MyConverterPen : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var thick = (double)values[0];
            var miter = (double)values[1];
            var end = (PenLineCap)values[2];
            var sta = (PenLineCap)values[3];
            var join = (PenLineJoin)values[4];
            Pen result = new(Brushes.Transparent, thick)
            {
                EndLineCap = end,
                StartLineCap = sta,
                LineJoin = join,
                MiterLimit = miter
            };
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //Segment用のPointCollection生成
    //ソースに影響を与えないためにクローン作成して、その先頭要素を削除して返す
    public class MyConverterSegmentPoints : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PointCollection pc && pc.Count > 0)
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