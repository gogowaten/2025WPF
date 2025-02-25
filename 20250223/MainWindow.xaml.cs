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

namespace _20250223;

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
        //MyRoot.MyItemData.Serialize("E:\\20250223.xml");
        MyRoot.MyFocusThumb?.MyItemData.Serialize("E:\\20250223.xml");
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        if (MyBuilder.MakeThumb("E:\\20250223.xml") is KisoThumb t)
        {
            MyRoot.AddThumbToActiveGroup3(t, 32, 32);
        }
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
        if (MyBuilder.MakeThumb("E:\\20250223.xml") is KisoThumb t)
        {
            if (MyRoot.MyFocusThumb?.MyItemData.MyZIndex is int ind)
            {
                MyRoot.AddThumbInsertToActiveGroup(t, ind + 1, 32, 32);
            }
        }
    }

    private void AddGroup_Click(object sender, RoutedEventArgs e)
    {
        MyRoot.AddGroupFromSelected();
    }

    private void Remove_Click(object sender, RoutedEventArgs e)
    {
        var inu = MyRoot.MySelectedThumbs[0].MyItemData.MyGuid;
        var neko1 = MyRoot.MyItemData.MyThumbsItemData[0].MyGuid;
        var neko2 = MyRoot.MyItemData.MyThumbsItemData[1].MyGuid;
        var neko3 = MyRoot.MyItemData.MyThumbsItemData[2].MyGuid;

        MyRoot.RemoveSelectedThumbs();
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
        else if(data.MyThumbType== ThumbType.Group)
        {
            return new GroupThumb(data);
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