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

namespace _20250308;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void AnchorOnOff_Click(object sender, RoutedEventArgs e)
    {
        MyBeziThumb.AnchorOnOffSwitch();
    }

    private void test_Click(object sender, RoutedEventArgs e)
    {
        MyBeziThumb.UpdatePointsAndSizeWithTransform();
    }

    private void AddPoint_Click(object sender, RoutedEventArgs e)
    {
        MyBeziThumb.AddPoint(new Point(200, 20));
    }
}