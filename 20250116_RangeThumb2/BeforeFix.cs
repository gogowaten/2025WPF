using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;

namespace _20250116_RangeThumb2
{
    internal class BeforeFix:Thumb
    {

        private Thumb MyThumbTopLeft { get; set; }
        private Thumb MyThumbTop { get; set; }
        private Thumb MyThumbTopRight { get; set; }
        private Thumb MyThumbLeft { get; set; }
        private Thumb MyThumbRight { get; set; }
        private Thumb MyThumbBottomLeft { get; set; }
        private Thumb MyThumbBottom { get; set; }
        private Thumb MyThumbBottomRight { get; set; }

        static BeforeFix()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BeforeFix), new FrameworkPropertyMetadata(typeof(BeforeFix)));
        }
        public BeforeFix()
        {
            MyThumbTopLeft = new() { Width = 20, Height = 20 };
            MyThumbTop = new() { Width = 20, Height = 20 };
            MyThumbTopRight = new() { Width = 20, Height = 20 };
            MyThumbRight = new() { Width = 20, Height = 20 };
            MyThumbLeft = new() { Width = 20, Height = 20 };
            MyThumbBottomLeft = new() { Width = 20, Height = 20 };
            MyThumbBottom = new() { Width = 20, Height = 20 };
            MyThumbBottomRight = new() { Width = 20, Height = 20 };

            DragDelta += MyThumb_DragDelta;
            MyThumbTopLeft.DragDelta += MyThumb_DragDelta;
            MyThumbTop.DragDelta += MyThumb_DragDelta;
            MyThumbTopRight.DragDelta += MyThumb_DragDelta;
            MyThumbRight.DragDelta += MyThumb_DragDelta;
            MyThumbLeft.DragDelta += MyThumb_DragDelta;
            MyThumbBottomLeft.DragDelta += MyThumb_DragDelta;
            MyThumbBottom.DragDelta += MyThumb_DragDelta;
            MyThumbBottomRight.DragDelta += MyThumb_DragDelta;
        }


        private void MyThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double addHorWidth = Width + e.HorizontalChange;
            double subHorWidth = Width - e.HorizontalChange;
            double addVerHeight = Height + e.VerticalChange;
            double subVerHeight = Height - e.VerticalChange;
            double left = Canvas.GetLeft(this) + e.HorizontalChange;
            double top = Canvas.GetTop(this) + e.VerticalChange;

            if (sender == MyThumbBottom)
            {
                Height = Math.Max(1, addVerHeight);
            }
            else if (sender == MyThumbRight)
            {
                Width = Math.Max(1, addHorWidth);
            }
            else if (sender == MyThumbBottomRight)
            {
                Width = Math.Max(1, addHorWidth);
                Height = Math.Max(1, addVerHeight);
            }
            else if (sender == MyThumbLeft)
            {
                Width = Math.Max(1, subHorWidth);
                Canvas.SetLeft(this, left);
            }
            else if (sender == MyThumbTop)
            {
                Height = Math.Max(1, subVerHeight);
                Canvas.SetTop(this, top);
            }
            else if (sender == MyThumbBottomLeft)
            {
                Width = Math.Max(1, subHorWidth);
                Height = Math.Max(1, addVerHeight);
                Canvas.SetLeft(this, left);
            }
            else if (sender == MyThumbTopRight)
            {
                Width = Math.Max(1, addHorWidth);
                Height = Math.Max(1, subVerHeight);
                Canvas.SetTop(this, top);
            }
            else if (sender == MyThumbTopLeft)
            {
                Width = Math.Max(1, subHorWidth);
                Height = Math.Max(1, subVerHeight);
                Canvas.SetLeft(this, left);
                Canvas.SetTop(this, top);
            }

            else if (sender == this)
            {
                Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
                Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
            }
            e.Handled = true;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_canvas") is Canvas panel)
            {
                panel.Children.Add(MyThumbTopLeft);

                panel.Children.Add(MyThumbTop);
                MyThumbTop.DataContext = this;
                MyThumbTop.SetBinding(Canvas.LeftProperty, new Binding(nameof(Width)) { Mode = BindingMode.TwoWay, Converter = new MyConverterHalf() });

                panel.Children.Add(MyThumbTopRight);
                MyThumbTopRight.DataContext = this;
                MyThumbTopRight.SetBinding(Canvas.LeftProperty, new Binding(nameof(Width)) { Mode = BindingMode.TwoWay });

                panel.Children.Add(MyThumbRight);
                MyThumbRight.DataContext = this;
                MyThumbRight.SetBinding(Canvas.LeftProperty, new Binding(nameof(Width)) { Mode = BindingMode.TwoWay });
                MyThumbRight.SetBinding(Canvas.TopProperty, new Binding(nameof(Height)) { Mode = BindingMode.TwoWay, Converter = new MyConverterHalf() });

                panel.Children.Add(MyThumbLeft);
                MyThumbLeft.DataContext = this;
                MyThumbLeft.SetBinding(Canvas.TopProperty, new Binding(nameof(Height)) { Mode = BindingMode.TwoWay, Converter = new MyConverterHalf() });

                panel.Children.Add(MyThumbBottomLeft);
                MyThumbBottomLeft.DataContext = this;
                MyThumbBottomLeft.SetBinding(Canvas.TopProperty, new Binding(nameof(Height)) { Mode = BindingMode.TwoWay });

                panel.Children.Add(MyThumbBottom);
                MyThumbBottom.DataContext = this;
                MyThumbBottom.SetBinding(Canvas.TopProperty, new Binding(nameof(Height)) { Mode = BindingMode.TwoWay });
                MyThumbBottom.SetBinding(Canvas.LeftProperty, new Binding(nameof(Width)) { Mode = BindingMode.TwoWay, Converter = new MyConverterHalf() });

                panel.Children.Add(MyThumbBottomRight);
                MyThumbBottomRight.DataContext = this;
                MyThumbBottomRight.SetBinding(Canvas.LeftProperty, new Binding(nameof(Width)) { Mode = BindingMode.TwoWay });
                MyThumbBottomRight.SetBinding(Canvas.TopProperty, new Binding(nameof(Height)) { Mode = BindingMode.TwoWay });
            }
        }
    }
}
