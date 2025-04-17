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

namespace _20250416
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KisoThumb MyThumb { get; set; } = null!;
        GeoShapeThumb MyThumb2 { get; set; } = null!;
        public MainWindow()
        {
            InitializeComponent();
            MyThumb = Test1();
            MyThumb2 = Test2();
            DataContext = MyThumb2;

        }

        private TextBlockThumb Test1()
        {
            ItemData data = new(ItemType.TextBlock)
            {
                MyText = "test",
                MyLeft = 100,
                MyTop = 100
            };
            var thumb = new TextBlockThumb(data);
            MyCanvas.Children.Add(thumb);
            return thumb;
        }
        private GeoShapeThumb Test2()
        {
            ItemData data = new(ItemType.GeoShape)
            {
                MyPoints = [new Point(), new Point(100, 0)],
                StrokeThickness = 40,
                MyLeft = 200,
                MyTop = 200
            };
            var thumb = new GeoShapeThumb(data);
            MyCanvas.Children.Add(thumb);
            return thumb;
        }

        private void Handle_Click(object sender, RoutedEventArgs e)
        {
            if (MyThumb2.MyInsideElement is GeoShapeWithAnchorHandle geo)
            {
                geo.AnchorHandleSwitch();
            }
            //if(MyThumb2.MyInsideElement is GeoShapePanel panel)
            //{
            //    panel.MyGeoShapeWithAnchor.AnchorHandleSwitch();
            //}
        }

        private void SetScaleX_Click(object sender, RoutedEventArgs e)
        {
            var v = MyScaleX.Value;
            MyThumb2.SetScaleX(v);
        }

        private void GeoTest_Click(object sender, RoutedEventArgs e)
        {
            MyThumb2.GeoTest();
        }
    }
}