using System.Configuration;
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

namespace _20250401_01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private Rect MyRedBounds;
        public MainWindow()
        {
            InitializeComponent();

            //MyRedBounds = new Rect(GetLeft(MyRed), GetTop(MyRed), MyRed.Width, MyRed.Height);


            Test1();
        }

        private void Test1()
        {
            Rect r = MyRed.RenderTransform.TransformBounds(new Rect(0, 0, MyRed.Width, MyRed.Height));
            r.Offset(GetLeft(MyRed), GetTop(MyRed));
            MySetBounds(MyBlackWaku, r);
        }






        private Point GetPoint(FrameworkElement element) => new Point(Canvas.GetLeft(element), Canvas.GetTop(element));
        private double GetLeft(FrameworkElement element) => Canvas.GetLeft(element);
        private double GetTop(FrameworkElement element) => Canvas.GetTop(element);
        private void SetLeft(FrameworkElement element, double value) => Canvas.SetLeft(element, value);
        private void SetTop(FrameworkElement element, double value) => Canvas.SetTop(element, value);
        private void MySetTopLeft(FrameworkElement element, Point poi)
        {
            SetLeft(element, poi.X);
            SetTop(element, poi.Y);
        }
        private void MySetBounds(FrameworkElement element, Rect bounds)
        {
            SetLeft(element, bounds.Left);
            SetTop(element, bounds.Top);
            element.Width= bounds.Width;
            element.Height= bounds.Height;
        }
    }
}