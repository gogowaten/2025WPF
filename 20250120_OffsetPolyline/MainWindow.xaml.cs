using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
//WPF、カスタムコントロールのThumbに表示するPolylineはGridよりCanvasに乗せると都合がいい - 午後わてんのブログ
//https://gogowaten.hatenablog.com/entry/2025/01/23/125044

namespace _20250120_OffsetPolyline
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MyButton1_Click(object sender, RoutedEventArgs e)
        {
            MyLine1.MyStrokeThickness += 10;
        }

        private void MyButton2_Click(object sender, RoutedEventArgs e)
        {
            if (MyLine1.MyStrokeThickness > 10)
            {
                MyLine1.MyStrokeThickness -= 10;
            }
        }

        private void MyButton3_Click(object sender, RoutedEventArgs e)
        {
            MyLine2.MyStrokeThickness += 10;
        }

        private void MyButton4_Click(object sender, RoutedEventArgs e)
        {
            if (MyLine2.MyStrokeThickness > 10)
            {
                MyLine2.MyStrokeThickness -= 10;
            }
        }

        private void MyButton5_Click(object sender, RoutedEventArgs e)
        {
            Point p = MyLine1.MyPoints[1];
            MyLine1.MyPoints[1] = new Point(p.X + 10, p.Y + 10);
        }

        private void MyButton6_Click(object sender, RoutedEventArgs e)
        {
            Point p = MyLine2.MyPoints[1];
            MyLine2.MyPoints[1] = new Point(p.X + 10, p.Y + 10);
        }

        /// <summary>
        /// 要素を画像に変換、ただし回転などの変形された要素には非対応
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private static RenderTargetBitmap ElementToBitmap(FrameworkElement element)
        {
            DrawingVisual dv = new();
            double w = element.ActualWidth;
            double h = element.ActualHeight;
            using (DrawingContext dc = dv.RenderOpen())
            {
                dc.DrawRectangle(new VisualBrush(element), null, new Rect(0, 0, w, h));
            }
            var bmp = new RenderTargetBitmap((int)w, (int)h, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(dv);
            return bmp;
        }

        //アルファ値を失わずに画像のコピペできた、.NET WPFのClipboard - 午後わてんのブログ
        //        https://gogowaten.hatenablog.com/entry/2021/02/10/134406
        //より
        private static void BitmapToPngImageToClipboard(BitmapSource source)
        {
            //画像をPNGにエンコード
            PngBitmapEncoder pngEnc = new();
            pngEnc.Frames.Add(BitmapFrame.Create(source));
            //エンコードした画像をMemoryStreamにSava
            using var ms = new System.IO.MemoryStream();
            pngEnc.Save(ms);
            //MemoryStreamをクリップボードにコピー
            Clipboard.SetData("PNG", ms);
        }

        private void MyButton7_Click(object sender, RoutedEventArgs e)
        {
            BitmapToPngImageToClipboard(ElementToBitmap(MyLine1));
        }

        private void MyButton8_Click(object sender, RoutedEventArgs e)
        {
            BitmapToPngImageToClipboard(ElementToBitmap(MyLine2));
        }
    }
}