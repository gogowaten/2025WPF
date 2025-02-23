using _20250223;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
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
using System.Xml;

namespace _20250223
{


    //Thumbの種類の識別用
    public enum ThumbType { None = 0, Root, Group, Text, Ellipse, Rect, Anchor }


    /// <summary>
    /// 基礎Thumb、すべてのCustomControlThumbの派生元
    /// </summary>
    [DebuggerDisplay("{MyThumbType} {MyItemData.MyText}")]
    public abstract class KisoThumb : Thumb
    {
        //クリックダウンとドラッグ移動完了時に使う、直前に選択されたものかの判断用
        bool IsPreviewSelected { get; set; }

        public ThumbType MyThumbType { get; protected set; }

        //親要素の識別用。自身がグループ化されたときに親要素のGroupThumbを入れておく
        public GroupThumb? MyParentThumb { get; set; }


        static KisoThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KisoThumb), new FrameworkPropertyMetadata(typeof(KisoThumb)));
        }
        public KisoThumb()
        {

            MyItemData = new() { MyThumbType = ThumbType.None };
            InitializeWakuBrush();

            DataContext = MyItemData;
            //DataContext = this;
            Focusable = true;

            MyThumbType = ThumbType.None;

            Initialized += KisoThumb_Initialized;

            PreviewMouseDown += KisoThumb_PreviewMouseDownTest;
            PreviewMouseUp += KisoThumb_PreviewMouseUpTest;

            DragStarted += KisoThumb_DragStarted2;
            DragCompleted += KisoThumb_DragCompleted2;
            DragDelta += Thumb_DragDelta3;

            KeyUp += KisoThumb_KeyUp;
            PreviewKeyDown += KisoThumb_PreviewKeyDown;

        }
        public KisoThumb(ItemData data) : this()
        {
            MyThumbType = data.MyThumbType;
        }

        private void KisoThumb_Initialized(object? sender, EventArgs e)
        {

            //デザイン画面で作成された要素の場合、ItemDataは無いので新規作成後に
            //デザイン画面の設定をItemDataに反映してからバインド設定

            //Anchorはバインド無し
            if (MyThumbType == ThumbType.Anchor)
            {
                MyItemData.MyThumbType = MyThumbType;
            }
            else if (MyItemData.MyThumbType == ThumbType.None)
            {
                MyItemData.MyThumbType = MyThumbType;
                //CopyValueToItemData();
                //MyItemDataBind();
            }
            //ファイルやItemDataから作成された要素の場合、そのままItemDataとバインド
            else
            {
                //MyItemDataBind();
            }

            //DataContext = MyItemData; //ここで適用
        }


        //XAMLの設定をItemDataにコピー
        //バインドの前に、ItemDataなしのコンストラクタで使う
        private void CopyValueToItemData()
        {
            //XAMLでの設定をItemDataに入れる
            #region 共通

            //MyItemData.MyLeft = MyLeft;
            //MyItemData.MyTop = MyTop;
            //MyItemData.MyZIndex = MyZIndex;
            //MyItemData.MyWidth = MyWidth;
            //MyItemData.MyHeight = MyHeight;

            //MyItemData.MyText = MyText;
            //MyItemData.MyFontSize = FontSize;

            #endregion 共通
            #region ブラシ

            //Color bc = ((SolidColorBrush)MyForeground).Color;
            //MyItemData.MyForegroundA = bc.A;
            //MyItemData.MyForegroundR = bc.R;
            //MyItemData.MyForegroundG = bc.G;
            //MyItemData.MyForegroundB = bc.B;

            //Color bc;
            //bc = ((SolidColorBrush)MyBackground).Color;
            //MyItemData.MyBackgroundA = bc.A;
            //MyItemData.MyBackgroundR = bc.R;
            //MyItemData.MyBackgroundG = bc.G;
            //MyItemData.MyBackgroundB = bc.B;

            //bc = ((SolidColorBrush)MyFill).Color;
            //MyItemData.MyFillA = bc.A;
            //MyItemData.MyFillR = bc.R;
            //MyItemData.MyFillG = bc.G;
            //MyItemData.MyFillB = bc.B;

            #endregion ブラシ
            #region 枠表示用

            ////いる？
            //MyItemData.IsWakuVisible = IsWakuVisible;
            //MyItemData.IsActiveGroup = IsActiveGroup;
            //MyItemData.IsFocus = IsFocus;
            //MyItemData.IsSelectable = IsSelectable;
            //MyItemData.IsSelected = IsSelected;


            #endregion 枠表示用
        }


        //バインド、ItemDataをソース
        private void MyItemDataBind()
        {
            //バインド、ItemDataをソース
            #region 共通
            //SetBinding(MyLeftProperty, nameof(MyItemData.MyLeft));
            //SetBinding(MyTopProperty, nameof(MyItemData.MyTop));
            //SetBinding(MyZIndexProperty, nameof(MyItemData.MyZIndex));
            //BindingOperations.SetBinding(this, Panel.ZIndexProperty, new Binding(nameof(MyItemData.MyZIndex)){Mode=BindingMode.TwoWay, Source = MyItemData });
            //SetBinding(MyWidthProperty, nameof(MyItemData.MyWidth));
            //SetBinding(MyHeightProperty, nameof(MyItemData.MyHeight));

            //SetBinding(MyTextProperty, nameof(MyItemData.MyText));
            //SetBinding(FontSizeProperty, nameof(MyItemData.MyFontSize));

            #endregion 共通
            #region ブラシ

            //var mb = new MultiBinding() { Converter = new MyConverterARGBtoSolidBrush() };
            //mb.Bindings.Add(new Binding(nameof(MyItemData.MyForegroundA)) { Source = MyItemData });
            //mb.Bindings.Add(new Binding(nameof(MyItemData.MyForegroundR)) { Source = MyItemData });
            //mb.Bindings.Add(new Binding(nameof(MyItemData.MyForegroundG)) { Source = MyItemData });
            //mb.Bindings.Add(new Binding(nameof(MyItemData.MyForegroundB)) { Source = MyItemData });
            //SetBinding(MyForegroundProperty, mb);

            //mb = new() { Converter = new MyConverterARGBtoSolidBrush() };
            //mb.Bindings.Add(new Binding(nameof(MyItemData.MyBackgroundA)) { Source = MyItemData });
            //mb.Bindings.Add(new Binding(nameof(MyItemData.MyBackgroundR)) { Source = MyItemData });
            //mb.Bindings.Add(new Binding(nameof(MyItemData.MyBackgroundG)) { Source = MyItemData });
            //mb.Bindings.Add(new Binding(nameof(MyItemData.MyBackgroundB)) { Source = MyItemData });
            //SetBinding(MyBackgroundProperty, mb);

            //mb = new() { Converter = new MyConverterARGBtoSolidBrush() };
            //mb.Bindings.Add(new Binding(nameof(MyItemData.MyFillA)) { Source = MyItemData });
            //mb.Bindings.Add(new Binding(nameof(MyItemData.MyFillR)) { Source = MyItemData });
            //mb.Bindings.Add(new Binding(nameof(MyItemData.MyFillG)) { Source = MyItemData });
            //mb.Bindings.Add(new Binding(nameof(MyItemData.MyFillB)) { Source = MyItemData });
            #endregion ブラシ
            #region 枠表示用
            //SetBinding(IsWakuVisibleProperty, nameof(MyItemData.IsWakuVisible));
            //SetBinding(IsActiveGroupProperty, nameof(MyItemData.IsActiveGroup));
            //SetBinding(IsFocusProperty, nameof(MyItemData.IsFocus));
            //SetBinding(IsSelectableProperty, nameof(MyItemData.IsSelectable));
            //SetBinding(IsSelectedProperty, nameof(MyItemData.IsSelected));

            #endregion 枠表示用

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
                    kiso.MyItemData.MyLeft -= amount;
                    break;
                case Key.Right:
                    kiso.MyItemData.MyLeft += amount;
                    break;
                case Key.Up:
                    kiso.MyItemData.MyTop -= amount;
                    break;
                case Key.Down:
                    kiso.MyItemData.MyTop += amount;
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
                        current.MyItemData.MyLeft = (int)(current.MyItemData.MyLeft + 0.5);
                        current.MyItemData.MyTop = (int)(current.MyItemData.MyTop + 0.5);
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
                        item.MyItemData.MyLeft += (int)(e.HorizontalChange + 0.5);
                        item.MyItemData.MyTop += (int)(e.VerticalChange + 0.5);
                        //item.MyLeft += (int)(e.HorizontalChange + 0.5);
                        //item.MyTop += (int)(e.VerticalChange + 0.5);

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


        public List<Brush> MyBrushList
        {
            get { return (List<Brush>)GetValue(MyBrushListProperty); }
            set { SetValue(MyBrushListProperty, value); }
        }
        public static readonly DependencyProperty MyBrushListProperty =
            DependencyProperty.Register(nameof(MyBrushList), typeof(List<Brush>), typeof(KisoThumb), new PropertyMetadata(null));

        #region 共通


        #endregion 共通



        #region 枠表示用


        public Visibility IsWakuVisible
        {
            get { return (Visibility)GetValue(IsWakuVisibleProperty); }
            set { SetValue(IsWakuVisibleProperty, value); }
        }
        public static readonly DependencyProperty IsWakuVisibleProperty =
            DependencyProperty.Register(nameof(IsWakuVisible), typeof(Visibility), typeof(KisoThumb), new PropertyMetadata(Visibility.Visible));


        public bool IsActiveGroup
        {
            get { return (bool)GetValue(IsActiveGroupProperty); }
            set { SetValue(IsActiveGroupProperty, value); }
        }
        public static readonly DependencyProperty IsActiveGroupProperty =
            DependencyProperty.Register(nameof(IsActiveGroup), typeof(bool), typeof(KisoThumb), new PropertyMetadata(false));




        public bool IsSelectable
        {
            get { return (bool)GetValue(IsSelectableProperty); }
            set { SetValue(IsSelectableProperty, value); }
        }
        public static readonly DependencyProperty IsSelectableProperty =
            DependencyProperty.Register(nameof(IsSelectable), typeof(bool), typeof(KisoThumb), new PropertyMetadata(false));


        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(KisoThumb), new PropertyMetadata(false));


        public bool IsFocus
        {
            get { return (bool)GetValue(IsFocusProperty); }
            set { SetValue(IsFocusProperty, value); }
        }
        public static readonly DependencyProperty IsFocusProperty =
            DependencyProperty.Register(nameof(IsFocus), typeof(bool), typeof(KisoThumb), new PropertyMetadata(false));


        #endregion 枠表示用

        #endregion 依存関係プロパティ


        #region メソッド

        #region 内部メソッド

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
                    gt.MyThumbs[i].MyItemData.MyZIndex = i;
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

        #endregion 内部メソッド

        #region public


        public void Serialize(string filePath)
        {
            DataContractSerializer serializer = new(typeof(ItemData));
            XmlWriterSettings settings = new() { Indent = true, Encoding = new UTF8Encoding(false) };
            using var writer = XmlWriter.Create(filePath, settings);
            serializer.WriteObject(writer, this.MyItemData);
        }


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
            Visibility = Visibility.Hidden;
            Focusable = false;
            DragDelta -= Thumb_DragDelta3;
            DragStarted -= KisoThumb_DragStarted2;
            DragCompleted -= KisoThumb_DragCompleted2;
        }
        public AnchorThumb(KisoThumb thumb) : this()
        {
            Width = thumb.ActualWidth;
            Height = thumb.ActualHeight;
            MyItemData = new()
            {
                MyLeft = thumb.MyItemData.MyLeft,
                MyTop = thumb.MyItemData.MyTop
            };

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
        public TextBlockThumb(ItemData data) : base(data)
        {
            MyItemData = data;
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
        public EllipseTextThumb(ItemData data) : base(data)
        {
            //MyThumbType = ThumbType.Ellipse;
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

        public ObservableCollection<KisoThumb> MyThumbs
        {
            get { return (ObservableCollection<KisoThumb>)GetValue(MyThumbsProperty); }
            set { SetValue(MyThumbsProperty, value); }
        }
        public static readonly DependencyProperty MyThumbsProperty =
            DependencyProperty.Register(nameof(MyThumbs), typeof(ObservableCollection<KisoThumb>), typeof(GroupThumb),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

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

            //ZIndexの再振り当て
            //何故かこれをしないとXAMLでのThumbのZがすべて0になる
            FixZindex();
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
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems?[0] is KisoThumb addItem)
            {
                addItem.MyParentThumb = this;
                //リストにItemDataを追加
                MyItemData.MyThumbsItemData.Insert(e.NewStartingIndex, addItem.MyItemData);

                //ZIndexをCollectionのIndexに合わせる、
                //挿入箇所より後ろの要素はすべて変更
                int index = e.NewStartingIndex;
                for (int i = index; i < MyThumbs.Count; i++)
                {
                    MyThumbs[i].MyItemData.MyZIndex = i;
                }

            }
            else if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems?[0] is KisoThumb remItem)
            {
                //リストからItemData削除
                MyItemData.MyThumbsItemData.Remove(remItem.MyItemData);

                //ZIndexをCollectionのIndexに合わせる、
                //変更対象条件は、IsSelectedではない＋削除箇所より後ろ
                int index = e.OldStartingIndex;
                for (int i = index; i < MyThumbs.Count; i++)
                {
                    if (!MyThumbs[i].IsSelected)
                    {
                        MyThumbs[i].MyItemData.MyZIndex = i;

                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Move)
            {
            }
        }

        #endregion イベントハンドラ

        #region 内部メソッド


        /// <summary>
        /// MyThumbsのZIndexを再振り当てする、Loadedで使用、専用？
        /// </summary>
        private void FixZindex()
        {
            for (int i = 0; i < MyThumbs.Count; i++)
            {
                MyThumbs[i].MyItemData.MyZIndex = i;
            }
        }

        #endregion 内部メソッド

        #region publicメソッド


        /// <summary>
        /// アンカーThumbをHiddenで追加
        /// </summary>
        public void AddAnchorThumb(KisoThumb thumb)
        {
            MyAnchorThumb = new AnchorThumb(thumb);

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
                    if (left > item.MyItemData.MyLeft) { left = item.MyItemData.MyLeft; }
                    if (top > item.MyItemData.MyTop) { top = item.MyItemData.MyTop; }
                }
            }

            if (left != MyItemData.MyLeft)
            {
                //座標変化の場合は、自身と全ての子要素の座標を変更する
                foreach (var item in MyThumbs) { item.MyItemData.MyLeft -= left; }

                //自身がroot以外なら修正
                if (MyThumbType != ThumbType.Root) { MyItemData.MyLeft += left; }
            }

            if (top != MyItemData.MyTop)
            {
                foreach (var item in MyThumbs) { item.MyItemData.MyTop -= top; }

                if (MyThumbType != ThumbType.Root) { MyItemData.MyTop += top; }
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
            KeyUp -= KisoThumb_KeyUp;
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
        //public void AddThumbToActiveGroup(KisoThumb thumb, int insertIndex = -1)
        //{
        //    thumb.IsSelectable = true;
        //    int index = insertIndex == -1 ? MyThumbs.Count : insertIndex;
        //    if (MyFocusThumb != null)
        //    {
        //        thumb.MyItemData.MyLeft += MyFocusThumb.MyItemData.MyLeft;
        //        thumb.MyItemData.MyTop += MyFocusThumb.MyItemData.MyTop;
        //        index = MyActiveGroupThumb.MyThumbs.IndexOf(MyFocusThumb) + 1;
        //    }
        //    else if (MyActiveGroupThumb.MyThumbs.Count == 0)
        //    {
        //        thumb.MyItemData.MyLeft = 0; thumb.MyItemData.MyTop = 0;
        //    }
        //    MyActiveGroupThumb.MyThumbs.Insert(index, thumb);
        //    SelectedThumbsClearAndAddThumb(thumb);
        //    ReLayout3();
        //}

        //その場に追加
        public void AddThumbToActiveGroup2(KisoThumb thumb, int insertIndex = -1)
        {
            thumb.IsSelectable = true;
            int index = insertIndex == -1 ? MyThumbs.Count : insertIndex;
            if (MyFocusThumb != null)
            {
                thumb.MyItemData.MyLeft = MyFocusThumb.MyItemData.MyLeft;
                thumb.MyItemData.MyTop = MyFocusThumb.MyItemData.MyTop;
                index = MyActiveGroupThumb.MyThumbs.IndexOf(MyFocusThumb) + 1;
            }
            else if (MyActiveGroupThumb.MyThumbs.Count == 0)
            {
                thumb.MyItemData.MyLeft = 0; thumb.MyItemData.MyTop = 0;
            }
            MyActiveGroupThumb.MyThumbs.Insert(index, thumb);
            SelectedThumbsClearAndAddThumb(thumb);
            ReLayout3();
        }
        /// <summary>
        /// オフセット位置を指定して追加
        /// </summary>
        /// <param name="thumb"></param>
        /// <param name="left">基準からの横距離</param>
        /// <param name="top">基準からの縦距離</param>
        /// <param name="insertIndex"></param>
        public void AddThumbToActiveGroup3(KisoThumb thumb, double left = 0, double top = 0)
        {
            thumb.IsSelectable = true;
            if (MyFocusThumb != null)
            {
                thumb.MyItemData.MyLeft = MyFocusThumb.MyItemData.MyLeft + left;
                thumb.MyItemData.MyTop = MyFocusThumb.MyItemData.MyTop + top;
            }
            else if (MyActiveGroupThumb.MyThumbs.Count == 0)
            {
                thumb.MyItemData.MyLeft = 0; thumb.MyItemData.MyTop = 0;
            }
            MyActiveGroupThumb.MyThumbs.Add(thumb);
            SelectedThumbsClearAndAddThumb(thumb);
            ReLayout3();
        }

        public void AddThumbToActiveGroup3(KisoThumb thumb, int insertIndex, double left = 0, double top = 0)
        {
            thumb.IsSelectable = true;
            if (MyFocusThumb != null)
            {
                thumb.MyItemData.MyLeft = MyFocusThumb.MyItemData.MyLeft + left;
                thumb.MyItemData.MyTop = MyFocusThumb.MyItemData.MyTop + top;
            }
            else if (MyActiveGroupThumb.MyThumbs.Count == 0)
            {
                thumb.MyItemData.MyLeft = 0; thumb.MyItemData.MyTop = 0;
            }
            MyActiveGroupThumb.MyThumbs.Insert(insertIndex, thumb);
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

            AddThumbToActiveGroup2(group, group.MyItemData.MyZIndex);
            //AddThumbToActiveGroup(group, group.MyItemData.MyZIndex);
        }

        /// <summary>
        /// SelectedThumbsからGroupThumbを作成
        /// GroupThumbのZIndexは選択中の最大値-(選択個数 - 1)
        /// </summary>
        /// <returns></returns>
        private GroupThumb MakeGroupFromSelectedThumbs()
        {
            List<KisoThumb> list = GetSortedSelectedThumbs();
            int minZIndex = MySelectedThumbs.Max(x => x.MyItemData.MyZIndex);
            minZIndex -= list.Count - 1;
            double minLeft = MySelectedThumbs.Min(x => x.MyItemData.MyLeft);
            double minTop = MySelectedThumbs.Min(x => x.MyItemData.MyTop);
            GroupThumb group = new();
            foreach (var item in list)
            {
                item.MyItemData.MyLeft -= minLeft;
                item.MyItemData.MyTop -= minTop;
                group.MyThumbs.Add(item);
            }
            group.IsSelectable = true;
            group.MyItemData.MyLeft = minLeft;
            group.MyItemData.MyTop = minTop;
            group.MyItemData.MyZIndex = minZIndex;
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
                    MyActiveGroupThumb.MyThumbs.Insert(item.MyItemData.MyZIndex, item);
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
                    item.MyItemData.MyLeft += group.MyItemData.MyLeft;
                    item.MyItemData.MyTop += group.MyItemData.MyTop;
                    item.MyItemData.MyZIndex += group.MyItemData.MyZIndex;
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