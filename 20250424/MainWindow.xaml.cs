using System.IO;
using System.Reflection;
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
//要素を画像として保存
//TextBlock
//Paddingが0だとズレてぼやけることがある
//フォントや文字によっては、ずれないときもある
//対象は左端の文字、1文字目だけ
//左端の文字がTだとずれる、けどUだとずれない
//防ぐにはPaddingを1以上にするしかない？
//ずれるかどうかの判定は
//VisualTreeHelper.GetDescendantBounds(element);
//これで得られるRectのX座標がマイナスの値だとずれる、0ならずれない

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

            //SavePng(MyTextBlock);
            string fileName = $"E:20250424_";
            Test2WithClearType(MyUC, fileName);
            Test2(MyUC, fileName);
            Test3(MyUC, fileName);
            Test4(MyUC, fileName);
            Test5(MyUC, fileName);
            Test6(MyUC, fileName);
        }


        //よくわからん
        private void Test6(FrameworkElement element, string fileName)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(element);

            var dv = new DrawingVisual();
            dv.Offset = new Vector(-bounds.X, -bounds.Y);
            bounds.Width += bounds.X;
            bounds.Height += bounds.Y;
            using (var context = dv.RenderOpen())
            {
                BitmapCacheBrush bru = new(element);
                context.DrawRectangle(bru, null, bounds);
            }
            RenderTargetBitmap bmp = new((int)Math.Ceiling(bounds.Width), (int)Math.Ceiling(bounds.Height), 96, 96, PixelFormats.Pbgra32);
            bmp.Render(dv);

            string? methodName = MethodBase.GetCurrentMethod()?.Name;
            Save(bmp, $"{fileName}{methodName}.png");
        }

        //GetDescendantBoundsのサイズと描画位置を0,0で保存
        private void Test5(FrameworkElement element, string fileName)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(element);
            bounds = new Rect(0, 0, bounds.Width, bounds.Height);

            var dv = new DrawingVisual();
            using (var context = dv.RenderOpen())
            {
                BitmapCacheBrush bru = new(element);
                context.DrawRectangle(bru, null, bounds);
            }
            RenderTargetBitmap bmp = new((int)Math.Ceiling(bounds.Width), (int)Math.Ceiling(bounds.Height), 96, 96, PixelFormats.Pbgra32);
            bmp.Render(dv);

            string? methodName = MethodBase.GetCurrentMethod()?.Name;
            Save(bmp, $"{fileName}{methodName}.png");
        }

        //GetDescendantBoundsのサイズとDrawingVisualをオフセットして保存
        private void Test4(FrameworkElement element, string fileName)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(element);

            var dv = new DrawingVisual();
            dv.Offset = new Vector(-bounds.X, -bounds.Y);
            using (var context = dv.RenderOpen())
            {
                BitmapCacheBrush bru = new(element);
                context.DrawRectangle(bru, null, bounds);
            }
            RenderTargetBitmap bmp = new((int)Math.Ceiling(bounds.Width), (int)Math.Ceiling(bounds.Height), 96, 96, PixelFormats.Pbgra32);
            bmp.Render(dv);

            string? methodName = MethodBase.GetCurrentMethod()?.Name;
            Save(bmp, $"{fileName}{methodName}.png");
        }

        //GetDescendantBoundsのサイズと位置で保存
        private void Test3(FrameworkElement element, string fileName)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(element);

            var dv = new DrawingVisual();
            using (var context = dv.RenderOpen())
            {
                BitmapCacheBrush bru = new(element);
                context.DrawRectangle(bru, null, bounds);
            }
            RenderTargetBitmap bmp = new((int)Math.Ceiling(bounds.Width), (int)Math.Ceiling(bounds.Height), 96, 96, PixelFormats.Pbgra32);
            bmp.Render(dv);

            string? methodName = MethodBase.GetCurrentMethod()?.Name;
            Save(bmp, $"{fileName}{methodName}.png");
        }

        //普通に要素のサイズで、BitmapCacheBrushで描画
        private void Test2(FrameworkElement element, string fileName)
        {
            Rect bounds = new(0, 0, element.ActualWidth, element.ActualHeight);

            var dv = new DrawingVisual();
            using (var context = dv.RenderOpen())
            {
                BitmapCacheBrush bru = new(element);
                context.DrawRectangle(bru, null, bounds);
            }
            RenderTargetBitmap bmp = new((int)Math.Ceiling(bounds.Width), (int)Math.Ceiling(bounds.Height), 96, 96, PixelFormats.Pbgra32);
            bmp.Render(dv);

            string? methodName = MethodBase.GetCurrentMethod()?.Name;
            Save(bmp, $"{fileName}{methodName}.png");
        }

        //BitmapCacheBrushのキャッシュモードのEnableClearTypeを有効にして、描画した画像を保存
        //要素に文字列があるとき、ClearTypeが有効時の見た目で保存される
        private void Test2WithClearType(FrameworkElement element, string fileName)
        {
            Rect bounds = new(0, 0, element.ActualWidth, element.ActualHeight);

            var dv = new DrawingVisual();
            using (var context = dv.RenderOpen())
            {
                BitmapCacheBrush bru = new(element);
                BitmapCache bcMode = new() { EnableClearType = true };
                bru.BitmapCache = bcMode;
                context.DrawRectangle(bru, null, bounds);
            }

            RenderTargetBitmap bmp = new((int)Math.Ceiling(bounds.Width), (int)Math.Ceiling(bounds.Height), 96, 96, PixelFormats.Pbgra32);
            bmp.Render(dv);

            string? methodName = MethodBase.GetCurrentMethod()?.Name;
            Save(bmp, $"{fileName}{methodName}.png");
        }

        private void Save(BitmapSource bmp, string fileName)
        {
            var enc = new PngBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(bmp));
            using FileStream stream = new(fileName, FileMode.Create, FileAccess.Write);
            enc.Save(stream);
        }
    }
}