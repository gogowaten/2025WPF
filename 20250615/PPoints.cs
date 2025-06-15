using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;


namespace _20250615
{
    public class PPoints: FrameworkElement
    {
        public PPoints()
        {
            MyOriginPoints = [];
            var mb = new MultiBinding() { Converter = new MyConvMage() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyOriginPointsProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyIntervalProperty) });
            SetBinding(MyPointsProperty, mb);
        }


        #region 依存関係プロパティ

        public int MyInterval
        {
            get { return (int)GetValue(MyIntervalProperty); }
            set { SetValue(MyIntervalProperty, value); }
        }
        public static readonly DependencyProperty MyIntervalProperty =
            DependencyProperty.Register(nameof(MyInterval), typeof(int), typeof(PPoints), new PropertyMetadata(1));

        public PointCollection MyOriginPoints
        {
            get { return (PointCollection)GetValue(MyOriginPointsProperty); }
            set { SetValue(MyOriginPointsProperty, value); }
        }
        public static readonly DependencyProperty MyOriginPointsProperty =
            DependencyProperty.Register(nameof(MyOriginPoints), typeof(PointCollection), typeof(PPoints), new PropertyMetadata(null));

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(PPoints), new PropertyMetadata(null));


        #endregion 依存関係プロパティ
    }

    public class MyConvMage : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var ori = (PointCollection)values[0];
            var interval = (int)values[1];
            var ppc = ChoiceAnchorPoint(ori.Clone(), interval);
            return ppc;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
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
            if(points.Count == 0) { return selectedPoints; }

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
