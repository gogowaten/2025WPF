using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250124
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ComboFillRule.ItemsSource = Enum.GetValues(typeof(FillRule));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MyPath.MyPoints.Remove(MyPath.MyPoints[0]);
        }
    }
}