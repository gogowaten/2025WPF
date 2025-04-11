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
            data.Points = [new Point(), new Point(100, 00)];
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
                Rect GRB = geo.MyGeometryRenderBounds;
                Rect GRBWP = geo.MyGeometryRenderBoundsWithPen;
                Rect STB = geo.MyShapeTransformedBounds;
                Geometry Geo = geo.MyGeometry;
                Geometry RG = geo.RenderedGeometry;
                Rect RGB = RG.Bounds;
                Rect RGRBP = RG.GetRenderBounds(geo.MyPen);
                ItemData data = MyGeoThumb.MyItemData;
                double x = GRBWP.Left + GRBWP.Width * data.CenterX;
                double y = GRBWP.Top + GRBWP.Height * data.CenterY;
                TransformGroup transform = new();
                transform.Children.Add(new ScaleTransform(data.ScaleX, data.ScaleY, x, y));
                transform.Children.Add(new RotateTransform(data.Angle, x, y));
                Geometry clone = geo.RenderedGeometry.Clone();
                Geometry clone2 = clone.Clone();
                clone.Transform = transform;
                Rect cloneBounds = clone.Bounds;
                Rect clonePenBouds = clone.GetRenderBounds(geo.MyPen);
                clone2.Transform = geo.MyRenderTransform;
                Rect clone2PenBounds = clone2.GetRenderBounds(geo.MyPen);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //RotateTransform ro = new(10, 0, 0);
            //for (int i = 0; i < MyPoly.Points.Count; i++)
            //{
            //    MyPoly.Points[i] = ro.Transform(MyPoly.Points[i]);
            //}
            if (MyGeoThumb.MyInsideElement is GeoShape shape)
            {
                ItemData data = MyGeoThumb.MyItemData;
                RotateTransform ro = new(data.Angle);
                for (int i = 0; i < data.Points.Count; i++)
                {
                    data.Points[i]=ro.Transform(data.Points[i]);
                }

            }
        }
    }
}