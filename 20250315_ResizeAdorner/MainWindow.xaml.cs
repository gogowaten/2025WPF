using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace _20250315_ResizeAdorner;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void MyButtonChangeHandleVisible_Click(object sender, RoutedEventArgs e)
    {
        ResizeAdorner? adorner = SwitchAdorner(MyTarget);
        MyStackPanel.DataContext = adorner;
        _ = SwitchAdorner(MyButton);
        _ = SwitchAdorner(MyButtonInStackPanel);
        _ = SwitchAdorner(MyEllipse);
    }

    /// <summary>
    /// 対象にリサイズ用のハンドル(装飾)を付け外しする
    /// </summary>
    /// <param name="elem">対象要素</param>
    /// <returns>装飾</returns>
    private static ResizeAdorner? SwitchAdorner(FrameworkElement elem)
    {
        if (AdornerLayer.GetAdornerLayer(elem) is AdornerLayer layer)
        {
            var items = layer.GetAdorners(elem);
            if (items != null)
            {
                foreach (var item in items.OfType<ResizeAdorner>())
                {
                    layer.Remove(item);
                }
                return null;
            }
            else
            {
                var adorner = new ResizeAdorner(elem);
                layer.Add(adorner);
                return adorner;
            }
        }
        return null;
    }

    private void MyButtonChangeBackground_Click(object sender, RoutedEventArgs e)
    {
        if (MyGrid.Background == null || MyGrid.Background == Brushes.White)
        {
            MyGrid.Background = Brushes.Black;
        }
        else
        {
            MyGrid.Background = Brushes.White;
        }
    }

}