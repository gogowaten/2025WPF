using System.Windows;
using System.Windows.Media;
//WPF、カスタムコントロールのThumbに表示するPolylineはGridよりCanvasに乗せると都合がいい - 午後わてんのブログ
//https://gogowaten.hatenablog.com/entry/2025/01/23/125044

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