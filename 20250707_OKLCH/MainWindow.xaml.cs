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

namespace _20250707_OKLCH
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Iro MyIro { get; set; } = new();
        public Iro2 MyIro2 { get; set; } = new();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var iro = MyIro;
            var (L, C, H) = Oklch.RgbToOklch(MyIro.R, MyIro.G, MyIro.B);
            var c = C / 0.4 * 100;
        }
    }
}