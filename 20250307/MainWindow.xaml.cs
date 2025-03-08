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

namespace _20250307;

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
        //MyLine.AdornerSwitch();
        MyBezierThumb.AdornerSwitch();
    }

    private void Relayout_Click(object sender, RoutedEventArgs e)
    {
        MyBezierThumb.UpdatePointAndSize();
        //MyBezierThumb.Relayout();
    }

    private void test_Click(object sender, RoutedEventArgs e)
    {
        MyBezierThumb.UpdatePointsAndSizeWithoutZeroFixTest();
    }

    private void pointZero_Click(object sender, RoutedEventArgs e)
    {
        //MyBezier.FixPointsLocateAndSize2();
        MyBezierThumb.MyPoints[0] = new Point(2, 178);
        MyBezierThumb.MyPoints[1] = new Point(0, 79);
        MyBezierThumb.MyPoints[2] = new Point(154, 0);
        MyBezierThumb.MyPoints[3] = new Point(121, 219);
        MyBezierThumb.FixAdornerLocate();


    }
}
