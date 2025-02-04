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

namespace _20250204
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //this.VisualTextRenderingMode = TextRenderingMode.Aliased;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GeneralTransform transformR = nemo1.TransformToVisual(MyCanvas);
            var ez = transformR.TransformBounds(new Rect(nemo1.MyBounds.Size));
            var ez2 = transformR.TransformBounds(new Rect(nemo1.RenderSize));

            //var tf = MyRect2.TransformToVisual(MyCanvas2);
            //var tf2 = MyRect2.TransformToAncestor(MyCanvas2);
            ////var tf3 = MyRect2.TransformToDescendant(MyCanvas2);

            //var R = tf.TransformBounds(new Rect(0, 0, MyRect2.ActualWidth, MyRect2.ActualHeight));
            //LeftShift(MyCanvas2, R.X);
            //TopShift(MyCanvas2, R.Y);
            //LeftShift(MyRect2,-R.X);
            //TopShift(MyRect2, -R.Y);
            //MyCanvas2.Width = R.Width;
            //MyCanvas2.Height = R.Height;
        }
        private void LeftShift(FrameworkElement element,double left)
        {
            Canvas.SetLeft(element,Canvas.GetLeft(element) + left);
        }
        private void TopShift(FrameworkElement element,double top)
        {
            Canvas.SetTop(element,Canvas.GetTop(element) + top);
        }
    }
}