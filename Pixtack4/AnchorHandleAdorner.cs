using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;

namespace Pixtack4
{

    /// <summary>
    /// ベジェ曲線の方向線表示用、2色破線
    /// PointCollectionと連携させる
    /// OnRenderで直線描画、その上にDefiningGeometryで破線描画
    /// </summary>
    public class ControlLine : Shape
    {
        //[028722]ベジエ曲線の各部の名称
        //https://support.justsystems.com/faq/1032/app/servlet/qadoc?QID=028722

        //ベジェ曲線の方向線とアンカーポイント、制御点を表示してみた - 午後わてんのブログ
        //https://gogowaten.hatenablog.com/entry/15547295
        //WPF、ベジェ曲線で直線表示、アンカー点の追加と削除 - 午後わてんのブログ
        //https://gogowaten.hatenablog.com/entry/2022/06/14/132217

        public ControlLine()
        {
            Stroke = Brushes.White;
            StrokeThickness = 1.0;
            StrokeDashArray = [15.0];
            //UseLayoutRounding = true;//効果がない感じ
            SetMyBind();
        }

        //実線のPenのバインド
        private void SetMyBind()
        {
            var mb = new MultiBinding() { Converter = new MyConvLinePen() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyStrokeBaseProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(StrokeThicknessProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(StrokeDashArrayProperty) });
            SetBinding(MyPenProperty, mb);
        }

        //実線を描画
        protected override void OnRender(DrawingContext drawingContext)
        {
            for (int i = 0; i < MyPoints.Count - 1; i++)
            {
                if ((i - 1) % 3 != 0)
                {
                    drawingContext.DrawLine(MyPen, MyPoints[i], MyPoints[i + 1]);
                }
            }
            base.OnRender(drawingContext);
        }

        //破線を描画
        protected override Geometry DefiningGeometry
        {
            get
            {
                if (MyPoints == null) { return Geometry.Empty; }
                StreamGeometry geo = new();
                using var context = geo.Open();
                for (int i = 0; i < MyPoints.Count - 1; i++)
                {
                    if ((i - 1) % 3 != 0)
                    {
                        context.BeginFigure(MyPoints[i], isFilled: false, isClosed: false);
                        context.LineTo(MyPoints[i + 1], isStroked: true, isSmoothJoin: false);
                    }
                }
                geo.Freeze();
                return geo;
            }
        }


        #region 依存関係プロパティ


        public Pen MyPen
        {
            get { return (Pen)GetValue(MyPenProperty); }
            set { SetValue(MyPenProperty, value); }
        }
        public static readonly DependencyProperty MyPenProperty =
            DependencyProperty.Register(nameof(MyPen), typeof(Pen), typeof(ControlLine), new PropertyMetadata(null));

