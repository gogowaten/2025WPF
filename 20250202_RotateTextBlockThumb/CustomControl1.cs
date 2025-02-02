using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace _20250202_RotateTextBlockThumb
{


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
                kiso.MyX += e.HorizontalChange;
                kiso.MyY += e.VerticalChange;
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


        #region 読み取り専用

        private static readonly DependencyPropertyKey MyBaseCanvasPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyBaseCanvas), typeof(Canvas), typeof(KisoThumb), new PropertyMetadata(null));
        public static readonly DependencyProperty MyBaseCanvasProperty = MyBaseCanvasPropertyKey.DependencyProperty;
        public Canvas MyBaseCanvas
        {
            get { return (Canvas)GetValue(MyBaseCanvasPropertyKey.DependencyProperty); }
            internal set { SetValue(MyBaseCanvasPropertyKey, value); }
        }
        #endregion 読み取り専用

        #region 通常

        //表示するコンテンツを入れておく用、読み取り専用にしたほうがいいかも
        public TextBlock MyContent
        {
            get { return (TextBlock)GetValue(MyContentProperty); }
            set { SetValue(MyContentProperty, value); }
        }
        public static readonly DependencyProperty MyContentProperty =
            DependencyProperty.Register(nameof(MyContent), typeof(TextBlock), typeof(TextBlockThumb), new PropertyMetadata(null));

        //自身のX座標
        public double MyX
        {
            get { return (double)GetValue(MyXProperty); }
            set { SetValue(MyXProperty, value); }
        }
        public static readonly DependencyProperty MyXProperty =
            DependencyProperty.Register(nameof(MyX), typeof(double), typeof(KisoThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MyY
        {
            get { return (double)GetValue(MyYProperty); }
            set { SetValue(MyYProperty, value); }
        }
        public static readonly DependencyProperty MyYProperty =
            DependencyProperty.Register(nameof(MyY), typeof(double), typeof(KisoThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

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



        //変形前コンテンツのサイズRect
        public Rect MyBounds
        {
            get { return (Rect)GetValue(MyBoundsProperty); }
            set { SetValue(MyBoundsProperty, value); }
        }
        public static readonly DependencyProperty MyBoundsProperty =
            DependencyProperty.Register(nameof(MyBounds), typeof(Rect), typeof(KisoThumb), new PropertyMetadata(null));

        //変形後のコンテンツのサイズRect
        public Rect MyTransformedBounds
        {
            get { return (Rect)GetValue(MyTransformedBoundsProperty); }
            set { SetValue(MyTransformedBoundsProperty, value); }
        }
        public static readonly DependencyProperty MyTransformedBoundsProperty =
            DependencyProperty.Register(nameof(MyTransformedBounds), typeof(Rect), typeof(KisoThumb), new PropertyMetadata(null));

        //自身のオフセット位置X
        public double MyOffsetX
        {
            get { return (double)GetValue(MyOffsetXProperty); }
            set { SetValue(MyOffsetXProperty, value); }
        }
        public static readonly DependencyProperty MyOffsetXProperty =
            DependencyProperty.Register(nameof(MyOffsetX), typeof(double), typeof(KisoThumb), new PropertyMetadata(0.0));

        public double MyOffsetY
        {
            get { return (double)GetValue(MyOffsetYProperty); }
            set { SetValue(MyOffsetYProperty, value); }
        }
        public static readonly DependencyProperty MyOffsetYProperty =
            DependencyProperty.Register(nameof(MyOffsetY), typeof(double), typeof(KisoThumb), new PropertyMetadata(0.0));

        #endregion 通常

        #region 図形関連

        //線の描画を考慮したBoundsを使ってオフセット表示をする
        public bool MyIsOffset
        {
            get { return (bool)GetValue(MyIsOffsetProperty); }
            set { SetValue(MyIsOffsetProperty, value); }
        }
        public static readonly DependencyProperty MyIsOffsetProperty =
            DependencyProperty.Register(nameof(MyIsOffset), typeof(bool), typeof(KisoThumb), new FrameworkPropertyMetadata(false,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));

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
                FrameworkPropertyMetadataOptions.AffectsRender |// デザイン画面上での更新で必要
                FrameworkPropertyMetadataOptions.AffectsMeasure)); // 必要ないかも？

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


        //読み取り専用にしたい
        public Pen MyPen
        {
            get { return (Pen)GetValue(MyPenProperty); }
            set { SetValue(MyPenProperty, value); }
        }
        public static readonly DependencyProperty MyPenProperty =
            DependencyProperty.Register(nameof(MyPen), typeof(Pen), typeof(KisoThumb), new FrameworkPropertyMetadata(new Pen(),
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        //private static readonly DependencyPropertyKey MyPenPropertyKey =
        //    DependencyProperty.RegisterReadOnly(nameof(MyPen), typeof(Pen), typeof(KisoThumb), new PropertyMetadata(new Pen()));
        //public static readonly DependencyProperty MyPenProperty = MyPenPropertyKey.DependencyProperty;
        //public Pen MyPen
        //{
        //    get { return (Pen)GetValue(MyPenPropertyKey.DependencyProperty); }
        //    internal set { SetValue(MyPenPropertyKey, value); }
        //}



        #endregion 図形関連

        #endregion 依存関係プロパティ



    }


    public class TextBlockThumb : KisoThumb
    {

        static TextBlockThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBlockThumb), new FrameworkPropertyMetadata(typeof(TextBlockThumb)));
        }
        public TextBlockThumb()
        {
            Loaded += TextBlockThumb_Loaded;
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("text") is TextBlock element)
            {
                MyContent = element;
            }
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            var neko = MyTransformedBounds.Left;
        }
        private void TextBlockThumb_Loaded(object sender, RoutedEventArgs e)
        {

            //コンテンツのサイズを取得
            MyBindContentRect();
            //変形後のコンテンツのサイズを取得
            MyBindCotentTransfomedRect2();
            //MyBindCotentTransfomedRect();
            //自身のサイズを変形後のコンテンツに合わせる
            MyBindFitContentSize();
            //オフセットの計算
            MyBindOffset();

            //コンテンツのオフセット位置
            MyBindContentOffset();
        }

        //コンテンツのサイズを取得
        private void MyBindContentRect()
        {
            MultiBinding mb = new() { Converter = new MyConverterRenderSizeRect() };
            Binding aw = new()
            {
                Source = MyContent,
                Path = new PropertyPath(ActualWidthProperty),
                Mode = BindingMode.OneWay
            };
            Binding ah = new()
            {
                Source = MyContent,
                Path = new PropertyPath(ActualHeightProperty),
                Mode = BindingMode.OneWay
            };
            mb.Bindings.Add(aw);
            mb.Bindings.Add(ah);
            SetBinding(MyBoundsProperty, mb);

        }

        //変形後のコンテンツのサイズを取得
        private void MyBindCotentTransfomedRect2()
        {
            MultiBinding mb = new() { Converter = new MyConverterRenderSizeTransformedRect() };
            mb.Bindings.Add(new Binding()
            {
                Source = this,
                Path = new PropertyPath(MyBoundsProperty),
                Mode = BindingMode.OneWay
            });
            mb.Bindings.Add(new Binding()
            {
                Source = MyContent,
                Path = new PropertyPath(RenderTransformProperty),
                Mode = BindingMode.OneWay
            });
            SetBinding(MyTransformedBoundsProperty, mb);
        }

        //自身のサイズを変形後のコンテンツに合わせる
        private void MyBindFitContentSize()
        {
            SetBinding(WidthProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(MyTransformedBoundsProperty),
                Mode = BindingMode.OneWay,
                Converter = new MyConverterRectWidth()
            });
            SetBinding(HeightProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(MyTransformedBoundsProperty),
                Mode = BindingMode.OneWay,
                Converter = new MyConverterRectHeight()
            });
        }

        //オフセットの計算
        private void MyBindOffset()
        {
            MultiBinding mb = new() { Converter = new MyConverterOffsetX() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyXProperty), Mode = BindingMode.OneWay });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyTransformedBoundsProperty), Mode = BindingMode.OneWay });
            SetBinding(MyOffsetXProperty, mb);
            mb = new() { Converter = new MyConverterOffsetY() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyYProperty), Mode = BindingMode.OneWay });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyTransformedBoundsProperty), Mode = BindingMode.OneWay });
            SetBinding(MyOffsetYProperty, mb);

        }

        //コンテンツのオフセット位置
        private void MyBindContentOffset()
        {
            MyContent.SetBinding(Canvas.LeftProperty, new Binding() { Source = this, Path = new PropertyPath(MyTransformedBoundsProperty), Mode = BindingMode.OneWay, Converter = new MyConverterContentOffsetX() });
            MyContent.SetBinding(Canvas.TopProperty, new Binding() { Source = this, Path = new PropertyPath(MyTransformedBoundsProperty), Mode = BindingMode.OneWay, Converter = new MyConverterContentOffsetY() });

        }

    }


    #region コンバーター

    //コンテンツのオフセット位置Y
    public class MyConverterContentOffsetY : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (Rect)value;
            return -r.Top;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //コンテンツのオフセット位置X
    public class MyConverterContentOffsetX : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (Rect)value;
            return -r.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //Geometry系の図形用、RotateTransformのCenterの値を計算
    public class MyConverterCenterPoint : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var pc = (PointCollection)value;
            if (pc is null) { return new Point(); }
            double x = 0, y = 0;
            foreach (var item in pc)
            {
                x += item.X;
                y += item.Y;
            }
            return new Point(x / pc.Count, y / pc.Count);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

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

    //変形後のコンテンツのRectから自身の高さに変換
    public class MyConverterRectHeight : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (Rect)value;
            return r.Height;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //変形後のコンテンツのRectから自身の高さに変換
    public class MyConverterRectWidth : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (Rect)value;
            return r.Width;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //変形後のコンテンツのRectを計算
    //コンテンツのサイズの半分の値をRotateTransformのCenterの値にして
    //TransformBoundsメソッドにコンテンツのサイズを入れて計算
    public class MyConverterRenderSizeTransformedRect : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (Rect)values[0];
            if (values[1] is RotateTransform tf)
            {
                tf.CenterX = r.Width / 2.0;
                tf.CenterY = r.Height / 2.0;
                return tf.TransformBounds(r);
            }
            else { return new Rect(); }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //変形”前”のコンテンツのRectを計算
    public class MyConverterRenderSizeRect : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var w = (double)values[0];
            var h = (double)values[1];
            return new Rect(0, 0, w, h);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion コンバーター

}
