//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Controls.Primitives;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Media;
//using System.Windows;
//using System.Windows.Shapes;
//using System.Windows.Data;
//using System.Windows.Input;


//namespace _20250313
//{

//    //public class RangeTransparentAdorner : Adorner
//    //{
//    //    VisualCollection MyVisuals;
//    //    protected override int VisualChildrenCount => MyVisuals.Count;
//    //    protected override Visual GetVisualChild(int index) => MyVisuals[index];
//    //    private readonly TTRange MyTarget;
//    //    private readonly TransparentThumb thumbTopL, thumbBotR, thumbTopR, thumbBotL, thumbTop, thumbBot, thumbL, thumbR;

//    //    public RangeTransparentAdorner(TTRange adornedElement) : base(adornedElement)
//    //    {
//    //        MyTarget = adornedElement;
//    //        MyVisuals = new VisualCollection(this);

//    //    }

//    //}

//    //How to Create a Resize Adorner in WPF - YouTube
//    //https://www.youtube.com/watch?v=ddVXKMpWGME


//    ////ドラッグ移動中はTTRootでのLayoutUpdateは実行しないで、
//    ////ドラッグ移動終了後に実行する
//    ///// <summary>
//    ///// 範囲選択用ThumbのTTRange専用Adorner、ハンドル形状は○
//    ///// </summary>
//    //public class RangeAdorner : Adorner
//    //{
//    //    readonly VisualCollection MyVisuals;
//    //    protected override int VisualChildrenCount => MyVisuals.Count;
//    //    protected override Visual GetVisualChild(int index) => MyVisuals[index];


//    //    private readonly TwoColorWakuEllipseThumb thumbTopL, thumbBotR, thumbTopR, thumbBotL, thumbTop, thumbBot, thumbL, thumbR;
//    //    private readonly TThumb MyTarget;
//    //    //public double ThumbSize { get; private set; } = 30.0;


//    //    public double ThumbSize
//    //    {
//    //        get { return (double)GetValue(ThumbSizeProperty); }
//    //        set { SetValue(ThumbSizeProperty, value); }
//    //    }
//    //    public static readonly DependencyProperty ThumbSizeProperty =
//    //        DependencyProperty.Register(nameof(ThumbSize), typeof(double), typeof(RangeAdorner),
//    //            new FrameworkPropertyMetadata(40.0,
//    //                FrameworkPropertyMetadataOptions.AffectsRender |
//    //                FrameworkPropertyMetadataOptions.AffectsMeasure |
//    //                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



//    //    public RangeAdorner(TThumb adornedElement) : base(adornedElement)
//    //    {
//    //        MyTarget = adornedElement;
//    //        MyVisuals = new VisualCollection(this);
//    //        thumbTop = new TwoColorWakuEllipseThumb() { Width = ThumbSize, Height = ThumbSize };
//    //        thumbTopL = new TwoColorWakuEllipseThumb() { Width = ThumbSize, Height = ThumbSize };
//    //        thumbTopR = new TwoColorWakuEllipseThumb() { Width = ThumbSize, Height = ThumbSize };
//    //        thumbBot = new TwoColorWakuEllipseThumb() { Width = ThumbSize, Height = ThumbSize };
//    //        thumbBotL = new TwoColorWakuEllipseThumb() { Width = ThumbSize, Height = ThumbSize };
//    //        thumbBotR = new TwoColorWakuEllipseThumb() { Width = ThumbSize, Height = ThumbSize };
//    //        thumbL = new TwoColorWakuEllipseThumb() { Width = ThumbSize, Height = ThumbSize };
//    //        thumbR = new TwoColorWakuEllipseThumb() { Width = ThumbSize, Height = ThumbSize };

//    //        thumbTopL.DragDelta += ThumbTopL_DragDelta;
//    //        thumbBotR.DragDelta += ThumbBotR_DragDelta;
//    //        thumbTop.DragDelta += ThumbTop_DragDelta;
//    //        thumbTopR.DragDelta += ThumbTopR_DragDelta;
//    //        thumbBot.DragDelta += ThumbBot_DragDelta;
//    //        thumbBotL.DragDelta += ThumbBotL_DragDelta;
//    //        thumbL.DragDelta += ThumbL_DragDelta;
//    //        thumbR.DragDelta += ThumbR_DragDelta;

//    //        thumbTop.DragCompleted += Thumb_DragCompleted;
//    //        thumbTopL.DragCompleted += Thumb_DragCompleted;
//    //        thumbTopR.DragCompleted += Thumb_DragCompleted;
//    //        thumbBot.DragCompleted += Thumb_DragCompleted;
//    //        thumbBotL.DragCompleted += Thumb_DragCompleted;
//    //        thumbBotR.DragCompleted += Thumb_DragCompleted;
//    //        thumbL.DragCompleted += Thumb_DragCompleted;
//    //        thumbR.DragCompleted += Thumb_DragCompleted;

//    //        MyVisuals.Add(thumbTop);
//    //        MyVisuals.Add(thumbTopL);
//    //        MyVisuals.Add(thumbTopR);
//    //        MyVisuals.Add(thumbBot);
//    //        MyVisuals.Add(thumbBotL);
//    //        MyVisuals.Add(thumbL);
//    //        MyVisuals.Add(thumbR);
//    //        //右下を最後に追加
//    //        MyVisuals.Add(thumbBotR);

