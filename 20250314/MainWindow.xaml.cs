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

namespace _20250314;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }


    private void MyButton_Click(object sender, RoutedEventArgs e)
    {
        SwitchAdorner(MyButton);
        SwitchAdorner(MyGroup);
        SwitchAdorner(MyButtonOnCanvas);
    }

    private void SwitchAdorner(FrameworkElement elem)
    {
        if (AdornerLayer.GetAdornerLayer(elem) is AdornerLayer layer)
        {
            var items = layer.GetAdorners(elem);
            if (items != null)
            {
                foreach (var item in items.OfType<ResizeAdorner>())
                {
                    layer.Remove(item);
                }
            }
            else
            {
                layer.Add(new ResizeAdorner(elem));
            }
        }
    }
}