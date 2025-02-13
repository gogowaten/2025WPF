using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250213_TopLeftRotateEzLineThumb
{
    /// <summary>
    /// 頂点移動用のThumbの基礎
    /// </summary>
    public abstract class AnchorThumb : Thumb
    {
        static AnchorThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnchorThumb), new FrameworkPropertyMetadata(typeof(AnchorThumb)));
        }
        public AnchorThumb()
        {

        }

        public double MyOutSize
        {
            get { return (double)GetValue(MyOutSizeProperty); }
            set { SetValue(MyOutSizeProperty, value); }
        }
        public static readonly DependencyProperty MyOutSizeProperty =
            DependencyProperty.Register(nameof(MyOutSize), typeof(double), typeof(AnchorThumb), new PropertyMetadata(21.0));

        public double MyInSize
        {
            get { return (double)GetValue(MyInSizeProperty); }
            set { SetValue(MyInSizeProperty, value); }
        }
        public static readonly DependencyProperty MyInSizeProperty =
            DependencyProperty.Register(nameof(MyInSize), typeof(double), typeof(AnchorThumb), new PropertyMetadata(21.0));

        public Brush MyOutBrush
        {
            get { return (Brush)GetValue(MyOutBrushProperty); }
            set { SetValue(MyOutBrushProperty, value); }
        }
        public static readonly DependencyProperty MyOutBrushProperty =
            DependencyProperty.Register(nameof(MyOutBrush), typeof(Brush), typeof(AnchorThumb), new PropertyMetadata(Brushes.Red));

        public Brush MyInBrush
        {
            get { return (Brush)GetValue(MyInBrushProperty); }
            set { SetValue(MyInBrushProperty, value); }
        }
        public static readonly DependencyProperty MyInBrushProperty =
            DependencyProperty.Register(nameof(MyInBrush), typeof(Brush), typeof(AnchorThumb), new PropertyMetadata(Brushes.White));

        public double MyOutThickness
        {
            get { return (double)GetValue(MyOutThicknessProperty); }
            set { SetValue(MyOutThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyOutThicknessProperty =
            DependencyProperty.Register(nameof(MyOutThickness), typeof(double), typeof(AnchorThumb), new PropertyMetadata(1.0));

        public double MyInThickness
        {
            get { return (double)GetValue(MyInThicknessProperty); }
            set { SetValue(MyInThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyInThicknessProperty =
            DependencyProperty.Register(nameof(MyInThickness), typeof(double), typeof(AnchorThumb), new PropertyMetadata(1.0));

        public DoubleCollection MyDashArray
        {
            get { return (DoubleCollection)GetValue(MyDashArrayProperty); }
            set { SetValue(MyDashArrayProperty, value); }
        }
        public static readonly DependencyProperty MyDashArrayProperty =
            DependencyProperty.Register(nameof(MyDashArray), typeof(DoubleCollection), typeof(AnchorThumb), new PropertyMetadata(new DoubleCollection() { 1.0 }));


        public Brush MyOutBackground
        {
            get { return (Brush)GetValue(MyOutBackgroundProperty); }
            set { SetValue(MyOutBackgroundProperty, value); }
        }
        public static readonly DependencyProperty MyOutBackgroundProperty =
            DependencyProperty.Register(nameof(MyOutBackground), typeof(Brush), typeof(AnchorThumb), new PropertyMetadata(Brushes.Transparent));

    }

    public class AnchorEllipseThumb : AnchorThumb
    {
        static AnchorEllipseThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnchorEllipseThumb), new FrameworkPropertyMetadata(typeof(AnchorEllipseThumb)));
        }
        public AnchorEllipseThumb()
        {

        }
    }

    public class KisoThumb : Thumb
    {
        static KisoThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KisoThumb), new FrameworkPropertyMetadata(typeof(KisoThumb)));
        }
        public KisoThumb()
        {
            DataContext = this;
            DragDelta += KisoThumb_DragDelta;
        }

        private void KisoThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is KisoThumb kiso)
            {
                kiso.MyLeft += e.HorizontalChange;
                kiso.MyTop += e.VerticalChange;
                e.Handled = true;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_Canvas") is Canvas panel)
            {
                MyBaseCanvas = panel;
            }
        }

        #region 依存関係プロパティ

        #region 共通

        //テキスト系要素のText要素
        public string MyText
        {
            get { return (string)GetValue(MyTextProperty); }
            set { SetValue(MyTextProperty, value); }
        }
        public static readonly DependencyProperty MyTextProperty =
            DependencyProperty.Register(nameof(MyText), typeof(string), typeof(KisoThumb),
                new FrameworkPropertyMetadata(string.Empty,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //コンテンツの回転角度
        public double MyAngle
        {
            get { return (double)GetValue(MyAngleProperty); }
            set { SetValue(MyAngleProperty, value); }
        }
        public static readonly DependencyProperty MyAngleProperty =
            DependencyProperty.Register(nameof(MyAngle), typeof(double), typeof(KisoThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        public Canvas MyBaseCanvas
        {
            get { return (Canvas)GetValue(MyBaseCanvasProperty); }
            private set { SetValue(MyBaseCanvasProperty, value); }
        }
        public static readonly DependencyProperty MyBaseCanvasProperty =
            DependencyProperty.Register(nameof(MyBaseCanvas), typeof(Canvas), typeof(KisoThumb), new PropertyMetadata(null));


        public double MyLeft
        {
            get { return (double)GetValue(MyLeftProperty); }
            set { SetValue(MyLeftProperty, value); }
        }
        public static readonly DependencyProperty MyLeftProperty =
            DependencyProperty.Register(nameof(MyLeft), typeof(double), typeof(KisoThumb),
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
            DependencyProperty.Register(nameof(MyTop), typeof(double), typeof(KisoThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public double MyOffsetLeft
        {
            get { return (double)GetValue(MyOffsetLeftProperty); }
            internal set { SetValue(MyOffsetLeftProperty, value); }
        }
        public static readonly DependencyProperty MyOffsetLeftProperty =
            DependencyProperty.Register(nameof(MyOffsetLeft), typeof(double), typeof(KisoThumb), new PropertyMetadata(0.0));

        public double MyOffsetTop
        {
            get { return (double)GetValue(MyOffsetTopProperty); }
            internal set { SetValue(MyOffsetTopProperty, value); }
        }
        public static readonly DependencyProperty MyOffsetTopProperty =
            DependencyProperty.Register(nameof(MyOffsetTop), typeof(double), typeof(KisoThumb), new PropertyMetadata(0.0));


        #endregion 共通

        #region 図形関連
        #region 図形基本

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(KisoThumb),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public double MyStrokeThickness
        {
            get { return (double)GetValue(MyStrokeThicknessProperty); }
            set { SetValue(MyStrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeThicknessProperty =
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(KisoThumb), new FrameworkPropertyMetadata(1.0,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Brush MyStroke
        {
            get { return (Brush)GetValue(MyStrokeProperty); }
            set { SetValue(MyStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeProperty =
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(KisoThumb), new FrameworkPropertyMetadata(Brushes.Magenta,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        public Brush MyFill
        {
            get { return (Brush)GetValue(MyFillProperty); }
            set { SetValue(MyFillProperty, value); }
        }
        public static readonly DependencyProperty MyFillProperty =
            DependencyProperty.Register(nameof(MyFill), typeof(Brush), typeof(KisoThumb), new FrameworkPropertyMetadata(Brushes.Pink,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        public bool MyIsFilled
        {
            get { return (bool)GetValue(MyIsFilledProperty); }
            set { SetValue(MyIsFilledProperty, value); }
        }
        public static readonly DependencyProperty MyIsFilledProperty =
            DependencyProperty.Register(nameof(MyIsFilled), typeof(bool), typeof(KisoThumb), new FrameworkPropertyMetadata(true,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        public bool MyIsStroked
        {
            get { return (bool)GetValue(MyIsStrokedProperty); }
            set { SetValue(MyIsStrokedProperty, value); }
        }
        public static readonly DependencyProperty MyIsStrokedProperty =
            DependencyProperty.Register(nameof(MyIsStroked), typeof(bool), typeof(KisoThumb), new FrameworkPropertyMetadata(true,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        public bool MyIsClosed
        {
            get { return (bool)GetValue(MyIsClosedProperty); }
            set { SetValue(MyIsClosedProperty, value); }
        }
        public static readonly DependencyProperty MyIsClosedProperty =
            DependencyProperty.Register(nameof(MyIsClosed), typeof(bool), typeof(KisoThumb), new FrameworkPropertyMetadata(false,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        public bool MyIsSmoothJoin
        {
            get { return (bool)GetValue(MyIsSmoothJoinProperty); }
            set { SetValue(MyIsSmoothJoinProperty, value); }
        }
        public static readonly DependencyProperty MyIsSmoothJoinProperty =
            DependencyProperty.Register(nameof(MyIsSmoothJoin), typeof(bool), typeof(KisoThumb),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public FillRule MyFillRule
        {
            get { return (FillRule)GetValue(MyFillRuleProperty); }
            set { SetValue(MyFillRuleProperty, value); }
        }
        public static readonly DependencyProperty MyFillRuleProperty =
            DependencyProperty.Register(nameof(MyFillRule), typeof(FillRule), typeof(KisoThumb),
                new FrameworkPropertyMetadata(FillRule.Nonzero,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public PenLineJoin MyStrokeLineJoin
        {
            get { return (PenLineJoin)GetValue(MyStrokeLineJoinProperty); }
            set { SetValue(MyStrokeLineJoinProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeLineJoinProperty =
            DependencyProperty.Register(nameof(MyStrokeLineJoin), typeof(PenLineJoin), typeof(KisoThumb),
                new FrameworkPropertyMetadata(PenLineJoin.Miter,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 図形基本

        #region 図形細部

        public DoubleCollection MyStrokeDashArray
        {
            get { return (DoubleCollection)GetValue(MyStrokeDashArrayProperty); }
            set { SetValue(MyStrokeDashArrayProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeDashArrayProperty =
            DependencyProperty.Register(nameof(MyStrokeDashArray), typeof(DoubleCollection), typeof(KisoThumb),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public PenLineCap MyStrokeDashCap
        {
            get { return (PenLineCap)GetValue(MyStrokeDashCapProperty); }
            set { SetValue(MyStrokeDashCapProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeDashCapProperty =
            DependencyProperty.Register(nameof(MyStrokeDashCap), typeof(PenLineCap), typeof(KisoThumb),
                new FrameworkPropertyMetadata(PenLineCap.Flat,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public double MyStrokeDashOffset
        {
            get { return (double)GetValue(MyStrokeDashOffsetProperty); }
            set { SetValue(MyStrokeDashOffsetProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeDashOffsetProperty =
            DependencyProperty.Register(nameof(MyStrokeDashOffset), typeof(double), typeof(KisoThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public PenLineCap MyStrokeEndLineCap
        {
            get { return (PenLineCap)GetValue(MyStrokeEndLineCapProperty); }
            set { SetValue(MyStrokeEndLineCapProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeEndLineCapProperty =
            DependencyProperty.Register(nameof(MyStrokeEndLineCap), typeof(PenLineCap), typeof(KisoThumb),
                new FrameworkPropertyMetadata(PenLineCap.Flat,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public PenLineCap MyStrokeStartLineCap
        {
            get { return (PenLineCap)GetValue(MyStrokeStartLineCapProperty); }
            set { SetValue(MyStrokeStartLineCapProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeStartLineCapProperty =
            DependencyProperty.Register(nameof(MyStrokeStartLineCap), typeof(PenLineCap), typeof(KisoThumb),
                new FrameworkPropertyMetadata(PenLineCap.Flat,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public double MyStrokeMiterLimit
        {
            get { return (double)GetValue(MyStrokeMiterLimitProperty); }
            set { SetValue(MyStrokeMiterLimitProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeMiterLimitProperty =
            DependencyProperty.Register(nameof(MyStrokeMiterLimit), typeof(double), typeof(KisoThumb),
                new FrameworkPropertyMetadata(10.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 図形細部
        #endregion 図形関連

        #region 特殊

        //public Rect MyContentBounds
        //{
        //    get { return (Rect)GetValue(MyContentBoundsProperty); }
        //    set { SetValue(MyContentBoundsProperty, value); }
        //}
        //public static readonly DependencyProperty MyContentBoundsProperty =
        //    DependencyProperty.Register(nameof(MyContentBounds), typeof(Rect), typeof(KisoThumb), new PropertyMetadata(new Rect()));

        #endregion 特殊
        #endregion 依存関係プロパティ

    }


    public class EzLineThumb : KisoThumb
    {
        //public ObservableCollection<Thumb> MyAnchors { get; private set; } = [];
        static EzLineThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EzLineThumb), new FrameworkPropertyMetadata(typeof(EzLineThumb)));
        }
        public EzLineThumb()
        {

        }


        #region 頂点移動用のThumb表示用のAdorner

        //表示
        public void AnchorOn()
        {
            if (AdornerLayer.GetAdornerLayer(MyEzLine) is AdornerLayer layer)
            {
                AnchorClear(layer);
                EzLineAdorner ado = new(MyEzLine);
                layer.Add(ado);
            }
        }

        //削除
        public void AnchorOff()
        {
            if (AdornerLayer.GetAdornerLayer(MyEzLine) is AdornerLayer layer)
            {
                AnchorClear(layer);
            }
        }

        //指定AdornerLayerに含まれるEzLineAdornerすべてを削除
        private void AnchorClear(AdornerLayer layer)
        {
            var ados = layer.GetAdorners(MyEzLine);
            if (ados is null) { return; }
            for (int i = 0; i < ados.Length; i++)
            {
                if (ados[i] is EzLineAdorner ezado)
                {
                    layer.Remove(ezado);
                }
            }
        }

        //頂点追加時、Adornerをリセット、効率よくないけど簡単
        //Pointの追加、削除時に使用
        public void AnchorReset()
        {
            AnchorOff();
            AnchorOn();
        }

        #endregion 頂点移動用のThumb表示用のAdorner

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //テンプレートの中からEzLineを取得してバインド設定
            if (GetTemplateChild("ez") is EzLine ez)
            {
                MyEzLine = ez;

                //オフセット位置のバインド
                //このタイミングじゃないとバインドできないし、XAMLでもできない。
                //ソースにEzLineを使っているから、取得後じゃないとできないみたい
                MultiBinding mb = new() { Converter = new MyConverterOffsetX() };
                mb.Bindings.Add(MakeOneWayBind(MyLeftProperty));
                mb.Bindings.Add(new Binding() { Source = MyEzLine, Path = new PropertyPath(EzLine.MyBounds4Property), Mode = BindingMode.OneWay });
                SetBinding(MyOffsetLeftProperty, mb);

                mb = new() { Converter = new MyConverterOffsetY() };
                mb.Bindings.Add(MakeOneWayBind(MyTopProperty));
                mb.Bindings.Add(new Binding() { Source = MyEzLine, Path = new PropertyPath(EzLine.MyBounds4Property), Mode = BindingMode.OneWay });
                SetBinding(MyOffsetTopProperty, mb);
            }
        }

        /// <summary>
        /// Binding生成。ソースはthis固定、ModeはOneWay固定
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private Binding MakeOneWayBind(DependencyProperty property)
        {
            return new Binding() { Source = this, Path = new PropertyPath(property), Mode = BindingMode.OneWay };
        }


        public EzLine MyEzLine
        {
            get { return (EzLine)GetValue(MyEzLineProperty); }
            private set { SetValue(MyEzLineProperty, value); }
        }
        public static readonly DependencyProperty MyEzLineProperty =
            DependencyProperty.Register(nameof(MyEzLine), typeof(EzLine), typeof(EzLineThumb), new PropertyMetadata(null));

        /// <summary>
        /// 回転軸を左上にする
        /// 頂点移動や追加削除で回転軸がズレてしまったのを直す
        /// </summary>
        public void ZeroFix()
        {
            var rect1 = MyEzLine.MyBounds1;
            //全頂点座標の修正
            for (int i = 0; i < MyPoints.Count; i++)
            {
                Point po = MyPoints[i];
                MyPoints[i] = new Point(po.X - rect1.X, po.Y - rect1.Y);
            }
            //自身の座標を修正
            MyLeft += rect1.Left;
            MyTop += rect1.Top;

            AnchorReset();
        }


        /// <summary>
        /// 頂点追加
        /// </summary>
        /// <param name="po">座標</param>
        /// <param name="id">index、リストの中での位置</param>
        public void AddPoint(Point po, int id)
        {
            MyPoints.Insert(id, po);
            AnchorReset();
        }

        /// <summary>
        /// 頂点削除
        /// </summary>
        /// <param name="id">index、リストの中での位置</param>
        public void RemovePoint(int id)
        {
            MyPoints.RemoveAt(id);
            AnchorReset();
        }
    }

    #region コンバーター


    //指定表示座標とコンテンツの変形後のRectから、自身の実際のY座標を返す
    public class MyConverterOffsetY : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var y = (double)values[0];
            var r = (Rect)values[1];
            return y + r.Top;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //指定表示座標とコンテンツの変形後のRectから、自身の実際のX座標を返す
    public class MyConverterOffsetX : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var x = (double)values[0];
            var r = (Rect)values[1];
            return x + r.Left;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion コンバーター
}
