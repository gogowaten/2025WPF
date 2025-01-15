using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250115
{
    public class KnobThumb : Thumb
    {
        static KnobThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KnobThumb), new FrameworkPropertyMetadata(typeof(KnobThumb)));
        }
        public KnobThumb()
        {

        }

    }
    public class RangeThumb2 : Thumb
    {
        Panel? MyPanel;
        //Rectangle MyRect;


        private KnobThumb MyThumb0 = new();
        private KnobThumb MyThumb1 = new();
        private KnobThumb MyThumb2 = new();
        private KnobThumb MyThumb3 = new();
        private KnobThumb MyThumb4 = new();
        private KnobThumb MyThumb5 = new();
        private KnobThumb MyThumb6 = new();
        private KnobThumb MyThumb7 = new();


        static RangeThumb2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeThumb2), new FrameworkPropertyMetadata(typeof(RangeThumb2)));
        }
        public RangeThumb2()
        {
            Loaded += RangeThumb2_Loaded;
            DragDelta += RangeThumb2_DragDelta;
            MyThumb7.DragDelta += MyThumb7_DragDelta;
        }

        private void RangeThumb2_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                var left = Canvas.GetLeft(t) + e.HorizontalChange;
                var top = Canvas.GetTop(t) + e.VerticalChange;
                if (left < 0) { left = 0; }
                if (top < 0) { top = 0; }
                Canvas.SetLeft(t, left);
                Canvas.SetTop(t, top);
                e.Handled = true;
            }

        }

        private void MyThumb7_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                var left = Canvas.GetLeft(t) + e.HorizontalChange;
                var top = Canvas.GetTop(t) + e.VerticalChange;
                if (left < 1) { left = 1; } if (top < 1) { top = 1; }
                Canvas.SetLeft(t, left);
                Canvas.SetTop(t, top);
                e.Handled = true;
            }

        }

        private void RangeThumb2_Loaded(object sender, RoutedEventArgs e)
        {
            if (GetTemplateChild("MyPanel") is Panel panel)
            {
                MyPanel = panel;
                MyPanel.Width = 100; MyPanel.Height = 100; MyPanel.Background = Brushes.Red;
                MyPanel.Children.Add(MyThumb7);
                MyThumb7.SetBinding(Canvas.LeftProperty, new Binding() { Source = MyPanel, Path = new PropertyPath(WidthProperty) ,Mode=BindingMode.TwoWay });
                MyThumb7.SetBinding(Canvas.TopProperty, new Binding() { Source = MyPanel, Path = new PropertyPath(HeightProperty),Mode=BindingMode.TwoWay });

            }
        }
    }

    public class RangeThumb : Thumb
    {
        Grid? MyPanel;
        Rectangle MyRect;


        private KnobThumb MyThumb0 = new();
        private KnobThumb MyThumb1 = new();
        private KnobThumb MyThumb2 = new();
        private KnobThumb MyThumb3 = new();
        private KnobThumb MyThumb4 = new();
        private KnobThumb MyThumb5 = new();
        private KnobThumb MyThumb6 = new();
        private KnobThumb MyThumb7 = new();


        static RangeThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeThumb), new FrameworkPropertyMetadata(typeof(RangeThumb)));
        }
        public RangeThumb()
        {

            Loaded += RangeThumb_Loaded;
            MyRect = new() { Width = 200, Height = 200, Fill = Brushes.DodgerBlue };

            MyThumb0.DragDelta += MyThumb_DragDelta;
            MyThumb2.DragDelta += MyThumb_DragDelta;
            MyThumb5.DragDelta += MyThumb_DragDelta;
            MyThumb7.DragDelta += MyThumb_DragDelta;
            MyThumb1.DragDelta += MyThumb_DragDeltaOnlyVertical;
            MyThumb6.DragDelta += MyThumb_DragDeltaOnlyVertical;
            MyThumb3.DragDelta += MyThumb_DragDeltaOnlyHorizontal;
            MyThumb4.DragDelta += MyThumb_DragDeltaOnlyHorizontal;
            DragDelta += My_DragDelta;

        }


        private void RangeThumb_Loaded(object sender, RoutedEventArgs e)
        {
            if (GetTemplateChild("MyPanel") is Grid panel)
            {
                MyPanel = panel;
                MyPanel.Children.Add(MyRect);
                //MyPanel.Children.Add(MyThumb0);
                //MyPanel.Children.Add(MyThumb1);
                //MyPanel.Children.Add(MyThumb2);
                //MyPanel.Children.Add(MyThumb3);
                //MyPanel.Children.Add(MyThumb4);
                //MyPanel.Children.Add(MyThumb5);
                //MyPanel.Children.Add(MyThumb6);
                MyPanel.Children.Add(MyThumb7);
                MyThumb7.SetBinding(Canvas.LeftProperty, new Binding() { Source = MyRect, Path = new PropertyPath(WidthProperty) });
                MyThumb7.SetBinding(Canvas.TopProperty, new Binding() { Source = MyRect, Path = new PropertyPath(HeightProperty) });

            }
        }


        #region ドラッグ移動        

        private void My_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is RangeThumb me)
            {
                double left = Math.Max(0, Canvas.GetLeft(me) + e.HorizontalChange);
                double top = Math.Max(0, Canvas.GetTop(me) + e.VerticalChange);
                //Canvas.SetLeft(me, Math.Max(0, Canvas.GetLeft(me) + e.HorizontalChange));
                //Canvas.SetTop(me, Math.Max(0, Canvas.GetTop(me) + e.VerticalChange));
                Canvas.SetLeft(me, left);
                Canvas.SetTop(me, top);
            }

        }



        private void MyThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                var h = Canvas.GetLeft(t);
                var v = Canvas.GetTop(t);
                Canvas.SetLeft(t, h);
                Canvas.SetTop(t, v);
                e.Handled = true;
            }

        }
        private void MyThumb_DragDeltaOnlyVertical(object sender, DragDeltaEventArgs e)
        {
            if (sender is not Thumb t) { return; }
            Canvas.SetTop(t, Canvas.GetTop(t) + e.VerticalChange);
            e.Handled = true;
        }
        private void MyThumb_DragDeltaOnlyHorizontal(object sender, DragDeltaEventArgs e)
        {
            if (sender is not Thumb t) { return; }
            Canvas.SetLeft(t, Canvas.GetLeft(t) + e.HorizontalChange);
            e.Handled = true;
        }

        #endregion ドラッグ移動        




    }



}
