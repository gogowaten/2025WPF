using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace _20250128_EzLineOffsetByLineWidth
{

    public class EzLine : Control
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


        public bool MyIsOffset
        {
            get { return (bool)GetValue(MyIsOffsetProperty); }
            set { SetValue(MyIsOffsetProperty, value); }
        }
        public static readonly DependencyProperty MyIsOffsetProperty =
            DependencyProperty.Register(nameof(MyIsOffset), typeof(bool), typeof(EzLine),
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


        private static readonly DependencyPropertyKey MyBoundsWithAnglePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyBoundsWithAngle), typeof(Rect), typeof(EzLine), new PropertyMetadata(new Rect()));
        public static readonly DependencyProperty MyBoundsWithAngleProperty = MyBoundsWithAnglePropertyKey.DependencyProperty;
        public Rect MyBoundsWithAngle
        {
            get { return (Rect)GetValue(MyBoundsWithAnglePropertyKey.DependencyProperty); }
            internal set { SetValue(MyBoundsWithAnglePropertyKey, value); }
        }



        //図形の基準点(左上頂点)からの距離、線が太くなるほど大きくマイナスになる
        private static readonly DependencyPropertyKey MyOffsetLeftPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyOffsetLeft), typeof(double), typeof(EzLine), new PropertyMetadata(0.0));
        public static readonly DependencyProperty MyOffsetLeftProperty = MyOffsetLeftPropertyKey.DependencyProperty;
        public double MyOffsetLeft
        {
            get { return (double)GetValue(MyOffsetLeftPropertyKey.DependencyProperty); }
            internal set { SetValue(MyOffsetLeftPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey MyOffsetTopPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyOffsetTop), typeof(double), typeof(EzLine), new PropertyMetadata(0.0));
        public static readonly DependencyProperty MyOffsetTopProperty = MyOffsetTopPropertyKey.DependencyProperty;
        public double MyOffsetTop
        {
            get { return (double)GetValue(MyOffsetTopPropertyKey.DependencyProperty); }
            internal set { SetValue(MyOffsetTopPropertyKey, value); }
        }


        private static readonly DependencyPropertyKey MyReverseOffsetLeftPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyReverseOffsetLeft), typeof(double), typeof(EzLine), new PropertyMetadata(0.0));
        public static readonly DependencyProperty MyReverseOffsetLeftProperty = MyReverseOffsetLeftPropertyKey.DependencyProperty;
        public double MyReverseOffsetLeft
        {
            get { return (double)GetValue(MyReverseOffsetLeftPropertyKey.DependencyProperty); }
            internal set { SetValue(MyReverseOffsetLeftPropertyKey, value); }
        }


        private static readonly DependencyPropertyKey MyReverseOffsetTopPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyReverseOffsetTop), typeof(double), typeof(EzLine), new PropertyMetadata(0.0));
        public static readonly DependencyProperty MyReverseOffsetTopProperty = MyReverseOffsetTopPropertyKey.DependencyProperty;
        public double MyReverseOffsetTop
        {
            get { return (double)GetValue(MyReverseOffsetTopPropertyKey.DependencyProperty); }
            internal set { SetValue(MyReverseOffsetTopPropertyKey, value); }
        }



        #endregion 読み取り専用依存関係プロパティ


        public Canvas? MyBasePanel { get; set; }
        static EzLine()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EzLine), new FrameworkPropertyMetadata(typeof(EzLine)));
        }
        public EzLine()
        {
            DataContext = this;

        }


        //起動中のTemplate構築時に中のPathを取得
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_Path") is Path path)
            {
                MyPath = path;
            }
            if (GetTemplateChild("PART_Canvas") is Canvas panel)
            {
                MyBasePanel = panel;
            }
        }
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            //SetBounds2();
        }
        //描画直後に線(Pen)の描画を考慮した位置とサイズの取得
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            SetBounds2();
        }

        private void SetBounds2()
        {
            Rect rectGeoPen = MyPath.Data.GetRenderBounds(MyPen);
            if (rectGeoPen.Width > 0)
            {
                //MyBoundsWithPen = rectGeoPen;

                if (MyBasePanel != null)
                {
                    //MyBasePanel.Width = rectGeoPen.Width;
                    //MyBasePanel.Height = rectGeoPen.Height;

                    //var cloneGeo = MyPath.Data.Clone();//これだとクローンできない
                    var geo = MyPath.RenderedGeometry.Clone();
                    var rotateTF = (RotateTransform)MyPath.RenderTransform;
                    Rect motoGeoRect = geo.Bounds;
                    Rect motoGeoPenRect = geo.GetRenderBounds(MyPen);

                    //rotateTF.CenterX = motoGeoPenRect.Width / 2;
                    //rotateTF.CenterY = motoGeoPenRect.Height / 2;

                    Rect motoGeoPenTRRect = rotateTF.TransformBounds(motoGeoPenRect);
                                        
                    geo.Transform = rotateTF;
                    Rect rectGeoTF = geo.Bounds;
                    Rect rectGeoTFPen = geo.GetRenderBounds(MyPen);

                    Width = rectGeoTFPen.Width;
                    Height = rectGeoTFPen.Height;

                    MyOffsetLeft = -rectGeoTFPen.Left;
                    MyOffsetTop = -rectGeoTFPen.Top;
                    //MyOffsetLeft = -motoGeoPenRect.Left/2;
                    //MyOffsetTop = -motoGeoPenRect.Top/2;

                    //MyReverseOffsetLeft = rectGeoTFPen.Left;
                    //MyReverseOffsetTop = rectGeoTFPen.Top;

                    MyBoundsWithAngle = rectGeoTFPen;
                }




                //Width = bounds.Width;
                //Height = bounds.Height;
            }
        }


    }



    #region コンバーター


    public class MyConverterReverse : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double v = (double)value;
            return -v;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// PointCollectionをクローンして、その最初の要素を除いて返す
    /// </summary>
    public class MyConverterSegment : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PointCollection pc)
            {
                if (pc.Count >= 2)
                {
                    PointCollection cpc = pc.Clone();
                    cpc.RemoveAt(0);
                    return cpc;
                }
                else
                {
                    return pc;
                }
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

    /// <summary>
    /// 太さから透明色のPenを作って返す
    /// </summary>
    public class MyConverterPen : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double bold = (double)value;
            return new Pen(Brushes.Transparent, bold);
        }

        //こっちは未使用になるはず        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion コンバーター

}