//    //        thumbTop.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbTop.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbTopL.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbTopL.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbTopR.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbTopR.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbBot.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbBot.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbBotL.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbBotL.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbBotR.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbBotR.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbR.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbR.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbL.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbL.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });

//    //        ContextMenu context = new();
//    //        this.ContextMenu = context;
//    //        context.Items.Add(new MenuItem() { Header = "test" });
//    //    }

//    //    #region ドラッグ移動時の動作

//    //    //移動中はマイナス座標でも修正しない
//    //    /// <summary>
//    //    /// ハンドルの横移動時に使う
//    //    /// </summary>
//    //    /// <param name="horizontalChange"></param>
//    //    /// <param name="toLeft">左要素のハンドルならtrue指定、右ならfalse</param>
//    //    private void DeltaHorizontal(double horizontalChange, bool toLeft)
//    //    {
//    //        double w;
//    //        if (toLeft)
//    //        {
//    //            w = MyTarget.Width - horizontalChange;
//    //            MyTarget.TTLeft += horizontalChange;
//    //        }
//    //        else
//    //        {
//    //            w = MyTarget.Width + horizontalChange;
//    //            if (w < 1)
//    //            {
//    //                MyTarget.TTLeft += horizontalChange;
//    //            }
//    //        }
//    //        if (w < 1) w = 1;
//    //        MyTarget.Width = w;
//    //    }
//    //    private void DeltaVertical(double verticalChange, bool toTop)
//    //    {
//    //        double h;
//    //        if (toTop)
//    //        {
//    //            h = MyTarget.Height - verticalChange;
//    //            MyTarget.TTTop += verticalChange;
//    //        }
//    //        else
//    //        {
//    //            h = MyTarget.Height + verticalChange;
//    //            if (h < 1)
//    //            {
//    //                MyTarget.TTTop += verticalChange;
//    //            }
//    //        }
//    //        if (h < 1) h = 1;
//    //        MyTarget.Height = h;
//    //    }
//    //    //右
//    //    private void ThumbR_DragDelta(object sender, DragDeltaEventArgs e)
//    //    {
//    //        DeltaHorizontal(e.HorizontalChange, false);
//    //    }

//    //    //左
//    //    private void ThumbL_DragDelta(object sender, DragDeltaEventArgs e)
//    //    {
//    //        DeltaHorizontal(e.HorizontalChange, true);
//    //    }

//    //    //左下
//    //    private void ThumbBotL_DragDelta(object sender, DragDeltaEventArgs e)
//    //    {
//    //        DeltaHorizontal(e.HorizontalChange, true);
//    //        DeltaVertical(e.VerticalChange, false);
//    //    }

//    //    //下
//    //    private void ThumbBot_DragDelta(object sender, DragDeltaEventArgs e)
//    //    {
//    //        DeltaVertical(e.VerticalChange, false);
//    //    }

//    //    //右上
//    //    private void ThumbTopR_DragDelta(object sender, DragDeltaEventArgs e)
//    //    {
//    //        DeltaVertical(e.VerticalChange, true);
//    //        DeltaHorizontal(e.HorizontalChange, false);
//    //    }

//    //    //上
//    //    private void ThumbTop_DragDelta(object sender, DragDeltaEventArgs e)
//    //    {
//    //        DeltaVertical(e.VerticalChange, true);
//    //    }

//    //    //右下
//    //    private void ThumbBotR_DragDelta(object sender, DragDeltaEventArgs e)
//    //    {
//    //        DeltaHorizontal(e.HorizontalChange, false);
//    //        DeltaVertical(e.VerticalChange, false);
//    //    }

//    //    //左上
//    //    private void ThumbTopL_DragDelta(object sender, DragDeltaEventArgs e)
//    //    {
//    //        DeltaHorizontal(e.HorizontalChange, true);
//    //        DeltaVertical(e.VerticalChange, true);
//    //    }
//    //    #endregion ドラッグ移動時の動作

//    //    //ドラッグ移動終了後にLayoutUpdateを実行
//    //    private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
//    //    {
//    //        MyTarget.TTParent?.TTGroupUpdateLayout();
//    //    }

//    //    protected override Size ArrangeOverride(Size finalSize)
//    //    {
//    //        double tsHalf = ThumbSize / 2.0;
//    //        double w = AdornedElement.DesiredSize.Width;
//    //        double h = AdornedElement.DesiredSize.Height;

//    //        double horizontalCenter = (w / 2.0) - tsHalf;
//    //        double verticalCenter = (h / 2.0) - tsHalf;
//    //        double horizontalRight = w - tsHalf;
//    //        double verticalBottom = h - tsHalf;
//    //        thumbTop.Arrange(new Rect(horizontalCenter, -tsHalf, ThumbSize, ThumbSize));
//    //        thumbTopL.Arrange(new Rect(-tsHalf, -tsHalf, ThumbSize, ThumbSize));
//    //        thumbTopR.Arrange(new Rect(horizontalRight, -tsHalf, ThumbSize, ThumbSize));
//    //        thumbBot.Arrange(new Rect(horizontalCenter, verticalBottom, ThumbSize, ThumbSize));
//    //        thumbBotL.Arrange(new Rect(-tsHalf, verticalBottom, ThumbSize, ThumbSize));
//    //        thumbBotR.Arrange(new Rect(horizontalRight, verticalBottom, ThumbSize, ThumbSize));
//    //        thumbL.Arrange(new Rect(-tsHalf, verticalCenter, ThumbSize, ThumbSize));
//    //        thumbR.Arrange(new Rect(horizontalRight, verticalCenter, ThumbSize, ThumbSize));


