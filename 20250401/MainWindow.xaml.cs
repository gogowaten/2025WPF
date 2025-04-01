using Microsoft.Windows.Themes;
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

namespace _20250401
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            OrangeBounds();
            BlueBounds();
            GreenBounds();
            WhiteBounds();
            RedBounds();

        }


        private void RedBounds()
        {
            var canvasMatrix = MyCanvas.RenderTransform.Value;
            var redMatrix = MyRed.RenderTransform.Value;
            Rect redR = new(0, 0, MyRed.Width, MyRed.Height);
            Point redPoi = new(GetLeft(MyRed), GetTop(MyRed));
            redPoi = canvasMatrix.Transform(redPoi);
            canvasMatrix.Append(redMatrix);
            MatrixTransform mtf = new(canvasMatrix);
            redR = mtf.TransformBounds(redR);
            MyMurasakiBounds.Width = redR.Width;
            MyMurasakiBounds.Height = redR.Height;
            SetLeft(MyMurasakiBounds, GetLeft(MyCanvas) + redR.Left + redPoi.X);
            SetTop(MyMurasakiBounds, GetTop(MyCanvas) + redR.Top + redPoi.Y);
        }

        private void WhiteBounds()
        {
            var canvasMatrix = MyCanvas.RenderTransform.Value;
            var redMatrix = MyRed.RenderTransform.Value;
            Rect redR = new(0, 0, MyRed.Width, MyRed.Height);
            canvasMatrix.Append(redMatrix);
            MatrixTransform mtf = new(canvasMatrix);
            redR = mtf.TransformBounds(redR);
            MyWhiteBounds.Width = redR.Width;
            MyWhiteBounds.Height = redR.Height;
            SetLeft(MyWhiteBounds, GetLeft(MyCanvas) + redR.Left);
            SetTop(MyWhiteBounds, GetTop(MyCanvas) + redR.Top);
        }

        private void OrangeBounds()
        {
            var redTF = MyRed.RenderTransform;
            Rect redR = new(0, 0, MyRed.Width, MyRed.Height);
            redR = redTF.TransformBounds(redR);
            MyOrangeBounds.Width = redR.Width;
            MyOrangeBounds.Height = redR.Height;

        }

        private void BlueBounds()
        {
            var canvasTF = MyCanvas.RenderTransform;
            var redTF = MyRed.RenderTransform;
            Rect redR = new(0, 0, MyRed.Width, MyRed.Height);
            redR = redTF.TransformBounds(redR);
            redR = canvasTF.TransformBounds(redR);
            MyBlueBounds.Width = redR.Width;
            MyBlueBounds.Height = redR.Height;

        }

        private void GreenBounds()
        {
            var canvasMatrix = MyCanvas.RenderTransform.Value;
            var redMatrix = MyRed.RenderTransform.Value;
            Rect redR = new(0, 0, MyRed.Width, MyRed.Height);
            canvasMatrix.Append(redMatrix);
            MatrixTransform mtf = new(canvasMatrix);
            redR = mtf.TransformBounds(redR);
            MyGreenBounds.Width = redR.Width;
            MyGreenBounds.Height = redR.Height;

        }



        private void SetLeft(FrameworkElement element, double left) => Canvas.SetLeft(element, left);
        private double GetLeft(FrameworkElement element) => Canvas.GetLeft(element);

        private void SetTop(FrameworkElement element, double top) => Canvas.SetTop(element, top);
        private double GetTop(FrameworkElement element) => Canvas.GetTop(element);
    }
}