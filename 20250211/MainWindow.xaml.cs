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

namespace _20250211
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
            //MyEz.AnchorsOn();
            if (AdornerLayer.GetAdornerLayer(MyEz.MyEzLine) is AdornerLayer layer)
            {
                layer.Add(new EzAdoner(MyEz.MyEzLine));
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MyEz.AnchorsOff();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MyEz.MyPoints[0] = new Point(-50, 0);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MyEz.MyPoints[0] = new Point(0, 0);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Point p = MyEz.MyPoints[0];
            Point np = new Point(p.X - 10, p.Y);
            MyEz.MyPoints[0] = np;
        }
    }
}
