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

namespace _20250503_01
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FormattedText text = new(MyText.Text, System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(FontFamily.Source), 30.0, Brushes.Red, VisualTreeHelper.GetDpi(this).PixelsPerInchY);
            Geometry geo = text.BuildHighlightGeometry(new Point());
            MyRectangle.Width = geo.Bounds.Width;
            MyRectangle.Height = geo.Bounds.Height;
            geo = text.BuildGeometry(new Point());
            MyRectangle.Clip = geo;

        }
    }
}