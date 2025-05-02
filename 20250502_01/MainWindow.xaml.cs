using System.Globalization;
using System.Numerics;
using System.Text;
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

namespace _20250502_01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }


    public class MyEndPoint : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var end = (double)value;
            return new Point(end, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            //return new MyEndPoint();
            return this;
        }
    }

    public class MyEndPoint2 : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var end = (FrameworkElement)value;
            return new Point(end.ActualWidth, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            //return new MyEndPoint();
            return this;
        }
    }

    public class MyAbsBrush : MarkupExtension, IValueConverter
    {
        public Color C1 { get; set; } = Colors.White;
        public Color C2 { get; set; } = Colors.Blue;
        //public FrameworkElement fe { get; set; }

        public MyAbsBrush() { }
        public MyAbsBrush(Color c1, Color c2)
        {
            this.C1 = c1;
            this.C2 = c2;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //var end = (FrameworkElement)value;
            var end = (double)value;
            LinearGradientBrush brush = new(C1, C2, new Point(), new Point(end, 0))
            {
                MappingMode = BrushMappingMode.Absolute
            };
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            //return new MyEndPoint();
            return this;
        }
    }

}