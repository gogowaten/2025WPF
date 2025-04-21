using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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

namespace _20250416
{


    public class GeoShapePanel : Control
    {
        public GeoShapeWithAnchorHandle MyGeoShapeWithAnchor { get; set; } = null!;

        static GeoShapePanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GeoShapePanel), new FrameworkPropertyMetadata(typeof(GeoShapePanel)));
        }
        public GeoShapePanel()
        {

        }
        public GeoShapePanel(ItemData data)
        {
            MyItemData = data;
            DataContext = MyItemData;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("geo") is GeoShapeWithAnchorHandle geo)
            {
                MyGeoShapeWithAnchor = geo;
            }
        }


        public ItemData MyItemData
        {
            get { return (ItemData)GetValue(MyItemDataProperty); }
            set { SetValue(MyItemDataProperty, value); }
        }
        public static readonly DependencyProperty MyItemDataProperty =
            DependencyProperty.Register(nameof(MyItemData), typeof(ItemData), typeof(GeoShapePanel), new PropertyMetadata(null));

    }


    //public class GeoShapePanelThumb : KisoThumb
    //{
    //    public GeoShapePanel GeoShapePanel { get; set; } = null!;
    //    public GeoShapeWithAnchorHandle GeoShapeWithAnchorHandle { get; set; } = null!;

    //    static GeoShapePanelThumb()
    //    {
    //        DefaultStyleKeyProperty.OverrideMetadata(typeof(GeoShapePanelThumb), new FrameworkPropertyMetadata(typeof(GeoShapePanelThumb)));
    //    }
    //    public GeoShapePanelThumb(ItemData data) : base(data)
    //    {

    //    }
    //    public override void OnApplyTemplate()
    //    {
    //        base.OnApplyTemplate();
    //        if (MyInsideElement is GeoShapePanel panel)
    //        {
    //            this.GeoShapePanel = panel;

    //        }
    //    }
    //}

    public class GeoShapeThumb : KisoThumb
    {
        public GeoShapeWithAnchorHandle MyGeoShape { get; private set; } = null!;

        static GeoShapeThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GeoShapeThumb), new FrameworkPropertyMetadata(typeof(GeoShapeThumb)));
        }
        public GeoShapeThumb() { }

        private void GeoShapeThumb_Loaded(object sender, RoutedEventArgs e)
        {
            MyBindActualLocate();
            MyBindInsideElementLocate();
        }

        public GeoShapeThumb(ItemData data) : base(data)
        {
            Loaded += GeoShapeThumb_Loaded;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (MyInsideElement is GeoShapeWithAnchorHandle geo)
            {
                this.MyGeoShape = geo;
            }
            else { throw new ArgumentException("TemplateからGeoShapeWithAnchorHandleが見つからなかった"); }
        }


        public Point MyTestCenterPoint
        {
            get { return (Point)GetValue(MyTestCenterPointProperty); }
            set { SetValue(MyTestCenterPointProperty, value); }
        }
        public static readonly DependencyProperty MyTestCenterPointProperty =
            DependencyProperty.Register(nameof(MyTestCenterPoint), typeof(Point), typeof(GeoShapeThumb), new PropertyMetadata(new Point()));


        #region 変形メソッド

        public void SetScaleX()
        {
            MyPointsTransform(MakeScaleTransform(), MyGeoShape.MyPoints);
        }

        private ScaleTransform MakeScaleTransform()
        {
            var (cx, cy) = MakeCenterXY();
            return new ScaleTransform(MyItemData.MyScaleX, MyItemData.MyScaleY, cx, cy);
        }

        public void SetAngle(bool isRotateRight = true)
        {
            if (isRotateRight)
            {
                //右回転
                MyPointsTransform(MakeRotateTransform(), MyGeoShape.MyPoints);
            }
            else
            {
                //左回転
                var ro = MakeRotateTransform();
                ro.Angle = 360.0 - ro.Angle;
                MyPointsTransform(ro, MyGeoShape.MyPoints);
            }
        }

        private RotateTransform MakeRotateTransform()
        {
            (double cx, double cy) = MakeCenterXY();
            return new RotateTransform(MyItemData.MyAngle, cx, cy);
        }

        private (double cx, double cy) MakeCenterXY()
        {
            double cx = MyInsideElementTransformedBounds.Width * MyItemData.MyCenterX + MyInsideElementTransformedBounds.Left;
            double cy = MyInsideElementTransformedBounds.Height * MyItemData.MyCenterY + MyInsideElementTransformedBounds.Top;
            return (cx, cy);
        }

        private static void MyPointsTransform(Transform transform, PointCollection pois)
        {
            for (int i = 0; i < pois.Count; i++)
            {
                pois[i] = transform.Transform(pois[i]);
            }
        }
        #endregion 変形メソッド

    }

    public class TextBlockThumb : KisoThumb
    {
        static TextBlockThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBlockThumb), new FrameworkPropertyMetadata(typeof(TextBlockThumb)));
        }
        public TextBlockThumb() { }
        public TextBlockThumb(ItemData data) : base(data)
        {

        }
    }

    public class KisoThumb : Thumb
    {
        public FrameworkElement MyInsideElement { get; set; } = null!;
        static KisoThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KisoThumb), new FrameworkPropertyMetadata(typeof(KisoThumb)));
        }
        public KisoThumb()
        {
            Loaded += KisoThumb_Loaded;
            DragDelta += KisoThumb_DragDelta;
        }

        public KisoThumb(ItemData data) : this()
        {
            MyItemData = data;
            DataContext = MyItemData;
        }


        private void KisoThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            MyItemData.MyLeft += e.HorizontalChange;
            MyItemData.MyTop += e.VerticalChange;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("InsideElement") is FrameworkElement element)
            {
                MyInsideElement = element;
            }
            else
            {
                MessageBox.Show("内部要素の取得に失敗");
            }
        }

        private void KisoThumb_Loaded(object sender, RoutedEventArgs e)
        {
            if (MyInsideElement is GeoShape geo)
            {
                //Geometry図形要素用のバインド
                MyBindGeoShapePenBounds(geo);
            }
            else
            {
                //通常要素用の各種バインド設定
                MyBindInsideElementRenderTransform();
                MyBindInsideElementRenderTransformBounds();
                MyBindActualLocate();
                MyBindInsideElementLocate();
            }

        }

        #region 初期バインド設定

        private void MyBindGeoShapePenBounds(GeoShape geo)
        {
            var bind = new MultiBinding() { Converter = new MyConvRenderGeometryPenBounds() };
            bind.Bindings.Add(new Binding() { Source = geo, Path = new PropertyPath(GeoShape.MyGeometryProperty) });
            bind.Bindings.Add(new Binding() { Source = geo, Path = new PropertyPath(GeoShape.MyPenProperty) });
            bind.Bindings.Add(new Binding(nameof(ItemData.MyScaleX)) { Source = MyItemData });
            bind.Bindings.Add(new Binding(nameof(ItemData.MyScaleY)) { Source = MyItemData });
            bind.Bindings.Add(new Binding(nameof(ItemData.MyAngle)) { Source = MyItemData });
            SetBinding(MyInsideElementTransformedBoundsProperty, bind);
        }

        /// <summary>
        /// 中の要素の位置
        /// 回転拡縮などでの変化する位置の修正用
        /// </summary>
        protected void MyBindInsideElementLocate()
        {
            BindingOperations.SetBinding(MyInsideElement, Canvas.LeftProperty, new Binding() { Source = this, Path = new PropertyPath(MyInsideElementTransformedBoundsProperty), Converter = new MyConvInsideElementLeft() });
            BindingOperations.SetBinding(MyInsideElement, Canvas.TopProperty, new Binding() { Source = this, Path = new PropertyPath(MyInsideElementTransformedBoundsProperty), Converter = new MyConvInsideElementTop() });

        }

        /// <summary>
        /// 自身の実際の位置設定用
        /// 回転拡縮などでの変化する位置修正後を実際の位置とする(canvas.leftなどとバインドする)
        /// 指定横位置(myleft)と中の要素のBoundsのLeftから取得
        /// </summary>
        protected void MyBindActualLocate()
        {
            var mb = new MultiBinding() { Converter = new MyConvActualLeft() };
            mb.Bindings.Add(new Binding(nameof(ItemData.MyLeft)) { Source = MyItemData });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyInsideElementTransformedBoundsProperty) });
            SetBinding(MyActualLeftProperty, mb);

            mb = new() { Converter = new MyConvActualTop() };
            mb.Bindings.Add(new Binding(nameof(ItemData.MyTop)) { Source = MyItemData });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyInsideElementTransformedBoundsProperty) });
            SetBinding(MyActualTopProperty, mb);

        }

        /// <summary>
        /// 中の要素の変形後Bounds作成
        /// </summary>
        private void MyBindInsideElementRenderTransformBounds()
        {
            var mb = new MultiBinding() { Converter = new MyConvRenderTransormBounds() };
            mb.Bindings.Add(new Binding() { Source = MyInsideElement, Path = new PropertyPath(RenderTransformProperty) });
            mb.Bindings.Add(new Binding() { Source = MyInsideElement, Path = new PropertyPath(ActualWidthProperty) });
            mb.Bindings.Add(new Binding() { Source = MyInsideElement, Path = new PropertyPath(ActualHeightProperty) });
            SetBinding(MyInsideElementTransformedBoundsProperty, mb);
        }

        /// <summary>
        /// 中の要素のRenderTransformをItemDataと要素の縦横サイズから
        /// </summary>
        private void MyBindInsideElementRenderTransform()
        {
            var mb = new MultiBinding() { Converter = new MyConvRenderTransform2() };
            mb.Bindings.Add(new Binding() { Source = MyInsideElement, Path = new PropertyPath(ActualWidthProperty) });
            mb.Bindings.Add(new Binding() { Source = MyInsideElement, Path = new PropertyPath(ActualHeightProperty) });
            mb.Bindings.Add(new Binding(nameof(ItemData.MyScaleX)) { Source = MyItemData });
            mb.Bindings.Add(new Binding(nameof(ItemData.MyScaleY)) { Source = MyItemData });
            mb.Bindings.Add(new Binding(nameof(ItemData.MyAngle)) { Source = MyItemData });
            mb.Bindings.Add(new Binding(nameof(ItemData.MyCenterX)) { Source = MyItemData });
            mb.Bindings.Add(new Binding(nameof(ItemData.MyCenterY)) { Source = MyItemData });
            MyInsideElement.SetBinding(RenderTransformProperty, mb);
        }
        #endregion 初期バインド設定

        #region 依存関係プロパティ


        /// <summary>
        /// 実際の表示位置に使う。Canvas.Leftとバインドする
        /// </summary>
        public double MyActualLeft
        {
            get { return (double)GetValue(MyActualLeftProperty); }
            set { SetValue(MyActualLeftProperty, value); }
        }
        public static readonly DependencyProperty MyActualLeftProperty =
            DependencyProperty.Register(nameof(MyActualLeft), typeof(double), typeof(KisoThumb), new PropertyMetadata(0.0));

        public double MyActualTop
        {
            get { return (double)GetValue(MyActualTopProperty); }
            set { SetValue(MyActualTopProperty, value); }
        }
        public static readonly DependencyProperty MyActualTopProperty =
            DependencyProperty.Register(nameof(MyActualTop), typeof(double), typeof(KisoThumb), new PropertyMetadata(0.0));

        /// <summary>
        /// 中の要素の変形後Bounds。表示されている要素がピッタリ収まるRect
        /// </summary>
        public Rect MyInsideElementTransformedBounds
        {
            get { return (Rect)GetValue(MyInsideElementTransformedBoundsProperty); }
            set { SetValue(MyInsideElementTransformedBoundsProperty, value); }
        }
        public static readonly DependencyProperty MyInsideElementTransformedBoundsProperty =
            DependencyProperty.Register(nameof(MyInsideElementTransformedBounds), typeof(Rect), typeof(KisoThumb), new PropertyMetadata(new Rect()));


        public ItemData MyItemData
        {
            get { return (ItemData)GetValue(MyItemDataProperty); }
            set { SetValue(MyItemDataProperty, value); }
        }
        public static readonly DependencyProperty MyItemDataProperty =
            DependencyProperty.Register(nameof(MyItemData), typeof(ItemData), typeof(KisoThumb), new PropertyMetadata(null));
        #endregion 依存関係プロパティ
    }
}