using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250703HSV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += MainWindow_Loaded;
            //var sa1 = Sikisa.Ciede2000(240, 110, 80, 240, 160, 30);
            //var sa2 = Sikisa.Ciede2000(150, 250, 30, 100, 250, 80);
            //var sa31 = Sikisa.Ciede2000(200, 77, 121, 171, 49, 96);
            //var sa32 = Sikisa.Ciede2000(200, 77, 121, 204, 77, 105);

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MultiBinding mb;
            mb = new() { Converter = new MyConvRGBBrush() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyRProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyGProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyBProperty) });
            SetBinding(MyBrushProperty, mb);

            //mb = new() { Converter = new MyConvRGBBrush() };
            //mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyRProperty) });
            //mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyGProperty) });
            //mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyBProperty) });
            //SetBinding(MyBrushProperty, mb);



        }





        private static void OnChangedHSV(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MainWindow w)
            {
                (w.MyR, w.MyG, w.MyB) = MathHSV.Hsv2rgb(w.MyHue, w.MySat, w.MyVal);
            }
        }

        private static void OnChangedRGB(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MainWindow mw)
            {
                //(mw.MyHue, mw.MySat, mw.MyVal) = MathHSV.Rgb2hsv(mw.MyR, mw.MyG, mw.MyB);
            }
        }


        public Brush MyBrush
        {
            get { return (Brush)GetValue(MyBrushProperty); }
            set { SetValue(MyBrushProperty, value); }
        }
        public static readonly DependencyProperty MyBrushProperty =
            DependencyProperty.Register(nameof(MyBrush), typeof(Brush), typeof(MainWindow), new PropertyMetadata(null));

        public double MyHue
        {
            get { return (double)GetValue(MyHueProperty); }
            set { SetValue(MyHueProperty, value); }
        }
        public static readonly DependencyProperty MyHueProperty =
            DependencyProperty.Register(nameof(MyHue), typeof(double), typeof(MainWindow), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnChangedHSV)));

        public double MySat
        {
            get { return (double)GetValue(MySatProperty); }
            set { SetValue(MySatProperty, value); }
        }
        public static readonly DependencyProperty MySatProperty =
            DependencyProperty.Register(nameof(MySat), typeof(double), typeof(MainWindow), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnChangedHSV)));

        public double MyVal
        {
            get { return (double)GetValue(MyValProperty); }
            set { SetValue(MyValProperty, value); }
        }
        public static readonly DependencyProperty MyValProperty =
            DependencyProperty.Register(nameof(MyVal), typeof(double), typeof(MainWindow), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnChangedHSV)));

        public byte MyR
        {
            get { return (byte)GetValue(MyRProperty); }
            set { SetValue(MyRProperty, value); }
        }
        public static readonly DependencyProperty MyRProperty =
            DependencyProperty.Register(nameof(MyR), typeof(byte), typeof(MainWindow), new FrameworkPropertyMetadata((byte)0, new PropertyChangedCallback(OnChangedRGB)));

        public byte MyG
        {
            get { return (byte)GetValue(MyGProperty); }
            set { SetValue(MyGProperty, value); }
        }
        public static readonly DependencyProperty MyGProperty =
            DependencyProperty.Register(nameof(MyG), typeof(byte), typeof(MainWindow), new FrameworkPropertyMetadata((byte)0, new PropertyChangedCallback(OnChangedRGB)));

        public byte MyB
        {
            get { return (byte)GetValue(MyBProperty); }
            set { SetValue(MyBProperty, value); }
        }
        public static readonly DependencyProperty MyBProperty =
            DependencyProperty.Register(nameof(MyB), typeof(byte), typeof(MainWindow), new FrameworkPropertyMetadata((byte)0, new PropertyChangedCallback(OnChangedRGB)));

    }




    public class MyConvHSVBrush : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var h = (double)values[0];
            var s = (double)values[1];
            var v = (double)values[2];
            var (r, g, b) = MathHSV.Hsv2rgb(h, s, v);
            return new SolidColorBrush(Color.FromRgb(r, g, b));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



    public class MyConvRGBBrush : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (byte)values[0];
            var g = (byte)values[1];
            var b = (byte)values[2];
            return new SolidColorBrush(Color.FromRgb(r, g, b));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }





}