using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
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

namespace _20250103
{



    //Thumbの種類の識別用
    public enum ThumbType { None = 0, Root, Group, Text, Ellipse, Rect, Anchor }


    /// <summary>
    /// 基礎Thumb、すべてのCustomControlThumbの派生元
    /// </summary>
    [DebuggerDisplay("{MyType} {MyText}")]
    public abstract class KisoThumb : Thumb
    {


        #region 依存関係プロパティ


        public ObservableCollection<KisoThumb> MyThumbs
        {
            get { return (ObservableCollection<KisoThumb>)GetValue(MyThumbsProperty); }
            set { SetValue(MyThumbsProperty, value); }
        }
        public static readonly DependencyProperty MyThumbsProperty =
            DependencyProperty.Register(nameof(MyThumbs), typeof(ObservableCollection<KisoThumb>), typeof(GroupThumb), new PropertyMetadata(null));


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


        public string MyText
        {
            get { return (string)GetValue(MyTextProperty); }
            set { SetValue(MyTextProperty, value); }
        }
        public static readonly DependencyProperty MyTextProperty =
            DependencyProperty.Register(nameof(MyText), typeof(string), typeof(KisoThumb), new PropertyMetadata(string.Empty));



        #endregion 依存関係プロパティ

        #region 読み取り専用依存関係プロパティ


        private static readonly DependencyPropertyKey IsSelectablePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(IsSelectable), typeof(bool), typeof(KisoThumb), new PropertyMetadata(false));
        public static readonly DependencyProperty IsSelectableProperty = IsSelectablePropertyKey.DependencyProperty;
        public bool IsSelectable
        {
            get { return (bool)GetValue(IsSelectablePropertyKey.DependencyProperty); }
            internal set { SetValue(IsSelectablePropertyKey, value); }
        }


