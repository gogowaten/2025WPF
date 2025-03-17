using System.Runtime.InteropServices;
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

namespace _20250317;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Anchor_Click(object sender, RoutedEventArgs e)
    {
        MyThumb.AnchorSwitch();
    }

    private void AddAtTop_Click(object sender, RoutedEventArgs e)
    {
        Random r = new();
        MyThumb.AddPoint(0, new Point(r.Next(300), r.Next(300)));
    }

    private void AddAtEnd_Click(object sender, RoutedEventArgs e)
    {
        Random r = new();
        MyThumb.AddPoint(MyThumb.MyPoints.Count, new Point(r.Next(300), r.Next(300)));
    }





    private void AddRandomPoint_Click(object sender, RoutedEventArgs e)
    {
        Random r = new();
        int id = r.Next(MyThumb.MyPoints.Count);
        MyThumb.AddPoint(id, new Point(r.Next(200), r.Next(200)));

    }

    private void RemoveTopPoint_Click(object sender, RoutedEventArgs e)
    {
        MyThumb.RemovePoint(0);
    }

    private void RemoveEndPoint_Click(object sender, RoutedEventArgs e)
    {
        MyThumb.RemovePoint(MyThumb.MyPoints.Count - 1);
    }

    private void RemoveRandomPoint_Click(object sender, RoutedEventArgs e)
    {
        Random r = new();
        MyThumb.RemovePoint(r.Next(MyThumb.MyPoints.Count));
    }

    private void ResetPoint_Click(object sender, RoutedEventArgs e)
    {
        var pc = MyThumb.MyPoints;
        while (pc.Count > 2)
        {
            MyThumb.RemovePoint(0);
        }
    }

    private void ChangeType_Click(object sender, RoutedEventArgs e)
    {
        MyThumb.ChangeShapeType();
    }
}