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

namespace _20250206
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MyEz.AnchorsOn();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MyEz.AnchorsOff();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MyEz.MyPoints[0] = new Point(-50, 0);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MyEz.MyPoints[0] = new Point(0, 0);
        }
    }
}