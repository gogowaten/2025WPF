using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;


namespace _20250118
{
    public class MyLineShape : Shape
    {

        public MyLineShape() { }

        protected override Geometry DefiningGeometry
        {
            get
            {
                if (MyObPoints is null || MyObPoints.Count == 0) { return Geometry.Empty; }

                StreamGeometry geometry = new();
                using (var context = geometry.Open())
                {
                    Point begin = MyObPoints[0];
                    Point end = MyObPoints[^1];
                    DrawLine(context, begin, end);

                }
                geometry.Freeze();
                return geometry;
            }
        }

        private void DrawLine(StreamGeometryContext context, Point begin, Point end)
        {
            context.BeginFigure(begin, false, false);
            for (int i = 1; i < MyObPoints.Count - 1; i++)
            {
                context.LineTo(MyObPoints[i], true, false);
            }
            context.LineTo(end, true, false);
        }



        //[TypeConverter(typeof(MyTypeConverterStringToPoints))]
        //public ObservableCollection<Point> MyObPoints
        //{
        //    get { return (ObservableCollection<Point>)GetValue(MyPointsProperty); }
        //    set { SetValue(MyPointsProperty, value); }
        //}
        //public static readonly DependencyProperty MyPointsProperty =
        //    DependencyProperty.Register(nameof(MyObPoints), typeof(ObservableCollection<Point>), typeof(MyLineShape),
        //        new FrameworkPropertyMetadata(null,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        public PointCollection MyObPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyObPoints), typeof(PointCollection), typ0eof(MyLineShape),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    }


    /// <summary>
    /// 文字列をObservableCollectionに変換、要素の型はPoint
    /// 例：XAMLでの入力が"0,10 20,100"なら、Point(0,10)とPoint(20,100)に変換する
    /// </summary>
    public class MyTypeConverterStringToPoints : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }
        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value == null) return null;
            if (value is string str)
            {
                string[] ss = str.Split(' ');
                ObservableCollection<Point> points = [];
                foreach (var item in ss)
                {
                    string[] xy = item.Split(',');
                    if (double.TryParse(xy[0], out double x) && double.TryParse(xy[1], out double y))
                    {
                        Point point = new(x, y);
                        points.Add(point);
                    }
                }
                return points;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }



}
