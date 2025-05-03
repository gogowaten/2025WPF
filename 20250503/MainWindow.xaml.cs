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

namespace _20250503
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            (Geometry textHighlightGeo, Geometry textGeo) = GetTextGeometry("aaaaaaaa");
            Rectangle re = new()
            {
                Width = textHighlightGeo.Bounds.Width,
                Height = textHighlightGeo.Bounds.Height,
                Fill = new LinearGradientBrush(Colors.Cyan, Colors.Magenta, new Point(), new Point(1, 1)),
                Clip = textGeo,
            };
            MyCanvas.Children.Add(re);
        }

        private (Geometry textHighlightGeo, Geometry textGeo) GetTextGeometry(string text)
        {
            //dpi取得は
            DpiScale dpi = VisualTreeHelper.GetDpi(this);

            var ftext = new FormattedText(text, System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(FontFamily.Source), 50.0, Brushes.Red, dpi.PixelsPerInchX);

            //行間考慮した文字列描画のサイズ取得はこれ、1文字ごとのサイズも取得できる
            Geometry highGeo = ftext.BuildHighlightGeometry(new Point());

            //文字の輪郭のGeometry
            Geometry geo = ftext.BuildGeometry(new Point());
            return (highGeo, geo);
        }
    }
}