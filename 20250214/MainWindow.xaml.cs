using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace _20250214;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var pc = new PointCollection
        {
            new Point(),
            new Point(100, 200)
        };
        ItemData data = new()
        {
            MyLeft = 200,
            MyPoints = pc,

        };

        //EzLineThumb ez = new(data);
        //ez.Name = "eeeeeeeeee";
        //MyCanvas.Children.Add(ez);
        //var neko = ez.MyItemData;
    }

    private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
    {
        if (MyEz.MyPoints != null && MyEz.MyPoints.Count != 0)
        {
            MyEz.MyPoints[MyEz.MyPoints.Count - 1] = e.GetPosition(MyCanvas);
        }
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
            MyEz.RemovePoint(MyEz.MyPoints.Count - 1);
        }
    }

    private void Button_Click_4(object sender, RoutedEventArgs e)
    {
        MyEz.ZeroFix();
    }

    private void OekakiOn()
    {
        MyEz.MyPoints = [];
        MyCanvas.PreviewMouseLeftButtonDown += MyCanvas_PreviewMouseLeftButtonDown;
        //MyCanvas.MouseLeftButtonDown += MyCanvas_MouseLeftButtonDown;
        MyCanvas.MouseMove += MyCanvas_MouseMove;
        MyCanvas.MouseRightButtonDown += MyCanvas_MouseRightButtonDown;
    }

    private void MyCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (MyEz.MyPoints.Count == 0)
        {
            MyEz.MyPoints.Add(e.GetPosition(MyCanvas));
        }
        MyEz.MyPoints.Add(e.GetPosition(MyCanvas));
    }

    private void MyCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        MyCanvas.PreviewMouseLeftButtonDown -= MyCanvas_PreviewMouseLeftButtonDown;
        //MyCanvas.MouseLeftButtonDown -= MyCanvas_MouseLeftButtonDown;
        MyCanvas.MouseMove -= MyCanvas_MouseMove;
        MyCanvas.MouseRightButtonDown -= MyCanvas_MouseRightButtonDown;
    }

    private void Button_Click_5(object sender, RoutedEventArgs e)
    {
        var data = MyEz.MyItemData;



        
        string filePath = "E:\\20250214.xml";
        
        SaveLine(MyEz.MyItemData, filePath);

    }


    private void SaveLine(ItemData line, string filePath)
    {
        DataContractSerializer serializer = new(typeof(ItemData));
        using FileStream stream = new(filePath, FileMode.Create);
        serializer.WriteObject(stream, line);
    }

}