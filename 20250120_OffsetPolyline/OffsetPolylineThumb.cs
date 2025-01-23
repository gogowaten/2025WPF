using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _20250120_OffsetPolyline
{

    public abstract class KisoPolyThumb : Thumb
    {
        #region 依存関係プロパティ

        public Polyline MyPolyline
        {
            get { return (Polyline)GetValue(MyPolylineProperty); }
            set { SetValue(MyPolylineProperty, value); }
        }
        public static readonly DependencyProperty MyPolylineProperty =
            DependencyProperty.Register(nameof(MyPolyline), typeof(Polyline), typeof(KisoPolyThumb), new PropertyMetadata(null));

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(KisoPolyThumb), new PropertyMetadata(null));

        public double MyStrokeThickness
        {
            get { return (double)GetValue(MyStrokeThicknessProperty); }
            set { SetValue(MyStrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeThicknessProperty =
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(KisoPolyThumb), new PropertyMetadata(1.0));
        #endregion 依存関係プロパティ

        static KisoPolyThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KisoPolyThumb), new FrameworkPropertyMetadata(typeof(KisoPolyThumb)));
        }
        public KisoPolyThumb()
        {
            DragDelta += KisoPolyThumb_DragDelta;
        }

        //マウスドラッグ移動
        private void KisoPolyThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is KisoPolyThumb t)
            {
                Canvas.SetLeft(t, Canvas.GetLeft(t) + e.HorizontalChange);
                Canvas.SetTop(t, Canvas.GetTop(t) + e.VerticalChange);
                e.Handled = true;
            }
        }

        //Polylineのサイズ変更時に
        //Polylineのオフセットと自身(Thumb)のサイズ変更
        private void MyPolyline_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(MyPolyline);
            Canvas.SetLeft(MyPolyline, -bounds.Left);
            Canvas.SetTop(MyPolyline, -bounds.Top);
            Width = bounds.Width;
            Height = bounds.Height;
        }

        //起動時のTemplate適用後に
        //TemplateからPolylineを取得
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_polyline") is Polyline polyline)
            {
                MyPolyline = polyline;
                MyPolyline.SizeChanged += MyPolyline_SizeChanged;
            }
        }

    }



    public class OffsetPolylineThumb : KisoPolyThumb
    {
        static OffsetPolylineThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OffsetPolylineThumb), new FrameworkPropertyMetadata(typeof(OffsetPolylineThumb)));
        }
        public OffsetPolylineThumb()
        {
            DataContext = this;
        }
    }


    public class PolylineThumb : KisoPolyThumb
    {
        static PolylineThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PolylineThumb), new FrameworkPropertyMetadata(typeof(PolylineThumb)));
        }
        public PolylineThumb()
        {
            DataContext = this;
        }
    }

}
