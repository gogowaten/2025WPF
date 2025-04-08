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

namespace _20250408
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

        private void Test1()
        {
            MyShape0.AnchorSwitch();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Test1();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MyShape0.MyPoints[1] = new Point(300, 100);
        }
    }
}