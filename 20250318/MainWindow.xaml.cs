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
        data.MyPoints = [new Point(), new Point(200, 0)];
        data.MyShapeType = ShapeType.Line;
        //data.MyPoints = [new Point(), new Point(200, 0), new Point(200, 100), new Point(0, 100)];
        //data.MyShapeType = ShapeType.Bezier;
        data.MyStrokeThickness = 40;

        MyThumb = new(data);
        MyCanvas.Children.Add(MyThumb);
        DataContext = MyThumb;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        MyThumb.Test();
    }

    private void AdornerSwitch_Click(object sender, RoutedEventArgs e)
    {
        MyThumb.ResizeHandleSwitch();
        MyThumb.AnchorHandleSwitch();
    }

    private void ShapTypeSwitch_Click(object sender, RoutedEventArgs e)
    {
        //MyThumb.MyShapeThumb.ChangeShapeType();
        MyThumb.ChangeShapeType();
    }

    private void FitSizeAndPos_Click(object sender, RoutedEventArgs e)
    {
        MyThumb.FitSizeAndPos();
    }

    private void Line_Click(object sender, RoutedEventArgs e)
    {        
        MyThumb.ChangeToLine();
    }

    private void Bezier_Click(object sender, RoutedEventArgs e)
    {
        MyThumb.ChangeToBezier();
    }

    private void AddPointToEnd_Click(object sender, RoutedEventArgs e)
    {
        Random r = new();
        MyThumb.AddPoint(new Point(r.Next(300), r.Next(200)));
    }

    private void RemovePointEnd_Click(object sender, RoutedEventArgs e)
    {
        MyThumb.RemovePointEnd();
    }

    private void FitHandleSizeAndPos_Click(object sender, RoutedEventArgs e)
    {
        MyThumb.FitSizeAndPosAdorner();
    }

    private void ResizeHandleType_Click(object sender, RoutedEventArgs e)
    {
        MyThumb.ChangeResizeHandleType();
    }
}