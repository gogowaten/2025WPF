﻿using System;
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

namespace _20250323
{


    /// <summary>
    /// リサイズ用のハンドルThumb
    /// </summary>
    public class HandleThumb : Thumb
    {
        static HandleThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HandleThumb), new FrameworkPropertyMetadata(typeof(HandleThumb)));
        }
        public HandleThumb()
        {

        }

        //Canvas.Leftとバインドする用
        public double MyLeft
        {
            get { return (double)GetValue(MyLeftProperty); }
            set { SetValue(MyLeftProperty, value); }
        }
        public static readonly DependencyProperty MyLeftProperty =
            DependencyProperty.Register(nameof(MyLeft), typeof(double), typeof(HandleThumb),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MyTop
        {
            get { return (double)GetValue(MyTopProperty); }
            set { SetValue(MyTopProperty, value); }
        }
        public static readonly DependencyProperty MyTopProperty =
            DependencyProperty.Register(nameof(MyTop), typeof(double), typeof(HandleThumb),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    }





    /// <summary>
    /// アンカーハンドルの表示切替とドラッグ移動できるGeoShape
    /// </summary>
    public class GeoShapeThumb : Thumb
    {
        #region フィールド

        //アンカーハンドルを表示する装飾
        public AnchorHandleAdorner? MyShapesAnchorHandleAdorner { get; private set; }

        //アンカーハンドルを表示する装飾のLayer
        public AdornerLayer MyShapesAdornerLayer { get; private set; } = null!;

        //中に表示している図形
        private GeoShape MyGeoShape { get; set; } = null!;
        #endregion フィールド

        //イベント
        //アンカーハンドル移動終了時にそれを知らせる用
        public event Action<DragCompletedEventArgs>? OnAnchorHandleDragComleted;


        static GeoShapeThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GeoShapeThumb), new FrameworkPropertyMetadata(typeof(GeoShapeThumb)));
        }
        public GeoShapeThumb()
        {
            DragDelta += GeoShapeThumb_DragDelta;
        }

        //起動時、Templateの中から図形を取り出してバインド設定
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("shape") is GeoShape shape)
            {
                MyGeoShape = shape;
                MyShapesAdornerLayer = AdornerLayer.GetAdornerLayer(MyGeoShape);

                //MyPointsのバインド、自身のMyPointsが
                //nullなら図形のMyPointsをソースにする、
                //nullじゃなければ自身のMyPointsをソースにする
                if (MyPoints != null)
                {
                    MyGeoShape.SetBinding(GeoShape.MyPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty), Mode = BindingMode.TwoWay });
                }
                else
                {
                    SetBinding(MyPointsProperty, new Binding() { Source = MyGeoShape, Path = new PropertyPath(GeoShape.MyPointsProperty), Mode = BindingMode.TwoWay });
                }
            }
            else
            {
                throw new ArgumentNullException("Templateの中に図形が見つからない");
            }
        }



        //自身のドラッグ移動
        private void GeoShapeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            //アンカーハンドルが表示されているときだけ移動
            if (MyShapesAnchorHandleAdorner != null)
            {
                MyLeft += e.HorizontalChange;
                MyTop += e.VerticalChange;
                e.Handled = true;
            }
        }

        #region 依存関係プロパティ
        #region 図形とのバインド用


        public double MyAngle
        {
            get { return (double)GetValue(MyAngleProperty); }
            set { SetValue(MyAngleProperty, value); }
        }
        public static readonly DependencyProperty MyAngleProperty =
            DependencyProperty.Register(nameof(MyAngle), typeof(double), typeof(GeoShapeThumb), new PropertyMetadata(0.0));

        public ShapeType MyShapeType
        {
            get { return (ShapeType)GetValue(MyShapeTypeProperty); }
            set { SetValue(MyShapeTypeProperty, value); }
        }
        public static readonly DependencyProperty MyShapeTypeProperty =
            DependencyProperty.Register(nameof(MyShapeType), typeof(ShapeType), typeof(GeoShapeThumb),
                new FrameworkPropertyMetadata(ShapeType.Line,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(GeoShapeThumb), new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure |
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public double MyStrokeThickness
        {
            get { return (double)GetValue(MyStrokeThicknessProperty); }
            set { SetValue(MyStrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeThicknessProperty =
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(GeoShapeThumb),
                new FrameworkPropertyMetadata(10.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion 図形とのバインド用


        public double MyLeft
        {
            get { return (double)GetValue(MyLeftProperty); }
            set { SetValue(MyLeftProperty, value); }
        }
        public static readonly DependencyProperty MyLeftProperty =
            DependencyProperty.Register(nameof(MyLeft), typeof(double), typeof(GeoShapeThumb),
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
            DependencyProperty.Register(nameof(MyTop), typeof(double), typeof(GeoShapeThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion 依存関係プロパティ


        #region メソッド

        /// <summary>
        /// 図形が収まるRectの取得
        /// </summary>
        /// <returns></returns>
        public Rect GetShapeRenderBounds()
        {
            ////RenderTransformが変更されていることを考慮して、更新してから取得
            //MyGeoShape.UpdateRenderBounds();
            //return MyGeoShape.MyRenderBounds;
            return MyGeoShape.GetRenderBounds();
        }




        /// <summary>
        /// 直線とベジェ曲線の切り替え
        /// </summary>
        public void ChangeShapeType()
        {
            if (MyShapeType == ShapeType.Line) { ChangeToBezier(); }
            else { ChangeToLine(); }
        }
        public void ChangeToLine()
        {
            if (MyShapeType == ShapeType.Bezier)
            {
                MyShapeType = ShapeType.Line;
                MyShapesAnchorHandleAdorner?.RemoveControlLine();
                MyGeoShape.MyShapeType = ShapeType.Line;
            }
        }
        public void ChangeToBezier()
        {
            if (MyShapeType == ShapeType.Line)
            {
                MyShapeType = ShapeType.Bezier;
                MyShapesAnchorHandleAdorner?.AddControlLine();
                MyGeoShape.MyShapeType = ShapeType.Bezier;
            }
        }

        #region Pointの追加と削除
        //ItemDataのMyPointsは操作しないで、ShapeThumbのメソッドを利用する

        /// <summary>
        /// Pointの追加
        /// </summary>
        /// <param name="index">挿入箇所Index</param>
        /// <param name="poi">Point</param>
        public void AddPoint(int index, Point poi)
        {
            MyPoints.Insert(index, poi);
            //図形に装飾が在れば、装飾の更新
            MyShapesAnchorHandleAdorner?.AddAnchorHandleThumb(index, poi);
        }

        /// <summary>
        /// Pointを末尾に追加
        /// </summary>
        /// <param name="poi"></param>
        public void AddPoint(Point poi)
        {
            int id = MyPoints.Count;
            AddPoint(id, poi);
        }

        /// <summary>
        /// Pointの削除
        /// </summary>
        /// <param name="index"></param>
        public void RemovePoint(int index)
        {
            //最低2個は残して削除処理
            if (MyPoints.Count > 2)
            {
                MyPoints.RemoveAt(index);
                MyShapesAnchorHandleAdorner?.RemoveAnchorHandleThumb(index);
            }
        }

        #endregion Pointの追加と削除

        /// <summary>
        /// アンカーハンドルの表示切替、Adornerの付け外し
        /// </summary>
        /// <returns>装飾</returns>
        public AnchorHandleAdorner? AnchorSwitch()
        {
            //図形のAdornerLayerに装飾が在れば削除、なければ作成、追加する
            if (MyShapesAnchorHandleAdorner == null)
            {
                MyShapesAnchorHandleAdorner = new AnchorHandleAdorner(MyGeoShape);
                MyShapesAnchorHandleAdorner.OnAnchorThumbDragCompleted += MyShapesAnchorHandleAdorner_OnDragCompleted;
                MyShapesAdornerLayer.Add(MyShapesAnchorHandleAdorner);
                return MyShapesAnchorHandleAdorner;
            }
            else
            {
                MyShapesAnchorHandleAdorner.OnAnchorThumbDragCompleted -= MyShapesAnchorHandleAdorner_OnDragCompleted;
                MyShapesAdornerLayer.Remove(MyShapesAnchorHandleAdorner);
                MyShapesAnchorHandleAdorner = null;
                return null;
            }
        }


        private void MyShapesAnchorHandleAdorner_OnDragCompleted(DragCompletedEventArgs obj)
        {
            OnAnchorHandleDragComleted?.Invoke(obj);
        }
        #endregion メソッド

    }







    /// <summary>
    /// 基礎Thumb、すべてのCustomControlThumbの派生元
    /// </summary>
    [DebuggerDisplay("{MyThumbType} {MyItemData.MyText}")]
    public abstract class KisoThumb : Thumb
    {
        //クリックダウンとドラッグ移動完了時に使う、直前に選択されたものかの判断用
        internal bool IsPreviewSelected { get; set; }


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

            Initialized += KisoThumb_Initialized;
            Loaded += KisoThumb_Loaded;

            PreviewMouseDown += KisoThumb_PreviewMouseDown2;
            PreviewMouseUp += KisoThumb_PreviewMouseUp2;

            DragStarted += KisoThumb_DragStarted2;
            DragDelta += Thumb_DragDelta3;
            DragCompleted += KisoThumb_DragCompleted3;

            KeyUp += KisoThumb_KeyUp;
            PreviewKeyDown += KisoThumb_PreviewKeyDown;
        }




        #region 初期化


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

        #endregion 初期化

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



        // マウスダウン時の処理、これはレフトボタンダウン時にしたほうがいいかも
        // このままだと右クリックでも発生する
        protected void KisoThumb_PreviewMouseDown2(object sender, MouseButtonEventArgs e)
        {
            if (this is RootThumb) { return; }
            var sou = e.Source;
            var ori = e.OriginalSource;
            //イベントのOriginalSourceからクリックされたThumbとFocusThumb候補を取得
            if (GetClickedCandidateThumb(e) is KisoThumb clicked
                && GetSelectableThumb(clicked) is KisoThumb focus)
            {
                //フォーカス候補とthisが一致したときだけ処理する、
                //こうしないとグループ内の他のThumbまで処理してしまう
                if (focus.MyItemData.MyGuid == this.MyItemData.MyGuid
                && GetRootThumb() is RootThumb root)
                {
                    clicked.Focusable = false;
                    focus.Focusable = false;
                    root.TestPreviewMouseDown(focus, clicked);
                }
                //e.Handled = true;
                //ここでtrueにするとドラッグ移動が動かなくなってしまう
                //ここでtrueにしないとグループの入れ子の数だけイベントが発生して、
                //同じ処理を繰り返すことになってしまう
            }

        }


        protected void KisoThumb_PreviewMouseUp2(object sender, MouseButtonEventArgs e)
        {
            if (this is RootThumb) { return; }

            if (GetClickedCandidateThumb((DependencyObject)e.OriginalSource) is KisoThumb clicked
                && GetSelectableThumb(clicked) is KisoThumb focus)
            {
                if (focus.MyItemData.MyGuid == this.MyItemData.MyGuid)
                {
                    clicked.Focusable = true;
                    focus.Focusable = true;
                    //重要、BringIntoViewこれがないとすっ飛んでいく
                    clicked.BringIntoView();

                    //こちらだとグループ全体が表示されるスクロールになる
                    //focus.BringIntoView();

                    //trueにすると、なぜか移動後のレイアウト更新が実行されなくなる
                    //e.Handled = true;
                }
            }
        }


        /// <summary>
        /// クリックイベントのオリジナルソースを元に、クリックされた基礎Thumbを返す
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private KisoThumb? GetClickedCandidateThumb(MouseButtonEventArgs e)
        {
            if (e.OriginalSource is DependencyObject dependency)
            {
                return GetClickedCandidateThumb(dependency);
            }
            return null;
        }

        /// <summary>
        /// クリックイベントのオリジナルソースを元に、クリックされた基礎Thumbを返す
        /// </summary>
        /// <param name="originalSource"></param>
        /// <returns></returns>
        private KisoThumb? GetClickedCandidateThumb(DependencyObject? originalSource)
        {
            if (originalSource is null) { return null; }
            if (originalSource is KisoThumb kiso) { return kiso; }
            return GetClickedCandidateThumb(VisualTreeHelper.GetParent(originalSource));
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
                            //Parentの自動リサイズを停止する
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
        /// SelectedThumb全てを移動
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

        // ドラッグ移動終了時
        protected void KisoThumb_DragCompleted3(object sender, DragCompletedEventArgs e)
        {
            if (this is RootThumb) { return; }

            if (GetClickedCandidateThumb((DependencyObject)e.OriginalSource) is KisoThumb clicked
                && GetSelectableThumb(clicked) is KisoThumb focus)
            {
                if (focus.MyItemData.MyGuid == this.MyItemData.MyGuid
                && GetRootThumb() is RootThumb root)
                {
                    if (focus.MyParentThumb?.MyExCanvas is ExCanvas canvas)
                    {
                        canvas.IsAutoResize = true;
                        focus.MyParentThumb?.ReLayout3();
                    }
                    root.TestDragCompleted(focus, e.HorizontalChange != 0 || e.VerticalChange != 0);
                    e.Handled = true;
                }
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


    /// <summary>
    /// Pointの追加削除はItemDataの操作じゃなくて、メソッドを使う、AddPoint
    /// </summary>
    public class GeoShapeThumb2 : KisoThumb
    {
        private AdornerLayer MyShepeAdornerLayer { get; set; } = null!;
        //public AnchorHandleThumb? MyAnchorHandle { get; private set; }
        public AnchorHandleAdorner? MyAnchorHandleAdorner { get; private set; }
        public GeoShape MyGeoShape { get; private set; } = null!;
        static GeoShapeThumb2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GeoShapeThumb2), new FrameworkPropertyMetadata(typeof(GeoShapeThumb2)));
        }
        public GeoShapeThumb2() { }
        public GeoShapeThumb2(ItemData data)
        {
            MyItemData = data;
            Loaded += GeoShapeThumb2_Loaded;
        }

        private void GeoShapeThumb2_Loaded(object sender, RoutedEventArgs e)
        {
            MyInitialize();
        }

        //初期化、図形の位置と自身のサイズを設定
        private void MyInitialize()
        {
            var shapeBounds = MyGeoShape.GetRenderBounds();
            Canvas.SetLeft(MyGeoShape, -shapeBounds.Left);
            Canvas.SetTop(MyGeoShape, -shapeBounds.Top);
            Width = shapeBounds.Width;
            Height = shapeBounds.Height;
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("geoShape") is GeoShape shape)
            {
                MyGeoShape = shape;
                MyShepeAdornerLayer = AdornerLayer.GetAdornerLayer(MyGeoShape);
            }
            else
            {
                throw new NullReferenceException("内部図形の取得に失敗");
            }
        }

        #region イベント
        /// <summary>
        /// アンカーハンドルThumbのドラッグ移動終了時
        /// </summary>
        /// <param name="obj"></param>
        private void MyAnchorHandleAdorner_OnDragCompleted(DragCompletedEventArgs obj)
        {
            //位置とサイズの修正、全体の再レイアウト
            UpdateLocateAndSize();
            MyParentThumb?.ReLayout3();
        }

        #endregion イベント

        #region メソッド

        /// <summary>
        /// 直線とベジェ曲線の切り替え
        /// </summary>
        public void ShapeTypeSwitch()
        {
            if (MyGeoShape.MyShapeType == ShapeType.Line)
            {
                ShapeTypeToBezier();
            }
            else
            {
                ShapeTypeToLine();
            }
        }
        public void ShapeTypeToLine()
        {
            MyGeoShape.MyShapeType = ShapeType.Line;
            MyAnchorHandleAdorner?.RemoveControlLine();
            UpdateLocateAndSize();
            MyParentThumb?.ReLayout3();
        }
        public void ShapeTypeToBezier()
        {
            MyGeoShape.MyShapeType = ShapeType.Bezier;
            MyAnchorHandleAdorner?.AddControlLine();
            UpdateLocateAndSize();
            MyParentThumb?.ReLayout3();
        }

        #region アンカーPointの追加と削除

        /// <summary>
        /// Pointの追加(挿入)
        /// </summary>
        /// <param name="poi"></param>
        /// <param name="id">追加(挿入)位置、省略時は末尾に追加する</param>
        public void AddPoint(Point poi, int id = -1)
        {
            if (id == -1) { id = MyItemData.MyPoints.Count; }
            MyItemData.MyPoints.Insert(id, poi);
            //アンカーハンドルが表示されている場合は、アンカーハンドルも追加する
            MyAnchorHandleAdorner?.AddAnchorHandleThumb(id, poi);
            UpdateLocateAndSize();
            MyParentThumb?.ReLayout3();
        }

        /// <summary>
        /// Pointの削除
        /// </summary>
        /// <param name="id">削除位置、省略時は末尾のPointを削除する</param>
        public void RemovePoint(int id = -1)
        {
            if(id == -1) {id = MyItemData.MyPoints.Count-1; }
            MyItemData.MyPoints.RemoveAt(id);
            MyAnchorHandleAdorner?.RemoveAnchorHandleThumb(id);
            UpdateLocateAndSize();
            MyParentThumb?.ReLayout3();
        }

        #endregion アンカーPointの追加と削除

        #region アンカーハンドルの表示切り替え
        public void AnchorSwitch()
        {
            if (MyAnchorHandleAdorner is null) { AnchorOn(); }
            else { AnchorOff(); }
        }
        public void AnchorOn()
        {
            if (MyAnchorHandleAdorner is null)
            {
                MyAnchorHandleAdorner = new(MyGeoShape);
                MyAnchorHandleAdorner.OnAnchorThumbDragCompleted += MyAnchorHandleAdorner_OnDragCompleted;
                MyShepeAdornerLayer.Add(MyAnchorHandleAdorner);
                UpdateLocateAndSize();
                MyParentThumb?.ReLayout3();
            }
        }
        public void AnchorOff()
        {
            if (MyAnchorHandleAdorner != null)
            {
                MyAnchorHandleAdorner.OnAnchorThumbDragCompleted -= MyAnchorHandleAdorner_OnDragCompleted;
                MyShepeAdornerLayer.Remove(MyAnchorHandleAdorner);
                MyAnchorHandleAdorner = null;
                UpdateLocateAndSize();
                MyParentThumb?.ReLayout3();
            }
        }
        #endregion アンカーハンドルの表示切り替え

        //位置とサイズの更新
        public void UpdateLocateAndSize()
        {
            //図形のBounds(図形が収まるRect)から決める、ただし
            //アンカーハンドルが表示されている場合は、
            //アンカーハンドルのBoundsと合成(union)したものから決める
            Rect neko = MyGeoShape.GetRenderBounds();
            Rect unionRect = MyGeoShape.GetRenderBounds();
            if (MyAnchorHandleAdorner?.GetHandlesRenderBounds() is Rect handlesRect)
            {
                unionRect.Union(handlesRect);
            }

            //サイズはそのままBoundsのサイズ
            Width = unionRect.Width;
            Height = unionRect.Height;

            //図形の位置を修正する前に元の位置を取得
            var shapeLeft = Canvas.GetLeft(MyGeoShape);
            var shapeTop = Canvas.GetTop(MyGeoShape);
            Canvas.SetLeft(MyGeoShape, -unionRect.Left);
            Canvas.SetTop(MyGeoShape, -unionRect.Top);

            //自身の位置、図形の位置と反対方向
            MyItemData.MyLeft += unionRect.Left + shapeLeft;
            MyItemData.MyTop += unionRect.Top + shapeTop;
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

        #endregion メソッド


        #region 依存関係プロパティ



        #endregion 依存関係プロパティ

    }






    /// <summary>
    /// Canvasの中にGeoShapeThumbを配置したThumb
    /// 作成にはItemDataが必要
    /// 図形のアンカーハンドルの表示切替
    /// 自身のリサイズ用のハンドルの表示切替
    /// 図形の移動と自身の移動
    /// </summary>
    public class GeoShapeTThumb : KisoThumb
    {
        private AdornerLayer MyAdornerLayer { get; set; } = null!;
        static GeoShapeTThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GeoShapeTThumb), new FrameworkPropertyMetadata(typeof(GeoShapeTThumb)));
        }
        public GeoShapeTThumb() { }
        public GeoShapeTThumb(ItemData data)
        {
            MyItemData = data;
            //DataContext = data;
            //DragDelta += GeoShapeTThumb_DragDelta;
            Loaded += GeoShapeTThumb_Loaded;
        }

        //private void GeoShapeTThumb_DragDelta(object sender, DragDeltaEventArgs e)
        //{
        //    MyLeft += e.HorizontalChange;
        //    MyTop += e.VerticalChange;
        //    e.Handled = true;
        //}

        private void GeoShapeTThumb_Loaded(object sender, RoutedEventArgs e)
        {
            MyResizeHandleAdorner = new ResizeHandleAdorner(this);
            if (AdornerLayer.GetAdornerLayer(this) is AdornerLayer layer)
            {
                MyAdornerLayer = layer;
            }
            if (MyShapeThumb != null)
            {
                FitToShapeAndAnchorHandle();
            }
            else
            {
                throw new NullReferenceException("内部要素がない");

            }
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("shapeThumb") is GeoShapeThumb shape)
            {
                MyShapeThumb = shape;
                MyShapeThumb.OnAnchorHandleDragComleted += MyShapeThumb_OnAnchorHandleDragComleted;
                if (MyItemData.MyPoints == null)
                {
                    MyItemData.MyPoints = [new Point(0, 0), new Point(200, 100)];
                }

            }

        }

        //アンカーハンドルのドラッグ移動終了時には、位置とサイズを更新する
        private void MyShapeThumb_OnAnchorHandleDragComleted(DragCompletedEventArgs obj)
        {
            FitToShapeAndAnchorHandle();
        }




        #region 依存関係プロパティ

        //リサイズハンドル
        public ResizeHandleAdorner MyResizeHandleAdorner
        {
            get { return (ResizeHandleAdorner)GetValue(MyResizeHandleAdornerProperty); }
            protected set { SetValue(MyResizeHandleAdornerProperty, value); }
        }
        public static readonly DependencyProperty MyResizeHandleAdornerProperty =
            DependencyProperty.Register(nameof(MyResizeHandleAdorner), typeof(ResizeHandleAdorner), typeof(GeoShapeTThumb), new PropertyMetadata(null));



        #region 図形Thumbとのバインド用

        public GeoShapeThumb MyShapeThumb
        {
            get { return (GeoShapeThumb)GetValue(MyShapeThumbProperty); }
            set { SetValue(MyShapeThumbProperty, value); }
        }
        public static readonly DependencyProperty MyShapeThumbProperty =
            DependencyProperty.Register(nameof(MyShapeThumb), typeof(GeoShapeThumb), typeof(GeoShapeTThumb), new PropertyMetadata(null));
        #endregion 図形とのバインド用

        #region 自身用

        //public ItemData MyItemData
        //{
        //    get { return (ItemData)GetValue(MyItemDataProperty); }
        //    protected set { SetValue(MyItemDataProperty, value); }
        //}
        //public static readonly DependencyProperty MyItemDataProperty =
        //    DependencyProperty.Register(nameof(MyItemData), typeof(ItemData), typeof(GeoShapeTThumb), new PropertyMetadata(null));


        //public double MyLeft
        //{
        //    get { return (double)GetValue(MyLeftProperty); }
        //    set { SetValue(MyLeftProperty, value); }
        //}
        //public static readonly DependencyProperty MyLeftProperty =
        //    DependencyProperty.Register(nameof(MyLeft), typeof(double), typeof(GeoShapeTThumb),
        //        new FrameworkPropertyMetadata(0.0,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //public double MyTop
        //{
        //    get { return (double)GetValue(MyTopProperty); }
        //    set { SetValue(MyTopProperty, value); }
        //}
        //public static readonly DependencyProperty MyTopProperty =
        //    DependencyProperty.Register(nameof(MyTop), typeof(double), typeof(GeoShapeTThumb),
        //        new FrameworkPropertyMetadata(0.0,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion 自身用

        #endregion 依存関係プロパティ

        #region メソッド

        #region Pointの追加と削除

        /// <summary>
        /// Pointの追加
        /// </summary>
        /// <param name="index">挿入箇所Index</param>
        /// <param name="poi">Point</param>
        public void AddPoint(int index, Point poi)
        {
            MyShapeThumb.AddPoint(index, poi);
        }

        /// <summary>
        /// Pointを末尾に追加
        /// </summary>
        /// <param name="poi"></param>
        public void AddPoint(Point poi)
        {
            int id = MyItemData.MyPoints.Count;
            AddPoint(id, poi);
        }

        /// <summary>
        /// 指定IndexのPointを削除
        /// </summary>
        /// <param name="index"></param>
        public void RemovePoint(int index)
        {
            //最低2個は残して削除処理
            if (MyItemData.MyPoints.Count > 2)
            {
                //MyItemData.MyPoints.RemoveAt(index);
                MyShapeThumb.RemovePoint(index);
            }
        }

        /// <summary>
        /// 先頭のPointを削除
        /// </summary>
        public void RemovePointTop()
        {
            RemovePoint(0);
        }
        /// <summary>
        /// 末尾のPointを削除
        /// </summary>
        public void RemovePointEnd()
        {
            RemovePoint(MyItemData.MyPoints.Count - 1);
        }
        #endregion Pointの追加と削除



        //直線とベジェ曲線の切り替え
        public void ChangeShapeType()
        {
            MyShapeThumb.ChangeShapeType();
        }
        public void ChangeToLine()
        {
            MyShapeThumb.ChangeToLine();
        }
        public void ChangeToBezier()
        {
            MyShapeThumb.ChangeToBezier();
        }

        /// <summary>
        /// 自身をリサイズとアンカーの両方のハンドルを含むサイズと位置にする
        /// </summary>
        public void FitSizeAndPosAdorner()
        {
            //リサイズハンドルのBounds
            MyResizeHandleAdorner.GetHandlesRenderBounds();
        }

        //リサイズハンドルの表示位置変更
        public void ChangeResizeHandleType()
        {
            var handleLayout = MyResizeHandleAdorner.MyHandleLayout;
            if (handleLayout == HandleLayoutType.In)
            {
                handleLayout = HandleLayoutType.Middle;
            }
            else if (handleLayout == HandleLayoutType.Middle)
            {
                handleLayout = HandleLayoutType.Out;
            }
            else
            {
                handleLayout = HandleLayoutType.In;
            }
            MyResizeHandleAdorner.MyHandleLayout = handleLayout;
        }

        /// <summary>
        /// サイズと位置の調整、
        /// 図形のアンカーハンドルが表示されていれば、ハンドルと図形の両方に合わせる
        /// ハンドルが非表示ならば、図形に合わせる
        /// </summary>
        public void FitToShapeAndAnchorHandle()
        {
            //Rect? AnchorBounds = MyShapeThumb.MyShapesAnchorHandleAdorner?.GetHandlesRenderBounds();
            //Rect shapeBounds = MyShapeThumb.GetShapeRenderBounds();

            //図形とそのアンカーハンドルが収まるサイズと位置の取得
            //図形のBoundsと全ハンドルのBoundsをUnionすると取得できる

            Rect union = MyShapeThumb.GetShapeRenderBounds();
            if (MyShapeThumb.MyShapesAnchorHandleAdorner?.GetHandlesRenderBounds() is Rect rr)
            {
                union.Union(rr);
            }

            //サイズ変更、サイズはUnionのサイズと同じ
            Width = union.Width;
            Height = union.Height;


            //位置変更は今の図形の位置とUnionとの差を計算、差分を図形に適用、自身は反対側に移動

            var offsetLeft = union.Left + MyShapeThumb.MyLeft;
            var offsetTop = union.Top + MyShapeThumb.MyTop;

            MyShapeThumb.MyLeft -= offsetLeft;
            MyShapeThumb.MyTop -= offsetTop;
            //MyLeft += offsetLeft;
            //MyTop += offsetTop;
            MyItemData.MyLeft += offsetLeft;
            MyItemData.MyTop += offsetTop;

        }

        /// <summary>
        /// 自身のサイズと位置を図形に合わせる、けど
        /// FitToShapeAndAnchorHandleがあるから必要ないかも
        /// </summary>
        public void FitSizeAndPos()
        {
            var shapeBounds = MyShapeThumb.GetShapeRenderBounds();

            //var myBounds = new Rect(MyLeft, MyTop, Width, Height);
            //var unionBouns = new Rect(MyLeft, MyTop, Width, Height);
            //unionBouns.Union(shapeBounds);

            //自身と図形の横位置
            var offsetLeft = MyShapeThumb.MyLeft + shapeBounds.Left;
            MyItemData.MyLeft += offsetLeft;
            //MyLeft += offsetLeft;
            MyShapeThumb.MyLeft -= offsetLeft;

            //自身と図形の縦位置
            var offsetTop = MyShapeThumb.MyTop + shapeBounds.Top;
            MyItemData.MyTop += offsetTop;
            //MyTop += offsetTop;
            MyShapeThumb.MyTop -= offsetTop;

            //自身のサイズ
            Width = shapeBounds.Width;
            Height = shapeBounds.Height;
        }
        public void AnchorHandleSwitch()
        {
            MyShapeThumb?.AnchorSwitch();
        }


        #region リサイズハンドル

        /// <summary>
        /// リサイズハンドルの表示切替、Adornerの付け外し
        /// </summary>
        public void ResizeHandleSwitch()
        {
            //図形のAdornerLayerを調べてリサイズ用の装飾が在れば削除、なければ作成追加する
            var items = MyAdornerLayer.GetAdorners(this);
            if (items != null)
            {
                RemoveResizeHandleAdorner();
            }
            else
            {
                AddResizeHandleAdorner();
            }
        }

        /// <summary>
        /// リサイズハンドルの追加と
        /// リサイズ時の縦横位置の変更量を通知するイベントを購読
        /// </summary>
        public void AddResizeHandleAdorner()
        {
            MyResizeHandleAdorner.OnTargetLeftChanged += MyResizeHandleAdorner_OnTargetLeftChanged;
            MyResizeHandleAdorner.OnTargetTopChanged += MyResizeHandleAdorner_OnTargetTopChanged;
            MyAdornerLayer.Add(MyResizeHandleAdorner);
        }

        /// <summary>
        /// リサイズハンドルの削除と
        /// リサイズ時の縦横位置の変更量を通知するイベントの購読を解除
        /// </summary>
        public void RemoveResizeHandleAdorner()
        {
            MyResizeHandleAdorner.OnTargetLeftChanged -= MyResizeHandleAdorner_OnTargetLeftChanged;
            MyResizeHandleAdorner.OnTargetTopChanged -= MyResizeHandleAdorner_OnTargetTopChanged;
            MyAdornerLayer.Remove(MyResizeHandleAdorner);
        }

        /// <summary>
        /// リサイズ時に縦位置が変更されたとき、図形Thumbを逆方向へ移動
        /// </summary>
        /// <param name="obj"></param>
        private void MyResizeHandleAdorner_OnTargetTopChanged(double obj)
        {
            MyShapeThumb.MyTop -= obj;
        }

        private void MyResizeHandleAdorner_OnTargetLeftChanged(double obj)
        {
            MyShapeThumb.MyLeft -= obj;
        }
        #endregion リサイズハンドル

        #endregion メソッド


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

        //private void AddItemsData()
        //{
        //    for (int i = 0; i < MyThumbs.Count; i++)
        //    {
        //        MyItemData.MyThumbsItemData.Add(MyThumbs[i].MyItemData);
        //    }
        //}
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
            //DragCompleted -= KisoThumb_DragCompleted2;
            DragCompleted -= KisoThumb_DragCompleted3;
            PreviewMouseDown -= KisoThumb_PreviewMouseDown2;
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
        /// 子要素のマウス移動後、選択ThumbとFocusThumbの更新処理
        /// </summary>
        /// <param name="thumb">移動したThumb</param>
        /// <param name="isMoved">移動した？</param>
        internal void TestDragCompleted(KisoThumb thumb, bool isMoved)
        {
            if (MySelectedThumbs.Count <= 1) { return; }

            //移動していない＋通常クリック
            //選択Thumbクリア後に対象を追加
            else if (!isMoved && Keyboard.Modifiers == ModifierKeys.None)
            {
                SelectedThumbsClearAndAddThumb(thumb);
            }
            //移動していない＋ctrlクリック
            //直前追加じゃなければ対象を削除して、FocusThumbの選定
            else if (!isMoved && Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (!thumb.IsPreviewSelected)
                {
                    int currrentIndex = MySelectedThumbs.IndexOf(thumb);
                    MySelectedThumbs.Remove(thumb);
                    if (currrentIndex == 0)
                    {
                        MyFocusThumb = MySelectedThumbs[0];
                    }
                    else
                    {
                        MyFocusThumb = MySelectedThumbs[currrentIndex - 1];
                    }
                }
                thumb.IsPreviewSelected = false;
            }
        }

        /// <summary>
        /// 子要素のマウスダウン時の処理、FocusThumbとClickedThumbの更新、選択Thumbの更新
        /// </summary>
        /// <param name="focusCandidate">FocusThumb候補</param>
        /// <param name="clickedCandidate">ClickedThumb候補</param>
        internal void TestPreviewMouseDown(KisoThumb focusCandidate, KisoThumb clickedCandidate)
        {
            MyClickedThumb = clickedCandidate;
            bool withControlKey = Keyboard.Modifiers == ModifierKeys.Control;
            bool isContains = MySelectedThumbs.Contains(focusCandidate);
            //選択Thumb以外をクリック、入れ替え
            if (!isContains && Keyboard.Modifiers == ModifierKeys.None)
            {
                SelectedThumbsClearAndAddThumb(focusCandidate);
            }
            //選択Thumb以外をctrlクリック→追加
            else if (!isContains && Keyboard.Modifiers == ModifierKeys.Control)
            {
                SelectedThumbsToAdd(focusCandidate);
                focusCandidate.IsPreviewSelected = true;
            }
        }

        /// <summary>
        /// MySelectedThumbsへの追加
        /// </summary>
        /// <param name="kiso"></param>
        internal void SelectedThumbsToAdd(KisoThumb kiso)
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
            SelectedThumbsToAdd(kiso);
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
                    SelectedThumbsToAdd(motoGroup);
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
                    SelectedThumbsToAdd(MyClickedThumb);
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
            SelectedThumbsToAdd(thumb);
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




    /// <summary>
    /// GeoShape用のアンカーハンドルThumb
    /// </summary>
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


        public double MyLeft
        {
            get { return (double)GetValue(MyLeftProperty); }
            set { SetValue(MyLeftProperty, value); }
        }
        public static readonly DependencyProperty MyLeftProperty =
            DependencyProperty.Register(nameof(MyLeft), typeof(double), typeof(AnchorHandleThumb),
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
            DependencyProperty.Register(nameof(MyTop), typeof(double), typeof(AnchorHandleThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

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
            else if (data.MyThumbType == ThumbType.PolyLine)
            {
                return new GeoShapeTThumb(data);
            }
            else if (data.MyThumbType == ThumbType.GeoShape)
            {
                return new GeoShapeThumb2(data);
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