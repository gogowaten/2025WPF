using System.Collections.ObjectModel;
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

namespace _20250512
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Brush> MyBrushes { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            MyBrushes =
            [
                Brushes.Red,
                Brushes.Orange,
                Brushes.Maroon,
                Brushes.Salmon,
                Brushes.IndianRed,
                Brushes.Olive,
                Brushes.Gold,
                Brushes.DarkGreen,
                Brushes.SteelBlue
            ];
            DataContext = MyBrushes;
        }
    }

}