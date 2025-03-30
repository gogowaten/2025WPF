﻿using Microsoft.Win32;
using System.Drawing;
using System.IO;
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

//2025WPF / 20250310 at main · gogowaten/2025WPF
//https://github.com/gogowaten/2025WPF/tree/main/20250310
//これに
//2025WPF/20250327_GeoShapeThumbSerialize at main · gogowaten/2025WPF
//https://github.com/gogowaten/2025WPF/tree/main/20250327_GeoShapeThumbSerialize
//この矢印図形を追加した感じ
//リサイズ機能は外した

//WPF、矢印図形Thumbのシリアライズテスト - 午後わてんのブログ
//https://gogowaten.hatenablog.com/entry/2025/03/28/151812

namespace _20250329;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private RootThumb MyRoot { get; set; } = null!;
    string SaveFileName = "E:\\" + DateTime.Now.ToString("yyyyMMdd") + ".xml";
    string SaveRootFileName = "E:\\" + DateTime.Now.ToString("yyyyMMdd") + "Root.xml";
    string SaveFileNameZip = "E:\\" + DateTime.Now.ToString("yyyyMMdd") + ".zip";
    string SaveRootFileNameZip = "E:\\" + DateTime.Now.ToString("yyyyMMdd") + "Root.zip";

    public MainWindow()
    {
        InitializeComponent();

        MyInitialize();
    }

    #region 初期処理
    private void MyInitialize()
    {
        AllowDrop = true;
        Drop += MainWindow_Drop;

        ItemData data = new(ThumbType.Root) { MyBackground = Brushes.DeepSkyBlue };
        if (new RootThumb(data) is RootThumb root)
        {
            MyRoot = root;
        }
        MyScrollViewer.Content = MyRoot;
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



    #endregion メソッド

    private void MyTest2_Click(object sender, RoutedEventArgs e)
    {
        //ItemData data = new(ThumbType.Image);
        //var bmp = MyRoot.GetBitmap("D:\\ブログ用\\テスト用画像\\hueRectT000.png");
        //data.MyBitmapSource = bmp;
        //MyRoot.AddNewThumbFromItemData(data, MyRoot);
        if(MyRoot.LoadItemData(SaveFileNameZip)is ItemData data)
        {
            data.MyLeft = 0;data.MyTop = 0;
            MyRoot.AddNewThumbFromItemData(data);
        }
        
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
}