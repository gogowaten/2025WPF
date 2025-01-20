using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250119
{
    /// <summary>
    /// このカスタム コントロールを XAML ファイルで使用するには、手順 1a または 1b の後、手順 2 に従います。
    ///
    /// 手順 1a) 現在のプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:_20250119"
    ///
    ///
    /// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:_20250119;assembly=_20250119"
    ///
    /// また、XAML ファイルのあるプロジェクトからこのプロジェクトへのプロジェクト参照を追加し、
    /// リビルドして、コンパイル エラーを防ぐ必要があります:
    ///
    ///     ソリューション エクスプローラーで対象のプロジェクトを右クリックし、
    ///     [参照の追加] の [プロジェクト] を選択してから、このプロジェクトを参照し、選択します。
    ///
    ///
    /// 手順 2)
    /// コントロールを XAML ファイルで使用します。
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class GridPolyLine : Control
    {

        public Polyline MyPolyline
        {
            get { return (Polyline)GetValue(MyPolylineProperty); }
            set { SetValue(MyPolylineProperty, value); }
        }
        public static readonly DependencyProperty MyPolylineProperty =
            DependencyProperty.Register(nameof(MyPolyline), typeof(Polyline), typeof(GridPolyLine), new PropertyMetadata(null));

        static GridPolyLine()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GridPolyLine), new FrameworkPropertyMetadata(typeof(GridPolyLine)));
        }
        public GridPolyLine() { }
    }

    [ContentProperty(nameof(MyPoints))]
    public class CanvasPolyLine : Control
    {

        public double MyStrokeThickness
        {
            get { return (double)GetValue(MyStrokeThicknessProperty); }
            set { SetValue(MyStrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeThicknessProperty =
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(CanvasPolyLine), new PropertyMetadata(1.0));

        public Polyline MyPolyline
        {
            get { return (Polyline)GetValue(MyPolylineProperty); }
            set { SetValue(MyPolylineProperty, value); }
        }
        public static readonly DependencyProperty MyPolylineProperty =
            DependencyProperty.Register(nameof(MyPolyline), typeof(Polyline), typeof(CanvasPolyLine), new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsArrange |
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public Canvas MyCanvas
        {
            get { return (Canvas)GetValue(MyCanvasProperty); }
            set { SetValue(MyCanvasProperty, value); }
        }
        public static readonly DependencyProperty MyCanvasProperty =
            DependencyProperty.Register(nameof(MyCanvas), typeof(Canvas), typeof(CanvasPolyLine), new PropertyMetadata(null));


        public double MyDescendantWidth
        {
            get { return (double)GetValue(MyDescendantWidthProperty); }
            set { SetValue(MyDescendantWidthProperty, value); }
        }
        public static readonly DependencyProperty MyDescendantWidthProperty =
            DependencyProperty.Register(nameof(MyDescendantWidth), typeof(double), typeof(CanvasPolyLine), new FrameworkPropertyMetadata(0.0,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsArrange));

        public double MyDescendantHeight
        {
            get { return (double)GetValue(MyDescendantHeightProperty); }
            set { SetValue(MyDescendantHeightProperty, value); }
        }
        public static readonly DependencyProperty MyDescendantHeightProperty =
            DependencyProperty.Register(nameof(MyDescendantHeight), typeof(double), typeof(CanvasPolyLine), new FrameworkPropertyMetadata(0.0,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsArrange));

        public double MyDescendantX
        {
            get { return (double)GetValue(MyDescendantXProperty); }
            set { SetValue(MyDescendantXProperty, value); }
        }
        public static readonly DependencyProperty MyDescendantXProperty =
            DependencyProperty.Register(nameof(MyDescendantX), typeof(double), typeof(CanvasPolyLine), new PropertyMetadata(0.0));
        public double MyDescendantOffsetX
        {
            get { return (double)GetValue(MyDescendantOffsetXProperty); }
            set { SetValue(MyDescendantOffsetXProperty, value); }
        }
        public static readonly DependencyProperty MyDescendantOffsetXProperty =
            DependencyProperty.Register(nameof(MyDescendantOffsetX), typeof(double), typeof(CanvasPolyLine), new PropertyMetadata(0.0));

        public double MyDescendantY
        {
            get { return (double)GetValue(MyDescendantYProperty); }
            set { SetValue(MyDescendantYProperty, value); }
        }
        public static readonly DependencyProperty MyDescendantYProperty =
            DependencyProperty.Register(nameof(MyDescendantY), typeof(double), typeof(CanvasPolyLine), new PropertyMetadata(0.0));

        public double MyDescendantOffsetY
        {
            get { return (double)GetValue(MyDescendantOffsetYProperty); }
            set { SetValue(MyDescendantOffsetYProperty, value); }
        }
        public static readonly DependencyProperty MyDescendantOffsetYProperty =
            DependencyProperty.Register(nameof(MyDescendantOffsetY), typeof(double), typeof(CanvasPolyLine), new PropertyMetadata(0.0));


        public Rect MyDescendantBounds
        {
            get { return (Rect)GetValue(MyDescendantBoundsProperty); }
            set { SetValue(MyDescendantBoundsProperty, value); }
        }
        public static readonly DependencyProperty MyDescendantBoundsProperty =
            DependencyProperty.Register(nameof(MyDescendantBounds), typeof(Rect), typeof(CanvasPolyLine), new PropertyMetadata(null));

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(CanvasPolyLine), new PropertyMetadata(null, new PropertyChangedCallback(OnMyPoins)));

        private static void OnMyPoins(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CanvasPolyLine cp)
            {
                var n = e.NewValue;
                var o = e.OldValue;
                var p = e.Property;
                //cp.UpdateLayout();//効果なし
                cp.UpdateDescendantBounds();
            }
        }

        //public Rect MyDescendantBounds { get; private set; }

        static CanvasPolyLine()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CanvasPolyLine), new FrameworkPropertyMetadata(typeof(CanvasPolyLine)));
        }
        public CanvasPolyLine()
        {
            Loaded += CanvasPolyLine_Loaded;

        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_Line") is Polyline line)
            {
                MyPolyline = line;
                Canvas.SetLeft(MyPolyline, 0);
                Canvas.SetTop(MyPolyline, 0);

                //Bindingを切る
                //MyPolyline.Points = MyPoints;

                MyPolyline.SizeChanged += MyPolyline_SizeChanged;
            }
            if (GetTemplateChild("PART_Panel") is Canvas panel)
            {
                MyCanvas = panel;
                Canvas.SetLeft(MyCanvas, 0);
                Canvas.SetTop(MyCanvas, 0);
            }

        }

        private void MyPolyline_SizeChanged(object sender, SizeChangedEventArgs e)
        {
           var w = e.WidthChanged;
            var h=e.HeightChanged;
            var si = e.NewSize;
            var pre = e.PreviousSize;
            UpdateDescendantBounds();
        }

        private void CanvasPolyLine_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDescendantBounds();

            //MyCanvas.SetBinding(Canvas.LeftProperty, new Binding() { Source = this, Path = new PropertyPath(MyDescendantOffsetXProperty) });
            //SetBinding(Canvas.LeftProperty, new Binding() { Source = this, Path = new PropertyPath(MyDescendantOffsetXProperty) });
            MyPolyline.SetBinding(Canvas.LeftProperty, new Binding() { Source = this, Path = new PropertyPath(MyDescendantOffsetXProperty) });
            MyPolyline.SetBinding(Canvas.TopProperty, new Binding() { Source = this, Path = new PropertyPath(MyDescendantOffsetYProperty) });

            SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(MyDescendantWidthProperty) });
            SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(MyDescendantHeightProperty) });


            
        }

        private void UpdateDescendantBounds()
        {
            if(MyCanvas == null) { return; }
            //UpdateLayout();//効果なし

            var des = VisualTreeHelper.GetDescendantBounds(MyPolyline);
            //var des = VisualTreeHelper.GetDescendantBounds(MyCanvas);
            //var des = VisualTreeHelper.GetContentBounds(MyCanvas);
            //var des = VisualTreeHelper.GetContentBounds(MyPolyline);
            MyDescendantWidth = des.Width;
            MyDescendantHeight = des.Height;
            MyDescendantX = des.X;
            MyDescendantOffsetX = -des.X;
            MyDescendantY = des.Y;
            MyDescendantOffsetY = -des.Y;
            MyDescendantBounds = des;

        }

        public void PointChange()
        {
            var old = MyPoints[1];
            var np = new Point(old.X+20, old.Y+20);
            MyPoints[1] = np;
            UpdateLayout();//効果あり
            UpdateDescendantBounds();
        }
        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
        }
        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        }
        protected override void OnChildDesiredSizeChanged(UIElement child)
        {
            base.OnChildDesiredSizeChanged(child);
        }
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            //base.OnRenderSizeChanged(sizeInfo);
            //var des = VisualTreeHelper.GetDescendantBounds(MyPolyline);
            //MyDescendantBounds = des;

        }
    }


}
