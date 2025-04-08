using System.Windows;
//WPF、昨日のは失敗だった、変形時にPoint変化で全体が移動してしまう - 午後わてんのブログ
//https://gogowaten.hatenablog.com/entry/2025/04/08/133930
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
            MyShape0.MyPoints[1] = new Point(200, 000);
        }
    }
}