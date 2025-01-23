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

        #endregion 依存関係プロパティ
        public ExLine()
        {
            
            Binding b1 = new() { Source = this, Path = new PropertyPath(StrokeProperty) };
            Binding b2 = new() { Source = this, Path = new PropertyPath(StrokeThicknessProperty) };
            MultiBinding mb = new();
            mb.Bindings.Add(b1);
            mb.Bindings.Add(b2);
            mb.Converter = new MyConverterPen();
            SetBinding(MyPenProperty, mb);
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
                //MyBounds = VisualTreeHelper.GetDescendantBounds(this);
                //PathGeometry neko = geo.GetWidenedPathGeometry(MyPen);
                //MyBounds = neko.Bounds;
                MyBounds = geo.GetWidenedPathGeometry(MyPen).Bounds;


                Canvas.SetLeft(this, -MyBounds.Left);
                Canvas.SetTop(this, -MyBounds.Top);
                //Width = MyBounds.Width;
                //Height = MyBounds.Height;
                return geo;
            }
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
}
