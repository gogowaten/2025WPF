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
    public class FHGrid : Grid
    {
        private bool IsDrawing;
        public Polyline MyPolyline { get; set; } = new();
        public FHGrid()
        {
            MouseLeftButtonDown += FHGrid_MouseLeftButtonDown;
            MouseMove += FHGrid_MouseMove;
            PreviewMouseLeftButtonUp += FHGrid_PreviewMouseLeftButtonUp;

            //MyPPoints = new()
            //{
            //    MyOriginPoints = [new Point(), new Point(0, 100), new Point(100, 100)]
            //};




        }

        private void FHGrid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsDrawing = false;
            MyPolyline.SetBinding(Polyline.PointsProperty, new Binding() { Source = MyPPoints, Path = new PropertyPath(PPoints.MyPointsProperty) });
        }

        private void FHGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDrawing)
            {
                MyPPoints.MyOriginPoints.Add(e.GetPosition(this));
            }
        }

        private void FHGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsDrawing= true;
            MyPPoints = new();
            MyPPoints.SetBinding(PPoints.MyIntervalProperty, new Binding() { Source = this, Path = new PropertyPath(MyIntervalProperty) });


            MyPolyline = new Polyline()
            {
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round,
                StrokeLineJoin = PenLineJoin.Round,
            };
            Children.Add(MyPolyline);
            MyPolyline.SetBinding(Shape.StrokeProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeProperty) });
            MyPolyline.SetBinding(Shape.StrokeThicknessProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeThicknessProperty) });

            MyPolyline.SetBinding(Polyline.PointsProperty, new Binding() { Source = MyPPoints, Path = new PropertyPath(PPoints.MyOriginPointsProperty) });
        }



        public int MyInterval
        {
            get { return (int)GetValue(MyIntervalProperty); }
            set { SetValue(MyIntervalProperty, value); }
        }
        public static readonly DependencyProperty MyIntervalProperty =
            DependencyProperty.Register(nameof(MyInterval), typeof(int), typeof(FHGrid), new PropertyMetadata(1));


        public Brush MyStroke
        {
            get { return (Brush)GetValue(MyStrokeProperty); }
            set { SetValue(MyStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeProperty =
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(FHGrid), new PropertyMetadata(Brushes.Gray));


        public double MyStrokeThickness
        {
            get { return (double)GetValue(MyStrokeThicknessProperty); }
            set { SetValue(MyStrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeThicknessProperty =
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(FHGrid), new PropertyMetadata(20.0));


        public PPoints MyPPoints
        {
            get { return (PPoints)GetValue(MyPPointsProperty); }
            set { SetValue(MyPPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPPointsProperty =
            DependencyProperty.Register(nameof(MyPPoints), typeof(PPoints), typeof(FHGrid), new PropertyMetadata(null));

    }

    public class FreehandGrid : Grid
    {
        //private Polyline MyPolyline { get; set; } = new();
        private Polyline MyOriginPolyline { get; set; } = new();
        //private List<PointCollection> MyListOfOriginPointCollection = [];
        private List<Polyline> MyListOfPolyline = [];
        private bool IsDrawing;
        public FreehandGrid()
        {
            MouseLeftButtonDown += FreehandGrid_MouseLeftButtonDown;
            MouseMove += FreehandGrid_MouseMove;
            PreviewMouseLeftButtonUp += FreehandGrid_PreviewMouseLeftButtonUp;

            //PPolyline pp = new();
            //Children.Add(pp);
        }

        // ドラッグ移動終了時
        private void FreehandGrid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsDrawing = false;
            MyListOfPolyline.Add(MyOriginPolyline);
            Kakou(MyOriginPolyline.Points);
        }

        // マウス移動中
        private void FreehandGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDrawing && e.LeftButton == MouseButtonState.Pressed)
            {
                //MyOriginPoints.Add(e.GetPosition(this));
                MyOriginPolyline.Points.Add(e.GetPosition(this));
            }
        }

        // 開始
        private void FreehandGrid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IsDrawing = true;
            //MyInitializePolyline();
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
        public void Kakou(PointCollection pc)
        {
            for (int i = 0; i < 10; i++)
            {
                pc.Remove(pc[^1]);
            }
        }

        public void Test()
        {
            var pc = MyListOfPolyline[0].Points;
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
            MyOriginPolyline = new()
            {
                Stroke = Brushes.Gray,
                StrokeLineJoin = PenLineJoin.Round,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round,

            };
            //SetZIndex(polyline, 0);
            Children.Add(MyOriginPolyline);

            MyOriginPolyline.SetBinding(Shape.StrokeThicknessProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeThicknessProperty) });


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
