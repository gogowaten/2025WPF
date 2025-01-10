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

namespace _20250109_SelectedThumbs
{


    //Thumbの種類の識別用
    public enum ThumbType { None = 0, Root, Group, Text, Ellipse, Rect, Anchor }


    /// <summary>
    /// 基礎Thumb、すべてのCustomControlThumbの派生元
    /// </summary>
    [DebuggerDisplay("{MyType} {MyText}")]
    public abstract class KisoThumb : Thumb
    {
        //クリックダウンとドラッグ移動完了時に使う、直前に選択されたものかの判断用
        internal bool IsPreviewSelected { get; set; }

        #region 依存関係プロパティ


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


        public ObservableCollectionKisoThumb MySelectedThumbs
        {
            get { return (ObservableCollectionKisoThumb)GetValue(MySelectedThumbsProperty); }
            set { SetValue(MySelectedThumbsProperty, value); }
        }
        public static readonly DependencyProperty MySelectedThumbsProperty =
            DependencyProperty.Register(nameof(MySelectedThumbs), typeof(ObservableCollectionKisoThumb), typeof(RootThumb), new PropertyMetadata(null));

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



        #endregion 依存関係プロパティ

        #region 読み取り専用依存関係プロパティ



        private static readonly DependencyPropertyKey IsActiveGroupPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(IsActiveGroup), typeof(bool), typeof(KisoThumb), new PropertyMetadata(false));
        public static readonly DependencyProperty IsActiveGroupProperty = IsActiveGroupPropertyKey.DependencyProperty;
        public bool IsActiveGroup
        {
            get { return (bool)GetValue(IsActiveGroupPropertyKey.DependencyProperty); }
            internal set { SetValue(IsActiveGroupPropertyKey, value); }
        }




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
            MyBrushList = [];
            //透明
            MyBrushList.Add(MakeDushImageBrush.MakeBrush2ColorsDash(1, Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0)));
            //青DodgerBlue:IsFocus
            MyBrushList.Add(MakeDushImageBrush.MakeBrush2ColorsDash(8, Color.FromArgb(255, 30, 144, 255), Color.FromArgb(255, 255, 255, 255)));
            //青:IsSelected
            MyBrushList.Add(MakeDushImageBrush.MakeBrush2ColorsDash(8, Color.FromArgb(255, 135, 206, 250), Color.FromArgb(255, 255, 255, 255)));
            //灰色:IsSelectable
            MyBrushList.Add(MakeDushImageBrush.MakeBrush2ColorsDash(8, Color.FromArgb(64, 0, 0, 0), Color.FromArgb(64, 255, 255, 255)));
            //紫色:
            MyBrushList.Add(MakeDushImageBrush.MakeBrush2ColorsDash(8, Color.FromArgb(255, 186, 85, 211), Color.FromArgb(255, 255, 255, 255)));

            DataContext = this;
            //Focusable = true;
            //Focusable = false;
            MyType = ThumbType.None;
            PreviewMouseDown += KisoThumb_PreviewMouseDownTest;
            PreviewMouseUp += KisoThumb_PreviewMouseUpTest;
            DragStarted += KisoThumb_DragStarted2;
            DragCompleted += KisoThumb_DragCompleted2;
            //DragDelta += Thumb_DragDelta;
            //DragDelta += Thumb_DragDelta2;
            DragDelta += Thumb_DragDelta3;
        }

        #region イベントハンドラ

        /// <summary>
        /// クリックダウン時、
        /// ClickedThumb更新後、SelectedThumbsを更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void KisoThumb_PreviewMouseDownTest(object sender, MouseButtonEventArgs e)
        {
            //e.Sourceとe.OriginalSourceが一致したときのthisがクリックされたThumbと一致する
            if (e.Source == e.OriginalSource)
            {
                if (GetRootThumb() is RootThumb root)
                {
                    //クリックされたThumbをRootのClickedThumbプロパティに登録
                    root.MyClickedThumb = this;

                    //RootThumbのSelectedThumbsプロパティを更新
                    if (GetSelectableParentThumb(this) is KisoThumb current)
                    {
                        int selectedCount = root.MySelectedThumbs.Count;
                        bool isContained = root.MySelectedThumbs.Contains(current);
                        if (selectedCount == 0)
                        {
                            //追加
                            root.MySelectedThumbs.Add(current);
                            root.MyFocusThumb = current;
                        }
                        else if (!isContained && Keyboard.Modifiers == ModifierKeys.Control)
                        {
                            //追加
                            root.MySelectedThumbs.Add(current);
                            root.MyFocusThumb = current;
                            //直前追加のフラグ
                            current.IsPreviewSelected = true;
                        }
                        else if (!isContained && Keyboard.Modifiers == ModifierKeys.None)
                        {
                            //入れ替え
                            root.MySelectedThumbs.Clear();
                            root.MySelectedThumbs.Add(current);
                            root.MyFocusThumb = current;
                        }
                        else if (selectedCount > 1)
                        {
                            //直前追加ではない、のフラグ
                            current.IsPreviewSelected = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// マウスアップ時、BringIntoViewを実行する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void KisoThumb_PreviewMouseUpTest(object sender, MouseButtonEventArgs e)
        {
            if (e.Source == e.OriginalSource)
            {
                if (sender is KisoThumb t)
                {
                    //重要、BringIntoViewこれがないとすっ飛んでいく

                    //直接クリックしたThumbが対象になる、GroupThumbの中のThumbとか
                    t.BringIntoView();

                    //直接クリックしたものを含むSelectableなThumbが対象になる
                    //if (GetSelectableParentThumb(t) is KisoThumb current)
                    //{
                    //    current.BringIntoView();
                    //}
                }
            }
        }


        /// <summary>
        /// ドラッグ移動開始時
        /// アンカーThumbを作成追加、
        /// ぼやけ回避のため、座標を四捨五入してドットに合わせる
        /// </summary>
        internal void KisoThumb_DragStarted2(object sender, DragStartedEventArgs e)
        {
            if (sender is KisoThumb kiso)
            {
                if (GetSelectableParentThumb(kiso) is KisoThumb current)
                {
                    if (current.MyParentThumb is GroupThumb parent)
                    {
                        parent.AddAnchorThumb(current);
                        //座標を四捨五入で整数にしてぼやけ回避
                        current.MyLeft = (int)(current.MyLeft + 0.5);
                        current.MyTop = (int)(current.MyTop + 0.5);
                        e.Handled = true;
                    }
                }
            }

        }

        /// <summary>
        /// 移動距離を四捨五入(丸めて)整数ドラッグ移動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void Thumb_DragDelta3(object sender, DragDeltaEventArgs e)
        {
            if (sender is KisoThumb t && t.IsSelectable)
            {
                if (GetRootThumb() is RootThumb root)
                {
                    foreach (var item in root.MySelectedThumbs)
                    {
                        item.MyLeft += (int)(e.HorizontalChange + 0.5);
                        item.MyTop += (int)(e.VerticalChange + 0.5);
                    }
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// ドラッグ移動終了時
        /// アンカーThumbを削除と要素再配置後に親要素の再配置
        /// </summary>
        internal void KisoThumb_DragCompleted2(object sender, DragCompletedEventArgs e)
        {
            //SelectedThumbsの更新
            if (e.Source == e.OriginalSource)
            {
                if (GetRootThumb() is RootThumb root)
                {
                    if (GetSelectableParentThumb(this) is KisoThumb current)
                    {
                        if (root.MySelectedThumbs.Count > 1)
                        {
                            if (!current.IsPreviewSelected && e.HorizontalChange == 0 && e.VerticalChange == 0)
                            {
                                //直前に選択されたものじゃなければ削除
                                if (Keyboard.Modifiers == ModifierKeys.None)
                                {
                                    //入れ替え
                                    root.MySelectedThumbs.Clear();
                                    root.MySelectedThumbs.Add(current);
                                    root.MyFocusThumb = current;
                                }
                                else if (Keyboard.Modifiers == ModifierKeys.Control)
                                {
                                    int myIndex = root.MySelectedThumbs.IndexOf(current);
                                    root.MySelectedThumbs.Remove(current);
                                    //一個前の要素をFocusにする
                                    if (myIndex == 0)
                                    {
                                        root.MyFocusThumb = root.MySelectedThumbs[0];
                                    }
                                    else
                                    {
                                        root.MyFocusThumb = root.MySelectedThumbs[myIndex - 1];
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (e.Source == e.OriginalSource)
            {
                if (e.Source is KisoThumb kiso)
                {
                    if (GetSelectableParentThumb(kiso) is KisoThumb current)
                    {
                        current.MyParentThumb?.RemoveAnchorThumb();
                    }
                    //再レイアウト配置
                    kiso.MyParentThumb?.ReLayout3();
                    e.Handled = true;
                }
            }

        }

        #endregion イベントハンドラ


        #region internalメソッド

        /// <summary>
        /// RootThumbを取得
        /// </summary>
        /// <returns></returns>
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
        /// SelectableなThumbをParentを辿って取得する
        /// </summary>
        /// <param name="thumb"></param>
        /// <returns></returns>
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


        #endregion internalメソッド


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
        public void AddAnchorThumb(KisoThumb thumb)
        {
            MyAnchorThumb = new AnchorThumb
            {
                Visibility = Visibility.Hidden,
                Width = thumb.ActualWidth,
                Height = thumb.ActualHeight,
                MyLeft = thumb.MyLeft,
                MyTop = thumb.MyTop
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
        static RootThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RootThumb), new FrameworkPropertyMetadata(typeof(RootThumb)));
        }
        public RootThumb()
        {
            Focusable = true;
            MyType = ThumbType.Root;
            MySelectedThumbs = [];
            DragDelta -= Thumb_DragDelta3;
            DragStarted -= KisoThumb_DragStarted2;
            DragCompleted -= KisoThumb_DragCompleted2;
            Loaded += RootThumb_Loaded;
        }

        #region internalメソッド

        /// <summary>
        /// MySelectedThumbsの更新、クリックしたときに使う
        /// FocusThumbの更新も行っているけど、SelectionThumbのイベントで行ったほうがいいかも？
        /// ctrl＋クリックで対象Thumbがリストになければ追加、あれば削除
        /// 通常クリックならリストをクリアして追加
        /// </summary>
        /// <param name="thumb">対象Thumb</param>
        internal void UpdateSelectedThumbs(KisoThumb thumb)
        {
            if (thumb.IsSelectable == false) { return; }

            //要素数が0のときは普通に追加
            if (MySelectedThumbs.Count == 0)
            {
                MySelectedThumbs.Add(thumb);
                MyFocusThumb = thumb;
            }
            //通常クリックのとき
            if (Keyboard.Modifiers == ModifierKeys.None)
            {
                //リストクリア後に追加してFocusThumbにする
                MySelectedThumbs.Clear();
                MySelectedThumbs.Add(thumb);
                MyFocusThumb = thumb;
            }

            //ctrlクリックの場合
            else if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (MySelectedThumbs.Count >= 1 && MySelectedThumbs.Contains(thumb) == false)
                {
                    //選択数が1個以上＋対象が未選択の場合、追加してFocusThumb
                    MySelectedThumbs.Add(thumb);
                    MyFocusThumb = thumb;
                }
                else if (MySelectedThumbs.Count > 1 && MySelectedThumbs.Contains(thumb))
                {
                    //選択数が2個以上＋対象が選択状態の場合、マウスドラッグ移動を確認するので保留
                    //さらにFocusThumbだった場合は、リストの最後の要素をFocusThumbにする
                    MySelectedThumbs.Remove(thumb);
                    if (thumb == MyFocusThumb)
                    {
                        MyFocusThumb = MySelectedThumbs[^1];
                    }
                }
            }
        }

        #endregion internalメソッド

        #region パブリックなメソッド

        /// <summary>
        /// ActiveGroupThumbを外(Root)側のGroupThumbへ変更
        /// </summary>
        public void ActiveGroupToOutside()
        {
            if (MyActiveGroupThumb?.MyParentThumb is GroupThumb gt)
            {
                GroupThumb motoGroup = MyActiveGroupThumb;
                if (ChangeActiveGroupThumb(gt))
                {
                    MyFocusThumb = motoGroup;
                    MySelectedThumbs.Add(motoGroup);
                };
            }
        }

        /// <summary>
        /// ActiveGroupThumbを内側のGroupThumbへ変更
        /// </summary>
        public void ActiveGroupToInside()
        {
            if (MyFocusThumb is GroupThumb gt)
            {
                if (ChangeActiveGroupThumb(gt))
                {
                    if (gt.MyThumbs.Contains(MyClickedThumb))
                    {
                        MyFocusThumb = MyClickedThumb;
                        MySelectedThumbs.Add(MyClickedThumb);
                    }
                    else
                    {
                        //クリックの系列＋選択可能をフォーカスにする
                        if (GetIsSelectableParent(MyClickedThumb) is GroupThumb selectabelPrent)
                        {
                            MyFocusThumb = selectabelPrent;
                            MySelectedThumbs.Clear();
                            MySelectedThumbs.Add(selectabelPrent);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 対象Thumbの親を辿ってIsSelectableなThumbを返す
        /// </summary>
        /// <param name="thumb">対象Thumb</param>
        /// <returns></returns>
        private static KisoThumb? GetIsSelectableParent(KisoThumb thumb)
        {
            if (thumb.MyParentThumb is GroupThumb gt)
            {
                if (gt.IsSelectable)
                {
                    return gt;
                }
                else if (gt.MyParentThumb is GroupThumb mo)
                {
                    return GetIsSelectableParent(mo);
                }
            }
            return null;
        }



        /// <summary>
        /// ClickedのParentをActiveGroupThumbにする
        /// </summary>
        public void ActiveGroupFromClickedThumbsParent()
        {
            if (MyClickedThumb?.MyParentThumb is GroupThumb gt)
            {
                if (ChangeActiveGroupThumb(gt))
                {
                    MySelectedThumbs.Add(MyClickedThumb);
                    MyFocusThumb = MyClickedThumb;
                };
            }
        }

        /// <summary>
        /// ActiveGroupThumbの変更
        /// </summary>
        /// <param name="group">指定GroupThumb</param>
        private bool ChangeActiveGroupThumb(GroupThumb group)
        {
            if (MyActiveGroupThumb != group)
            {
                MyActiveGroupThumb = group;
                return true;
            }
            return false;
        }

        #endregion パブリックなメソッド

        #region イベントでの処理


        private void RootThumb_Loaded(object sender, RoutedEventArgs e)
        {
            //ActiveGroupThumbの指定
            MyActiveGroupThumb = this;
            foreach (var item in MyThumbs)
            {
                item.IsSelectable = true;
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
            ////FocusThumbの更新
            //if (d is RootThumb rt)
            //{
            //    if (e.NewValue is KisoThumb n && n.IsSelectable)
            //    {
            //        rt.MyFocusThumb = n;
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

        /// <summary>
        /// ActiveGroupThumb変更に伴う処理
        /// SelectedThumbsの処理
        /// FocusThumbの変更
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnMyActiveGroupThumbChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RootThumb rt)
            {
                //SelectedThumbsの処理
                rt.MySelectedThumbs.Clear();
                if (e.OldValue is GroupThumb o)
                {
                    o.IsActiveGroup = false;
                    foreach (var item in o.MyThumbs)
                    {
                        item.IsSelectable = false;
                    }
                }
                if (e.NewValue is GroupThumb n)
                {
                    n.IsActiveGroup = true;
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
        /// フォーカスされたThumbが変更されたとき、IsFocusの変更
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnMyFocusThumbChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RootThumb rt)
            {
                if (e.NewValue is KisoThumb n) { n.IsFocus = true; }
                if (e.OldValue is KisoThumb o) { o.IsFocus = false; }
            }
        }



        #endregion 依存関係プロパティ

    }


}
