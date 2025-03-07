using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250307
{



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

        #region 起動時

        private void EzShapeThumb_Loaded(object sender, RoutedEventArgs e)
        {
            //SetMyBind();
            //Relayout();
            UpdatePointsAndSizeWithoutZeroFix();
        }

        private void EzShapeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
            Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
            e.Handled = true;
        }

        //起動時、Templateの中からCanvasとEzShapeを取得
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_Canvas") is Canvas panel)
            {
                MyPART_Canvas = panel;
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

        //private void SetMyBind()
        //{
        //    var mb = new MultiBinding() { Converter = new MyConvShapeAndAnchorBounds() };
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyEzShapeAdornerProperty) });
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty) });
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyEzShapeProperty) });
        //    SetBinding(MyShapeWithAnchorBoundsProperty, mb);
        //}
        #endregion 起動時

        #region 依存関係プロパティ


        public Rect MyShapeWithAnchorBounds
        {
            get { return (Rect)GetValue(MyShapeWithAnchorBoundsProperty); }
            set { SetValue(MyShapeWithAnchorBoundsProperty, value); }
        }
        public static readonly DependencyProperty MyShapeWithAnchorBoundsProperty =
            DependencyProperty.Register(nameof(MyShapeWithAnchorBounds), typeof(Rect), typeof(EzShapeThumb), new PropertyMetadata(Rect.Empty));


        //内部図形のアンカーポイント表示用のAdorner
        public EzShapeAdorner? MyEzShapeAdorner
        {
            get { return (EzShapeAdorner)GetValue(MyEzShapeAdornerProperty); }
            protected set { SetValue(MyEzShapeAdornerProperty, value); }
        }
        public static readonly DependencyProperty MyEzShapeAdornerProperty =
            DependencyProperty.Register(nameof(MyEzShapeAdorner), typeof(EzShapeAdorner), typeof(EzShapeThumb), new PropertyMetadata(null));


        //確認用なので必要ない
        public Canvas MyPART_Canvas
        {
            get { return (Canvas)GetValue(MyPART_CanvasProperty); }
            protected set { SetValue(MyPART_CanvasProperty, value); }
        }
        public static readonly DependencyProperty MyPART_CanvasProperty =
            DependencyProperty.Register(nameof(MyPART_Canvas), typeof(Canvas), typeof(EzShapeThumb), new PropertyMetadata(null));


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
                var myrect = GetBoundsFromPointsAndAnchorThumb();
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

        //頂点移動後に実行
        //Thumbのサイズと位置を更新する、アンカーポイント表示の有無で変化する
        //処理の順番は
        //MyPointsのBoundsが0,0になるように全体を移動、
        //アンカーポイントも移動、
        //Layout更新、
        //Thumbサイズ更新、
        //内部図形の移動、
        //Thumbの移動
        public void UpdatePointAndSize()
        {
            var (left, top) = GetTopLeftFromPoints();
            FixPointsZero(left, top);// PointsのゼロFix移動
            FixAdornerLocate();// AdornerをPointsの表示位置に合わせる
            UpdateLayout();// 要る？→必要            

            var pointsRect = GetBoundsFromAnchorThumb();
            var r4 = MyEzShape.MyBounds4;
            var unionR = new Rect();
            unionR.Union(pointsRect);
            unionR.Union(r4);
            Width = unionR.Width;
            Height = unionR.Height;
            //内部図形の位置の変更する前に今の位置を取得しておく
            var ImaShapeLeft = Canvas.GetLeft(MyEzShape);
            var ImaShapeTop = Canvas.GetTop(MyEzShape);
            var ll = Canvas.GetLeft(MyEzShape) + unionR.Left + left;
            var tt = Canvas.GetTop(MyEzShape) + unionR.Top + top;
            ll += Canvas.GetLeft(this);
            tt += Canvas.GetTop(this);

            SetLocate(MyEzShape, -unionR.Left, -unionR.Top);
            SetLocate(this, ll, tt);
        }

        //MyPointsのゼロFixなしでのサイズと位置更新
        public void UpdatePointsAndSizeWithoutZeroFix()
        {
            var pointsRect = GetBoundsFromAnchorThumb();
            var r4 = MyEzShape.MyBounds4;
            var unionR = new Rect();
            unionR.Union(pointsRect);
            unionR.Union(r4);
            Width = unionR.Width;
            Height = unionR.Height;
            //内部図形の位置の変更する前に今の位置を取得しておく
            var ll = Canvas.GetLeft(MyEzShape) + unionR.Left;
            var tt = Canvas.GetTop(MyEzShape) + unionR.Top;
            ll += Canvas.GetLeft(this);
            tt += Canvas.GetTop(this);

            SetLocate(MyEzShape, -unionR.Left, -unionR.Top);
            SetLocate(this, ll, tt);
        }

        public void UpdatePointsAndSizeWithoutZeroFixTest()
        {
            var pointsRect = GetBoundsFromAnchorThumb();
            var r4 = MyEzShape.MyBounds4;
            var unionR = new Rect();
            unionR.Union(pointsRect);
            unionR.Union(r4);
            var maeShapeLeft = Canvas.GetLeft(MyEzShape);
            var maeShapeTo = Canvas.GetTop(MyEzShape);
            SetLocate(MyEzShape, -unionR.Left, -unionR.Top);
            var atoShapeLeft = Canvas.GetLeft(MyEzShape);
            var atoShapeTop = Canvas.GetTop(MyEzShape);
            var maeatoLeft = maeShapeLeft - atoShapeLeft;
            Width = unionR.Width;
            Height = unionR.Height;
            //内部図形の位置の変更する前に今の位置を取得しておく
            var ll = unionR.Left + maeatoLeft;
            var tt = unionR.Top;
            ll += Canvas.GetLeft(this);
            tt += Canvas.GetTop(this);
            var lll = Canvas.GetLeft(this);
            Canvas.SetLeft(this, lll + maeatoLeft);
            Canvas.SetTop(this, Canvas.GetTop(this) + maeShapeTo - atoShapeTop);
            //SetLocate(this, ll, tt);
        }


        private void SetLocate(FrameworkElement element, double left, double top)
        {
            Canvas.SetLeft(element, left);
            Canvas.SetTop(element, top);
        }

        /// <summary>
        /// アンカーハンドルの表示切替
        /// </summary>
        public void AdornerSwitch()
        {
            if (AdornerLayer.GetAdornerLayer(MyEzShape) is AdornerLayer layer)
            {
                //無ければ追加(表示)
                if (MyEzShapeAdorner is null)
                {
                    EzShapeAdorner adorner = new(MyEzShape);
                    layer.Add(adorner);
                    MyEzShapeAdorner = adorner;
                }
                //在れば削除
                else
                {
                    layer.Remove(MyEzShapeAdorner);
                    MyEzShapeAdorner = null;
                }
            }
        }


        public void FixAdornerLocate()
        {
            MyEzShapeAdorner?.ResetAnchorLocate();
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
        public void FixPointsZero(double offsetX, double offsetY)
        {
            for (int i = 0; i < MyPoints.Count; i++)
            {
                Point p = MyPoints[i];
                MyPoints[i] = new Point(p.X - offsetX, p.Y - offsetY);
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

        /// <summary>
        /// Pointsとアンカーポイントが収まるRect
        /// </summary>
        /// <returns></returns>
        private Rect GetBoundsFromPointsAndAnchorThumb()
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

        /// <summary>
        /// 全部のアンカーポイントが収まるRectを返す
        /// アンカーポイントが表示されていなければ0を返す
        /// </summary>
        /// <returns></returns>
        private Rect GetBoundsFromAnchorThumb()
        {
            if (MyEzShapeAdorner != null)
            {
                double anchorSize = MyEzShapeAdorner.AnchorSize;
                double anchorSizeHalf = anchorSize / 2.0;//アンカーポイントの中心位置
                Rect r = new();
                foreach (var item in MyPoints)
                {
                    Rect pr = new(item.X - anchorSizeHalf, item.Y - anchorSizeHalf, anchorSize, anchorSize);
                    r.Union(pr);
                }
                return r;
            }
            else
            {
                return new Rect();
            }

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


    #region コンバーター
    public class MyConvShapeAndAnchorBounds : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var ador = (EzShapeAdorner)values[0];
            var points = (PointCollection)values[1];
            var shape = (EzShape)values[2];
            Rect bounds = new();
            if (points == null) { return bounds; }
            double anchorSize = 0;
            double halfSize = 0;

            if (ador != null)
            {
                anchorSize = ador.AnchorSize;
                halfSize = anchorSize / 2.0;
            }
            //PointCollection tempPC = [];
            //double left = double.MaxValue;
            //double top = double.MaxValue;
            //foreach (var item in points)
            //{
            //    left = Math.Min(left, item.X);
            //    if (top > item.Y) { top = item.Y; }
            //}

            foreach (var item in points)
            {
                //Rect temp = new(item.X - halfSize - left, item.Y - halfSize - top, anchorSize, anchorSize);
                Rect temp = new(item.X - halfSize, item.Y - halfSize, anchorSize, anchorSize);
                bounds.Union(temp);
            }
            bounds.Union(shape.MyBounds4);
            return bounds;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion コンバーター



}
