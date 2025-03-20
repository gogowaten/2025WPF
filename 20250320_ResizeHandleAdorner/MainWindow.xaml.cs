using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace _20250320_ResizeHandleAdorner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MyComboBox.ItemsSource = Enum.GetValues(typeof(HandleLayoutType));
            MyComboBox.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(AdornerLayer.GetAdornerLayer(MyElement) is AdornerLayer layer)
            {
                var adorner = new ResizeHandleAdorner(MyElement);
                adorner.UseLayoutRounding = true;
                adorner.SetBinding(ResizeHandleAdorner.MyHandleLayoutProperty, new Binding() { Source = MyComboBox, Path = new PropertyPath(ComboBox.SelectedItemProperty) });
                adorner.SetBinding(ResizeHandleAdorner.MyHandleSizeProperty, new Binding() { Source = MySlider, Path = new PropertyPath(Slider.ValueProperty) });

                layer.Add(adorner);

            }
        }
    }
}