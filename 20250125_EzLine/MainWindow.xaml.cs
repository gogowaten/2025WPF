using System.Windows;

namespace _20250125_EzLine
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Random r = new();
            MyEzLine.MyPoints.Add(new Point(r.Next(300),r.Next(300)));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (MyEzLine.MyPoints.Count > 0)
            {
                MyEzLine.MyPoints.RemoveAt(0);
            }
        }
    }
}