//    //        return base.ArrangeOverride(finalSize);
//    //    }
//    //}


//    //public class ResizeAdorner : Adorner
//    //{
//    //    #region 必要

//    //    readonly VisualCollection MyVisuals;
//    //    protected override int VisualChildrenCount => MyVisuals.Count;
//    //    protected override Visual GetVisualChild(int index) => MyVisuals[index];
//    //    #endregion 必要

//    //    #region 依存関係プロパティ


//    //    public double ThumbSize
//    //    {
//    //        get { return (double)GetValue(ThumbSizeProperty); }
//    //        set { SetValue(ThumbSizeProperty, value); }
//    //    }
//    //    public static readonly DependencyProperty ThumbSizeProperty =
//    //        DependencyProperty.Register(nameof(ThumbSize), typeof(double), typeof(ResizeAdorner),
//    //            new FrameworkPropertyMetadata(40.0,
//    //                FrameworkPropertyMetadataOptions.AffectsRender |
//    //                FrameworkPropertyMetadataOptions.AffectsMeasure |
//    //                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
//    //    #endregion 依存関係プロパティ

//    //    private readonly TwoColorWakuEllipseThumb thumbTopL, thumbBotR, thumbTopR, thumbBotL, thumbTop, thumbBot, thumbL, thumbR;
//    //    private readonly FrameworkElement MyTarget;
//    //    //public double ThumbSize { get; private set; } = 30.0;



//    //    public ResizeAdorner(UIElement adornedElement) : base(adornedElement)
//    //    {
//    //        MyTarget = (FrameworkElement)adornedElement;
//    //        MyVisuals = new VisualCollection(this);
//    //        thumbTop = new TwoColorWakuEllipseThumb() { Width = ThumbSize, Height = ThumbSize };
//    //        thumbTopL = new TwoColorWakuEllipseThumb() { Width = ThumbSize, Height = ThumbSize };
//    //        thumbTopR = new TwoColorWakuEllipseThumb() { Width = ThumbSize, Height = ThumbSize };
//    //        thumbBot = new TwoColorWakuEllipseThumb() { Width = ThumbSize, Height = ThumbSize };
//    //        thumbBotL = new TwoColorWakuEllipseThumb() { Width = ThumbSize, Height = ThumbSize };
//    //        thumbBotR = new TwoColorWakuEllipseThumb() { Width = ThumbSize, Height = ThumbSize };
//    //        thumbL = new TwoColorWakuEllipseThumb() { Width = ThumbSize, Height = ThumbSize };
//    //        thumbR = new TwoColorWakuEllipseThumb() { Width = ThumbSize, Height = ThumbSize };

//    //        thumbTopL.DragDelta += ThumbTopL_DragDelta;
//    //        thumbBotR.DragDelta += ThumbBotR_DragDelta;
//    //        thumbTop.DragDelta += ThumbTop_DragDelta;
//    //        thumbTopR.DragDelta += ThumbTopR_DragDelta;
//    //        thumbBot.DragDelta += ThumbBot_DragDelta;
//    //        thumbBotL.DragDelta += ThumbBotL_DragDelta;
//    //        thumbL.DragDelta += ThumbL_DragDelta;
//    //        thumbR.DragDelta += ThumbR_DragDelta;

//    //        //thumbTop.DragCompleted += Thumb_DragCompleted;
//    //        //thumbTopL.DragCompleted += Thumb_DragCompleted;
//    //        //thumbTopR.DragCompleted += Thumb_DragCompleted;
//    //        //thumbBot.DragCompleted += Thumb_DragCompleted;
//    //        //thumbBotL.DragCompleted += Thumb_DragCompleted;
//    //        //thumbBotR.DragCompleted += Thumb_DragCompleted;
//    //        //thumbL.DragCompleted += Thumb_DragCompleted;
//    //        //thumbR.DragCompleted += Thumb_DragCompleted;

//    //        MyVisuals.Add(thumbTop);
//    //        MyVisuals.Add(thumbTopL);
//    //        MyVisuals.Add(thumbTopR);
//    //        MyVisuals.Add(thumbBot);
//    //        MyVisuals.Add(thumbBotL);
//    //        MyVisuals.Add(thumbL);
//    //        MyVisuals.Add(thumbR);
//    //        //右下を最後に追加
//    //        MyVisuals.Add(thumbBotR);

//    //        thumbTop.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbTop.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbTopL.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbTopL.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbTopR.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbTopR.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbBot.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbBot.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbBotL.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbBotL.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbBotR.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbBotR.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbR.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbR.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbL.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
//    //        thumbL.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });

//    //        ContextMenu context = new();
//    //        this.ContextMenu = context;
//    //        context.Items.Add(new MenuItem() { Header = "test" });
//    //    }

//    //    #region ドラッグ移動時の動作


//    //    private void MySetLeft(double horizontalChange)
//    //    {
//    //        Canvas.SetLeft(MyTarget, Canvas.GetLeft(MyTarget) + horizontalChange);
//    //    }

//    //    private void MySetTop(double verticalChange)
//    //    {
//    //        Canvas.SetTop(MyTarget, Canvas.GetTop(MyTarget) + verticalChange);
//    //    }

