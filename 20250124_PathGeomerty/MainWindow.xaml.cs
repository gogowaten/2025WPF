using System.Windows;
using System.Windows.Media;
//WPF、PathGeometryで直線図形を描画したときの動作確認してみた、FillRule、IsFilled、IsClosed、IsStroked、IsSmoothJoin - 午後わてんのブログ
//https://gogowaten.hatenablog.com/entry/2025/01/24/151432


namespace _20250124_PathGeomerty
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ComboFillRule.ItemsSource = Enum.GetValues(typeof(FillRule));
        }
    }
}