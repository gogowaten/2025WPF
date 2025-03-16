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

namespace _20250315;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private GeoShapeTThumb? MyGeoShapeTThumb { get; set; }
    public MainWindow()
    {
        InitializeComponent();

    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (MyGeoShapeTThumb != null)
        {
            _ = SwitchAdorner(MyGeoShapeTThumb);
        }
    }


    /// <summary>
    /// 対象にリサイズ用のハンドル(装飾)を付け外しする
    /// </summary>
    /// <param name="elem">対象要素</param>
    /// <returns>装飾</returns>
    private static ResizeHandleAdorner? SwitchAdorner(FrameworkElement elem)
    {
        if (AdornerLayer.GetAdornerLayer(elem) is AdornerLayer layer)
        {
            var items = layer.GetAdorners(elem);
            if (items != null)
            {
                foreach (var item in items.OfType<ResizeHandleAdorner>())
                {
                    layer.Remove(item);
                }
                return null;
            }
            else
            {
                var adorner = new ResizeHandleAdorner(elem);
                layer.Add(adorner);
                return adorner;
            }
        }
        return null;
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        var data = new ItemData();
        data.MyStrokeThickness = 40.0;
        MyGeoShapeTThumb = new(data);
        MyCanvas.Children.Add(MyGeoShapeTThumb);
        DataContext = MyGeoShapeTThumb;
    }
}