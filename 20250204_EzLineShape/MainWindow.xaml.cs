using System.Windows;
using System.Windows.Media;

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