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

namespace _20250406
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
            //図形の見た目上での中心を軸に回転するRotateTransformを作成(取得)する
            Rect geometryBounds = MyPoly.RenderedGeometry.Bounds;
            double boundsCenterX = geometryBounds.Width * 0.5;//0.5で中心になる。0なら左上になる
            double centerX = boundsCenterX + geometryBounds.Left;
            double boundsCenterY = geometryBounds.Height * 0.5;
            double centerY = boundsCenterY + geometryBounds.Top;
            RotateTransform rotate = new(45, centerX, centerY);
            MyPoly.RenderTransform = rotate;

            //作成したRotateTransformを利用して
            //図形の見た目上でのBoundsを取得する
            Pen myPen = new(Brushes.Transparent, MyPoly.StrokeThickness);
            Rect geometryRenderBounds = MyPoly.RenderedGeometry.GetRenderBounds(myPen);
            Rect transformedGeometryRenderBounds = rotate.TransformBounds(geometryRenderBounds);
        }
    }
}