using System.Windows;

namespace _20250103_ZIndex
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


        private void MyButtonZTop_Click(object sender, RoutedEventArgs e)
        {
            if (MyRootGroup.MyFocusThumb is KisoThumb kiso)
            {
                kiso.ZIndexTop();
            }
        }

        private void MyButtonZUp_Click(object sender, RoutedEventArgs e)
        {
            if (MyRootGroup.MyFocusThumb is KisoThumb kiso)
            {
                kiso.ZIndexUp();
            }
        }

        private void MyButtonZDown_Click(object sender, RoutedEventArgs e)
        {
            if (MyRootGroup.MyFocusThumb is KisoThumb kiso)
            {
                kiso.ZIndexDown();
            }
        }

        private void MyButtonZBottom_Click(object sender, RoutedEventArgs e)
        {
            if (MyRootGroup.MyFocusThumb is KisoThumb kiso)
            {
                kiso.ZIndexBottom();
            }
        }
    }
}