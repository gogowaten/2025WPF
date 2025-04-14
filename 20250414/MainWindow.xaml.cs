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

namespace _20250414
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Test();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var size = MyRectRed.RenderSize;
            var rt = MyRectRed.RenderTransform;
            var RR = MyRectRed.RenderTransform.TransformBounds(new Rect(size));

            var polysize = MyPoly.RenderSize;
            var PR = MyPoly.RenderTransform.TransformBounds(new Rect(polysize));
            var geo = MyPoly.RenderedGeometry;
            var neko = MyPoly.GeometryTransform.TransformBounds(new Rect(polysize));
            var inu = MyPoly.GeometryTransform.TransformBounds(new Rect(new Size(50,50)));
            var tako = MyPoly.RenderTransform.TransformBounds(new Rect(0, 0, 50, 50));

        }

        //private void Test()
        //{
        //    RotateTransform rotate = new(20);
        //    ScaleTransform scale = new(2, 1);

        //    TransformGroup transform = new();
        //    transform.Children.Add(rotate);
        //    transform.Children.Add(scale);
        //    MyRectBlue.RenderTransform = transform;

        //    transform = new();
        //    transform.Children.Add(scale);
        //    transform.Children.Add(rotate);
        //    MyRectCyan.RenderTransform = transform;

        //}
    }
}