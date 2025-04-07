using System.Windows;

//WPF、回転した子要素の中の回転した複数子要素がピッタリ収まるBoundsの取得 - 午後わてんのブログ
//https://gogowaten.hatenablog.com/entry/2025/04/03/121228

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