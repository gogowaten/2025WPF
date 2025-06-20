﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Globalization;
using System.Net;
using System.ComponentModel;


namespace _20250616
{
    public class Freehand : Grid
    {
        //public List<Polyline> MyListOfPolyline = [];
        //public List<PointCollection> MyListOfPointCollection = [];
        public List<AAA> MyListOfAAA { get; set; } = [];
        public Polyline MyPolyline { get; set; } = null!;
        private bool IsDrawing;
        public Freehand()
        {
            MouseLeftButtonDown += Freehand_MouseLeftButtonDown;
            PreviewMouseLeftButtonUp += Freehand_PreviewMouseLeftButtonUp;
            MouseMove += Freehand_MouseMove;
            MouseLeave += Freehand_MouseLeave;

            Background = new SolidColorBrush(Color.FromArgb(20, 0, 0, 0));


            MyPolyline = MakePolyline();
            SetZIndex(MyPolyline, 1);
            Children.Add(MyPolyline);
            MyPolyline.Visibility = Visibility.Collapsed;


        }

        // 描画完了
        private void Freehand_MouseLeave(object sender, MouseEventArgs e)
        {
            if (IsDrawing)
            {
                IsDrawing = false;
                DrawEnd();
            }
        }

        private void Freehand_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDrawing)
            {
                MyPolyline.Points.Add(e.GetPosition(this));
            }
        }

        // 描画完了
        private void Freehand_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsDrawing)
            {
                IsDrawing = false;
                DrawEnd();
            }
        }

        // 描画開始
        private void Freehand_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsDrawing = true;
            MyPolyline.Visibility = Visibility.Visible;
            MyPolyline.Points.Clear();
            MyPolyline.Points.Add(e.GetPosition(this));
        }

        private void DrawEnd()
        {
            AAA a = new();
            MyListOfAAA.Add(a);
            a.MyOriginPoints = MyPolyline.Points.Clone();

            Children.Add(a.MyPolyline);

            var mb = new MultiBinding() { Converter = new MyConvMage() };
            mb.Bindings.Add(new Binding() { Source = a, Path = new PropertyPath(AAA.MyOriginPointsProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyIntervalProperty) });
            BindingOperations.SetBinding(a, AAA.MyPointsProperty, mb);
            BindingOperations.SetBinding(a.MyPolyline, Shape.StrokeThicknessProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeThicknessProperty) });

            MyPolyline.Visibility = Visibility.Collapsed;
            MyPolyline.Points.Clear();

        }



        private PointCollection MakeOriginPoints()
        {
            PointCollection pc = [];
            for (int i = 0; i < 10; i++)
            {
                pc.Add(new Point(i, i * 10));
            }
            return pc;
        }

        private Polyline MakePolyline()
        {
            Polyline polyline = new()
            {
                Stroke = Brushes.Gray,
                StrokeThickness = MyStrokeThickness,
                StrokeLineJoin = PenLineJoin.Round,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round,
            };
            return polyline;
        }



        public int MyInterval
        {
            get { return (int)GetValue(MyIntervalProperty); }
            set { SetValue(MyIntervalProperty, value); }
        }
        public static readonly DependencyProperty MyIntervalProperty =
            DependencyProperty.Register(nameof(MyInterval), typeof(int), typeof(Freehand), new PropertyMetadata(1));


        public double MyStrokeThickness
        {
            get { return (double)GetValue(MyStrokeThicknessProperty); }
            set { SetValue(MyStrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeThicknessProperty =
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(Freehand), new PropertyMetadata(20.0));




    }

    public class AAA : DependencyObject
    {
        public Polyline MyPolyline { get; set; }

        public AAA()
        {

            MyPolyline = new()
            {
                Stroke = Brushes.DodgerBlue,
                StrokeThickness = 20.0,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round,
                StrokeLineJoin = PenLineJoin.Round,
            };
            MyBind();

        }

        private void MyBind()
        {
            MyPolyline.SetBinding(Polyline.PointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty) });
        }


        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(AAA), new PropertyMetadata(null));

        public PointCollection MyOriginPoints
        {
            get { return (PointCollection)GetValue(MyOriginPointsProperty); }
            set { SetValue(MyOriginPointsProperty, value); }
        }
        public static readonly DependencyProperty MyOriginPointsProperty =
            DependencyProperty.Register(nameof(MyOriginPoints), typeof(PointCollection), typeof(AAA), new PropertyMetadata(null));


    }



    public class MyConvMage : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var origin = (PointCollection)values[0];
            var interval = (int)values[1];
            return ChoiceAnchorPoint(origin, interval);
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
