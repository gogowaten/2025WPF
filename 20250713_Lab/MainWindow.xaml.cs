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

            //double v = 3.0;
            //double x = Math.Pow(v, 1.0 / 3.0);// 1.4422495703074083
            //double y = Math.Pow(x, 3.0);// 2.99999999999999
            //double neko = Math.Pow(29.0 / 3.0, 3.0);// 903.29629629629608
            //double f = 1.0 / 3.0 * Math.Pow(29.0 / 6.0, 2.0);// 7.7870370370370354
            //double ff = 4.0 / 29.0;// 0.13793103448275862


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var data = MySikisai;
        }
    }







}