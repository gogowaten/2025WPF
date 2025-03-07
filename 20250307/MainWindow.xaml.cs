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

        MyBezierThumb.Relayout();
    }

    private void test_Click(object sender, RoutedEventArgs e)
    {
        MyBezierThumb.FixPointsLocateAndSize();
    }

    private void pointZero_Click(object sender, RoutedEventArgs e)
    {
        //MyBezier.FixPointsLocateAndSize2();
        MyBezierThumb.MyPoints[3] = new Point(200, 100);
        MyBezierThumb.MyPoints[1] = new Point(0, 100);
        MyBezierThumb.FixAdornerLocate();


    }
}
