using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _20250121_PseudoBindingPointCollection
{


    public abstract class KisoPolylineThumb : Thumb
    {

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(KisoPolylineThumb), new PropertyMetadata(null));


        public Polyline MyPolyline
        {
            get { return (Polyline)GetValue(MyPolylineProperty); }
            set { SetValue(MyPolylineProperty, value); }
        }
        public static readonly DependencyProperty MyPolylineProperty =
            DependencyProperty.Register(nameof(MyPolyline), typeof(Polyline), typeof(KisoPolylineThumb), new PropertyMetadata(null));


        static KisoPolylineThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KisoPolylineThumb), new FrameworkPropertyMetadata(typeof(KisoPolylineThumb)));
        }
        public KisoPolylineThumb()
        {
            DragDelta += KisoPolylineThumb_DragDelta;
        }

        private void KisoPolylineThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is KisoPolylineThumb t)
            {
                Canvas.SetLeft(t, Canvas.GetLeft(t) + e.HorizontalChange);
                Canvas.SetTop(t, Canvas.GetTop(t) + e.VerticalChange);
                e.Handled = true;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_Polyline") is Polyline element)
            {
                MyPolyline = element;
            }
        }
    }

    public class PolylineThumb1 : KisoPolylineThumb
    {
        static PolylineThumb1()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PolylineThumb1), new FrameworkPropertyMetadata(typeof(PolylineThumb1)));
        }
        public PolylineThumb1()
        {
            Loaded += PolylineThumb1_Loaded;
        }

        private void PolylineThumb1_Loaded(object sender, RoutedEventArgs e)
        {
            MyPoints = MyPolyline.Points;
        }
    }


    public class PolylineThumb2 : KisoPolylineThumb
    {
        static PolylineThumb2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PolylineThumb2), new FrameworkPropertyMetadata(typeof(PolylineThumb2)));
        }
        public PolylineThumb2()
        {

        }
    }



    public class PolylineThumb3 : KisoPolylineThumb
    {
        static PolylineThumb3()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PolylineThumb3), new FrameworkPropertyMetadata(typeof(PolylineThumb3)));
        }
        public PolylineThumb3()
        {
            DataContext = this;
        }
    }


}
