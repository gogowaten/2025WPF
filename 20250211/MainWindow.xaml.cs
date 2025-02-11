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

namespace _20250211
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MyEz.AnchorsOn();
            if (AdornerLayer.GetAdornerLayer(MyEz.MyEzLine) is AdornerLayer layer)
            {
                layer.Add(new EzAdoner(MyEz.MyEzLine));
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //MyEz.AnchorsOff();
            if (AdornerLayer.GetAdornerLayer(MyEz.MyEzLine) is AdornerLayer layer)
            {
               var ado = layer.GetAdorners(MyEz.MyEzLine);
                layer.Remove(ado[0]);
            }
        }


    }
}
