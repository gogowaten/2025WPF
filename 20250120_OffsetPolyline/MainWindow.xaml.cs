using System.Windows;

namespace _20250120_OffsetPolyline
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

        private void MyButton1_Click(object sender, RoutedEventArgs e)
        {
            MyLine1.MyStrokeThickness += 10;
        }

        private void MyButton2_Click(object sender, RoutedEventArgs e)
        {
            if (MyLine1.MyStrokeThickness > 10)
            {
                MyLine1.MyStrokeThickness -= 10;
            }
        }

        private void MyButton3_Click(object sender, RoutedEventArgs e)
        {
            MyLine2.MyStrokeThickness += 10;
        }

        private void MyButton4_Click(object sender, RoutedEventArgs e)
        {
            if (MyLine2.MyStrokeThickness > 10)
            {
                MyLine2.MyStrokeThickness -= 10;
            }
        }

        private void MyButton5_Click(object sender, RoutedEventArgs e)
        {
            Point p = MyLine1.MyPoints[0];
            MyLine1.MyPoints[0] = new Point(p.X + 10, p.Y + 10);
        }
    }
}