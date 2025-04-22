using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

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
            var bmp = GetAreaBitmap();
            MyRootThumb.SaveBitmap(bmp, 90);
        }


        //AreaThumb用、選択範囲の画像作成
        private RenderTargetBitmap GetAreaBitmap()
        {
            //描画のRect取得
            var bounds = MyAreaThumb.TransformToVisual(MyRootThumb)
                .TransformBounds(VisualTreeHelper.GetDescendantBounds(MyAreaThumb));
            DrawingVisual dv = new() { Offset = new Vector(-bounds.X, -bounds.Y) };
            using (var context = dv.RenderOpen())
            {
                VisualBrush vb = new(MyRootThumb) { Stretch = Stretch.None };
                context.DrawRectangle(vb, null, VisualTreeHelper.GetDescendantBounds(MyRootThumb));
            }
            RenderTargetBitmap bitmap = new(
                (int)Math.Ceiling(bounds.Width), (int)Math.Ceiling(bounds.Height)
                , 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(dv);
            return bitmap;
        }

        #region 右クリックメニュー
        private ContextMenu MakeContext()
        {
            ContextMenu menu = new();
            MenuItem item;
            item = new() { Header = "グループ化" }; menu.Items.Add(item);
            //item.Click += (s, e) => { MyRoot.AddGroup(); };
            //item = new() { Header = "画像で複製" }; menu.Items.Add(item);
            //item.Click += (s, e) => { MyRoot.DuplicateImageSelectedThumbs(); };
            //item = new() { Header = "Dataで複製" }; menu.Items.Add(item);
            //item.Click += (s, e) => { MyRoot.DuplicateDataSelectedThumbs(); };

            //menu.Items.Add(new Separator() { Height = 10 });
            //item = new() { Header = "削除" }; menu.Items.Add(item);
            //item.Click += (s, e) => { MyRoot.RemoveThumb(); };
            //menu.Items.Add(new Separator() { Height = 10 });

            ////item = new() { Header = "Z移動" };
            //menu.Items.Add(MakeMenuItemZMove());

            //item = new() { Header = "複数選択解除" }; menu.Items.Add(item);
            //item.Click += (s, e) => { throw new ArgumentException(); };

            return menu;
        }


        #endregion 右クリックメニュー

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