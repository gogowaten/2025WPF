using System.Drawing;
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
using Point = System.Windows.Point;


//WPF、要素をファイルに保存と復元テスト - 午後わてんのブログ
//https://gogowaten.hatenablog.com/entry/2025/02/28/130209
//を改変、EzShape追加

namespace _20250327_GeoShapeThumbSerialize;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private RootThumb MyRoot { get; set; } = null!;
    string SaveFileName = "E:\\" + DateTime.Now.ToString("yyyyMMdd") + ".xml";
    string SaveRootFileName = "E:\\" + DateTime.Now.ToString("yyyyMMdd") + "Root.xml";

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

        data = new(ThumbType.GeoShape)
        {
            MyPoints = [new Point(), new Point(100, 0)],
            MyStroke = Brushes.Maroon,
            MyStrokeThickness = 40,
            MyTop = 0,
        };
        //GeoShapeThumb2 thumb2 = new(data);
        MyRoot.AddNewThumbFromItemData(data, MyRoot);
    }

    private void MyTest2_Click(object sender, RoutedEventArgs e)
    {
        //if (MyRoot.MyFocusThumb is EzShapeThumb shape)
        //{
        //    var p = new System.Windows.Point(100, 150);
        //    var maeP = shape.MyItemData.MyPoints[^1];

        //    shape.AddPoint(maeP);
        //    shape.AddPoint(p);
        //    shape.AddPoint(p);

        //}

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

        _ = MyRoot.MyFocusThumb?.MyItemData.Serialize(SaveFileName);
        //_ = MyRoot.MyFocusThumb?.MyItemData.Serialize("E:\\20250227.xml");
    }

    private void ReadToFile_Click(object sender, RoutedEventArgs e)
    {
        if (ItemData.Deserialize(SaveFileName) is ItemData data)
        //if (ItemData.Deserialize("E:\\20250227.xml") is ItemData data)
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
        _ = MyRoot.MyItemData.Serialize(SaveRootFileName);
    }

    private void ReadRootFile_Click(object sender, RoutedEventArgs e)
    {
        if (ItemData.Deserialize(SaveRootFileName) is ItemData data)
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

    private void AddBeziThumb_Click(object sender, RoutedEventArgs e)
    {
        var data = new ItemData(ThumbType.Bezier)
        {
            MyText = "Bezier",
            MyPoints = [new System.Windows.Point(), new System.Windows.Point(100, 0), new System.Windows.Point(100, 100), new System.Windows.Point(0, 100)],
            MyForeground = Brushes.RosyBrown,
            MyBackground = Brushes.SeaShell,
            MyStroke = Brushes.Tomato,
            MyStrokeThickness = 10.0,
        };
        MyRoot.AddNewThumbFromItemData(data, MyRoot.MyActiveGroupThumb);
    }

    private void AnchorSwitch_Click(object sender, RoutedEventArgs e)
    {
        //if (MyRoot.MyFocusThumb is EzBezierThumb bezi)
        //{
        //    bezi.AnchorOnOffSwitch();
        //}
        if (MyRoot.MyFocusThumb is GeoShapeThumb2 shape)
        {
            shape.AnchorSwitch();
        }
    }

    private void AddPolyLine_Click(object sender, RoutedEventArgs e)
    {
        var data = new ItemData(ThumbType.GeoShape)
        {
            MyText = "折れ線",
            MyPoints = [new Point(), new Point(100, 0), new Point(100, 100), new Point(0, 100)],
            MyForeground = Brushes.RosyBrown,
            MyBackground = Brushes.SeaShell,
            MyStroke = Brushes.Tomato,
            MyStrokeThickness = 40.0,
        };
        MyRoot.AddNewThumbFromItemData(data, MyRoot.MyActiveGroupThumb);
        //var data = new ItemData(ThumbType.PolyLine)
        //{
        //    MyText = "折れ線",
        //    MyPoints = [new Point(), new Point(100, 0), new Point(100, 100), new Point(0, 100)],
        //    MyForeground = Brushes.RosyBrown,
        //    MyBackground = Brushes.SeaShell,
        //    MyStroke = Brushes.Tomato,
        //    MyStrokeThickness = 30.0,
        //};
        //MyRoot.AddNewThumbFromItemData(data, MyRoot.MyActiveGroupThumb);

    }

    private void AddPoint_Click(object sender, RoutedEventArgs e)
    {
        if (MyRoot.MyFocusThumb is GeoShapeThumb2 geo2)
        {
            Random r = new();
            //Point maeP = geo2.MyItemData.MyPoints[^1];

            //geo2.MyItemData.MyPoints.Add(new Point(100, 140));
            geo2.AddPoint(new Point(r.Next(200), r.Next(200)));

        }

    }

    private void RemovePoint_Click(object sender, RoutedEventArgs e)
    {
        if (MyRoot.MyFocusThumb is GeoShapeThumb2 geo2)
        {
            geo2.RemovePoint();
        }
    }

    private void ShapeTypeSwitch_Click(object sender, RoutedEventArgs e)
    {
        if (MyRoot.MyFocusThumb is GeoShapeThumb2 geo2)
        {
            geo2.ShapeTypeSwitch();
        }
    }

    private void ShapeCapSwitch_Click(object sender, RoutedEventArgs e)
    {
        if(MyRoot.MyFocusThumb is GeoShapeThumb2 shape)
        {
            HeadType type = shape.MyItemData.MyGeoShapeHeadCapType;
            if (type == HeadType.None)
            {
                shape.MyItemData.MyGeoShapeHeadCapType = HeadType.Arrow;
            }
            else
            {
                shape.MyItemData.MyGeoShapeHeadCapType = HeadType.None;
            }
        }
    }
}