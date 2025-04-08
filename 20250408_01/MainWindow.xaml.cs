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

namespace _20250408_01
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
            MyShape0.AnchorHandleSwitch();
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var bounds = MyShape0.GetRenderBounds();
            waku.Width = bounds.Width;
            waku.Height = bounds.Height;
            Canvas.SetLeft(waku, bounds.Left);
            Canvas.SetTop(waku, bounds.Top);

            MyPoly.Points = [new Point(100, 100), new Point(300, 100)];
        }
    }
}