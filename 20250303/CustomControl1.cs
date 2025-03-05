using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250303
{

    public class LineThumb : Thumb
    {

        static LineThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LineThumb), new FrameworkPropertyMetadata(typeof(LineThumb)));
        }
        public LineThumb()
        {

            DragDelta += LineThumb_DragDelta;
            Loaded += LineThumb_Loaded;
        }

        private void LineThumb_Loaded(object sender, RoutedEventArgs e)
        {
            Relayout();

        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("line") is EzLine line)
            {
                MyEzLine = line;
                if (MyPoints is null)
                {
                    MyPoints = MyEzLine.MyPoints;
                }
                else
                {
                    MyEzLine.MyPoints = MyPoints;
                }
            }
            if (GetTemplateChild("PART_Canvas") is Canvas panel)
            {
                MyCanvas = panel;
            }

        }

        private void LineThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
            Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
            e.Handled = true;
        }



        #region 依存関係プロパティ

        public double MyLeft
        {
            get { return (double)GetValue(MyLeftProperty); }
            set { SetValue(MyLeftProperty, value); }
        }
        public static readonly DependencyProperty MyLeftProperty =
            DependencyProperty.Register(nameof(MyLeft), typeof(double), typeof(LineThumb), new PropertyMetadata(0.0));

        public double MyTop
        {
            get { return (double)GetValue(MyTopProperty); }
            set { SetValue(MyTopProperty, value); }
        }
        public static readonly DependencyProperty MyTopProperty =
            DependencyProperty.Register(nameof(MyTop), typeof(double), typeof(LineThumb), new PropertyMetadata(0.0));


        public double MyOffsetLeft
        {
            get { return (double)GetValue(MyOffsetLeftProperty); }
            set { SetValue(MyOffsetLeftProperty, value); }
        }
        public static readonly DependencyProperty MyOffsetLeftProperty =
            DependencyProperty.Register(nameof(MyOffsetLeft), typeof(double), typeof(LineThumb), new PropertyMetadata(0.0));

        public double MyOffsetTop
        {
            get { return (double)GetValue(MyOffsetTopProperty); }
            set { SetValue(MyOffsetTopProperty, value); }
        }
        public static readonly DependencyProperty MyOffsetTopProperty =
            DependencyProperty.Register(nameof(MyOffsetTop), typeof(double), typeof(LineThumb), new PropertyMetadata(0.0));


        public Canvas MyCanvas
        {
            get { return (Canvas)GetValue(MyCanvasProperty); }
            set { SetValue(MyCanvasProperty, value); }
        }
        public static readonly DependencyProperty MyCanvasProperty =
            DependencyProperty.Register(nameof(MyCanvas), typeof(Canvas), typeof(LineThumb), new PropertyMetadata(null));

        public EzLine MyEzLine
        {
            get { return (EzLine)GetValue(MyEzLineProperty); }
            set { SetValue(MyEzLineProperty, value); }
        }
        public static readonly DependencyProperty MyEzLineProperty =
            DependencyProperty.Register(nameof(MyEzLine), typeof(EzLine), typeof(LineThumb), new PropertyMetadata(null));

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(LineThumb), new PropertyMetadata(null));
        #endregion 依存関係プロパティ

        /// <summary>
        /// 再描画
        /// </summary>
        public void Relayout()
        {
            var r4 = MyEzLine.MyBounds4;
            Width = r4.Width;
            Height = r4.Height;

            //変更する前の位置を使って計算しておく、タイミング重要
            double tasLeft = Canvas.GetLeft(MyEzLine) + Canvas.GetLeft(this) + r4.Left;
            double tasTop = Canvas.GetTop(MyEzLine) + Canvas.GetTop(this) + r4.Top;

            //図形の位置を変更、オフセット
            Canvas.SetLeft(MyEzLine, -r4.Left);
            Canvas.SetTop(MyEzLine, -r4.Top);

            //自身の位置を変更、図形の位置に合わせる
            Canvas.SetLeft(this, tasLeft);
            Canvas.SetTop(this, tasTop);
            //Canvas.SetLeft(this, Canvas.GetLeft(this) + tasLeft);
            //Canvas.SetTop(this, Canvas.GetTop(this) + tasTop);

        }

        /// <summary>
        /// アンカーハンドルの表示切替
        /// </summary>
        public void AdornerSwitch()
        {
            if (AdornerLayer.GetAdornerLayer(MyEzLine) is AdornerLayer layer)
            {
                Adorner[] ados = layer.GetAdorners(MyEzLine);
                //無ければ追加(表示)
                if (ados is null)
                {
                    layer.Add(new EzLineAdorner(MyEzLine));
                }
                //在れば削除
                else
                {
                    foreach (var item in ados.OfType<EzLineAdorner>())
                    {
                        layer.Remove(item);
                    }
                }
            }
        }


    }

    #region コンバーター

    public class MyConvRectWidth : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (Rect)value;
            return r.Width;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MyConvRectHeight : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (Rect)value;
            return r.Height;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MyConverterLeftRectLeft : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var left = (double)values[0];
            var r = (Rect)values[1];
            return left + r.Left;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConverterTopRectTop : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var top = (double)values[0];
            var r = (Rect)values[1];
            return top + r.Top;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class MyConverterNegativeRectLeft : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (Rect)value;
            return -r.X;
            //return -r.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConverterNegativeRectTop : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (Rect)value;
            return -r.Top;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion コンバーター


    public abstract class EzShapeThumb : Thumb
    {
        static EzShapeThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EzShapeThumb), new FrameworkPropertyMetadata(typeof(EzShapeThumb)));
        }
        public EzShapeThumb()
        {
            DragDelta += EzShapeThumb_DragDelta;
            Loaded += EzShapeThumb_Loaded;
            Canvas.SetLeft(this, 0);
        }

        private void EzShapeThumb_Loaded(object sender, RoutedEventArgs e)
        {
            //Relayout();
        }

        private void EzShapeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
            Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
            e.Handled = true;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_Canvas") is Canvas panel)
            {
                if (GetChildEzShape(panel) is EzShape shape)
                {
                    MyEzShape = shape;
                    if (MyPoints is null)
                    {
                        MyPoints = MyEzShape.MyPoints;
                    }
                    else
                    {
                        MyEzShape.MyPoints = MyPoints;
                    }
                }
            }
        }

        private static EzShape? GetChildEzShape(FrameworkElement element)
        {
            var count = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < count; i++)
            {
                if (VisualTreeHelper.GetChild(element, i) is EzShape shape)
                {
                    return shape;
                }
            }
            return null;
        }

        #region 依存関係プロパティ



        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(EzShapeThumb), new PropertyMetadata(null));


        public EzShape MyEzShape
        {
            get { return (EzShape)GetValue(MyEzShapeProperty); }
            set { SetValue(MyEzShapeProperty, value); }
        }
        public static readonly DependencyProperty MyEzShapeProperty =
            DependencyProperty.Register(nameof(MyEzShape), typeof(EzShape), typeof(EzShapeThumb), new PropertyMetadata(null));
        #endregion 依存関係プロパティ


        /// <summary>
        /// 再描画
        /// </summary>
        public void Relayout()
        {
            if (MyEzShape != null)
            {
                var myrect = GetRect();
                var r4 = MyEzShape.MyBounds4;
                //自身のサイズを変更
                this.Width = r4.Width;
                this.Height = r4.Height;

                //変更する前の位置を使って計算しておく、タイミング重要
                double tasLeft = Canvas.GetLeft(MyEzShape) + Canvas.GetLeft(this) + r4.Left;
                double tasTop = Canvas.GetTop(MyEzShape) + Canvas.GetTop(this) + r4.Top;

                //図形の位置を変更、オフセット
                Canvas.SetLeft(MyEzShape, -r4.Left);
                Canvas.SetTop(MyEzShape, -r4.Top);

                //自身の位置を変更、図形の位置に合わせる
                Canvas.SetLeft(this, tasLeft);
                Canvas.SetTop(this, tasTop);
            }
        }
        public void Relayout2()
        {
            if (MyEzShape != null)
            {
                var myrect = GetRect();
                var adorect = MyEzShape.MyAdornerRect;
                var r4 = MyEzShape.MyBounds4;
                double w = Math.Max(adorect.Width, r4.Width);
                double h = Math.Max(adorect.Height, r4.Height);
                //自身のサイズを変更
                Width = myrect.Width;
                Height = myrect.Height;

                //変更する前の位置を使って計算しておく、タイミング重要


                double minLeft = r4.Left;
                if (Math.Abs(adorect.Left) > Math.Abs(r4.Left))
                {
                    minLeft = adorect.Left;
                }

                double minTop = r4.Top;
                if (Math.Abs(adorect.Top) > Math.Abs(r4.Top))
                {
                    minTop = adorect.Top;
                }
                //double minLeft = Math.Min(adorect.Left, r4.Left);
                //double minTop = Math.Min(adorect.Top, r4.Top);

                double tasLeft = Canvas.GetLeft(MyEzShape) + Canvas.GetLeft(this) + minLeft;
                double tasTop = Canvas.GetTop(MyEzShape) + Canvas.GetTop(this) + minTop;


                //図形の位置を変更、オフセット
                Canvas.SetLeft(MyEzShape, -minLeft);
                Canvas.SetTop(MyEzShape, -minTop);


                //自身の位置を変更、図形の位置に合わせる
                Canvas.SetLeft(this, tasLeft);
                Canvas.SetTop(this, tasTop);
            }
        }
        public void Relayout3()
        {
            if (MyEzShape != null)
            {
                var myrect = GetRect();
                var myrect2 = GetRect2();

                //var adorect = MyEzShape.MyAdornerRect;
                var r4 = MyEzShape.MyBounds4;
                //自身のサイズを変更
                //サイズはこれであっている気がする
                Width = myrect.Width;
                Height = myrect.Height;

                //変更する前の位置を使って計算しておく、タイミング重要
                double ezleft = Canvas.GetLeft(MyEzShape);
                double eztop = Canvas.GetTop(MyEzShape);
                double myleft = Canvas.GetLeft(this);
                double mytop = Canvas.GetTop(this);
                double tasLeft = myrect.Width - r4.Width + myrect.Left;
                double tasTop = myrect.Height - r4.Height + myrect.Height;


                //オフセットはPointsのゼロfix分をすれば良さそう

                ////図形の位置を変更、オフセット
                //Canvas.SetLeft(MyEzShape, -myrect2.Left);
                //Canvas.SetTop(MyEzShape, -myrect2.Top);


                ////自身の位置を変更、図形の位置に合わせる
                //Canvas.SetLeft(this, tasLeft);
                //Canvas.SetTop(this, tasTop);

            }
        }
        public void Relayout4()
        {
            //FixPointsZero();
            //FixAdornerLocate();

            var myrect = GetRect();
            Width = myrect.Width;
            Height = myrect.Height;

            double ezleft = Canvas.GetLeft(MyEzShape);
            double eztop = Canvas.GetTop(MyEzShape);
            double myleft = Canvas.GetLeft(this);
            double mytop = Canvas.GetTop(this);

            //SetLocate(MyEzShape, -myrect.Left, -myrect.Top);

            var r4 = MyEzShape.MyBounds4;
            var (left, top) = GetTopLeftFromPoints();
            double tasLeft = myleft + left;
            double tasTop = mytop + top;
            SetLocate(this, tasLeft, tasTop);
        }

        //サイズ測定、Pointsを左上に移動、サイズ変更
        public void FixPointsLocateAndSize()
        {

            var (left, top) = GetTopLeftFromPoints();
            FixPointsZero();
            FixAdornerLocate();
            UpdateLayout();

            var ImaShapeLeft = Canvas.GetLeft(MyEzShape);
            var ImaShapeTop = Canvas.GetTop(MyEzShape);
            var pointsRect = GetRect();
            var r4 = MyEzShape.MyBounds4;
            var unionR = new Rect();
            unionR.Union(pointsRect);
            unionR.Union(r4);
            var pointRect_r4Heiht = pointsRect.Height - r4.Height;
            var pointRect_r4Width = pointsRect.Width - r4.Width;

            SetLocate(MyEzShape, -unionR.Left, -unionR.Top);

            Width = unionR.Width;
            Height = unionR.Height;
            var thumbWidht = Width;var thumbHeight = Height;
            var r4Width = r4.Width;var r4Height = r4.Height;
            var pointsWidth = pointsRect.Width;var pointsHeight = pointsRect.Height;

            OffsetLocate(this, left, top);
        }

        //サイズ測定、Pointsを左上に移動、サイズ変更
        public void FixPointsLocateAndSize2()
        {
            var r4 = MyEzShape.MyBounds4;
            var (left, top) = GetTopLeftFromPoints();
            FixPointsZero();
            FixAdornerLocate();
            var rPointsAndAnchor = GetRect();
            Width = rPointsAndAnchor.Width;
            Height = rPointsAndAnchor.Height;
            var l2 = Math.Min(r4.Left, left);
            var t2 = Math.Min(r4.Top, top);

            OffsetLocate(this, l2, t2);
        }

        private void OffsetLocate(FrameworkElement element, double left, double top)
        {
            Canvas.SetLeft(element, Canvas.GetLeft(element) + left);
            Canvas.SetTop(element, Canvas.GetTop(element) + top);
        }

        private void SetLocate(FrameworkElement element, double left, double top)
        {
            Canvas.SetLeft(element, left);
            Canvas.SetTop(element, top);
        }
        private void SetLocate(FrameworkElement element, Point locate)
        {
            Canvas.SetLeft(element, locate.X);
            Canvas.SetTop(element, locate.Y);
        }

        /// <summary>
        /// アンカーハンドルの表示切替
        /// </summary>
        public void AdornerSwitch()
        {
            if (AdornerLayer.GetAdornerLayer(MyEzShape) is AdornerLayer layer)
            {
                Adorner[] ados = layer.GetAdorners(MyEzShape);
                //無ければ追加(表示)
                if (ados is null)
                {
                    layer.Add(new EzShapeAdorner(MyEzShape));
                }
                //在れば削除
                else
                {
                    foreach (var item in ados.OfType<EzShapeAdorner>())
                    {
                        layer.Remove(item);
                    }
                }
            }
        }

        public void FixAdornerLocate()
        {
            if (AdornerLayer.GetAdornerLayer(MyEzShape) is AdornerLayer layer)
            {
                Adorner[] ados = layer.GetAdorners(MyEzShape);
                if (ados != null)
                {
                    foreach (var item in ados)
                    {
                        if (item is EzShapeAdorner shapeado)
                        {
                            shapeado.ResetAnchorLocate();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Points全体のリセット、左上に寄せる、TopLeftを0にする
        /// </summary>
        public void FixPointsZero()
        {
            var (left, top) = GetTopLeftFromPoints();
            for (int i = 0; i < MyPoints.Count; i++)
            {
                Point p = MyPoints[i];
                MyPoints[i] = new Point(p.X - left, p.Y - top);
            }
        }

        private (double left, double top) GetTopLeftFromPoints()
        {
            double left = double.MaxValue;
            double top = double.MaxValue;
            foreach (var item in MyPoints)
            {
                if (left > item.X) { left = item.X; }
                if (top > item.Y) { top = item.Y; }
            }
            return (left, top);
        }

        private Rect GetRect()
        {
            Rect r = new();
            double left = double.MaxValue; double top = double.MaxValue;
            foreach (var item in MyPoints)
            {
                if (left > item.X) { left = item.X; }
                if (top > item.Y) { top = item.Y; }
            }

            PointCollection pc = [];
            foreach (var item in MyPoints)
            {
                pc.Add(new Point(item.X - left, item.Y - top));
            }

            foreach (var item in pc)
            {
                Rect pr = new(item.X - 10, item.Y - 10, 20, 20);
                r.Union(pr);
            }
            return r;
        }

        private Rect GetRect2()
        {
            Rect r = new();
            foreach (var item in MyPoints)
            {
                Rect pr = new(item.X - 0, item.Y - 0, 40, 40);
                r.Union(pr);
            }
            return r;
        }


    }


    public class EzBezierThumb : EzShapeThumb
    {
        static EzBezierThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EzBezierThumb), new FrameworkPropertyMetadata(typeof(EzBezierThumb)));
        }
        public EzBezierThumb()
        {

        }
    }

}