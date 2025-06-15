using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Windows.Input;


namespace _20250615
{


    public class FreehandGrid : Grid
    {
        //private Polyline MyPolyline { get; set; } = new();
        private Polyline MyOriginPolyline { get; set; } = new();
        private List<PointCollection> MyListOfOriginPointCollection = [];
        private bool IsDrawing;
        public FreehandGrid()
        {

            //MyPoints = [];
            //_ = Children.Add(MyPolyline);
            MouseLeftButtonDown += FreehandGrid_MouseLeftButtonDown;
            MouseMove += FreehandGrid_MouseMove;
            PreviewMouseLeftButtonUp += FreehandGrid_PreviewMouseLeftButtonUp;
        }

        // ドラッグ移動終了時
        private void FreehandGrid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsDrawing = false;
            MyListOfOriginPointCollection.Add(MyPoints);
        }

        // マウス移動中
        private void FreehandGrid_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (IsDrawing && e.LeftButton == MouseButtonState.Pressed)
            {
                MyPoints.Add(e.GetPosition(this));
            }
        }

        private void FreehandGrid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IsDrawing = true;
            MyInitializePolyline();
            MyInitializeOriginPolyline();

        }


        #region 依存関係プロパティ


        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(FreehandGrid), new PropertyMetadata());

        public PointCollection MyOriginPoints
        {
            get { return (PointCollection)GetValue(MyOriginPointsProperty); }
            set { SetValue(MyOriginPointsProperty, value); }
        }
        public static readonly DependencyProperty MyOriginPointsProperty =
            DependencyProperty.Register(nameof(MyOriginPoints), typeof(PointCollection), typeof(FreehandGrid), new PropertyMetadata());


        public double MyStrokeThickness
        {
            get { return (double)GetValue(MyStrokeThicknessProperty); }
            set { SetValue(MyStrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeThicknessProperty =
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(FreehandGrid), new PropertyMetadata(20.0));

        //public Brush Stroke
        //{
        //    get { return (Brush)GetValue(StrokeProperty); }
        //    set { SetValue(StrokeProperty, value); }
        //}
        //public static readonly DependencyProperty StrokeProperty =
        //    DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(FreehandGrid), new PropertyMetadata(Brushes.Magenta));

        #endregion 依存関係プロパティ

        #region メソッド
        public void Test()
        {
            var pc = MyListOfOriginPointCollection[0];
            for (int i = 0; i < 10; i++)
            {
                pc.Remove(pc[^1]);
            }
        }
        private void MyInitializePolyline()
        {
            Polyline polyline = new()
            {
                Stroke = Brushes.Magenta,
                StrokeLineJoin = PenLineJoin.Round,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round,

            };
            SetZIndex(polyline, 1);
            Children.Add(polyline);
            polyline.SetBinding(Shape.StrokeThicknessProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeThicknessProperty) });
            MyPoints = [];
            polyline.Points = MyPoints;
        }

        private void MyInitializeOriginPolyline()
        {
            Polyline polyline = new()
            {
                Stroke = Brushes.Gray,
                StrokeLineJoin = PenLineJoin.Round,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round,

            };
            SetZIndex(polyline, 0);
            Children.Add(polyline);

            polyline.SetBinding(Shape.StrokeThicknessProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeThicknessProperty) });
            MyOriginPoints = [];
            polyline.Points = MyOriginPoints;
        }


        //private Polyline MakePolyline()
        //{
        //    Polyline temp = new()
        //    {
        //        Stroke = Brushes.Magenta,
        //        StrokeLineJoin = PenLineJoin.Round,
        //        StrokeStartLineCap = PenLineCap.Round,
        //        StrokeEndLineCap = PenLineCap.Round,

        //    };

        //    _ = temp.SetBinding(StrokeThicknessProperty,
        //        new Binding() { Source = this, Path = new PropertyPath(StrokeThicknessProperty) });
        //    return temp;
        //}
        #endregion メソッド

    }

}
