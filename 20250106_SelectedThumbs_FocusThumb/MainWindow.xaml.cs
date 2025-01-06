using System.Windows;

namespace _20250106_SelectedThumbs_FocusThumb
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void MyButtonTest_Click(object sender, RoutedEventArgs e)
        {
            MyRootGroup.ActiveThumbParentToActiveGroupThumb();
        }

        private void MyButtonTest2_Click(object sender, RoutedEventArgs e)
        {
            MyRootGroup.FocusThumbToActiveGroupThumb();
        }

        private void MyButtonTest3_Click(object sender, RoutedEventArgs e)
        {
            //MyRootGroup.ClickedThumbToActiveGroupThumb();
        }

        private void MyButtonTest4_Click(object sender, RoutedEventArgs e)
        {
            MyRootGroup.ClickedThumbsParentToActiveGroupThumb();
        }
    }
}