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

namespace _20250225;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly RootThumb MyRoot = null!;
    double OffsetLeft = 32;
    double OffsetTop = 32;

    public MainWindow()
    {
        InitializeComponent();
        ItemData data = new(ThumbType.Root) { MyBackground = Brushes.Khaki };
        if (new RootThumb(data) is RootThumb root)
        {
            MyRoot = root;
        }
        MyScrollViewer.Content = MyRoot;
        DataContext = MyRoot;
    }

    private void MyInitialize2_Click(object sender, RoutedEventArgs e)
    {
        var type = MyRoot.MyThumbType;
        var datatype = MyRoot.MyItemData.MyThumbType;
    }

    private void AddTextThumb()
    {
        var data = new ItemData(ThumbType.Text)
        {
            MyText = "TextBlock",
            MyFontSize = 30,
            MyForegroundR = 255,
            MyBackground = Brushes.MistyRose,
        };
        data.MyLeft += OffsetLeft;
        data.MyTop += OffsetTop;
        MyRoot.AddNewThumbFromItemData(data, MyRoot.MyActiveGroupThumb);
    }
    private void AddEllipseTextThumb()
    {
        var data = new ItemData(ThumbType.Ellipse)
        {
            MyText = "Ellipse",
            MyFontSize = 30,
            MyForegroundR = 255,
            MyBackground = Brushes.Gold,
            MyFill = Brushes.MistyRose,
            MyWidth = 80,
            MyHeight = 80
        };
        data.MyLeft += OffsetLeft;
        data.MyTop += OffsetTop;
        MyRoot.AddNewThumbFromItemData(data, MyRoot.MyActiveGroupThumb);
    }


    private void AddTextThumb_Click(object sender, RoutedEventArgs e)
    {
        AddTextThumb();
    }

    private void AddEllipseThumb_Click(object sender, RoutedEventArgs e)
    {
        AddEllipseTextThumb();
    }

    private void SaveToFile_Click(object sender, RoutedEventArgs e)
    {
        _ = MyRoot.MyFocusThumb?.MyItemData.Serialize("E:\\20250225.xml");
    }

    private void ReadToFile_Click(object sender, RoutedEventArgs e)
    {
        if (ItemData.Deserialize("E:\\20250225.xml") is ItemData data)
        {
            MyRoot.AddNewThumbFromItemData(data, MyRoot.MyActiveGroupThumb);
        }
    }

    private void AddGroup_Click(object sender, RoutedEventArgs e)
    {
        MyRoot.AddGroupFromSelected();
    }

    private void Ungroup_Click(object sender, RoutedEventArgs e)
    {
        MyRoot.UngroupFocusThumb();
    }
}


public static class MyBuilder
{
    public static KisoThumb? MakeThumb(ItemData data)
    {
        if (data.MyThumbType == ThumbType.Text)
        {
            return new TextBlockThumb(data);
        }
        else if (data.MyThumbType == ThumbType.Ellipse)
        {
            return new EllipseTextThumb(data);
        }
        else if (data.MyThumbType == ThumbType.Group)
        {
            return new GroupThumb(data);
        }
        else if (data.MyThumbType == ThumbType.Root)
        {
            return new RootThumb(data);
        }
        else { return null; }
    }

    public static KisoThumb? MakeThumb(string filePath)
    {
        if (ItemData.Deserialize(filePath) is ItemData data)
        {
            return MakeThumb(data);
        }
        else { return null; }
    }
}