        //AffectRender必須
        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(ControlLine),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public SolidColorBrush MyStrokeBase
        {
            get { return (SolidColorBrush)GetValue(MyStrokeBaseProperty); }
            set { SetValue(MyStrokeBaseProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeBaseProperty =
            DependencyProperty.Register(nameof(MyStrokeBase), typeof(SolidColorBrush), typeof(ControlLine), new PropertyMetadata(Brushes.Black));
        #endregion 依存関係プロパティ

    }



    public class AnchorHandleAdorner : Adorner
    {

        #region VisualCollectionで必要        
        protected override int VisualChildrenCount => MyVisualCollection.Count;
        protected override Visual GetVisualChild(int index) => MyVisualCollection[index];
        #endregion VisualCollectionで必要

        private readonly ControlLine MyControlLine;//制御線
        public readonly List<AnchorHandleThumb> MyAnchorHandleThumbsList = [];//アンカーハンドルThumb
        private readonly Canvas MyCanvas = new();
        private readonly VisualCollection MyVisualCollection;
        private readonly GeoShape MyTarget;//装飾ターゲット

        public AnchorHandleAdorner(GeoShape adornedElement) : base(adornedElement)
        {
            MyVisualCollection = new(this) { MyCanvas };
            MyTarget = adornedElement;

            //制御線
            MyControlLine = new()
            {
                MyPoints = MyTarget.MyPoints
            };
            //制御線はアンカーハンドルより下側に表示したいのでzindexを-1指定
            Panel.SetZIndex(MyControlLine, -1);
            MySetControlLine();

            //アンカーハンドルを追加
            AddAnchorThumb();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            MyCanvas.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            return base.ArrangeOverride(finalSize);
        }

        private void MySetControlLine()
        {
            //装飾ターゲットがベジェ曲線なら制御線を作成、追加            
            if (MyTarget.MyShapeType == ShapeType.Bezier)
            {
                AddControlLine();
            }
        }

        //制御線の追加(表示)
        public void AddControlLine()
        {
            MyCanvas.Children.Add(MyControlLine);
        }

        //制御線の削除(非表示)
        public void RemoveControlLine()
        {
            MyCanvas.Children.Remove(MyControlLine);
        }

        private void AddAnchorThumb()
        {
            for (int i = 0; i < MyTarget.MyPoints.Count; i++)
            {
                var thumb = MakeAnchorHandleThumb(i, MyTarget.MyPoints[i]);
                MyCanvas.Children.Insert(i, thumb);
                MyAnchorHandleThumbsList.Insert(i, thumb);
            }
        }

        /// <summary>
        /// アンカーハンドルの追加(挿入)、
        /// Point追加時にも使う
        /// </summary>
        /// <param name="index"></param>
        /// <param name="poi"></param>
        public void AddAnchorHandleThumb(int index, Point poi)
        {
            var thumb = MakeAnchorHandleThumb(index, poi);
            MyCanvas.Children.Insert(index, thumb);
            MyAnchorHandleThumbsList.Insert(index, thumb);
            //挿入箇所Indexより後ろのTagを更新する
            for (int i = index; i < MyAnchorHandleThumbsList.Count; i++)
            {
                MyAnchorHandleThumbsList[i].Tag = i;
            }
        }

        //削除
        public bool RemoveAnchorHandleThumb(int index)
        {
            if (index <= MyAnchorHandleThumbsList.Count - 1)
            {
                MyCanvas.Children.RemoveAt(index);
                MyAnchorHandleThumbsList.RemoveAt(index);
                //削除箇所Indexより後ろのTagを更新する
                for (int i = index; i < MyAnchorHandleThumbsList.Count; i++)
                {
                    MyAnchorHandleThumbsList[i].Tag = i;
                }
                return true;
            }
            return false;
        }

        private AnchorHandleThumb MakeAnchorHandleThumb(int id, Point poi)
        {
            AnchorHandleThumb thumb = new()
            {
                Cursor = Cursors.Hand,
                Tag = id,
                MyLeft = poi.X - MyAnchorHandleSize / 2.0,
                MyTop = poi.Y - MyAnchorHandleSize / 2.0
            };
            thumb.SetBinding(AnchorHandleThumb.MySizeProperty,
                new Binding()
                {
                    Source = this,
                    Path = new PropertyPath(MyAnchorHandleSizeProperty),
                    Mode = BindingMode.TwoWay
                });
            thumb.DragDelta += Thumb_DragDelta;
            thumb.DragCompleted += Thumb_DragCompleted;
            thumb.DragStarted += Thumb_DragStarted;
            return thumb;
        }


        /// <summary>
        /// ハンドル移動終了時にそれを知らせるためのイベント、
        /// DragCompletedEventArgsを送っているけどいらないかも
        /// </summary>
        public event Action<DragCompletedEventArgs>? OnHandleThumbDragCompleted;
        private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            OnHandleThumbDragCompleted?.Invoke(e);
        }

        public event Action<DragStartedEventArgs>? OnHandleThumbDragStarted;
        private void Thumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            OnHandleThumbDragStarted?.Invoke(e);
        }

        //ハンドルThumbのマウスドラッグ移動、対応するPointも更新する
        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is AnchorHandleThumb thumb)
            {
                int id = (int)thumb.Tag;
                Point poi = MyTarget.MyPoints[id];
                MyTarget.MyPoints[id] = new Point(poi.X + e.HorizontalChange, poi.Y + e.VerticalChange);
                thumb.MyLeft += e.HorizontalChange;
                thumb.MyTop += e.VerticalChange;
                e.Handled = true;
            }

        }

