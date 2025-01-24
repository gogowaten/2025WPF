using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace _20250124
{
    internal class Class1
    {
    }
    public class ExLine : FrameworkElement
    {

        //public PointConverter MyPoints
        //{
        //    get { return (PointConverter)GetValue(MyPointsProperty); }
        //    set { SetValue(MyPointsProperty, value); }
        //}
        //public static readonly DependencyProperty MyPointsProperty =
        //    DependencyProperty.Register(nameof(MyPoints), typeof(PointConverter), typeof(ExLine), new PropertyMetadata(null));

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
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(ExLine), new FrameworkPropertyMetadata(0.0,
                FrameworkPropertyMetadataOptions.AffectsRender |// デザイン画面上での更新で必要
                FrameworkPropertyMetadataOptions.AffectsMeasure)); // 必要ないかも？

        public Brush MyStroke
        {
            get { return (Brush)GetValue(MyStrokeProperty); }
            set { SetValue(MyStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeProperty =
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(ExLine), new PropertyMetadata(Brushes.YellowGreen));

        public ExLine()
        {

        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            PathGeometry geo = new();
            //PolyLineSegment seg = new(MyPoints.Skip(1), false);
            PolyLineSegment seg = new(MyPoints.Skip(1), true);
            PathFigure fig = new();
            fig.Segments.Add(seg);
            fig.StartPoint = MyPoints[0];
            fig.IsFilled = false;
            geo.Figures.Add(fig);
            //geo.FillRule = FillRule.EvenOdd;
            geo.FillRule = FillRule.Nonzero;

            //drawingContext.DrawGeometry(MyStroke, null, geo);
            drawingContext.DrawGeometry(MyStroke, new Pen(Brushes.Red, MyStrokeThickness), geo);

            //base.OnRender(drawingContext);
        }

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





}
