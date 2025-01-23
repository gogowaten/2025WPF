using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;

namespace _20250123
{
    class Class1
    {
    }
    public class ExLine : Shape
    {
        #region 依存関係プロパティ

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(ExLine),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | // デザイン画面上での更新で必須
                    FrameworkPropertyMetadataOptions.AffectsRender | // 必要ないかも
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)); // 必要ないかも



        public Rect MyBounds
        {
            get { return (Rect)GetValue(MyBoundsProperty); }
            set { SetValue(MyBoundsProperty, value); }
        }
        public static readonly DependencyProperty MyBoundsProperty =
            DependencyProperty.Register(nameof(MyBounds), typeof(Rect), typeof(ExLine), new PropertyMetadata(Rect.Empty));


        public Pen MyPen
        {
            get { return (Pen)GetValue(MyPenProperty); }
            set { SetValue(MyPenProperty, value); }
        }
        public static readonly DependencyProperty MyPenProperty =
            DependencyProperty.Register(nameof(MyPen), typeof(Pen), typeof(ExLine), new PropertyMetadata(null));


        public double MyAngle
        {
            get { return (double)GetValue(MyAngleProperty); }
            set { SetValue(MyAngleProperty, value); }
        }
        public static readonly DependencyProperty MyAngleProperty =
            DependencyProperty.Register(nameof(MyAngle), typeof(double), typeof(ExLine), new PropertyMetadata(0.0, new PropertyChangedCallback(OnMyAngleChanged)));
        private static void OnMyAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ExLine line)
            {
                double nAngle = (double)e.NewValue;
                RotateTransform rt = new(nAngle);
                Geometry geoClone = line.DefiningGeometry.Clone();
                
                ////冗長かもしれないけど、これがないと微妙にずれる
                geoClone.Transform = rt;
                line.MyBounds = geoClone.GetWidenedPathGeometry(line.MyPen).Bounds;
                Canvas.SetLeft(line, -line.MyBounds.Left);
                Canvas.SetTop(line, -line.MyBounds.Top);

            }
        }

        public RotateTransform MyRotate
        {
            get { return (RotateTransform)GetValue(MyRotateProperty); }
            set { SetValue(MyRotateProperty, value); }
        }
        public static readonly DependencyProperty MyRotateProperty =
            DependencyProperty.Register(nameof(MyRotate), typeof(RotateTransform), typeof(ExLine), new PropertyMetadata(new RotateTransform(0)));


        #endregion 依存関係プロパティ
        public ExLine()
        {

            Binding b1 = new() { Source = this, Path = new PropertyPath(StrokeProperty) };
            Binding b2 = new() { Source = this, Path = new PropertyPath(StrokeThicknessProperty) };
            MultiBinding mb = new();
            mb.Bindings.Add(b1);
            mb.Bindings.Add(b2);
            mb.Converter = new MyConverterPen();
            _ = SetBinding(MyPenProperty, mb);

            _ = SetBinding(RenderTransformProperty, new Binding() { Source = this, Path = new PropertyPath(MyAngleProperty), Converter = new MyConverterRotate(),Mode=BindingMode.TwoWay });
            
        }

        

        protected override Geometry DefiningGeometry
        {
            get
            {
                if (MyPoints is null) { return Geometry.Empty; }
                if (MyPoints.Count == 0) { return Geometry.Empty; }

                StreamGeometry geo = new();

                using (var context = geo.Open())
                {
                    DrawLine(context, MyPoints[0], MyPoints[^1]);
                }
                geo.Freeze();

                //MyBounds = VisualTreeHelper.GetDescendantBounds(this);//違う
                //MyBounds = geo.GetWidenedPathGeometry(MyPen).Bounds;//回転とか変形無しならこれでいい
                
                var geoClone = geo.Clone();
                //var geoClone = geo.Clone();
                //geoClone.Transform = RenderTransform;
                //geoClone.Transform = MyRotate;
                geoClone.Transform = RenderTransform;

                PathGeometry pg = geoClone.GetWidenedPathGeometry(MyPen);
                MyBounds = pg.Bounds;

                Canvas.SetLeft(this, -MyBounds.Left);
                Canvas.SetTop(this, -MyBounds.Top);
                //Width = MyBounds.Width;
                //Height = MyBounds.Height;
                return geo;
            }
        }

        //回転とか無しならこれでいい
        //protected override Geometry DefiningGeometry
        //{
        //    get
        //    {
        //        if (MyPoints is null) { return Geometry.Empty; }
        //        if (MyPoints.Count == 0) { return Geometry.Empty; }

        //        StreamGeometry geo = new();

        //        using (var context = geo.Open())
        //        {
        //            DrawLine(context, MyPoints[0], MyPoints[^1]);
        //        }
        //        geo.Freeze();
        //        //MyBounds = VisualTreeHelper.GetDescendantBounds(this);//違う
        //        MyBounds = geo.GetWidenedPathGeometry(MyPen).Bounds;//回転とか変形無しならこれでいい

        //        Canvas.SetLeft(this, -MyBounds.Left);
        //        Canvas.SetTop(this, -MyBounds.Top);
        //        //Width = MyBounds.Width;
        //        //Height = MyBounds.Height;
        //        return geo;
        //    }
        //}




        /// <summary>
        /// 直線の描画
        /// </summary>
        /// <param name="context"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        private void DrawLine(StreamGeometryContext context, Point begin, Point end)
        {
            context.BeginFigure(begin, false, false);
            for (int i = 1; i < MyPoints.Count - 1; i++)
            {
                context.LineTo(MyPoints[i], true, false);
            }
            context.LineTo(end, true, false);
        }


    }

    public class MyConverterPen : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Brush b = (Brush)values[0];
            double thickness = (double)values[1];
            return new Pen(b, thickness);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConverterRotate : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double angle = (double)value;
            return new RotateTransform(angle);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
