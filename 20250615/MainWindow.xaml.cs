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

namespace _20250615
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public PPolyline MyPPolyline { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            //MyPPolyline = new();
            //DataContext = MyPPolyline;
            //MyPPolyline.MyOriginPoints = [new Point(), new Point(0, 100), new Point(100, 100)];

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MyFreehand.Test();
            //MyPPolyline.MyOriginPoints.Add(new Point(200, 2));
            MyFreehand.MyPPoints.MyOriginPoints.Add(new Point(200, 22));
        }
    }
}