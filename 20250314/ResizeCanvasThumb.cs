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

namespace _20250314
{
    public class Handle : Thumb
    {
        static Handle()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Handle), new FrameworkPropertyMetadata(typeof(Handle)));
        }
        public Handle()
        {

        }


        public double MyLeft
        {
            get { return (double)GetValue(MyLeftProperty); }
            set { SetValue(MyLeftProperty, value); }
        }
        public static readonly DependencyProperty MyLeftProperty =
            DependencyProperty.Register(nameof(MyLeft), typeof(double), typeof(Handle), new PropertyMetadata(0.0));

        public double MyTop
        {
            get { return (double)GetValue(MyTopProperty); }
            set { SetValue(MyTopProperty, value); }
        }
        public static readonly DependencyProperty MyTopProperty =
            DependencyProperty.Register(nameof(MyTop), typeof(double), typeof(Handle), new PropertyMetadata(0.0));

    }



    public class ResizeCanvasThumb : Thumb
    {
        static ResizeCanvasThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ResizeCanvasThumb), new FrameworkPropertyMetadata(typeof(ResizeCanvasThumb)));
        }
        public ResizeCanvasThumb()
        {            
            Loaded += ResizeCanvasThumb_Loaded;
            DragDelta += ResizeCanvasThumb_DragDelta;
        }

        private void ResizeCanvasThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            //if(sender is ResizeCanvasThumb t)
            //{
            //    t.MyLeft += e.HorizontalChange;
            //    t.MyTop += e.VerticalChange;
            //    e.Handled = true;
            //}
            MyLeft += e.HorizontalChange;
            MyTop += e.VerticalChange;
            e.Handled = true;
        }

        private void ResizeCanvasThumb_Loaded(object sender, RoutedEventArgs e)
        {
            if (AdornerLayer.GetAdornerLayer(this) is AdornerLayer layer)
            {
                ResizeAdorner adorner = new(this);
                adorner.SetBinding(ResizeAdorner.MyHandleSizeProperty, new Binding() { Source = this, Path = new PropertyPath(MyAdornerHandleSizeProperty) });
                layer.Add(adorner);               
                
            }
            //SetBinding(Canvas.LeftProperty, new Binding() { Source = this, Path = new PropertyPath(MyLeftProperty) });

        }


        public double MyAdornerHandleSize
        {
            get { return (double)GetValue(MyAdornerHandleSizeProperty); }
            set { SetValue(MyAdornerHandleSizeProperty, value); }
        }
        public static readonly DependencyProperty MyAdornerHandleSizeProperty =
            DependencyProperty.Register(nameof(MyAdornerHandleSize), typeof(double), typeof(ResizeCanvasThumb), new PropertyMetadata(20.0));


        //public double MyLeft
        //{
        //    get { return (double)GetValue(MyLeftProperty); }
        //    set { SetValue(MyLeftProperty, value); }
        //}
        //public static readonly DependencyProperty MyLeftProperty =
        //    DependencyProperty.Register(nameof(MyLeft), typeof(double), typeof(ResizeCanvasThumb),
        //        new FrameworkPropertyMetadata(0.0,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MyLeft
        {
            get { return (double)GetValue(MyLeftProperty); }
            set { SetValue(MyLeftProperty, value); }
        }
        public static readonly DependencyProperty MyLeftProperty =
            DependencyProperty.Register(nameof(MyLeft), typeof(double), typeof(ResizeCanvasThumb), new PropertyMetadata(0.0));

        public double MyTop
        {
            get { return (double)GetValue(MyTopProperty); }
            set { SetValue(MyTopProperty, value); }
        }
        public static readonly DependencyProperty MyTopProperty =
            DependencyProperty.Register(nameof(MyTop), typeof(double), typeof(ResizeCanvasThumb), new PropertyMetadata(0.0));


    }
}