        #region 依存関係プロパティ

        public double MyAnchorHandleSize
        {
            get { return (double)GetValue(MyAnchorHandleSizeProperty); }
            set { SetValue(MyAnchorHandleSizeProperty, value); }
        }
        public static readonly DependencyProperty MyAnchorHandleSizeProperty =
            DependencyProperty.Register(nameof(MyAnchorHandleSize), typeof(double), typeof(AnchorHandleAdorner), new PropertyMetadata(40.0));

        #endregion 依存関係プロパティ

        #region メソッド


        /// <summary>
        /// すべてのアンカーハンドルが収まるRectを返す、
        /// 回転を含むTransformにも対応
        /// </summary>
        /// <returns></returns>
        public Rect GetHandlesRenderBounds()
        {
            //全てのPointにRenderTransformを適用したPointCollectionを作成
            PointCollection pc = [];
            foreach (var item in MyTarget.MyPoints)
            {
                pc.Add(MyTarget.RenderTransform.Transform(item));
            }

            //全てのPointが収まるRectを取得
            var bounds = GetPointCollectionBounds(pc);

            //アンカーハンドルのサイズを考慮
            bounds.Inflate(MyAnchorHandleSize / 2.0, MyAnchorHandleSize / 2.0);
            return bounds;

        }

        /// <summary>
        /// 全てのPointが収まるRectを反す
        /// </summary>
        /// <param name="pc"></param>
        /// <returns></returns>
        public static Rect GetPointCollectionBounds(PointCollection pc)
        {
            double left = double.MaxValue;
            double top = double.MaxValue;
            double width = double.MinValue;
            double height = double.MinValue;
            foreach (var item in pc)
            {
                if (left > item.X) { left = item.X; }
                if (top > item.Y) { top = item.Y; }
                if (width < item.X) { width = item.X; }
                if (height < item.Y) { height = item.Y; }
            }
            width -= left;
            height -= top;
            var bounds = new Rect(left, top, width, height);
            return bounds;
        }
        #endregion メソッド
    }






    #region コンバーター

    /// <summary>
    /// 制御線に使うPen
    /// </summary>
    public class MyConvLinePen : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var brush = (SolidColorBrush)values[0];
            var thickness = (double)values[1];
            var dashArray = (DoubleCollection)values[2];
            Pen pen = new(brush, thickness);
            DashStyle dash = new()
            {
                Offset = dashArray[0],
                Dashes = dashArray
            };
            pen.DashStyle = dash;
            pen.Freeze();
            return pen;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //回転軸のY座標、見た目通りの矩形(Bounds2)の中央にしている
    public class MyConverterCenterY : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (Rect)value;
            return (r.Y * 2 + r.Height) / 2.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MyConverterCenterX : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (Rect)value;
            return (r.X * 2 + r.Width) / 2.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConvRotateTransform : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var angle = (double)value;
            return new RotateTransform(angle);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    //RenderTransformはRotateTransformに決め打ちしている
    public class MyConverterRenderTransform : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var angle = (double)values[0];
            var x = (double)values[1];
            var y = (double)values[2];
            return new RotateTransform(angle, x, y);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    ////Penの生成、各種プロパティも反映
    //public class MyConverterPen : IMultiValueConverter
    //{
    //    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        var thick = (double)values[0];
    //        var miter = (double)values[1];
    //        var end = (PenLineCap)values[2];
    //        var sta = (PenLineCap)values[3];
    //        var join = (PenLineJoin)values[4];
    //        Pen result = new(Brushes.Transparent, thick)
    //        {
    //            EndLineCap = end,
    //            StartLineCap = sta,
    //            LineJoin = join,
    //            MiterLimit = miter
    //        };
    //        return result;
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    #endregion コンバーター



}