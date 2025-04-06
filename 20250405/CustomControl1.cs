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

namespace _20250405
{
    public class GeoShapeThumb : KisoThumb
    {

        static GeoShapeThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GeoShapeThumb), new FrameworkPropertyMetadata(typeof(GeoShapeThumb)));
        }
        public GeoShapeThumb() { }
        public GeoShapeThumb(ItemData data) : base(data)
        {
            Loaded += GeoShapeThumb_Loaded;
        }

        private void GeoShapeThumb_Loaded(object sender, RoutedEventArgs e)
        {
            MyInsideGeoShape = (GeoShape)MyInsideElement;
            //各種バインド設定
            MyBindInsideElementRenderTransform();
            MyBindInsideElementRenderTransformBounds();

            //MyBindActualLocate();
            //MyBindInsideElementLocate();

        }

        /// <summary>
        /// 中の要素の位置決定用
        /// 回転拡縮などでの変化した分をオフセットする
        /// </summary>
        private void MyBindInsideElementLocate()
        {
            BindingOperations.SetBinding(MyInsideElement, Canvas.LeftProperty, new Binding() { Source = this, Converter = new MyConvInsideElementLeft(), Path = new PropertyPath(MyInsideElementRenderTransformBoundsProperty) });
            BindingOperations.SetBinding(MyInsideElement, Canvas.TopProperty, new Binding() { Source = this, Converter = new MyConvInsideElementTop(), Path = new PropertyPath(MyInsideElementRenderTransformBoundsProperty) });

            //BindingOperations.SetBinding(MyInsideElement, Canvas.LeftProperty, new Binding(nameof(MyInsideGeoShape.MyRenderBounds)) { Source = MyInsideGeoShape, Converter = new MyConvInsideElementLeft() });
            //BindingOperations.SetBinding(MyInsideElement, Canvas.TopProperty, new Binding(nameof(MyInsideGeoShape.MyRenderBounds)) { Source = MyInsideGeoShape, Converter = new MyConvInsideElementTop() });


        }

        /// <summary>
        /// 自身の実際の位置設定用
        /// 回転拡縮などでの変化する位置修正後を実際の位置とする(canvas.leftとバインドする)
        /// 指定横位置(myleft)と中の要素のBoundsのLeftから取得
        /// </summary>
        private void MyBindActualLocate()
        {
            var mb = new MultiBinding() { Converter = new MyConvActualLeft() };
            mb.Bindings.Add(new Binding(nameof(ItemData.MyLeft)) { Source = MyItemData });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyInsideElementRenderTransformBoundsProperty) });
            SetBinding(MyActualLeftProperty, mb);

            mb = new() { Converter = new MyConvActualTop() };
            mb.Bindings.Add(new Binding(nameof(ItemData.MyTop)) { Source = MyItemData });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyInsideElementRenderTransformBoundsProperty) });
            SetBinding(MyActualTopProperty, mb);

        }

        /// <summary>
        /// 中の要素の回転拡縮後Bounds取得用。これを元に自身の位置とサイズを決める
        /// GeoShapeではBoundsからじゃなくて、Geometryから求める
        /// </summary>
        private void MyBindInsideElementRenderTransformBounds()
        {

            var mb = new MultiBinding() { Converter = new MyConvRenderTransormBoundsForGeoShape(), ConverterParameter = MyInsideElement };


            mb.Bindings.Add(new Binding(nameof(MyInsideGeoShape.RenderedGeometry)) { Source = MyInsideGeoShape });
            mb.Bindings.Add(new Binding(nameof(ItemData.MyAngle)) { Source = MyItemData });
            mb.Bindings.Add(new Binding(nameof(ItemData.MyScaleX)) { Source = MyItemData });
            mb.Bindings.Add(new Binding(nameof(ItemData.MyScaleY)) { Source = MyItemData });
            mb.Bindings.Add(new Binding(nameof(ItemData.MyTransformOrigin)) { Source = MyItemData });
            mb.Bindings.Add(new Binding()
            { Source = MyInsideGeoShape, Path = new PropertyPath(GeoShape.MyPenProperty) });
            mb.Bindings.Add(new Binding()
            { Source = MyInsideGeoShape, Path = new PropertyPath(GeoShape.MyRenderBoundsProperty) });

            SetBinding(MyInsideElementRenderTransformBoundsProperty, mb);
        }

        /// <summary>
        /// 中の要素のRenderTransformを、ItemDataと要素の縦横サイズから取得
        /// </summary>
        private void MyBindInsideElementRenderTransform()
        {
            //中心で回転拡縮したいので、縦横サイズも必要
            var mb = new MultiBinding() { Converter = new MyConvRenderTransform() };

            mb.Bindings.Add(new Binding(nameof(ItemData.MyTransformOrigin)) { Source = MyItemData });
            mb.Bindings.Add(new Binding(nameof(MyInsideGeoShape.MyRenderBounds.Width))
            { Source = MyInsideGeoShape.MyRenderBounds });
            mb.Bindings.Add(new Binding(nameof(MyInsideGeoShape.MyRenderBounds.Height))
            { Source = MyInsideGeoShape.MyRenderBounds });
            mb.Bindings.Add(new Binding(nameof(ItemData.MyScaleX)) { Source = MyItemData });
            mb.Bindings.Add(new Binding(nameof(ItemData.MyScaleY)) { Source = MyItemData });
            mb.Bindings.Add(new Binding(nameof(ItemData.MyAngle)) { Source = MyItemData });

            MyInsideElement.SetBinding(RenderTransformProperty, mb);
        }


        public GeoShape MyInsideGeoShape
        {
            get { return (GeoShape)GetValue(MyInsideGeoShapeProperty); }
            set { SetValue(MyInsideGeoShapeProperty, value); }
        }
        public static readonly DependencyProperty MyInsideGeoShapeProperty =
            DependencyProperty.Register(nameof(MyInsideGeoShape), typeof(GeoShape), typeof(GeoShapeThumb), new PropertyMetadata(null));

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
            //各種バインド設定
            MyBindInsideElementRenderTransform();
            MyBindInsideElementRenderTransformBounds();
            MyBindActualLocate();
            MyBindInsideElementLocate();

        }

        #region 初期バインド設定

        /// <summary>
        /// 中の要素の位置決定用
        /// 回転拡縮などでの変化した分をオフセットする
        /// </summary>
        private void MyBindInsideElementLocate()
        {
            BindingOperations.SetBinding(MyInsideElement, Canvas.LeftProperty, new Binding() { Source = this, Path = new PropertyPath(MyInsideElementRenderTransformBoundsProperty), Converter = new MyConvInsideElementLeft() });
            BindingOperations.SetBinding(MyInsideElement, Canvas.TopProperty, new Binding() { Source = this, Path = new PropertyPath(MyInsideElementRenderTransformBoundsProperty), Converter = new MyConvInsideElementTop() });

        }

        /// <summary>
        /// 自身の実際の位置設定用
        /// 回転拡縮などでの変化する位置修正後を実際の位置とする(canvas.leftとバインドする)
        /// 指定横位置(myleft)と中の要素のBoundsのLeftから取得
        /// </summary>
        private void MyBindActualLocate()
        {
            var mb = new MultiBinding() { Converter = new MyConvActualLeft() };
            mb.Bindings.Add(new Binding(nameof(ItemData.MyLeft)) { Source = MyItemData });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyInsideElementRenderTransformBoundsProperty) });
            SetBinding(MyActualLeftProperty, mb);

            mb = new() { Converter = new MyConvActualTop() };
            mb.Bindings.Add(new Binding(nameof(ItemData.MyTop)) { Source = MyItemData });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyInsideElementRenderTransformBoundsProperty) });
            SetBinding(MyActualTopProperty, mb);

        }

        /// <summary>
        /// 中の要素の回転拡縮後Bounds取得用。これを元に自身の位置とサイズを決める
        /// </summary>
        private void MyBindInsideElementRenderTransformBounds()
        {
            var mb = new MultiBinding() { Converter = new MyConvRenderTransormBounds(), ConverterParameter = MyInsideElement };
            mb.Bindings.Add(new Binding() { Source = MyInsideElement, Path = new PropertyPath(RenderTransformProperty) });
            mb.Bindings.Add(new Binding() { Source = MyInsideElement, Path = new PropertyPath(ActualWidthProperty) });
            mb.Bindings.Add(new Binding() { Source = MyInsideElement, Path = new PropertyPath(ActualHeightProperty) });
            SetBinding(MyInsideElementRenderTransformBoundsProperty, mb);
        }

        /// <summary>
        /// 中の要素のRenderTransformを、ItemDataと要素の縦横サイズから取得
        /// </summary>
        private void MyBindInsideElementRenderTransform()
        {
            //中心で回転拡縮したいので、縦横サイズも必要
            var mb = new MultiBinding() { Converter = new MyConvRenderTransform() };
            mb.Bindings.Add(new Binding(nameof(ItemData.MyTransformOrigin)) { Source = MyItemData });
            mb.Bindings.Add(new Binding() { Source = MyInsideElement, Path = new PropertyPath(ActualWidthProperty) });
            mb.Bindings.Add(new Binding() { Source = MyInsideElement, Path = new PropertyPath(ActualHeightProperty) });
            mb.Bindings.Add(new Binding(nameof(ItemData.MyScaleX)) { Source = MyItemData });
            mb.Bindings.Add(new Binding(nameof(ItemData.MyScaleY)) { Source = MyItemData });
            mb.Bindings.Add(new Binding(nameof(ItemData.MyAngle)) { Source = MyItemData });
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
        /// 中の要素のBounds。表示されている要素がピッタリ収まるRect
        /// </summary>
        public Rect MyInsideElementRenderTransformBounds
        {
            get { return (Rect)GetValue(MyInsideElementRenderTransformBoundsProperty); }
            set { SetValue(MyInsideElementRenderTransformBoundsProperty, value); }
        }
        public static readonly DependencyProperty MyInsideElementRenderTransformBoundsProperty =
            DependencyProperty.Register(nameof(MyInsideElementRenderTransformBounds), typeof(Rect), typeof(KisoThumb), new PropertyMetadata(new Rect()));


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