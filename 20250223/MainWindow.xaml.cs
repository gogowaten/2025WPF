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
        if (ItemData.Deserialize("E:\\20250223.xml") is ItemData data)
        {
            if (MyBuilder.MakeThumb(data) is KisoThumb t)
            {


                MyRoot.AddThumbToActiveGroup3(t, 32, 32);

            }
        }
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
        if (ItemData.Deserialize("E:\\20250223.xml") is ItemData data)
        {
            if (MyBuilder.MakeThumb(data) is KisoThumb t)
            {
                if (MyRoot.MyFocusThumb?.MyItemData.MyZIndex is int ind)
                {
                    MyRoot.AddThumbInsertToActiveGroup(t, ind + 1, 32, 32);
                }
            }
        }
        //var data = new ItemData
        //{
        //    MyThumbType = ThumbType.Text,
        //    MyText = "AddFromData"
        //};
        //var thumb = MyBuilder.MakeThumb(data);
        //if (thumb != null)
        //{
        //    MyRoot.AddThumbToActiveGroup3(thumb, 32, 32);
        //}
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
        else { return null; }
    }
}