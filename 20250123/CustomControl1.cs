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

namespace _20250123
{

    public class LineThumb : Thumb
    {

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(LineThumb), new PropertyMetadata(null));


        public Brush MyStroke
        {
            get { return (Brush)GetValue(MyStrokeProperty); }
            set { SetValue(MyStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeProperty =
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(LineThumb), new PropertyMetadata(null));


        public double MyStrokeThickness
        {
            get { return (double)GetValue(MyStrokeThicknessProperty); }
            set { SetValue(MyStrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeThicknessProperty =
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(LineThumb), new PropertyMetadata(1.0));

        //読み取り専用にしたほうがいい
        public ExLine MyExLine
        {
            get { return (ExLine)GetValue(MyExLineProperty); }
            set { SetValue(MyExLineProperty, value); }
        }
        public static readonly DependencyProperty MyExLineProperty =
            DependencyProperty.Register(nameof(MyExLine), typeof(ExLine), typeof(LineThumb), new PropertyMetadata(null));

        static LineThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LineThumb), new FrameworkPropertyMetadata(typeof(LineThumb)));
        }
        public LineThumb()
        {
            DataContext = this;
            Loaded += LineThumb_Loaded;
            DragDelta += LineThumb_DragDelta;
        }

        private void LineThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is LineThumb t)
            {
                Canvas.SetLeft(t, Canvas.GetLeft(t) + e.HorizontalChange);
                Canvas.SetTop(t, Canvas.GetTop(t) + e.VerticalChange);
                e.Handled = true;
            }
        }

        private void LineThumb_Loaded(object sender, RoutedEventArgs e)
        {
            if (GetTemplateChild("PART_ExLine") is ExLine line)
            {
                MyExLine = line;
                SetBinding(WidthProperty, new Binding() { Source = MyExLine, Path = new PropertyPath(ExLine.MyBoundsProperty), Converter = new MyConverterBoundsToWidth() });
                SetBinding(HeightProperty, new Binding() { Source = MyExLine, Path = new PropertyPath(ExLine.MyBoundsProperty), Converter = new MyConverterBoundsToHeight() });

            }
        }

    }

    public class MyConverterBoundsToHeight : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Rect r = (Rect)value;
            return r.Height;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConverterBoundsToWidth : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Rect r = (Rect)value;
            return r.Width;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
