using System.DirectoryServices.ActiveDirectory;
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

namespace _20250415
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

        private void test_Click(object sender, RoutedEventArgs e)
        {
            var po = MyGeo.MyPoints;
            var inpo = MyGeo.MyInternalPoints;
            //MyGeo.MyInternalPoints.Add(new Point());
        }

        private void Bai_Click(object sender, RoutedEventArgs e)
        {
            var (x, y) = GetCenterXY(MyGeo.RenderedGeometry, MyGeo.MyPen);
            MyPointsTransform(new ScaleTransform(2, 2, x, y));
        }

        private void Half_Click(object sender, RoutedEventArgs e)
        {
            MyPointsTransform(new ScaleTransform(0.5, 0.5));
        }

        private void GetBounds_Click(object sender, RoutedEventArgs e)
        {
            var geo = MyGeo.RenderedGeometry;
            var geo2 = MyGeo.MyGeometry;
            var bounds = MyGeo.RenderedGeometry.GetRenderBounds(MyGeo.MyPen);

        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            var (x, y) = GetCenterXY(MyGeo.RenderedGeometry, MyGeo.MyPen);
            MyPointsTransform(new RotateTransform(15, x, y));
        }

        /// <summary>
        /// 中心座標
        /// </summary>
        /// <param name="geo"></param>
        /// <param name="pen"></param>
        /// <returns></returns>
        private static (double x, double y) GetCenterXY(Geometry geo, Pen pen)
        {
            var penBounds = geo.GetRenderBounds(pen);
            double x = penBounds.Width / 2.0 + penBounds.Left;
            double y = penBounds.Height / 2.0 + penBounds.Top;
            return (x, y);
        }

        private void MyPointsTransform(Transform transform)
        {
            for (int i = 0; i < MyGeo.MyPoints.Count; i++)
            {
                MyGeo.MyPoints[i] = transform.Transform(MyGeo.MyPoints[i]);
            }
        }


    }
}