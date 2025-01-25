using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace _20250125_EzLine
{
    public class EzLine : FrameworkElement
    {
        #region 依存関係プロパティ
        // FrameworkPropertyMetadataOptions.AffectsRender // デザイン画面上での更新で必要
        // FrameworkPropertyMetadataOptions.AffectsMeasure // 必要ないかも？



        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(EzLine),
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
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(EzLine), new FrameworkPropertyMetadata(1.0,
                FrameworkPropertyMetadataOptions.AffectsRender |// デザイン画面上での更新で必要
                FrameworkPropertyMetadataOptions.AffectsMeasure)); // 必要ないかも？

        public Brush MyStroke
        {
            get { return (Brush)GetValue(MyStrokeProperty); }
            set { SetValue(MyStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeProperty =
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(EzLine), new FrameworkPropertyMetadata(Brushes.Magenta,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        public Brush MyFill
        {
            get { return (Brush)GetValue(MyFillProperty); }
            set { SetValue(MyFillProperty, value); }
        }
        public static readonly DependencyProperty MyFillProperty =
            DependencyProperty.Register(nameof(MyFill), typeof(Brush), typeof(EzLine), new FrameworkPropertyMetadata(Brushes.Pink,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        public bool MyIsFilled
        {
            get { return (bool)GetValue(MyIsFilledProperty); }
            set { SetValue(MyIsFilledProperty, value); }
        }
        public static readonly DependencyProperty MyIsFilledProperty =
            DependencyProperty.Register(nameof(MyIsFilled), typeof(bool), typeof(EzLine), new FrameworkPropertyMetadata(true,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        public bool MyIsStroked
        {
            get { return (bool)GetValue(MyIsStrokedProperty); }
            set { SetValue(MyIsStrokedProperty, value); }
        }
        public static readonly DependencyProperty MyIsStrokedProperty =
            DependencyProperty.Register(nameof(MyIsStroked), typeof(bool), typeof(EzLine), new FrameworkPropertyMetadata(true,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        public bool MyIsClosed
        {
            get { return (bool)GetValue(MyIsClosedProperty); }
            set { SetValue(MyIsClosedProperty, value); }
        }
        public static readonly DependencyProperty MyIsClosedProperty =
            DependencyProperty.Register(nameof(MyIsClosed), typeof(bool), typeof(EzLine), new FrameworkPropertyMetadata(false,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        public FillRule MyFillRule
        {
            get { return (FillRule)GetValue(MyFillRuleProperty); }
            set { SetValue(MyFillRuleProperty, value); }
        }
        public static readonly DependencyProperty MyFillRuleProperty =
            DependencyProperty.Register(nameof(MyFillRule), typeof(FillRule), typeof(EzLine),
                new FrameworkPropertyMetadata(FillRule.Nonzero,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        //読み取り専用のPen、なぜかバインドできない、エラーになる
        //private static readonly DependencyPropertyKey MyPenPropertyKey =
        //    DependencyProperty.RegisterReadOnly(nameof(MyPen), typeof(Pen), typeof(EzLine), new PropertyMetadata(null));
        //public static readonly DependencyProperty MyPenProperty = MyPenPropertyKey.DependencyProperty;
        //public Pen MyPen
        //{
        //    get { return (Pen)GetValue(MyPenPropertyKey.DependencyProperty); }
        //    internal set { SetValue(MyPenPropertyKey, value); }
        //}

        public Pen MyPen
        {
            get { return (Pen)GetValue(MyPenProperty); }
            set { SetValue(MyPenProperty, value); }
        }
        public static readonly DependencyProperty MyPenProperty =
            DependencyProperty.Register(nameof(MyPen), typeof(Pen), typeof(EzLine), new FrameworkPropertyMetadata(new Pen(),
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        #endregion 依存関係プロパティ

        #region 読み取り専用依存関係プロパティ

        private static readonly DependencyPropertyKey MyPathGeometryPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyPathGeometry), typeof(PathGeometry), typeof(EzLine), new PropertyMetadata(null));
        public static readonly DependencyProperty MyPathGeometryProperty = MyPathGeometryPropertyKey.DependencyProperty;
        public PathGeometry MyPathGeometry
        {
            get { return (PathGeometry)GetValue(MyPathGeometryPropertyKey.DependencyProperty); }
            internal set { SetValue(MyPathGeometryPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey MyPathFigurePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyPathFigure), typeof(PathFigure), typeof(EzLine), new PropertyMetadata(null));
        public static readonly DependencyProperty MyPathFigureProperty = MyPathFigurePropertyKey.DependencyProperty;
        public PathFigure MyPathFigure
        {
            get { return (PathFigure)GetValue(MyPathFigurePropertyKey.DependencyProperty); }
            internal set { SetValue(MyPathFigurePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey MyPolyLineSegmentPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyPolyLineSegment), typeof(PolyLineSegment), typeof(EzLine), new PropertyMetadata(null));
        public static readonly DependencyProperty MyPolyLineSegmentProperty = MyPolyLineSegmentPropertyKey.DependencyProperty;
        public PolyLineSegment MyPolyLineSegment
        {
            get { return (PolyLineSegment)GetValue(MyPolyLineSegmentPropertyKey.DependencyProperty); }
            internal set { SetValue(MyPolyLineSegmentPropertyKey, value); }
        }

        #endregion 読み取り専用依存関係プロパティ

        public EzLine()
        {
            //DataContext = this;
            MyPathGeometry = new();
            MyPathFigure = new();
            Initialized += EzLine_Initialized;
        }



        private void EzLine_Initialized(object? sender, EventArgs e)
        {
            MyPolyLineSegment = new();
            MyPathFigure.Segments.Add(MyPolyLineSegment);
            if (MyPoints != null && MyPoints.Count >= 1)
            {
                MyPathFigure.StartPoint = MyPoints[0];
            }

            MyPathGeometry.Figures.Add(MyPathFigure);
            MySetBindings();
        }

        private void MySetBindings()
        {
            //PolylineSegmentとのバインド
            _ = BindingOperations.SetBinding(MyPolyLineSegment, PathSegment.IsStrokedProperty, new Binding() { Source = this, Path = new PropertyPath(MyIsStrokedProperty), Mode = BindingMode.TwoWay });
            _ = BindingOperations.SetBinding(MyPolyLineSegment, PolyLineSegment.PointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty), Converter = new MyConverterSegment() });

            //PathFigureとのバインド
            _ = BindingOperations.SetBinding(MyPathFigure, PathFigure.IsClosedProperty, new Binding() { Source = this, Path = new PropertyPath(MyIsClosedProperty), Mode = BindingMode.TwoWay });
            _ = BindingOperations.SetBinding(MyPathFigure, PathFigure.IsFilledProperty, new Binding() { Source = this, Path = new PropertyPath(MyIsFilledProperty), Mode = BindingMode.TwoWay });
            _ = BindingOperations.SetBinding(MyPathFigure, PathFigure.StartPointProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty), Converter = new MyConverterStratPoint() });

            //StrokeとStrokeThicknessからPen
            Binding b1 = new() { Source = this, Path = new PropertyPath(MyStrokeProperty) };
            Binding b2 = new() { Source = this, Path = new PropertyPath(MyStrokeThicknessProperty) };
            MultiBinding mb = new() { Converter = new MyCovnerterPen(), Mode = BindingMode.TwoWay };
            mb.Bindings.Add(b1);
            mb.Bindings.Add(b2);
            _ = BindingOperations.SetBinding(this, MyPenProperty, mb);

            //PathGeometryとのバインド
            _ = BindingOperations.SetBinding(MyPathGeometry, PathGeometry.FillRuleProperty, new Binding() { Source = this, Path = new PropertyPath(MyFillRuleProperty) });

        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawGeometry(MyFill, MyPen, MyPathGeometry);
            SetSize();
            //base.OnRender(drawingContext);
        }

        //サイズをセット
        //Geometryからサイズを計算
        private void SetSize()
        {
            //Geometryをクローンしたものからサイズ計算しているけど、
            //今回は回転などの変形を使っていないので必要ない
            PathGeometry geo = MyPathGeometry.Clone();
            Rect rectGeo = geo.Bounds;
            Rect rectFlat = geo.GetFlattenedPathGeometry().Bounds;// 元のサイズと同じ
            Rect rectOutLine = geo.GetOutlinedPathGeometry().Bounds;// 元のサイズと同じ
            Rect rectWide = geo.GetWidenedPathGeometry(MyPen).Bounds;// 線の太さを加味したサイズ

            if (!rectWide.IsEmpty)
            {
                Width = rectWide.Width;
                Height = rectWide.Height;
                //Canvas.SetLeft(this, -rectWide.Left);
                //Canvas.SetTop(this, -rectWide.Top);
            }
            else if (!rectGeo.IsEmpty)
            {
                Width = rectGeo.Width;
                Height = rectGeo.Height;
            }

        }
    }



    #region コンバーター


    /// <summary>
    /// PointCollectionの最初の要素を返す、PathFigureのStartPoint用
    /// </summary>
    public class MyConverterStratPoint : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PointCollection pc && pc.Count > 0)
            {
                return pc[0];
            }
            return new Point();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// PointCollectionをクローンして、その最初の要素を除いて返す
    /// PolyLineSegmentのPoints用
    /// </summary>
    public class MyConverterSegment : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PointCollection pc)
            {
                if (pc.Count >= 2)
                {
                    PointCollection cpc = pc.Clone();
                    cpc.RemoveAt(0);
                    return cpc;
                }
                else
                {
                    return pc;
                }
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

    /// <summary>
    /// ブラシと太さからPenを作って返す
    /// </summary>
    public class MyCovnerterPen : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Brush b = (Brush)values[0];
            double thickness = (double)values[1];
            return new Pen(b, thickness);
        }

        //こっちは未使用になるはず
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
            //Pen pen = (Pen)value;
            //return [pen.Brush, pen.Thickness];
        }
    }

    #endregion コンバーター
}

