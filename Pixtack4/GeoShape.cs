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


namespace Pixtack4
{
    public enum HeadType { None = 0, Arrow, }
    public enum ShapeType { Line = 0, Bezier, }

    public class GeoShape : Shape
    {
        public GeoShape()
        {
            MyInitializeBind();

        }

        #region 初期処理

        private void MyInitializeBind()
        {
            //Pointsの先頭を外したPointCollection
            _ = SetBinding(MySegmentPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty), Mode = BindingMode.OneWay, Converter = new MyConverterSegmentPoints() });

            ////Penのバインド、Penは図形のBoundsを計測するために必要
            //MultiBinding mb = new() { Converter = new MyConverterPen() };
            //mb.Bindings.Add(MakeOneWayBind(StrokeThicknessProperty));
            //mb.Bindings.Add(MakeOneWayBind(StrokeMiterLimitProperty));
            //mb.Bindings.Add(MakeOneWayBind(StrokeEndLineCapProperty));
            //mb.Bindings.Add(MakeOneWayBind(StrokeStartLineCapProperty));
            //mb.Bindings.Add(MakeOneWayBind(StrokeLineJoinProperty));
            //_ = SetBinding(MyPenProperty, mb);
        }

        private Binding MakeOneWayBind(DependencyProperty property)
        {
            return new Binding() { Source = this, Path = new PropertyPath(property), Mode = BindingMode.OneWay };
        }
        #endregion 初期処理


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

                    switch (MyHeadEndType)
                    {
                        case HeadType.None:
                            break;
                        case HeadType.Arrow:
                            end = DrawArrow(context, end, MyPoints[^2]);
                            break;
                        default:
                            break;
                    }

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



                ////Boundsの更新はここで行う必要がある。OnRenderではなんか違う
                ////MyBounds1 = geo.Bounds;
                ////MyBounds2 = geo.GetRenderBounds(MyPen);
                ////回転後のBounds
                //var clone = geo.Clone();
                //clone.Transform = RenderTransform;
                ////MyBounds3 = clone.Bounds;
                //MyRenderBounds = clone.GetRenderBounds(MyPen);

                return geo;
            }
        }






        #region 依存関係プロパティ

        #region 読み取り用

        ////サイズと位置
        //public Rect MyRenderBounds
        //{
        //    get { return (Rect)GetValue(MyRenderBoundsProperty); }
        //    set { SetValue(MyRenderBoundsProperty, value); }
        //}
        //public static readonly DependencyProperty MyRenderBoundsProperty =
        //    DependencyProperty.Register(nameof(MyRenderBounds), typeof(Rect), typeof(GeoShape), new PropertyMetadata(new Rect()));

        ////サイズと位置の計算に使う
        //public Pen MyPen
        //{
        //    get { return (Pen)GetValue(MyPenProperty); }
        //    set { SetValue(MyPenProperty, value); }
        //}
        //public static readonly DependencyProperty MyPenProperty =
        //    DependencyProperty.Register(nameof(MyPen), typeof(Pen), typeof(GeoShape), new PropertyMetadata(new Pen()));

        //MyPointsから作成
        public PointCollection MySegmentPoints
        {
            get { return (PointCollection)GetValue(MySegmentPointsProperty); }
            set { SetValue(MySegmentPointsProperty, value); }
        }
        public static readonly DependencyProperty MySegmentPointsProperty =
            DependencyProperty.Register(nameof(MySegmentPoints), typeof(PointCollection), typeof(GeoShape), new PropertyMetadata(null));


        #endregion 読み取り用



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


        ///// <summary>
        ///// RenderBoundsの更新、
        ///// RenderBoundsはRenderTransformを変更しても更新されないので、その時用
        ///// </summary>
        //public void UpdateRenderBounds()
        //{
        //    //回転後のBounds
        //    Geometry clone = this.DefiningGeometry.Clone();
        //    clone.Transform = RenderTransform;
        //    MyRenderBounds = clone.GetRenderBounds(MyPen);
        //}


        #region メソッド

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
            //            List<Point> bezier = MyPoints.Skip(1).Take(MyPoints.Count - 2).ToList();
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
        #endregion メソッド


        #region パフリックメソッド

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
        /// 図形が収まるRectを返す
        /// </summary>
        /// <returns></returns>
        public Rect GetRenderBounds()
        {
            //自身のGeometryのクローンを使う
            //自身に適用されているRenderTransformとPenをクローンに適用して
            //クローンのGetRenderBoundsで得られる
            var geo = DefiningGeometry.Clone();
            geo.Transform = RenderTransform;
            Pen myPen = new(Brushes.Transparent, StrokeThickness)
            {
                EndLineCap = StrokeEndLineCap,
                StartLineCap = StrokeStartLineCap,
                LineJoin = StrokeLineJoin,
                MiterLimit = StrokeMiterLimit,
            };
            return geo.GetRenderBounds(myPen);
        }

        #endregion パフリックメソッド



    }


    //Segment用のPointCollection生成
    //ソースに影響を与えないためにクローン作成して、その始点と終点要素を削除して返す
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




}