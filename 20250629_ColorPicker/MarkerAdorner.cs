using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Input;

namespace _20250629_ColorPicker
{
    public class MarkerAdorner : Adorner
    {
        #region Adornerに必要

        private VisualCollection MyVisuals { get; set; }
        protected override int VisualChildrenCount => MyVisuals.Count;
        protected override Visual GetVisualChild(int index) => MyVisuals[index];

        // 超必要、これがないとCanvasのサイズが0
        protected override Size ArrangeOverride(Size finalSize)
        {
            MyCanvas.Arrange(new Rect(finalSize));
            return base.ArrangeOverride(finalSize);
        }

        #endregion Adornerに必要

        public EllipseThumb MyMarkerThumb = new();
        public Canvas MyCanvas = new();
        public FrameworkElement MyTargetElement;
        private double MyXOffset;
        private double MyYOffset;

        public MarkerAdorner(FrameworkElement adornedElement) : base(adornedElement)
        {
            MyTargetElement = adornedElement;

            MyVisuals = new VisualCollection(this) { MyCanvas };
            SetMyCanvas();
            SetMyMarker();

        }


        // Canvasのクリック字
        private void MyCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // クリック座標を割合にしたのが彩度と明度
            Point poi = e.GetPosition(MyCanvas);
            var x = poi.X / MyCanvas.Width;
            var y = poi.Y / MyCanvas.Height;
            MySaturation = x;
            MyValue = y;

            // MarkerThumbのイベントを発動？これを実行すると
            // DragStartedが動く
            MyMarkerThumb.RaiseEvent(e);
        }

        // マウスドラッグ移動開始
        private void MyMarkerThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            MyXOffset = e.HorizontalOffset - MyMarkerSize / 2.0;
            MyYOffset = e.VerticalOffset - MyMarkerSize / 2.0;
        }


        // マウスドラッグ移動
        private void MyMarkerThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            // 移動量に応じて彩度と明度を変更
            // x移動量を割合で取得
            double mr = (MyXOffset + e.HorizontalChange) / MyCanvas.Width;
            // 彩度に移動量を足して0から1.0に制限したのが答え
            MySaturation = double.Clamp(mr + MySaturation, 0.0, 1.0);

            // y
            mr = (MyYOffset + e.VerticalChange) / MyCanvas.Height;
            MyValue = double.Clamp(mr + MyValue, 0.0, 1.0);
        }


        // Canvasの初期化
        private void SetMyCanvas()
        {
            MyCanvas.MouseLeftButtonDown += MyCanvas_MouseLeftButtonDown;

            MyCanvas.Background = Brushes.Transparent;
            MyCanvas.Children.Add(MyMarkerThumb);

            MyCanvas.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ActualWidthProperty) });
            MyCanvas.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ActualHeightProperty) });
        }

        // Markerの初期化
        private void SetMyMarker()
        {
            MyMarkerThumb.DragDelta += MyMarkerThumb_DragDelta;
            MyMarkerThumb.DragStarted += MyMarkerThumb_DragStarted;

            // 輪郭線の色
            MyMarkerThumb.SetBinding(EllipseThumb.MyInsideStrokeProperty, new Binding() { Source = this, Path = new PropertyPath(MyInsideStrokeProperty) });
            MyMarkerThumb.SetBinding(EllipseThumb.MyOutsideStrokeProperty, new Binding() { Source = this, Path = new PropertyPath(MyOutsideStrokeProperty) });

            // サイズ
            MyMarkerThumb.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(MyMarkerSizeProperty) });
            MyMarkerThumb.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(MyMarkerSizeProperty) });

            // x座標、Saturation
            MultiBinding mb;
            mb = new MultiBinding() { Converter = new MyConvSV() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyMarkerSizeProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MySaturationProperty) });
            mb.Bindings.Add(new Binding() { Source = MyTargetElement, Path = new PropertyPath(ActualWidthProperty) });
            _ = MyMarkerThumb.SetBinding(Canvas.LeftProperty, mb);

            // y座標、Value
            mb = new MultiBinding() { Converter = new MyConvSV() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyMarkerSizeProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyValueProperty) });
            mb.Bindings.Add(new Binding() { Source = MyTargetElement, Path = new PropertyPath(ActualHeightProperty) });
            _ = MyMarkerThumb.SetBinding(Canvas.TopProperty, mb);

        }


        #region 依存関係プロパティ

        public double MySaturation
        {
            get { return (double)GetValue(MySaturationProperty); }
            set { SetValue(MySaturationProperty, value); }
        }
        public static readonly DependencyProperty MySaturationProperty =
            DependencyProperty.Register(nameof(MySaturation), typeof(double), typeof(MarkerAdorner), new PropertyMetadata(0.0));

        public double MyValue
        {
            get { return (double)GetValue(MyValueProperty); }
            set { SetValue(MyValueProperty, value); }
        }
        public static readonly DependencyProperty MyValueProperty =
            DependencyProperty.Register(nameof(MyValue), typeof(double), typeof(MarkerAdorner), new PropertyMetadata(0.0));

        public Brush MyInsideStroke
        {
            get { return (Brush)GetValue(MyInsideStrokeProperty); }
            set { SetValue(MyInsideStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyInsideStrokeProperty =
            DependencyProperty.Register(nameof(MyInsideStroke), typeof(Brush), typeof(MarkerAdorner), new PropertyMetadata(Brushes.Black));

        public Brush MyOutsideStroke
        {
            get { return (Brush)GetValue(MyOutsideStrokeProperty); }
            set { SetValue(MyOutsideStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyOutsideStrokeProperty =
            DependencyProperty.Register(nameof(MyOutsideStroke), typeof(Brush), typeof(MarkerAdorner), new PropertyMetadata(Brushes.White));


        public double MyMarkerSize
        {
            get { return (double)GetValue(MyMarkerSizeProperty); }
            set { SetValue(MyMarkerSizeProperty, value); }
        }
        public static readonly DependencyProperty MyMarkerSizeProperty =
            DependencyProperty.Register(nameof(MyMarkerSize), typeof(double), typeof(MarkerAdorner), new PropertyMetadata(20.0));

        #endregion 依存関係プロパティ
    }


    // SをMarkerのX座標に変換、Vはy座標
    public class MyConvSV : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var markerSize = (double)values[0];
            var sv = (double)values[1];
            var targetSize = (double)values[2];
            return (sv * targetSize) - (markerSize / 2.0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



}
