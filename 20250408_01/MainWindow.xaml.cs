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
           //var poi= MyShape0.MyPoints;
           //var tr = MyShape0.GeometryTransform;
           //var ttr = MyShape0.RenderedGeometry.Transform;
           //var angle = MyShape0.MyAngle2;
           // MyShape0.MyAngle2 = 20;
           // MyShape0.MyPoints.Add(new Point(300, 100));
        }
    }
}