using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Globalization;
using System.ComponentModel;
using System.Windows;


namespace _20250118
{
    public class GeoLine : Shape
    {
        public GeoLine() { }

        protected override Geometry DefiningGeometry
        {

            get
            {
                if (MyPoints is null || MyPoints.Count == 0) { return Geometry.Empty; }

                StreamGeometry geometry = new();
                using (var context = geometry.Open())
                {
                    Point begin = MyPoints[0];
                    Point end = MyPoints[^1];
                    DrawLine(context, begin, end);

                }
                geometry.Freeze();
                return geometry;
            }
        }

        private void DrawLine(StreamGeometryContext context, Point begin, Point end)
        {
            context.BeginFigure(begin, false, false);
            for (int i = 1; i < MyPoints.Count - 1; i++)
            {
                context.LineTo(MyPoints[i], true, false);
            }
            context.LineTo(end, true, false);
        }





        //AffectsMeasureはXAMLでの更新に必要
        //AffectsRenderもあったほうがいい、起動中にXAML変更が反映される
        //TwoWayは？
        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(GeoLine),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    }
}
