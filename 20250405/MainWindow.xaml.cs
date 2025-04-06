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

namespace _20250405
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GeoShapeThumb MyGeoShapeThumb { get; set; } = null!;
        public MainWindow()
        {
            InitializeComponent();
            Test1();
            DataContext = MyGeoShapeThumb;

        }


        public void Test1()
        {
            ItemData data = new()
            {
                MyLeft = 100,
                MyTop = 100,
                MyPoints = [new Point(00, 0), new Point(200, 0)],
                MyAngle = 20,
            };

            MyGeoShapeThumb = new(data);
            MyGeoShapeThumb.Opacity = 0.5;
            
            MyCanvas.Children.Add(MyGeoShapeThumb);
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            //geo.AnchorHandleOn();
            //MyGeoShapeThumb.MyInsideGeoShape.MyPointReset();
            Geometry geo =MyGeoShapeThumb.MyInsideGeoShape.RenderedGeometry;
            RotateTransform rotate = new(20);
            var clone = geo.Clone();
            geo.Transform = rotate;
           var bounds = geo.GetRenderBounds(MyGeoShapeThumb.MyInsideGeoShape.MyPen);
        }
    }
}