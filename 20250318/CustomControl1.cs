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

namespace _20250318
{

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
    /// アンカーハンドルの表示切替とドラッグ移動できるGeoShape
    /// </summary>
    public class GeoShapeThumb : Thumb
    {
        #region フィールド

        //アンカーハンドルを表示する装飾
        public AnchorHandleAdorner? MyShapesAnchorHandleAdorner { get; private set; }

        //装飾のLayer
        public AdornerLayer MyShapesAdornerLayer { get; private set; } = null!;

        //中に表示している図形
        private GeoShape MyGeoShape { get; set; } = null!;
        #endregion フィールド


        static GeoShapeThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GeoShapeThumb), new FrameworkPropertyMetadata(typeof(GeoShapeThumb)));
        }
        public GeoShapeThumb()
        {
            DragDelta += GeoShapeThumb_DragDelta;
        }

        //起動時、Templateの中から図形を取り出してバインド設定
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("shape") is GeoShape shape)
            {
                MyGeoShape = shape;
                MyShapesAdornerLayer = AdornerLayer.GetAdornerLayer(MyGeoShape);

                //MyPointsのバインド、自身のMyPointsが
                //nullなら図形のMyPointsをソースにする、
                //nullじゃなければ自身のMyPointsをソースにする
                if (MyPoints != null)
                {
                    MyGeoShape.SetBinding(GeoShape.MyPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty), Mode = BindingMode.TwoWay });
                }
                else
                {
                    SetBinding(MyPointsProperty, new Binding() { Source = MyGeoShape, Path = new PropertyPath(GeoShape.MyPointsProperty), Mode = BindingMode.TwoWay });
                }
            }
            else
            {
                throw new ArgumentNullException("Templateの中に図形が見つからない");
            }
        }



        //自身のドラッグ移動
        private void GeoShapeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            //アンカーハンドルが表示されているときだけ移動
            if (MyShapesAnchorHandleAdorner != null)
            {
                MyLeft += e.HorizontalChange;
                MyTop += e.VerticalChange;
                e.Handled = true;
            }
        }

        #region 依存関係プロパティ
        #region 図形とのバインド用

        public ShapeType MyShapeType
        {
            get { return (ShapeType)GetValue(MyShapeTypeProperty); }
            set { SetValue(MyShapeTypeProperty, value); }
        }
        public static readonly DependencyProperty MyShapeTypeProperty =
            DependencyProperty.Register(nameof(MyShapeType), typeof(ShapeType), typeof(GeoShapeThumb),
                new FrameworkPropertyMetadata(ShapeType.Line,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

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
        #endregion 図形とのバインド用


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

        public Rect GetShapeRenderBounds()
        {
            return MyGeoShape.MyRenderBounds;
        }


        /// <summary>
        /// 直線とベジェ曲線の切り替え
        /// </summary>
        public void ChangeShapeType()
        {
            if (MyShapeType == ShapeType.Line)
            {
                MyShapeType = ShapeType.Bezier;
                MyShapesAnchorHandleAdorner?.AddControlLine();
                MyGeoShape.MyShapeType = ShapeType.Bezier;
            }
            else
            {
                MyShapeType = ShapeType.Line;
                MyShapesAnchorHandleAdorner?.RemoveControlLine();
                MyGeoShape.MyShapeType = ShapeType.Line;
            }
        }
        //public void ChangeShapeType()
        //{
        //    if (MyGeoShape.MyShapeType == ShapeType.Line)
        //    {
        //        MyShapesAnchorHandleAdorner?.AddControlLine();
        //        MyGeoShape.MyShapeType = ShapeType.Bezier;
        //    }
        //    else if (MyGeoShape.MyShapeType == ShapeType.Bezier)
        //    {
        //        MyShapesAnchorHandleAdorner?.RemoveControlLine();
        //        MyGeoShape.MyShapeType = ShapeType.Line;
        //    }
        //}

        #region Pointの追加と削除
        
        /// <summary>
        /// Pointの追加
        /// </summary>
        /// <param name="index">挿入箇所Index</param>
        /// <param name="poi">Point</param>
        public void AddPoint(int index, Point poi)
        {
            MyPoints.Insert(index, poi);
            //図形に装飾が在れば、装飾の更新
            MyShapesAnchorHandleAdorner?.AddAnchorHandleThumb(index, poi);
        }

        /// <summary>
        /// Pointを末尾に追加
        /// </summary>
        /// <param name="poi"></param>
        public void AddPoint(Point poi)
        {
            int id = MyPoints.Count;
            AddPoint(id, poi);
        }

        /// <summary>
        /// Pointの削除
        /// </summary>
        /// <param name="index"></param>
        public void RemovePoint(int index)
        {
            //最低2個は残して削除処理
            if (MyPoints.Count > 2)
            {
                MyPoints.RemoveAt(index);
                MyShapesAnchorHandleAdorner?.RemoveAnchorHandleThumb(index);
            }
        }

        #endregion Pointの追加と削除

        /// <summary>
        /// アンカーハンドルの表示切替、Adornerの付け外し
        /// </summary>
        /// <returns>装飾</returns>
        public AnchorHandleAdorner? AnchorSwitch()
        {
            //図形のAdornerLayerに装飾が在れば削除、なければ作成、追加する
            if (MyShapesAnchorHandleAdorner == null)
            {
                MyShapesAnchorHandleAdorner = new AnchorHandleAdorner(MyGeoShape);
                MyShapesAdornerLayer.Add(MyShapesAnchorHandleAdorner);
                return MyShapesAnchorHandleAdorner;
            }
            else
            {
                MyShapesAdornerLayer.Remove(MyShapesAnchorHandleAdorner);
                MyShapesAnchorHandleAdorner = null;
                return null;
            }

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
        private AdornerLayer MyAdornerLayer { get; set; } = null!;
        static GeoShapeTThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GeoShapeTThumb), new FrameworkPropertyMetadata(typeof(GeoShapeTThumb)));
        }
        public GeoShapeTThumb() { }
        public GeoShapeTThumb(ItemData data)
        {
            MyItemData = data;
            DataContext = data;
            DragDelta += GeoShapeTThumb_DragDelta;
            Loaded += GeoShapeTThumb_Loaded;
        }

        private void GeoShapeTThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            MyLeft += e.HorizontalChange;
            MyTop += e.VerticalChange;
            e.Handled = true;
        }

        private void GeoShapeTThumb_Loaded(object sender, RoutedEventArgs e)
        {
            Width = MyShapeThumb.ActualWidth;
            Height = MyShapeThumb.ActualHeight;
            MyResizeHandleAdorner = new ResizeHandleAdorner(this);
            if (AdornerLayer.GetAdornerLayer(this) is AdornerLayer layer)
            {
                MyAdornerLayer = layer;
            }
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("shapeThumb") is GeoShapeThumb shape)
            {
                MyShapeThumb = shape;
                if (MyItemData.MyPoints == null)
                {
                    MyItemData.MyPoints = [new Point(0, 0), new Point(200, 100)];
                }
                MyBind();
            }

            void MyBind()
            {
                //MyShapeThumb.SetBinding(GeoShapeThumb.MyStrokeThicknessProperty, new Binding(nameof(ItemData.MyStrokeThickness)));

                //MyShapeThumb.SetBinding(GeoShapeThumb.MyPointsProperty, new Binding() { Source = MyItemData, Path = new PropertyPath(ItemData.MyPointsProperty), Mode = BindingMode.TwoWay });
            }
        }




        #region 依存関係プロパティ


        public ResizeHandleAdorner MyResizeHandleAdorner
        {
            get { return (ResizeHandleAdorner)GetValue(MyResizeHandleAdornerProperty); }
            protected set { SetValue(MyResizeHandleAdornerProperty, value); }
        }
        public static readonly DependencyProperty MyResizeHandleAdornerProperty =
            DependencyProperty.Register(nameof(MyResizeHandleAdorner), typeof(ResizeHandleAdorner), typeof(GeoShapeTThumb), new PropertyMetadata(null));



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

        public void FitSizeAndPos()
        {
            var myBounds = new Rect(MyLeft, MyTop, Width, Height);
            var unionBouns = new Rect(MyLeft, MyTop, Width, Height);
            var shapeBounds = MyShapeThumb.GetShapeRenderBounds();
            unionBouns.Union(shapeBounds);

            var shapeLeft = MyShapeThumb.MyLeft;
            var offsetLeft = shapeLeft + shapeBounds.Left;
            MyLeft += offsetLeft;
            MyShapeThumb.MyLeft -= offsetLeft;

            var shapeTop = MyShapeThumb.MyTop;
            var offsetTop = shapeTop + shapeBounds.Top;
            MyTop += offsetTop;
            MyShapeThumb.MyTop -= offsetTop;

            Width = shapeBounds.Width;
            Height = shapeBounds.Height;
        }
        public void AnchorHandleSwitch()
        {
            MyShapeThumb?.AnchorSwitch();
        }


        #region リサイズハンドル

        /// <summary>
        /// リサイズハンドルの表示切替、Adornerの付け外し
        /// </summary>
        public void ResizeHandleSwitch()
        {
            //図形のAdornerLayerを調べてリサイズ用の装飾が在れば削除、なければ作成追加する
            var items = MyAdornerLayer.GetAdorners(this);
            if (items != null)
            {
                RemoveResizeHandleAdorner();
            }
            else
            {
                AddResizeHandleAdorner();
            }
        }

        /// <summary>
        /// リサイズハンドルの追加と
        /// リサイズ時の縦横位置の変更量を通知するイベントを購読
        /// </summary>
        public void AddResizeHandleAdorner()
        {
            MyResizeHandleAdorner.OnTargetLeftChanged += MyResizeHandleAdorner_OnTargetLeftChanged;
            MyResizeHandleAdorner.OnTargetTopChanged += MyResizeHandleAdorner_OnTargetTopChanged;
            MyAdornerLayer.Add(MyResizeHandleAdorner);
        }

        /// <summary>
        /// リサイズハンドルの削除と
        /// リサイズ時の縦横位置の変更量を通知するイベントの購読を解除
        /// </summary>
        public void RemoveResizeHandleAdorner()
        {
            MyResizeHandleAdorner.OnTargetLeftChanged -= MyResizeHandleAdorner_OnTargetLeftChanged;
            MyResizeHandleAdorner.OnTargetTopChanged -= MyResizeHandleAdorner_OnTargetTopChanged;
            MyAdornerLayer.Remove(MyResizeHandleAdorner);
        }

        /// <summary>
        /// リサイズ時に縦位置が変更されたとき、図形を逆方向へ移動
        /// </summary>
        /// <param name="obj"></param>
        private void MyResizeHandleAdorner_OnTargetTopChanged(double obj)
        {
            MyShapeThumb.MyTop -= obj;
        }

        private void MyResizeHandleAdorner_OnTargetLeftChanged(double obj)
        {
            MyShapeThumb.MyLeft -= obj;
        }
        #endregion リサイズハンドル

        #endregion メソッド


    }



}