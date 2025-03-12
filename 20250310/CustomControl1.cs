using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
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

namespace _20250310
{



    /// <summary>
    /// 基礎Thumb、すべてのCustomControlThumbの派生元
    /// </summary>
    [DebuggerDisplay("{MyThumbType} {MyItemData.MyText}")]
    public abstract class KisoThumb : Thumb
    {
        //クリックダウンとドラッグ移動完了時に使う、直前に選択されたものかの判断用
        bool IsPreviewSelected { get; set; }


        //public ThumbType MyThumbType { get; protected set; }

        //親要素の識別用。自身がグループ化されたときに親要素のGroupThumbを入れておく
        public GroupThumb? MyParentThumb { get; set; }


        static KisoThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KisoThumb), new FrameworkPropertyMetadata(typeof(KisoThumb)));
        }
        public KisoThumb()
        {

            //MyItemData = new() { MyThumbType = ThumbType.None };
            InitializeWakuBrush();

            DataContext = MyItemData;
            Focusable = true;
            //Focusable = false;

            //MyThumbType = ThumbType.None;

            Initialized += KisoThumb_Initialized;
            Loaded += KisoThumb_Loaded;

            PreviewMouseDown += KisoThumb_PreviewMouseDownTest;
            PreviewMouseUp += KisoThumb_PreviewMouseUpTest;

            DragStarted += KisoThumb_DragStarted2;
            DragCompleted += KisoThumb_DragCompleted2;
            DragDelta += Thumb_DragDelta3;

            KeyUp += KisoThumb_KeyUp;
            PreviewKeyDown += KisoThumb_PreviewKeyDown;


        }

        private void KisoThumb_Loaded(object sender, RoutedEventArgs e)
        {
            MyBind();
        }


        private void MyBind()
        {
            if (MyInsideElement != null)
            {
                //内部表示要素のTransformBounds(回転後のサイズと位置)
                var mb = new MultiBinding() { Converter = new MyConvRenderBounds() };
                mb.Bindings.Add(new Binding() { Source = MyInsideElement, Path = new PropertyPath(ActualWidthProperty) });
                mb.Bindings.Add(new Binding() { Source = MyInsideElement, Path = new PropertyPath(ActualHeightProperty) });
                mb.Bindings.Add(new Binding() { Source = MyInsideElement, Path = new PropertyPath(RenderTransformProperty) });
                SetBinding(MyInsideElementBoundsProperty, mb);

                //内部表示要素のオフセット表示に使う
                SetBinding(MyInsideElementOffsetLeftProperty, new Binding() { Source = this, Path = new PropertyPath(MyInsideElementBoundsProperty), Converter = new MyConvRectToOffsetLeft() });
                SetBinding(MyInsideElementOffsetTopProperty, new Binding() { Source = this, Path = new PropertyPath(MyInsideElementBoundsProperty), Converter = new MyConvRectToOffsetTop() });

            }
        }


        public KisoThumb(ItemData data) : this()
        {
            //MyThumbType = data.MyThumbType;
            MyItemData = data;
            //MyItemData.PropertyChanged += MyItemData_PropertyChanged;

        }

        private void KisoThumb_Initialized(object? sender, EventArgs e)
        {
            if (MyItemData.MyThumbType == ThumbType.None)
            {
                MyItemData.MyThumbType = MyThumbType;
            }
        }

        //内部の表示要素を取得
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("element") is FrameworkElement elem)
            {
                MyInsideElement = elem;
            }
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
                Focusable = false;
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
                    Focusable = true;
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
                        if (parent.MyExCanvas is ExCanvas canvas)
                        {
                            canvas.IsAutoResize = false;
                        }
                        //parent.AddAnchorThumb(current);
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
                    //ima.MyParentThumb?.RemoveAnchorThumb();
                    if (ima.MyParentThumb?.MyExCanvas is ExCanvas canvas)
                    {
                        canvas.IsAutoResize = true;
                        //canvas.InvalidateArrange();
                    }
                }
                //再レイアウト配置
                kiso.MyParentThumb?.ReLayout3();
                e.Handled = true;
            }

        }
        #endregion マウスドラッグ移動

        #endregion イベントハンドラ

        #region 依存関係プロパティ

        //内部表示要素のオフセット表示に使う
        public double MyInsideElementOffsetTop
        {
            get { return (double)GetValue(MyInsideElementOffsetTopProperty); }
            protected set { SetValue(MyInsideElementOffsetTopProperty, value); }
        }
        public static readonly DependencyProperty MyInsideElementOffsetTopProperty =
            DependencyProperty.Register(nameof(MyInsideElementOffsetTop), typeof(double), typeof(KisoThumb), new PropertyMetadata(0.0));

        public double MyInsideElementOffsetLeft
        {
            get { return (double)GetValue(MyInsideElementOffsetLeftProperty); }
            protected set { SetValue(MyInsideElementOffsetLeftProperty, value); }
        }
        public static readonly DependencyProperty MyInsideElementOffsetLeftProperty =
            DependencyProperty.Register(nameof(MyInsideElementOffsetLeft), typeof(double), typeof(KisoThumb), new PropertyMetadata(0.0));


        //表示している要素のBounds、TextBlockThumbならTextBlock
        public Rect MyInsideElementBounds
        {
            get { return (Rect)GetValue(MyInsideElementBoundsProperty); }
            protected set { SetValue(MyInsideElementBoundsProperty, value); }
        }
        public static readonly DependencyProperty MyInsideElementBoundsProperty =
            DependencyProperty.Register(nameof(MyInsideElementBounds), typeof(Rect), typeof(KisoThumb), new PropertyMetadata(null));

        //表示している要素、TextBlockThumbならTextBlock
        public FrameworkElement MyInsideElement
        {
            get { return (FrameworkElement)GetValue(MyInsideElementProperty); }
            protected set { SetValue(MyInsideElementProperty, value); }
        }
        public static readonly DependencyProperty MyInsideElementProperty =
            DependencyProperty.Register(nameof(MyInsideElement), typeof(FrameworkElement), typeof(KisoThumb), new PropertyMetadata(null));


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


        public ThumbType MyThumbType
        {
            get { return (ThumbType)GetValue(MyThumbTypeProperty); }
            set { SetValue(MyThumbTypeProperty, value); }
        }
        public static readonly DependencyProperty MyThumbTypeProperty =
            DependencyProperty.Register(nameof(MyThumbType), typeof(ThumbType), typeof(KisoThumb), new PropertyMetadata(ThumbType.None));

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

        //public Rect GetInsideElementBounds()
        //{
        //    if (MyInsideElement is FrameworkElement elem)
        //    {
        //        var tf = elem.RenderTransform;
        //        var r = tf.TransformBounds(new Rect(0, 0, elem.ActualWidth, elem.ActualHeight));
        //        return r;
        //    }
        //    else { return Rect.Empty; }
        //}

        //public void Serialize(string filePath)
        //{
        //    DataContractSerializer serializer = new(typeof(ItemData));
        //    XmlWriterSettings settings = new() { Indent = true, Encoding = new UTF8Encoding(false) };
        //    using var writer = XmlWriter.Create(filePath, settings);
        //    serializer.WriteObject(writer, this.MyItemData);
        //}


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



    public class TextBlockThumb : KisoThumb
    {
        static TextBlockThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBlockThumb), new FrameworkPropertyMetadata(typeof(TextBlockThumb)));
        }
        //public TextBlockThumb()
        //{
        //    MyThumbType = ThumbType.Text;
        //    MyItemData.MyThumbType = ThumbType.Text;
        //}
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
        //public EllipseTextThumb()
        //{
        //    MyThumbType = ThumbType.Ellipse;
        //    MyItemData.MyThumbType = ThumbType.Ellipse;
        //}
        public EllipseTextThumb(ItemData data) : base(data)
        {
            MyItemData = data;
        }
    }





    public abstract class EzShapeThumb : KisoThumb
    {
        static EzShapeThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EzShapeThumb), new FrameworkPropertyMetadata(typeof(EzShapeThumb)));
        }
        //public EzShapeThumb()
        //{
        //    DataContext = MyItemData;
        //    DragDelta += EzShapeThumb_DragDelta;
        //    Loaded += EzShapeThumb_Loaded;
        //}
        public EzShapeThumb(ItemData data) : base()
        {
            MyItemData = data;
            //DataContext = MyItemData;
            Loaded += EzShapeThumb_Loaded;

        }

        #region 起動時

        private void EzShapeThumb_Loaded(object sender, RoutedEventArgs e)
        {
            //起動時だけ位置がずれるので応急処置
            var neko = MyItemData.MyLeft;
            var inu = MyItemData.MyTop;
            //UpdatePointsAndSizeWithoutZeroFix();
            UpdatePointsAndSizeWithTransform();
            //UpdatePointsAndSizeWithTransform();
            MyItemData.MyLeft = neko;
            MyItemData.MyTop = inu;
        }


        //起動時、Templateの中からCanvasとEzShapeを取得
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_Canvas") is Canvas panel)
            {
                MyPART_Canvas = panel;
                if (GetChildEzShape(panel) is EzShape shape)
                {
                    MyEzShape = shape;
                    //MyEzShape.MyParentShapeThumb = this;
                }
                else
                {
                    MessageBox.Show("内部表示要素の図形が見つからない");
                }
            }
        }

        private static EzShape? GetChildEzShape(FrameworkElement element)
        {
            var count = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < count; i++)
            {
                if (VisualTreeHelper.GetChild(element, i) is EzShape shape)
                {
                    return shape;
                }
            }
            return null;
        }

        #endregion 起動時

        #region 依存関係プロパティ


        //内部図形のアンカーポイント表示用のAdorner
        public EzShapeAdorner? MyEzShapeAdorner
        {
            get { return (EzShapeAdorner)GetValue(MyEzShapeAdornerProperty); }
            protected set { SetValue(MyEzShapeAdornerProperty, value); }
        }
        public static readonly DependencyProperty MyEzShapeAdornerProperty =
            DependencyProperty.Register(nameof(MyEzShapeAdorner), typeof(EzShapeAdorner), typeof(EzShapeThumb), new PropertyMetadata(null));


        //確認用なので必要ない
        public Canvas MyPART_Canvas
        {
            get { return (Canvas)GetValue(MyPART_CanvasProperty); }
            protected set { SetValue(MyPART_CanvasProperty, value); }
        }
        public static readonly DependencyProperty MyPART_CanvasProperty =
            DependencyProperty.Register(nameof(MyPART_Canvas), typeof(Canvas), typeof(EzShapeThumb), new PropertyMetadata(null));



        public EzShape MyEzShape
        {
            get { return (EzShape)GetValue(MyEzShapeProperty); }
            set { SetValue(MyEzShapeProperty, value); }
        }
        public static readonly DependencyProperty MyEzShapeProperty =
            DependencyProperty.Register(nameof(MyEzShape), typeof(EzShape), typeof(EzShapeThumb), new PropertyMetadata(null));
        #endregion 依存関係プロパティ


        #region イベントハンドラ

        private void EzShapeAnchor_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (sender is AnchorHandleThumb handle)
            {
                UpdatePointsAndSizeWithTransform();
                this.MyParentThumb?.ReLayout3();
            }

        }

        /// <summary>
        /// アンカーのドラッグ移動時の処理
        /// 対応Pointを更新、アンカーの移動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EzShapeAnchor_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t && MyEzShapeAdorner != null)
            {
                int id = (int)t.Tag;
                Point po = MyItemData.MyPoints[id];
                double left = po.X + e.HorizontalChange;
                double top = po.Y + e.VerticalChange;

                //Pointの更新
                MyItemData.MyPoints[id] = new(left, top);

                //アンカーの移動
                Canvas.SetLeft(t, left - MyEzShapeAdorner.MyAnchorHandleSize / 2.0);
                Canvas.SetTop(t, top - MyEzShapeAdorner.MyAnchorHandleSize / 2.0);

                e.Handled = true;
            }
        }
        #endregion イベントハンドラ



        #region メソッド



        public void AddPoint(Point point)
        {
            //MyEzShape.AddPoint(point);
            MyItemData.MyPoints.Add(point);
            if (MyEzShapeAdorner?.AddAnchorThumb(point) is AnchorHandleThumb anchor)
            {
                AnchorHandleThumbAddDragHandler(anchor);
            }
            UpdatePointsAndSizeWithTransform();
            MyParentThumb?.ReLayout3();
        }


        #region 使わない？
        ///// <summary>
        ///// 再描画、不完全
        ///// </summary>
        //public void Relayout()
        //{
        //    if (MyEzShape != null)
        //    {
        //        var myrect = GetBoundsFromPointsAndAnchorThumb();
        //        var r4 = MyEzShape.MyBounds4;
        //        //自身のサイズを変更
        //        this.Width = r4.Width;
        //        this.Height = r4.Height;

        //        //変更する前の位置を使って計算しておく、タイミング重要
        //        double tasLeft = Canvas.GetLeft(MyEzShape) + Canvas.GetLeft(this) + r4.Left;
        //        double tasTop = Canvas.GetTop(MyEzShape) + Canvas.GetTop(this) + r4.Top;

        //        //図形の位置を変更、オフセット
        //        Canvas.SetLeft(MyEzShape, -r4.Left);
        //        Canvas.SetTop(MyEzShape, -r4.Top);

        //        //自身の位置を変更、図形の位置に合わせる
        //        Canvas.SetLeft(this, tasLeft);
        //        Canvas.SetTop(this, tasTop);
        //    }
        //}

        //必要ないかも、UpdatePointsAndSizeWithTransformへ移行
        /// <summary>
        ///頂点移動後に実行
        ///Thumbのサイズと位置を更新する、アンカーポイント表示の有無で変化する
        ///処理の順番は
        ///MyPointsのBoundsが0,0になるように全体を移動、
        ///アンカーポイントも移動、
        ///Layout更新、
        ///Thumbサイズ更新、
        ///内部図形の移動、
        ///Thumbの移動 
        /// </summary>
        public void UpdatePointAndSize()
        {
            var (left, top) = GetTopLeftFromPoints(MyItemData.MyPoints);
            var topLeft = new Point(left, top);
            FixPointsZero(left, top);// PointsのゼロFix移動
            MyEzShapeAdorner?.ResetAnchorLocate();// AdornerをPointsの表示位置に合わせる
            UpdateLayout();// 要る？→必要            

            var pointsRect = GetBoundsFromAnchorThumb();
            //var r4 = MyEzShape.MyBounds4;//確認用
            var unionR = MyEzShape.MyBounds4;
            if (MyEzShapeAdorner != null)
            {
                unionR.Union(pointsRect);
            }
            Width = unionR.Width;
            Height = unionR.Height;
            //内部図形の位置の変更する前に今の位置を取得しておく
            var ll = Canvas.GetLeft(MyEzShape) + unionR.Left + left;
            var tt = Canvas.GetTop(MyEzShape) + unionR.Top + top;
            ll += Canvas.GetLeft(this);
            tt += Canvas.GetTop(this);

            SetLocate(MyEzShape, -unionR.Left, -unionR.Top);
            SetLocate(this, ll, tt);
        }

        //MyPointsのゼロFixなしでのサイズと位置更新、不完全
        public void UpdatePointsAndSizeWithoutZeroFix()
        {
            var pointsRect = GetBoundsFromAnchorThumb();
            var r4 = MyEzShape.MyBounds4;
            var unionR = new Rect();
            unionR.Union(pointsRect);
            unionR.Union(r4);
            Width = unionR.Width;
            Height = unionR.Height;
            //内部図形の位置の変更する前に今の位置を取得しておく
            var ll = Canvas.GetLeft(MyEzShape) + unionR.Left;
            var tt = Canvas.GetTop(MyEzShape) + unionR.Top;
            ll += Canvas.GetLeft(this);
            tt += Canvas.GetTop(this);

            SetLocate(MyEzShape, -unionR.Left, -unionR.Top);
            SetLocate(this, ll, tt);
        }

        #endregion 使わない？

        /// <summary>
        /// 回転対応！！！！！！！！！！！！！！！！！！
        /// </summary>
        public void UpdatePointsAndSizeWithTransform()
        {
            var (left, top) = GetTopLeftFromPoints(MyItemData.MyPoints);
            var topLeft = new Point(left, top);
            Point rotatePoint = MyEzShape.RenderTransform.Transform(topLeft);
            FixPointsZero(left, top);// PointsのゼロFix移動
            MyEzShapeAdorner?.ResetAnchorLocate();// AdornerをPointsの表示位置に合わせる
            UpdateLayout();// 要る？→必要            

            //図形だけのRect取得
            Rect unionR = MyEzShape.MyBounds4;
            //アンカーハンドルが表示されている場合
            if (MyEzShapeAdorner != null)
            {
                //アンカーハンドルThumbすべてが収まるRect取得して、図形だけのRectと合成(union)
                //unionR.Union(MyEzShape.MyAnchorHandleThumbsBounds);
                //unionR.Union(MyEzShapeAdorner.GetAnchorHandleThumbBounds());
                //unionR.Union(GetBoundsFromAnchorThumb());
                unionR.Union(GetBoundsFromAnchorThumbRotate(MyEzShape.RenderTransform, MyEzShape.MyPoints, MyEzShapeAdorner.MyAnchorHandleSize));

            }
            //自身のサイズ変更
            Width = unionR.Width;
            Height = unionR.Height;
            //自身の移動先を計算は、内部図形の位置の変更する前に行う
            var goLeft = Canvas.GetLeft(MyEzShape) + unionR.Left + rotatePoint.X;
            var goTop = Canvas.GetTop(MyEzShape) + unionR.Top + rotatePoint.Y;
            goLeft += MyItemData.MyLeft;
            goTop += MyItemData.MyTop;

            //図形と自身(ShapeThumb)の位置を変更
            SetLocate(MyEzShape, -unionR.Left, -unionR.Top);
            MyItemData.MyLeft = goLeft;
            MyItemData.MyTop = goTop;
        }
        //public void UpdatePointsAndSizeWithTransformTest()
        //{
        //    var (left, top) = GetTopLeftFromPoints(MyItemData.MyPoints);
        //    var topLeft = new Point(left, top);
        //    Point rotatePoint = MyEzShape.RenderTransform.Transform(topLeft);
        //    FixPointsZero(left, top);// PointsのゼロFix移動
        //    MyEzShape.MyAnchorHandleAdorner?.ResetAnchorLocate(); // AdornerをPointsの表示位置に合わせる
        //    UpdateLayout();// 要る？→必要            

        //    //図形だけのRect取得
        //    Rect unionR = MyEzShape.MyBounds4;
        //    //アンカーハンドルが表示されている場合
        //    if(MyEzShape.MyAnchorHandleAdorner !=null)
        //    //if (MyEzShapeAdorner != null)
        //    {
        //        var neko = MyEzShape.MyAnchorHandleThumbsBounds;
        //        var inu = MyEzShape.MyAnchorHandleAdorner.GetAnchorHandleThumbBounds();
        //        //アンカーハンドルThumbすべてが収まるRect取得して、図形だけのRectと合成(union)
        //        unionR.Union(MyEzShape.MyAnchorHandleThumbsBounds);
        //        //unionR.Union(MyEzShapeAdorner.GetAnchorHandleThumbBounds());
        //    }
        //    //自身のサイズ変更
        //    Width = unionR.Width;
        //    Height = unionR.Height;
        //    //自身の移動先を計算は、内部図形の位置の変更する前に行う
        //    var goLeft = Canvas.GetLeft(MyEzShape) + unionR.Left + rotatePoint.X;
        //    var goTop = Canvas.GetTop(MyEzShape) + unionR.Top + rotatePoint.Y;
        //    goLeft += MyItemData.MyLeft;
        //    goTop += MyItemData.MyTop;

        //    //図形と自身(ShapeThumb)の位置を変更
        //    SetLocate(MyEzShape, -unionR.Left, -unionR.Top);
        //    MyItemData.MyLeft = goLeft;
        //    MyItemData.MyTop = goTop;
        //}


        private void SetLocate(FrameworkElement element, double left, double top)
        {
            Canvas.SetLeft(element, left);
            Canvas.SetTop(element, top);
        }

        /// <summary>
        /// アンカーハンドルの表示切替
        /// </summary>
        public void AnchorOnOffSwitch()
        {
            //レイヤー取得
            if (AdornerLayer.GetAdornerLayer(MyEzShape) is AdornerLayer layer)
            {
                //無ければ追加(表示)
                if (MyEzShapeAdorner is null)
                {
                    EzShapeAdorner adorner = new(MyEzShape);
                    layer.Add(adorner);
                    MyEzShapeAdorner = adorner;
                    UpdatePointsAndSizeWithTransform();

                    foreach (var item in MyEzShapeAdorner.MyAnchorHandleThumbsList)
                    {
                        AnchorHandleThumbAddDragHandler(item);
                    }
                }
                //在れば削除
                else
                {
                    layer.Remove(MyEzShapeAdorner);
                    MyEzShapeAdorner = null;
                    UpdatePointsAndSizeWithTransform();
                }
            }
        }

        //アンカーハンドルにドラッグイベントハンドラー追加
        protected void AnchorHandleThumbAddDragHandler(AnchorHandleThumb item)
        {
            item.DragDelta += EzShapeAnchor_DragDelta;
            item.DragCompleted += EzShapeAnchor_DragCompleted;
        }


        /// <summary>
        /// Points全体のリセット、左上に寄せる、TopLeftを0にする
        /// </summary>
        public void FixPointsZero(PointCollection points)
        {
            var (left, top) = GetTopLeftFromPoints(points);
            for (int i = 0; i < points.Count; i++)
            {
                Point p = points[i];
                points[i] = new Point(p.X - left, p.Y - top);
            }
        }
        public void FixPointsZero(double offsetX, double offsetY)
        {
            for (int i = 0; i < MyItemData.MyPoints.Count; i++)
            {
                Point p = MyItemData.MyPoints[i];
                MyItemData.MyPoints[i] = new Point(p.X - offsetX, p.Y - offsetY);
            }
        }

        private (double left, double top) GetTopLeftFromPoints(PointCollection points)
        {
            double left = double.MaxValue;
            double top = double.MaxValue;
            foreach (var item in points)
            {
                if (left > item.X) { left = item.X; }
                if (top > item.Y) { top = item.Y; }
            }
            return (left, top);
        }


        /// <summary>
        /// 全部のアンカーポイントが収まるRectを返す
        /// アンカーポイントが表示されていなければ0を返す
        /// </summary>
        /// <returns></returns>
        private Rect GetBoundsFromAnchorThumb()
        {
            if (MyEzShapeAdorner != null)
            {
                double anchorSize = MyEzShapeAdorner.MyAnchorHandleSize;
                double anchorSizeHalf = anchorSize / 2.0;//アンカーポイントの中心位置
                Rect r = new();
                foreach (var item in MyItemData.MyPoints)
                {
                    Rect pr = new(item.X - anchorSizeHalf, item.Y - anchorSizeHalf, anchorSize, anchorSize);
                    r.Union(pr);
                }
                return r;
            }
            else
            {
                return new Rect();
            }
        }

        /// <summary>
        /// すべてのアンカーハンドルThumbを含んだ回転後(Transform)のRectを返す
        /// けど、ハンドル自体は回転しないで計算しているので多少の誤差がある
        /// </summary>
        /// <param name="transform">RenderTransform</param>
        /// <param name="points"></param>
        /// <param name="handleSize">アンカーハンドルThumbのサイズ</param>
        /// <returns></returns>
        private Rect GetBoundsFromAnchorThumbRotate(Transform transform, PointCollection points, double handleSize)
        {
            //Pointsを変形
            PointCollection tempPc = [];
            foreach (var item in points)
            {
                tempPc.Add(transform.Transform(item));
            }

            //各アンカーハンドルのRectを作成して
            //RectのUnionメソッドを利用すれば、
            //すべてのアンカーハンドルが収まるRectが作成できる
            double halfHandle = handleSize / 2.0;//アンカーポイントの中心位置
            Point p = tempPc[0];
            Rect r = new(p.X - halfHandle, p.Y - halfHandle, handleSize, handleSize);
            foreach (var item in tempPc)
            {
                Rect pr = new(item.X - halfHandle, item.Y - halfHandle, handleSize, handleSize);
                r.Union(pr);
            }
            return r;
        }



        #endregion メソッド

    }


    public class EzBezierThumb : EzShapeThumb
    {
        static EzBezierThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EzBezierThumb), new FrameworkPropertyMetadata(typeof(EzBezierThumb)));
        }

        public EzBezierThumb(ItemData data) : base(data)
        {
            
            //Loaded += EzBezierThumb_Loaded;
        }

        //private void EzBezierThumb_Loaded(object sender, RoutedEventArgs e)
        //{
        //    //UpdatePointsAndSizeWithTransform();
        //}

    }

    //public class EzBezierThumbTest : EzBezierThumb
    //{
    //    static EzBezierThumbTest()
    //    {
    //        DefaultStyleKeyProperty.OverrideMetadata(typeof(EzBezierThumbTest), new FrameworkPropertyMetadata(typeof(EzBezierThumbTest)));
    //    }
    //    public EzBezierThumbTest(ItemData data) : base(data)
    //    {

    //    }
    //}





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
            DependencyProperty.Register(nameof(MyThumbs), typeof(ObservableCollection<KisoThumb>), typeof(GroupThumb),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 依存関係プロパティ

        //子要素移動時にスクロールバー固定用のアンカー
        //public AnchorThumb? MyAnchorThumb { get; private set; }
        public ExCanvas? MyExCanvas { get; private set; }
        #region コンストラクタ

        static GroupThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GroupThumb), new FrameworkPropertyMetadata(typeof(GroupThumb)));
        }
        //public GroupThumb() { }

        public GroupThumb(ItemData data)
        {
            MyItemData = data;
            MyThumbs = [];
            Loaded += GroupThumb_Loaded;
            MyThumbs.CollectionChanged += MyThumbs_CollectionChanged;

            List<KisoThumb> thumbList = [];
            foreach (ItemData item in data.MyThumbsItemData)
            {
                var thumb = MyBuilder.MakeThumb(item);
                if (thumb != null)
                {
                    thumbList.Add(thumb);
                }
            }
            foreach (var item in thumbList)
            {
                MyThumbs.Add(item);
            }
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
                MyExCanvas = GetExCanvas(ic);
                if (MyExCanvas != null)
                {
                    _ = SetBinding(WidthProperty, new Binding() { Source = MyExCanvas, Path = new PropertyPath(ActualWidthProperty) });
                    _ = SetBinding(HeightProperty, new Binding() { Source = MyExCanvas, Path = new PropertyPath(ActualHeightProperty) });
                }
            }

            //ZIndexの再振り当て
            //何故かこれをしないとXAMLでのThumbのZがすべて0になる
            FixForXamlItemThumbs();

            //BindingOperations.SetBinding(MyItemData, ItemData.MyThumbsItemData2Property, new Binding() { Source = this, Path = new PropertyPath(MyThumbsProperty) ,Converter = new MyConverterItemData()});
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

        private void AddItemsData()
        {
            for (int i = 0; i < MyThumbs.Count; i++)
            {
                MyItemData.MyThumbsItemData.Add(MyThumbs[i].MyItemData);
            }
        }
        /// <summary>
        /// 子要素の追加時
        /// 子要素に親要素(自身)を登録
        /// </summary>
        private void MyThumbs_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems?[0] is KisoThumb addThumb)
            {
                addThumb.MyParentThumb = this;
                //リストにItemDataを追加と、枠表示を親とのバインド
                if (addThumb.MyItemData.MyThumbType != ThumbType.None)
                {
                    MyItemData.MyThumbsItemData.Insert(e.NewStartingIndex, addThumb.MyItemData);
                    addThumb.SetBinding(IsWakuVisibleProperty, new Binding() { Source = this, Path = new PropertyPath(IsWakuVisibleProperty) });
                }

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
                if (!MyItemData.MyThumbsItemData.Remove(remItem.MyItemData))
                {
                    throw new ArgumentException("ItemDataの削除でエラー");
                }

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
            //Clear全削除
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                //ItemData全削除
                MyItemData.MyThumbsItemData.Clear();
            }
        }

        #endregion イベントハンドラ

        #region 内部メソッド


        /// <summary>
        /// デザイン画面で追加したThumbがある場合に起動直後で使用する
        /// MyThumbsのZIndexを再振り当てする、Loadedで使用、専用？
        /// </summary>
        private void FixForXamlItemThumbs()
        {
            var datas = MyItemData.MyThumbsItemData;
            MyItemData.MyThumbsItemData.Clear();
            for (int i = 0; i < MyThumbs.Count; i++)
            {
                var data = MyThumbs[i].MyItemData;
                data.MyZIndex = i;
                MyItemData.MyThumbsItemData.Add(data);
            }
            var datas2 = MyItemData.MyThumbsItemData;
        }

        #endregion 内部メソッド

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


                if (left > item.MyItemData.MyLeft) { left = item.MyItemData.MyLeft; }
                if (top > item.MyItemData.MyTop) { top = item.MyItemData.MyTop; }

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
        //public RootThumb() { }
        public RootThumb(ItemData data) : base(data)
        {
            Focusable = true;
            //MyThumbType = ThumbType.Root;
            //MyItemData.MyThumbType = ThumbType.Root;
            MySelectedThumbs = [];
            DragDelta -= Thumb_DragDelta3;
            DragStarted -= KisoThumb_DragStarted2;
            DragCompleted -= KisoThumb_DragCompleted2;
            KeyUp -= KisoThumb_KeyUp;
            Loaded += RootThumb_Loaded;
        }
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
            if (kiso is null)
            {
                MyFocusThumb = null;
                return;
            }
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
        /// <param name="newGroupThumb">指定GroupThumb</param>
        private bool ChangeActiveGroupThumb(GroupThumb newGroupThumb)
        {
            if (MyActiveGroupThumb != newGroupThumb)
            {
                MyActiveGroupThumb = newGroupThumb;
                return true;
            }
            return false;
        }

        #endregion ActiveGroupThumbの変更

        #region Thumb追加と削除

        #region 追加

        public void AddNewThumbFromItemData(ItemData data, GroupThumb parent)
        {
            if (MyBuilder.MakeThumb(data) is KisoThumb thumb)
            {
                if (parent == MyActiveGroupThumb)
                {
                    AddThumbToActiveGroup(thumb, parent);
                }
                else
                {
                    AddThumb(thumb, parent);
                }
            }
        }
        public void AddThumb(KisoThumb thumb, GroupThumb parent)
        {
            if (MyFocusThumb is null)
            {
                parent.MyThumbs.Add(thumb);

            }
            else
            {
                thumb.MyItemData.MyLeft += MyFocusThumb.MyItemData.MyLeft + MyItemData.MyAddOffsetLeft;
                thumb.MyItemData.MyTop += MyFocusThumb.MyItemData.MyTop + MyItemData.MyAddOffsetTop;
                parent.MyThumbs.Add(thumb);
            }

            MySelectedThumbs.Clear();
            AddToSelectedThumbs(thumb);
        }
        public void AddThumbToActiveGroup(KisoThumb thumb, GroupThumb parent)
        {
            AddThumb(thumb, parent);
            thumb.IsSelectable = true;
        }




        /// <summary>
        /// ActiveGroupThumbにThumbを追加、オフセット位置を指定して追加
        /// 追加場所はFocusThumbがあればそれが基準になる、Z座標は一番上
        /// 最初の追加要素ならすべて0で配置
        /// </summary>
        /// <param name="thumb"></param>
        /// <param name="left">基準からの横距離</param>
        /// <param name="top">基準からの縦距離</param>
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

        /// <summary>
        /// ActiveGroupThumbにThumbを追加、オフセット位置とZ座標を指定して追加
        /// 追加場所はFocusThumbがあればそれが基準になる
        /// 最初の追加要素ならすべて0で配置
        /// </summary>
        /// <param name="thumb"></param>
        /// <param name="insertIndex">挿入先指定</param>
        /// <param name="left">基準からの横距離</param>
        /// <param name="top">基準からの縦距離</param>
        public void AddThumbInsertToActiveGroup(KisoThumb thumb, int insertIndex, double left = 0, double top = 0)
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

        #endregion 追加


        #region 削除
        /// <summary>
        /// 次のFocusThumb候補を取得
        /// 優先順に
        /// 今のFocusThumbの一段下層の要素
        /// 今のFocusThumbが最下層なら一段上層の要素
        /// それもなければnull
        /// </summary>
        /// <param name="nowForcusThumb"></param>
        /// <returns></returns>
        private KisoThumb? GetNextFocusThumb(KisoThumb nowForcusThumb)
        {
            if (nowForcusThumb.MyParentThumb is GroupThumb parent)
            {
                int nowIndex = nowForcusThumb.MyItemData.MyZIndex;
                if (parent.MyThumbs.Count == 1)
                {
                    return null;
                }
                else if (nowIndex == 0)
                {
                    return parent.MyThumbs[1];
                }
                else
                {
                    return parent.MyThumbs[nowIndex - 1];
                }
            }
            else { return null; }
        }

        /// <summary>
        /// SelectedThumbsをすべて削除、
        /// 削除処理の基本はこれを使う
        /// </summary>
        public void RemoveThumb()
        {
            if (MySelectedThumbs.Count == 0) { return; }

            if (MyActiveGroupThumb.MyThumbs.Count == MySelectedThumbs.Count)
            {
                RemoveAll();
            }
            else
            {
                //ActiveGroupThumbから削除
                foreach (var item in MySelectedThumbs)
                {
                    MyActiveGroupThumb.MyThumbs.Remove(item);
                }

                //削除後の処理
                //削除結果残った要素数が1ならグループにしておく必要がないのでグループ解除する。
                //最後の要素は1個上のグループに移動させるので位置調整、
                //ActiveGroupを削除、
                //ActiveGroupのMyThumbsをクリア、
                //ActiveGroupを変更、
                //残った1個をそこに追加(挿入)、
                //FocusThumbとSelectedThumbsを調整
                if (MyActiveGroupThumb.MyThumbs.Count == 1)
                {
                    if (MyActiveGroupThumb.MyParentThumb is GroupThumb parent)
                    {
                        var lastOne = MyActiveGroupThumb.MyThumbs[0];
                        lastOne.MyItemData.MyLeft += MyActiveGroupThumb.MyItemData.MyLeft;
                        lastOne.MyItemData.MyTop += MyActiveGroupThumb.MyItemData.MyTop;
                        lastOne.MyItemData.MyZIndex += MyActiveGroupThumb.MyItemData.MyZIndex;

                        parent.MyThumbs.Remove(MyActiveGroupThumb);
                        MyActiveGroupThumb.MyThumbs.Clear();
                        MyActiveGroupThumb = parent;
                        MyActiveGroupThumb.MyThumbs.Insert(lastOne.MyItemData.MyZIndex, lastOne);

                        SelectedThumbsClearAndAddThumb(lastOne);
                    }
                }
                //グループ維持の場合は、
                //FocusThumbとSelectedThumbsを調整、
                //FocusThumbはSelectedThumbsの下層のThumbにする、無ければ上層
                else
                {
                    int nextIndex = MySelectedThumbs.Min(x => x.MyItemData.MyZIndex);
                    if (nextIndex > 0) { nextIndex--; }
                    SelectedThumbsClearAndAddThumb(MyActiveGroupThumb.MyThumbs[nextIndex]);
                }
                MyActiveGroupThumb.ReLayout3();
            }
        }

        //基本的には使わない、
        //FocusThumb以外のThumbを削除するとき用
        public void RemoveThumb(KisoThumb? thumb, bool withRelayout = true)
        {
            //ParentがRootの場合
            if (thumb?.MyParentThumb is RootThumb root)
            {
                int itemCount = root.MyThumbs.Count;
                if (itemCount == 1)
                {
                    //全削除と同じ処理
                    RemoveAll();
                }
                else if (itemCount >= 2)
                {
                    //Clickedと次のFocusThumbを設定してから削除
                    if (MyClickedThumb == thumb)
                    {
                        MyClickedThumb = null;
                    }
                    if (thumb.IsFocus)
                    {
                        MyFocusThumb = GetNextFocusThumb(thumb);
                    }
                    root.MyThumbs.Remove(thumb);
                    if (withRelayout) { ReLayout3(); }
                }
            }
            //ParentがGroupの場合
            else if (thumb?.MyParentThumb is GroupThumb parentGroup)
            {
                int itemCount = parentGroup.MyThumbs.Count;
                if (itemCount == 1)
                {
                    //Parentグループ自体を削除
                    RemoveThumb(parentGroup, withRelayout);
                }
                //グループ解除が伴う
                else if (itemCount == 2)
                {
                    //削除してからグループ解除
                    //Clickedと次のFocusThumbを設定してから削除
                    if (MyClickedThumb == thumb)
                    {
                        MyClickedThumb = null;
                    }
                    if (thumb.IsFocus)
                    {
                        MyFocusThumb = GetNextFocusThumb(thumb);
                    }
                    parentGroup.MyThumbs.Remove(thumb);
                    //Parentグループ解除
                    Ungroup(parentGroup);

                    //グループ解除してから削除
                }
                else
                {
                    //普通に削除
                    //Clickedと次のFocusThumbを設定してから削除
                    if (MyClickedThumb == thumb)
                    {
                        MyClickedThumb = null;
                    }
                    if (thumb.IsFocus)
                    {
                        MyFocusThumb = GetNextFocusThumb(thumb);
                    }
                    parentGroup.MyThumbs.Remove(thumb);
                    if (withRelayout) { parentGroup.ReLayout3(); }
                }
                //return parentGroup.MyThumbs.Remove(thumb);
            }
            //else { return false; }
        }



        //public void RemoveSelectedThumbs2()
        //{
        //    if (MySelectedThumbs.Count == 0) { return; }

        //    //ActiveGroupから選択Thumbを削除
        //    foreach (var item in MySelectedThumbs)
        //    {
        //        MyActiveGroupThumb.MyThumbs.Remove(item);
        //    }

        //    //残った要素が1個だけならGroupを解除
        //    if (MyActiveGroupThumb.MyThumbs.Count == 1)
        //    {
        //        if (MyActiveGroupThumb.MyParentThumb is GroupThumb newGroup)
        //        {
        //            ChangeActiveGroupThumb(newGroup);
        //        }

        //        MyFocusThumb = MyActiveGroupThumb;
        //        UngroupFocusThumb();
        //        return;
        //    }

        //    //新たなFocusThumbの選定
        //    //基本は削除群の最小Index-1が対象、これが無ければ最小Index
        //    int index = MySelectedThumbs.Min(x => x.MyItemData.MyZIndex);
        //    MySelectedThumbs.Clear();
        //    KisoThumb? nextForcusThumb;
        //    if (index > 0)
        //    {
        //        nextForcusThumb = MyActiveGroupThumb.MyThumbs[index - 1];
        //    }
        //    else if (MyActiveGroupThumb.MyThumbs.Count > 0)
        //    {
        //        nextForcusThumb = MyActiveGroupThumb.MyThumbs[index];
        //    }
        //    else { nextForcusThumb = null; }
        //    SelectedThumbsClearAndAddThumb(nextForcusThumb);

        //    MyClickedThumb = null;
        //}

        ///// <summary>
        ///// SelectedThumbsの全要素をActiveGroupThumbから削除
        ///// </summary>
        ///// <param name="withReLayout">削除後に再配置処理をするならtrue</param>
        //public void RemoveSelectedThumbsFromActiveGroup(bool withReLayout = true)
        //{
        //    int selectedCount = MySelectedThumbs.Count;
        //    if (selectedCount == 0) { return; }
        //    int targetCount = MyActiveGroupThumb.MyThumbs.Count;

        //    if (IsSelectedWithParent(MyClickedThumb)) { MyClickedThumb = null; }
        //    MyFocusThumb = null;

        //    //全削除
        //    if (selectedCount == targetCount)
        //    {
        //        RemoveAll();
        //    }

        //    foreach (var item in MySelectedThumbs)
        //    {
        //        item.IsSelectable = false;
        //        MyActiveGroupThumb.MyThumbs.Remove(item);
        //    }
        //    if (withReLayout) { ReLayout3(); }
        //}

        public void RemoveSelectedThumbsFromActiveGroup2(bool withReLayout = true)
        {

            if (MySelectedThumbs.Count == 0) { return; }
            //int targetCount = MyActiveGroupThumb.MyThumbs.Count;

            if (IsSelectedWithParent(MyClickedThumb)) { MyClickedThumb = null; }
            MyFocusThumb = null;

            ////全削除
            //if (selectedCount == targetCount)
            //{
            //    RemoveAll();
            //}

            foreach (var item in MySelectedThumbs)
            {
                item.IsSelectable = false;
                MyActiveGroupThumb.MyThumbs.Remove(item);
            }
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

        #endregion 削除

        #region グループ化

        /// <summary>
        /// SelectedThumbsからGroupThumbを生成、ActiveThumbに追加
        /// </summary>
        public void AddGroupFromSelected()
        {
            //グループ化しない、
            //要素数が2個未満のとき、
            //すべての子要素が選択されているとき、ただしRootThumb上を除く
            if (MySelectedThumbs.Count < 2) { return; }
            if (MyActiveGroupThumb.MyThumbs.Count == MySelectedThumbs.Count &&
                this.MyThumbType != ThumbType.Root) { return; }

            //ActiveGroupから選択Thumbを削除(解除離脱)
            RemoveSelectedThumbsFromActiveGroup2(false);

            //選択Thumbを詰め込んだ新規グループ作成
            GroupThumb group = MakeGroupFromSelectedThumbs();

            //選択Thumbクリア
            MySelectedThumbs.Clear();
            group.IsSelectable = true;
            //MyFocusThumb = group;

            //ActiveGroupに新グループ追加
            AddThumbInsertToActiveGroup(group, group.MyItemData.MyZIndex);

        }

        /// <summary>
        /// SelectedThumbsからGroupThumbを作成
        /// GroupThumbのZIndexはSelectedThumbsの一番上と同じようになるようにしている
        /// </summary>
        /// <returns></returns>
        private GroupThumb MakeGroupFromSelectedThumbs()
        {
            int insertZIndex = MySelectedThumbs.Max(x => x.MyItemData.MyZIndex);
            insertZIndex -= MySelectedThumbs.Count - 1;
            double minLeft = MySelectedThumbs.Min(x => x.MyItemData.MyLeft);
            double minTop = MySelectedThumbs.Min(x => x.MyItemData.MyTop);
            ItemData data = new(ThumbType.Group)
            {
                MyLeft = minLeft,
                MyTop = minTop,
                MyZIndex = insertZIndex,
            };
            GroupThumb group = new(data);


            //選択ThumbをIndex順に並べたリスト
            List<KisoThumb> list = MySelectedThumbs.OrderBy(x => x.MyItemData.MyZIndex).Where(x => x.IsSelected).ToList();
            //Index順にMyThumbsに追加と位置合わせ
            foreach (var item in list)
            {
                item.MyItemData.MyLeft -= minLeft;
                item.MyItemData.MyTop -= minTop;
                group.MyThumbs.Add(item);
            }

            group.UpdateLayout();// 重要、これがないとサイズが合わない
            return group;
        }

        #endregion グループ化

        #region グループ解除

        //基本的には使わない、FocusThumb以外のグループ解除用
        public void Ungroup(GroupThumb ungroup)
        {
            if (ungroup is RootThumb) { return; }

            if (ungroup.MyParentThumb is GroupThumb parent)
            {
                RemoveThumb(ungroup, false);
                List<KisoThumb> children = MakeFixXYZDataCildren(ungroup);
                ungroup.MyThumbs.Clear();
                foreach (var item in children)
                {
                    parent.MyThumbs.Insert(item.MyItemData.MyZIndex, item);
                }

                if (ungroup == MyActiveGroupThumb)
                {
                    MyActiveGroupThumb = parent;
                }

                //FocusThumbの選定、Clickedが含まれていたらそれ、なければ先頭要素
                if (GetSelectableThumb(MyClickedThumb) is KisoThumb nextFocus)
                {
                    MyFocusThumb = nextFocus;
                }
                else
                {
                    MyFocusThumb = children[0];
                }
                SelectedThumbsClearAndAddThumb(MyFocusThumb);
            }

            static List<KisoThumb> MakeFixXYZDataCildren(GroupThumb group)
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


        /// <summary>
        /// グループ解除、FocusThumbが対象
        /// 解除後は元グループの要素全てを選択状態にする
        /// </summary>
        public void UngroupFocusThumb()
        {
            if (MyFocusThumb is GroupThumb group &&
                group.MyParentThumb is GroupThumb parent)
            {
                MyFocusThumb = null;
                List<KisoThumb> list = MakeUngroupList(group);
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
        #endregion グループ解除

        #endregion Thumb追加と削除

        #endregion パブリックなメソッド



        #region 依存関係プロパティ


        public ObservableCollectionKisoThumb MySelectedThumbs
        {
            get { return (ObservableCollectionKisoThumb)GetValue(MySelectedThumbsProperty); }
            set { SetValue(MySelectedThumbsProperty, value); }
        }
        public static readonly DependencyProperty MySelectedThumbsProperty =
            DependencyProperty.Register(nameof(MySelectedThumbs), typeof(ObservableCollectionKisoThumb), typeof(RootThumb), new PropertyMetadata(null));

        //public KisoThumb? MyClickedThumb
        //{
        //    get { return (KisoThumb)GetValue(MyClickedThumbProperty); }
        //    set { SetValue(MyClickedThumbProperty, value); }
        //}
        //public static readonly DependencyProperty MyClickedThumbProperty =
        //    DependencyProperty.Register(nameof(MyClickedThumb), typeof(KisoThumb), typeof(RootThumb), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnMyClickedThumbChanged)));

        //private static void OnMyClickedThumbChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    ////FocusThumbの更新
        //    //if (d is RootThumb rt)
        //    //{
        //    //    if (e.NewValue is KisoThumb n && n.IsSelectable)
        //    //    {
        //    //        rt.MyFocusThumb = n;
        //    //    }
        //    //}
        //}

        public KisoThumb? MyClickedThumb
        {
            get { return (KisoThumb?)GetValue(MyClickedThumbProperty); }
            set { SetValue(MyClickedThumbProperty, value); }
        }
        public static readonly DependencyProperty MyClickedThumbProperty =
            DependencyProperty.Register(nameof(MyClickedThumb), typeof(KisoThumb), typeof(RootThumb), new PropertyMetadata(null));

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



    public class AnchorHandleThumb : Thumb
    {
        static AnchorHandleThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnchorHandleThumb), new FrameworkPropertyMetadata(typeof(AnchorHandleThumb)));
        }
        public AnchorHandleThumb()
        {

        }

        #region 依存関係プロパティ

        public double MySize
        {
            get { return (double)GetValue(MySizeProperty); }
            set { SetValue(MySizeProperty, value); }
        }
        public static readonly DependencyProperty MySizeProperty =
            DependencyProperty.Register(nameof(MySize), typeof(double), typeof(AnchorHandleThumb), new PropertyMetadata(20.0));

        #endregion 依存関係プロパティ

    }


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


    public static class MyBuilder
    {
        public static KisoThumb? MakeThumb(ItemData data)
        {
            if (data.MyThumbType == ThumbType.Text)
            {
                return new TextBlockThumb(data);
            }
            else if (data.MyThumbType == ThumbType.Ellipse)
            {
                return new EllipseTextThumb(data);
            }
            else if (data.MyThumbType == ThumbType.Group)
            {
                return new GroupThumb(data);
            }
            else if (data.MyThumbType == ThumbType.Root)
            {
                return new RootThumb(data);
            }
            else if (data.MyThumbType == ThumbType.Bezier)
            {
                return new EzBezierThumb(data);
            }
            else { return null; }
        }

        public static KisoThumb? MakeThumb(string filePath)
        {
            if (ItemData.Deserialize(filePath) is ItemData data)
            {
                return MakeThumb(data);
            }
            else { return null; }
        }
    }


}