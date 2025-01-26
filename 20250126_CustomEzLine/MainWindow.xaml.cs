using System.Windows;

namespace _20250126_CustomEzLine
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            MyLine.MyIsStroked = !MyLine.MyIsStroked;
        }

        private void ToggleButton_Click_1(object sender, RoutedEventArgs e)
        {
            MyLine.MyIsClosed = !MyLine.MyIsClosed;
        }

        private void ToggleButton_Click_2(object sender, RoutedEventArgs e)
        {
            MyLine.MyIsSmoothJoin = !MyLine.MyIsSmoothJoin;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Random r = new();
            MyLine.MyPoints.Add(new Point(r.Next(300), r.Next(300)));
        }
    }
}