﻿using System.Text;
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

namespace _20250301;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private EzLineThumb2? MyLine2;
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        MyEz.AnchorOn();
        MyLine2.AnchorOn();
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        MyEz.AnchorOff();
        MyLine2.AnchorOff();
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
        //MyEz.ZeroFix();
        var data = new ItemData(ThumbType.Line)
        {
            MyPoints = [new Point(), new Point(100, 0)],
            MyStroke = Brushes.Red,
            MyStrokeThickness = 40,
            MyBackground = Brushes.MistyRose,

        };
        MyLine2 = new EzLineThumb2(data);
        MyCanvas.Children.Add(MyLine2);

    }

    private void Button_Click_5(object sender, RoutedEventArgs e)
    {
        var line = MyLine2;
        if (MyLine2 != null)
        {
            var offsettto = MyLine2.MyOffsetTop;
            var bounds = MyLine2.MyEzLine.MyBounds4;
        }
    }
}