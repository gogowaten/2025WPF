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


        private void MyButtonTest_Click(object sender, RoutedEventArgs e)
        {
            

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