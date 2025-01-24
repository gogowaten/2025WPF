using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace _20250124
{
    internal class Class1
    {
    }
    public class ExLine : FrameworkElement
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
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(ExLine),
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
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(ExLine), new FrameworkPropertyMetadata(1.0,
                FrameworkPropertyMetadataOptions.AffectsRender |// デザイン画面上での更新で必要
                FrameworkPropertyMetadataOptions.AffectsMeasure)); // 必要ないかも？

        public Brush MyStroke
        {
            get { return (Brush)GetValue(MyStrokeProperty); }
            set { SetValue(MyStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeProperty =
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(ExLine), new FrameworkPropertyMetadata(Brushes.Magenta,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        public Brush MyFill
        {
            get { return (Brush)GetValue(MyFillProperty); }
            set { SetValue(MyFillProperty, value); }
        }
        public static readonly DependencyProperty MyFillProperty =
            DependencyProperty.Register(nameof(MyFill), typeof(Brush), typeof(ExLine), new FrameworkPropertyMetadata(Brushes.Pink,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        public bool MyIsFilled
        {
            get { return (bool)GetValue(MyIsFilledProperty); }
            set { SetValue(MyIsFilledProperty, value); }
        }
        public static readonly DependencyProperty MyIsFilledProperty =
            DependencyProperty.Register(nameof(MyIsFilled), typeof(bool), typeof(ExLine), new FrameworkPropertyMetadata(true,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        public bool MyIsStroked
        {
            get { return (bool)GetValue(MyIsStrokedProperty); }
            set { SetValue(MyIsStrokedProperty, value); }
        }
        public static readonly DependencyProperty MyIsStrokedProperty =
            DependencyProperty.Register(nameof(MyIsStroked), typeof(bool), typeof(ExLine), new FrameworkPropertyMetadata(true,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        public bool MyIsClosed
        {
            get { return (bool)GetValue(MyIsClosedProperty); }
            set { SetValue(MyIsClosedProperty, value); }
        }
        public static readonly DependencyProperty MyIsClosedProperty =
            DependencyProperty.Register(nameof(MyIsClosed), typeof(bool), typeof(ExLine), new FrameworkPropertyMetadata(false,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        //読み取り専用のPen、なぜかバインドできない、エラーになる
        //private static readonly DependencyPropertyKey MyPenPropertyKey =
        //    DependencyProperty.RegisterReadOnly(nameof(MyPen), typeof(Pen), typeof(ExLine), new PropertyMetadata(null));
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
            DependencyProperty.Register(nameof(MyPen), typeof(Pen), typeof(ExLine), new PropertyMetadata(null));


        #endregion 依存関係プロパティ

        #region 読み取り専用依存関係プロパティ

        private static readonly DependencyPropertyKey MyPathGeometryPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyPathGeometry), typeof(PathGeometry), typeof(ExLine), new PropertyMetadata(null));
        public static readonly DependencyProperty MyPathGeometryProperty = MyPathGeometryPropertyKey.DependencyProperty;
        public PathGeometry MyPathGeometry
        {
            get { return (PathGeometry)GetValue(MyPathGeometryPropertyKey.DependencyProperty); }
            internal set { SetValue(MyPathGeometryPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey MyPathFigurePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyPathFigure), typeof(PathFigure), typeof(ExLine), new PropertyMetadata(null));
        public static readonly DependencyProperty MyPathFigureProperty = MyPathFigurePropertyKey.DependencyProperty;
        public PathFigure MyPathFigure
        {
            get { return (PathFigure)GetValue(MyPathFigurePropertyKey.DependencyProperty); }
            internal set { SetValue(MyPathFigurePropertyKey, value); }
        }


        private static readonly DependencyPropertyKey MyPolyLineSegmentPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyPolyLineSegment), typeof(PolyLineSegment), typeof(ExLine), new PropertyMetadata(null));
        public static readonly DependencyProperty MyPolyLineSegmentProperty = MyPolyLineSegmentPropertyKey.DependencyProperty;
        public PolyLineSegment MyPolyLineSegment
        {
            get { return (PolyLineSegment)GetValue(MyPolyLineSegmentPropertyKey.DependencyProperty); }
            internal set { SetValue(MyPolyLineSegmentPropertyKey, value); }
        }

        #endregion 読み取り専用依存関係プロパティ



        public ExLine()
        {
            DataContext = this;
            MyPathGeometry = new PathGeometry() { FillRule = FillRule.Nonzero };
            MyPathFigure = new PathFigure();

            Initialized += ExLine_Initialized;
        }


        private void ExLine_Initialized(object? sender, EventArgs e)
        {
            MyPolyLineSegment = new PolyLineSegment();

            MyPathFigure.Segments.Add(MyPolyLineSegment);
            if (MyPoints is null || MyPoints.Count == 0)
            {
                MyPathFigure.StartPoint = new Point();
            }
            else
            {
                MyPathFigure.StartPoint = MyPoints[0];
            }

            MyPathGeometry.Figures.Add(MyPathFigure);
            MySetBindings();


        }
        private void MySetBindings()
        {
            _ = BindingOperations.SetBinding(MyPolyLineSegment, PathSegment.IsStrokedProperty, new Binding() { Source = this, Path = new PropertyPath(MyIsStrokedProperty), Mode = BindingMode.TwoWay });
            _ = BindingOperations.SetBinding(MyPolyLineSegment, PolyLineSegment.PointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty), Converter = new MyConverterSegment() });

            _ = BindingOperations.SetBinding(MyPathFigure, PathFigure.IsClosedProperty, new Binding() { Source = this, Path = new PropertyPath(MyIsClosedProperty), Mode = BindingMode.TwoWay });
            _ = BindingOperations.SetBinding(MyPathFigure, PathFigure.IsFilledProperty, new Binding() { Source = this, Path = new PropertyPath(MyIsFilledProperty), Mode = BindingMode.TwoWay });
            _ = BindingOperations.SetBinding(MyPathFigure, PathFigure.StartPointProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty), Converter = new MyConverterStratPoint() });

            Binding b1 = new() { Source = this, Path = new PropertyPath(MyStrokeProperty) };
            Binding b2 = new() { Source = this, Path = new PropertyPath(MyStrokeThicknessProperty) };
            MultiBinding mb = new() { Converter = new MyCovnerterPen() };
            mb.Bindings.Add(b1);
            mb.Bindings.Add(b2);
            _ = BindingOperations.SetBinding(this, MyPenProperty, mb);

        }

        protected override void OnRender(DrawingContext drawingContext)
        {

            drawingContext.DrawGeometry(MyFill, MyPen, MyPathGeometry);


            //Bindingを使わないで描画
            //PolyLineSegment seg = new(MyPoints.Skip(1), MyIsStroked);
            //PathFigure fig = new() { IsClosed = MyIsClosed, IsFilled = MyIsFilled };
            //fig.Segments.Add(seg);
            //fig.StartPoint = MyPoints[0];
            //PathGeometry geo = new();
            //geo.Figures.Add(fig);
            ////geo.FillRule = FillRule.EvenOdd;
            //geo.FillRule = FillRule.Nonzero;

            ////drawingContext.DrawGeometry(MyStroke, null, geo);
            ////drawingContext.DrawGeometry(MyStroke, new Pen(Brushes.Red, MyStrokeThickness), geo);
            ////drawingContext.DrawGeometry(Brushes.Yellow,new Pen(Brushes.Green,5), geo);
            //drawingContext.DrawGeometry(MyFill,new Pen(Brushes.Green,5), geo);




            //base.OnRender(drawingContext);
        }

    }


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
            //throw new NotImplementedException();
            Pen pen = (Pen)value;
            return [pen.Brush, pen.Thickness];
        }
    }


}
