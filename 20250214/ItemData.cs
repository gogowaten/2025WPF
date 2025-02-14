using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace _20250214
{
    public class ItemData : DependencyObject, IExtensibleDataObject
    {
        public ExtensionDataObject? ExtensionData { get; set; }

        #region 依存関係プロパティ


        #region 共通

        //テキスト系要素のText要素
        public string MyText
        {
            get { return (string)GetValue(MyTextProperty); }
            set { SetValue(MyTextProperty, value); }
        }
        public static readonly DependencyProperty MyTextProperty =
            DependencyProperty.Register(nameof(MyText), typeof(string), typeof(ItemData),
                new FrameworkPropertyMetadata(string.Empty,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //コンテンツの回転角度
        public double MyAngle
        {
            get { return (double)GetValue(MyAngleProperty); }
            set { SetValue(MyAngleProperty, value); }
        }
        public static readonly DependencyProperty MyAngleProperty =
            DependencyProperty.Register(nameof(MyAngle), typeof(double), typeof(ItemData),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        public Canvas MyBaseCanvas
        {
            get { return (Canvas)GetValue(MyBaseCanvasProperty); }
            private set { SetValue(MyBaseCanvasProperty, value); }
        }
        public static readonly DependencyProperty MyBaseCanvasProperty =
            DependencyProperty.Register(nameof(MyBaseCanvas), typeof(Canvas), typeof(ItemData), new PropertyMetadata(null));


        public double MyLeft
        {
            get { return (double)GetValue(MyLeftProperty); }
            set { SetValue(MyLeftProperty, value); }
        }
        public static readonly DependencyProperty MyLeftProperty =
            DependencyProperty.Register(nameof(MyLeft), typeof(double), typeof(ItemData),
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
            DependencyProperty.Register(nameof(MyTop), typeof(double), typeof(ItemData),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        //public double MyOffsetLeft
        //{
        //    get { return (double)GetValue(MyOffsetLeftProperty); }
        //    internal set { SetValue(MyOffsetLeftProperty, value); }
        //}
        //public static readonly DependencyProperty MyOffsetLeftProperty =
        //    DependencyProperty.Register(nameof(MyOffsetLeft), typeof(double), typeof(ItemData), new PropertyMetadata(0.0));

        //public double MyOffsetTop
        //{
        //    get { return (double)GetValue(MyOffsetTopProperty); }
        //    internal set { SetValue(MyOffsetTopProperty, value); }
        //}
        //public static readonly DependencyProperty MyOffsetTopProperty =
        //    DependencyProperty.Register(nameof(MyOffsetTop), typeof(double), typeof(ItemData), new PropertyMetadata(0.0));


        #endregion 共通

        #region 図形関連
        #region 図形基本

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(ItemData),
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
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(ItemData), new FrameworkPropertyMetadata(1.0,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Brush MyStroke
        {
            get { return (Brush)GetValue(MyStrokeProperty); }
            set { SetValue(MyStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeProperty =
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(ItemData), new FrameworkPropertyMetadata(Brushes.Magenta,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        public Brush MyFill
        {
            get { return (Brush)GetValue(MyFillProperty); }
            set { SetValue(MyFillProperty, value); }
        }
        public static readonly DependencyProperty MyFillProperty =
            DependencyProperty.Register(nameof(MyFill), typeof(Brush), typeof(ItemData), new FrameworkPropertyMetadata(Brushes.Pink,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        public bool MyIsFilled
        {
            get { return (bool)GetValue(MyIsFilledProperty); }
            set { SetValue(MyIsFilledProperty, value); }
        }
        public static readonly DependencyProperty MyIsFilledProperty =
            DependencyProperty.Register(nameof(MyIsFilled), typeof(bool), typeof(ItemData), new FrameworkPropertyMetadata(true,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        public bool MyIsStroked
        {
            get { return (bool)GetValue(MyIsStrokedProperty); }
            set { SetValue(MyIsStrokedProperty, value); }
        }
        public static readonly DependencyProperty MyIsStrokedProperty =
            DependencyProperty.Register(nameof(MyIsStroked), typeof(bool), typeof(ItemData), new FrameworkPropertyMetadata(true,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        public bool MyIsClosed
        {
            get { return (bool)GetValue(MyIsClosedProperty); }
            set { SetValue(MyIsClosedProperty, value); }
        }
        public static readonly DependencyProperty MyIsClosedProperty =
            DependencyProperty.Register(nameof(MyIsClosed), typeof(bool), typeof(ItemData), new FrameworkPropertyMetadata(false,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        public bool MyIsSmoothJoin
        {
            get { return (bool)GetValue(MyIsSmoothJoinProperty); }
            set { SetValue(MyIsSmoothJoinProperty, value); }
        }
        public static readonly DependencyProperty MyIsSmoothJoinProperty =
            DependencyProperty.Register(nameof(MyIsSmoothJoin), typeof(bool), typeof(ItemData),
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
            DependencyProperty.Register(nameof(MyFillRule), typeof(FillRule), typeof(ItemData),
                new FrameworkPropertyMetadata(FillRule.Nonzero,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public PenLineJoin MyStrokeLineJoin
        {
            get { return (PenLineJoin)GetValue(MyStrokeLineJoinProperty); }
            set { SetValue(MyStrokeLineJoinProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeLineJoinProperty =
            DependencyProperty.Register(nameof(MyStrokeLineJoin), typeof(PenLineJoin), typeof(ItemData),
                new FrameworkPropertyMetadata(PenLineJoin.Miter,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 図形基本

        #region 図形細部

        public DoubleCollection MyStrokeDashArray
        {
            get { return (DoubleCollection)GetValue(MyStrokeDashArrayProperty); }
            set { SetValue(MyStrokeDashArrayProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeDashArrayProperty =
            DependencyProperty.Register(nameof(MyStrokeDashArray), typeof(DoubleCollection), typeof(ItemData),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public PenLineCap MyStrokeDashCap
        {
            get { return (PenLineCap)GetValue(MyStrokeDashCapProperty); }
            set { SetValue(MyStrokeDashCapProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeDashCapProperty =
            DependencyProperty.Register(nameof(MyStrokeDashCap), typeof(PenLineCap), typeof(ItemData),
                new FrameworkPropertyMetadata(PenLineCap.Flat,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public double MyStrokeDashOffset
        {
            get { return (double)GetValue(MyStrokeDashOffsetProperty); }
            set { SetValue(MyStrokeDashOffsetProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeDashOffsetProperty =
            DependencyProperty.Register(nameof(MyStrokeDashOffset), typeof(double), typeof(ItemData),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public PenLineCap MyStrokeEndLineCap
        {
            get { return (PenLineCap)GetValue(MyStrokeEndLineCapProperty); }
            set { SetValue(MyStrokeEndLineCapProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeEndLineCapProperty =
            DependencyProperty.Register(nameof(MyStrokeEndLineCap), typeof(PenLineCap), typeof(ItemData),
                new FrameworkPropertyMetadata(PenLineCap.Flat,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public PenLineCap MyStrokeStartLineCap
        {
            get { return (PenLineCap)GetValue(MyStrokeStartLineCapProperty); }
            set { SetValue(MyStrokeStartLineCapProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeStartLineCapProperty =
            DependencyProperty.Register(nameof(MyStrokeStartLineCap), typeof(PenLineCap), typeof(ItemData),
                new FrameworkPropertyMetadata(PenLineCap.Flat,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public double MyStrokeMiterLimit
        {
            get { return (double)GetValue(MyStrokeMiterLimitProperty); }
            set { SetValue(MyStrokeMiterLimitProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeMiterLimitProperty =
            DependencyProperty.Register(nameof(MyStrokeMiterLimit), typeof(double), typeof(ItemData),
                new FrameworkPropertyMetadata(10.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 図形細部
        #endregion 図形関連

        #region 特殊

        //public Rect MyContentBounds
        //{
        //    get { return (Rect)GetValue(MyContentBoundsProperty); }
        //    set { SetValue(MyContentBoundsProperty, value); }
        //}
        //public static readonly DependencyProperty MyContentBoundsProperty =
        //    DependencyProperty.Register(nameof(MyContentBounds), typeof(Rect), typeof(ItemData), new PropertyMetadata(new Rect()));

        #endregion 特殊
        #endregion 依存関係プロパティ

    }
}
