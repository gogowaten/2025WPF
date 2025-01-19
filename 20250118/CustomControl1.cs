using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250118
{
    public class CanvasThumb2 : Thumb
    {
        private AnchorThumb? MyThumb;
        private Canvas? MyCanvas;
        static CanvasThumb2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CanvasThumb2), new FrameworkPropertyMetadata(typeof(CanvasThumb)));
        }
        public CanvasThumb2()
        {

            DragDelta += Thumb_DragDelta;
        }

        public void ResizeOn()
        {
            if (MyThumb != null)
            {
                MyCanvas?.Children.Remove(MyThumb);
                MyThumb = null;

            }

            MyThumb = new() { Width = 20, Height = 20, Background = Brushes.DarkMagenta, BackgroundOpacity = 0.5 };
            Canvas.SetLeft(MyThumb, 0);
            Canvas.SetTop(MyThumb, 0);
            MyThumb.Cursor = Cursors.SizeNWSE;
            MyThumb.SetBinding(Canvas.LeftProperty, new Binding() { Source = MyCanvas, Path = new PropertyPath(WidthProperty), Mode = BindingMode.TwoWay });
            MyThumb.SetBinding(Canvas.TopProperty, new Binding() { Source = MyCanvas, Path = new PropertyPath(HeightProperty), Mode = BindingMode.TwoWay });

            MyThumb.DragDelta += Thumb_DragDelta;
            MyCanvas?.Children.Add(MyThumb);

        }
        public void ResizeOff()
        {
            if (MyThumb != null)
            {
                MyCanvas?.Children.Remove(MyThumb);
                MyThumb = null;
            }
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                if (t == MyThumb)
                {
                    //最小サイズ未満にならないようにThumbの移動
                    var left = Canvas.GetLeft(t);
                    var top = Canvas.GetTop(t);
                    Canvas.SetLeft(t, Math.Max(1, Canvas.GetLeft(t) + e.HorizontalChange));
                    Canvas.SetTop(t, Math.Max(1, Canvas.GetTop(t) + e.VerticalChange));
                    e.Handled = true;
                }
                else if (t == this)
                {
                    //最小座標未満にならないように自身の移動
                    Canvas.SetLeft(t, Math.Max(0, Canvas.GetLeft(t) + e.HorizontalChange));
                    Canvas.SetTop(t, Math.Max(0, Canvas.GetTop(t) + e.VerticalChange));
                    e.Handled = true;
                }
            }
        }

        public override void OnApplyTemplate()
        {
            //Templateの中のCanvasを取得してMyThumbを追加とBinding処理
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_Canvas") is Canvas panel)
            {

                MyCanvas = panel;

                ////バインド
                ////自身のサイズをソースにMyThumbの座標をバインド
                //MyThumb.DataContext = this;
                //_ = MyThumb.SetBinding(Canvas.LeftProperty,
                //    new Binding(nameof(Width)) { Mode = BindingMode.TwoWay });
                //_ = MyThumb.SetBinding(Canvas.TopProperty,
                //    new Binding(nameof(Height)) { Mode = BindingMode.TwoWay });
            }
        }

        internal void AddElement(UIElement element)
        {
            _ = (MyCanvas?.Children.Add(element));
        }
        internal void RemoveElement(UIElement element)
        {
            MyCanvas?.Children.Remove(element);
        }
    }




    public class AnchorThumb : Thumb
    {
        static AnchorThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnchorThumb), new FrameworkPropertyMetadata(typeof(AnchorThumb)));
        }
        public AnchorThumb()
        {
            //DragDelta += AnchorThumb_DragDelta;
        }

        //private void AnchorThumb_DragDelta(object sender, DragDeltaEventArgs e)
        //{
        //    if(sender is AnchorThumb at)
        //    {
        //        Canvas.SetLeft(at, Canvas.GetLeft(at) + e.HorizontalChange);
        //        Canvas.SetTop(at, Canvas.GetTop(at) + e.VerticalChange);
        //        e.Handled=true;
        //    }
        //}

        #region 依存関係プロパティ

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register(nameof(Fill), typeof(Brush), typeof(AnchorThumb), new PropertyMetadata(Brushes.Transparent));

        public Brush StrokeBaseBrush
        {
            get { return (Brush)GetValue(StrokeBaseBrushProperty); }
            set { SetValue(StrokeBaseBrushProperty, value); }
        }
        public static readonly DependencyProperty StrokeBaseBrushProperty =
            DependencyProperty.Register(nameof(StrokeBaseBrush), typeof(Brush), typeof(AnchorThumb), new PropertyMetadata(Brushes.White));

        public Brush StrokeDashBrush
        {
            get { return (Brush)GetValue(StrokeDashBrushProperty); }
            set { SetValue(StrokeDashBrushProperty, value); }
        }
        public static readonly DependencyProperty StrokeDashBrushProperty =
            DependencyProperty.Register(nameof(StrokeDashBrush), typeof(Brush), typeof(AnchorThumb), new PropertyMetadata(Brushes.Black));


        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }
        public static readonly DependencyProperty StrokeDashArrayProperty =
            DependencyProperty.Register(nameof(StrokeDashArray), typeof(DoubleCollection), typeof(AnchorThumb), new PropertyMetadata(new DoubleCollection() { 4 }));


        public double BackgroundOpacity
        {
            get { return (double)GetValue(BackgroundOpacityProperty); }
            set { SetValue(BackgroundOpacityProperty, value); }
        }
        public static readonly DependencyProperty BackgroundOpacityProperty =
            DependencyProperty.Register(nameof(BackgroundOpacity), typeof(double), typeof(AnchorThumb), new PropertyMetadata(1.0));

        #endregion 依存関係プロパティ
    }


    public class CanvasThumb : Thumb
    {
        private readonly Thumb MyThumb;
        private const double MinimumSize = 1;
        private const double MinimumLocate = 0;
        private const double ThumbSize = 20;
        private Canvas? MyCanvas;
        static CanvasThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CanvasThumb), new FrameworkPropertyMetadata(typeof(CanvasThumb)));
        }
        public CanvasThumb()
        {
            MyThumb = new()
            {
                Width = ThumbSize,
                Height = ThumbSize,
                Cursor = Cursors.SizeNWSE
            };
            MyThumb.DragDelta += Thumb_DragDelta;
            DragDelta += Thumb_DragDelta;
            SetInitialPosition();
        }
        private void SetInitialPosition()
        {
            Canvas.SetLeft(MyThumb, MinimumLocate);
            Canvas.SetTop(MyThumb, MinimumLocate);
            Canvas.SetLeft(this, MinimumLocate);
            Canvas.SetTop(this, MinimumLocate);
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                if (t == MyThumb)
                {
                    //最小サイズ未満にならないようにThumbの移動
                    var left = Canvas.GetLeft(t);
                    var top = Canvas.GetTop(t);
                    Canvas.SetLeft(t, Math.Max(MinimumSize, Canvas.GetLeft(t) + e.HorizontalChange));
                    Canvas.SetTop(t, Math.Max(MinimumSize, Canvas.GetTop(t) + e.VerticalChange));
                    e.Handled = true;
                }
                else if (t == this)
                {
                    //最小座標未満にならないように自身の移動
                    Canvas.SetLeft(t, Math.Max(MinimumLocate, Canvas.GetLeft(t) + e.HorizontalChange));
                    Canvas.SetTop(t, Math.Max(MinimumLocate, Canvas.GetTop(t) + e.VerticalChange));
                    e.Handled = true;
                }
            }
        }

        public override void OnApplyTemplate()
        {
            //Templateの中のCanvasを取得してMyThumbを追加とBinding処理
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_Canvas") is Canvas panel)
            {
                panel.Children.Add(MyThumb);
                MyCanvas = panel;

                //バインド
                //自身のサイズをソースにMyThumbの座標をバインド
                MyThumb.DataContext = this;
                _ = MyThumb.SetBinding(Canvas.LeftProperty,
                    new Binding(nameof(Width)) { Mode = BindingMode.TwoWay });
                _ = MyThumb.SetBinding(Canvas.TopProperty,
                    new Binding(nameof(Height)) { Mode = BindingMode.TwoWay });
            }
        }

        internal void AddElement(UIElement element)
        {
            _ = (MyCanvas?.Children.Add(element));
        }
        internal void RemoveElement(UIElement element)
        {
            MyCanvas?.Children.Remove(element);
        }
    }


    [ContentProperty(nameof(MyPoints))]
    public class PolylineThumb : CanvasThumb
    {
        public readonly List<AnchorThumb> MyThumbs = [];
        public Polyline? MyPolyLine;
        public List<AnchorThumb> MyAnchors = [];

        #region 依存関係プロパティ

        //public PointCollection MyPoints
        //{
        //    get { return (PointCollection)GetValue(MyPointsProperty); }
        //    set { SetValue(MyPointsProperty, value); }
        //}
        //public static readonly DependencyProperty MyPointsProperty =
        //    DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(PolylineThumb), new FrameworkPropertyMetadata(null,new PropertyChangedCallback(OnMyPointsChanged)));



        //private static void OnMyPointsChanged(DependencyObject d,DependencyPropertyChangedEventArgs e)
        //{
        //    if(d is PolylineThumb poly)
        //    {
        //        if(e.NewValue is Point p)
        //        {
        //            AnchorThumb anchor = new();
        //            Canvas.SetLeft(anchor, p.X);
        //            Canvas.SetTop(anchor, p.Y);
        //            poly.MyThumbs.Add(anchor);
        //        }
        //    }
        //}

        //public PointCollection MyPoints
        //{
        //    get { return (PointCollection)GetValue(MyPointsProperty); }
        //    set { SetValue(MyPointsProperty, value); }
        //}
        //public static readonly DependencyProperty MyPointsProperty =
        //    DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(PolylineThumb), new PropertyMetadata(null));


        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(PolylineThumb),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //public ObservableCollection<Point> MyPoints
        //{
        //    get { return (ObservableCollection<Point>)GetValue(MyPointsProperty); }
        //    set { SetValue(MyPointsProperty, value); }
        //}
        //public static readonly DependencyProperty MyPointsProperty =
        //    DependencyProperty.Register(nameof(MyPoints), typeof(ObservableCollection<Point>), typeof(PolylineThumb),
        //        new FrameworkPropertyMetadata(null,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(PolylineThumb), new PropertyMetadata(Brushes.DodgerBlue));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(PolylineThumb), new PropertyMetadata(1.0));

        #endregion 依存関係プロパティ


        static PolylineThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PolylineThumb), new FrameworkPropertyMetadata(typeof(PolylineThumb)));
        }
        public PolylineThumb()
        {

            Loaded += PolylineThumb_Loaded;
        }

        private void PolylineThumb_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public void Test()
        {
            var neko = MyPolyLine;
            var pt = MyThumbs;

        }

        public void AnchorsOn()
        {
            for (int i = 0; i < MyPoints.Count; i++)
            {
                AnchorThumb at = new() { Width = 20, Height = 20, Tag = i };
                at.DragDelta += At_DragDelta;

                Canvas.SetLeft(at, MyPoints[i].X);
                Canvas.SetTop(at, MyPoints[i].Y);
                //MyAnchors.Add(at);
                AddElement(at);


            }
        }

        private void At_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is AnchorThumb at)
            {
                //MyPoints[0] = new Point(111, 222);
                Canvas.SetLeft(at, Canvas.GetLeft(at) + e.HorizontalChange);
                Canvas.SetTop(at, Canvas.GetTop(at) + e.VerticalChange);
                int pointIndex = (int)at.Tag;
                Point motoPoint = MyPoints[pointIndex];
                Point newPoint = new(motoPoint.X + e.HorizontalChange, motoPoint.Y + e.VerticalChange);
                MyPoints[pointIndex] = newPoint;

                e.Handled = true;
            }
        }

        public void AnchorsOff()
        {
            MyAnchors.Clear();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("myPolyline") is Polyline poly)
            {
                MyPolyLine = poly;
                SetBinding(WidthProperty, new Binding() { Source = poly, Path = new PropertyPath(ActualWidthProperty) });
                SetBinding(HeightProperty, new Binding() { Source = poly, Path = new PropertyPath(ActualHeightProperty) });

            }
        }
    }


    [ContentProperty(nameof(MyPoints))]
    public class LineThumb : CanvasThumb
    {

        #region 依存関係プロパティ

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(LineThumb), new PropertyMetadata(null));




        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(LineThumb), new PropertyMetadata(Brushes.DodgerBlue));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(LineThumb), new PropertyMetadata(1.0));

        #endregion 依存関係プロパティ

        public List<AnchorThumb> MyAnchors { get; set; }
        static LineThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LineThumb), new FrameworkPropertyMetadata(typeof(LineThumb)));
        }

        MyLineShape? MyLineShape = null;
        public LineThumb()
        {
            MyAnchors = [];
            Loaded += LineThumb_Loaded;

        }

        private void LineThumb_Loaded(object sender, RoutedEventArgs e)
        {
            SetAnchors();
        }

        private void SetAnchors()
        {
            MyAnchors.Clear();
            for (int i = 0; i < MyPoints.Count; i++)
            {
                AnchorThumb t = new() { Width = 20, Height = 20 };
                Canvas.SetLeft(t, MyPoints[i].X);
                Canvas.SetTop(t, MyPoints[i].Y);
                MyAnchors.Add(t);
                AddElement(t);
                t.DragDelta += Thumb_DragDelta;
            }
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is AnchorThumb t)
            {
                double left = Canvas.GetLeft(t) + e.HorizontalChange;
                double top = Canvas.GetTop(t) + e.VerticalChange;
                Canvas.SetLeft(t, left);
                Canvas.SetTop(t, top);
                int index = MyAnchors.IndexOf(t);
                MyPoints[index] = new Point(left, top);
                e.Handled = true;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_line") is MyLineShape shape)
            {
                MyLineShape = shape;
                MyLineShape.MyObPoints = MyPoints;
            }
        }

    }



    public class PathLineThumb : CanvasThumb
    {

        #region 依存関係プロパティ

        //public PointCollection MyPoints
        //{
        //    get { return (PointCollection)GetValue(MyPointsProperty); }
        //    set { SetValue(MyPointsProperty, value); }
        //}
        //public static readonly DependencyProperty MyPointsProperty =
        //    DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(PathLineThumb), new PropertyMetadata(null));



        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(PathLineThumb),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        //[TypeConverter(typeof(MyTypeConverterStringToPoints))]
        //public ObservableCollection<Point> MyPoints
        //{
        //    get { return (ObservableCollection<Point>)GetValue(MyPointsProperty); }
        //    set { SetValue(MyPointsProperty, value); }
        //}
        //public static readonly DependencyProperty MyPointsProperty =
        //    DependencyProperty.Register(nameof(MyPoints), typeof(ObservableCollection<Point>), typeof(PathLineThumb),
        //        new FrameworkPropertyMetadata(null,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(PathLineThumb), new PropertyMetadata(Brushes.DodgerBlue));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(PathLineThumb), new PropertyMetadata(1.0));

        #endregion 依存関係プロパティ

        //MyLineShape? MyLineShape = null;
        public GeoLine? MyGeoLine { get; set; }
        public List<AnchorThumb> MyAnchors { get; set; }

        static PathLineThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PathLineThumb), new FrameworkPropertyMetadata(typeof(PathLineThumb)));
        }

        public PathLineThumb()
        {
            MyAnchors = [];
            Loaded += PathLineThumb_Loaded;

        }

        private void PathLineThumb_Loaded(object sender, RoutedEventArgs e)
        {
            if (GetTemplateChild("PART_line") is GeoLine line)
            {
                MyGeoLine = line;

                //MyPointのBindingを解除、これでXAMLでのPoint変更は無効になるけど
                //コードでの変更が反映されるようになる
                MyGeoLine.MyPoints = MyPoints;
            }
        }

        public void AnchorOff()
        {
            foreach (var item in MyAnchors)
            {
                RemoveElement(item);
            }
            MyAnchors.Clear();
        }

        public void AnchorOn()
        {
            //MyAnchors.Clear(); 
            for (int i = 0; i < MyPoints.Count; i++)
            {
                AnchorThumb thumb = new() { Width = 20, Height = 20 };
                Canvas.SetLeft(thumb, MyPoints[i].X);
                Canvas.SetTop(thumb, MyPoints[i].Y);
                MyAnchors.Add(thumb);
                AddElement(thumb);
                thumb.DragDelta += Thumb_DragDelta;
            }
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is AnchorThumb thumb)
            {
                int index = MyAnchors.IndexOf(thumb);
                SetPointAndAnchor(thumb, index, e);
                e.Handled = true;
            }
        }
        private void SetPointAndAnchor(FrameworkElement element, int index, DragDeltaEventArgs e)
        {
            Point point = MyPoints[index];
            double x = point.X + e.HorizontalChange;
            double y = point.Y + e.VerticalChange;
            Canvas.SetLeft(element, x);
            Canvas.SetTop(element, y);
            MyPoints[index] = new Point(x, y);
        }


    }




    public class MyConverterObToPo : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<Point> points = (ObservableCollection<Point>)value;
            PointCollection points2 = new PointCollection(points);
            return points2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
