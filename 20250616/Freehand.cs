using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;


namespace _20250616
{
    public class Freehand : Grid
    {
        public List<Polyline> MyListOfPolyline = [];
        public List<PointCollection> MyListOfPointCollection = [];
        public Polyline MyPolyline { get; set; } = null!;
        public Freehand()
        {
            for (int i = 0; i < 10; i++)
            {
                MyListOfPolyline.Add(MakePolyline());
            }
            Children.Add(MyListOfPolyline[0]);
        }

        private Polyline MakePolyline()
        {
            Polyline polyline = new()
            {
                Stroke = Brushes.Gray,
                StrokeThickness = 20,
                StrokeLineJoin = PenLineJoin.Round,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round,
            };
            for (int i = 0; i < 10; i++)
            {
                polyline.Points.Add(new System.Windows.Point(i, i + 100));
            }
            return polyline;
        }




        /// <summary>
        /// 元のPointCollectionから指定間隔で選んだPointCollectionを新たに作成して返す
        /// </summary>
        /// <param name="points"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        private static PointCollection ChoiceAnchorPoint(PointCollection points, int interval)
        {
            var selectedPoints = new PointCollection();
            if (points.Count == 0) { return selectedPoints; }

            if (interval < 1) { interval = 1; }//間隔は1以上
            for (int i = 0; i < points.Count - 1; i += interval)
            {
                selectedPoints.Add(points[i]);
            }
            selectedPoints.Add(points[^1]);//最後の一個は必ず入れる

            //選んだ頂点が3個以上あって、最後の頂点と最後から2番めが近いときは2番めを除去            
            if (selectedPoints.Count >= 3)
            {
                int mod = (points.Count - 2) % interval;
                if (interval / 2 > mod)
                {
                    selectedPoints.RemoveAt(selectedPoints.Count - 2);//除去
                }
            }
            return selectedPoints;
        }



    }
}
