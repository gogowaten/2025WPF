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

namespace _20250316;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    GeoShapeTThumb? MyTThumb { get; set; }
    public MainWindow()
    {
        InitializeComponent();

    }

    private void AddTThumb()
    {
        ItemData data = new();
        data.MyPoints = [new Point(), new Point(200, 100), new Point(0,100)];
        data.MyStrokeThickness = 40;
        MyTThumb = new GeoShapeTThumb(data);
        MyCanvas.Children.Add(MyTThumb);
    }
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        AddTThumb();
    }

    private void AnchorSwitch_Click(object sender, RoutedEventArgs e)
    {
        _ = MyShape.AnchorSwitch();
        MyTThumb?.AnchorHandleSwitch();
        MyTThumb?.ResizeHandleSwitch();
    }
}