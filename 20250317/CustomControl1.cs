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

namespace _20250317
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
                MyGeoShape.SetBinding(GeoShape.StrokeThicknessProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeThicknessProperty) });
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
            MyLeft += e.HorizontalChange;
            MyTop += e.VerticalChange;
            e.Handled = true;
        }

        #region 依存関係プロパティ
        #region 図形とのバインド用

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

        /// <summary>
        /// 直線とベジェ曲線の切り替え
        /// </summary>
        public void ChangeShapeType()
        {
            if (MyGeoShape.MyShapeType == ShapeType.Line)
            {
                MyShapesAnchorHandleAdorner?.AddControlLine();
                MyGeoShape.MyShapeType = ShapeType.Bezier;
            }
            else if (MyGeoShape.MyShapeType == ShapeType.Bezier)
            {
                MyShapesAnchorHandleAdorner?.RemoveControlLine();
                MyGeoShape.MyShapeType = ShapeType.Line;
            }
        }

        #region メソッド
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


            //if (AdornerLayer.GetAdornerLayer(MyGeoShape) is AdornerLayer layer)
            //{
            //    var items = layer.GetAdorners(MyGeoShape);
            //    if (items != null)
            //    {
            //        foreach (var item in items.OfType<AnchorHandleAdorner>())
            //        {
            //            layer.Remove(item);
            //        }
            //        return null;
            //    }
            //    else
            //    {
            //        var adorner = new AnchorHandleAdorner(MyGeoShape);
            //        layer.Add(adorner);
            //        return adorner;
            //    }
            //}
            //return null;
        }
        #endregion メソッド

    }






}