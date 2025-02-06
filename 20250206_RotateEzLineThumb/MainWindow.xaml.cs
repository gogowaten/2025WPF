using System.Windows;
//WPF、図形を中心で回転、PathGeometry＋Canvas＋Thumbのカスタムコントロール - 午後わてんのブログ
//https://gogowaten.hatenablog.com/entry/2025/02/06/132308

namespace _20250206_RotateEzLineThumb
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
            Point p = new(r.Next(200), r.Next(200));
            MyEz.MyPoints.Add(p);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (MyEz.MyPoints.Count > 0)
            {
                MyEz.MyPoints.RemoveAt(0);
            }
        }
    }
}