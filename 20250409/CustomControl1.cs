﻿using System;
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

namespace _20250409
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

        }
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
        public FrameworkElement MyInsideElement { get; private set; } = null!;
        public Grid MyInsideGrid { get; private set; } = null!;
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
            MyItemData.Left += e.HorizontalChange;
            MyItemData.Top += e.VerticalChange;
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
            if (GetTemplateChild("insideGrid") is Grid grid)
            {
                MyInsideGrid = grid;
            }
            else { MessageBox.Show("内部Gridの取得に失敗"); }
        }

        private void KisoThumb_Loaded(object sender, RoutedEventArgs e)
        {
            //各種バインド設定
            if (MyItemData.Type == ItemType.GeoShape)
            {
                //MyBindInsideGeoShapeTransformedBounds();
                MyBindInsideGeoShapeTransformedBounds2();
            }
            else
            {
                MyBindInsideElementTransformedBounds();
            }


            MyBindInsideGridRenderTransform();
            MyBindInsideGridRenderTransformOriginal();

            MyBindActualLocate();
            MyBindInsideElementLocate();

        }

        #region 初期バインド設定

        /// <summary>
        /// 中のGridの位置
        /// 回転拡縮などでの変化する位置の修正用
        /// </summary>
        private void MyBindInsideElementLocate()
        {
            BindingOperations.SetBinding(MyInsideGrid, Canvas.LeftProperty, new Binding() { Source = this, Path = new PropertyPath(MyInsideElementTransformedBoundsProperty), Converter = new MyConvInsideElementLeft() });
            BindingOperations.SetBinding(MyInsideGrid, Canvas.TopProperty, new Binding() { Source = this, Path = new PropertyPath(MyInsideElementTransformedBoundsProperty), Converter = new MyConvInsideElementTop() });
        }

        /// <summary>
        /// 自身の実際の位置設定用
        /// 回転拡縮などでの変化する位置修正後を実際の位置とする(canvas.leftなどとバインドする)
        /// 指定横位置(myleft)と中の要素のBoundsのLeftから取得
        /// </summary>
        private void MyBindActualLocate()
        {
            var mb = new MultiBinding() { Converter = new MyConvActualLeft() };
            mb.Bindings.Add(new Binding(nameof(ItemData.Left)) { Source = MyItemData });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyInsideElementTransformedBoundsProperty) });
            SetBinding(MyActualLeftProperty, mb);

            mb = new() { Converter = new MyConvActualTop() };
            mb.Bindings.Add(new Binding(nameof(ItemData.Top)) { Source = MyItemData });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyInsideElementTransformedBoundsProperty) });
            SetBinding(MyActualTopProperty, mb);

        }

        /// <summary>
        /// 中の図形要素の変形後のBounds取得用
        /// </summary>
        private void MyBindInsideGeoShapeTransformedBounds2()
        {
            var mb = new MultiBinding() { Converter = new MyConvInsideGeoShapeTransformedBounds2() };
            if (MyInsideElement is GeoShape shape)
            {
                mb.Bindings.Add(new Binding() { Source = shape, Path = new PropertyPath(GeoShape.MyGeometryProperty) });
                mb.Bindings.Add(new Binding() { Source = shape, Path = new PropertyPath(GeoShape.MyPenProperty) });

                mb.Bindings.Add(new Binding(nameof(ItemData.Angle)) { Source = MyItemData });
                mb.Bindings.Add(new Binding(nameof(ItemData.ScaleX)) { Source = MyItemData });
                mb.Bindings.Add(new Binding(nameof(ItemData.ScaleY)) { Source = MyItemData });
                mb.Bindings.Add(new Binding(nameof(ItemData.CenterX)) { Source = MyItemData });
                mb.Bindings.Add(new Binding(nameof(ItemData.CenterY)) { Source = MyItemData });
            }
            //mb.Bindings.Add(new Binding() { Source = MyInsideElement, Path = new PropertyPath(ActualWidthProperty) });
            //mb.Bindings.Add(new Binding() { Source = MyInsideElement, Path = new PropertyPath(ActualHeightProperty) });

            SetBinding(MyInsideElementTransformedBoundsProperty, mb);
        }

        /// <summary>
        /// 中の図形要素の変形後のBounds取得用
        /// </summary>
        private void MyBindInsideGeoShapeTransformedBounds()
        {
            var mb = new MultiBinding() { Converter = new MyConvInsideGeoShapeTransformedBounds() };
            if (MyInsideElement is GeoShape shape)
            {
                mb.Bindings.Add(new Binding() { Source = shape, Path = new PropertyPath(GeoShape.MyGeometryRenderBoundsProperty) });
                mb.Bindings.Add(new Binding(nameof(ItemData.Angle)) { Source = MyItemData });
                mb.Bindings.Add(new Binding(nameof(ItemData.ScaleX)) { Source = MyItemData });
                mb.Bindings.Add(new Binding(nameof(ItemData.ScaleY)) { Source = MyItemData });
                mb.Bindings.Add(new Binding(nameof(ItemData.CenterX)) { Source = MyItemData });
                mb.Bindings.Add(new Binding(nameof(ItemData.CenterY)) { Source = MyItemData });
            }
            //mb.Bindings.Add(new Binding() { Source = MyInsideElement, Path = new PropertyPath(ActualWidthProperty) });
            //mb.Bindings.Add(new Binding() { Source = MyInsideElement, Path = new PropertyPath(ActualHeightProperty) });

            SetBinding(MyInsideElementTransformedBoundsProperty, mb);
        }

        /// <summary>
        /// 中の通常要素の変形後のBounds取得用
        /// </summary>
        private void MyBindInsideElementTransformedBounds()
        {
            var mb = new MultiBinding() { Converter = new MyConvInsideTransformedBounds() };
            mb.Bindings.Add(new Binding(nameof(ItemData.Angle)) { Source = MyItemData });
            mb.Bindings.Add(new Binding(nameof(ItemData.ScaleX)) { Source = MyItemData });
            mb.Bindings.Add(new Binding(nameof(ItemData.ScaleY)) { Source = MyItemData });
            mb.Bindings.Add(new Binding(nameof(ItemData.CenterX)) { Source = MyItemData });
            mb.Bindings.Add(new Binding(nameof(ItemData.CenterY)) { Source = MyItemData });
            mb.Bindings.Add(new Binding() { Source = MyInsideElement, Path = new PropertyPath(ActualWidthProperty) });
            mb.Bindings.Add(new Binding() { Source = MyInsideElement, Path = new PropertyPath(ActualHeightProperty) });

            SetBinding(MyInsideElementTransformedBoundsProperty, mb);
        }

        ///// <summary>
        ///// 中の要素のBounds取得用
        ///// </summary>
        //private void MyBindInsideElementRenderTransformBounds()
        //{
        //    var mb = new MultiBinding() { Converter = new MyConvRenderTransormBounds() };
        //    mb.Bindings.Add(new Binding() { Source = MyInsideElement, Path = new PropertyPath(RenderTransformProperty) });
        //    mb.Bindings.Add(new Binding() { Source = MyInsideElement, Path = new PropertyPath(ActualWidthProperty) });
        //    mb.Bindings.Add(new Binding() { Source = MyInsideElement, Path = new PropertyPath(ActualHeightProperty) });
        //    SetBinding(MyInsideElementRenderBoundsProperty, mb);
        //}

        /// <summary>
        /// 中のGrid要素のRenderTransformOriginal
        /// </summary>
        private void MyBindInsideGridRenderTransformOriginal()
        {
            //中心で回転拡縮したいので、縦横サイズも必要
            var mb = new MultiBinding() { Converter = new MyConvPoint() };
            mb.Bindings.Add(new Binding(nameof(ItemData.CenterX)) { Source = MyItemData });
            mb.Bindings.Add(new Binding(nameof(ItemData.CenterY)) { Source = MyItemData });
            MyInsideGrid.SetBinding(RenderTransformOriginProperty, mb);
        }

        /// <summary>
        /// 中のGrid要素のRenderTransform
        /// </summary>
        private void MyBindInsideGridRenderTransform()
        {
            var mb = new MultiBinding() { Converter = new MyConvRenderTransform2() };
            mb.Bindings.Add(new Binding(nameof(ItemData.Angle)) { Source = MyItemData });
            mb.Bindings.Add(new Binding(nameof(ItemData.ScaleX)) { Source = MyItemData });
            mb.Bindings.Add(new Binding(nameof(ItemData.ScaleY)) { Source = MyItemData });
            MyInsideGrid.SetBinding(RenderTransformProperty, mb);
        }

        ///// <summary>
        ///// 中の要素のRenderTransformをItemDataと要素の縦横サイズから
        ///// </summary>
        //private void MyBindInsideElementRenderTransform()
        //{
        //    //中心で回転拡縮したいので、縦横サイズも必要
        //    var mb = new MultiBinding() { Converter = new MyConvRenderTransform() };
        //    mb.Bindings.Add(new Binding() { Source = MyInsideElement, Path = new PropertyPath(ActualWidthProperty) });
        //    mb.Bindings.Add(new Binding() { Source = MyInsideElement, Path = new PropertyPath(ActualHeightProperty) });
        //    mb.Bindings.Add(new Binding(nameof(ItemData.ScaleX)) { Source = MyItemData });
        //    mb.Bindings.Add(new Binding(nameof(ItemData.ScaleY)) { Source = MyItemData });
        //    mb.Bindings.Add(new Binding(nameof(ItemData.Angle)) { Source = MyItemData });
        //    MyInsideElement.SetBinding(RenderTransformProperty, mb);

        //}

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
        /// 中の要素の変形後のBounds。表示されている要素がピッタリ収まるRect
        /// </summary>
        public Rect MyInsideElementTransfomedBounds
        {
            get { return (Rect)GetValue(MyInsideElementTransformedBoundsProperty); }
            set { SetValue(MyInsideElementTransformedBoundsProperty, value); }
        }
        public static readonly DependencyProperty MyInsideElementTransformedBoundsProperty =
            DependencyProperty.Register(nameof(MyInsideElementTransfomedBounds), typeof(Rect), typeof(KisoThumb), new PropertyMetadata(new Rect()));


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