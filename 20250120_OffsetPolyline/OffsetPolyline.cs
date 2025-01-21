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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250120_OffsetPolyline
{

    public class OffsetPolyline : Thumb
    {

        public Polyline MyPolyline
        {
            get { return (Polyline)GetValue(MyPolylineProperty); }
            set { SetValue(MyPolylineProperty, value); }
        }
        public static readonly DependencyProperty MyPolylineProperty =
            DependencyProperty.Register(nameof(MyPolyline), typeof(Polyline), typeof(OffsetPolyline), new PropertyMetadata(null));

        public Rect MyPolylineDescendantBounds
        {
            get { return (Rect)GetValue(MyPolylineDescendantBoundsProperty); }
            set { SetValue(MyPolylineDescendantBoundsProperty, value); }
        }
        public static readonly DependencyProperty MyPolylineDescendantBoundsProperty =
            DependencyProperty.Register(nameof(MyPolylineDescendantBounds), typeof(Rect), typeof(OffsetPolyline), new PropertyMetadata(null));

        static OffsetPolyline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OffsetPolyline), new FrameworkPropertyMetadata(typeof(OffsetPolyline)));
        }
        public OffsetPolyline()
        {
            DataContext = this;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_polyline") is Polyline polyline)
            {
                MyPolyline = polyline;
                MyPolyline.SizeChanged += MyPolyline_SizeChanged;
            }
        }

        private void MyPolyline_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(MyPolyline);
            MyPolylineDescendantBounds = bounds;
            Canvas.SetLeft(MyPolyline, -bounds.Left);
            Canvas.SetTop(MyPolyline, -bounds.Top);
            Width= bounds.Width;
            Height= bounds.Height;
        }
    }

    public class MyConverterOffsetLeft : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Rect rect = (Rect)value;
            return -rect.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MyConverterOffsetTop : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Rect rect = (Rect)value;
            return -rect.Top;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }









}
