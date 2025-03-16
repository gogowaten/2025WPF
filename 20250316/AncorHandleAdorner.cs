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

namespace _20250316
{

    /// <summary>
    /// ベジェ曲線の方向線表示用、2色破線
    /// OnRenderで直線描画、その上にDefiningGeometryで破線描画
    /// </summary>

    class TwoColorDashLine : Shape
    {



        //[028722]ベジエ曲線の各部の名称
        //https://support.justsystems.com/faq/1032/app/servlet/qadoc?QID=028722

        //ベジェ曲線の方向線とアンカーポイント、制御点を表示してみた - 午後わてんのブログ
        //https://gogowaten.hatenablog.com/entry/15547295
        //WPF、ベジェ曲線で直線表示、アンカー点の追加と削除 - 午後わてんのブログ
        //https://gogowaten.hatenablog.com/entry/2022/06/14/132217

        public TwoColorDashLine()
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
            DependencyProperty.Register(nameof(MyPen), typeof(Pen), typeof(TwoColorDashLine), new PropertyMetadata(null));

        //AffectRender必須
        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(TwoColorDashLine),
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
            DependencyProperty.Register(nameof(MyStrokeBase), typeof(SolidColorBrush), typeof(TwoColorDashLine), new PropertyMetadata(Brushes.Black));
        #endregion 依存関係プロパティ

    }



    public class AncorHandleAdorner : Adorner
    {

        #region VisualCollectionで必要        
        protected override int VisualChildrenCount => MyVisualCollection.Count;
        protected override Visual GetVisualChild(int index) => MyVisualCollection[index];
        #endregion VisualCollectionで必要



        private TwoColorDashLine? MyControlLine;//制御線
        public readonly List<AnchorHandleThumb> MyAnchorHandleThumbsList = [];//アンカーハンドルThumb
        private readonly Canvas MyCanvas = new();
        private readonly VisualCollection MyVisualCollection;
        private readonly GeoShape MyTarget;//装飾ターゲット
        //private readonly EzShape MyTarget;//装飾ターゲット
        public AncorHandleAdorner(GeoShape adornedElement) : base(adornedElement)
        {
            MyVisualCollection = new(this) { MyCanvas };
            MyTarget = adornedElement;
            MySetControlLine();
            AddAnchorThumb();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            MyCanvas.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            return base.ArrangeOverride(finalSize);
        }

        private void MySetControlLine()
        {
            //装飾ターゲットがベジェ曲線なら制御線追加、
            //アンカーハンドルより下側に表示したいのでzindexを-1指定
            if (MyTarget.MyShapeType == ShapeType.Bezier)
            {
                MyControlLine = new TwoColorDashLine();
                Panel.SetZIndex(MyControlLine, -1);
                MyCanvas.Children.Add(MyControlLine);
            }
        }

        private void AddAnchorThumb()
        {
            for (int i = 0; i < MyTarget.MyPoints.Count; i++)
            {
                MyCanvas.Children.Add(MakeAnchorHandleThumb(i, MyTarget.MyPoints[i]));
            }
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
            thumb.SetBinding(AnchorHandleThumb.MySizeProperty, new Binding() { Source = this, Path = new PropertyPath(MyAnchorHandleSizeProperty), Mode = BindingMode.TwoWay });
            MyAnchorHandleThumbsList.Add(thumb);
            thumb.DragDelta += Thumb_DragDelta;
            return thumb;
        }

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
            DependencyProperty.Register(nameof(MyAnchorHandleSize), typeof(double), typeof(AncorHandleAdorner), new PropertyMetadata(20.0));

        #endregion 依存関係プロパティ
    }






    #region コンバーター

    ///// <summary>
    ///// EzShape用
    ///// すべてのアンカーハンドルThumbを含んだ回転後(Transform)のRectを返す
    ///// けど、ハンドル自体は回転しないで計算しているので
    ///// ハンドルの形状が円以外だと多少の誤差がある
    ///// </summary>
    //public class MyConvAnchorHandleThumbsBounds : IMultiValueConverter
    //{
    //    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        //var handleSize = (double)values[2];
    //        var adorner = (EzShapeAdorner)values[0];
    //        if(adorner is null) { return new Rect(); }
    //        var handleSize = adorner.MyAnchorHandleSize;
    //        var points = (PointCollection)values[1];
    //        var rTransform = (System.Windows.Media.Transform)values[2];

    //        //Pointsを変形
    //        PointCollection tempPc = [];
    //        foreach (var item in points)
    //        {
    //            tempPc.Add(rTransform.Transform(item));
    //        }

    //        //各アンカーハンドルのRectを作成して
    //        //RectのUnionメソッドを利用すれば、
    //        //すべてのアンカーハンドルが収まるRectが作成できる
    //        double halfHandle = handleSize / 2.0;//アンカーポイントの中心位置
    //        Point p = tempPc[0];
    //        Rect r = new(p.X - halfHandle, p.Y - halfHandle, handleSize, handleSize);
    //        foreach (var item in tempPc)
    //        {
    //            Rect pr = new(item.X - halfHandle, item.Y - halfHandle, handleSize, handleSize);
    //            r.Union(pr);
    //        }
    //        return r;
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

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

    //Penの生成、各種プロパティも反映
    public class MyConverterPen : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var thick = (double)values[0];
            var miter = (double)values[1];
            var end = (PenLineCap)values[2];
            var sta = (PenLineCap)values[3];
            var join = (PenLineJoin)values[4];
            Pen result = new(Brushes.Transparent, thick)
            {
                EndLineCap = end,
                StartLineCap = sta,
                LineJoin = join,
                MiterLimit = miter
            };
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //Segment用のPointCollection生成
    //ソースに影響を与えないためにクローン作成して、その先頭要素を削除して返す
    public class MyConverterSegmentPoints : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PointCollection pc && pc.Count > 0)
            {
                var clone = pc.Clone();
                clone.RemoveAt(0);
                return clone;
            }
            else
            {
                return new PointCollection();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion コンバーター



}