//    //    //移動中はマイナス座標でも修正しない
//    //    /// <summary>
//    //    /// ハンドルの横移動時に使う
//    //    /// </summary>
//    //    /// <param name="horizontalChange"></param>
//    //    /// <param name="isLeft">左要素のハンドルならtrue指定、右ならfalse</param>
//    //    private void DeltaHorizontal(double horizontalChange, bool isLeft)
//    //    {
//    //        double w;
//    //        if (isLeft)
//    //        {
//    //            w = MyTarget.Width - horizontalChange;
//    //            MySetLeft(horizontalChange);
//    //        }
//    //        else
//    //        {
//    //            w = MyTarget.Width + horizontalChange;
//    //            if (w < 1)
//    //            {
//    //                MySetLeft(horizontalChange);
//    //            }
//    //        }
//    //        if (w < 1) w = 1;
//    //        MyTarget.Width = w;
//    //    }

//    //    private void DeltaVertical(double verticalChange, bool toTop)
//    //    {
//    //        double h;
//    //        if (toTop)
//    //        {
//    //            h = MyTarget.Height - verticalChange;
//    //            MySetTop(verticalChange);
//    //        }
//    //        else
//    //        {
//    //            h = MyTarget.Height + verticalChange;
//    //            if (h < 1)
//    //            {
//    //                MySetTop(verticalChange);
//    //            }
//    //        }
//    //        if (h < 1) h = 1;
//    //        MyTarget.Height = h;
//    //    }
//    //    //右
//    //    private void ThumbR_DragDelta(object sender, DragDeltaEventArgs e)
//    //    {
//    //        DeltaHorizontal(e.HorizontalChange, false);
//    //    }

//    //    //左
//    //    private void ThumbL_DragDelta(object sender, DragDeltaEventArgs e)
//    //    {
//    //        DeltaHorizontal(e.HorizontalChange, true);
//    //    }

//    //    //左下
//    //    private void ThumbBotL_DragDelta(object sender, DragDeltaEventArgs e)
//    //    {
//    //        DeltaHorizontal(e.HorizontalChange, true);
//    //        DeltaVertical(e.VerticalChange, false);
//    //    }

//    //    //下
//    //    private void ThumbBot_DragDelta(object sender, DragDeltaEventArgs e)
//    //    {
//    //        DeltaVertical(e.VerticalChange, false);
//    //    }

//    //    //右上
//    //    private void ThumbTopR_DragDelta(object sender, DragDeltaEventArgs e)
//    //    {
//    //        DeltaVertical(e.VerticalChange, true);
//    //        DeltaHorizontal(e.HorizontalChange, false);
//    //    }

//    //    //上
//    //    private void ThumbTop_DragDelta(object sender, DragDeltaEventArgs e)
//    //    {
//    //        DeltaVertical(e.VerticalChange, true);
//    //    }

//    //    //右下
//    //    private void ThumbBotR_DragDelta(object sender, DragDeltaEventArgs e)
//    //    {
//    //        DeltaHorizontal(e.HorizontalChange, false);
//    //        DeltaVertical(e.VerticalChange, false);
//    //    }

//    //    //左上
//    //    private void ThumbTopL_DragDelta(object sender, DragDeltaEventArgs e)
//    //    {
//    //        DeltaHorizontal(e.HorizontalChange, true);
//    //        DeltaVertical(e.VerticalChange, true);
//    //    }
//    //    #endregion ドラッグ移動時の動作


//    //    ////ドラッグ移動終了後にLayoutUpdateを実行
//    //    //private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
//    //    //{
//    //    //    MyTarget.TTParent?.TTGroupUpdateLayout();
//    //    //}

//    //    protected override Size ArrangeOverride(Size finalSize)
//    //    {
//    //        double tsHalf = ThumbSize / 2.0;
//    //        double w = AdornedElement.DesiredSize.Width;
//    //        double h = AdornedElement.DesiredSize.Height;

//    //        double horizontalCenter = (w / 2.0) - tsHalf;
//    //        double verticalCenter = (h / 2.0) - tsHalf;
//    //        double horizontalRight = w - tsHalf;
//    //        double verticalBottom = h - tsHalf;
//    //        thumbTop.Arrange(new Rect(horizontalCenter, -tsHalf, ThumbSize, ThumbSize));
//    //        thumbTopL.Arrange(new Rect(-tsHalf, -tsHalf, ThumbSize, ThumbSize));
//    //        thumbTopR.Arrange(new Rect(horizontalRight, -tsHalf, ThumbSize, ThumbSize));
//    //        thumbBot.Arrange(new Rect(horizontalCenter, verticalBottom, ThumbSize, ThumbSize));
//    //        thumbBotL.Arrange(new Rect(-tsHalf, verticalBottom, ThumbSize, ThumbSize));
//    //        thumbBotR.Arrange(new Rect(horizontalRight, verticalBottom, ThumbSize, ThumbSize));
//    //        thumbL.Arrange(new Rect(-tsHalf, verticalCenter, ThumbSize, ThumbSize));
//    //        thumbR.Arrange(new Rect(horizontalRight, verticalCenter, ThumbSize, ThumbSize));


//    //        return base.ArrangeOverride(finalSize);
//    //    }

//    //}















//    /// <summary>
//    /// GeometricShape専用アドーナー、頂点座標にThumb表示と制御線も表示
//    /// VisualCollectionにはCanvasだけを追加
//    /// ThumbはCanvasに追加
//    /// </summary>
//    public class GeometryAdorner : Adorner
//    {
//        #region 必要