        private static readonly DependencyPropertyKey IsSelectedPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(IsSelected), typeof(bool), typeof(KisoThumb), new PropertyMetadata(false));
        public static readonly DependencyProperty IsSelectedProperty = IsSelectedPropertyKey.DependencyProperty;
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedPropertyKey.DependencyProperty); }
            internal set { SetValue(IsSelectedPropertyKey, value); }
        }


        private static readonly DependencyPropertyKey IsFocusPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(IsFocus), typeof(bool), typeof(KisoThumb), new PropertyMetadata(false));
        public static readonly DependencyProperty IsFocusProperty = IsFocusPropertyKey.DependencyProperty;
        public bool IsFocus
        {
            get { return (bool)GetValue(IsFocusPropertyKey.DependencyProperty); }
            internal set { SetValue(IsFocusPropertyKey, value); }
        }




        #endregion 読み取り専用依存関係プロパティ


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
            //Focusable = true;
            Focusable = false;
            MyType = ThumbType.None;
            PreviewMouseDown += KisoThumb_PreviewMouseDownTest;
            PreviewMouseUp += KisoThumb_PreviewMouseUpTest;
            DragStarted += KisoThumb_DragStarted2;
            DragCompleted += KisoThumb_DragCompleted2;
            //DragDelta += Thumb_DragDelta;
            //DragDelta += Thumb_DragDelta2;
            DragDelta += Thumb_DragDelta3;
            KeyDown += KisoThumb_KeyDown;
            KeyUp += KisoThumb_KeyUp;
            PreviewKeyDown += KisoThumb_PreviewKeyDown;
        }

        private void KisoThumb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is RootThumb rt)
            {
                if (e.Key == Key.Left)
                {
                    rt.MyClickedThumb.MyLeft -= 10;
                    e.Handled = true;
                }
                else if (e.Key == Key.Right)
                {
                    rt.MyClickedThumb.MyLeft += 10;
                    e.Handled = true;
                }
                else if (e.Key == Key.Up)
                {
                    rt.MyClickedThumb.MyTop -= 10;
                    e.Handled = true;
                }
                else if (e.Key == Key.Down)
                {
                    rt.MyClickedThumb.MyTop += 10;
                    e.Handled = true;
                }

            }
        }

        #region イベントハンドラ

        /// <summary>
        /// KeyUp時
        /// 再配置処理してからBringIntoViewすることで、
        /// 対象Thumbが表示される位置にスクロールする
        /// </summary>
        internal void KisoThumb_KeyUp(object sender, KeyEventArgs e)
        {
            if (sender is KisoThumb t)
            {
                t.MyParentThumb?.ReLayout3();
                t.BringIntoView();
                e.Handled = true;
            }
        }


        /// <summary>
        /// 方向キーの方向へ10ピクセル移動
        /// </summary>
        internal void KisoThumb_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender is KisoThumb t)
            {
                if (e.Key == Key.Left)
                {
                    t.MyLeft -= 10;
                    e.Handled = true;
                }
                else if (e.Key == Key.Right)
                {
                    t.MyLeft += 10;
                    e.Handled = true;
                }
                else if (e.Key == Key.Up)
                {
                    t.MyTop -= 10;
                    e.Handled = true;
                }
                else if (e.Key == Key.Down)
                {
                    t.MyTop += 10;
                    e.Handled = true;
                }

            }
        }

        /// <summary>
        /// マウスアップ時、フォーカスを有効化してフォーカスする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //internal void KisoThumb_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    var sou = e.Source;
        //    var ori = e.OriginalSource;
        //    if (e.Source == e.OriginalSource)
        //    {
        //        if (sender is KisoThumb kiso)
        //        {
        //            kiso.Focusable = true;
        //            kiso.Focus();
        //        }
        //    }
        //    //if (sender is KisoThumb t)
        //    //{
        //    //    //t.Focusable = true;
        //    //    t.Focus();
        //    //}

        //    //if (e.OriginalSource is KisoThumb t)
        //    //{
        //    //    t.Focusable = true;
        //    //    t.Focus();
        //    //}
        //}

        internal void KisoThumb_PreviewMouseUpTest(object sender, MouseButtonEventArgs e)
        {
            var sou = e.Source;
            var ori = e.OriginalSource;

        }

        /// <summary>
        /// クリックダウン時、フォーカス無効化する。
        /// フォーカスでスクロール位置がガクッと変更されて不自然なのを防ぐ
        /// </summary>
        //internal void KisoThumb_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    //イベント発生順序、Root、Group、その他Item
        //    var sou = e.Source;
        //    var ori = e.OriginalSource;
        //    if (e.Source == e.OriginalSource)
        //    {
        //        if (sender is KisoThumb kiso)
        //        {
        //            kiso.Focusable = false;
        //            //e.Handled = true;//これだとドラッグ移動イベントに到達しない
        //        }
        //    }

        //    //if (sender is KisoThumb t)
        //    //{
        //    //    t.Focusable = false;
        //    //    //e.Handled = true;//これだとドラッグ移動イベントに到達しない
        //    //}
        //}

        internal void KisoThumb_PreviewMouseDownTest(object sender, MouseButtonEventArgs e)
        {
            //イベント発生順序、Root、Group、その他Item
            var sou = e.Source;
            var ori = e.OriginalSource;
            if (e.Source == e.OriginalSource)
            {
                if (GetRootThumb() is RootThumb root)
                {
                    //クリックされたThumbをRootのClickedThumbプロパティに登録
                    root.MyClickedThumb = this;
                    //RootThumbのSelectedThumbsプロパティを更新
                    if (GetSelectableParentThumb(this) is KisoThumb thumb)
                    {
                        root.UpdateSelectedThumbs(thumb);
                    }
                }
            }
        }

        internal RootThumb? GetRootThumb()
        {
            if (this is RootThumb rt)
            {
                return rt;
            }
            else if (MyParentThumb is not null)
            {
                return MyParentThumb.GetRootThumb();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 移動距離を四捨五入(丸めて)整数ドラッグ移動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //internal void Thumb_DragDelta2(object sender, DragDeltaEventArgs e)
        //{
        //    if (sender is KisoThumb t)
        //    {

        //        t.MyLeft += (int)(e.HorizontalChange + 0.5);
        //        t.MyTop += (int)(e.VerticalChange + 0.5);
        //        e.Handled = true;
        //    }
        //}


        internal void Thumb_DragDelta3(object sender, DragDeltaEventArgs e)
        {

            if (sender is KisoThumb t && t.IsSelectable)
            {

                t.MyLeft += (int)(e.HorizontalChange + 0.5);
                t.MyTop += (int)(e.VerticalChange + 0.5);
                e.Handled = true;
            }
        }

        /// <summary>
        /// ドラッグ移動終了時
        /// アンカーThumbを削除と要素再配置後に親要素の再配置
        /// </summary>
        internal void KisoThumb_DragCompleted2(object sender, DragCompletedEventArgs e)
        {
            if (sender is KisoThumb t && t.MyParentThumb is not null)
            {
                if (t.MyParentThumb is GroupThumb gt)
                {
                    gt.RemoveAnchorThumb();
                }
                t.MyParentThumb.ReLayout3();
                //t.Focusable = true;
                //t.Focus();
                e.Handled = true;
            }

            //イベントをここで停止
            //e.Handled = true;
        }


        /// <summary>
        /// ドラッグ移動開始時
        /// アンカーThumbを作成追加、
        /// </summary>
        internal void KisoThumb_DragStarted2(object sender, DragStartedEventArgs e)
        {
            if (e.Source is KisoThumb t)
            {

                if (t.MyParentThumb is GroupThumb gt)
                {
                    //アンカーThumbを作成追加
                    gt.AddAnchorThumb(t.MyLeft, t.MyTop, t.ActualWidth, t.ActualHeight);

                    //座標を四捨五入で整数にしてぼやけ回避
                    t.MyLeft = (int)(t.MyLeft + 0.5);
                    t.MyTop = (int)(t.MyTop + 0.5);
                    //t.Focusable = false;
                    e.Handled = true;
                }
            }
        }

        #endregion イベントハンドラ


        #region internalメソッド

        internal KisoThumb? GetSelectableParentThumb(KisoThumb thumb)
        {
            if (thumb.IsSelectable) { return thumb; }
            if (thumb.MyParentThumb is GroupThumb gt)
            {
                if (gt.IsSelectable)
                {
                    return gt;
                }
                else
                {
                    return GetSelectableParentThumb(gt);
                }
            }
            return null;
        }

        ///// <summary>
        ///// クリックされたThumbを親要素に登録
        ///// </summary>
        ///// <param name="kisoThumb"></param>
        //internal void RegistClickedThumb(KisoThumb kisoThumb)
        //{
        //    if (this.MyParentThumb is RootThumb rt)
        //    {
        //        rt.MyClickedThumb = kisoThumb;
        //    }
        //    else if (this.MyParentThumb is GroupThumb gt)
        //    {
        //        gt.RegistClickedThumb(kisoThumb);
        //    }
        //}

        #endregion internalメソッド

        #region メソッド
        private void FixZIndex(int start, int end)
        {
            if (MyParentThumb is GroupThumb gt)
            {
                for (int i = start; i <= end; i++)
                {
                    gt.MyThumbs[i].MyZIndex = i;
                }
            }
        }

        public void ZIndexUp()
        {
            if (MyParentThumb is GroupThumb gt)
            {
                int moto = gt.MyThumbs.IndexOf(this);
                int limit = gt.MyThumbs.Count - 1;
                if (moto >= limit) { return; }
                int saki = moto + 1;
                gt.MyThumbs.Move(moto, saki);
                FixZIndex(moto, saki);
            }
        }

        public void ZIndexTop()
        {
            if (MyParentThumb is GroupThumb gt)
            {
                int moto = gt.MyThumbs.IndexOf(this);
                int limit = gt.MyThumbs.Count - 1;
                if (moto >= limit) { return; }
                gt.MyThumbs.Move(moto, limit);
                FixZIndex(moto, limit);
            }
        }

        public void ZIndexDown()
        {
            if (MyParentThumb is GroupThumb gt)
            {
                int moto = gt.MyThumbs.IndexOf(this);
                if (moto == 0) { return; }
                int saki = moto - 1;
                gt.MyThumbs.Move(moto, saki);
                FixZIndex(saki, moto);
            }
        }

        public void ZIndexBottom()
        {
            if (MyParentThumb is GroupThumb gt)
            {
                int moto = gt.MyThumbs.IndexOf(this);
                if (moto == 0) { return; }
                int saki = 0;
                gt.MyThumbs.Move(moto, 0);
                FixZIndex(saki, moto);
            }
        }

        #endregion メソッド

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
            MyType = ThumbType.Anchor;
            Focusable = false;
            //DragDelta -= Thumb_DragDelta2;
            DragDelta -= Thumb_DragDelta3;
            DragStarted -= KisoThumb_DragStarted2;
            DragCompleted -= KisoThumb_DragCompleted2;
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
            MyType = ThumbType.Ellipse;
        }
    }


    [ContentProperty(nameof(MyThumbs))]
    public class GroupThumb : KisoThumb
    {
        #region 依存関係プロパティ

        //public ObservableCollection<KisoThumb> MyThumbs
        //{
        //    get { return (ObservableCollection<KisoThumb>)GetValue(MyThumbsProperty); }
        //    set { SetValue(MyThumbsProperty, value); }
        //}
        //public static readonly DependencyProperty MyThumbsProperty =
        //    DependencyProperty.Register(nameof(MyThumbs), typeof(ObservableCollection<KisoThumb>), typeof(GroupThumb), new PropertyMetadata(null));

        #endregion 依存関係プロパティ

        //子要素移動時にスクロールバー固定用のアンカー
        public AnchorThumb? MyAnchorThumb { get; private set; }

        #region コンストラクタ

        static GroupThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GroupThumb), new FrameworkPropertyMetadata(typeof(GroupThumb)));
        }
        public GroupThumb()
        {
            MyType = ThumbType.Group;
            MyThumbs = [];
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

        /// <summary>
        /// 子要素の追加時
        /// 子要素に親要素(自身)を登録
        /// </summary>
        private void MyThumbs_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems?[0] is KisoThumb ni)
            {
                ni.MyParentThumb = this;
                ni.MyZIndex = MyThumbs.Count - 1;
                //ni.MyZIndex = MyThumbs.IndexOf(ni);//こっちでも同じ
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems?[0] is KisoThumb ot)
            {
                ot.MyParentThumb = null;
            }
            else if (e.Action == NotifyCollectionChangedAction.Move)
            {
            }
        }
        #endregion 初期化

        #region publicメソッド

        public void AddThumb(KisoThumb thumb)
        {
            MyThumbs.Add(thumb);
        }


        /// <summary>
        /// アンカーThumbをHiddenで追加
        /// </summary>
        public void AddAnchorThumb(double left, double top, double width, double height)
        {
            MyAnchorThumb = new AnchorThumb
            {
                Visibility = Visibility.Hidden,
                Width = width,
                Height = height,
                MyLeft = left,
                MyTop = top
            };
            MyThumbs.Add(MyAnchorThumb);
        }

        /// <summary>
        /// アンカーThumbを削除
        /// </summary>
        public void RemoveAnchorThumb()
        {
            if (MyAnchorThumb is not null)
            {
                MyThumbs.Remove(MyAnchorThumb);
                MyAnchorThumb = null;
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

    /// <summary>
    /// root用Thumb
    /// rootは移動させない、というか移動させないときの識別用クラス
    /// </summary>
    public class RootThumb : GroupThumb
    {
        /// <summary>
        /// 選択されたThumb
        /// </summary>
        //public ObservableCollection<KisoThumb> MySelectThumbs { get; set; }

        static RootThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RootThumb), new FrameworkPropertyMetadata(typeof(RootThumb)));
        }
        public RootThumb()
        {
            Focusable = true;
            MyType = ThumbType.Root;
            //MySelectThumbs = [];
            MySelectedThumbs = [];
            DragDelta -= Thumb_DragDelta3;
            //DragDelta -= Thumb_DragDelta2;
            DragStarted -= KisoThumb_DragStarted2;
            DragCompleted -= KisoThumb_DragCompleted2;
            KeyDown -= KisoThumb_KeyDown;
            KeyUp -= KisoThumb_KeyUp;
            //PreviewMouseDown += RootThumb_PreviewMouseDown;
            GotKeyboardFocus += RootThumb_GotKeyboardFocus;
            Loaded += RootThumb_Loaded;
            MySelectedThumbs.CollectionChanged += MySelectedThumbs_CollectionChanged;
        }

        private void MySelectedThumbs_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems?[0] is KisoThumb n)
                {
                    n.IsSelected = true;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems?[0] is KisoThumb o)
                {
                    o.IsSelected = false;
                }
            }
        }

        private void RootThumb_Loaded(object sender, RoutedEventArgs e)
        {
            //ActiveGroupThumbの指定
            MyActiveGroupThumb = this;
            foreach (var item in MyThumbs)
            {
                item.IsSelectable = true;
            }
        }

        #region internalメソッド

        /// <summary>
        /// MySelectedThumbsの更新
        /// ctrl＋クリックで対象Thumbがリストになければ追加、あれば削除
        /// 通常クリックならリストをクリアして追加
        /// </summary>
        /// <param name="thumb">対象Thumb</param>
        internal void UpdateSelectedThumbs(KisoThumb thumb)
        {
            if (thumb.IsSelectable == false) { return; }
            if (MySelectedThumbs.Count == 1 && thumb == MySelectedThumbs[0]) { return; }

            if (MySelectedThumbs.Count == 0)
            {
                MySelectedThumbs.Add(thumb);
            }

            //ctrlキー押しながらの選択時、
            //要素数2個以上＋クリック対象がリストに存在しているなら削除、ないなら追加
            else if (MySelectedThumbs.Count >= 1 && Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (!MySelectedThumbs.Remove(thumb))
                {
                    MySelectedThumbs.Add(thumb);
                }
            }
            else
            {
                MySelectedThumbs.Clear();
                MySelectedThumbs.Add(thumb);
            }
        }

        #endregion internalメソッド

        #region パブリックなメソッド

        #endregion パブリックなメソッド

        #region イベントでの処理

        private void RootThumb_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (e.NewFocus is KisoThumb kiso)
            {
                MyFocusThumb = kiso;
            }

        }
        #endregion イベントでの処理

        #region 依存関係プロパティ


        public KisoThumb MyClickedThumb
        {
            get { return (KisoThumb)GetValue(MyClickedThumbProperty); }
            set { SetValue(MyClickedThumbProperty, value); }
        }
        public static readonly DependencyProperty MyClickedThumbProperty =
            DependencyProperty.Register(nameof(MyClickedThumb), typeof(KisoThumb), typeof(RootThumb), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnMyClickedThumbChanged)));

        private static void OnMyClickedThumbChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //var neko = Keyboard.Modifiers == ModifierKeys.Control;
            //FocusThumbの更新
            if (d is RootThumb rt)
            {
                if (e.NewValue is KisoThumb n && n.IsSelectable)
                {
                    rt.MyFocusThumb = n;
                }
            }
            //if (d is RootThumb rt)
            //{
            //    if (e.NewValue is KisoThumb n && n.MyParentThumb is GroupThumb gt)
            //    {
            //        rt.MyActiveGroupThumb = gt;
            //    }
            //}
        }

        public GroupThumb MyActiveGroupThumb
        {
            get { return (GroupThumb)GetValue(MyActiveGroupThumbProperty); }
            set { SetValue(MyActiveGroupThumbProperty, value); }
        }
        public static readonly DependencyProperty MyActiveGroupThumbProperty =
            DependencyProperty.Register(nameof(MyActiveGroupThumb), typeof(GroupThumb), typeof(RootThumb), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnMyActiveGroupThumbChanged)));

        private static void OnMyActiveGroupThumbChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RootThumb rt)
            {
                if (e.OldValue is GroupThumb o)
                {
                    foreach (var item in o.MyThumbs)
                    {
                        item.IsSelectable = false;
                    }
                }
                if (e.NewValue is GroupThumb n)
                {
                    foreach (var item in n.MyThumbs)
                    {
                        item.IsSelectable = true;
                    }
                }
            }
        }


        /// <summary>
        /// フォーカスされたThumb
        /// </summary>
        public KisoThumb MyFocusThumb
        {
            get { return (KisoThumb)GetValue(MyFocusThumbProperty); }
            set { SetValue(MyFocusThumbProperty, value); }
        }
        public static readonly DependencyProperty MyFocusThumbProperty =
            DependencyProperty.Register(nameof(MyFocusThumb), typeof(KisoThumb), typeof(RootThumb), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnMyFocusThumbChanged)));


        /// <summary>
        /// フォーカスされたThumbが変更されたとき、そのThumbの親要素を取得してMyActiveGroupThumbに設定
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnMyFocusThumbChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //if (d is RootThumb rt)
            //{
            //    if (e.NewValue is KisoThumb n)
            //    {
            //        rt.MyActiveGroupThumb = n.MyParentThumb ?? rt;
            //        //n.Focusable = true;
            //        //FocusManager.SetFocusedElement(rt, n);
            //        //Keyboard.Focus(n);
            //        //n.Focus();
            //    }
            //}
        }

        public ObservableCollection<KisoThumb> MySelectedThumbs
        {
            get { return (ObservableCollection<KisoThumb>)GetValue(MySelectedThumbsProperty); }
            set { SetValue(MySelectedThumbsProperty, value); }
        }
        public static readonly DependencyProperty MySelectedThumbsProperty =
            DependencyProperty.Register(nameof(MySelectedThumbs), typeof(ObservableCollection<KisoThumb>), typeof(RootThumb), new PropertyMetadata(null));


        #endregion 依存関係プロパティ

    }

}
