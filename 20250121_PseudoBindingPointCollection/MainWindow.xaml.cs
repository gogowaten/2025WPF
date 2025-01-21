using System.Windows;

namespace _20250121_PseudoBindingPointCollection
{
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }
       
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            Point point = MyLine1.MyPoints[0];
            MyLine1.MyPoints[0] = new Point(point.X, point.Y + 10);
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            Point point = MyLine2.MyPoints[0];
            MyLine2.MyPoints[0] = new Point(point.X, point.Y + 10);
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            Point point = MyLine3.MyPoints[0];
            MyLine3.MyPoints[0] = new Point(point.X, point.Y + 10);
        }

    }
}