using System.Windows;

//WPFでテストアプリ、複数選択と枠表示 - 午後わてんのブログ
//https://gogowaten.hatenablog.com/entry/2025/01/09/185820

namespace _20250109_SelectedThumbs
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

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


    }

}