//        public VisualCollection MyVisuals { get; private set; }
//        protected override int VisualChildrenCount => MyVisuals.Count;
//        protected override Visual GetVisualChild(int index) => MyVisuals[index];
//        #endregion 必要

//        #region 依存関係プロパティ
//        public PointCollection MyPoints
//        {
//            get { return (PointCollection)GetValue(MyPointsProperty); }
//            set { SetValue(MyPointsProperty, value); }
//        }
//        public static readonly DependencyProperty MyPointsProperty =
//            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(GeometryAdorner),
//                new FrameworkPropertyMetadata(new PointCollection(),
//                    FrameworkPropertyMetadataOptions.AffectsRender |
//                    FrameworkPropertyMetadataOptions.AffectsMeasure |
//                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

//        //読み取り専用の依存関係プロパティ
//        //WPF4.5入門 その43 「読み取り専用の依存関係プロパティ」 - かずきのBlog@hatena
//        //        https://blog.okazuki.jp/entry/2014/08/18/083455
//        /// <summary>
//        /// 頂点Thumbすべてが収まるRect
//        /// </summary>
//        private static readonly DependencyPropertyKey MyVThumbsBoundsPropertyKey =
//            DependencyProperty.RegisterReadOnly(nameof(MyVThumbsBounds), typeof(Rect), typeof(GeometryAdorner),
//                new FrameworkPropertyMetadata(Rect.Empty,
//                    FrameworkPropertyMetadataOptions.AffectsRender |
//                    FrameworkPropertyMetadataOptions.AffectsMeasure));
//        public static readonly DependencyProperty MyVThumbsBoundsProperty =
//            MyVThumbsBoundsPropertyKey.DependencyProperty;
//        public Rect MyVThumbsBounds
//        {
//            get { return (Rect)GetValue(MyVThumbsBoundsProperty); }
//            private set { SetValue(MyVThumbsBoundsPropertyKey, value); }
//        }
//        public double MyAnchorThumbSize
//        {
//            get { return (double)GetValue(MyAnchorThumbSizeProperty); }
//            set { SetValue(MyAnchorThumbSizeProperty, value); }
//        }
//        public static readonly DependencyProperty MyAnchorThumbSizeProperty =
//            DependencyProperty.Register(nameof(MyAnchorThumbSize), typeof(double), typeof(GeometryAdorner),
//                new FrameworkPropertyMetadata(20.0,
//                    FrameworkPropertyMetadataOptions.AffectsRender |
//                    FrameworkPropertyMetadataOptions.AffectsMeasure |
//                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

//        #endregion 依存関係プロパティ


//        #region その他プロパティ

//        public Canvas MyCanvas { get; private set; } = new();
//        public List<AnchorThumb> MyThumbs { get; private set; } = new();
//        public TwoColorDashLine MyDirectionLine { get; private set; }
//        public GeometricShape MyTargetGeoShape { get; private set; }
//        public AnchorThumb? MyCurrentAnchorThumb { get; private set; }
//        #endregion その他プロパティ

//        //頂点Thumbの右クリックメニュー、頂点削除
//        public ContextMenu MyAnchorContextMenu { get; private set; }
//        private MenuItem MyMenuItemRemoveThumb;

//        //コンストラクタ
//        //Canvas要素をVisualCollectionに追加
//        //Canvasに頂点Thumbと制御線を追加する
//        public GeometryAdorner(GeometricShape adornedElement) : base(adornedElement)
//        {
//            MyTargetGeoShape = adornedElement;
//            MyVisuals = new VisualCollection(this) { MyCanvas, };
//            //if (MyTargetGeoShape.MyShapeType == ShapeType.Bezier)
//            //{
//            //}
//            MyDirectionLine = new TwoColorDashLine();
//            MyCanvas.Children.Add(MyDirectionLine);
//            MyAnchorContextMenu = new ContextMenu();
//            MyAnchorContextMenu.Opened += MyAnchorContextMenu_Opened;
//            MyMenuItemRemoveThumb = new() { Header = "頂点削除" };
//            MyMenuItemRemoveThumb.Click += Item_Click;
//            MyAnchorContextMenu.Items.Add(MyMenuItemRemoveThumb);

//            Loaded += GeometryAdorner_Loaded;
//        }


//        //頂点Thumbの右クリックメニュー開いたとき
//        //頂点削除の項目の有無効化を設定
//        private void MyAnchorContextMenu_Opened(object sender, RoutedEventArgs e)
//        {
//            if (MyCurrentAnchorThumb is AnchorThumb anchor)
//            {
//                int ii = MyThumbs.IndexOf(anchor);
//                if (MyTargetGeoShape.MyShapeType == ShapeType.Line)
//                {
//                    if (MyPoints.Count >= 2)
//                    {
//                        MyMenuItemRemoveThumb.IsEnabled = true;
//                    }
//                    else MyMenuItemRemoveThumb.IsEnabled = false;
//                }
//                else if (MyTargetGeoShape.MyShapeType == ShapeType.Bezier)
//                {
//                    if (ii % 3 == 0 && MyPoints.Count >= 7)
//                    {
//                        MyMenuItemRemoveThumb.IsEnabled = true;
//                    }
//                    else
//                    {
//                        MyMenuItemRemoveThumb.IsEnabled = false;
//                    }
//                }

//            }
//        }

