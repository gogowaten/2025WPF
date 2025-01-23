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

namespace _20250123
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //public PointCollection MyPointCollection
        //{
        //    get { return (PointCollection)GetValue(MyPointCollectionProperty); }
        //    set { SetValue(MyPointCollectionProperty, value); }
        //}
        //public static readonly DependencyProperty MyPointCollectionProperty =
        //    DependencyProperty.Register(nameof(MyPointCollection), typeof(PointCollection), typeof(MainWindow), new PropertyMetadata(null));

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //MyPointCollection = MyShape1.MyPoints;
        }

        private void MyButtonMove_Click(object sender, RoutedEventArgs e)
        {
            Point p = MyShape1.MyPoints[0];
            //MyPointCollection[0] = new Point(p.X + 10, p.Y + 10);
            MyShape1.MyPoints[0] = new Point(p.X + 10, p.Y + 10);
            MyShape1.InvalidateVisual();//描画更新
            //MyShape1.UpdateLayout();//これだと反応ない
            p = MyThumb1.MyPoints[0];
            MyThumb1.MyPoints[0] = new Point(p.X + 10, p.Y + 10);
            //MyThumb1.InvalidateVisual();
        }

        private void MyButtonStroke_Click(object sender, RoutedEventArgs e)
        {
            MyThumb1.MyStroke = Brushes.YellowGreen;
        }

    }
}