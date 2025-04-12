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

namespace _20250412
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
            MyShape.AnchorSwitch();
        }

        private void Bounds_Click(object sender, RoutedEventArgs e)
        {
            var shape = MyShape.MyGeoShape;
            var geo = shape.RenderedGeometry;
            var clone = geo.Clone();
            clone.Transform = shape.RenderTransform;
            var bounds = clone.GetRenderBounds(shape.MyPen);

            var geoBounds = shape.MyGeometryRenderBounds;
            var cx = MyShape.MyCenterX; var cy = MyShape.MyCenterY;
            var x = geoBounds.Width * cx + geoBounds.Left;
            var y = geoBounds.Height * cy + geoBounds.Top;
            ScaleTransform sc = new(MyShape.MyScaleX, MyShape.MyScaleY, x, y);
            RotateTransform ro = new(MyShape.MyAngle, x, y);
            clone.Transform = sc;
            Rect boundsPen = shape.RenderedGeometry.GetRenderBounds(shape.MyPen);
            var geoBoundsSc = sc.TransformBounds(boundsPen);
            var geoBoundsRo = ro.TransformBounds(geoBoundsSc);
        }
    }
}