//        //右クリックメニュー、頂点Thumb削除
//        public event Action<object>? PointRemoved;
//        private void Item_Click(object sender, RoutedEventArgs e)
//        {
//            //削除
//            if (RemovePointAndAnchor())
//            {
//                MyCurrentAnchorThumb = null;
//                PointRemoved?.Invoke(this);//通知で描画更新
//            }
//            ;

//        }
//        private bool RemovePointAndAnchor()
//        {
//            if (MyCurrentAnchorThumb is AnchorThumb anchor)
//            {
//                int ii = MyThumbs.IndexOf(anchor);
//                if (MyTargetGeoShape.MyShapeType == ShapeType.Line && MyPoints.Count > 2)
//                {
//                    RemovePointAndAnchor(anchor);
//                    return true;
//                }
//                else if (MyTargetGeoShape.MyShapeType == ShapeType.Bezier
//                    && MyPoints.Count >= 7)
//                {
//                    if (ii == 0)
//                    {
//                        RemovePointAndAnchor(ii);
//                        RemovePointAndAnchor(ii);
//                        RemovePointAndAnchor(ii);
//                        return true;
//                    }
//                    else if (ii == MyPoints.Count - 1)
//                    {
//                        RemovePointAndAnchor(MyPoints.Count - 1);
//                        RemovePointAndAnchor(MyPoints.Count - 1);
//                        RemovePointAndAnchor(MyPoints.Count - 1);
//                        return true;
//                    }
//                    else if (ii % 3 == 0)
//                    {
//                        RemovePointAndAnchor(ii - 1);
//                        RemovePointAndAnchor(ii - 1);
//                        RemovePointAndAnchor(ii - 1);
//                        return true;
//                    }
//                }
//                return false;
//            }
//            return false;
//        }
//        /// <summary>
//        /// 頂点Thumbと対応Pointを削除する
//        /// </summary>
//        /// <param name="anchor"></param>
//        private void RemovePointAndAnchor(AnchorThumb anchor)
//        {
//            int ii = MyThumbs.IndexOf(anchor);
//            MyPoints.RemoveAt(ii);
//            anchor.DragDelta -= Thumb_DragDelta;
//            anchor.DragCompleted -= Thumb_DragCompleted;
//            anchor.PreviewMouseDown -= Thumb_PreviewMouseDown;
//            MyThumbs.Remove(anchor);
//            MyCanvas.Children.Remove(anchor);
//        }
//        private void RemovePointAndAnchor(int ii)
//        {
//            MyPoints.RemoveAt(ii);
//            AnchorThumb anchor = MyThumbs[ii];
//            anchor.DragDelta -= Thumb_DragDelta;
//            anchor.DragCompleted -= Thumb_DragCompleted;
//            anchor.PreviewMouseDown -= Thumb_PreviewMouseDown;
//            MyThumbs.Remove(anchor);
//            MyCanvas.Children.Remove(anchor);
//        }


//        private void GeometryAdorner_Loaded(object sender, RoutedEventArgs e)
//        {
//            SetBinding(MyPointsProperty, new Binding() { Source = MyTargetGeoShape, Path = new PropertyPath(GeometricShape.MyPointsProperty) });

//            InitializeThumbs();
//            if (MyTargetGeoShape.MyShapeType == ShapeType.Bezier)
//            {
//                MyDirectionLine.MyPoints = MyTargetGeoShape.MyPoints;
//            }

//            //制御線のプロパティ設定
//            //MyDirectionLine.Stroke = Brushes.Red;
//            //MyDirectionLine.StrokeBase = Brushes.Black;
//            //MyDirectionLine.StrokeThickness = 1.0;


//            Rect ptsR = GetPointsRect(MyPoints);
//            Rect r = new(ptsR.X, ptsR.Y, ptsR.Width + MyAnchorThumbSize, ptsR.Height + MyAnchorThumbSize);
//            MyCanvas.Arrange(r);

//        }

//        private void InitializeThumbs()
//        {
//            foreach (var item in MyPoints)
//            {
//                AnchorThumb thumb = new(item);
//                MyThumbs.Add(thumb);
//                MyCanvas.Children.Add(thumb);
//                SetAnchorLocate(thumb, item);
//                thumb.DragDelta += Thumb_DragDelta;
//                thumb.DragCompleted += Thumb_DragCompleted;
//                thumb.PreviewMouseDown += Thumb_PreviewMouseDown;
//                thumb.Cursor = Cursors.Hand;
//                thumb.ContextMenu = MyAnchorContextMenu;
//            }
//        }

//        private void Thumb_PreviewMouseDown(object sender, MouseButtonEventArgs e)
//        {
//            if (sender is AnchorThumb anchor) { MyCurrentAnchorThumb = anchor; }
//        }


//        /// <summary>
//        /// Thumbの再配置、Thumbs位置をPointsに合わせる
//        /// </summary>
//        public void FixThumbsLocate()
//        {
//            for (int i = 0; i < MyPoints.Count; i++)
//            {
//                SetAnchorLocate(MyThumbs[i], MyPoints[i]);
//            }
//        }

//        #region ドラッグ移動


//        //ドラッグ移動終了後に、そのことをイベント通知
//        public event Action<object, Vector>? ThumbDragConpleted;
//        private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
//        {
//            Thumb t = (AnchorThumb)sender;
//            ThumbDragConpleted?.Invoke(this, new Vector(Canvas.GetLeft(t), Canvas.GetTop(t)));
//        }

