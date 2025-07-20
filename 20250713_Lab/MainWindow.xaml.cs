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

namespace _20250713_Lab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Sikisai MySikisai { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            MySikisai = new();
            DataContext = this;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var data = MySikisai;
        }
    }




    public class MyConvColorBrush : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (byte)values[0];
            var g = (byte)values[1];
            var b = (byte)values[2];
            return new SolidColorBrush(Color.FromArgb(255, r, g, b));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}