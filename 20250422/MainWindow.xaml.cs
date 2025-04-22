using Microsoft.Win32;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Point = System.Windows.Point;


//2025WPF / 20250421 at main · gogowaten/2025WPF
//https://github.com/gogowaten/2025WPF/tree/main/20250421
//を改変

namespace _20250422;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    //private ContextMenu MyContextMenu = null!;
    private RootThumb MyRoot { get; set; } = null!;
    private ManageExCanvas MyManageExCanvas { get; set; }=null!;

    string SaveFileName = "E:\\" + DateTime.Now.ToString("yyyyMMdd") + ".xml";
    string SaveRootFileName = "E:\\" + DateTime.Now.ToString("yyyyMMdd") + "Root.xml";
    string SaveFileNameZip = "E:\\" + DateTime.Now.ToString("yyyyMMdd") + ".zip";
    string SaveRootFileNameZip = "E:\\" + DateTime.Now.ToString("yyyyMMdd") + "Root.zip";

    public MainWindow()
    {
        InitializeComponent();

        MyInitialize();
        Loaded += MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        //SwitchAdorner(MyRange);

    }

    #region 初期処理
    private void MyInitialize()
    {
        AllowDrop = true;
        Drop += MainWindow_Drop;

        ItemData data = new(ThumbType.Root) { MyBackground = Brushes.SkyBlue };
        //if (new RootThumb(data) is RootThumb root)
        //{
        //    MyRoot = root;
        //}
        MyRoot = new RootThumb(data);
        //MyScrollViewer.Content = MyRoot;
        //MyPanel.Children.Add(MyRoot);
        MyManageExCanvas = new(MyRoot);
        MyScrollViewer.Content = MyManageExCanvas;
        DataContext = MyRoot;

    }


    private void MainWindow_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);
            MyRoot.OpenFiles(paths);
        }
    }



    #endregion 初期処理




    #region メソッド


    /// <summary>
    /// 対象にリサイズ用のハンドル(装飾)を付け外しする
    /// </summary>
    /// <param name="elem">対象要素</param>
    /// <returns>装飾</returns>
    private static ResizeHandleAdorner? SwitchAdorner(FrameworkElement elem)
    {
        if (AdornerLayer.GetAdornerLayer(elem) is AdornerLayer layer)
        {
            var items = layer.GetAdorners(elem);
            if (items != null)
            {
                foreach (var item in items.OfType<ResizeHandleAdorner>())
                {
                    layer.Remove(item);
                }
                return null;
            }
            else
            {
                var adorner = new ResizeHandleAdorner(elem);
                adorner.MyHandleLayout = HandleLayoutType.Inside;
                //adorner.MyHandleLayout = HandleLayoutType.Online;
                layer.Add(adorner);
                return adorner;
            }
        }
        return null;
    }


    #endregion メソッド

    private void MyTest2_Click(object sender, RoutedEventArgs e)
    {
        //ItemData data = new(ThumbType.Image);
        //var bmp = MyRoot.GetBitmap("D:\\ブログ用\\テスト用画像\\hueRectT000.png");
        //data.MyBitmapSource = bmp;
        //MyRoot.AddNewThumbFromItemData(data, MyRoot);


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

    //FocusThumbをファイルに保存
    private void SaveToFile_Click(object sender, RoutedEventArgs e)
    {
        //_ = MyRoot.MyFocusThumb?.MyItemData.Serialize(SaveFileName);
        if (MyRoot.MyFocusThumb?.MyItemData is ItemData data)
        {
            //if (MyRoot.SaveItemData(data, "E:\\20250329ItemData.zip"))
            if (MyRoot.SaveItemData(data, SaveFileNameZip))
            {
                MessageBox.Show("保存した");
            }
        }
    }

    //ファイルからの読み込み
    private void LoadFile_Click(object sender, RoutedEventArgs e)
    {
        //if (ItemData.Deserialize(SaveFileName) is ItemData data)
        //{
        //    //ファイルから追加するときは0座標にしないと離れた位置に追加される
        //    data.MyLeft = 0;
        //    data.MyTop = 0;
        //    MyRoot.AddNewThumbFromItemData(data, MyRoot.MyActiveGroupThumb);
        //}
        if (MyRoot.LoadItemData(SaveFileNameZip) is ItemData data)
        {
            data.MyLeft = 0; data.MyTop = 0;
            MyRoot.AddNewThumbFromItemData(data);
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

    //Rootの保存、全体の保存
    private void SaveRoot_Click(object sender, RoutedEventArgs e)
    {
        //_ = MyRoot.MyItemData.Serialize(SaveRootFileName);
        MyRoot.SaveItemData(MyRoot.MyItemData, SaveRootFileNameZip);
    }

    //Rootファイルの読み込み
    private void ReadRootFile_Click(object sender, RoutedEventArgs e)
    {
        ////画像なしDataのとき
        //if (ItemData.Deserialize(SaveRootFileName) is ItemData data)
        //{
        //    if (new RootThumb(data) is RootThumb root)
        //    {
        //        MyScrollViewer.Content = root;
        //        MyRoot = root;
        //        DataContext = root;
        //    }
        //}
        if (MyRoot.LoadItemData(SaveRootFileNameZip) is ItemData data)
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
            MyText = "図形",
            MyPoints = [new Point(), new Point(100, 0), new Point(100, 100), new Point(0, 100)],
            MyBackground = null,
            MyStroke = Brushes.Khaki,
            MyStrokeThickness = 30.0,
        };
        MyRoot.AddNewThumbFromItemData(data, MyRoot.MyActiveGroupThumb);
    }

    private void AddBezier_Click(object sender, RoutedEventArgs e)
    {
        var data = new ItemData(ThumbType.GeoShape)
        {
            MyText = "図形",
            MyPoints = [new Point(), new Point(200, 0), new Point(0, 100), new Point(200, 100)],
            MyBackground = null,
            MyStroke = Brushes.PaleGoldenrod,
            MyStrokeThickness = 30.0,
            MyShapeType = ShapeType.Bezier,
            MyGeoShapeHeadCapType = HeadType.Arrow,
        };
        MyRoot.AddNewThumbFromItemData(data, MyRoot.MyActiveGroupThumb);
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
            var neko = geo2.MyItemData.MyBackground;
        }
    }

    private void ShapeCapSwitch_Click(object sender, RoutedEventArgs e)
    {
        if (MyRoot.MyFocusThumb is GeoShapeThumb2 shape)
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
            shape.UpdateLocateAndSize();
            MyRoot.MyFocusThumb.MyParentThumb?.ReLayout3();
        }
    }

    //画像ファイルから追加、複数対応、未対応ファイルの場合は追加できなかったものをメッセージ表示
    private void AddImageFiles_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new() { Multiselect = true };

        if (dialog.ShowDialog() is true)
        {
            MyRoot.OpenFiles(dialog.FileNames);
        }
    }

    private void SaveToImageFile_Click(object sender, RoutedEventArgs e)
    {
        //FocusThumbを画像として保存
        MyRoot.SaveMyFocusThumbToImageFile();
    }

}