//        //ベジェ曲線の方向線とアンカーポイント、制御点を表示してみた - 午後わてんのブログ
//        //        https://gogowaten.hatenablog.com/entry/15547295
//        //20180226forMyBlog/20180612_ベジェ曲線のアンカーと制御点までの直線 at master · gogowaten/20180226forMyBlog
//        //        https://github.com/gogowaten/20180226forMyBlog/tree/master/20180612_%E3%83%99%E3%82%B8%E3%82%A7%E6%9B%B2%E7%B7%9A%E3%81%AE%E3%82%A2%E3%83%B3%E3%82%AB%E3%83%BC%E3%81%A8%E5%88%B6%E5%BE%A1%E7%82%B9%E3%81%BE%E3%81%A7%E3%81%AE%E7%9B%B4%E7%B7%9A


//        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
//        {

//            //ベジェ曲線のときは操作によって他の頂点も連動させたい
//            //+ctrlで対角線上連動、これは使わないからいらないかも
//            //+shiftで対角線上連動＋距離連動、+shiftは取り消し、初期値true
//            //固定点と制御点の連動、初期値true
//            //角度固定(長さのみ可変)

//            if (sender is AnchorThumb t)
//            {
//                int i = MyThumbs.IndexOf(t);
//                PointCollection points = MyPoints;
//                double x = points[i].X + e.HorizontalChange;
//                double y = points[i].Y + e.VerticalChange;

//                if (MyTargetGeoShape.MyShapeType == ShapeType.Line)
//                {
//                    points[i] = new Point(x, y);
//                    SetAnchorLocate(t, points[i]);

//                }
//                //ベジェ曲線のとき、今のところ固定点移動時に制御点も同時移動、制御点移動は対角線上になるように移動
//                else if (MyTargetGeoShape.MyShapeType == ShapeType.Bezier)
//                {
//                    if (Keyboard.Modifiers == ModifierKeys.Shift)
//                    {
//                        points[i] = new Point(x, y);
//                        SetAnchorLocate(t, points[i]);

//                    }
//                    else
//                    {
//                        //前制御点、固定点、後制御点の判定
//                        Point frontP, anchorP, rearP;
//                        int mod = i % 3;
//                        double xAdd = e.HorizontalChange;
//                        double yAdd = e.VerticalChange;
//                        //自身が固定点の場合
//                        if (mod == 0)
//                        {
//                            //固定点、前後の制御点も同じ移動
//                            anchorP = points[i];
//                            SetAnchorLocate2(i, new Point(anchorP.X + xAdd, anchorP.Y + yAdd));

//                            //自身が先頭固定点じゃないときは前制御点があるので移動させる
//                            if (i != 0)
//                            {
//                                frontP = points[i - 1];
//                                SetAnchorLocate2(i - 1, new Point(frontP.X + xAdd, frontP.Y + yAdd));
//                            }

//                            //自身が最終固定点じゃないときは後制御点があるので移動させる
//                            if (i != points.Count - 1)
//                            {
//                                rearP = points[i + 1];
//                                SetAnchorLocate2(i + 1, new Point(rearP.X + xAdd, rearP.Y + yAdd));
//                            }

//                        }
//                        //自身が後制御点
//                        else if (mod == 1)
//                        {
//                            rearP = points[i];
//                            SetAnchorLocate2(i, new Point(rearP.X + xAdd, rearP.Y + yAdd));

//                            //前制御点の移動、対角線上になるように移動
//                            if (i != 1)
//                            {
//                                anchorP = points[i - 1];
//                                double xDiff = rearP.X - anchorP.X;
//                                double yDiff = rearP.Y - anchorP.Y;
//                                SetAnchorLocate2(i - 2, new Point(anchorP.X - xDiff, anchorP.Y - yDiff));
//                            }
//                        }
//                        //自身が前制御点
//                        else
//                        {
//                            //後制御点の移動、対角線上になるように移動
//                            frontP = points[i];
//                            SetAnchorLocate2(i, new Point(frontP.X + xAdd, frontP.Y + yAdd));

//                            if (i != points.Count - 2)
//                            {
//                                anchorP = points[i + 1];
//                                double xDiff = frontP.X - anchorP.X;
//                                double yDiff = frontP.Y - anchorP.Y;
//                                SetAnchorLocate2(i + 2, new Point(anchorP.X - xDiff, anchorP.Y - yDiff));
//                            }
//                        }
//                    }

//                }


//            }
//        }
//        #endregion ドラッグ移動


//        private void SetAnchorLocate2(int index, Point point)
//        {
//            MyTargetGeoShape.MyPoints[index] = point;
//            SetAnchorLocate(MyThumbs[index], point);
//        }


//        private void SetAnchorLocate(AnchorThumb anchor, Point point)
//        {
//            Canvas.SetLeft(anchor, point.X);
//            Canvas.SetTop(anchor, point.Y);
//            //Canvas.SetLeft(anchor, point.X - (MyAnchorThumbSize / 2.0));
//            //Canvas.SetTop(anchor, point.Y - (MyAnchorThumbSize / 2.0));

//        }

