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
using System.ComponentModel;


namespace _20250408
{
    public enum HeadType { None = 0, Arrow, }
    public enum ShapeType { Line = 0, Bezier, }



    //アンカーハンドル付きのGeoShape
    public class GeoShapeWithAnchorHandle : GeoShape
    {
        public AdornerLayer MyAdornerLayer { get; set; } = null!;
        public AnchorHandleAdorner? MyAnchorHandleAdorner { get; set; }
        public GeoShapeWithAnchorHandle()
        {

            Loaded += GeoShapeWithAnchorHandle_Loaded;
        }

        private void GeoShapeWithAnchorHandle_Loaded(object sender, RoutedEventArgs e)
        {
            MyAdornerLayer = AdornerLayer.GetAdornerLayer(this);
        }


        #region アンカーハンドルの表示切り替え
        public void AnchorSwitch()
        {
            if (MyAnchorHandleAdorner is null) { AnchorHandleOn(); }
            else { AnchorHandleOff(); }
        }

        public void AnchorHandleOn()
        {
            if (MyAnchorHandleAdorner is null)
            {
                MyAnchorHandleAdorner = new AnchorHandleAdorner(this);
                //this.Visibility = Visibility.Visible;
                //MyAnchorHandleAdorner.OnAnchorThumbDragCompleted += MyAnchorHandleAdorner_OnDragCompleted;
                MyAdornerLayer.Add(MyAnchorHandleAdorner);
                //UpdateLocateAndSize();
                //MyParentThumb?.ReLayout3();
            }
        }

        public void AnchorHandleOff()
        {
            if (MyAnchorHandleAdorner != null)
            {
                //MyAnchorHandleAdorner.OnAnchorThumbDragCompleted -= MyAnchorHandleAdorner_OnDragCompleted;
                MyAdornerLayer.Remove(MyAnchorHandleAdorner);
                MyAnchorHandleAdorner = null;
                //UpdateLocateAndSize();
                //MyParentThumb?.ReLayout3();
            }
        }
        #endregion アンカーハンドルの表示切り替え

    }



    /// <summary>
    /// 矢印図形
    /// 見た目上での位置とサイズを表すプロパティMyShapeTransformedBoundsProperty
    /// </summary>
    public class GeoShape : Shape
    {

        public GeoShape()
        {
            MyInitializeBind();
            Canvas.SetLeft(this, 0);
            Canvas.SetTop(this, 0);

        }

        #region 初期処理

