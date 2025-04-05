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

namespace _20250405
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GeoShapeThumb MyGeoShapeThumb { get; set; } = null!;
        public MainWindow()
        {
            InitializeComponent();
            Test1();
            DataContext = MyGeoShapeThumb;

        }


        public void Test1()
        {
            ItemData data = new();
            data.MyLeft = 100;
            data.MyTop = 100;
            data.MyPoints = [new Point(100,0), new Point(300, 0)];
            MyGeoShapeThumb = new(data);
            MyGeoShapeThumb.Opacity = 0.5;
            
            MyCanvas.Children.Add(MyGeoShapeThumb);
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            geo.AnchorHandleOn();
        }
    }
}