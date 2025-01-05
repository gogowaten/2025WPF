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
            //MyRootGroup.FocusThumbToActiveGroupThumb();
            //MyRootGroup.ClickedThumbToActiveGroupThumb();
            MyRootGroup.ClickedThumbsParentToActiveGroupThumb();
        }
    }
}