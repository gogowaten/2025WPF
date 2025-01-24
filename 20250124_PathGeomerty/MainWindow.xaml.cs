using System.Windows;
using System.Windows.Media;

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