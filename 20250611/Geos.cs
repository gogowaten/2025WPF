using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Security.Cryptography.Xml;
namespace _20250611
{

    public class GeoPoints : DependencyObject
    {
        public GeoPoints() { }
        public GeoPoints(Point begin, List<Point> points)
        {
            BeginPoint = begin;
            Points = points;
        }
        public GeoPoints(List<Point> points)
        {
            BeginPoint = points[0];
            if (points.Count > 1)
            {
                Points = points.GetRange(1, points.Count);
            }
            else
            {

            }
        }

        public Point BeginPoint
        {
            get { return (Point)GetValue(BeginPointProperty); }
            set { SetValue(BeginPointProperty, value); }
        }
        public static readonly DependencyProperty BeginPointProperty =
            DependencyProperty.Register(nameof(BeginPoint), typeof(Point), typeof(GeoPoints), new PropertyMetadata(new Point()));


        public List<Point> Points
        {
            get { return (List<Point>)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }
        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register(nameof(Points), typeof(List<Point>), typeof(GeoPoints), new PropertyMetadata(new List<Point>()));

    }



    class Geos : Shape
    {
        public Geos()
        {
            GeoPointsList = [];
            GeoPoints points = new(new Point(), [new Point(100, 0), new Point(100, 100)]);
            GeoPointsList.Add(points);
            GeoPointsList.Add(new GeoPoints() { BeginPoint = new Point(100, 0), });
            GeoPointsList.Add(new GeoPoints(new Point(), [new Point(100, 0), new Point()]));
            GeoPointsList.Add(new GeoPoints([new Point()]));
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                //if (MyPoints is null) { return Geometry.Empty; }

                StreamGeometry geo = new();
                using (StreamGeometryContext context = geo.Open())
                {
                    //PointCollection po = MyPoints[0];
                    //context.BeginFigure(po[0], isFilled: true, isClosed: false);
                    //List<Point> pp = [new Point(), new Point(100, 100)];
                    //context.PolyLineTo(pp, true, false);
                    //context.PolyLineTo(pp.GetRange(1, pp.Count - 1), true, false);
                    //context.LineTo(po[^1], isStroked: true, isSmoothJoin: true);

                    DrawTest(context);
                }

                geo.Freeze();
                return geo;
            }

        }

        #region 依存関係プロパティ


        public List<GeoPoints> GeoPointsList
        {
            get { return (List<GeoPoints>)GetValue(GeoPointsListProperty); }
            set { SetValue(GeoPointsListProperty, value); }
        }
        public static readonly DependencyProperty GeoPointsListProperty =
            DependencyProperty.Register(nameof(GeoPointsList), typeof(List<GeoPoints>), typeof(Geos), new PropertyMetadata(new List<GeoPoints>()));


        #endregion 依存関係プロパティ

        private void DrawTest(StreamGeometryContext context)
        {
            //List<Point> points = [new Point(), new Point(0, 50)];
            //context.BeginFigure(new Point(100, 0), false, false);
            //context.PolyLineTo(points, true, true);

            //context.BeginFigure(new Point(0, 150), false, false);
            //List<Point> points2 = [new Point(50, 50), new Point(100, 100)];
            //context.PolyLineTo(points2, true, true);
            for (int i = 0; i < GeoPointsList.Count; i++)
            {
                context.BeginFigure(GeoPointsList[i].BeginPoint, false, false);
                context.PolyLineTo(GeoPointsList[i].Points, true, true);

            }

        }
    }
}
