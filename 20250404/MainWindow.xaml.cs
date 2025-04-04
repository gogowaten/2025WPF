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

namespace _20250404
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KisoThumb MyThumb { get; set; } = null!;
        public MainWindow()
        {
            InitializeComponent();
            MyThumb = Test1();
            DataContext = MyThumb;

            
        }
        
        private TextBlockThumb Test1()
        {
            ItemData data = new();
            data.MyText = "test";
            data.MyLeft = 100;
            data.MyTop = 100;
            var thumb = new TextBlockThumb(data);
            MyCanvas.Children.Add(thumb);
            return thumb;
        }
    }
}