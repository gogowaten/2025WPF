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

namespace _20250212
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

            if (AdornerLayer.GetAdornerLayer(MyEz.MyEzLine) is AdornerLayer layer)
            {
                layer.Add(new EzAdorn(MyEz.MyEzLine));
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MyEz.OffsetEzLineAndThis2();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MyEz.RepairSize();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Point po = MyEz.MyPoints[1];
            MyEz.MyPoints[1] = new Point(po.X + 100, po.Y + 0);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            MyEz.OffsetEzLine();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (AdornerLayer.GetAdornerLayer(MyEz.MyEzLine) is AdornerLayer layer)
            {
                var layerTF = layer.RenderTransform;
                var neko = layer.GetAdorners(MyEz.MyEzLine);
                var EzTF = MyEz.MyEzLine.RenderTransform;
                if (neko != null && neko[0] is EzAdorn adorn)
                {
                    adorn.RenderTransform = EzTF;
                }

                //layer.RenderTransform = EzTF;

            }
        }
    }
}