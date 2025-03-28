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

namespace _20250103
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
#if DEBUG
            Left = 50;
            Top = 300;
#endif
            InitializeComponent();
            MyRootGroup.GotKeyboardFocus += MyRootGroup_GotKeyboardFocus;
            MyRootGroup.GotFocus += MyRootGroup_GotFocus;
            MyRootGroup.PreviewGotKeyboardFocus += MyRootGroup_PreviewGotKeyboardFocus;
            //MyScrollV.CanContentScroll = false;

            MyComboBoxThumbType.ItemsSource = Enum.GetValues(typeof(ThumbType));

            var inu = typeof(Brushes).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            List<Brush> brushes = [];
            foreach (System.Reflection.PropertyInfo item in inu)
            {
                if (item.GetValue(null) is Brush b)
                {
                    brushes.Add(b);
                }
            }
            MyComboBoxBackgroundBrush.ItemsSource = brushes;
            //var ika = typeof(Brushes).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).ToDictionary(x => x.Name, x => x.GetValue(null) as Brush);
            //MyComboBoxBackgroundBrush2.ItemsSource = ika;


            //Dictionary<string, Brush> keyValuePairs = [];
            //foreach (var item in ika)
            //{
            //    if (item.GetType() == typeof(Brush) && item.Value != null)
            //    {
            //        keyValuePairs.Add(item.Key, item.Value);
            //    }
            //}

        }

        #region 依存関係プロパティ

        /// <summary>
        /// Thumb追加時のスライド量
        /// </summary>
        public int MySlideHorizontal
        {
            get { return (int)GetValue(MySlideHorizontalProperty); }
            set { SetValue(MySlideHorizontalProperty, value); }
        }
        public static readonly DependencyProperty MySlideHorizontalProperty =
            DependencyProperty.Register(nameof(MySlideHorizontal), typeof(int), typeof(KisoThumb), new PropertyMetadata(32));

        public int MySlideVertical
        {
            get { return (int)GetValue(MySlideVerticalProperty); }
            set { SetValue(MySlideVerticalProperty, value); }
        }
        public static readonly DependencyProperty MySlideVerticalProperty =
            DependencyProperty.Register(nameof(MySlideVertical), typeof(int), typeof(KisoThumb), new PropertyMetadata(32));

        #endregion 依存関係プロパティ

        private void AddEllipseText(string text, Brush fill, double width, double height)
        {
            EllipseTextThumb thumb = new() { MyText = text, MyFill = fill, MyWidth = width, MyHeight = height, MyLeft = MySlideHorizontal, MyTop = MySlideVertical };
            MyRootGroup.AddThumbToActiveGroup(thumb);
        }
        private void MyRootGroup_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            GotPreKey.Text = e.Source.GetType().ToString();
        }

        private void MyRootGroup_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void MyRootGroup_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            GotKey.Text = e.NewFocus.ToString();
        }

        private void MyButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEllipseText($"{DateTime.Now:ss秒FFF}", Brushes.YellowGreen, 50, 50);
        }

        private void MyButtonTest_Click(object sender, RoutedEventArgs e)
        {
            MyRootGroup.ActiveGroupToOutside();
        }

        private void MyButtonTest2_Click(object sender, RoutedEventArgs e)
        {
            MyRootGroup.ActiveGroupToInside();
        }

        private void MyButtonTest3_Click(object sender, RoutedEventArgs e)
        {
            //MyRootGroup.ClickedThumbToActiveGroupThumb();
        }

        private void MyButtonTest4_Click(object sender, RoutedEventArgs e)
        {
            MyRootGroup.ActiveGroupFromClickedThumbsParent();
        }

        private void MyScrollV_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {

            //e.Handled = true;
        }

        private void MyScrollV_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {

        }

        private void MyButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            MyRootGroup.RemoveSelectedThumbsFromActiveGroup();
        }

        private void MyButtonRemoveAll_Click(object sender, RoutedEventArgs e)
        {
            MyRootGroup.RemoveAll();
        }

        private void MyButtonMakeGroup_Click(object sender, RoutedEventArgs e)
        {
            MyRootGroup.AddGroupFromSelected();
        }

        private void MyButonUngroup_Click(object sender, RoutedEventArgs e)
        {
            MyRootGroup.UngroupFocusThumb();
        }
    }


    //public class EXFV : ScrollViewer
    //{

    //    protected override Size ArrangeOverride(Size arrangeSize)
    //    {
    //        return base.ArrangeOverride(arrangeSize);
    //    }
    //}
}