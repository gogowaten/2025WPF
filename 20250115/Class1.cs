using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace _20250115
{
    /// <summary>
    /// 子要素に合わせてサイズが変化するCanvas
    /// ただし、子要素のマージンとパディングは考慮していないし
    /// ArrangeOverrideを理解していないので不具合があるかも
    /// </summary>
    public class ExCanvas : Canvas
    {
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            if (double.IsNaN(Width) && double.IsNaN(Height))
            {
                base.ArrangeOverride(arrangeSize);
                Size resultSize = new();
                foreach (var item in Children.OfType<FrameworkElement>())
                {
                    double x = GetLeft(item) + item.ActualWidth;
                    double y = GetTop(item) + item.ActualHeight;
                    if (resultSize.Width < x) resultSize.Width = x;
                    if (resultSize.Height < y) resultSize.Height = y;
                }
                return resultSize;
            }
            else
            {
                return base.ArrangeOverride(arrangeSize);
            }
        }
    }

    public class ExCanvas2 : Canvas
    {
        private Thumb MyKnob { get; set; }
        private FrameworkElement? MyTargetElement { get; set; }
        public ExCanvas2()
        {
            MyKnob = new Thumb() { Width = 20, Height = 20 };
            Children.Add(MyKnob);
            MyKnob.DragDelta += MyKnob_DragDelta;
        }
        public void SetTargetElement(FrameworkElement element)
        {
            MyTargetElement = element;
            MyKnob.DataContext = MyTargetElement;
            MyKnob.SetBinding(LeftProperty, new Binding(nameof(Width)) { Mode = BindingMode.TwoWay });
            MyKnob.SetBinding(TopProperty, new Binding(nameof(Height)) { Mode = BindingMode.TwoWay });
        }

        private void MyKnob_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is KnobThumb t && MyTargetElement != null)
            {
                MyTargetElement.Width += e.HorizontalChange;
                MyTargetElement.Height += e.VerticalChange;
            }
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            if (double.IsNaN(Width) && double.IsNaN(Height))
            {
                base.ArrangeOverride(arrangeSize);
                Size resultSize = new();
                foreach (var item in Children.OfType<FrameworkElement>())
                {
                    double x = GetLeft(item) + item.ActualWidth;
                    double y = GetTop(item) + item.ActualHeight;
                    if (resultSize.Width < x) resultSize.Width = x;
                    if (resultSize.Height < y) resultSize.Height = y;
                }
                return resultSize;
            }
            else
            {
                return base.ArrangeOverride(arrangeSize);
            }
        }
    }


}