        private void MyInitializeBind()
        {
            Loaded += GeoShape_Loaded;
            //Pointsの始点と終点を外したPointCollection。始点と終点以外の線の描画に使う
            //このバインドの設定はLoadedでは遅いようで正しく描画されない、なのでここで行っている
            _ = SetBinding(MySegmentPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty), Mode = BindingMode.OneWay, Converter = new MyConverterSegmentPoints() });

        }

        private void GeoShape_Loaded(object sender, RoutedEventArgs e)
        {
            MyBindPen();
            MyBindRenderTransform2();
            MyBindShapeRenderTransformBounds2();
        }

        /// <summary>
        /// 図形の見た目上のBoundsのバインド
        /// </summary>
        private void MyBindShapeRenderTransformBounds2()
        {
            var mb = new MultiBinding() { Converter = new MyConvShapeTransformBounds() };
            mb.Bindings.Add(MakeOneWayBind(MyGeometryProperty));
            mb.Bindings.Add(MakeOneWayBind(MyRotateTransformProperty));
            mb.Bindings.Add(MakeOneWayBind(MyScaleTransformProperty));
            mb.Bindings.Add(MakeOneWayBind(MyPenProperty));
            SetBinding(MyShapeTransformedBoundsProperty, mb);
        }

        private void MyBindRenderTransform2()
        {
            var geoBounds = MakeOneWayBind(MyGeometryRenderBoundsProperty);
            var centerX = MakeOneWayBind(MyCenterXProperty);
            var centerY = MakeOneWayBind(MyCenterYProperty);

            var mb = new MultiBinding() { Converter = new MyConvRotateTransform() };
            mb.Bindings.Add(geoBounds);
            mb.Bindings.Add(centerX);
            mb.Bindings.Add(centerY);
            mb.Bindings.Add(MakeOneWayBind(MyAngleProperty));
            SetBinding(MyRotateTransformProperty, mb);

            mb = new MultiBinding() { Converter = new MyConvScaleTransform() };
            mb.Bindings.Add(geoBounds);
            mb.Bindings.Add(centerX);
            mb.Bindings.Add(centerY);
            mb.Bindings.Add(MakeOneWayBind(MyScaleXProperty));
            mb.Bindings.Add(MakeOneWayBind(MyScaleYProperty));
            SetBinding(MyScaleTransformProperty, mb);

            mb = new MultiBinding() { Converter = new MyConvRenderTransform2() };
            mb.Bindings.Add(MakeOneWayBind(MyRotateTransformProperty));
            mb.Bindings.Add(MakeOneWayBind(MyScaleTransformProperty));
            SetBinding(MyRenderTransformProperty, mb);
            SetBinding(RenderTransformProperty, mb);
        }


        private void MyBindPen()
        {
            //Penのバインド、Penは図形のBoundsを計測するために必要
            MultiBinding mb = new() { Converter = new MyConverterPen() };
            mb.Bindings.Add(MakeOneWayBind(StrokeThicknessProperty));
            mb.Bindings.Add(MakeOneWayBind(StrokeMiterLimitProperty));
            mb.Bindings.Add(MakeOneWayBind(StrokeEndLineCapProperty));
            mb.Bindings.Add(MakeOneWayBind(StrokeStartLineCapProperty));
            mb.Bindings.Add(MakeOneWayBind(StrokeLineJoinProperty));
            _ = SetBinding(MyPenProperty, mb);

        }

        private Binding MakeOneWayBind(DependencyProperty property)
        {
            return new Binding() { Source = this, Path = new PropertyPath(property), Mode = BindingMode.OneWay };
        }
        #endregion 初期処理

        //描画
        protected override Geometry DefiningGeometry
        {
            get
            {
                if (MyPoints is null || MyPoints.Count < 2) { return Geometry.Empty; }

                if (MyHeadBeginType != HeadType.None || MyHeadEndType != HeadType.None)
                {
                    Fill = Stroke;
                }

                StreamGeometry geo = new();
                using (var context = geo.Open())
                {
                    Point begin = MyPoints[0];
                    Point end = MyPoints[^1];
                    //先端の描画
                    switch (MyHeadBeginType)
                    {
                        case HeadType.None:
                            break;
                        case HeadType.Arrow:
                            begin = DrawArrow(context, begin, MyPoints[1]);
                            break;
                        default:
                            break;
                    }
                    //終端の描画
                    switch (MyHeadEndType)
                    {
                        case HeadType.None:
                            break;
                        case HeadType.Arrow:
                            end = DrawArrow(context, end, MyPoints[^2]);//終点とその一個前
                            break;
                        default:
                            break;
                    }
                    //線の描画
                    switch (MyShapeType)
                    {
                        case ShapeType.Line:
                            DrawLine(context, begin, end, MyIsFill, MyIsClosed, MyIsSmoothJoin);
                            break;
                        case ShapeType.Bezier:
                            DrawBezier(context, begin, end, MyIsFill, MyIsClosed, MyIsSmoothJoin);
                            break;
                        default:
                            break;
                    }
                }
                geo.Freeze();



                //Boundsの更新はここで行う必要がある？OnRenderではなんか違う
                MyGeometryRenderBounds = geo.Bounds;
                MyGeometry = geo;
                ////MyBounds1 = geo.Bounds;
                ////MyBounds2 = geo.GetRenderBounds(MyPen);
                ////回転後のBounds
                //var clone = geo.Clone();
                //clone.Transform = RenderTransform;
                ////MyBounds3 = clone.Bounds;
                //MyRenderBounds = clone.GetRenderBounds(MyPen);
                MyGeometryRenderBoundsWithPen = geo.GetRenderBounds(MyPen);



                return geo;
            }
        }






        #region 依存関係プロパティ

        #region 読み取り用

        public Transform MyRenderTransform
        {
            get { return (Transform)GetValue(MyRenderTransformProperty); }
            protected set { SetValue(MyRenderTransformProperty, value); }
        }
        public static readonly DependencyProperty MyRenderTransformProperty =
            DependencyProperty.Register(nameof(MyRenderTransform), typeof(Transform), typeof(GeoShape), new PropertyMetadata(null));

        public RotateTransform MyRotateTransform
        {
            get { return (RotateTransform)GetValue(MyRotateTransformProperty); }
            protected set { SetValue(MyRotateTransformProperty, value); }
        }
        public static readonly DependencyProperty MyRotateTransformProperty =
            DependencyProperty.Register(nameof(MyRotateTransform), typeof(RotateTransform), typeof(GeoShape), new PropertyMetadata(new RotateTransform()));

        public ScaleTransform MyScaleTransform
        {
            get { return (ScaleTransform)GetValue(MyScaleTransformProperty); }
            protected set { SetValue(MyScaleTransformProperty, value); }
        }
        public static readonly DependencyProperty MyScaleTransformProperty =
            DependencyProperty.Register(nameof(MyScaleTransform), typeof(ScaleTransform), typeof(GeoShape), new PropertyMetadata(new ScaleTransform()));

        /// <summary>
        /// Geometryそのまま
        /// </summary>
        public Geometry MyGeometry
        {
            get { return (Geometry)GetValue(MyGeometryProperty); }
            protected set { SetValue(MyGeometryProperty, value); }
        }
        public static readonly DependencyProperty MyGeometryProperty =
            DependencyProperty.Register(nameof(MyGeometry), typeof(Geometry), typeof(GeoShape), new PropertyMetadata(null));


        /// <summary>
        /// Geometry.Bounds。純粋なGeometryのみのBoundsで、PenStrokeもTransformも無し
        /// </summary>
        public Rect MyGeometryRenderBounds
        {
            get { return (Rect)GetValue(MyGeometryRenderBoundsProperty); }
            protected set { SetValue(MyGeometryRenderBoundsProperty, value); }
        }
        public static readonly DependencyProperty MyGeometryRenderBoundsProperty =
            DependencyProperty.Register(nameof(MyGeometryRenderBounds), typeof(Rect), typeof(GeoShape), new PropertyMetadata(new Rect()));

        /// <summary>
        /// 図形の変形後のBounds
        /// </summary>
        public Rect MyShapeTransformedBounds
        {
            get { return (Rect)GetValue(MyShapeTransformedBoundsProperty); }
            protected set { SetValue(MyShapeTransformedBoundsProperty, value); }
        }
        public static readonly DependencyProperty MyShapeTransformedBoundsProperty =
            DependencyProperty.Register(nameof(MyShapeTransformedBounds), typeof(Rect), typeof(GeoShape), new PropertyMetadata(new Rect()));


        /// <summary>
        /// StrokePenを考慮したGeometryのBounds。Transformは考慮しないBounds
        /// </summary>
        public Rect MyGeometryRenderBoundsWithPen
        {
            get { return (Rect)GetValue(MyGeometryRenderBoundsWithPenProperty); }
            protected set { SetValue(MyGeometryRenderBoundsWithPenProperty, value); }
        }
        public static readonly DependencyProperty MyGeometryRenderBoundsWithPenProperty =
            DependencyProperty.Register(nameof(MyGeometryRenderBoundsWithPen), typeof(Rect), typeof(GeoShape), new PropertyMetadata(new Rect()));

        //サイズと位置の計算に使う
        public Pen MyPen
        {
            get { return (Pen)GetValue(MyPenProperty); }
            protected set { SetValue(MyPenProperty, value); }
        }
        public static readonly DependencyProperty MyPenProperty =
            DependencyProperty.Register(nameof(MyPen), typeof(Pen), typeof(GeoShape), new PropertyMetadata(new Pen()));

        //MyPointsから作成
        public PointCollection MySegmentPoints
        {
            get { return (PointCollection)GetValue(MySegmentPointsProperty); }
            protected set { SetValue(MySegmentPointsProperty, value); }
        }
        public static readonly DependencyProperty MySegmentPointsProperty =
            DependencyProperty.Register(nameof(MySegmentPoints), typeof(PointCollection), typeof(GeoShape), new PropertyMetadata(null));


        #endregion 読み取り用

        #region Transform系


        public double MyCenterX
        {
            get { return (double)GetValue(MyCenterXProperty); }
            set { SetValue(MyCenterXProperty, value); }
        }
        public static readonly DependencyProperty MyCenterXProperty =
            DependencyProperty.Register(nameof(MyCenterX), typeof(double), typeof(GeoShape), new PropertyMetadata(0.0));

        public double MyCenterY
        {
            get { return (double)GetValue(MyCenterYProperty); }
            set { SetValue(MyCenterYProperty, value); }
        }
        public static readonly DependencyProperty MyCenterYProperty =
            DependencyProperty.Register(nameof(MyCenterY), typeof(double), typeof(GeoShape), new PropertyMetadata(0.0));

        public double MyScaleX
        {
            get { return (double)GetValue(MyScaleXProperty); }
            set { SetValue(MyScaleXProperty, value); }
        }
        public static readonly DependencyProperty MyScaleXProperty =
            DependencyProperty.Register(nameof(MyScaleX), typeof(double), typeof(GeoShape), new PropertyMetadata(1.0));

        public double MyScaleY
        {
            get { return (double)GetValue(MyScaleYProperty); }
            set { SetValue(MyScaleYProperty, value); }
        }
        public static readonly DependencyProperty MyScaleYProperty =
            DependencyProperty.Register(nameof(MyScaleY), typeof(double), typeof(GeoShape), new PropertyMetadata(1.0));

        public double MyAngle
        {
            get { return (double)GetValue(MyAngleProperty); }
            set { SetValue(MyAngleProperty, value); }
        }
        public static readonly DependencyProperty MyAngleProperty =
            DependencyProperty.Register(nameof(MyAngle), typeof(double), typeof(GeoShape), new PropertyMetadata(0.0));


        #endregion Transform系

        #region その他、試験用



        #endregion その他、試験用

        #region 通常


        public bool MyIsClosed
        {
            get { return (bool)GetValue(MyIsClosedProperty); }
            set { SetValue(MyIsClosedProperty, value); }
        }
        public static readonly DependencyProperty MyIsClosedProperty =
            DependencyProperty.Register(nameof(MyIsClosed), typeof(bool), typeof(GeoShape),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public bool MyIsSmoothJoin
        {
            get { return (bool)GetValue(MyIsSmoothJoinProperty); }
            set { SetValue(MyIsSmoothJoinProperty, value); }
        }
        public static readonly DependencyProperty MyIsSmoothJoinProperty =
            DependencyProperty.Register(nameof(MyIsSmoothJoin), typeof(bool), typeof(GeoShape),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public bool MyIsFill
        {
            get { return (bool)GetValue(MyIsFillProperty); }
            set { SetValue(MyIsFillProperty, value); }
        }
        public static readonly DependencyProperty MyIsFillProperty =
            DependencyProperty.Register(nameof(MyIsFill), typeof(bool), typeof(GeoShape),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public ShapeType MyShapeType
        {
            get { return (ShapeType)GetValue(MyShapeTypeProperty); }
            set { SetValue(MyShapeTypeProperty, value); }
        }
        public static readonly DependencyProperty MyShapeTypeProperty =
            DependencyProperty.Register(nameof(MyShapeType), typeof(ShapeType), typeof(GeoShape),
                new FrameworkPropertyMetadata(ShapeType.Line,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 終点のヘッドタイプ
        /// </summary>
        public HeadType MyHeadEndType
        {
            get { return (HeadType)GetValue(MyHeadEndTypeProperty); }
            set { SetValue(MyHeadEndTypeProperty, value); }
        }
        public static readonly DependencyProperty MyHeadEndTypeProperty =
            DependencyProperty.Register(nameof(MyHeadEndType), typeof(HeadType), typeof(GeoShape),
                new FrameworkPropertyMetadata(HeadType.None,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        /// <summary>
        /// 始点のヘッドタイプ
        /// </summary>
        public HeadType MyHeadBeginType
        {
            get { return (HeadType)GetValue(MyHeadBeginTypeProperty); }
            set { SetValue(MyHeadBeginTypeProperty, value); }
        }
        public static readonly DependencyProperty MyHeadBeginTypeProperty =
            DependencyProperty.Register(nameof(MyHeadBeginType), typeof(HeadType), typeof(GeoShape),
                new FrameworkPropertyMetadata(HeadType.None,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MyArrowHeadAngle
        {
            get { return (double)GetValue(MyArrowHeadAngleProperty); }
            set { SetValue(MyArrowHeadAngleProperty, value); }
        }
        public static readonly DependencyProperty MyArrowHeadAngleProperty =
            DependencyProperty.Register(nameof(MyArrowHeadAngle), typeof(double), typeof(GeoShape),
                new FrameworkPropertyMetadata(30.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(
                nameof(MyPoints),
                typeof(PointCollection),
                typeof(GeoShape),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 通常

        #endregion 依存関係プロパティ






        #region 描画メソッド

        /// <summary>
        /// 角度をラジアンに変換
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public static double DegreeToRadian(double degree)
        {
            return degree / 360.0 * (Math.PI * 2.0);
        }

        /// <summary>
        /// ベジェ曲線部分の描画
        /// </summary>
        /// <param name="context"></param>
        /// <param name="begin">始点図形との接点</param>
        /// <param name="end">終点図形との接点</param>
        private void DrawBezier(StreamGeometryContext context, Point begin, Point end, bool isFill, bool isClose, bool isSmoothJoin)
        {
            context.BeginFigure(begin, isFill, isClose);
            var bezier = MySegmentPoints.Clone();
            //List<Point> bezier = MyPoints.Skip(1).Take(MyPoints.Count - 2).ToList();
            bezier.Add(end);
            context.PolyBezierTo(bezier, true, isSmoothJoin);

        }


        /// <summary>
        /// 直線部分の描画
        /// </summary>
        /// <param name="context"></param>
        /// <param name="begin">始点図形との接点</param>
        /// <param name="end">終点図形との接点</param>
        private void DrawLine(StreamGeometryContext context, Point begin, Point end, bool isFill, bool isClosed, bool isSmoothJoin)
        {
            context.BeginFigure(begin, isFill, isClosed);
            context.PolyLineTo(MySegmentPoints, true, isSmoothJoin);
            //context.PolyLineTo(MyPoints.Skip(1).Take(MyPoints.Count - 2).ToList(), true, isSmoothJoin);
            context.LineTo(end, true, isSmoothJoin);
        }



        /// <summary>
        /// アローヘッド(三角形)描画
        /// </summary>
        /// <param name="context"></param>
        /// <param name="edge">端のPoint、始点ならPoints[0]、終点ならPoints[^1]</param>
        /// <param name="next">端から2番めのPoint、始点ならPoints[1]、終点ならPoints[^2]</param>
        /// <returns></returns>
        private Point DrawArrow(StreamGeometryContext context, Point edge, Point next)
        {
            //斜辺 hypotenuse ここでは二等辺三角形の底辺じゃない方の2辺
            //頂角 apex angle 先端の角
            //アローヘッドの斜辺(hypotenuse)の角度(ラジアン)を計算
            double lineRadian = Math.Atan2(next.Y - edge.Y, next.X - edge.X);
            double apexRadian = DegreeToRadian(MyArrowHeadAngle);
            double edgeSize = StrokeThickness * 2.0;
            double hypotenuseLength = edgeSize / Math.Cos(apexRadian);
            double hypotenuseRadian1 = lineRadian + apexRadian;

            //底角座標
            Point p1 = new(
                hypotenuseLength * Math.Cos(hypotenuseRadian1) + edge.X,
                hypotenuseLength * Math.Sin(hypotenuseRadian1) + edge.Y);

            double hypotenuseRadian2 = lineRadian - DegreeToRadian(MyArrowHeadAngle);
            Point p2 = new(
                hypotenuseLength * Math.Cos(hypotenuseRadian2) + edge.X,
                hypotenuseLength * Math.Sin(hypotenuseRadian2) + edge.Y);

            //アローヘッド描画、Fill(塗りつぶし)で描画
            context.BeginFigure(edge, true, false);//isFilled, isClose
            context.LineTo(p1, false, false);//isStroke, isSmoothJoin
            context.LineTo(p2, false, false);

            //アローヘッドと中間線の接点座標計算、
            //HeadSizeぴったりで計算すると僅かな隙間ができるので-1.0している、
            //-0.5でも隙間になる、-0.7で隙間なくなる
            return new Point(
                (edgeSize - 1.0) * Math.Cos(lineRadian) + edge.X,
                (edgeSize - 1.0) * Math.Sin(lineRadian) + edge.Y);
        }
        #endregion 描画メソッド


        #region パフリックメソッド

        public void MyPointReset()
        {
            double left = double.MaxValue; double top = double.MaxValue;
            foreach (Point item in MyPoints)
            {
                left = Math.Min(left, item.X);
                top = Math.Min(top, item.Y);
            }
            for (int i = 0; i < MyPoints.Count; i++)
            {
                Point poi = MyPoints[i];
                MyPoints[i] = new Point(poi.X - left, poi.Y - top);
            }
        }

        ///// <summary>
        ///// 図形が収まるRectを返す
        ///// </summary>
        ///// <returns></returns>
        //public Rect GetRenderBounds()
        //{
        //    //自身のGeometryのクローンを使う
        //    //自身に適用されているRenderTransformとPenをクローンに適用して
        //    //クローンのGetRenderBoundsで得られる
        //    Geometry geo = DefiningGeometry.Clone();
        //    geo.Transform = RenderTransform;
        //    Pen myPen = new(Brushes.Transparent, StrokeThickness)
        //    {
        //        EndLineCap = StrokeEndLineCap,
        //        StartLineCap = StrokeStartLineCap,
        //        LineJoin = StrokeLineJoin,
        //        MiterLimit = StrokeMiterLimit,
        //    };
        //    return geo.GetRenderBounds(myPen);
        //}

        #endregion パフリックメソッド



    }




    #region コンバーター

    /// <summary>
    /// 図形(見た目上)のBoundsを取得する
    /// </summary>
    public class MyConvShapeTransformBounds : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null) { return new Rect(); }
            var geo = (Geometry)values[0];
            var rotate = (RotateTransform)values[1];
            var scale = (ScaleTransform)values[2];
            var myPen = (Pen)values[3];

            //GeometryはCloneを使う、もしCloneしないと元のGeometryが変形してしまう
            Geometry clone = geo.Clone();

            //GeometryにRotateTransformだけ適用したものからStrokePenを使ってRotate後のBounds取得
            //それをScaleTransformしたものが答えになる
            clone.Transform = rotate;
            Rect rotateBounds = clone.GetRenderBounds(myPen);
            Rect result = scale.TransformBounds(rotateBounds);

            //なぜこんな面倒なことをしているのか？
            //もしGeometryのTransformにTransformGroupやRenderTransformをそのまま使うと
            //なぜかサイズが小さくなってしまうから
            return result;

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConvRenderTransform2 : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var rotate = (RotateTransform)values[0];
            var scale = (ScaleTransform)values[1];
            TransformGroup transformGroup = new();
            transformGroup.Children.Add(rotate);
            transformGroup.Children.Add(scale);
            return transformGroup;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class MyConvScaleTransform : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null) { return Transform.Identity; }
            var geoBounds = (Rect)values[0];
            var centerX = (double)values[1];
            var centerY = (double)values[2];
            var scaleX = (double)values[3];
            var scaleY = (double)values[4];

            centerX = geoBounds.Left + (geoBounds.Width * centerX);
            centerY = geoBounds.Top + (geoBounds.Height * centerY);

            return new ScaleTransform(scaleX, scaleY, centerX, centerY);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConvRotateTransform : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null) { return Transform.Identity; }
            var geoBounds = (Rect)values[0];
            var centerX = (double)values[1];
            var centerY = (double)values[2];
            var angle = (double)values[3];

            centerX = geoBounds.Left + (geoBounds.Width * centerX);
            centerY = geoBounds.Top + (geoBounds.Height * centerY);

            return new RotateTransform(angle, centerX, centerY);
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
                if (pc.Count > 1)
                {
                    var clone = pc.Clone();
                    clone.RemoveAt(0);
                    clone.RemoveAt(clone.Count - 1);
                    return clone;
                }
                return pc;
            }
            return new PointCollection();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
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

    #endregion コンバーター
}