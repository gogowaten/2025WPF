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

namespace _20250219
{



    //Thumbの種類の識別用
    public enum ThumbType { None = 0, Root, Group, Text, Ellipse, Rect, Anchor }


    /// <summary>
    /// 基礎Thumb、すべてのCustomControlThumbの派生元
    /// </summary>
    [DebuggerDisplay("{MyThumbType} {MyText}")]
    public abstract class KisoThumb : Thumb
    {
        //クリックダウンとドラッグ移動完了時に使う、直前に選択されたものかの判断用
        bool IsPreviewSelected { get; set; }



        public ThumbType MyThumbType { get; protected set; }

        //親要素の識別用。自身がグループ化されたときに親要素のGroupThumbを入れておく
        public GroupThumb? MyParentThumb { get; internal set; }
        static KisoThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KisoThumb), new FrameworkPropertyMetadata(typeof(KisoThumb)));
        }
        public KisoThumb()
        {
            MyItemData = new() { MyThumbType = ThumbType.None };
            InitializeWakuBrush();

            //DataContext = this;
            Focusable = true;
            //Focusable = false;
            //MyThumbType = ThumbType.None;

            Initialized += KisoThumb_Initialized;
            PreviewMouseDown += KisoThumb_PreviewMouseDownTest;
            PreviewMouseUp += KisoThumb_PreviewMouseUpTest;
            DragStarted += KisoThumb_DragStarted2;
            DragCompleted += KisoThumb_DragCompleted2;
            //DragDelta += Thumb_DragDelta;
            //DragDelta += Thumb_DragDelta2;
            DragDelta += Thumb_DragDelta3;
            //KeyDown += KisoThumb_KeyDown;
            KeyUp += KisoThumb_KeyUp;
            PreviewKeyDown += KisoThumb_PreviewKeyDown;

        }

        private void KisoThumb_Initialized(object? sender, EventArgs e)
        {
            //デザイン画面で作成された要素の場合、ItemDataは無いので新規作成後に
            //デザイン画面の設定をItemDataに反映してからバインド設定

            //Anchorはバインド無し
            if(MyThumbType == ThumbType.Anchor)
            {
                MyItemData.MyThumbType = MyThumbType;
            }
            else if (MyItemData.MyThumbType == ThumbType.None)
            {
                MyItemData.MyThumbType = MyThumbType;
                CopyValueToItemData();
                MyItemDataBind();
            }
            //ファイルやItemDataから作成された要素の場合、そのままItemDataとバインド
            else
            {
                MyItemDataBind();
            }

            DataContext = MyItemData; //ここで適用
        }


        //ItemDataなしのコンストラクタで使う
        //バインドの前に、XAMLからの設定をItemDataにいれる
        private void CopyValueToItemData()
        {
            //XAMLでの設定をItemDataに入れる
            MyItemData.MyLeft = MyLeft;
            MyItemData.MyTop = MyTop;
            MyItemData.MyText = MyText;
            MyItemData.MyFontSize = FontSize;
            MyItemData.MyZIndex = MyZIndex;

            Color bc = ((SolidColorBrush)MyForeground).Color;
            MyItemData.MyForegroundA = bc.A;
            MyItemData.MyForegroundR = bc.R;
            MyItemData.MyForegroundG = bc.G;
            MyItemData.MyForegroundB = bc.B;

            bc = ((SolidColorBrush)MyBackground).Color;
            MyItemData.MyBackgroundA = bc.A;
            MyItemData.MyBackgroundR = bc.R;
            MyItemData.MyBackgroundG = bc.G;
            MyItemData.MyBackgroundB = bc.B;

        }


        //バインド、ItemDataをソース
        private void MyItemDataBind()
        {
            //バインド、ItemDataをソース
            SetBinding(MyLeftProperty, nameof(MyItemData.MyLeft));
            SetBinding(MyTopProperty, nameof(MyItemData.MyTop));
            SetBinding(MyZIndexProperty, nameof(MyItemData.MyZIndex));
            

            SetBinding(MyTextProperty, nameof(MyItemData.MyText));
            SetBinding(FontSizeProperty, nameof(MyItemData.MyFontSize));
            var neko = MyItemData;

            var mb = new MultiBinding() { Converter = new MyConverterARGBtoSolidBrush() };
            mb.Bindings.Add(new Binding(nameof(MyItemData.MyForegroundA)));
            mb.Bindings.Add(new Binding(nameof(MyItemData.MyForegroundR)));
            mb.Bindings.Add(new Binding(nameof(MyItemData.MyForegroundG)));
            mb.Bindings.Add(new Binding(nameof(MyItemData.MyForegroundB)));
            SetBinding(MyForegroundProperty, mb);


            mb = new() { Converter = new MyConverterARGBtoSolidBrush() };
            mb.Bindings.Add(new Binding(nameof(MyItemData.MyBackgroundA)));
            mb.Bindings.Add(new Binding(nameof(MyItemData.MyBackgroundR)));
            mb.Bindings.Add(new Binding(nameof(MyItemData.MyBackgroundG)));
            mb.Bindings.Add(new Binding(nameof(MyItemData.MyBackgroundB)));
            SetBinding(MyBackgroundProperty, mb);

        }


        private void InitializeWakuBrush()
        {
            MyBrushList = [];
            //透明
            MyBrushList.Add(BitmapImageBrushMaker.MakeBrush2ColorsDash(1, Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0)));
            //青DodgerBlue:IsFocus
            MyBrushList.Add(BitmapImageBrushMaker.MakeBrush2ColorsDash(8, Color.FromArgb(255, 30, 144, 255), Color.FromArgb(255, 255, 255, 255)));
            //青:IsSelected
            MyBrushList.Add(BitmapImageBrushMaker.MakeBrush2ColorsDash(8, Color.FromArgb(255, 135, 206, 250), Color.FromArgb(255, 255, 255, 255)));
            //半透明灰色:IsSelectable
            MyBrushList.Add(BitmapImageBrushMaker.MakeBrush2ColorsDash(8, Color.FromArgb(64, 0, 0, 0), Color.FromArgb(64, 255, 255, 255)));
            //黄色:
            MyBrushList.Add(BitmapImageBrushMaker.MakeBrush2ColorsDash(8, Color.FromArgb(255, 186, 85, 211), Color.FromArgb(255, 255, 255, 255)));

        }


        #region イベントハンドラ

        #region キーボード


        private void KisoThumb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is RootThumb rt && rt.MyClickedThumb != null)
            {
                //選択Thumbを方向キーで10ピクセル移動
                MoveThumb(rt.MySelectedThumbs, e.Key, 10);
                e.Handled = true;
            }
        }

        private void MoveThumb(ObservableCollection<KisoThumb> thumbs, Key key, double amount)
        {
            foreach (var item in thumbs)
            {
                MoveThumb(item, key, amount);
            }
        }

        private void MoveThumb(KisoThumb kiso, Key key, double amount)
        {
            switch (key)
            {
                case Key.Left:
                    kiso.MyLeft -= amount;
                    break;
                case Key.Right:
                    kiso.MyLeft += amount;
                    break;
                case Key.Up:
                    kiso.MyTop -= amount;
                    break;
                case Key.Down:
                    kiso.MyTop += amount;
                    break;
            }
        }

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


        ///// <summary>
        ///// 方向キーの方向へ10ピクセル移動
        ///// </summary>
        //internal void KisoThumb_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (sender is KisoThumb t)
        //    {
        //        if (e.Key == Key.Left)
        //        {
        //            t.MyLeft -= 10;
        //            e.Handled = true;
        //        }
        //        else if (e.Key == Key.Right)
        //        {
        //            t.MyLeft += 10;
        //            e.Handled = true;
        //        }
        //        else if (e.Key == Key.Up)
        //        {
        //            t.MyTop -= 10;
        //            e.Handled = true;
        //        }
        //        else if (e.Key == Key.Down)
        //        {
        //            t.MyTop += 10;
        //            e.Handled = true;
        //        }

        //    }
        //}

        #endregion キーボード

        #region マウスクリック

        /// <summary>
        /// クリックダウン時、
        /// ClickedThumb更新後、SelectedThumbsを更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void KisoThumb_PreviewMouseDownTest(object sender, MouseButtonEventArgs e)
        {
            //イベント発生順序、Root、Group、その他Item
            var sou = e.Source;
            var ori = e.OriginalSource;

            if (e.Source == e.OriginalSource)
            {
                UpdateSelectedThumbs(this, Keyboard.Modifiers == ModifierKeys.Control);
            }
        }

        /// <summary>
        /// SelectedThumbsを更新、クリックダウン専用
        /// </summary>
        /// <param name="clickedThumb">クリックされたThumb</param>
        /// <param name="isControlPressed">ctrlキーが押されていた</param>
        internal void UpdateSelectedThumbs(KisoThumb clickedThumb, bool isControlPressed)
        {
            if (GetRootThumb() is RootThumb root && GetSelectableThumb(clickedThumb) is KisoThumb current)
            {
                root.MyClickedThumb = clickedThumb;
                int selectedCount = root.MySelectedThumbs.Count;
                bool isContained = root.MySelectedThumbs.Contains(current);
                if (selectedCount == 0)
                {
                    //追加
                    root.AddToSelectedThumbs(current);
                }
                //選択されていない＋ctrlありの場合、直前追加
                else if (!isContained && isControlPressed)
                {
                    //追加
                    root.AddToSelectedThumbs(current);
                    //直前追加のフラグ
                    current.IsPreviewSelected = true;
                }
                //選択されていない＋ctrl無しの場合、Selectedをクリアして追加
                else if (!isContained && !isControlPressed)
                {
                    //入れ替え
                    root.SelectedThumbsClearAndAddThumb(current);
                }
                //Selectedが1より多かった場合
                else if (selectedCount > 1)
                {
                    //直前追加ではない、のフラグ
                    current.IsPreviewSelected = false;
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

        #endregion マウスクリック

        #region マウスドラッグ移動

        /// <summary>
        /// ドラッグ移動開始時
        /// アンカーThumbを作成追加、
        /// ぼやけ回避のため、座標を四捨五入してドットに合わせる
        /// </summary>
        internal void KisoThumb_DragStarted2(object sender, DragStartedEventArgs e)
        {
            if (sender is KisoThumb kiso)
            {
                if (GetSelectableThumb(kiso) is KisoThumb current)
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
            if (e.Source != e.OriginalSource) { return; }

            //SelectedThumbsの更新
            if (GetRootThumb() is RootThumb root &&
                GetSelectableThumb(this) is KisoThumb current &&
                root.MySelectedThumbs.Count > 1 &&
                !current.IsPreviewSelected &&
                e.HorizontalChange == 0 &&
                e.VerticalChange == 0)
            {
                //直前に選択されたものじゃなければ削除
                if (Keyboard.Modifiers == ModifierKeys.None)
                {
                    //入れ替え
                    root.SelectedThumbsClearAndAddThumb(current);
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

            if (e.Source is KisoThumb kiso)
            {
                if (GetSelectableThumb(kiso) is KisoThumb ima)
                {
                    ima.MyParentThumb?.RemoveAnchorThumb();
                }
                //再レイアウト配置
                kiso.MyParentThumb?.ReLayout3();
                e.Handled = true;
            }

        }
        #endregion マウスドラッグ移動

        #endregion イベントハンドラ

        #region 依存関係プロパティ

        //特殊、フィールドにしたほうがいい？
        public ItemData MyItemData
        {
            get { return (ItemData)GetValue(MyItemDataProperty); }
            set { SetValue(MyItemDataProperty, value); }
        }
        public static readonly DependencyProperty MyItemDataProperty =
            DependencyProperty.Register(nameof(MyItemData), typeof(ItemData), typeof(KisoThumb), new PropertyMetadata(null));


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


        public Brush MyForeground
        {
            get { return (Brush)GetValue(MyForegroundProperty); }
            set { SetValue(MyForegroundProperty, value); }
        }
        public static readonly DependencyProperty MyForegroundProperty =
            DependencyProperty.Register(nameof(MyForeground), typeof(Brush), typeof(KisoThumb), new PropertyMetadata(Brushes.Black));


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

        #endregion 依存関係プロパティ


        #region メソッド

        #region protectedメソッド

        /// <summary>
        /// RootThumbを取得
        /// </summary>
        /// <returns></returns>
        protected RootThumb? GetRootThumb()
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
        protected KisoThumb? GetSelectableThumb(KisoThumb? thumb)
        {
            if (thumb is null) { return null; }
            if (thumb.IsSelectable) { return thumb; }
            if (thumb.MyParentThumb is GroupThumb gt)
            {
                if (gt.IsSelectable)
                {
                    return gt;
                }
                else
                {
                    return GetSelectableThumb(gt);
                }
            }
            return null;
        }


        /// <summary>
        /// ZIndexの修正、MyThumbsのIndexに合わせる
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
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


        /// <summary>
        /// Selectableの判定をParentを遡って行う
        /// </summary>
        /// <param name="kiso"></param>
        /// <returns></returns>
        protected static bool IsSelectedWithParent(KisoThumb? kiso)
        {
            if (kiso == null) { return false; }
            if (kiso.IsSelected) { return true; }
            else
            {
                return IsSelectedWithParent(kiso.MyParentThumb);
            }
        }

        #endregion protectedメソッド

        #region public

        #region ZIndex


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

        /// <summary>
        /// 最前面へ移動
        /// </summary>
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

        /// <summary>
        /// 最背面へ移動
        /// </summary>
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

        #endregion ZIndex

        #endregion public




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
            MyThumbType = ThumbType.Anchor;
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
            MyThumbType = ThumbType.Text;
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
            MyThumbType = ThumbType.Ellipse;
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
            MyThumbType = ThumbType.Group;
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
        #endregion 初期化

        #region イベントハンドラ

        /// <summary>
        /// 子要素の追加時
        /// 子要素に親要素(自身)を登録
        /// </summary>
        private void MyThumbs_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems?[0] is KisoThumb ni)
            {
                ni.MyParentThumb = this;
                //ZIndexをCollectionのIndexに合わせる、
                //挿入箇所より後ろの要素はすべて変更
                int index = e.NewStartingIndex;
                for (int i = index; i < MyThumbs.Count; i++)
                {
                    MyThumbs[i].MyZIndex = i;
                }

            }
            else if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems?[0] is KisoThumb ot)
            {
                //ot.MyParentThumb = null;

                //ZIndexをCollectionのIndexに合わせる、
                //変更対象条件は、IsSelectedではない＋削除箇所より後ろ
                int index = e.OldStartingIndex;
                for (int i = index; i < MyThumbs.Count; i++)
                {
                    if (!MyThumbs[i].IsSelected)
                    {
                        MyThumbs[i].MyZIndex = i;
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Move)
            {
            }
        }

        #endregion イベントハンドラ

        #region publicメソッド



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
                if (item.MyThumbType != ThumbType.Anchor)
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
                if (MyThumbType != ThumbType.Root) { MyLeft += left; }
            }

            if (top != MyTop)
            {
                foreach (var item in MyThumbs) { item.MyTop -= top; }

                if (MyThumbType != ThumbType.Root) { MyTop += top; }
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
            MyThumbType = ThumbType.Root;
            MySelectedThumbs = [];
            DragDelta -= Thumb_DragDelta3;
            DragStarted -= KisoThumb_DragStarted2;
            DragCompleted -= KisoThumb_DragCompleted2;
            //KeyDown -= KisoThumb_KeyDown;
            KeyUp -= KisoThumb_KeyUp;
            GotKeyboardFocus += RootThumb_GotKeyboardFocus;
            Loaded += RootThumb_Loaded;
        }

        #region internalメソッド


        /// <summary>
        /// MySelectedThumbsへの追加
        /// </summary>
        /// <param name="kiso"></param>
        internal void AddToSelectedThumbs(KisoThumb kiso)
        {
            MySelectedThumbs.Add(kiso);
            MyFocusThumb = kiso;
        }

        /// <summary>
        /// MySelectedThumbsへの入れ替え、クリア後に対象を追加
        /// </summary>
        /// <param name="kiso">対象Thumb</param>
        internal void SelectedThumbsClearAndAddThumb(KisoThumb? kiso)
        {
            if (kiso is null) { return; }
            kiso.IsSelectable = true;
            MySelectedThumbs.Clear();
            AddToSelectedThumbs(kiso);
        }


        #endregion internalメソッド

        #region パブリックなメソッド

        #region ActiveGroupThumbの変更


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
                    AddToSelectedThumbs(motoGroup);
                }
                ;
            }
        }

        /// <summary>
        /// ActiveGroupThumbを内側のGroupThumbへ変更
        /// FocusThumbをActiveGroupThumbに変更して潜っていく感じ
        /// </summary>
        public void ActiveGroupToInside()
        {
            if (MyFocusThumb is null) { return; }
            if (MyFocusThumb is GroupThumb nextGroup)
            {
                if (ChangeActiveGroupThumb(nextGroup))
                {
                    //次のFocusThumbの選定、ClickedThumbの親
                    KisoThumb? nextFocus = GetSelectableThumb(MyClickedThumb);
                    MyFocusThumb = nextFocus;
                    SelectedThumbsClearAndAddThumb(nextFocus);
                }
            }
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
                    AddToSelectedThumbs(MyClickedThumb);
                }
                ;
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

        #endregion ActiveGroupThumbの変更

        #region Thumb追加と削除

        /// <summary>
        /// ActiveGroupThumbにThumbを追加、
        /// 追加場所はFocusThumbがあればそれが基準になる、Z座標では+1
        /// 最初の追加要素ならすべて0で配置
        /// </summary>
        /// <param name="thumb"></param>
        /// <param name="insertIndex">挿入先指定</param>
        public void AddThumbToActiveGroup(KisoThumb thumb, int insertIndex = -1)
        {
            thumb.IsSelectable = true;
            int index = insertIndex == -1 ? MyThumbs.Count : insertIndex;
            if (MyFocusThumb != null)
            {
                thumb.MyLeft += MyFocusThumb.MyLeft;
                thumb.MyTop += MyFocusThumb.MyTop;
                index = MyActiveGroupThumb.MyThumbs.IndexOf(MyFocusThumb) + 1;
            }
            else if (MyActiveGroupThumb.MyThumbs.Count == 0)
            {
                thumb.MyLeft = 0; thumb.MyTop = 0;
            }
            MyActiveGroupThumb.MyThumbs.Insert(index, thumb);
            SelectedThumbsClearAndAddThumb(thumb);
            ReLayout3();
        }

        /// <summary>
        /// SelectedThumbsの全要素をActiveGroupThumbから削除
        /// </summary>
        /// <param name="withReLayout">削除後に再配置処理をするならtrue</param>
        public void RemoveSelectedThumbsFromActiveGroup(bool withReLayout = true)
        {
            int selectedCount = MySelectedThumbs.Count;
            if (selectedCount == 0) { return; }
            int targetCount = MyActiveGroupThumb.MyThumbs.Count;
            if (selectedCount == targetCount)
            {
                RemoveAll();
            }
            //else if(targetCount-selectedCount == 1)
            //{
            //    //グループ解除

            //}

            if (IsSelectedWithParent(MyClickedThumb)) { MyClickedThumb = null; }
            MyFocusThumb = null;

            foreach (var item in MySelectedThumbs)
            {
                item.IsSelectable = false;
                MyActiveGroupThumb.MyThumbs.Remove(item);
            }
            MySelectedThumbs.Clear();

            if (withReLayout) { ReLayout3(); }
        }

        /// <summary>
        /// 全削除
        /// </summary>
        public void RemoveAll()
        {
            MyThumbs.Clear();
            MyFocusThumb = null;
            MyClickedThumb = null;
            MyActiveGroupThumb = this;
            MySelectedThumbs.Clear();
            ReLayout3();
        }

        /// <summary>
        /// SelectedThumbsからGroupThumbを生成、追加
        /// </summary>
        public void AddGroupFromSelected()
        {
            if (MyActiveGroupThumb.MyThumbs.Count == MySelectedThumbs.Count) { return; }
            if (MySelectedThumbs.Count < 2) { return; }

            GroupThumb group = MakeGroupFromSelectedThumbs();

            //選択ThumbをActiveGroupThumbから一掃
            RemoveSelectedThumbsFromActiveGroup(false);

            AddThumbToActiveGroup(group, group.MyZIndex);
        }

        /// <summary>
        /// SelectedThumbsからGroupThumbを作成
        /// GroupThumbのZIndexは選択中の最大値-(選択個数 - 1)
        /// </summary>
        /// <returns></returns>
        private GroupThumb MakeGroupFromSelectedThumbs()
        {
            List<KisoThumb> list = GetSortedSelectedThumbs();
            int minZIndex = MySelectedThumbs.Max(x => x.MyZIndex);
            minZIndex -= list.Count - 1;
            double minLeft = MySelectedThumbs.Min(x => x.MyLeft);
            double minTop = MySelectedThumbs.Min(x => x.MyTop);
            GroupThumb group = new();
            foreach (var item in list)
            {
                item.MyLeft -= minLeft;
                item.MyTop -= minTop;
                group.MyThumbs.Add(item);
            }
            group.IsSelectable = true;
            group.MyLeft = minLeft;
            group.MyTop = minTop;
            group.MyZIndex = minZIndex;
            return group;
        }

        /// <summary>
        /// 選択ThumbをIndex順に並べたリストを返す
        /// </summary>
        /// <returns></returns>
        public List<KisoThumb> GetSortedSelectedThumbs()
        {
            List<KisoThumb> result = new();
            foreach (var item in MyActiveGroupThumb.MyThumbs)
            {
                if (item.IsSelected)
                {
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// グループ解除、
        /// 解除後は元グループの要素全てを選択状態にする
        /// </summary>
        public void UngroupFocusThumb()
        {
            if (MyFocusThumb is GroupThumb group &&
                group.MyParentThumb is GroupThumb parent)
            {
                MyFocusThumb = null;//優先処理
                var list = MakeUngroupList(group);
                group.MyThumbs.Clear();
                parent.MyThumbs.Remove(group);
                MySelectedThumbs.Clear();

                //ActiveGroupThumbとSelectedThumbsに要素を追加
                foreach (var item in list)
                {
                    item.IsSelectable = true;
                    MyActiveGroupThumb.MyThumbs.Insert(item.MyZIndex, item);
                    MySelectedThumbs.Add(item);
                }

                //FocusThumbの選定、Clickedが含まれていたらそれ、なければ先頭要素
                if (GetSelectableThumb(MyClickedThumb) is KisoThumb nextFocus)
                {
                    MyFocusThumb = nextFocus;
                }
                else
                {
                    MyFocusThumb = MySelectedThumbs[0];
                }

                ReLayout3();
            }

            static List<KisoThumb> MakeUngroupList(GroupThumb group)
            {
                List<KisoThumb> result = [];
                foreach (var item in group.MyThumbs)
                {
                    item.MyLeft += group.MyLeft;
                    item.MyTop += group.MyTop;
                    item.MyZIndex += group.MyZIndex;
                    result.Add(item);
                }
                return result;
            }
        }

        #endregion Thumb追加と削除

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

        private void RootThumb_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //if (e.NewFocus is KisoThumb kiso)
            //{
            //    MyFocusThumb = kiso;
            //}

        }
        #endregion イベントでの処理

        #region 依存関係プロパティ


        public ObservableCollectionKisoThumb MySelectedThumbs
        {
            get { return (ObservableCollectionKisoThumb)GetValue(MySelectedThumbsProperty); }
            set { SetValue(MySelectedThumbsProperty, value); }
        }
        public static readonly DependencyProperty MySelectedThumbsProperty =
            DependencyProperty.Register(nameof(MySelectedThumbs), typeof(ObservableCollectionKisoThumb), typeof(RootThumb), new PropertyMetadata(null));

        public KisoThumb? MyClickedThumb
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

        private static void OnMyActiveGroupThumbChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RootThumb rt)
            {
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
        public KisoThumb? MyFocusThumb
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
                if (e.NewValue is KisoThumb n)
                {
                    n.IsFocus = true;

                    //rt.MyActiveGroupThumb = n.MyParentThumb ?? rt;
                    //n.Focusable = true;
                    //FocusManager.SetFocusedElement(rt, n);
                    //Keyboard.Focus(n);
                    //n.Focus();
                }
                if (e.OldValue is KisoThumb o) { o.IsFocus = false; }
            }
        }



        #endregion 依存関係プロパティ

    }

    #region コンバーター


    public class MyWakuBrushConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            List<Brush> brushes = (List<Brush>)values[0];
            bool b1 = (bool)values[1];
            bool b2 = (bool)values[2];
            bool b3 = (bool)values[3];
            bool b4 = (bool)values[4];

            if (b1) { return brushes[1]; }//IsFocus
            else if (b2) { return brushes[2]; }//IsSelected
            else if (b3) { return brushes[3]; }//IsEelectable
            else if (b4) { return brushes[4]; }//IsActiveGroup
            else { return brushes[0]; }//それ以外
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConverterARGBtoSolidBrush : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var a = (byte)values[0];
            var r = (byte)values[1];
            var g = (byte)values[2];
            var b = (byte)values[3];
            return new SolidColorBrush(Color.FromArgb(a, r, g, b));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var br = (SolidColorBrush)value;
            return [br.Color.A, br.Color.R, br.Color.G, br.Color.B];

        }
    }

    #endregion コンバーター

    public class ObservableCollectionKisoThumb : ObservableCollection<KisoThumb>
    {
        protected override void ClearItems()
        {
            foreach (var item in Items)
            {
                item.IsSelected = false;
            }
            base.ClearItems();
        }
        protected override void SetItem(int index, KisoThumb item)
        {
            item.IsSelected = true;
            base.SetItem(index, item);
        }
        protected override void RemoveItem(int index)
        {
            Items[index].IsSelected = false;
            base.RemoveItem(index);
        }
        protected override void InsertItem(int index, KisoThumb item)
        {
            item.IsSelected = true;
            base.InsertItem(index, item);
        }
    }


}

