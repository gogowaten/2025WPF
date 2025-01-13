using System.Windows;
using System.Windows.Media;

namespace _20250113_GroupUngroup
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

        private void MyButtonTest4_Click(object sender, RoutedEventArgs e)
        {
            MyRootGroup.ActiveGroupFromClickedThumbsParent();
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

}