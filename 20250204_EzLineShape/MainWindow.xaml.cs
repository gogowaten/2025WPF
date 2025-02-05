using System.Windows;
using System.Windows.Media;
//WPF、簡単に折れ線描画できて見た目通りのサイズと位置が取得できるクラスをShapeクラス継承で - 午後わてんのブログ
//https://gogowaten.hatenablog.com/entry/2025/02/05/161559

namespace _20250204_EzLineShape
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MyComboJoin.ItemsSource = Enum.GetValues(typeof(PenLineJoin));
            MyComboStartCap.ItemsSource = Enum.GetValues(typeof(PenLineCap));
        }
    }
}