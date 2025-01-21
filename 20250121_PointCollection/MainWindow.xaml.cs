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

namespace _20250121_PointCollection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(MainWindow), new PropertyMetadata(null));

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            MyPoints = [new(100, 10), new(210, 100)];
        }

        private void MyButtonLine1_Click(object sender, RoutedEventArgs e)
        {
            Point p = MyLine1.Points[0];
            MyLine1.Points[0] = new Point(p.X + 10, p.Y + 10);
        }

        private void MyButtonLine2_Click(object sender, RoutedEventArgs e)
        {
            //Point p = MyLine2.Points[0];
            //MyLine2.Points[0] = new Point(p.X + 10, p.Y + 10);
            Point p = MyPoints[0];
            MyPoints[0] = new Point(p.X + 10, p.Y + 10);
        }
    }
}