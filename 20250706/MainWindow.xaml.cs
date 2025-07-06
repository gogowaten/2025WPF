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
// 無理、RGBとHSVをバインドだけで処理できないか試したけどできない
// 諦める

namespace _20250706
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MyIro = new Iro();
            DataContext = this;
        }


        public Iro MyIro
        {
            get { return (Iro)GetValue(MyIroProperty); }
            set { SetValue(MyIroProperty, value); }
        }
        public static readonly DependencyProperty MyIroProperty =
            DependencyProperty.Register(nameof(MyIro), typeof(Iro), typeof(MainWindow), new PropertyMetadata(null));

    }
}