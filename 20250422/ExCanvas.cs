using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Diagnostics;
using System.Xml.Linq;

namespace _20250422
{
    public class ManageExCanvas : ExCanvas
    {
        public RootThumb MyRootThumb { get; set; }
        public AreaThumb MyAreaThumb { get; set; }
        private ContextMenu MyContextMenuForAreaThumb { get; set; } = new();

        //public ManageExCanvas() { }
        public ManageExCanvas(RootThumb rootThumb)
        {
            MyRootThumb = rootThumb;
            MyAreaThumb = new();
            Children.Add(MyRootThumb);
            Children.Add(MyAreaThumb);

            MyAreaThumb.Width = 100;
            MyAreaThumb.Height = 100;
            MyAreaThumb.Opacity = 0.5;
            MyAreaThumb.Background = Brushes.Red;
            InitializeContextMenu();
            MyAreaThumb.ContextMenu = MyContextMenuForAreaThumb;
        }


        private void InitializeContextMenu()
        {
            MenuItem item;
            item = new() { Header = "範囲を画像として保存" };
            MyContextMenuForAreaThumb.Items.Add(item);
            item.Click += Item_Click;
        }

        private void Item_Click(object sender, RoutedEventArgs e)
        {
            var bmp = GetAreaBitmap2();
            RootThumb.SaveBitmap(bmp, 90);
        }

        //WPF/C# コントロールの要素をキャプチャする #C# - Qiita
        //https://qiita.com/Sakurai-Shinya/items/81a9c413c3265f0e8587

