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

namespace _20250309;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public EzBezierThumb MyBeziThumb { get; set; }
    public MainWindow()
    {
        InitializeComponent();
        var data = new ItemData(ThumbType.GeoShape);
        data.MyPoints = [new Point(), new Point(100, 0), new Point(100, 100), new Point(0, 100)];
        MyBeziThumb = new EzBezierThumb(data);
        DataContext = MyBeziThumb;
        data.MyBackground = Brushes.MistyRose;
        MyCanvas.Children.Add(MyBeziThumb);
    }

    private void AnchorOnOff_Click(object sender, RoutedEventArgs e)
    {
        MyBeziThumb.AnchorOnOffSwitch();
    }

    private void test_Click(object sender, RoutedEventArgs e)
    {
        MyBeziThumb.UpdatePointsAndSizeWithTransform();
    }

    private void AddPoint_Click(object sender, RoutedEventArgs e)
    {
        MyBeziThumb.AddPoint(new Point(200, 20));
    }
}