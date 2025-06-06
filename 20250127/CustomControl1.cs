﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250127
{

    public class CanvasThumb : Thumb
    {
        public Canvas? MyBasePanel { get; set; }
        static CanvasThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CanvasThumb), new FrameworkPropertyMetadata(typeof(CanvasThumb)));
        }
        public CanvasThumb()
        {
            DragDelta += CanvasThumb_DragDelta;
        }

        private void CanvasThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                var left = Canvas.GetLeft(t);
                Canvas.SetLeft(t, Canvas.GetLeft(t) + e.HorizontalChange);
                Canvas.SetTop(t, Canvas.GetTop(t) + e.VerticalChange);
                e.Handled = true;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_Canvas") is Canvas panel)
            {
                MyBasePanel = panel;
            }
        }
    }

    public class WithAnchorThumb : CanvasThumb
    {
        //Canvas? MyBasePanel { get; set; }
        Thumb? MyAnchorThumb { get; set; }
        static WithAnchorThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WithAnchorThumb), new FrameworkPropertyMetadata(typeof(WithAnchorThumb)));
        }
        public WithAnchorThumb()
        {

        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("AnchorThumb") is Thumb anchor)
            {
                MyAnchorThumb = anchor;
                MyAnchorThumb.DragDelta += MyAnchorThumb_DragDelta;
            }
        }

        private void MyAnchorThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb anchor)
            {
                Canvas.SetLeft(anchor, Math.Max(1, Canvas.GetLeft(anchor) + e.HorizontalChange));
                Canvas.SetTop(anchor, Math.Max(1, Canvas.GetTop(anchor) + e.VerticalChange));
                e.Handled = true;
            }
        }
    }

    public class EzLineThumb : WithAnchorThumb
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
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(EzLineThumb),
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
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(EzLineThumb), new FrameworkPropertyMetadata(1.0,
                FrameworkPropertyMetadataOptions.AffectsRender |// デザイン画面上での更新で必要
                FrameworkPropertyMetadataOptions.AffectsMeasure)); // 必要ないかも？

        public Brush MyStroke
        {
            get { return (Brush)GetValue(MyStrokeProperty); }
            set { SetValue(MyStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeProperty =
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(EzLineThumb), new FrameworkPropertyMetadata(Brushes.Magenta,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Brush MyFill
        {
            get { return (Brush)GetValue(MyFillProperty); }
            set { SetValue(MyFillProperty, value); }
        }
        public static readonly DependencyProperty MyFillProperty =
            DependencyProperty.Register(nameof(MyFill), typeof(Brush), typeof(EzLineThumb), new FrameworkPropertyMetadata(Brushes.Pink,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        public bool MyIsFilled
        {
            get { return (bool)GetValue(MyIsFilledProperty); }
            set { SetValue(MyIsFilledProperty, value); }
        }
        public static readonly DependencyProperty MyIsFilledProperty =
            DependencyProperty.Register(nameof(MyIsFilled), typeof(bool), typeof(EzLineThumb), new FrameworkPropertyMetadata(true,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        public bool MyIsStroked
        {
            get { return (bool)GetValue(MyIsStrokedProperty); }
            set { SetValue(MyIsStrokedProperty, value); }
        }
        public static readonly DependencyProperty MyIsStrokedProperty =
            DependencyProperty.Register(nameof(MyIsStroked), typeof(bool), typeof(EzLineThumb), new FrameworkPropertyMetadata(true,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        public bool MyIsClosed
        {
            get { return (bool)GetValue(MyIsClosedProperty); }
            set { SetValue(MyIsClosedProperty, value); }
        }
        public static readonly DependencyProperty MyIsClosedProperty =
            DependencyProperty.Register(nameof(MyIsClosed), typeof(bool), typeof(EzLineThumb), new FrameworkPropertyMetadata(false,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        public bool MyIsSmoothJoin
        {
            get { return (bool)GetValue(MyIsSmoothJoinProperty); }
            set { SetValue(MyIsSmoothJoinProperty, value); }
        }
        public static readonly DependencyProperty MyIsSmoothJoinProperty =
            DependencyProperty.Register(nameof(MyIsSmoothJoin), typeof(bool), typeof(EzLineThumb),
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
            DependencyProperty.Register(nameof(MyFillRule), typeof(FillRule), typeof(EzLineThumb),
                new FrameworkPropertyMetadata(FillRule.Nonzero,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public bool MyIsOffsetShape
        {
            get { return (bool)GetValue(MyIsOffsetShapeProperty); }
            set { SetValue(MyIsOffsetShapeProperty, value); }
        }
        public static readonly DependencyProperty MyIsOffsetShapeProperty =
            DependencyProperty.Register(nameof(MyIsOffsetShape), typeof(bool), typeof(EzLineThumb), new FrameworkPropertyMetadata(false,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        #endregion 依存関係プロパティ


        #region 読み取り専用依存関係プロパティ

        private static readonly DependencyPropertyKey MyLinePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyLine), typeof(EzLine), typeof(EzLineThumb), new PropertyMetadata(null));
        public static readonly DependencyProperty MyLineProperty = MyLinePropertyKey.DependencyProperty;
        public EzLine MyLine
        {
            get { return (EzLine)GetValue(MyLinePropertyKey.DependencyProperty); }
            internal set { SetValue(MyLinePropertyKey, value); }
        }


        //private static readonly DependencyPropertyKey MyLineWithPenBoundsPropertyKey =
        //    DependencyProperty.RegisterReadOnly(nameof(MyLineWithPenBounds), typeof(Rect), typeof(EzLineThumb), new PropertyMetadata(new Rect()));
        //public static readonly DependencyProperty MyLineWithPenBoundsProperty = MyLineWithPenBoundsPropertyKey.DependencyProperty;
        //public Rect MyLineWithPenBounds
        //{
        //    get { return (Rect)GetValue(MyLineWithPenBoundsPropertyKey.DependencyProperty); }
        //    internal set { SetValue(MyLineWithPenBoundsPropertyKey, value); }
        //}






        #endregion 読み取り専用依存関係プロパティ

        static EzLineThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EzLineThumb), new FrameworkPropertyMetadata(typeof(EzLineThumb)));
        }
        public EzLineThumb()
        {
            DataContext = this;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("EzLine") is EzLine line)
            {
                MyLine = line;
                //BindingOperations.SetBinding(this, MyLineWithPenBoundsProperty, new Binding() { Source = MyLine, Path = new PropertyPath(EzLine.MyBoundsWithPenProperty) });

                var neko = MyPoints;



            }
        }
    }



    public class EzLine : Control
    {
        #region 依存関係プロパティ
        // FrameworkPropertyMetadataOptions.AffectsRender // デザイン画面上での更新で必要
        // FrameworkPropertyMetadataOptions.AffectsMeasure // 必要ないかも？


        //線の描画を考慮したBoundsを使ってオフセット表示をする
        public bool MyIsOffset
        {
            get { return (bool)GetValue(MyIsOffsetProperty); }
            set { SetValue(MyIsOffsetProperty, value); }
        }
        public static readonly DependencyProperty MyIsOffsetProperty =
            DependencyProperty.Register(nameof(MyIsOffset), typeof(bool), typeof(EzLine), new FrameworkPropertyMetadata(false,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


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


        private static readonly DependencyPropertyKey MyNegativeOffsetLeftPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyNegativeOffsetLeft), typeof(double), typeof(EzLine), new PropertyMetadata(0.0));
        public static readonly DependencyProperty MyNegativeOffsetLeftProperty = MyNegativeOffsetLeftPropertyKey.DependencyProperty;
        public double MyNegativeOffsetLeft
        {
            get { return (double)GetValue(MyNegativeOffsetLeftPropertyKey.DependencyProperty); }
            internal set { SetValue(MyNegativeOffsetLeftPropertyKey, value); }
        }


        private static readonly DependencyPropertyKey MyNegativeOffsetTopPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyNegativeOffsetTop), typeof(double), typeof(EzLine), new PropertyMetadata(0.0));
        public static readonly DependencyProperty MyNegativeOffsetTopProperty = MyNegativeOffsetTopPropertyKey.DependencyProperty;
        public double MyNegativeOffsetTop
        {
            get { return (double)GetValue(MyNegativeOffsetTopPropertyKey.DependencyProperty); }
            internal set { SetValue(MyNegativeOffsetTopPropertyKey, value); }
        }




        #endregion 読み取り専用依存関係プロパティ

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
        }

        //描画直後に線(Pen)の描画を考慮した位置とサイズの取得
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            var bounds = MyPath.Data.GetRenderBounds(MyPen);
            //var bounds = MyPath.Data.GetWidenedPathGeometry(MyPen).Bounds;
            if (bounds.Width > 0)
            {
                MyBoundsWithPen = bounds;
                
                MyNegativeOffsetLeft = -bounds.Left;
                MyNegativeOffsetTop = -bounds.Top;
                ////オフセット表示
                //if (MyIsOffset)
                //{
                //    Canvas.SetLeft(this, -bounds.Left);
                //    Canvas.SetTop(this, -bounds.Top);
                //}
            }
        }
    }






    #region コンバーター

    public class MyConverterOffset : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = (bool)values[0];
            double d = (double)values[1];
            if (b) { return d; }
            else { return 0.0; }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConverterRectHeight : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Rect r = (Rect)value;
            return r.Height;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConverterRectWidth : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Rect r = (Rect)value;
            return r.Width;
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
