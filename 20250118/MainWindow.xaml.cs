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

namespace _20250118
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MyButtonTest_Click(object sender, RoutedEventArgs e)
        {
            //MyPolyline1.Test();
            //MyPolyline1.AnchorsOn();
            //var poi = MyLine.MyPoints;
            //poi[0]=new Point(0,0);
            //poi.Add(new Point(300, 22));
            //MyLine.AnchorOn();
            MyLine.ResizeOn();

        }

        private void MyButtonAnchorOff_Click(object sender, RoutedEventArgs e)
        {
            //MyLine.AnchorOff();
            MyLine.ResizeOff();
        }
    }
}