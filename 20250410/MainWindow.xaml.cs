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

namespace _20250410
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KisoThumb MyThumb { get; set; } = null!;
        KisoThumb MyGeoThumb { get; set; } = null!;
        public MainWindow()
        {
            InitializeComponent();
            MyThumb = Test1();
            MyCanvas.Children.Add(MyThumb);
            DataContext = MyThumb;
            MyGeoThumb = Test2();
            MyCanvas.Children.Add(MyGeoThumb);
            DataContext = MyGeoThumb;
        }

        private TextBlockThumb Test1()
        {
            ItemData data = new(ItemType.TextBlock);
            data.Text = "test";
            data.Left = 100;
            data.Top = 100;
            var thumb = new TextBlockThumb(data);
            return thumb;
        }
        private GeoShapeThumb Test2()
        {
            ItemData data = new(ItemType.GeoShape);
            data.Text = "geo";
            data.Points = [new Point(), new Point(100, 100)];
            data.Left = 200;
            data.Top = 200;
            var thumb = new GeoShapeThumb(data);
            return thumb;
        }

        private void handle_Click(object sender, RoutedEventArgs e)
        {
            if (MyGeoThumb.MyInsideElement is GeoShapeWithAnchorHandle geo)
            {
                geo.AnchorHandleSwitch();
            }
        }

        private void datacontext_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext == MyThumb)
            {
                DataContext = MyGeoThumb;
            }
            else
            {
                DataContext = MyThumb;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //MyGeoThumb.MyItemData.Points[0] = new Point(100, 0);
            if (MyGeoThumb.MyInsideElement is GeoShapeWithAnchorHandle geo)
            {
                var geoGetRenderB = geo.GetRenderBounds();
                var renderdGeo = geo.RenderedGeometry.Bounds;
                var myGeoGeo = geo.MyGeometry;
                var myGeoRender = geo.MyGeometryRenderBounds;//geo.Bounds;
                var myGeoRenderPen = geo.MyGeometryRenderBoundsWithPen;//geo.GetRenderBounds(MyPen);
                var transform = geo.MyRenderTransform;
                var insideWidth = MyGeoThumb.MyInsideElement.ActualWidth;
                //geo.PointsMoveToTopLeft();
                //geo.MyAnchorHandleAdorner?.HandlesLocateToPoints();
            }
        }
    }
}