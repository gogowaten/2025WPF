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

namespace _20250206_RotateEzLineThumb
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
            Random r = new();
            Point p = new(r.Next(200), r.Next(200));
            MyEz.MyPoints.Add(p);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (MyEz.MyPoints.Count > 0)
            {
                MyEz.MyPoints.RemoveAt(0);
            }
        }
    }
}