        //AreaThumb用、選択範囲の画像作成
        private RenderTargetBitmap GetAreaBitmap2()
        {
            //描画のRect取得
            var bounds = MyAreaThumb.TransformToVisual(MyRootThumb)
                .TransformBounds(VisualTreeHelper.GetDescendantBounds(MyAreaThumb));

            var tformat = TextOptions.GetTextFormattingMode(MyRootThumb);//Ideal
            var hint = TextOptions.GetTextHintingMode(MyRootThumb);//Auto
            var rend = TextOptions.GetTextRenderingMode(MyRootThumb);//Auto
            //これらは保存される画像には影響しない
            //TextOptions.SetTextFormattingMode(MyRootThumb, TextFormattingMode.Display);
            //TextOptions.SetTextRenderingMode(MyRootThumb, TextRenderingMode.Grayscale);
            //TextOptions.SetTextHintingMode(MyRootThumb, TextHintingMode.Animated);

            //これらも変化なし、影響ない
            var chint = RenderOptions.GetCachingHint(MyRootThumb);//Unspecified
            var crehint = RenderOptions.GetClearTypeHint(MyRootThumb);//Auto
            //RenderOptions.SetCachingHint(MyRootThumb, CachingHint.Cache);
            //RenderOptions.SetClearTypeHint(MyRootThumb, ClearTypeHint.Enabled);

            //RootThumbのキャッシュモードのフォントのClearTypeを有効にする
            BitmapCache bc = new() { EnableClearType = true };
            MyRootThumb.CacheMode = bc;
            DrawingVisual dv = new() { Offset = new Vector(-bounds.X, -bounds.Y) };
            using (var context = dv.RenderOpen())
            {
                Rect drect = VisualTreeHelper.GetDescendantBounds(MyRootThumb);
                BitmapCacheBrush vb = new(MyRootThumb);
                //VisualBrush vb = new(MyRootThumb) { Stretch = Stretch.None };
                context.DrawRectangle(vb, null, drect);
                //dvc = dv.ContentBounds;
            }
            //dvc = dv.ContentBounds;
            RenderTargetBitmap bitmap = new(
                (int)Math.Ceiling(bounds.Width),
                (int)Math.Ceiling(bounds.Height)
                , 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(dv);
            //キャッシュモードを元のnullに戻す
            MyRootThumb.CacheMode = null;
            return bitmap;
        }

        private RenderTargetBitmap GetAreaBitmap()
        {
            //描画のRect取得
            var bounds = MyAreaThumb.TransformToVisual(MyRootThumb)
                .TransformBounds(VisualTreeHelper.GetDescendantBounds(MyAreaThumb));
            var neko = Canvas.GetLeft(MyAreaThumb);
            var inu = GetTop(MyAreaThumb);
            var tako = MyAreaThumb.TransformToVisual(MyRootThumb);
            var ika = VisualTreeHelper.GetDescendantBounds(MyAreaThumb);
            var uma = tako.TransformBounds(ika);
            var saru = VisualTreeHelper.GetDescendantBounds(MyRootThumb);
            var rw = MyRootThumb.Width;
            var rh = MyRootThumb.Height;
            //DrawingVisual dv = new() { Offset = new Vector(-bounds.X - saru.X, -bounds.Y - saru.Y) };
            //DrawingVisual dv = new() { Offset = new Vector(-bounds.X + saru.X, -bounds.Y + saru.Y) };
            //DrawingVisual dv = new() { Offset = new Vector(-bounds.X, -bounds.Y) };
            DrawingVisual dv = new();
            BitmapCache bitmapCache = new();
            bitmapCache.SnapsToDevicePixels = true;
            dv.CacheMode = bitmapCache;
            using (var context = dv.RenderOpen())
            {
                BitmapCacheBrush vb = new(MyRootThumb);
                //VisualBrush vb = new(MyRootThumb) { Stretch = Stretch.None };
                VisualBrush vvv = new(MyRootThumb);
                vvv.Stretch = Stretch.None;
                vvv.AlignmentX = AlignmentX.Left;
                vvv.AlignmentY = AlignmentY.Top;
                vvv.TileMode = TileMode.None;

                BitmapCache bc = new(1.0);
                bc.EnableClearType = true;
                bc.SnapsToDevicePixels = true;
                vb.BitmapCache = bc;

                RenderOptions.SetEdgeMode(vvv, EdgeMode.Aliased);
                RenderOptions.SetEdgeMode(vb, EdgeMode.Aliased);

                //context.DrawRectangle(vb, null, new Rect(0, 0, MyRootThumb.Width, MyRootThumb.Height));
                context.DrawRectangle(vvv, null, new Rect(0, 0, saru.Width, saru.Height));
                //context.DrawRectangle(vb, null, VisualTreeHelper.GetDescendantBounds(MyRootThumb));
            }
            RenderTargetBitmap bitmap = new(
                (int)Math.Ceiling(saru.Width),
                (int)Math.Ceiling(saru.Height),
                96, 96, PixelFormats.Pbgra32);
            bitmap.Render(dv);
            return bitmap;
        }

        public RenderTargetBitmap GetRenderTargetBitmap(FrameworkElement element)
        {
            //描画のRect取得
            var bounds = element.TransformToVisual(MyRootThumb)
                .TransformBounds(VisualTreeHelper.GetDescendantBounds(element));
            DrawingVisual dv = new() { Offset = new Vector(-bounds.X, -bounds.Y) };
            using (var context = dv.RenderOpen())
            {
                BitmapCacheBrush vb = new(MyRootThumb);
                //VisualBrush vb = new(MyRootThumb) { Stretch = Stretch.None };
                context.DrawRectangle(vb, null, VisualTreeHelper.GetDescendantBounds(MyRootThumb));
            }
            RenderTargetBitmap bitmap = new(
                (int)Math.Ceiling(bounds.Width), (int)Math.Ceiling(bounds.Height)
                , 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(dv);
            return bitmap;
        }

        #region 画像保存




        //public bool SaveBitmap2(BitmapSource bitmap)
        //{
        //    Microsoft.Win32.SaveFileDialog dialog = new()
        //    {
        //        Filter = "*.png|*.png|*.jpg|*.jpg;*.jpeg|*.bmp|*.bmp|*.gif|*.gif|*.tiff|*.tiff",
        //        AddExtension = true,
        //    };
        //    if (dialog.ShowDialog() == true)
        //    {
        //        (BitmapEncoder? encoder, BitmapMetadata? meta) = GetEncoderWithMetaData(dialog.FilterIndex);
        //        if (encoder is null) { return false; }
        //        encoder.Frames.Add(BitmapFrame.Create(bitmap, null, meta, null));
        //        try
        //        {
        //            using FileStream stream = new(dialog.FileName, FileMode.Create, FileAccess.Write);
        //            encoder.Save(stream);
        //            return true;
        //        }
        //        catch (Exception)
        //        {
        //            return false;
        //        }
        //    }
        //    return false;
        //}

        #endregion 画像保存
    }



    /// <summary>
    /// 子要素全体が収まるようにサイズが自動変化するCanvas
    /// ただし、子要素のマージンとパディングは考慮していないし
    /// ArrangeOverrideを理解していないので不具合があるかも
    /// </summary>
    public class ExCanvas : Canvas
    {
        private bool isAutoResize = true;

        /// <summary>
        /// 自動リサイズの切り替えフラグ
        /// falseからtrueに変更時はInvalidateArrangeを実行してリサイズ
        /// </summary>
        public bool IsAutoResize
        {
            get => isAutoResize;
            set
            {
                if (isAutoResize != value)
                {
                    isAutoResize = value;
                    if (value) { InvalidateArrange(); }
                }
            }
        }

        public ExCanvas()
        {
            Loaded += ExCanvas_Loaded;
        }

        private void ExCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ActualWidthProperty) });
            SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ActualHeightProperty) });
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            //if (double.IsNaN(Width) && double.IsNaN(Height) && IsAutoResize)
            if (IsAutoResize)
            {
                base.ArrangeOverride(arrangeSize);
                Size resultSize = new();
                foreach (var item in Children.OfType<FrameworkElement>())
                {

                    double x = GetLeft(item) + item.ActualWidth;
                    if (double.IsNaN(x)) { x = 0 + item.ActualWidth; }
                    double y = GetTop(item) + item.ActualHeight;
                    if (double.IsNaN(y)) { y = 0 + item.ActualHeight; }
                    if (resultSize.Width < x) resultSize.Width = x;
                    if (resultSize.Height < y) resultSize.Height = y;
                }
                //base.ArrangeOverride(resultSize);
                return resultSize;
            }
            else
            {
                return base.ArrangeOverride(arrangeSize);
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            return base.MeasureOverride(constraint);
        }
    }


}