//        private void UpdateCanvasBounds()
//        {
//            //CanvasのサイズをArrange()で指定する、サイズは頂点Thumbが収まるサイズ
//            Rect ptsRect = GetPointsRect(MyPoints);
//            //座標もここで指定できそうなんだけど、なぜか指定値の半分になるのでここではしていない
//            var r = new Rect(new Size(ptsRect.Size.Width + MyAnchorThumbSize, ptsRect.Size.Height + MyAnchorThumbSize));
//            MyCanvas.Arrange(r);
//            MyVThumbsBounds = r;
//        }
//        //最終的？なRectの指定
//        protected override Size ArrangeOverride(Size finalSize)
//        {
//            UpdateCanvasBounds();
//            return base.ArrangeOverride(finalSize);
//        }

//        /// <summary>
//        /// PointCollectionのRectを返す
//        /// </summary>
//        /// <param name="pt">PointCollection</param>
//        /// <returns></returns>
//        public static Rect GetPointsRect(PointCollection pt)
//        {
//            if (pt.Count == 0) return new Rect();
//            double minX = double.MaxValue;
//            double minY = double.MaxValue;
//            double maxX = double.MinValue;
//            double maxY = double.MinValue;
//            foreach (var item in pt)
//            {
//                if (minX > item.X) minX = item.X;
//                if (minY > item.Y) minY = item.Y;
//                if (maxX < item.X) maxX = item.X;
//                if (maxY < item.Y) maxY = item.Y;
//            }
//            return new Rect(minX, minY, maxX - minX, maxY - minY);
//        }

//    }

//    ///// <summary>
//    ///// 頂点座標にThumb表示するアドーナー。GeometricShape専用
//    ///// VisualCollectionにはCanvasだけを追加
//    ///// ThumbはCanvasに追加
//    ///// </summary>
//    //public class GeometryAdorner : Adorner
//    //{
//    //    public VisualCollection MyVisuals { get; private set; }
//    //    protected override int VisualChildrenCount => MyVisuals.Count;
//    //    protected override Visual GetVisualChild(int index) => MyVisuals[index];

//    //    public Canvas MyCanvas { get; private set; } = new();
//    //    public List<AnchorThumb> MyThumbs { get; private set; } = new();
//    //    //public List<Thumb> MyThumbs { get; private set; } = new();
//    //    public GeometricShape MyTargetGeoShape { get; private set; }
//    //    public GeometryAdorner(GeometricShape adornedElement) : base(adornedElement)
//    //    {
//    //        MyVisuals = new VisualCollection(this)
//    //        {
//    //            MyCanvas
//    //        };
//    //        MyTargetGeoShape = adornedElement;
//    //        Loaded += GeometryAdorner_Loaded;
//    //    }

//    //    /// <summary>
//    //    /// Thumbの再配置、Pointと位置がずれたときに使う
//    //    /// </summary>
//    //    public void RelocationThumbs()
//    //    {
//    //        for (int i = 0; i < MyTargetGeoShape.MyPoints.Count; i++)
//    //        {
//    //            SetAnchorLocate(MyThumbs[i], MyTargetGeoShape.MyPoints[i]);
//    //        }
//    //    }
//    //    private void InitializeThumbs()
//    //    {
//    //        foreach (var item in MyTargetGeoShape.MyPoints)
//    //        {
//    //            AnchorThumb thumb = new(item);
//    //            MyThumbs.Add(thumb);
//    //            MyCanvas.Children.Add(thumb);
//    //            SetAnchorLocate(thumb, item);
//    //            thumb.DragDelta += Thumb_DragDelta;
//    //            thumb.DragCompleted += Thumb_DragCompleted;
//    //        }
//    //    }

//    //    private void GeometryAdorner_Loaded(object sender, RoutedEventArgs e)
//    //    {
//    //        InitializeThumbs();
//    //    }

//    //    #region ドラッグ移動

//    //    //ドラッグ移動終了後に親Thumbを位置修正＋頂点Thumbの位置修正
//    //    private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
//    //    {
//    //        if (MyTargetGeoShape.MyOwnerTThumb?.FixLocate() == true)
//    //        {
//    //            RelocationThumbs();
//    //        }
//    //    }

//    //    private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
//    //    {
//    //        if (sender is AnchorThumb t)
//    //        {
//    //            int i = MyThumbs.IndexOf(t);
//    //            PointCollection points = MyTargetGeoShape.MyPoints;
//    //            double x = points[i].X + e.HorizontalChange;
//    //            double y = points[i].Y + e.VerticalChange;
//    //            points[i] = new Point(x, y);
//    //            SetAnchorLocate(t, points[i]);
//    //        }
//    //    }
//    //    #endregion ドラッグ移動

//    //    private static void SetAnchorLocate(AnchorThumb anchor, Point point)
//    //    {
//    //        Canvas.SetLeft(anchor, point.X - (anchor.Size / 2.0));
//    //        Canvas.SetTop(anchor, point.Y - (anchor.Size / 2.0));
//    //    }

//    //    //最終的？なRectの指定
//    //    protected override Size ArrangeOverride(Size finalSize)
//    //    {
//    //        //Thumbが収まるRectをCanvasのArrangeに指定する
//    //        Rect canvasRect = VisualTreeHelper.GetDescendantBounds(MyCanvas);
//    //        if (canvasRect.IsEmpty)
//    //        {
//    //            MyCanvas.Arrange(new Rect(finalSize));
//    //        }
//    //        else
//    //        {
//    //            //座標を0,0したRectにする、こうしないとマイナス座表示に不具合
//    //            canvasRect = new(canvasRect.Size);
//    //            MyCanvas.Arrange(canvasRect);
//    //        }
//    //        return base.ArrangeOverride(finalSize);
//    //    }

//    //}


//}