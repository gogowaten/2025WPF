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
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows.Media.Imaging;


namespace _20250405
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
                MyAnchorHandleAdorner = new(this);
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



    public class GeoShape : Shape, INotifyPropertyChanged
    {
        #region 必要
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion 必要

        //private bool _isOffset;
        //public bool IsOffset
        //{
        //    get => _isOffset;
        //    set
        //    {
        //        SetProperty(ref _isOffset, value);
        //        if (value)
        //        {
        //            MyBindActualLocate();
        //        }
        //        else
        //        {
        //            MyBindActualLocateKaijo();
        //        }
        //    }
        //}

        public GeoShape()
        {
            MyInitializeBind();
            Canvas.SetLeft(this, 0);
            Canvas.SetTop(this, 0);


        }

        #region 初期処理

        private void MyInitializeBind()
        {
            //Pointsの始点と終点を外したPointCollection。始点と終点以外の線の描画に使う
            _ = SetBinding(MySegmentPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty), Mode = BindingMode.OneWay, Converter = new MyConverterSegmentPoints() });
            MyBindPen();
            //MyBindTTT();

            //Loaded += GeoShape_Loaded;
        }

        //private void GeoShape_Loaded(object sender, RoutedEventArgs e)
        //{

        //    //MyBindActualLocate();
        //    //if (RenderTransform == Transform.Identity)
        //    //{

        //    //    TransformGroup group = new();
        //    //    group.Children.Add(new RotateTransform());
        //    //    group.Children.Add(new ScaleTransform());
        //    //    RenderTransform = group;
        //    //}
        //    //else
        //    //{
        //    //    var neko = RenderTransform;
        //    //}

        //}
        //private void MyBindTTT()
        //{
        //    SetBinding(MyOffsetLeftProperty, new Binding() { Source = this, Path = new PropertyPath(MyRenderBoundsProperty), Converter = new MyConvOffsetLeft() });
        //    SetBinding(MyOffsetTopProperty, new Binding() { Source = this, Path = new PropertyPath(MyRenderBoundsProperty), Converter = new MyConvOffsetTop() });


        //}

        //private void MyBindActualLocate()
        //{
        //    var mb = new MultiBinding() { Converter = new MyConvActualLeftForGeoShape() };
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(Canvas.LeftProperty) });
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyRenderBoundsProperty) });
        //    SetBinding(Canvas.LeftProperty, mb);

        //    mb = new MultiBinding() { Converter = new MyConvActualTopForGeoShape() };
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(Canvas.TopProperty) });
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyRenderBoundsProperty) });
        //    SetBinding(Canvas.TopProperty, mb);
        //}
        //private void MyBindActualLocateKaijo()
        //{
        //    SetBinding(Canvas.LeftProperty, new Binding());
        //    SetBinding(Canvas.TopProperty, new Binding());
        //}

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



                ////Boundsの更新はここで行う必要がある。OnRenderではなんか違う
                ////MyBounds1 = geo.Bounds;
                ////MyBounds2 = geo.GetRenderBounds(MyPen);
                ////回転後のBounds
                //var clone = geo.Clone();
                //clone.Transform = RenderTransform;
                ////MyBounds3 = clone.Bounds;
                //MyRenderBounds = clone.GetRenderBounds(MyPen);
                MyRenderBounds = geo.GetRenderBounds(MyPen);



                return geo;
            }
        }






        #region 依存関係プロパティ

        #region 読み取り用


        public double MyOffsetLeft
        {
            get { return (double)GetValue(MyOffsetLeftProperty); }
            set { SetValue(MyOffsetLeftProperty, value); }
        }
        public static readonly DependencyProperty MyOffsetLeftProperty =
            DependencyProperty.Register(nameof(MyOffsetLeft), typeof(double), typeof(GeoShape), new PropertyMetadata(0.0));

        public double MyOffsetTop
        {
            get { return (double)GetValue(MyOffsetTopProperty); }
            set { SetValue(MyOffsetTopProperty, value); }
        }
        public static readonly DependencyProperty MyOffsetTopProperty =
            DependencyProperty.Register(nameof(MyOffsetTop), typeof(double), typeof(GeoShape), new PropertyMetadata(0.0));

        public double MyActualLeft
        {
            get { return (double)GetValue(MyActualLeftProperty); }
            set { SetValue(MyActualLeftProperty, value); }
        }
        public static readonly DependencyProperty MyActualLeftProperty =
            DependencyProperty.Register(nameof(MyActualLeft), typeof(double), typeof(GeoShape), new PropertyMetadata(0.0));

        public double MyActualTop
        {
            get { return (double)GetValue(MyActualTopProperty); }
            set { SetValue(MyActualTopProperty, value); }
        }
        public static readonly DependencyProperty MyActualTopProperty =
            DependencyProperty.Register(nameof(MyActualTop), typeof(double), typeof(GeoShape), new PropertyMetadata(0.0));

        public bool MyIsOffset
        {
            get { return (bool)GetValue(MyIsOffsetProperty); }
            set { SetValue(MyIsOffsetProperty, value); }
        }
        public static readonly DependencyProperty MyIsOffsetProperty =
            DependencyProperty.Register(nameof(MyIsOffset), typeof(bool), typeof(GeoShape), new PropertyMetadata(false));


        //Strokeを考慮したBounds。Transformは考慮しないBounds
        public Rect MyRenderBounds
        {
            get { return (Rect)GetValue(MyRenderBoundsProperty); }
            set { SetValue(MyRenderBoundsProperty, value); }
        }
        public static readonly DependencyProperty MyRenderBoundsProperty =
            DependencyProperty.Register(nameof(MyRenderBounds), typeof(Rect), typeof(GeoShape), new PropertyMetadata(new Rect()));

        //サイズと位置の計算に使う
        public Pen MyPen
        {
            get { return (Pen)GetValue(MyPenProperty); }
            set { SetValue(MyPenProperty, value); }
        }
        public static readonly DependencyProperty MyPenProperty =
            DependencyProperty.Register(nameof(MyPen), typeof(Pen), typeof(GeoShape), new PropertyMetadata(new Pen()));

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

        /// <summary>
        /// 図形が収まるRectを返す
        /// </summary>
        /// <returns></returns>
        public Rect GetRenderBounds()
        {
            //自身のGeometryのクローンを使う
            //自身に適用されているRenderTransformとPenをクローンに適用して
            //クローンのGetRenderBoundsで得られる
            Geometry geo = DefiningGeometry.Clone();
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



    /// <summary>
    /// GeoShape用のアンカーハンドルThumb
    /// </summary>
    public class AnchorHandleThumb : Thumb
    {
        static AnchorHandleThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnchorHandleThumb), new FrameworkPropertyMetadata(typeof(AnchorHandleThumb)));
        }
        public AnchorHandleThumb()
        {

        }

        #region 依存関係プロパティ


        public double MyLeft
        {
            get { return (double)GetValue(MyLeftProperty); }
            set { SetValue(MyLeftProperty, value); }
        }
        public static readonly DependencyProperty MyLeftProperty =
            DependencyProperty.Register(nameof(MyLeft), typeof(double), typeof(AnchorHandleThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MyTop
        {
            get { return (double)GetValue(MyTopProperty); }
            set { SetValue(MyTopProperty, value); }
        }
        public static readonly DependencyProperty MyTopProperty =
            DependencyProperty.Register(nameof(MyTop), typeof(double), typeof(AnchorHandleThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MySize
        {
            get { return (double)GetValue(MySizeProperty); }
            set { SetValue(MySizeProperty, value); }
        }
        public static readonly DependencyProperty MySizeProperty =
            DependencyProperty.Register(nameof(MySize), typeof(double), typeof(AnchorHandleThumb), new PropertyMetadata(20.0));

        #endregion 依存関係プロパティ

    }


    #region コンバーター
    public class MyConvOffsetLeft : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bounds = (Rect)value;
            return -bounds.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MyConvOffsetTop : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bounds = (Rect)value;
            return -bounds.Top;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class MyConvActualLeftForGeoShape : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var left = (double)values[0];
            var bounds = (Rect)values[1];
            return left - bounds.Left;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConvActualTopForGeoShape : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var top = (double)values[0];
            var bounds = (Rect)values[1];
            return top - bounds.Top;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //public class MyConvActualLeftForGeoShape : IMultiValueConverter
    //{
    //    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        var flag = (bool)values[0];
    //        var left = (double)values[1];
    //        var bounds = (Rect)values[2];
    //        if (flag)
    //        {
    //            return left - bounds.Left;
    //        }
    //        else { return left; }
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class MyConvActualTopForGeoShape : IMultiValueConverter
    //{
    //    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        var flag = (bool)values[0];
    //        var top = (double)values[1];
    //        var bounds = (Rect)values[2];
    //        if (flag)
    //        {
    //            return top - bounds.Top;
    //        }
    //        else { return top; }
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}


    public class MyConvGeoShapeBounds : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            if (values[0] is not Geometry) { return new Rect(); }
            var geo = (Geometry)parameter;
            var clone = geo.Clone();

            var thickness = (double)values[0];
            var endCap = (PenLineCap)values[1];
            var beginCap = (PenLineCap)values[2];
            var join = (PenLineJoin)values[3];
            var miter = (double)values[4];
            Pen myPen = new(Brushes.Transparent, thickness)
            {
                EndLineCap = endCap,
                StartLineCap = beginCap,
                LineJoin = join,
                MiterLimit = miter,
            };

            return clone.GetRenderBounds(myPen);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
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