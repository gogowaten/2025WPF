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
            double x = MyGeo.RenderedGeometry.Bounds.Width / 2.0;
            double y = MyGeo.RenderedGeometry.Bounds.Height / 2.0;
            ScaleTransform sc = new(2, 2,x,y);
            for (int i = 0; i < MyGeo.MyPoints.Count; i++)
            {
                Point poi = MyGeo.MyPoints[i];
                poi = sc.Transform(poi);
                MyGeo.MyPoints[i] = poi;
            }
        }

        private void Half_Click(object sender, RoutedEventArgs e)
        {
            ScaleTransform sc = new(0.5, 0.5);
            for (int i = 0; i < MyGeo.MyPoints.Count; i++)
            {
                //var poi = MyGeo.MyPoints[i];
                //poi = sc.Transform(poi);
                //MyGeo.MyPoints[i] = poi;
                MyGeo.MyPoints[i]=sc.Transform(MyGeo.MyPoints[i]);
            }
        }

        private void GetBounds_Click(object sender, RoutedEventArgs e)
        {
           var geo = MyGeo.RenderedGeometry;
           var geo2= MyGeo.MyGeometry;
           var bounds = geo.GetRenderBounds(MyGeo.MyPen);

        }
    }
}