using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250115
{
    public class KnobThumb : Thumb
    {
        static KnobThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KnobThumb), new FrameworkPropertyMetadata(typeof(KnobThumb)));
        }
        public KnobThumb()
        {

        }

    }
    public class RangeThumb2 : Thumb
    {
        Panel? MyPanel;
        //Rectangle MyRect;


        private KnobThumb MyThumb0 = new();
        private KnobThumb MyThumb1 = new();
        private KnobThumb MyThumb2 = new();
        private KnobThumb MyThumb3 = new();
        private KnobThumb MyThumb4 = new();
        private KnobThumb MyThumb5 = new();
        private KnobThumb MyThumb6 = new();
        private KnobThumb MyThumb7 = new();


        static RangeThumb2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeThumb2), new FrameworkPropertyMetadata(typeof(RangeThumb2)));
        }
        public RangeThumb2()
        {
            Loaded += RangeThumb2_Loaded;
            DragDelta += RangeThumb2_DragDelta;
            MyThumb7.DragDelta += MyThumb7_DragDelta;
        }

        private void RangeThumb2_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                var left = Canvas.GetLeft(t) + e.HorizontalChange;
                var top = Canvas.GetTop(t) + e.VerticalChange;
                if (left < 0) { left = 0; }
                if (top < 0) { top = 0; }
                Canvas.SetLeft(t, left);
                Canvas.SetTop(t, top);
                e.Handled = true;
            }

        }

        private void MyThumb7_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                var left = Canvas.GetLeft(t) + e.HorizontalChange;
                var top = Canvas.GetTop(t) + e.VerticalChange;
                if (left < 1) { left = 1; }
                if (top < 1) { top = 1; }
                Canvas.SetLeft(t, left);
                Canvas.SetTop(t, top);
                e.Handled = true;
            }

        }

        private void RangeThumb2_Loaded(object sender, RoutedEventArgs e)
        {
            if (GetTemplateChild("MyPanel") is Panel panel)
            {
                MyPanel = panel;
                MyPanel.Width = 100; MyPanel.Height = 100; MyPanel.Background = Brushes.Red;
                MyPanel.Children.Add(MyThumb7);
                MyThumb7.SetBinding(Canvas.LeftProperty, new Binding() { Source = MyPanel, Path = new PropertyPath(WidthProperty), Mode = BindingMode.TwoWay });
                MyThumb7.SetBinding(Canvas.TopProperty, new Binding() { Source = MyPanel, Path = new PropertyPath(HeightProperty), Mode = BindingMode.TwoWay });

            }
        }
    }

    public class RangeThumb : Thumb
    {
        Grid? MyPanel;
        Rectangle MyRect;


        private KnobThumb MyThumb0 = new();
        private KnobThumb MyThumb1 = new();
        private KnobThumb MyThumb2 = new();
        private KnobThumb MyThumb3 = new();
        private KnobThumb MyThumb4 = new();
        private KnobThumb MyThumb5 = new();
        private KnobThumb MyThumb6 = new();
        private KnobThumb MyThumb7 = new();


        static RangeThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeThumb), new FrameworkPropertyMetadata(typeof(RangeThumb)));
        }
        public RangeThumb()
        {

            Loaded += RangeThumb_Loaded;
            MyRect = new() { Width = 200, Height = 200, Fill = Brushes.DodgerBlue };

            MyThumb0.DragDelta += MyThumb_DragDelta;
            MyThumb2.DragDelta += MyThumb_DragDelta;
            MyThumb5.DragDelta += MyThumb_DragDelta;
            MyThumb7.DragDelta += MyThumb_DragDelta;
            MyThumb1.DragDelta += MyThumb_DragDeltaOnlyVertical;
            MyThumb6.DragDelta += MyThumb_DragDeltaOnlyVertical;
            MyThumb3.DragDelta += MyThumb_DragDeltaOnlyHorizontal;
            MyThumb4.DragDelta += MyThumb_DragDeltaOnlyHorizontal;
            DragDelta += My_DragDelta;

        }


        private void RangeThumb_Loaded(object sender, RoutedEventArgs e)
        {
            if (GetTemplateChild("MyPanel") is Grid panel)
            {
                MyPanel = panel;
                MyPanel.Children.Add(MyRect);
                //MyPanel.Children.Add(MyThumb0);
                //MyPanel.Children.Add(MyThumb1);
                //MyPanel.Children.Add(MyThumb2);
                //MyPanel.Children.Add(MyThumb3);
                //MyPanel.Children.Add(MyThumb4);
                //MyPanel.Children.Add(MyThumb5);
                //MyPanel.Children.Add(MyThumb6);
                MyPanel.Children.Add(MyThumb7);
                MyThumb7.SetBinding(Canvas.LeftProperty, new Binding() { Source = MyRect, Path = new PropertyPath(WidthProperty) });
                MyThumb7.SetBinding(Canvas.TopProperty, new Binding() { Source = MyRect, Path = new PropertyPath(HeightProperty) });

            }
        }


        #region ドラッグ移動        

        private void My_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is RangeThumb me)
            {
                double left = Math.Max(0, Canvas.GetLeft(me) + e.HorizontalChange);
                double top = Math.Max(0, Canvas.GetTop(me) + e.VerticalChange);
                //Canvas.SetLeft(me, Math.Max(0, Canvas.GetLeft(me) + e.HorizontalChange));
                //Canvas.SetTop(me, Math.Max(0, Canvas.GetTop(me) + e.VerticalChange));
                Canvas.SetLeft(me, left);
                Canvas.SetTop(me, top);
            }

        }



        private void MyThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                var h = Canvas.GetLeft(t);
                var v = Canvas.GetTop(t);
                Canvas.SetLeft(t, h);
                Canvas.SetTop(t, v);
                e.Handled = true;
            }

        }
        private void MyThumb_DragDeltaOnlyVertical(object sender, DragDeltaEventArgs e)
        {
            if (sender is not Thumb t) { return; }
            Canvas.SetTop(t, Canvas.GetTop(t) + e.VerticalChange);
            e.Handled = true;
        }
        private void MyThumb_DragDeltaOnlyHorizontal(object sender, DragDeltaEventArgs e)
        {
            if (sender is not Thumb t) { return; }
            Canvas.SetLeft(t, Canvas.GetLeft(t) + e.HorizontalChange);
            e.Handled = true;
        }

        #endregion ドラッグ移動        




    }



    //Thumbの種類の識別用
    public enum ThumbType { None = 0, Root, Group, Text, Ellipse, Rect, Anchor }

    public class CanvasThumb : Thumb
    {
        private Thumb MyThumb { get; set; }
        private Thumb MyThumbRight { get; set; }
        private Thumb MyThumbTopRight { get; set; }
        private Thumb MyThumbTop { get; set; }
        private Thumb MyThumbTopLeft { get; set; }
        private Thumb MyThumbLeft { get; set; }
        private Thumb MyThumbBottomLeft { get; set; }
        private Thumb MyThumbBottom { get; set; }

        static CanvasThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CanvasThumb), new FrameworkPropertyMetadata(typeof(CanvasThumb)));
        }
        public CanvasThumb()
        {
            MyThumb = new() { Width = 20, Height = 20 };
            MyThumbRight = new() { Width = 20, Height = 20 };
            MyThumbTopRight = new() { Width = 20, Height = 20 };
            MyThumbTop = new() { Width = 20, Height = 20 };
            MyThumbTopLeft = new() { Width = 20, Height = 20 };
            MyThumbLeft = new() { Width = 20, Height = 20 };
            MyThumbBottomLeft = new() { Width = 20, Height = 20 };
            MyThumbBottom = new() { Width = 20, Height = 20 };

            DragDelta += MyThumb_DragDelta;
            MyThumb.DragDelta += MyThumb_DragDelta;
            MyThumbRight.DragDelta += MyThumb_DragDelta;
            MyThumbTopRight.DragDelta += MyThumb_DragDelta;
            MyThumbTop.DragDelta += MyThumb_DragDelta;
            MyThumbTopLeft.DragDelta += MyThumb_DragDelta;
            MyThumbLeft.DragDelta += MyThumb_DragDelta;
            MyThumbBottomLeft.DragDelta += MyThumb_DragDelta;
            MyThumbBottom.DragDelta += MyThumb_DragDelta;
        }

        //private void MyThumbBottom_DragDelta(object sender, DragDeltaEventArgs e)
        //{
        //    double hh = Height + e.VerticalChange;
        //    double top = Canvas.GetTop(this) + e.VerticalChange;
        //    Height = Math.Max(1, hh);
        //    if (Height == 1 && top >= 0)
        //    {
        //        Canvas.SetTop(this, top);
        //    }
        //    e.Handled = true;
        //}

        //private void MyThumbBottomLeft_DragDelta(object sender, DragDeltaEventArgs e)
        //{
        //    Height = Math.Max(1, Height + e.VerticalChange);
        //    double top = Canvas.GetTop(this) + e.VerticalChange;
        //    if (top >= 0 && Height == 1)
        //    {
        //        Canvas.SetTop(this, top);
        //    }

        //    e.Handled = true;
        //}

        //private void MyThumbLeft_DragDelta(object sender, DragDeltaEventArgs e)
        //{
        //    double left = Canvas.GetLeft(this) + e.HorizontalChange;
        //    Canvas.SetLeft(this, Math.Max(0, left));
        //    if (left > 0)
        //    {
        //        Width = Math.Max(1, Width - e.HorizontalChange);
        //    }

        //    e.Handled = true;
        //}

        //private void MyThumbTopLeft_DragDelta(object sender, DragDeltaEventArgs e)
        //{
        //    double top = Canvas.GetTop(this) + e.VerticalChange;
        //    double left = Canvas.GetLeft(this) + e.HorizontalChange;

        //    Canvas.SetLeft(this, Math.Max(0, left));
        //    Canvas.SetTop(this, Math.Max(0, top));

        //    if (left > 0)
        //    {
        //        Width = Math.Max(1, Width - e.HorizontalChange);
        //    }
        //    if (top > 0)
        //    {
        //        Height = Math.Max(1, Height - e.VerticalChange);
        //    }

        //    e.Handled = true;
        //}

        //private void MyThumbTop_DragDelta(object sender, DragDeltaEventArgs e)
        //{

        //    double top = Canvas.GetTop(this) + e.VerticalChange;
        //    if (top < 0)
        //    {
        //        Canvas.SetTop(this, 0);
        //    }
        //    else
        //    {
        //        Canvas.SetTop(this, top);
        //        Height = Math.Max(1, Height - e.VerticalChange);
        //    }

        //    e.Handled = true;
        //}

        //private void MyThumbTopRight_DragDelta(object sender, DragDeltaEventArgs e)
        //{
        //    if (sender == MyThumbTopRight)
        //    {
        //        //0サイズとマイナス座標を回避
        //        double ww = Width + e.HorizontalChange;
        //        double left = Canvas.GetLeft(this) + e.HorizontalChange;
        //        double top = Canvas.GetTop(this) + e.VerticalChange;
        //        Canvas.SetTop(this, Math.Max(0, top));
        //        if (top >= 0)
        //        {
        //            Height = Math.Max(1, Height - e.VerticalChange);
        //        }

        //        if(ww < 1)
        //        {
        //            ww = 1;
        //            Canvas.SetLeft(this, Math.Max(0, left));
        //        }
        //        Width = ww;

        //        e.Handled = true;
        //    }
        //}

        //private void MyThumbRight_DragDelta(object sender, DragDeltaEventArgs e)
        //{
        //    if (sender == MyThumbRight)
        //    {
        //        double ww = Width + e.HorizontalChange;
        //        double left = Canvas.GetLeft(this);

        //        //0サイズとマイナス座標を回避
        //        if (ww < 1 && left > 0)
        //        {
        //            Canvas.SetLeft(this, Math.Max(0, ww + left));
        //        }

        //        Width = Math.Max(1, ww);
        //        e.Handled = true;
        //    }
        //}

        private void MyThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double ww = Width + e.HorizontalChange;
            double hh = Height + e.VerticalChange;
            double left = Canvas.GetLeft(this) + e.HorizontalChange;
            double top = Canvas.GetTop(this) + e.VerticalChange;

            if (sender == MyThumbBottom)
            {
                Height = Math.Max(1, hh);
                if (Height == 1 && top >= 0)
                {
                    Canvas.SetTop(this, top);
                }
            }
            else if (sender == MyThumbBottomLeft)
            {
                Height = Math.Max(1, hh);
                if (top >= 0 && Height == 1)
                {
                    Canvas.SetTop(this, top);
                }
                
                Canvas.SetLeft(this, Math.Max(0,left));
                if(left > 0)
                {
                    Width = Math.Max(1, Width - e.HorizontalChange);
                }
            }
            else if (sender == MyThumbLeft)
            {
                Canvas.SetLeft(this, Math.Max(0, left));
                if (left > 0)
                {
                    Width = Math.Max(1, Width - e.HorizontalChange);
                }

            }
            else if (sender == MyThumbTopLeft)
            {
                Canvas.SetLeft(this, Math.Max(0, left));
                Canvas.SetTop(this, Math.Max(0, top));

                if (left > 0)
                {
                    Width = Math.Max(1, Width - e.HorizontalChange);
                }
                if (top > 0)
                {
                    Height = Math.Max(1, Height - e.VerticalChange);
                }
            }
            else if (sender == MyThumbTop)
            {
                if (top < 0)
                {
                    Canvas.SetTop(this, 0);
                }
                else
                {
                    Canvas.SetTop(this, top);
                    Height = Math.Max(1, Height - e.VerticalChange);
                }
            }
            else if (sender == MyThumbTopRight)
            {
                //0サイズとマイナス座標を回避

                Canvas.SetTop(this, Math.Max(0, top));
                if (top >= 0)
                {
                    Height = Math.Max(1, Height - e.VerticalChange);
                }

                if (ww < 1)
                {
                    ww = 1;
                    Canvas.SetLeft(this, Math.Max(0, left));
                }
                Width = ww;
            }
            else if (sender == MyThumbRight)
            {
                if (ww < 1 && left > 0)
                {
                    Canvas.SetLeft(this, Math.Max(0, ww + left));
                }

                Width = Math.Max(1, ww);
            }
            else if (sender == MyThumb)
            {
                Width = Math.Max(1, ww);
                Height = Math.Max(1, hh);

            }

            else if (sender == this)
            {
                Canvas.SetLeft(this, Math.Max(0, Canvas.GetLeft(this) + e.HorizontalChange));
                Canvas.SetTop(this, Math.Max(0, Canvas.GetTop(this) + e.VerticalChange));
            }
            e.Handled = true;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_canvas") is Canvas panel)
            {
                panel.Children.Add(MyThumb);
                MyThumb.DataContext = this;
                MyThumb.SetBinding(Canvas.LeftProperty, new Binding(nameof(Width)) { Mode = BindingMode.TwoWay });
                MyThumb.SetBinding(Canvas.TopProperty, new Binding(nameof(Height)) { Mode = BindingMode.TwoWay });

                panel.Children.Add(MyThumbRight);
                MyThumbRight.SetBinding(Canvas.LeftProperty, new Binding(nameof(Width)) { Source = this, Mode = BindingMode.TwoWay });
                MyThumbRight.SetBinding(Canvas.TopProperty, new Binding(nameof(Height)) { Source = this, Mode = BindingMode.TwoWay, Converter = new MyConverterHalf() });

                panel.Children.Add(MyThumbTopRight);
                MyThumbTopRight.DataContext = this;
                MyThumbTopRight.SetBinding(Canvas.LeftProperty, new Binding(nameof(Width)) { Mode = BindingMode.TwoWay });

                panel.Children.Add(MyThumbTop);
                MyThumbTop.DataContext = this;
                MyThumbTop.SetBinding(Canvas.LeftProperty, new Binding(nameof(Width)) { Mode = BindingMode.TwoWay, Converter = new MyConverterHalf() });

                panel.Children.Add(MyThumbTopLeft);

                panel.Children.Add(MyThumbLeft);
                MyThumbLeft.DataContext = this;
                MyThumbLeft.SetBinding(Canvas.TopProperty, new Binding(nameof(Height)) { Mode = BindingMode.TwoWay, Converter = new MyConverterHalf() });

                panel.Children.Add(MyThumbBottomLeft);
                MyThumbBottomLeft.DataContext = this;
                MyThumbBottomLeft.SetBinding(Canvas.TopProperty, new Binding(nameof(Height)) { Mode = BindingMode.TwoWay });

                panel.Children.Add(MyThumbBottom);
                MyThumbBottom.DataContext = this;
                MyThumbBottom.SetBinding(Canvas.TopProperty, new Binding(nameof(Height)) { Mode = BindingMode.TwoWay });
                MyThumbBottom.SetBinding(Canvas.LeftProperty, new Binding(nameof(Width)) { Mode = BindingMode.TwoWay, Converter = new MyConverterHalf() });

            }
        }

    }

    public class MyConverterHalf : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //double v = (double)value;
            return (double)value / 2.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



    /// <summary>
    /// 基礎Thumb、すべてのCustomControlThumbの派生元
    /// </summary>
    [DebuggerDisplay("{MyType} {MyText}")]
    public abstract class KisoThumb : Thumb
    {
        //クリックダウンとドラッグ移動完了時に使う、直前に選択されたものかの判断用
        bool IsPreviewSelected { get; set; }

        #region 依存関係プロパティ



        #region 共通

        public Visibility IsWakuVisible
        {
            get { return (Visibility)GetValue(IsWakuVisibleProperty); }
            set { SetValue(IsWakuVisibleProperty, value); }
        }
        public static readonly DependencyProperty IsWakuVisibleProperty =
            DependencyProperty.Register(nameof(IsWakuVisible), typeof(Visibility), typeof(KisoThumb), new PropertyMetadata(Visibility.Visible));

        public List<Brush> MyBrushList
        {
            get { return (List<Brush>)GetValue(MyBrushListProperty); }
            set { SetValue(MyBrushListProperty, value); }
        }
        public static readonly DependencyProperty MyBrushListProperty =
            DependencyProperty.Register(nameof(MyBrushList), typeof(List<Brush>), typeof(KisoThumb), new PropertyMetadata(null));


        public double MyLeft
        {
            get { return (double)GetValue(MyLeftProperty); }
            set { SetValue(MyLeftProperty, value); }
        }
        public static readonly DependencyProperty MyLeftProperty =
            DependencyProperty.Register(nameof(MyLeft), typeof(double), typeof(KisoThumb), new PropertyMetadata(0.0));

        public double MyTop
        {
            get { return (double)GetValue(MyTopProperty); }
            set { SetValue(MyTopProperty, value); }
        }
        public static readonly DependencyProperty MyTopProperty =
            DependencyProperty.Register(nameof(MyTop), typeof(double), typeof(KisoThumb), new PropertyMetadata(0.0));


        public int MyZIndex
        {
            get { return (int)GetValue(MyZIndexProperty); }
            set { SetValue(MyZIndexProperty, value); }
        }
        public static readonly DependencyProperty MyZIndexProperty =
            DependencyProperty.Register(nameof(MyZIndex), typeof(int), typeof(KisoThumb), new PropertyMetadata(0));


        public double MyWidth
        {
            get { return (double)GetValue(MyWidthProperty); }
            set { SetValue(MyWidthProperty, value); }
        }
        public static readonly DependencyProperty MyWidthProperty =
            DependencyProperty.Register(nameof(MyWidth), typeof(double), typeof(KisoThumb), new PropertyMetadata(0.0));


        public double MyHeight
        {
            get { return (double)GetValue(MyHeightProperty); }
            set { SetValue(MyHeightProperty, value); }
        }
        public static readonly DependencyProperty MyHeightProperty =
            DependencyProperty.Register(nameof(MyHeight), typeof(double), typeof(KisoThumb), new PropertyMetadata(0.0));


        public string MyText
        {
            get { return (string)GetValue(MyTextProperty); }
            set { SetValue(MyTextProperty, value); }
        }
        public static readonly DependencyProperty MyTextProperty =
            DependencyProperty.Register(nameof(MyText), typeof(string), typeof(KisoThumb), new PropertyMetadata(string.Empty));


        public Brush MyBackground
        {
            get { return (Brush)GetValue(MyBackgroundProperty); }
            set { SetValue(MyBackgroundProperty, value); }
        }
        public static readonly DependencyProperty MyBackgroundProperty =
            DependencyProperty.Register(nameof(MyBackground), typeof(Brush), typeof(KisoThumb), new PropertyMetadata(Brushes.Transparent));


        public Brush MyFill
        {
            get { return (Brush)GetValue(MyFillProperty); }
            set { SetValue(MyFillProperty, value); }
        }
        public static readonly DependencyProperty MyFillProperty =
            DependencyProperty.Register(nameof(MyFill), typeof(Brush), typeof(KisoThumb), new PropertyMetadata(Brushes.Transparent));

        #endregion 共通

        #endregion 依存関係プロパティ

        public ThumbType MyType { get; internal set; }

        //親要素の識別用。自身がグループ化されたときに親要素のGroupThumbを入れておく
        public GroupThumb? MyParentThumb { get; internal set; }
        static KisoThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KisoThumb), new FrameworkPropertyMetadata(typeof(KisoThumb)));
        }
        public KisoThumb()
        {
            DataContext = this;
            Focusable = true;
            //Focusable = false;
            MyType = ThumbType.None;
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
    }


    public class TextBlockThumb : KisoThumb
    {
        static TextBlockThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBlockThumb), new FrameworkPropertyMetadata(typeof(TextBlockThumb)));
        }
        public TextBlockThumb()
        {
            MyType = ThumbType.Text;
        }
    }

    public class ShapeKisoThumb : KisoThumb
    {
        private Canvas? MyPanel { get; set; }
        private KnobThumb MyKnob { get; set; }
        static ShapeKisoThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ShapeKisoThumb), new FrameworkPropertyMetadata(typeof(ShapeKisoThumb)));
        }
        public ShapeKisoThumb()
        {
            MyKnob = new() { Width = 20, Height = 20 };
            MyKnob.DragDelta += MyKnob_DragDelta;
        }

        private void MyKnob_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is KnobThumb && MyPanel != null)
            {

                Width = Math.Max(1, Width + e.HorizontalChange);
                Height = Math.Max(1, Height + e.VerticalChange);
                e.Handled = true;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_Panel") is Canvas panel)
            {
                MyPanel = panel;
                MyPanel.Children.Add(MyKnob);
                MyKnob.SetBinding(Canvas.LeftProperty, new Binding() { Source = this, Path = new PropertyPath(WidthProperty), Mode = BindingMode.TwoWay });
                MyKnob.SetBinding(Canvas.TopProperty, new Binding() { Source = this, Path = new PropertyPath(HeightProperty), Mode = BindingMode.TwoWay });

            }
        }
    }

    public class EllipseTextThumb : TextBlockThumb
    {
        static EllipseTextThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EllipseTextThumb), new FrameworkPropertyMetadata(typeof(EllipseTextThumb)));
        }
        public EllipseTextThumb()
        {
            MyType = ThumbType.Ellipse;
        }
    }


    [ContentProperty(nameof(MyThumbs))]
    public class GroupThumb : KisoThumb
    {
        internal ExCanvas? MyCanvas { get; set; }
        internal ExCanvas? MyCanvas2 { get; set; }
        private Thumb MyKnob { get; set; }
        private KisoThumb? MyTargetElement { get; set; }

        #region 依存関係プロパティ

        public ObservableCollection<KisoThumb> MyThumbs
        {
            get { return (ObservableCollection<KisoThumb>)GetValue(MyThumbsProperty); }
            set { SetValue(MyThumbsProperty, value); }
        }
        public static readonly DependencyProperty MyThumbsProperty =
            DependencyProperty.Register(nameof(MyThumbs), typeof(ObservableCollection<KisoThumb>), typeof(GroupThumb), new PropertyMetadata(null));


        public ObservableCollection<FrameworkElement> MyElemants
        {
            get { return (ObservableCollection<FrameworkElement>)GetValue(MyObjectsProperty); }
            set { SetValue(MyObjectsProperty, value); }
        }
        public static readonly DependencyProperty MyObjectsProperty =
            DependencyProperty.Register(nameof(MyElemants), typeof(ObservableCollection<FrameworkElement>), typeof(GroupThumb), new PropertyMetadata(null));

        #endregion 依存関係プロパティ

        #region コンストラクタ

        static GroupThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GroupThumb), new FrameworkPropertyMetadata(typeof(GroupThumb)));
        }
        public GroupThumb()
        {
            MyType = ThumbType.Group;
            MyThumbs = [];
            MyElemants = [];

            Loaded += GroupThumb_Loaded;
            //MyThumbs.CollectionChanged += MyThumbs_CollectionChanged;
            MyKnob = new Thumb() { Width = 20, Height = 20 };
            Canvas.SetLeft(MyKnob, 0); Canvas.SetTop(MyKnob, 0);
            MyElemants.Add(MyKnob);
            MyKnob.DragDelta += MyKnob_DragDelta;
        }

        #endregion コンストラクタ

        #region 初期化

        /// <summary>
        /// 起動直後にBindingの設定
        /// Templateの中にあるExCanvasを取得して、自身の縦横サイズのBinding
        /// </summary>
        private void GroupThumb_Loaded(object sender, RoutedEventArgs e)
        {
            var temp = GetTemplateChild("PART_ItemsControl");
            if (temp is ItemsControl ic)
            {
                MyCanvas = GetExCanvas(ic);
                if (MyCanvas != null)
                {
                    _ = SetBinding(WidthProperty, new Binding() { Source = MyCanvas, Path = new PropertyPath(ActualWidthProperty) });
                    _ = SetBinding(HeightProperty, new Binding() { Source = MyCanvas, Path = new PropertyPath(ActualHeightProperty) });
                }
            }

            temp = GetTemplateChild("PART_ItemsControl2");
            if (temp is ItemsControl ic2)
            {
                MyCanvas2 = GetExCanvas(ic2);
                if (MyCanvas2 != null)
                {
                    MyCanvas2.Visibility = Visibility.Collapsed;
                    _ = MyCanvas2.SetBinding(WidthProperty, new Binding() { Source = MyCanvas, Path = new PropertyPath(ActualWidthProperty) });
                    _ = MyCanvas2.SetBinding(HeightProperty, new Binding() { Source = MyCanvas, Path = new PropertyPath(ActualHeightProperty) });
                }
            }

        }

        /// <summary>
        /// Templateの中にあるExCanvasの取得
        /// </summary>
        private static ExCanvas? GetExCanvas(DependencyObject d)
        {
            if (d is ExCanvas canvas) { return canvas; }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(d); i++)
            {
                ExCanvas? c = GetExCanvas(VisualTreeHelper.GetChild(d, i));
                if (c is not null) return c;
            }
            return null;
        }
        #endregion 初期化

        #region イベントハンドラ
        private void MyKnob_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t && MyTargetElement != null)
            {
                MyTargetElement.MyWidth += e.HorizontalChange;
                MyTargetElement.MyHeight += e.VerticalChange;
                e.Handled = true;
            }
        }
        public void SetTargetElement(KisoThumb element)
        {
            if (MyCanvas2 != null)
            {
                MyCanvas2.Visibility = Visibility.Visible;
            }
            MyTargetElement = element;
            MyKnob.DataContext = MyTargetElement;
            MyKnob.SetBinding(Canvas.LeftProperty, new Binding(nameof(MyWidth)) { Mode = BindingMode.TwoWay });
            MyKnob.SetBinding(Canvas.TopProperty, new Binding(nameof(MyHeight)) { Mode = BindingMode.TwoWay });

        }


        #endregion イベントハンドラ

        #region publicメソッド



        /// <summary>
        /// 再配置、ReLayoutからの改変、余計な処理をなくした。
        /// 子要素全体での左上座標を元に子要素全部と自身の位置を修正する
        /// ただし、自身がrootだった場合は子要素だけを修正する
        /// </summary>
        public void ReLayout3()
        {
            //全体での左上座標を取得
            double left = double.MaxValue; double top = double.MaxValue;
            foreach (var item in MyThumbs)
            {
                if (item.MyType != ThumbType.Anchor)
                {
                    if (left > item.MyLeft) { left = item.MyLeft; }
                    if (top > item.MyTop) { top = item.MyTop; }
                }
            }

            if (left != MyLeft)
            {
                //座標変化の場合は、自身と全ての子要素の座標を変更する
                foreach (var item in MyThumbs) { item.MyLeft -= left; }

                //自身がroot以外なら修正
                if (MyType != ThumbType.Root) { MyLeft += left; }
            }

            if (top != MyTop)
            {
                foreach (var item in MyThumbs) { item.MyTop -= top; }

                if (MyType != ThumbType.Root) { MyTop += top; }
            }

            //ParentThumbがあれば、そこでも再配置処理
            MyParentThumb?.ReLayout3();
        }

        #endregion publicメソッド

    }

}
