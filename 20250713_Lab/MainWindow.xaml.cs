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
}