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

namespace _20250409
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
            data.Points = [new Point(), new Point(200, 0)];
            data.Left = 200;
            data.Top = 200;
            var thumb = new GeoShapeThumb(data);
            return thumb;
        }

        private void handle_Click(object sender, RoutedEventArgs e)
        {
           if( MyGeoThumb.MyInsideElement is GeoShapeWithAnchorHandle geo)
            {
                geo.AnchorHandleSwitch();
            }
        }

        private void datacontext_Click(object sender, RoutedEventArgs e)
        {
            if(DataContext== MyThumb)
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
            var rtf = MyRect.RenderTransform;
            Rect r=new(0,0,MyRect.Width,MyRect.Height);
           var bounds = rtf.TransformBounds(r);
            RotateTransform ro = new(90);
            ScaleTransform scale = new(2, 1);
            TransformGroup group = new();
            group.Children.Add(scale);
            group.Children.Add(ro);
            var bounds2=group.TransformBounds(r);
            
            Rect bounds3 = ro.TransformBounds(r);
            bounds3= scale.TransformBounds(bounds3);

            Rect bounds4=scale.TransformBounds(r);
            bounds4 = ro.TransformBounds(bounds4);
        }
    }
}