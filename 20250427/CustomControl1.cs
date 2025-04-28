using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250427
{
    public class DrawPanel : Control
    {
        private bool IsDrawing;
        public GeoShape MyShape { get; set; } = new();
        public Panel MyPanel { get; private set; } = null!;
        public PointCollection MyPoints { get; private set; } = [];
        static DrawPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DrawPanel), new FrameworkPropertyMetadata(typeof(DrawPanel)));
        }
        public DrawPanel()
        {
            IsDrawing = true;
            MouseLeftButtonDown += DrawPanel_MouseLeftButtonDown;
            MouseMove += DrawPanel_MouseMove;
            Loaded += DrawPanel_Loaded;
            MouseDoubleClick += DrawPanel_MouseDoubleClick;
        }

        private void DrawPanel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IsDrawing = false;
            MyPoints.RemoveAt(MyPoints.Count - 1);
            var neko = MyPoints;
        }

        private void DrawPanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (Parent is FrameworkElement element)
            {
                this.Width = element.ActualWidth;
                this.Height = element.ActualHeight;
            }
        }

        private void DrawPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDrawing && MyPoints.Count > 1)
            {
                MyPoints[^1] = e.GetPosition(this);
            }
        }

        private void DrawPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsDrawing)
            {
                int id = MyPoints.Count;
                if (id == 0)
                {
                    MyPoints.Add(e.GetPosition(this));
                }
                MyPoints.Add(e.GetPosition(this));
                //MyShape.MyPoints.Add(e.GetPosition(this));
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_Panel") is Panel panel)
            {
                MyPanel = panel;
                MyPanel.Background = Brushes.Orange;
                MyPanel.Children.Add(MyShape);
                MyShape.MyPoints = this.MyPoints;
                MyShape.Stroke = Brushes.Red;
                MyShape.StrokeThickness = 10;
                MyShape.MyHeadEndType = HeadType.Arrow;
            }
        }
    }


    public class CustomControl1 : Control
    {
        static CustomControl1()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomControl1), new FrameworkPropertyMetadata(typeof(CustomControl1)));
        }
    }
}
