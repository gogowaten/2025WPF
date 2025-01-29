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

namespace _20250129
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

        public void Bounds()
        {
            var geoTF = (RotateTransform)MyLine.RenderTransform;
            var geoTFe = MyLine.GeometryTransform;
            var geoT = MyLine.Data.Clone();
            var geo = (PathGeometry)MyLine.RenderedGeometry.Clone();
            tbAngle.Text = $"angle = {geoTF.Angle.ToString()}";
            var pen = new Pen(Brushes.Red, MyLine.StrokeThickness);
            var bounds = MyLine.Data.GetWidenedPathGeometry(pen).Bounds;
            var penRect = MyLine.Data.GetRenderBounds(pen);
            var motoRect = MyLine.Data.Bounds;
            var centerX = motoRect.Width / 2;
            var centerY = motoRect.Height / 2;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Bounds();

        }
    }
}