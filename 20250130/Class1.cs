using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace _20250130
{
    internal class Class1
    {
    }
    public class EzLine : Shape
    {
        public EzLine()
        {

            MultiBinding mb = new() { Converter = new MyConverterRotateTF() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyAngleProperty), Mode = BindingMode.OneWay });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyRectProperty), Mode = BindingMode.OneWay });
            SetBinding(RenderTransformProperty, mb);

        }
        protected override Geometry DefiningGeometry
        {
            get
            {
                StreamGeometry geo = new();
                using (var context = geo.Open())
                {
                    context.BeginFigure(MyPoints[0], isFilled: true, isClosed: false);
                    PointCollection pc = new(MyPoints.Clone());
                    pc.RemoveAt(0);
                    context.PolyLineTo(pc, isStroked: true, isSmoothJoin: false);

                }
                geo.Freeze();

                return geo;
            }
        }



        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            MyRect = RenderedGeometry.Bounds;
            Pen pen = new(Stroke, StrokeThickness);
            //MyRectWithPen = RenderedGeometry.GetRenderBounds(pen);
            //var clone = RenderedGeometry.Clone();
            //clone.Transform = RenderTransform;
            //MyRectWithPenAndRoate = clone.GetRenderBounds(pen);
        }


        //public Point MyCenterPoint
        //{
        //    get { return (Point)GetValue(MyCenterPointProperty); }
        //    set { SetValue(MyCenterPointProperty, value); }
        //}
        //public static readonly DependencyProperty MyCenterPointProperty =
        //    DependencyProperty.Register(nameof(MyCenterPoint), typeof(Point), typeof(EzLine), new PropertyMetadata(new Point()));


        //public RotateTransform MyRotateTF
        //{
        //    get { return (RotateTransform)GetValue(MyRotateTFProperty); }
        //    set { SetValue(MyRotateTFProperty, value); }
        //}
        //public static readonly DependencyProperty MyRotateTFProperty =
        //    DependencyProperty.Register(nameof(MyRotateTF), typeof(RotateTransform), typeof(EzLine),
        //        new FrameworkPropertyMetadata(new RotateTransform(),
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public double MyAngle
        {
            get { return (double)GetValue(MyAngleProperty); }
            set { SetValue(MyAngleProperty, value); }
        }
        public static readonly DependencyProperty MyAngleProperty =
            DependencyProperty.Register(nameof(MyAngle), typeof(double), typeof(EzLine),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        //private static readonly DependencyPropertyKey MyRectWithPenAndRoatePropertyKey =
        //    DependencyProperty.RegisterReadOnly(nameof(MyRectWithPenAndRoate), typeof(Rect), typeof(EzLine), new PropertyMetadata(new Rect()));
        //public static readonly DependencyProperty MyRectWithPenAndRoateProperty = MyRectWithPenAndRoatePropertyKey.DependencyProperty;
        //public Rect MyRectWithPenAndRoate
        //{
        //    get { return (Rect)GetValue(MyRectWithPenAndRoatePropertyKey.DependencyProperty); }
        //    internal set { SetValue(MyRectWithPenAndRoatePropertyKey, value); }
        //}



        //private static readonly DependencyPropertyKey MyRectWithPenPropertyKey =
        //    DependencyProperty.RegisterReadOnly(nameof(MyRectWithPen), typeof(Rect), typeof(EzLine), new PropertyMetadata(new Rect()));
        //public static readonly DependencyProperty MyRectWithPenProperty = MyRectWithPenPropertyKey.DependencyProperty;
        //public Rect MyRectWithPen
        //{
        //    get { return (Rect)GetValue(MyRectWithPenPropertyKey.DependencyProperty); }
        //    internal set { SetValue(MyRectWithPenPropertyKey, value); }
        //}


        private static readonly DependencyPropertyKey MyRectPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyRect), typeof(Rect), typeof(EzLine), new PropertyMetadata(new Rect()));
        public static readonly DependencyProperty MyRectProperty = MyRectPropertyKey.DependencyProperty;
        public Rect MyRect
        {
            get { return (Rect)GetValue(MyRectPropertyKey.DependencyProperty); }
            internal set { SetValue(MyRectPropertyKey, value); }
        }



        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(EzLine),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    }





    public class MyConverterRotateTF : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double angle = (double)values[0];
            Rect r = (Rect)values[1];
            return new RotateTransform(angle, r.Width / 2.0, r.Height / 2.0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
