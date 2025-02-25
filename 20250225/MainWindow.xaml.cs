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

    private void MyInitialize_Click(object sender, RoutedEventArgs e)
    {
        var data = new ItemData(ThumbType.Text)
        {
            MyText = "Item1",
            MyFontSize = 30,
            MyForegroundR = 255,
            MyBackground = Brushes.MistyRose,
        };
        MyRoot.AddNewThumb(data, MyRoot.MyActiveGroupThumb);

        //if (MyBuilder.MakeThumb(data) is KisoThumb thumb)
        //{            
        //    MyRoot.AddThumbToActiveGroup3(thumb);
        //}

    }

    private void MyInitialize2_Click(object sender, RoutedEventArgs e)
    {
        var type = MyRoot.MyThumbType;
        var datatype = MyRoot.MyItemData.MyThumbType;
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