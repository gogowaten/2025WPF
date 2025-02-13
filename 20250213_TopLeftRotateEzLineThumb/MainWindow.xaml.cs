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
//WPF、図形の回転後の頂点移動できた、ただし回転軸は左上 - 午後わてんのブログ
//https://gogowaten.hatenablog.com/entry/2025/02/13/183700

namespace _20250213_TopLeftRotateEzLineThumb;

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
        MyEz.AnchorOn();

    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        MyEz.AnchorOff();
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
        Random r = new();
        MyEz.AddPoint(new Point(r.Next(200), r.Next(200)), MyEz.MyPoints.Count);
    }

    private void Button_Click_3(object sender, RoutedEventArgs e)
    {
        if (MyEz.MyPoints.Count > 0)
        {
            MyEz.RemovePoint(MyEz.MyPoints.Count-1);
        }
    }

    private void Button_Click_4(object sender, RoutedEventArgs e)
    {
        MyEz.ZeroFix();
    }
}