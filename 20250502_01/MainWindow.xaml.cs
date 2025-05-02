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

    //[MarkupExtensionReturnType(typeof(LinearGradientBrush))]
    //public class AbsoluteBrushExtension : MarkupExtension
    //{
    //    //public FrameworkElement Element { get; set; } = null!;
    //    public Color StartColor { get; set; }
    //    public Color EndColor { get; set; }
    //    public double Width { get; set; }

    //    public AbsoluteBrushExtension() { }
    //    public AbsoluteBrushExtension( Color start, Color end, double width)
    //    {
    //        Width = width;
    //        StartColor = start; EndColor = end;

    //    }
    //    public override object ProvideValue(IServiceProvider serviceProvider)
    //    {            
    //        LinearGradientBrush brush = new(StartColor, EndColor, new Point(), new Point(Width, 0));
    //        brush.MappingMode = BrushMappingMode.Absolute;
    //        return brush;
    //    }
    //}


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
        Color c1;
        Color c2;
        FrameworkElement fe;

        public MyAbsBrush(Color c1,Color c2, FrameworkElement fe)
        {
            this.c1 = c1;
            this.c2 = c2;
            this.fe = fe;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var end = (FrameworkElement)value;
            LinearGradientBrush brush = new(c1, c2, new Point(), new Point(100, 0));
            brush.MappingMode = BrushMappingMode.Absolute;
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