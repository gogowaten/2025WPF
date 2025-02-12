using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace _20250212
{
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

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_Canvas") is Canvas panel)
            {
                MyBaseCanvas = panel;
            }
        }

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



        public Canvas MyBaseCanvas
        {
            get { return (Canvas)GetValue(MyBaseCanvasProperty); }
            private set { SetValue(MyBaseCanvasProperty, value); }
        }
        public static readonly DependencyProperty MyBaseCanvasProperty =
            DependencyProperty.Register(nameof(MyBaseCanvas), typeof(Canvas), typeof(KisoThumb), new PropertyMetadata(null));


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

        #region 図形細部

        public DoubleCollection MyStrokeDashArray
        {
            get { return (DoubleCollection)GetValue(MyStrokeDashArrayProperty); }
            set { SetValue(MyStrokeDashArrayProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeDashArrayProperty =
            DependencyProperty.Register(nameof(MyStrokeDashArray), typeof(DoubleCollection), typeof(KisoThumb),
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
            DependencyProperty.Register(nameof(MyStrokeDashCap), typeof(PenLineCap), typeof(KisoThumb),
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
            DependencyProperty.Register(nameof(MyStrokeDashOffset), typeof(double), typeof(KisoThumb),
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
            DependencyProperty.Register(nameof(MyStrokeEndLineCap), typeof(PenLineCap), typeof(KisoThumb),
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
            DependencyProperty.Register(nameof(MyStrokeStartLineCap), typeof(PenLineCap), typeof(KisoThumb),
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
            DependencyProperty.Register(nameof(MyStrokeMiterLimit), typeof(double), typeof(KisoThumb),
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
        //    DependencyProperty.Register(nameof(MyContentBounds), typeof(Rect), typeof(KisoThumb), new PropertyMetadata(new Rect()));

        #endregion 特殊
        #endregion 依存関係プロパティ

    }


    public class EzLineThumb : KisoThumb
    {
        public ObservableCollection<Thumb> MyAnchors { get; private set; } = [];
        static EzLineThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EzLineThumb), new FrameworkPropertyMetadata(typeof(EzLineThumb)));
        }
        public EzLineThumb()
        {
            Loaded += EzLineThumb_Loaded;
        }

        private void EzLineThumb_Loaded(object sender, RoutedEventArgs e)
        {

            //Width = MyEzLine.MyBounds4.Width;
            //Height = MyEzLine.MyBounds4.Height;
            OffsetEzLineAndThis();
            RepairSize();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //テンプレートの中からEzLineを取得してバインド設定
            if (GetTemplateChild("ez") is EzLine ez)
            {
                MyEzLine = ez;

                //オフセット位置のバインド
                //このタイミングじゃないとバインドできないし、XAMLでもできない。
                //ソースにEzLineを使っているから、取得後じゃないとできないみたい
                MultiBinding mb = new() { Converter = new MyConverterOffsetX() };
                mb.Bindings.Add(MakeOneWayBind(MyLeftProperty));
                mb.Bindings.Add(new Binding() { Source = MyEzLine, Path = new PropertyPath(EzLine.MyBounds4Property), Mode = BindingMode.OneWay });
                SetBinding(MyOffsetLeftProperty, mb);

                mb = new() { Converter = new MyConverterOffsetY() };
                mb.Bindings.Add(MakeOneWayBind(MyTopProperty));
                mb.Bindings.Add(new Binding() { Source = MyEzLine, Path = new PropertyPath(EzLine.MyBounds4Property), Mode = BindingMode.OneWay });
                SetBinding(MyOffsetTopProperty, mb);

                //MyBindRotateOffset();
            }
        }

        //private void MyBindRotateOffset()
        //{
        //    MultiBinding b = new() { Converter = new MyConverterBoundsLeft() };
        //    b.Bindings.Add(new Binding() { Source = MyEzLine, Path = new PropertyPath(EzLine.MyBounds4Property) });
        //    b.Bindings.Add(new Binding() { Source = MyEzLine, Path = new PropertyPath(EzLine.MyBounds2Property) });
        //    SetBinding(MyLeftProperty, b);

        //    b = new() { Converter = new MyConverterBoundsTop() };
        //    b.Bindings.Add(new Binding() { Source = MyEzLine, Path = new PropertyPath(EzLine.MyBounds4Property) });
        //    b.Bindings.Add(new Binding() { Source = MyEzLine, Path = new PropertyPath(EzLine.MyBounds2Property) });
        //    SetBinding(MyTopProperty, b);



        //}

        /// <summary>
        /// Binding生成。ソースはthis固定、ModeはOneWay固定
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private Binding MakeOneWayBind(DependencyProperty property)
        {
            return new Binding() { Source = this, Path = new PropertyPath(property), Mode = BindingMode.OneWay };
        }


        public EzLine MyEzLine
        {
            get { return (EzLine)GetValue(MyEzLineProperty); }
            protected set { SetValue(MyEzLineProperty, value); }
        }
        public static readonly DependencyProperty MyEzLineProperty =
            DependencyProperty.Register(nameof(MyEzLine), typeof(EzLine), typeof(EzLineThumb), new PropertyMetadata(null));


        public void OffsetEzLine()
        {
            RepairSize();
            var bounds4 = MyEzLine.MyBounds4;
            //var ezleft = Canvas.GetLeft(MyEzLine);
            //var eztop = Canvas.GetTop(MyEzLine);
            //var bTop = bounds4.Top;
            //var ezTopHenka = eztop + bTop;

            MyLeft += Canvas.GetLeft(MyEzLine) + bounds4.Left;
            MyTop += Canvas.GetTop(MyEzLine) + bounds4.Top;
            Canvas.SetLeft(MyEzLine, -bounds4.Left);
            Canvas.SetTop(MyEzLine, -bounds4.Top);
        }

        public void OffsetEzLineAndThis2()
        {
            var ezleft = Canvas.GetLeft(MyEzLine);
            var eztop = Canvas.GetTop(MyEzLine);
            var bounds4 = MyEzLine.MyBounds4;
            var bounds2 = MyEzLine.MyBounds2;
            var b4top = bounds4.Top;
            if (ezleft != -bounds4.Left)
            {
                Canvas.SetLeft(MyEzLine, -bounds4.X);
                MyLeft += bounds4.X - bounds2.X;
            }
            if (eztop != -bounds4.Top)
            {
                Canvas.SetTop(MyEzLine, -bounds4.Y);
                var neko = bounds4.Y - bounds2.Y;
                var inu = MyTop + neko;
                MyTop += bounds4.Y - bounds2.Y;
            }

        }

        public void OffsetEzLineAndThis()
        {
            var bounds4 = MyEzLine.MyBounds4;
            Canvas.SetLeft(MyEzLine, -bounds4.X);
            Canvas.SetTop(MyEzLine, -bounds4.Y);

            var bounds2 = MyEzLine.MyBounds2;
            //MyLeft += bounds2.X - bounds4.X;
            //MyTop += bounds2.Y - bounds4.Y;

            //回転したときだけ実行したい
            MyLeft += bounds4.X - bounds2.X;
            MyTop += bounds4.Y - bounds2.Y;
        }

      

        public void RepairSize()
        {
            var bounds = MyEzLine.MyBounds4;
            Width = bounds.Width;
            Height = bounds.Height;

        }

    }




    #region コンバーター

    //public class MyConverterBoundsLeft : IMultiValueConverter
    //{
    //    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        var r4 = (Rect)values[0];
    //        var r2 = (Rect)values[1];
    //        return r4.Left - r2.Left;
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class MyConverterBoundsTop : IMultiValueConverter
    //{
    //    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        var r4 = (Rect)values[0];
    //        var r2 = (Rect)values[1];
    //        return r4.Top - r2.Top;
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}


    //指定表示座標とコンテンツの変形後のRectから、自身の実際のY座標を返す
    public class MyConverterOffsetY : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var y = (double)values[0];
            var r = (Rect)values[1];
            return y + r.Top;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //指定表示座標とコンテンツの変形後のRectから、自身の実際のX座標を返す
    public class MyConverterOffsetX : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var x = (double)values[0];
            var r = (Rect)values[1];
            return x + r.Left;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion コンバーター
}
