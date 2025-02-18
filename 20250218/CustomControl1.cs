using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250218
{

    public enum DashColor { Transparent = 0, A, B, C, D, E, F }




    public class DashBorder : Control
    {

        static DashBorder()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DashBorder), new FrameworkPropertyMetadata(typeof(DashBorder)));
        }
        public DashBorder()
        {
            DataContext = this;

        }


        #region 依存関係プロパティ


        public DoubleCollection MyStrokeDashArray
        {
            get { return (DoubleCollection)GetValue(MyStrokeDashArrayProperty); }
            set { SetValue(MyStrokeDashArrayProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeDashArrayProperty =
            DependencyProperty.Register(nameof(MyStrokeDashArray), typeof(DoubleCollection), typeof(DashBorder), new PropertyMetadata(new DoubleCollection() { 4, 4 }));


        public double MyBorderThickness
        {
            get { return (double)GetValue(MyBorderThicknessProperty); }
            set { SetValue(MyBorderThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyBorderThicknessProperty =
            DependencyProperty.Register(nameof(MyBorderThickness), typeof(double), typeof(DashBorder), new PropertyMetadata(1.0));



        #region Brashes

        public Brush MyDashBrushF
        {
            get { return (Brush)GetValue(MyDashBrushFProperty); }
            set { SetValue(MyDashBrushFProperty, value); }
        }
        public static readonly DependencyProperty MyDashBrushFProperty =
            DependencyProperty.Register(nameof(MyDashBrushF), typeof(Brush), typeof(DashBorder), new PropertyMetadata(Brushes.Silver));

        public Brush MyDashBrushE
        {
            get { return (Brush)GetValue(MyDashBrushEProperty); }
            set { SetValue(MyDashBrushEProperty, value); }
        }
        public static readonly DependencyProperty MyDashBrushEProperty =
            DependencyProperty.Register(nameof(MyDashBrushE), typeof(Brush), typeof(DashBorder), new PropertyMetadata(Brushes.Orange));

        public Brush MyDashBrushD
        {
            get { return (Brush)GetValue(MyDashBrushDProperty); }
            set { SetValue(MyDashBrushDProperty, value); }
        }
        public static readonly DependencyProperty MyDashBrushDProperty =
            DependencyProperty.Register(nameof(MyDashBrushD), typeof(Brush), typeof(DashBorder), new PropertyMetadata(Brushes.LimeGreen));

        public Brush MyDashBrushC
        {
            get { return (Brush)GetValue(MyDashBrushCProperty); }
            set { SetValue(MyDashBrushCProperty, value); }
        }
        public static readonly DependencyProperty MyDashBrushCProperty =
            DependencyProperty.Register(nameof(MyDashBrushC), typeof(Brush), typeof(DashBorder), new PropertyMetadata(Brushes.Blue));

        public Brush MyDashBrushB
        {
            get { return (Brush)GetValue(MyDashBrushBProperty); }
            set { SetValue(MyDashBrushBProperty, value); }
        }
        public static readonly DependencyProperty MyDashBrushBProperty =
            DependencyProperty.Register(nameof(MyDashBrushB), typeof(Brush), typeof(DashBorder), new PropertyMetadata(Brushes.DodgerBlue));

        public Brush MyDashBrushA
        {
            get { return (Brush)GetValue(MyDashBrushAProperty); }
            set { SetValue(MyDashBrushAProperty, value); }
        }
        public static readonly DependencyProperty MyDashBrushAProperty =
            DependencyProperty.Register(nameof(MyDashBrushA), typeof(Brush), typeof(DashBorder), new PropertyMetadata(Brushes.OrangeRed));

        public Brush MyDashBrushTransparent
        {
            get { return (Brush)GetValue(MyDashBrushTransparentProperty); }
            set { SetValue(MyDashBrushTransparentProperty, value); }
        }
        public static readonly DependencyProperty MyDashBrushTransparentProperty =
            DependencyProperty.Register(nameof(MyDashBrushTransparent), typeof(Brush), typeof(DashBorder), new PropertyMetadata(Brushes.Transparent));


        public Brush MyLowerBrush
        {
            get { return (Brush)GetValue(MyLowerBrushProperty); }
            set { SetValue(MyLowerBrushProperty, value); }
        }
        public static readonly DependencyProperty MyLowerBrushProperty =
            DependencyProperty.Register(nameof(MyLowerBrush), typeof(Brush), typeof(DashBorder), new PropertyMetadata(Brushes.White));
        #endregion Brashes


        public DashColor MyDashColorType
        {
            get { return (DashColor)GetValue(MyDashColorTypeProperty); }
            set { SetValue(MyDashColorTypeProperty, value); }
        }
        public static readonly DependencyProperty MyDashColorTypeProperty =
            DependencyProperty.Register(nameof(MyDashColorType), typeof(DashColor), typeof(DashBorder), new PropertyMetadata(DashColor.A));
        #endregion 依存関係プロパティ


    }



    public class KisoThumb : Thumb
    {
        static KisoThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KisoThumb), new FrameworkPropertyMetadata(typeof(KisoThumb)));
        }
        public KisoThumb()
        {
            DataContext = this;
            DragDelta += KisoThumb_DragDelta;
        }

        private void KisoThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is KisoThumb kiso)
            {
                kiso.MyLeft += e.HorizontalChange;
                kiso.MyTop += e.VerticalChange;
                e.Handled = true;
            }
        }

        //public override void OnApplyTemplate()
        //{
        //    base.OnApplyTemplate();
        //    if (GetTemplateChild("PART_Canvas") is Canvas panel)
        //    {
        //        MyBaseCanvas = panel;
        //    }
        //}

        #region 依存関係プロパティ

        #region 共通

        //テキスト系要素のText要素
        public string MyText
        {
            get { return (string)GetValue(MyTextProperty); }
            set { SetValue(MyTextProperty, value); }
        }
        public static readonly DependencyProperty MyTextProperty =
            DependencyProperty.Register(nameof(MyText), typeof(string), typeof(KisoThumb),
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
            DependencyProperty.Register(nameof(MyAngle), typeof(double), typeof(KisoThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        //public Canvas MyBaseCanvas
        //{
        //    get { return (Canvas)GetValue(MyBaseCanvasProperty); }
        //    private set { SetValue(MyBaseCanvasProperty, value); }
        //}
        //public static readonly DependencyProperty MyBaseCanvasProperty =
        //    DependencyProperty.Register(nameof(MyBaseCanvas), typeof(Canvas), typeof(KisoThumb), new PropertyMetadata(null));


        public double MyLeft
        {
            get { return (double)GetValue(MyLeftProperty); }
            set { SetValue(MyLeftProperty, value); }
        }
        public static readonly DependencyProperty MyLeftProperty =
            DependencyProperty.Register(nameof(MyLeft), typeof(double), typeof(KisoThumb),
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
            DependencyProperty.Register(nameof(MyTop), typeof(double), typeof(KisoThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public double MyOffsetLeft
        {
            get { return (double)GetValue(MyOffsetLeftProperty); }
            internal set { SetValue(MyOffsetLeftProperty, value); }
        }
        public static readonly DependencyProperty MyOffsetLeftProperty =
            DependencyProperty.Register(nameof(MyOffsetLeft), typeof(double), typeof(KisoThumb), new PropertyMetadata(0.0));

        public double MyOffsetTop
        {
            get { return (double)GetValue(MyOffsetTopProperty); }
            internal set { SetValue(MyOffsetTopProperty, value); }
        }
        public static readonly DependencyProperty MyOffsetTopProperty =
            DependencyProperty.Register(nameof(MyOffsetTop), typeof(double), typeof(KisoThumb), new PropertyMetadata(0.0));



        public double MyBorderThickness
        {
            get { return (double)GetValue(MyBorderThicknessProperty); }
            set { SetValue(MyBorderThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyBorderThicknessProperty =
            DependencyProperty.Register(nameof(MyBorderThickness), typeof(double), typeof(KisoThumb), new PropertyMetadata(1.0));


        public DashColor MyDashColorType
        {
            get { return (DashColor)GetValue(MyDashColorTypeProperty); }
            set { SetValue(MyDashColorTypeProperty, value); }
        }
        public static readonly DependencyProperty MyDashColorTypeProperty =
            DependencyProperty.Register(nameof(MyDashColorType), typeof(DashColor), typeof(KisoThumb), new PropertyMetadata(DashColor.A));


        #endregion 共通

        #region 図形関連
        #region 図形基本

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(KisoThumb),
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
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(KisoThumb), new FrameworkPropertyMetadata(1.0,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Brush MyStroke
        {
            get { return (Brush)GetValue(MyStrokeProperty); }
            set { SetValue(MyStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeProperty =
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(KisoThumb), new FrameworkPropertyMetadata(Brushes.Magenta,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        public Brush MyFill
        {
            get { return (Brush)GetValue(MyFillProperty); }
            set { SetValue(MyFillProperty, value); }
        }
        public static readonly DependencyProperty MyFillProperty =
            DependencyProperty.Register(nameof(MyFill), typeof(Brush), typeof(KisoThumb), new FrameworkPropertyMetadata(Brushes.Pink,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        public bool MyIsFilled
        {
            get { return (bool)GetValue(MyIsFilledProperty); }
            set { SetValue(MyIsFilledProperty, value); }
        }
        public static readonly DependencyProperty MyIsFilledProperty =
            DependencyProperty.Register(nameof(MyIsFilled), typeof(bool), typeof(KisoThumb), new FrameworkPropertyMetadata(true,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        public bool MyIsStroked
        {
            get { return (bool)GetValue(MyIsStrokedProperty); }
            set { SetValue(MyIsStrokedProperty, value); }
        }
        public static readonly DependencyProperty MyIsStrokedProperty =
            DependencyProperty.Register(nameof(MyIsStroked), typeof(bool), typeof(KisoThumb), new FrameworkPropertyMetadata(true,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        public bool MyIsClosed
        {
            get { return (bool)GetValue(MyIsClosedProperty); }
            set { SetValue(MyIsClosedProperty, value); }
        }
        public static readonly DependencyProperty MyIsClosedProperty =
            DependencyProperty.Register(nameof(MyIsClosed), typeof(bool), typeof(KisoThumb), new FrameworkPropertyMetadata(false,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        public bool MyIsSmoothJoin
        {
            get { return (bool)GetValue(MyIsSmoothJoinProperty); }
            set { SetValue(MyIsSmoothJoinProperty, value); }
        }
        public static readonly DependencyProperty MyIsSmoothJoinProperty =
            DependencyProperty.Register(nameof(MyIsSmoothJoin), typeof(bool), typeof(KisoThumb),
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
            DependencyProperty.Register(nameof(MyFillRule), typeof(FillRule), typeof(KisoThumb),
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
            DependencyProperty.Register(nameof(MyStrokeLineJoin), typeof(PenLineJoin), typeof(KisoThumb),
                new FrameworkPropertyMetadata(PenLineJoin.Miter,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 図形基本

        #endregion 図形関連

        #endregion 依存関係プロパティ

    }




    public class TextThumb : KisoThumb
    {
        static TextThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextThumb), new FrameworkPropertyMetadata(typeof(TextThumb)));
        }
        public TextThumb()
        {
            DragDelta += TextThumb_DragDelta;
        }

        private void TextThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is TextThumb t)
            {
                t.MyLeft += e.HorizontalChange;
                t.MyTop += e.VerticalChange;
                e.Handled = true;
            }
        }


    }

    #region コンバーター
    
    public class MyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, System.Type targetType, object parameter, CultureInfo culture)
        {
            var type = (DashColor)values[0];
            var iro1 = (Brush)values[1];
            var iro2 = (Brush)values[2];
            var iro3 = (Brush)values[3];
            var iro4 = (Brush)values[4];
            var iro5 = (Brush)values[5];
            var iro6 = (Brush)values[6];
            var iro7 = (Brush)values[7];
            return type switch
            {
                DashColor.Transparent => iro1,
                DashColor.A => iro2,
                DashColor.B => iro3,
                DashColor.C => iro4,
                DashColor.D => iro5,
                DashColor.E => iro6,
                DashColor.F => iro7,
                _ => iro1
            };
        }

        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConverterThickness : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var v = (double)value;
            return new Thickness(v);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (Thickness)value;
            return v.Top;
        }
    }

    #endregion コンバーター


}
