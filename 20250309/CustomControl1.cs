using Microsoft.VisualBasic;
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

namespace _20250309
{



    public abstract class EzShapeThumb : Thumb
    {
        static EzShapeThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EzShapeThumb), new FrameworkPropertyMetadata(typeof(EzShapeThumb)));
        }
        public EzShapeThumb()
        {
            DataContext = MyItemData;
            DragDelta += EzShapeThumb_DragDelta;
            Loaded += EzShapeThumb_Loaded;
        }
        public EzShapeThumb(ItemData data) : this()
        {
            MyItemData = data;
        }

        #region 起動時

        private void EzShapeThumb_Loaded(object sender, RoutedEventArgs e)
        {
            UpdatePointsAndSizeWithoutZeroFix();
        }

        private void EzShapeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            MyItemData.MyLeft += e.HorizontalChange;
            MyItemData.MyTop += e.VerticalChange;
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

        #endregion 起動時

        #region 依存関係プロパティ


        public ItemData MyItemData
        {
            get { return (ItemData)GetValue(MyItemDataProperty); }
            set { SetValue(MyItemDataProperty, value); }
        }
        public static readonly DependencyProperty MyItemDataProperty =
            DependencyProperty.Register(nameof(MyItemData), typeof(ItemData), typeof(EzShapeThumb), new PropertyMetadata(null));


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


        //public PointCollection MyPoints
        //{
        //    get { return (PointCollection)GetValue(MyPointsProperty); }
        //    set { SetValue(MyPointsProperty, value); }
        //}
        //public static readonly DependencyProperty MyPointsProperty =
        //    DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(EzShapeThumb), new PropertyMetadata(null));


        public EzShape MyEzShape
        {
            get { return (EzShape)GetValue(MyEzShapeProperty); }
            set { SetValue(MyEzShapeProperty, value); }
        }
        public static readonly DependencyProperty MyEzShapeProperty =
            DependencyProperty.Register(nameof(MyEzShape), typeof(EzShape), typeof(EzShapeThumb), new PropertyMetadata(null));
        #endregion 依存関係プロパティ


        public abstract void AddPoint(Point point);


        /// <summary>
        /// 再描画、不完全
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

        //
        /// <summary>
        ///頂点移動後に実行
        ///Thumbのサイズと位置を更新する、アンカーポイント表示の有無で変化する
        ///処理の順番は
        ///MyPointsのBoundsが0,0になるように全体を移動、
        ///アンカーポイントも移動、
        ///Layout更新、
        ///Thumbサイズ更新、
        ///内部図形の移動、
        ///Thumbの移動 
        /// </summary>
        public void UpdatePointAndSize()
        {
            var (left, top) = GetTopLeftFromPoints(MyItemData.MyPoints);
            var topLeft = new Point(left, top);
            FixPointsZero(left, top);// PointsのゼロFix移動
            FixAdornerLocate();// AdornerをPointsの表示位置に合わせる
            UpdateLayout();// 要る？→必要            

            var pointsRect = GetBoundsFromAnchorThumb();
            //var r4 = MyEzShape.MyBounds4;//確認用
            var unionR = MyEzShape.MyBounds4;
            if (MyEzShapeAdorner != null)
            {
                unionR.Union(pointsRect);
            }
            Width = unionR.Width;
            Height = unionR.Height;
            //内部図形の位置の変更する前に今の位置を取得しておく
            var ll = Canvas.GetLeft(MyEzShape) + unionR.Left + left;
            var tt = Canvas.GetTop(MyEzShape) + unionR.Top + top;
            ll += Canvas.GetLeft(this);
            tt += Canvas.GetTop(this);

            SetLocate(MyEzShape, -unionR.Left, -unionR.Top);
            SetLocate(this, ll, tt);
        }

        //MyPointsのゼロFixなしでのサイズと位置更新、不完全
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

        /// <summary>
        /// 回転対応！！！！！！！！！！！！！！！！！！
        /// </summary>
        public void UpdatePointsAndSizeWithTransform()
        {
            var (left, top) = GetTopLeftFromPoints(MyItemData.MyPoints);
            var topLeft = new Point(left, top);
            Point rotatePoint = MyEzShape.RenderTransform.Transform(topLeft);
            FixPointsZero(left, top);// PointsのゼロFix移動
            FixAdornerLocate();// AdornerをPointsの表示位置に合わせる
            UpdateLayout();// 要る？→必要            

            //図形だけのRect取得
            Rect unionR = MyEzShape.MyBounds4;
            //アンカーハンドルが表示されている場合
            if (MyEzShapeAdorner != null)
            {
                //Thumbすべてが収まるRect取得して、図形だけのRectと合成(union)
                unionR.Union(GetBoundsFromAnchorThumbRotate(MyEzShape.RenderTransform, MyItemData.MyPoints, MyEzShapeAdorner.AnchorSize));
            }
            //サイズ変更
            Width = unionR.Width;
            Height = unionR.Height;
            //内部図形の位置の変更する前に今の位置を取得しておく
            var ll = Canvas.GetLeft(MyEzShape) + unionR.Left + rotatePoint.X;
            var tt = Canvas.GetTop(MyEzShape) + unionR.Top + rotatePoint.Y;
            ll += Canvas.GetLeft(this);
            tt += Canvas.GetTop(this);

            //図形と自身(ShapeThumb)の位置を変更
            SetLocate(MyEzShape, -unionR.Left, -unionR.Top);
            SetLocate(this, ll, tt);
        }


        private void SetLocate(FrameworkElement element, double left, double top)
        {
            Canvas.SetLeft(element, left);
            Canvas.SetTop(element, top);
        }

        /// <summary>
        /// アンカーハンドルの表示切替
        /// </summary>
        public void AnchorOnOffSwitch()
        {
            if (AdornerLayer.GetAdornerLayer(MyEzShape) is AdornerLayer layer)
            {
                //無ければ追加(表示)
                if (MyEzShapeAdorner is null)
                {
                    EzShapeAdorner adorner = new(MyEzShape);
                    layer.Add(adorner);
                    MyEzShapeAdorner = adorner;
                    //UpdatePointAndSize();
                    UpdatePointsAndSizeWithTransform();

                    foreach (var item in MyEzShapeAdorner.MyAnchorThumbsList)
                    {
                        item.DragDelta += EzShapeAnchor_DragDelta;
                        item.DragCompleted += EzShapeAnchor_DragCompleted;
                    }
                }
                //在れば削除
                else
                {
                    layer.Remove(MyEzShapeAdorner);
                    MyEzShapeAdorner = null;
                    UpdatePointsAndSizeWithTransform();
                }
            }
        }

        private void EzShapeAnchor_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            //UpdatePointAndSize();
            UpdatePointsAndSizeWithTransform();
        }

        /// <summary>
        /// アンカーのドラッグ移動時の処理
        /// 対応Pointを更新、アンカーの移動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EzShapeAnchor_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t && MyEzShapeAdorner != null)
            {
                int id = (int)t.Tag;
                Point po = MyItemData.MyPoints[id];
                double left = po.X + e.HorizontalChange;
                double top = po.Y + e.VerticalChange;

                //Pointの更新
                MyItemData.MyPoints[id] = new(left, top);

                //アンカーの移動
                Canvas.SetLeft(t, left - MyEzShapeAdorner.AnchorSize / 2.0);
                Canvas.SetTop(t, top - MyEzShapeAdorner.AnchorSize / 2.0);

                e.Handled = true;
            }
        }

        public void FixAdornerLocate()
        {
            MyEzShapeAdorner?.ResetAnchorLocate();
        }





        /// <summary>
        /// Points全体のリセット、左上に寄せる、TopLeftを0にする
        /// </summary>
        public void FixPointsZero(PointCollection points)
        {
            var (left, top) = GetTopLeftFromPoints(points);
            for (int i = 0; i < points.Count; i++)
            {
                Point p = points[i];
                points[i] = new Point(p.X - left, p.Y - top);
            }
        }
        public void FixPointsZero(double offsetX, double offsetY)
        {
            for (int i = 0; i < MyItemData.MyPoints.Count; i++)
            {
                Point p = MyItemData.MyPoints[i];
                MyItemData.MyPoints[i] = new Point(p.X - offsetX, p.Y - offsetY);
            }
        }

        private (double left, double top) GetTopLeftFromPoints(PointCollection points)
        {
            double left = double.MaxValue;
            double top = double.MaxValue;
            foreach (var item in points)
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
            foreach (var item in MyItemData.MyPoints)
            {
                if (left > item.X) { left = item.X; }
                if (top > item.Y) { top = item.Y; }
            }

            PointCollection pc = [];
            foreach (var item in MyItemData.MyPoints)
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
                foreach (var item in MyItemData.MyPoints)
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

        /// <summary>
        /// すべてのアンカーハンドルThumbを含んだ回転後(Transform)のRectを返す
        /// けど、ハンドル自体は回転しないで計算しているので多少の誤差がある
        /// </summary>
        /// <param name="transform">RenderTransform</param>
        /// <param name="points"></param>
        /// <param name="handleSize">アンカーハンドルThumbのサイズ</param>
        /// <returns></returns>
        private Rect GetBoundsFromAnchorThumbRotate(Transform transform, PointCollection points, double handleSize)
        {
            //Pointsを変形
            PointCollection tempPc = [];
            foreach (var item in points)
            {
                tempPc.Add(transform.Transform(item));
            }

            //各アンカーハンドルのRectを作成して
            //RectのUnionメソッドを利用すれば、
            //すべてのアンカーハンドルが収まるRectが作成できる
            double halfHandle = handleSize / 2.0;//アンカーポイントの中心位置
            Point p = tempPc[0];
            Rect r = new(p.X - halfHandle, p.Y - halfHandle, handleSize, handleSize);
            foreach (var item in tempPc)
            {
                Rect pr = new(item.X - halfHandle, item.Y - halfHandle, handleSize, handleSize);
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
        public EzBezierThumb(ItemData data) : base(data)
        {

        }

        public override void AddPoint(Point point)
        {
            MyEzShape.AddPoint(point);
            //MyEzShapeAdorner?.ResetAnchorLocate();
        }
    }


    public class Waku : Control
    {
        static Waku()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Waku), new FrameworkPropertyMetadata(typeof(Waku)));
        }
        public Waku()
        {

        }
    }


}