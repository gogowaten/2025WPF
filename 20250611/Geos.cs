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
using System.Windows.Markup;
using System.Collections.ObjectModel;
namespace _20250611
{
    [ContentProperty(nameof(Points))]
    public class GeoPoints : DependencyObject
    {
        public GeoPoints() { }
        public GeoPoints(Point begin, ObservableCollection<Point> points)
        {
            BeginPoint = begin;
            //if (points.Count == 0)
            //{
            //    this.Points.Add(new Point(begin.X, begin.Y));
            //}
            Points = points;
        }


        #region 依存関係プロパティ

        //public Point BeginPoint
        //{
        //    get { return (Point)GetValue(BeginPointProperty); }
        //    set { SetValue(BeginPointProperty, value); }
        //}
        //public static readonly DependencyProperty BeginPointProperty =
        //    DependencyProperty.Register(nameof(BeginPoint), typeof(Point), typeof(GeoPoints), new PropertyMetadata(new Point()));

        public Point BeginPoint
        {
            get { return (Point)GetValue(BeginPointProperty); }
            set { SetValue(BeginPointProperty, value); }
        }
        public static readonly DependencyProperty BeginPointProperty =
            DependencyProperty.Register(nameof(BeginPoint), typeof(Point), typeof(GeoPoints),
                new FrameworkPropertyMetadata(new Point(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        //public List<Point> Points
        //{
        //    get { return (List<Point>)GetValue(PointsProperty); }
        //    set { SetValue(PointsProperty, value); }
        //}
        //public static readonly DependencyProperty PointsProperty =
        //    DependencyProperty.Register(nameof(Points), typeof(List<Point>), typeof(GeoPoints), new PropertyMetadata(new List<Point>()));

        //public PointCollection Points
        //{
        //    get { return (PointCollection)GetValue(PointsProperty); }
        //    set { SetValue(PointsProperty, value); }
        //}
        //public static readonly DependencyProperty PointsProperty =
        //    DependencyProperty.Register(nameof(Points), typeof(PointCollection), typeof(GeoPoints), new PropertyMetadata(new PointCollection()));


        //public PointCollection Points
        //{
        //    get { return (PointCollection)GetValue(PointsProperty); }
        //    set { SetValue(PointsProperty, value); }
        //}
        //public static readonly DependencyProperty PointsProperty =
        //    DependencyProperty.Register(nameof(Points), typeof(PointCollection), typeof(GeoPoints),
        //        new FrameworkPropertyMetadata(new PointCollection(),
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public ObservableCollection<Point> Points
        {
            get { return (ObservableCollection<Point>)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }
        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register(nameof(Points), typeof(ObservableCollection<Point>), typeof(GeoPoints),
                new FrameworkPropertyMetadata(new ObservableCollection<Point>(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        #endregion 依存関係プロパティ
    }



    class Geos : Shape
    {
        public Geos()
        {
          
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                //if (MyPoints is null) { return Geometry.Empty; }

                StreamGeometry geo = new();
                using (StreamGeometryContext context = geo.Open())
                {
                    
                    DrawTest(context);
                }

                geo.Freeze();
                return geo;
            }

        }

        #region 依存関係プロパティ


        public ObservableCollection<GeoPoints> GeoPointsList
        {
            get { return (ObservableCollection<GeoPoints>)GetValue(GeoPointsListProperty); }
            set { SetValue(GeoPointsListProperty, value); }
        }
        public static readonly DependencyProperty GeoPointsListProperty =
            DependencyProperty.Register(nameof(GeoPointsList), typeof(ObservableCollection<GeoPoints>), typeof(Geos),
                new FrameworkPropertyMetadata(new ObservableCollection<GeoPoints>(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        //public List<GeoPoints> GeoPointsList
        //{
        //    get { return (List<GeoPoints>)GetValue(GeoPointsListProperty); }
        //    set { SetValue(GeoPointsListProperty, value); }
        //}
        //public static readonly DependencyProperty GeoPointsListProperty =
        //    DependencyProperty.Register(nameof(GeoPointsList), typeof(List<GeoPoints>), typeof(Geos), new PropertyMetadata(new List<GeoPoints>()));

        //public List<GeoPoints> GeoPointsList
        //{
        //    get { return (List<GeoPoints>)GetValue(GeoPointsListProperty); }
        //    set { SetValue(GeoPointsListProperty, value); }
        //}
        //public static readonly DependencyProperty GeoPointsListProperty =
        //    DependencyProperty.Register(nameof(GeoPointsList), typeof(List<GeoPoints>), typeof(Geos),
        //        new FrameworkPropertyMetadata(new List<GeoPoints>(),
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        #endregion 依存関係プロパティ

        private void DrawTest(StreamGeometryContext context)
        {
            for (int i = 0; i < GeoPointsList.Count; i++)
            {
                context.BeginFigure(GeoPointsList[i].BeginPoint, false, false);
                context.PolyLineTo(GeoPointsList[i].Points, true, true);

            }

        }
    }
}
