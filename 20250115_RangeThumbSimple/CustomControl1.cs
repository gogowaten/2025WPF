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

namespace _20250115_RangeThumbSimple
{
    public class RangeThumb : Thumb
    {
        private Thumb MyThumb = new() { Width = 20, Height = 20 };
        static RangeThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeThumb), new FrameworkPropertyMetadata(typeof(RangeThumb)));
        }
        public RangeThumb()
        {
            MyThumb.DragDelta += MyThumb_DragDelta;
        }

        private void MyThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                var left = Canvas.GetLeft(t) + e.HorizontalChange;
                var top = Canvas.GetTop(t) + e.VerticalChange;
                if (left < 0) { left = 0; }
                if (top < 0) { top = 0; }
                Canvas.SetLeft(t, left);
                Canvas.SetTop(t, top);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_MyCanvas") is Canvas panel)
            {
                panel.Children.Add(MyThumb);
                MyThumb.SetBinding(Canvas.LeftProperty, new Binding() { Source = panel, Path = new PropertyPath(WidthProperty), Mode = BindingMode.TwoWay });
                MyThumb.SetBinding(Canvas.TopProperty, new Binding() { Source = panel, Path = new PropertyPath(HeightProperty), Mode = BindingMode.TwoWay });

            }
        }
    }
}
