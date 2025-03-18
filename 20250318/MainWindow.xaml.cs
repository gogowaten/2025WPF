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

namespace _20250318;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private GeoShapeTThumb MyThumb { get; set; }
    public MainWindow()
    {
        InitializeComponent();
        var data = new ItemData();
        MyThumb = new(data);
        MyCanvas.Children.Add(MyThumb);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        //if(AdornerLayer.GetAdornerLayer(MyRect) is AdornerLayer layer)
        //{
        //    layer.Add(new ResizeHandleAdorner(MyRect));
        //}
        MyThumb.ResizeHandleSwitch();
        MyThumb.AnchorHandleSwitch();
    }
}