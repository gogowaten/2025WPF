using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace _20250125
{
    //サイズの更新が難しい
    //  違ったShapeでもOnRenderで行えばいいだけだった
    //以下は間違い
    //DefiningGeometryのGetの中ではタイミングが早すぎて描画が見切れてしまう
    //OnRenderSizeChangedの中ではマイナス方向に伸びたときはイベントが発生しないので更新できない
    //ArrangeOverrideのオーバーライドの中、今のところこれが一番マシ、2回更新が発生してしまうのが欠点
    //これだけめんどくさいならいっそのこと、サイズ変更に関わるプロパティ変更時に更新したほうがいいかも？
    //サイズ変更に関わるプロパティは
    //MyPoints、StrokeThickness、IsSmoothJoin、IsClosed、IsStroked、回転角度、拡大率
    //結構たくさんある

    public class EzLineShape : Shape
    {
        #region 依存関係プロパティ
        // FrameworkPropertyMetadataOptions.AffectsRender // デザイン画面上での更新で必要
        // FrameworkPropertyMetadataOptions.AffectsMeasure // 必要ないかも？



        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(EzLineShape),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //public double MyStrokeThickness
        //{
        //    get { return (double)GetValue(MyStrokeThicknessProperty); }
        //    set { SetValue(MyStrokeThicknessProperty, value); }
        //}
        //public static readonly DependencyProperty MyStrokeThicknessProperty =
        //    DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(EzLineShape), new FrameworkPropertyMetadata(1.0,
        //        FrameworkPropertyMetadataOptions.AffectsRender |// デザイン画面上での更新で必要
        //        FrameworkPropertyMetadataOptions.AffectsMeasure)); // 必要ないかも？

        //public Brush MyStroke
        //{
        //    get { return (Brush)GetValue(MyStrokeProperty); }
        //    set { SetValue(MyStrokeProperty, value); }
        //}
        //public static readonly DependencyProperty MyStrokeProperty =
        //    DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(EzLineShape), new FrameworkPropertyMetadata(Brushes.Magenta,
        //        FrameworkPropertyMetadataOptions.AffectsRender |
        //        FrameworkPropertyMetadataOptions.AffectsMeasure));


        public Brush MyFill
        {
            get { return (Brush)GetValue(MyFillProperty); }
            set { SetValue(MyFillProperty, value); }
        }
        public static readonly DependencyProperty MyFillProperty =
            DependencyProperty.Register(nameof(MyFill), typeof(Brush), typeof(EzLineShape), new FrameworkPropertyMetadata(Brushes.Pink,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        public bool MyIsFilled
        {
            get { return (bool)GetValue(MyIsFilledProperty); }
            set { SetValue(MyIsFilledProperty, value); }
        }
        public static readonly DependencyProperty MyIsFilledProperty =
            DependencyProperty.Register(nameof(MyIsFilled), typeof(bool), typeof(EzLineShape), new FrameworkPropertyMetadata(true,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        public bool MyIsStroked
        {
            get { return (bool)GetValue(MyIsStrokedProperty); }
            set { SetValue(MyIsStrokedProperty, value); }
        }
        public static readonly DependencyProperty MyIsStrokedProperty =
            DependencyProperty.Register(nameof(MyIsStroked), typeof(bool), typeof(EzLineShape), new FrameworkPropertyMetadata(true,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        public bool MyIsClosed
        {
            get { return (bool)GetValue(MyIsClosedProperty); }
            set { SetValue(MyIsClosedProperty, value); }
        }
        public static readonly DependencyProperty MyIsClosedProperty =
            DependencyProperty.Register(nameof(MyIsClosed), typeof(bool), typeof(EzLineShape), new FrameworkPropertyMetadata(false,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        public bool MyIsSmoothJoin
        {
            get { return (bool)GetValue(MyIsSmoothJoinProperty); }
            set { SetValue(MyIsSmoothJoinProperty, value); }
        }
        public static readonly DependencyProperty MyIsSmoothJoinProperty =
            DependencyProperty.Register(nameof(MyIsSmoothJoin), typeof(bool), typeof(EzLineShape),
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
            DependencyProperty.Register(nameof(MyFillRule), typeof(FillRule), typeof(EzLineShape),
                new FrameworkPropertyMetadata(FillRule.Nonzero,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        //読み取り専用のPen、なぜかバインドできない、エラーになる
        //private static readonly DependencyPropertyKey MyPenPropertyKey =
        //    DependencyProperty.RegisterReadOnly(nameof(MyPen), typeof(Pen), typeof(EzLine), new PropertyMetadata(null));
        //public static readonly DependencyProperty MyPenProperty = MyPenPropertyKey.DependencyProperty;
        //public Pen MyPen
        //{
        //    get { return (Pen)GetValue(MyPenPropertyKey.DependencyProperty); }
        //    internal set { SetValue(MyPenPropertyKey, value); }
        //}

        public Pen MyPen
        {
            get { return (Pen)GetValue(MyPenProperty); }
            set { SetValue(MyPenProperty, value); }
        }
        public static readonly DependencyProperty MyPenProperty =
            DependencyProperty.Register(nameof(MyPen), typeof(Pen), typeof(EzLineShape), new FrameworkPropertyMetadata(new Pen(),
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));



        public PointCollection MySegmentPoints
        {
            get { return (PointCollection)GetValue(MySegmentPointsProperty); }
            set { SetValue(MySegmentPointsProperty, value); }
        }
        public static readonly DependencyProperty MySegmentPointsProperty =
            DependencyProperty.Register(nameof(MySegmentPoints), typeof(PointCollection), typeof(EzLineShape), new PropertyMetadata(new PointCollection()));



        #endregion 依存関係プロパティ

        /// <summary>
        /// Pen(Stroke)の太さを考慮した位置とサイズ
        /// </summary>
        private static readonly DependencyPropertyKey MyBoundsPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyBoundsWithPen), typeof(Rect), typeof(EzLineShape), new PropertyMetadata(new Rect()));
        public static readonly DependencyProperty MyBoundsWithPenProperty = MyBoundsPropertyKey.DependencyProperty;
        public Rect MyBoundsWithPen
        {
            get { return (Rect)GetValue(MyBoundsPropertyKey.DependencyProperty); }
            internal set { SetValue(MyBoundsPropertyKey, value); }
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                if (MyPoints is null || MyPoints.Count == 0)
                {
                    return Geometry.Empty;
                }

                //GeometryDrawing geoDraw = new(Fill, new Pen(Stroke, StrokeThickness), geo);
                StreamGeometry geo = new() { FillRule = MyFillRule };
                using (var context = geo.Open())
                {
                    context.BeginFigure(MyPoints[0], MyIsFilled, MyIsClosed);
                    context.PolyLineTo(MySegmentPoints, MyIsStroked, MyIsSmoothJoin);
                }
                geo.Freeze();


                //var dgb = geo.Bounds;
                //PathGeometry wide = geo.GetWidenedPathGeometry(MyPen);
                //var rWide = wide.Bounds;

                return geo;
            }
        }

        static EzLineShape()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EzLineShape), new FrameworkPropertyMetadata(typeof(EzLineShape)));
        }
        public EzLineShape()
        {
            DataContext = this;
            Initialized += EzLineShape_Initialized;
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            SetSize();
        }
        private void SetSize()
        {
            //PathGeometry wideGeo = DefiningGeometry.GetWidenedPathGeometry(MyPen);
            //Rect bounds = wideGeo.Bounds;

            //GetWidenedPathGeometry(Pen).Boundsと同じ値が取得できる
            Rect bounds = DefiningGeometry.GetRenderBounds(MyPen);
            MyBoundsWithPen = bounds;
            //Width = bounds.Width;
            //Height = bounds.Height;

        }

        ////サイズの設定はDefiningGeometryの描画直後では早すぎだったので
        ////このタイミングにしてみた
        ////OnRenderSizeChangedが2回発生することになるけど、他に思いつかない
        ////  できてない、マイナス方向に伸びたときは発生しないので更新できない
        //protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        //{
        //    base.OnRenderSizeChanged(sizeInfo);
        //    //SetSize();
        //}


        protected override Size MeasureOverride(Size constraint)
        {
            //なんか違う
            //var rect = DefiningGeometry.GetRenderBounds(MyPen);
            ////Width = rect.Width;
            ////Height = rect.Height;
            //return base.MeasureOverride(rect.Size);
            return base.MeasureOverride(constraint);
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            ////2回更新になるか、1回更新で見切れるの2択なので2回更新で
            //var rect = DefiningGeometry.GetRenderBounds(MyPen);
            //Width = rect.Width;
            //Height = rect.Height;
            //// return base.ArrangeOverride(rect.Size);
            return base.ArrangeOverride(finalSize);
        }
        private void EzLineShape_Initialized(object? sender, EventArgs e)
        {
            BindingOperations.SetBinding(this, MySegmentPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty), Converter = new MyConverterSegment() });
            BindingOperations.SetBinding(this, MyPenProperty, new Binding() { Source = this, Path = new PropertyPath(StrokeThicknessProperty), Converter = new MyConverterPen() });

        }

    }

}
