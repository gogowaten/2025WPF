using System.IO;
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

namespace _20250424
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SavePng(MyTextBlock);
        }

        private void SavePng(FrameworkElement element)
        {
            var enc = new PngBitmapEncoder();
            var bounds = VisualTreeHelper.GetDescendantBounds(MyCanvas);
            var textblocksize = MyTextBlock.RenderSize;
            var dv = new DrawingVisual();
            using (var context = dv.RenderOpen())
            {
                BitmapCacheBrush bru = new(element);
                bounds = new Rect(0, 0, element.ActualWidth, element.ActualHeight);
                context.DrawRectangle(bru, null, bounds);
            }
            RenderTargetBitmap bmp = new((int)bounds.Width, (int)bounds.Height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(dv);

            enc.Frames.Add(BitmapFrame.Create(bmp));
            using FileStream stream = new($"E:20250424.png", FileMode.Create, FileAccess.Write);
            enc.Save(stream);
        }
    }
}