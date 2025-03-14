﻿using System;
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

namespace _20250313
{
    public enum HeadType { None = 0, Arrow, }
    public enum ShapeType { Line = 0, Bezier, }

    public class GeoShape : Shape
    {
        protected override Geometry DefiningGeometry
        {
            get
            {
                if (MyPoints.Count == 0) { return Geometry.Empty; }

                if(MyHeadBeginType != HeadType.None || MyHeadEndType!= HeadType.None)
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



                //Boundsの更新はここで行う必要がある。OnRenderではなんか違う
                //MyBounds1 = geo.Bounds;
                //MyBounds2 = geo.GetRenderBounds(MyPen);
                //回転後のBounds
                var clone = geo.Clone();
                clone.Transform = RenderTransform;
                //MyBounds3 = clone.Bounds;
                MyRenderBounds = clone.GetRenderBounds(MyPen);

                return geo;
            }
        }






        #region 依存関係プロパティ


        public Rect MyRenderBounds
        {
            get { return (Rect)GetValue(MyRenderBoundsProperty); }
            set { SetValue(MyRenderBoundsProperty, value); }
        }
        public static readonly DependencyProperty MyRenderBoundsProperty =
            DependencyProperty.Register(nameof(MyRenderBounds), typeof(Rect), typeof(GeoShape), new PropertyMetadata(new Rect()));

        public Pen MyPen
        {
            get { return (Pen)GetValue(MyPenProperty); }
            set { SetValue(MyPenProperty, value); }
        }
        public static readonly DependencyProperty MyPenProperty =
            DependencyProperty.Register(nameof(MyPen), typeof(Pen), typeof(GeoShape), new PropertyMetadata(new Pen()));


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

        public PointCollection MyMySegmentPoints
        {
            get { return (PointCollection)GetValue(MyMySegmentPointsProperty); }
            set { SetValue(MyMySegmentPointsProperty, value); }
        }
        public static readonly DependencyProperty MyMySegmentPointsProperty =
            DependencyProperty.Register(nameof(MyMySegmentPoints), typeof(PointCollection), typeof(GeoShape), new PropertyMetadata(null));

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

        #endregion 依存関係プロパティ




        /// <summary>
        /// ベジェ曲線部分の描画
        /// </summary>
        /// <param name="context"></param>
        /// <param name="begin">始点図形との接点</param>
        /// <param name="end">終点図形との接点</param>
        private void DrawBezier(StreamGeometryContext context, Point begin, Point end, bool isFill, bool isClose, bool isSmoothJoin)
        {
            context.BeginFigure(begin, isFill, isClose);
            List<Point> bezier = MyPoints.Skip(1).Take(MyPoints.Count - 2).ToList();
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
            context.PolyLineTo(MyPoints.Skip(1).Take(MyPoints.Count - 2).ToList(), true, isSmoothJoin);
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

        /// <summary>
        /// 角度をラジアンに変換
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public static double DegreeToRadian(double degree)
        {
            return degree / 360.0 * (Math.PI * 2.0);
        }





    }






}
