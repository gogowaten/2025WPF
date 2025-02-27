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

namespace _20250227_SerializeRootGroup;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private RootThumb MyRoot { get; set; } = null!;
    public MainWindow()
    {
        InitializeComponent();

        ItemData data = new(ThumbType.Root) { MyBackground = Brushes.SkyBlue };
        if (new RootThumb(data) is RootThumb root)
        {
            MyRoot = root;
        }
        MyScrollViewer.Content = MyRoot;
        DataContext = MyRoot;

    }

    private void MyInitialize2_Click(object sender, RoutedEventArgs e)
    {

        //var gg = MyRoot.MyFocusThumb.Background;
        //var gb = MyRoot.MyFocusThumb.MyItemData.MyBackground;
    }

    private void AddTextThumb()
    {
        var data = new ItemData(ThumbType.Text)
        {
            MyText = "TextBlock",
            MyFontSize = 30,
            MyForeground = Brushes.RosyBrown,
            MyBackground = Brushes.SeaShell,
        };
        MyRoot.AddNewThumbFromItemData(data, MyRoot.MyActiveGroupThumb);
    }
    private void AddEllipseTextThumb()
    {
        var data = new ItemData(ThumbType.Ellipse)
        {
            MyText = "Ellipse",
            MyFontSize = 30,
            MyForeground = Brushes.LightCoral,
            MyFill = Brushes.MistyRose,
            MyWidth = 80,
            MyHeight = 80
        };
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
        _ = MyRoot.MyFocusThumb?.MyItemData.Serialize("E:\\20250227.xml");
    }

    private void ReadToFile_Click(object sender, RoutedEventArgs e)
    {
        if (ItemData.Deserialize("E:\\20250227.xml") is ItemData data)
        {
            //ファイルから追加するときは0座標にしないと離れた位置に追加される
            data.MyLeft = 0;
            data.MyTop = 0;
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

    private void Remove_Click(object sender, RoutedEventArgs e)
    {
        //MyRoot.RemoveThumb(MyRoot.MyFocusThumb);
        MyRoot.RemoveThumb();
    }

    private void WakuVisible_Click(object sender, RoutedEventArgs e)
    {

        if (MyRoot.IsWakuVisible == Visibility.Visible)
        {
            MyRoot.IsWakuVisible = Visibility.Collapsed;
        }
        else
        {
            MyRoot.IsWakuVisible = Visibility.Visible;
        }
        //MyRoot.WakuVisible(false);
    }

    private void In_Click(object sender, RoutedEventArgs e)
    {
        MyRoot.ActiveGroupToInside();
    }

    private void Out_Click(object sender, RoutedEventArgs e)
    {
        MyRoot.ActiveGroupToOutside();
    }

    private void Up_Click(object sender, RoutedEventArgs e)
    {
        MyRoot.MyFocusThumb?.ZIndexUp();
    }

    private void Down_Click(object sender, RoutedEventArgs e)
    {
        MyRoot.MyFocusThumb?.ZIndexDown();
    }

    private void Top_Click(object sender, RoutedEventArgs e)
    {
        MyRoot.MyFocusThumb?.ZIndexTop();
    }

    private void Bottom_Click(object sender, RoutedEventArgs e)
    {
        MyRoot.MyFocusThumb?.ZIndexBottom();
    }

    private void SaveRoot_Click(object sender, RoutedEventArgs e)
    {
        _ = MyRoot.MyItemData.Serialize("E:\\20250227Root.xml");
    }

    private void ReadRootFile_Click(object sender, RoutedEventArgs e)
    {
        if (ItemData.Deserialize("E:\\20250227Root.xml") is ItemData data)
        {
            if (new RootThumb(data) is RootThumb root)
            {
                MyScrollViewer.Content = root;
                MyRoot = root;
                DataContext = root;
            }
        }
    }

    private void RemoveAll_Click(object sender, RoutedEventArgs e)
    {
        MyRoot.RemoveAll();
    }
}


