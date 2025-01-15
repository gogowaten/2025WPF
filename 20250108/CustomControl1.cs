using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250108
{
    //Thumbの種類の識別用
    public enum Type { None = 0, Root, Group, Item, Anchor, Range, Text, Ellipse }


    /// <summary>
    /// 基礎Thumb、すべてのCustomControlThumbの派生元
    /// </summary>
    [DebuggerDisplay("{MyType}_{MyText}")]
    public abstract class KisoThumb : Thumb
    {
        #region 依存関係プロパティ

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

        public string MyText
        {
            get { return (string)GetValue(MyTextProperty); }
            set { SetValue(MyTextProperty, value); }
        }
        public static readonly DependencyProperty MyTextProperty =
            DependencyProperty.Register(nameof(MyText), typeof(string), typeof(KisoThumb), new PropertyMetadata(string.Empty));

        #endregion 依存関係プロパティ

        public Type MyType { get; internal set; }

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
            MyType = Type.None;
            PreviewMouseDown += KisoThumb_PreviewMouseDown;
            PreviewMouseUp += KisoThumb_PreviewMouseUp;
            DragStarted += KisoThumb_DragStarted;
            DragDelta += KisoThumb_DragDelta;
            DragCompleted += KisoThumb_DragCompleted;
        }
        private void KisoThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            if (e.Source is KisoThumb t)
            {
                //アンカーThumbをHidden、在るけど見えないだけ
                if (t.MyParentThumb is GroupThumb gt)
                {
                    AnchorThumb anchor = gt.MyAnchorThumb;
                    anchor.Visibility = Visibility.Hidden;
                    anchor.Width = t.ActualWidth;
                    anchor.Height = t.ActualHeight;
                    anchor.MyLeft = t.MyLeft;
                    anchor.MyTop = t.MyTop;

                }
            }
        }

        private void KisoThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {

            if (sender is KisoThumb t)
            {
                t.MyLeft += e.HorizontalChange;
                t.MyTop += e.VerticalChange;
                e.Handled = true;
            }

        }

        private void KisoThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (sender is KisoThumb t && t.MyParentThumb is not null)
            {
                if (t.MyParentThumb is GroupThumb gt)
                {
                    AnchorThumb anchor = gt.MyAnchorThumb;
                    anchor.Visibility = Visibility.Collapsed;
                    anchor.MyLeft = t.MyLeft;
                    anchor.MyTop = t.MyTop;
                }
                t.MyParentThumb.ReLayout3();
            }
        }



        /// <summary>
        /// クリックダウン時、フォーカス無効化する。
        /// フォーカスでスクロール位置がガクッと変更されて不自然なのを防ぐ
        /// </summary>
        private void KisoThumb_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is KisoThumb t)
            {
                t.Focusable = false;
            }
        }

        /// <summary>
        /// マウスアップ時、フォーカスを有効化してフォーカスする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KisoThumb_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is KisoThumb t)
            {
                t.Focusable = true;
                t.Focus();
            }
        }

    }

    public class RangeThumb : GroupThumb
    {
        KnobThumb KULeft = new();
        KnobThumb KURight = new();
        KnobThumb KDLeft = new();
        KnobThumb KDRight = new();
        static RangeThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeThumb), new FrameworkPropertyMetadata(typeof(RangeThumb)));
        }
        public RangeThumb()
        {
            MyType = Type.Range;
            Focusable = false;
            //Loaded += RangeThumb_Loaded;
            Panel.SetZIndex(this, int.MaxValue);

            MyThumbs2.Add(KULeft);
            MyThumbs2.Add(KURight);
            MyThumbs2.Add(KDLeft);
            MyThumbs2.Add(KDRight);
            //KULeft.MyLeft = 0; KULeft.MyTop = 0;
            //KDRight.MyLeft = 100; KDRight.MyTop = 100;
            Canvas.SetLeft(KDRight, 100); Canvas.SetTop(KDRight, 100);

            //KULeft.DragDelta += Thumb_DragDelta;
            //KURight.DragDelta += Thumb_DragDelta;
            //KDLeft.DragDelta += Thumb_DragDelta;
            //KDRight.DragDelta += Thumb_DragDelta;
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                Canvas.SetLeft(t, Canvas.GetLeft(t) + e.HorizontalChange);
                Canvas.SetTop(t, Canvas.GetTop(t) + e.VerticalChange);
                e.Handled = true;
            }
        }

        private void RangeThumb_Loaded(object sender, RoutedEventArgs e)
        {
            //DependencyObject d = GetTemplateChild("MyCanvas");
            //if (GetExCanvas(d) is ExCanvas canvas)
            //{
            //    Canvas.SetLeft(rightBottom, 150); Canvas.SetTop(rightBottom, 100);
            //    canvas.Children.Add(rightBottom);

            //    _ = SetBinding(WidthProperty, new Binding() { Source = canvas, Path = new PropertyPath(ActualWidthProperty) });
            //    _ = SetBinding(HeightProperty, new Binding() { Source = canvas, Path = new PropertyPath(ActualHeightProperty) });
            //}
        }

        ///// <summary>
        ///// Templateの中にあるExCanvasの取得
        ///// </summary>
        //private static ExCanvas? GetExCanvas(DependencyObject d)
        //{
        //    if (d is ExCanvas canvas) { return canvas; }

        //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(d); i++)
        //    {
        //        ExCanvas? c = GetExCanvas(VisualTreeHelper.GetChild(d, i));
        //        if (c is not null) return c;
        //    }
        //    return null;
        //}

    }

    public class RangeThumb2 : ExCanvas
    {
        KnobThumb KULeft = new();
        KnobThumb KURight = new();
        KnobThumb KDLeft = new();
        KnobThumb KDRight = new();

        static RangeThumb2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeThumb2), new FrameworkPropertyMetadata(typeof(RangeThumb2)));
        }
        public RangeThumb2()
        {
            Children.Add(KULeft);
            Children.Add(KURight);
            Children.Add(KDLeft);
            Children.Add(KDRight);
            SetLeft(KULeft, 0); SetTop(KULeft, 0);
            SetLeft(KDRight, 100); SetTop(KDRight, 100);
            KDRight.DragDelta += Knob_DragDelta;
        }

        private void Knob_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                if (t == KDRight)
                {
                    Canvas.SetLeft(t, Canvas.GetLeft(t) + e.HorizontalChange);
                    Canvas.SetTop(t, Canvas.GetTop(t) + e.VerticalChange);

                    //this.Width = this.ActualWidth+ e.HorizontalChange;
                    //this.Height+= e.VerticalChange;
                }
            }
        }
    }

    public class RangeThumb3 : Thumb
    {

        KnobThumb KULeft = new();
        //KnobThumb KURight = new();
        //KnobThumb KDLeft = new();
        KnobThumb KDRight = new();
        List<KnobThumb> MyKnobThumbs;
        static RangeThumb3()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeThumb3), new FrameworkPropertyMetadata(typeof(RangeThumb3)));
        }
        public RangeThumb3()
        {
            MyKnobThumbs = [];
            MyKnobThumbs.Add(KULeft);
            MyKnobThumbs.Add(KDRight);
            Canvas.SetLeft(this, 0); Canvas.SetTop(this, 0);
            Canvas.SetLeft(KULeft, 0); Canvas.SetTop(KULeft, 0);
            Canvas.SetLeft(KDRight, 100); Canvas.SetTop(KDRight, 150);
            Loaded += RangeThumb3_Loaded;
            KDRight.DragDelta += Knob_DragDelta;
            KULeft.DragDelta += Knob_DragDelta;
            KDRight.DragCompleted += Knob_DragCompleted;
            KULeft.DragCompleted += Knob_DragCompleted;
        }

        private void Knob_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            ReLayout3();
        }

        private void RangeThumb3_Loaded(object sender, RoutedEventArgs e)
        {
            if (GetTemplateChild("MyCanvas") is Canvas panel)
            {
                panel.Background = new SolidColorBrush(Color.FromArgb(64, 0, 0, 0));

                //SetBinding(WidthProperty, new Binding() { Source = KDRight, Path = new PropertyPath(Canvas.LeftProperty) });
                //SetBinding(HeightProperty, new Binding() { Source = KDRight, Path = new PropertyPath(Canvas.TopProperty) });


                panel.Children.Add(KULeft);
                panel.Children.Add(KDRight);

            }
        }

        private void Knob_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                double Hori = Canvas.GetLeft(t) + e.HorizontalChange;
                double Vert = Canvas.GetTop(t) + e.VerticalChange;
                if (t == KDRight)
                {
                    if (Hori > 1)
                    {
                        Canvas.SetLeft(t, Hori);
                    }
                    if (Vert > 1)
                    {
                        Canvas.SetTop(t, Vert);
                    }
                }
                else if (t == KULeft)
                {
                    //Canvas.SetLeft(t, Canvas.GetLeft(t) + e.HorizontalChange);
                    //Canvas.SetTop(t, Canvas.GetTop(t) + e.VerticalChange);

                    Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
                    Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
                    this.Width -= e.HorizontalChange;
                    this.Height -= e.VerticalChange;
                }
            }
        }


        /// <summary>
        /// 再配置、ReLayoutからの改変、余計な処理をなくした。
        /// 子要素全体での左上座標を元に子要素全部と自身の位置を修正する
        /// ただし、自身がrootだった場合は子要素だけを修正する
        /// </summary>
        public void ReLayout3()
        {
            //全体での左上座標を取得
            double left = double.MaxValue; double top = double.MaxValue;
            foreach (var item in MyKnobThumbs)
            {
                left = Math.Min(left, Canvas.GetLeft(item));
                top = Math.Min(top, Canvas.GetTop(item));
            }

            double MyLeft = Canvas.GetLeft(this);
            double MyTop = Canvas.GetTop(this);
            if (left != MyLeft)
            {
                //座標変化の場合は、自身と全ての子要素の座標を変更する
                foreach (var item in MyKnobThumbs)
                {
                    Canvas.SetLeft(item, Canvas.GetLeft(item) - left);
                }

                //自身修正
                Canvas.SetLeft(this, Canvas.GetLeft(this) + left);
            }

            if (top != MyTop)
            {
                foreach (var item in MyKnobThumbs)
                {
                    Canvas.SetTop(item, Canvas.GetTop(item) - top);
                }
                //foreach (var item in MyKnobThumbs) { item.MyTop -= top; }

                Canvas.SetTop(this, Canvas.GetTop(this) + top);
                //if (MyType != Type.Root) { MyTop += top; }
            }

        }



    }



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

    /// <summary>
    /// 子要素の移動時にスクロール一時固定に使うアンカーThumb
    /// </summary>
    public class AnchorThumb : KisoThumb
    {
        static AnchorThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnchorThumb), new FrameworkPropertyMetadata(typeof(AnchorThumb)));
        }
        public AnchorThumb()
        {
            MyType = Type.Anchor;
            Focusable = false;
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
            MyType = Type.Text;
        }
    }

    public class EllipseTextThumb : TextBlockThumb
    {
        #region 依存関係プロパティ

        public double MySize
        {
            get { return (double)GetValue(MySizeProperty); }
            set { SetValue(MySizeProperty, value); }
        }
        public static readonly DependencyProperty MySizeProperty =
            DependencyProperty.Register(nameof(MySize), typeof(double), typeof(EllipseTextThumb), new PropertyMetadata(30.0));
        #endregion 依存関係プロパティ

        static EllipseTextThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EllipseTextThumb), new FrameworkPropertyMetadata(typeof(EllipseTextThumb)));
        }
        public EllipseTextThumb()
        {
            MyType = Type.Ellipse;
        }
    }

    public class RangeRectThumb : Thumb
    {
        static RangeRectThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeRectThumb), new FrameworkPropertyMetadata(typeof(RangeRectThumb)));
        }
        public RangeRectThumb()
        {
            DragDelta += RangeRectThumb_DragDelta;
            Background = new SolidColorBrush(Color.FromArgb(100, 0, 0, 255));
        }

        private void RangeRectThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double left = Canvas.GetLeft(this) + e.HorizontalChange;
            double top = Canvas.GetTop(this) + e.VerticalChange;
            left = left < 0 ? 0 : left;
            top = top < 0 ? 0 : top;
            Canvas.SetLeft(this, left);
            Canvas.SetTop(this, top);
        }
    }

    [ContentProperty(nameof(MyThumbs))]
    public class GroupThumb : KisoThumb
    {
        #region 依存関係プロパティ

        public ObservableCollection<KisoThumb> MyThumbs
        {
            get { return (ObservableCollection<KisoThumb>)GetValue(MyThumbsProperty); }
            set { SetValue(MyThumbsProperty, value); }
        }
        public static readonly DependencyProperty MyThumbsProperty =
            DependencyProperty.Register(nameof(MyThumbs), typeof(ObservableCollection<KisoThumb>), typeof(GroupThumb), new PropertyMetadata(null));


        public ObservableCollection<Thumb> MyThumbs2
        {
            get { return (ObservableCollection<Thumb>)GetValue(MyThumbs2Property); }
            set { SetValue(MyThumbs2Property, value); }
        }
        public static readonly DependencyProperty MyThumbs2Property =
            DependencyProperty.Register(nameof(MyThumbs2), typeof(ObservableCollection<Thumb>), typeof(GroupThumb), new PropertyMetadata(null));


        #endregion 依存関係プロパティ

        //子要素移動時にスクロールバー固定用のアンカー
        public AnchorThumb MyAnchorThumb { get; private set; }

        #region コンストラクタ

        static GroupThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GroupThumb), new FrameworkPropertyMetadata(typeof(GroupThumb)));
        }
        public GroupThumb()
        {
            MyType = Type.Group;
            MyThumbs = [];
            MyThumbs2 = [];
            MyAnchorThumb = new AnchorThumb() { Visibility = Visibility.Collapsed };
            MyThumbs.Add(MyAnchorThumb);//ダミー設置
            Loaded += GroupThumb_Loaded;
            MyThumbs.CollectionChanged += MyThumbs_CollectionChanged;
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
                var canvas = GetExCanvas(ic);
                if (canvas != null)
                {
                    _ = SetBinding(WidthProperty, new Binding() { Source = canvas, Path = new PropertyPath(ActualWidthProperty) });
                    _ = SetBinding(HeightProperty, new Binding() { Source = canvas, Path = new PropertyPath(ActualHeightProperty) });
                }
            }
        }

        /// <summary>
        /// Templateの中にあるExCanvasの取得
        /// </summary>
        internal static ExCanvas? GetExCanvas(DependencyObject d)
        {
            if (d is ExCanvas canvas) { return canvas; }
            if (d == null) { return null; }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(d); i++)
            {
                ExCanvas? c = GetExCanvas(VisualTreeHelper.GetChild(d, i));
                if (c is not null) return c;
            }
            return null;
        }

        /// <summary>
        /// 子要素の追加時
        /// 子要素に親要素(自身)を登録
        /// </summary>
        private void MyThumbs_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems?[0] is KisoThumb nnt)
            {
                nnt.MyParentThumb = this;
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems?[0] is KisoThumb ot)
            {
                ot.MyParentThumb = null;
            }
        }
        #endregion 初期化

        /// <summary>
        /// 再配置、
        /// 子要素全体での左上座標を元に
        /// 子要素全部と自身の位置を修正する
        /// 自身がrootだった場合は変更があっても0,0に修正
        /// </summary>
        //public void ReLayout()
        //{
        //    //全体での左上座標を取得
        //    double left = double.MaxValue; double top = double.MaxValue;
        //    foreach (var item in MyThumbs)
        //    {
        //        if (item.MyType != Type.Anchor)
        //        {
        //            if (left > item.MyLeft) { left = item.MyLeft; }
        //            if (top > item.MyTop) { top = item.MyTop; }
        //        }
        //    }

        //    //自身の座標と比較、同じ(変化なし)なら終了
        //    if (left == MyLeft && top == MyTop) return;

        //    //座標変化の場合は、自身と全ての子要素の座標を変更する
        //    if (left != 0)
        //    {
        //        foreach (var item in MyThumbs) { item.MyLeft -= left; }
        //        //自身がrootだった場合は座標を0に、それ以外なら変更
        //        if (MyType == Type.Root) { MyLeft = 0; }
        //        else MyLeft += left;
        //    }

        //    if (top != 0)
        //    {
        //        foreach (var item in MyThumbs) { item.MyTop -= top; }
        //        if (MyType == Type.Root) { MyTop = 0; }
        //        else MyTop += top;
        //    }

        //    //ParentThumbにも伝播させる
        //    MyParentThumb?.ReLayout();
        //}


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
                if (item.MyType != Type.Anchor)
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
                if (MyType != Type.Root) { MyLeft += left; }
            }

            if (top != MyTop)
            {
                foreach (var item in MyThumbs) { item.MyTop -= top; }

                if (MyType != Type.Root) { MyTop += top; }
            }

            //ParentThumbがあれば、そこでも再配置処理
            MyParentThumb?.ReLayout3();
        }



    }

    /// <summary>
    /// root用Thumb
    /// rootは移動させない、というか移動させないときの識別用クラス
    /// </summary>
    public class RootThumb : GroupThumb
    {
        //public RangeThumb2 MyRange { get; private set; }
        //KnobThumb KULeft = new();
        //KnobThumb KURight = new();
        //KnobThumb KDLeft = new();
        private KnobThumb MyThumb0 = new();
        private KnobThumb MyThumb1 = new();
        private KnobThumb MyThumb2 = new();
        private KnobThumb MyThumb3 = new();
        private KnobThumb MyThumb4 = new();
        private KnobThumb MyThumb5 = new();
        private KnobThumb MyThumb6 = new();
        private KnobThumb MyThumb7 = new();
        private RangeRectThumb MyRect = new() { Width = 100, Height = 100 };
        //RangeRect MyRange = new() { Background = Brushes.DodgerBlue };
        Canvas? MyCanvas { get; set; }

        RangeThumb MyRange = new();

        static RootThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RootThumb), new FrameworkPropertyMetadata(typeof(RootThumb)));
        }
        public RootThumb()
        {
            MyType = Type.Root;
            Canvas.SetLeft(MyRect, 0); Canvas.SetTop(MyRect, 0);
            MyThumbs2.Add(MyRect);
            MyThumbs2.Add(MyThumb0);
            MyThumbs2.Add(MyThumb1);
            MyThumbs2.Add(MyThumb2);
            MyThumbs2.Add(MyThumb3);
            MyThumbs2.Add(MyThumb4);
            MyThumbs2.Add(MyThumb5);
            MyThumbs2.Add(MyThumb6);
            MyThumbs2.Add(MyThumb7);

            Loaded += RootThumb_Loaded;
            MyThumb0.DragDelta += MyThumb_DragDelta;
            MyThumb2.DragDelta += MyThumb_DragDelta;
            MyThumb5.DragDelta += MyThumb_DragDelta;
            MyThumb7.DragDelta += MyThumb_DragDelta;
            MyThumb1.DragDelta += MyThumb_DragDeltaOnlyVertical;
            MyThumb6.DragDelta += MyThumb_DragDeltaOnlyVertical;
            MyThumb3.DragDelta += MyThumb_DragDeltaOnlyHorizontal;
            MyThumb4.DragDelta += MyThumb_DragDeltaOnlyHorizontal;

            Test1(MyRect);

        }


        private void RootThumb_Loaded(object sender, RoutedEventArgs e)
        {
            //DependencyObject d = GetTemplateChild("MyCanvas");
            if (GetCanvas(GetTemplateChild("PART_ItemsControl2")) is Canvas panel)
            {
                MyCanvas = panel;


            }
        }

        internal static Canvas? GetCanvas(DependencyObject d)
        {
            if (d is Canvas canvas) { return canvas; }
            if (d == null) { return null; }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(d); i++)
            {
                Canvas? c = GetCanvas(VisualTreeHelper.GetChild(d, i));
                if (c is not null) return c;
            }
            return null;
        }


        #region ドラッグ移動        
        
        private void MyThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is not Thumb t) { return; }
            Canvas.SetLeft(t, Canvas.GetLeft(t) + e.HorizontalChange);
            Canvas.SetTop(t, Canvas.GetTop(t) + e.VerticalChange);
        }
        private void MyThumb_DragDeltaOnlyVertical(object sender, DragDeltaEventArgs e)
        {
            if (sender is not Thumb t) { return; }
            Canvas.SetTop(t, Canvas.GetTop(t) + e.VerticalChange);
        }
        private void MyThumb_DragDeltaOnlyHorizontal(object sender, DragDeltaEventArgs e)
        {
            if (sender is not Thumb t) { return; }
            Canvas.SetLeft(t, Canvas.GetLeft(t) + e.HorizontalChange);
        }
        
        #endregion ドラッグ移動        


        /// <summary>
        /// TwoWayモードのBinding作成
        /// </summary>
        /// <param name="source">ソース要素</param>
        /// <param name="dp">依存関係プロパティ</param>
        /// <returns></returns>
        private static Binding MakeBinding(FrameworkElement source, DependencyProperty dp)
        {
            Binding b = new()
            {
                Source = source,
                Path = new PropertyPath(dp),
                Mode = BindingMode.TwoWay
            };
            return b;
        }


        /// <summary>
        /// マルチBindingを作成
        /// </summary>
        /// <param name="converter">コンバーター</param>
        /// <param name="param">ConverterParameter</param>
        /// <param name="bindings">Bindingリスト</param>
        /// <returns></returns>
        private static MultiBinding MakeMultiBinding(
            IMultiValueConverter converter, object? param = null, params Binding[] bindings)
        {
            MultiBinding m = new()
            {
                ConverterParameter = param,
                Converter = converter,
                Mode = BindingMode.TwoWay
            };
            foreach (var item in bindings)
            {
                m.Bindings.Add(item);
            }
            return m;
        }

        private void Test1(FrameworkElement element)
        {
            Binding b0 = MakeBinding(element, Canvas.LeftProperty);
            Binding b1 = MakeBinding(element, WidthProperty);
            Binding b2 = MakeBinding(element, Canvas.TopProperty);
            Binding b3 = MakeBinding(element, HeightProperty);

            MultiBinding m0 = MakeMultiBinding(new MMM0(), element, b0, b1);
            MultiBinding m1 = MakeMultiBinding(new MMM1(), element, b2, b3);
            MultiBinding m2 = MakeMultiBinding(new MMM2(), element, b0, b1);
            MultiBinding m3 = MakeMultiBinding(new MMM3(), element, b0, b1);
            MultiBinding m4 = MakeMultiBinding(new MMM2(), element, b2, b3);
            MultiBinding m5 = MakeMultiBinding(new MMM4(), element, b2, b3);

            MyThumb0.SetBinding(Canvas.LeftProperty, m0);
            MyThumb0.SetBinding(Canvas.TopProperty, m1);

            MyThumb1.SetBinding(Canvas.LeftProperty, m2);
            MyThumb1.SetBinding(Canvas.TopProperty, m1);

            MyThumb2.SetBinding(Canvas.LeftProperty, m3);
            MyThumb2.SetBinding(Canvas.TopProperty, m1);

            MyThumb3.SetBinding(Canvas.LeftProperty, m0);
            MyThumb3.SetBinding(Canvas.TopProperty, m4);

            MyThumb4.SetBinding(Canvas.LeftProperty, m3);
            MyThumb4.SetBinding(Canvas.TopProperty, m4);

            MyThumb5.SetBinding(Canvas.LeftProperty, m0);
            MyThumb5.SetBinding(Canvas.TopProperty, m5);

            MyThumb6.SetBinding(Canvas.LeftProperty, m2);
            MyThumb6.SetBinding(Canvas.TopProperty, m5);

            MyThumb7.SetBinding(Canvas.LeftProperty, m3);
            MyThumb7.SetBinding(Canvas.TopProperty, m5);
        }
    }



    #region コンバーター
    public class MMM0 : IMultiValueConverter
    {
        public object Convert(object[] values, System.Type targetType, object parameter, CultureInfo culture)
        {
            return (double)values[0];
        }

        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, CultureInfo culture)
        {
            FrameworkElement element = (FrameworkElement)parameter;
            double total = element.Width + Canvas.GetLeft(element);
            double left = (double)value;
            double width = total - left;

            object[] result = [left, width];
            //サイズが1未満にならないように調整
            if (width < 1.0)
            {
                result[0] = total - 1.0;
                result[1] = 1.0;
            }
            return result;
        }
    }
    public class MMM1 : IMultiValueConverter
    {
        public object Convert(object[] values, System.Type targetType, object parameter, CultureInfo culture)
        {
            return (double)values[0];
        }

        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, CultureInfo culture)
        {
            FrameworkElement element = (FrameworkElement)parameter;
            double total = element.Height + Canvas.GetTop(element);
            double top = (double)value;
            double height = total - top;

            object[] result = [top, height];
            if (height < 1.0)
            {
                result[0] = total - 1.0;
                result[1] = 1.0;
            }
            return result;
        }
    }

    public class MMM2 : IMultiValueConverter
    {
        public object Convert(object[] values, System.Type targetType, object parameter, CultureInfo culture)
        {
            return (double)values[0] + (double)values[1] / 2.0;
        }

        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



    public class MMM3 : IMultiValueConverter
    {
        public object Convert(object[] values, System.Type targetType, object parameter, CultureInfo culture)
        {
            return (double)values[0] + (double)values[1];
        }

        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, CultureInfo culture)
        {
            FrameworkElement element = (FrameworkElement)parameter;
            double left = (double)Canvas.GetLeft(element);
            double width = (double)value - left;

            object[] result = [left, width];
            if (width < 1.0) { result[1] = 1.0; }
            return result;
        }
    }
    public class MMM4 : IMultiValueConverter
    {
        public object Convert(object[] values, System.Type targetType, object parameter, CultureInfo culture)
        {
            return (double)values[0] + (double)values[1];
        }

        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, CultureInfo culture)
        {
            FrameworkElement element = (FrameworkElement)parameter;
            double top = (double)Canvas.GetTop(element);
            double height = (double)value - top;

            object[] result = [top, height];
            if (height < 1.0) { result[1] = 1.0; }
            return result;
        }
    }
    #endregion コンバーター
}
