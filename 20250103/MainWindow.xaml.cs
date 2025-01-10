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

namespace _20250103
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
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
            GroupThumb? group = null;
            if (MyRootGroup.MyFocusThumb is GroupThumb gt)
            {
                group = gt;
            }
            else if (MyRootGroup.MyFocusThumb.MyParentThumb is GroupThumb kiso)
            {
                group = kiso;
            }
            if (group != null)
            {
                KisoThumb? kiso = null;
                if (MyComboBoxThumbType.SelectedItem is ThumbType.Text)
                {
                    kiso = new TextBlockThumb();
                    kiso.MyText = MyTextBoxMyText.Text;
                    kiso.Background = MyComboBoxBackgroundBrush.SelectedItem as Brush;
                }
                if (kiso != null)
                {
                    group.MyThumbs.Add(kiso);
                }
            }
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


    }


    public class EXFV : ScrollViewer
    {
        
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            return base.ArrangeOverride(arrangeSize);
        }
    }
}