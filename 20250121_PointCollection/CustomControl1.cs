using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250121_PointCollection
{

    public class CustomControl1 : Control
    {
        static CustomControl1()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomControl1), new FrameworkPropertyMetadata(typeof(CustomControl1)));
        }
    }

    public class PolylineThumb : Thumb
    {
        #region 依存関係プロパティ

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(PolylineThumb), new PropertyMetadata(null));

        public double MyStrokeThickness
        {
            get { return (double)GetValue(MyStrokeThicknessProperty); }
            set { SetValue(MyStrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeThicknessProperty =
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(PolylineThumb), new PropertyMetadata(null));

        public Brush MyStroke
        {
            get { return (Brush)GetValue(MyStrokeProperty); }
            set { SetValue(MyStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeProperty =
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(PolylineThumb), new PropertyMetadata(Brushes.Red));

        public Polyline MyPolyline
        {
            get { return (Polyline)GetValue(MyPolylineProperty); }
            set { SetValue(MyPolylineProperty, value); }
        }
        public static readonly DependencyProperty MyPolylineProperty =
            DependencyProperty.Register(nameof(MyPolyline), typeof(Polyline), typeof(PolylineThumb), new PropertyMetadata(null));

        public double MyPolylineAngle
        {
            get { return (double)GetValue(MyPolylineAngleProperty); }
            set { SetValue(MyPolylineAngleProperty, value); }
        }
        public static readonly DependencyProperty MyPolylineAngleProperty =
            DependencyProperty.Register(nameof(MyPolylineAngle), typeof(double), typeof(PolylineThumb), new PropertyMetadata(0.0));


        public Brush MyBackground
        {
            get { return (Brush)GetValue(MyBackgroundProperty); }
            set { SetValue(MyBackgroundProperty, value); }
        }
        public static readonly DependencyProperty MyBackgroundProperty =
            DependencyProperty.Register(nameof(MyBackground), typeof(Brush), typeof(PolylineThumb), new PropertyMetadata(null));



        #endregion 依存関係プロパティ
        static PolylineThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PolylineThumb), new FrameworkPropertyMetadata(typeof(PolylineThumb)));
        }
        public PolylineThumb()
        {

            DataContext = this;
            DragDelta += PolylineThumb_DragDelta;

        }

        private void PolylineThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                Canvas.SetLeft(t, Canvas.GetLeft(t) + e.HorizontalChange);
                Canvas.SetTop(t, Canvas.GetTop(t) + e.VerticalChange);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_Polyline") is Polyline poly)
            {
                MyPolyline = poly;
                MyPolyline.SizeChanged += MyPolyline_SizeChanged;
            }
        }

        private void MyPolyline_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Test2();
            var bounds = VisualTreeHelper.GetDescendantBounds(MyPolyline);
            Transform neko = MyPolyline.RenderTransform;
            RotateTransform rotate = (RotateTransform)neko;
            var rrect = rotate.TransformBounds(bounds);

            if (bounds.Width > 0)
            {
                this.Width = bounds.Width;
                this.Height = bounds.Height;
                Canvas.SetLeft(MyPolyline, -bounds.Left);
                Canvas.SetTop(MyPolyline, -bounds.Top);

            }
        }


        //      WPF、PolylineをぴったりサイズのBitmapSourceとして取得できた！PolylineよりPath使った方がいい - 午後わてんのブログ
        //https://gogowaten.hatenablog.com/entry/2023/03/08/144258#Polyline%E3%81%8B%E3%82%89BitmapSource

        private void Test2()
        {
            PathFigure fig = new() { StartPoint = MyPoints[0] };
            fig.Segments.Add(new PolyLineSegment(MyPoints.Skip(1), true));
            PathGeometry fillPg = new();
            fillPg.Figures.Add(fig);

            Pen pen = new(MyStroke, MyStrokeThickness);
            PathGeometry strokePg = fillPg.GetWidenedPathGeometry(pen);
            strokePg.Transform = MyPolyline.RenderTransform;
            Rect rect = strokePg.Bounds;

            //fillPg.Transform = MyPolyline.RenderTransform;

            DrawingVisual dv = new() { Offset = new Vector(-rect.X, -rect.Y) };
            using (var context = dv.RenderOpen())
            {
                //context.DrawGeometry(Brushes.Red, null, fillPg);
                context.DrawGeometry(pen.Brush, null, strokePg);
            }
            RenderTargetBitmap bmp = new((int)(rect.Width + 1), (int)(rect.Height + 1), 96, 96, PixelFormats.Pbgra32);
            bmp.Render(dv);
            BitmapToPngImageToClipboard(bmp);
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




    }





}
