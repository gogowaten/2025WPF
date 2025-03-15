using System.Windows;
using System.Windows.Controls.Primitives;

namespace _20250315_ResizeAdorner
{
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
}
