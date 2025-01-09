using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250109_SelectedThumbs
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
