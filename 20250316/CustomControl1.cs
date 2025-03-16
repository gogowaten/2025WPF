using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace _20250316
{

    /// <summary>
    /// GeoShape用のアンカーハンドルThumb
    /// </summary>
    public class AnchorHandleThumb : Thumb
    {
        static AnchorHandleThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnchorHandleThumb), new FrameworkPropertyMetadata(typeof(AnchorHandleThumb)));
        }
        public AnchorHandleThumb()
        {

        }

        #region 依存関係プロパティ


        public double MyLeft
        {
            get { return (double)GetValue(MyLeftProperty); }
            set { SetValue(MyLeftProperty, value); }
        }
        public static readonly DependencyProperty MyLeftProperty =
            DependencyProperty.Register(nameof(MyLeft), typeof(double), typeof(AnchorHandleThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MyTop
        {
            get { return (double)GetValue(MyTopProperty); }
            set { SetValue(MyTopProperty, value); }
        }
        public static readonly DependencyProperty MyTopProperty =
            DependencyProperty.Register(nameof(MyTop), typeof(double), typeof(AnchorHandleThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MySize
        {
            get { return (double)GetValue(MySizeProperty); }
            set { SetValue(MySizeProperty, value); }
        }
        public static readonly DependencyProperty MySizeProperty =
            DependencyProperty.Register(nameof(MySize), typeof(double), typeof(AnchorHandleThumb), new PropertyMetadata(20.0));

        #endregion 依存関係プロパティ

    }


    /// <summary>
    /// リサイズ用のハンドルThumb
    /// </summary>
    public class HandleThumb : Thumb
    {
        static HandleThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HandleThumb), new FrameworkPropertyMetadata(typeof(HandleThumb)));
        }
        public HandleThumb()
        {

        }

        //Canvas.Leftとバインドする用
        public double MyLeft
        {
            get { return (double)GetValue(MyLeftProperty); }
            set { SetValue(MyLeftProperty, value); }
        }
        public static readonly DependencyProperty MyLeftProperty =
            DependencyProperty.Register(nameof(MyLeft), typeof(double), typeof(HandleThumb),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MyTop
        {
            get { return (double)GetValue(MyTopProperty); }
            set { SetValue(MyTopProperty, value); }
        }
        public static readonly DependencyProperty MyTopProperty =
            DependencyProperty.Register(nameof(MyTop), typeof(double), typeof(HandleThumb),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    }



    /// <summary>
    /// アンカーハンドルの表示切替とドラッグ移動できるGeoShape
    /// </summary>
    public class GeoShapeThumb : Thumb
    {
        static GeoShapeThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GeoShapeThumb), new FrameworkPropertyMetadata(typeof(GeoShapeThumb)));
        }
        public GeoShapeThumb()
        {
            DragDelta += GeoShapeThumb_DragDelta;
            
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("shape") is GeoShape shape)
            {
                MyGeoShape = shape;
                MyGeoShape.SetBinding(GeoShape.StrokeThicknessProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeThicknessProperty) });
                //MyPointsのバインド、自身のMyPointsが
                //nullなら図形のMyPointsをソースにする、
                //nullじゃなければ自身をソースにする
                if (MyPoints != null)
                {
                    MyGeoShape.SetBinding(GeoShape.MyPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty), Mode = BindingMode.TwoWay });
                }
                else
                {
                    SetBinding(MyPointsProperty, new Binding() { Source = MyGeoShape, Path = new PropertyPath(GeoShape.MyPointsProperty), Mode = BindingMode.TwoWay });
                }

                //if (MyPoints != null)
                //{
                //    MyGeoShape.MyPoints = MyPoints;
                //}
                //else { MyPoints = MyGeoShape.MyPoints; }
            }
            else
            {
                throw new ArgumentNullException("Templateの中に図形が見つからない");
            }
        }



        private void GeoShapeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            MyLeft += e.HorizontalChange;
            MyTop += e.VerticalChange;
            e.Handled = true;
        }

        #region 依存関係プロパティ

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(GeoShapeThumb), new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure |
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public double MyStrokeThickness
        {
            get { return (double)GetValue(MyStrokeThicknessProperty); }
            set { SetValue(MyStrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeThicknessProperty =
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(GeoShapeThumb),
                new FrameworkPropertyMetadata(10.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public GeoShape MyGeoShape
        {
            get { return (GeoShape)GetValue(MyGeoShapeProperty); }
            protected set { SetValue(MyGeoShapeProperty, value); }
        }
        public static readonly DependencyProperty MyGeoShapeProperty =
            DependencyProperty.Register(nameof(MyGeoShape), typeof(GeoShape), typeof(GeoShapeThumb), new PropertyMetadata(null));

        public double MyLeft
        {
            get { return (double)GetValue(MyLeftProperty); }
            set { SetValue(MyLeftProperty, value); }
        }
        public static readonly DependencyProperty MyLeftProperty =
            DependencyProperty.Register(nameof(MyLeft), typeof(double), typeof(GeoShapeThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MyTop
        {
            get { return (double)GetValue(MyTopProperty); }
            set { SetValue(MyTopProperty, value); }
        }
        public static readonly DependencyProperty MyTopProperty =
            DependencyProperty.Register(nameof(MyTop), typeof(double), typeof(GeoShapeThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion 依存関係プロパティ


        #region メソッド
        /// <summary>
        /// アンカーハンドルの表示切替、Adornerの付け外し
        /// </summary>
        /// <returns>装飾</returns>
        public AncorHandleAdorner? AnchorSwitch()
        {
            //図形のAdornerLayerを調べて装飾が在れば削除、なければ作成、追加する
            if (AdornerLayer.GetAdornerLayer(MyGeoShape) is AdornerLayer layer)
            {
                var items = layer.GetAdorners(MyGeoShape);
                if (items != null)
                {
                    foreach (var item in items.OfType<AncorHandleAdorner>())
                    {
                        layer.Remove(item);
                    }
                    return null;
                }
                else
                {
                    var adorner = new AncorHandleAdorner(MyGeoShape);
                    layer.Add(adorner);
                    return adorner;
                }
            }
            return null;
        }
        #endregion メソッド

    }



    /// <summary>
    /// Canvasの中にGeoShapeThumbを配置したThumb
    /// 作成にはItemDataが必要
    /// 図形のアンカーハンドルの表示切替
    /// 自身のリサイズ用のハンドルの表示切替
    /// 図形の移動と自身の移動
    /// </summary>
    public class GeoShapeTThumb : Thumb
    {
        static GeoShapeTThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GeoShapeTThumb), new FrameworkPropertyMetadata(typeof(GeoShapeTThumb)));
        }
        //public GeoShapeTThumb() { }
        public GeoShapeTThumb(ItemData data)
        {
            MyItemData = data;
            DataContext = data;
            DragDelta += ShapeThumb_DragDelta;
            Loaded += GeoShapeTThumb_Loaded;
        }

        private void GeoShapeTThumb_Loaded(object sender, RoutedEventArgs e)
        {
            Width = MyShapeThumb.ActualWidth;
            Height = MyShapeThumb.ActualHeight;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("shapeThumb") is GeoShapeThumb shape)
            {
                MyShapeThumb = shape;
                if (MyItemData.MyPoints == null)
                {
                    MyItemData.MyPoints = [new Point(0, 0),new Point(200,100)];
                }
                MyBind();
            }

            void MyBind()
            {
                MyShapeThumb.SetBinding(GeoShapeThumb.MyStrokeThicknessProperty, new Binding(nameof(ItemData.MyStrokeThickness)));

                MyShapeThumb.SetBinding(GeoShapeThumb.MyPointsProperty, new Binding() { Source = MyItemData, Path = new PropertyPath(ItemData.MyPointsProperty), Mode = BindingMode.TwoWay });
            }
        }

        private void ShapeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            MyLeft += e.HorizontalChange;
            MyTop += e.VerticalChange;
            e.Handled = true;
        }



        #region 依存関係プロパティ

        #region 図形とのバインド用

        public GeoShapeThumb MyShapeThumb
        {
            get { return (GeoShapeThumb)GetValue(MyShapeThumbProperty); }
            set { SetValue(MyShapeThumbProperty, value); }
        }
        public static readonly DependencyProperty MyShapeThumbProperty =
            DependencyProperty.Register(nameof(MyShapeThumb), typeof(GeoShapeThumb), typeof(GeoShapeTThumb), new PropertyMetadata(null));
        #endregion 図形とのバインド用

        #region 自身用

        public ItemData MyItemData
        {
            get { return (ItemData)GetValue(MyItemDataProperty); }
            set { SetValue(MyItemDataProperty, value); }
        }
        public static readonly DependencyProperty MyItemDataProperty =
            DependencyProperty.Register(nameof(MyItemData), typeof(ItemData), typeof(GeoShapeTThumb), new PropertyMetadata(null));


        public double MyLeft
        {
            get { return (double)GetValue(MyLeftProperty); }
            set { SetValue(MyLeftProperty, value); }
        }
        public static readonly DependencyProperty MyLeftProperty =
            DependencyProperty.Register(nameof(MyLeft), typeof(double), typeof(GeoShapeTThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MyTop
        {
            get { return (double)GetValue(MyTopProperty); }
            set { SetValue(MyTopProperty, value); }
        }
        public static readonly DependencyProperty MyTopProperty =
            DependencyProperty.Register(nameof(MyTop), typeof(double), typeof(GeoShapeTThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion 自身用

        #endregion 依存関係プロパティ

        #region メソッド

        public void AnchorHandleSwitch()
        {
            MyShapeThumb?.AnchorSwitch();
        }


        /// <summary>
        /// リサイズハンドルの表示切替、Adornerの付け外し
        /// </summary>
        /// <returns>装飾</returns>
        public ResizeHandleAdorner? ResizeHandleSwitch()
        {
            //図形のAdornerLayerを調べて装飾が在れば削除、なければ作成、追加する
            if (AdornerLayer.GetAdornerLayer(this) is AdornerLayer layer)
            {
                var items = layer.GetAdorners(this);
                if (items != null)
                {
                    foreach (var item in items.OfType<ResizeHandleAdorner>())
                    {
                        layer.Remove(item);
                    }
                    return null;
                }
                else
                {
                    var adorner = new ResizeHandleAdorner(this);
                    layer.Add(adorner);
                    return adorner;
                }
            }
            return null;
        }
        #endregion メソッド


    }


}