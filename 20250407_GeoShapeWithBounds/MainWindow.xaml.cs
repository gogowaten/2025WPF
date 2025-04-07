using System.Globalization;
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

namespace _20250407_GeoShapeWithBounds
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MyShape.MyHeadEndType == HeadType.Arrow)
            {
                MyShape.MyHeadEndType = HeadType.None;
                MyShape0.MyHeadEndType = HeadType.None;
            }
            else
            {
                MyShape.MyHeadEndType = HeadType.Arrow;
                MyShape0.MyHeadEndType = HeadType.Arrow;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(MyShape0.MyShapeType == ShapeType.Line)
            {
                MyShape0.MyShapeType = ShapeType.Bezier;
                MyShape.MyShapeType = ShapeType.Bezier;
            }
            else
            {
                MyShape0.MyShapeType = ShapeType.Line;
                MyShape.MyShapeType = ShapeType.Line;
            }
        }
    }

}