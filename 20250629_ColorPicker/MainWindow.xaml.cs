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
        public MainWindow()
        {
            InitializeComponent();
            MySikisai = new Sikisai();
            MySikisai = new Sikisai(Colors.Magenta);
            MySikisaiBrush = new SikisaiBrush(Brushes.Magenta);
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var neko = MySikisai;
            var inu = MySikisaiBrush;
        }
    }



}