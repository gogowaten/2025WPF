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

namespace _20250629_ColorPicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Sikisai MySikisai { get; set; }
        public SikisaiBrush MySikisaiBrush { get; set; }
        //public MarkerAdorner MyMarkerAdorner { get; set; } = null!;
        public MainWindow()
        {
            InitializeComponent();
            MySikisai = new Sikisai();
            MySikisai = new Sikisai(Colors.Magenta);
            MySikisaiBrush = new SikisaiBrush(Brushes.Magenta);
            Loaded += MainWindow_Loaded;
            DataContext = this;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Adorner  Layer   結果
            // Viewbox  Viewbox おｋ
            // Viewbox  Image   おｋ
            // Image    Viewbox クソデカMarker
            // Image    Image   クソデカMarker

            MyMarkerAdorner = new MarkerAdorner(MyViewbox);

            // AdornerLayerを取得して、MarkerAdornerを設置
            if (AdornerLayer.GetAdornerLayer(MyViewbox) is AdornerLayer layer)
            {
                layer.Add(MyMarkerAdorner);
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var neko = MySVImageWithMarker.MySikisai;
            var inu = MySikisaiBrush;
            var svah = MySVImage.ActualHeight;
            if (AdornerLayer.GetAdornerLayer(MySVImage) is AdornerLayer layer)
            {
                var ah = layer.ActualHeight;
            }
        }



        public MarkerAdorner MyMarkerAdorner
        {
            get { return (MarkerAdorner)GetValue(MyMarkerAdornerProperty); }
            set { SetValue(MyMarkerAdornerProperty, value); }
        }
        public static readonly DependencyProperty MyMarkerAdornerProperty =
            DependencyProperty.Register(nameof(MyMarkerAdorner), typeof(MarkerAdorner), typeof(MainWindow), new PropertyMetadata(null));

    }



}