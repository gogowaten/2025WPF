using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace _20250116_RangeThumb2
{

    public class CanvasThumb : Thumb
    {
        private readonly List<Thumb> thumbs;

        static CanvasThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CanvasThumb), new FrameworkPropertyMetadata(typeof(CanvasThumb)));
        }
        public CanvasThumb()
        {
            thumbs = [];
            for (int i = 0; i < 8; i++)
            {
                Thumb t = new() { Width = 20, Height = 20, DataContext = this };
                t.DragDelta += Thumb_DragDelta;
                thumbs.Add(t);
            }
            DragDelta += Thumb_DragDelta;

        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            //0 TopLeft        //1 Top           //2 TopRight
            //3 Left           //4 Right
            //5 BottomLeft     //6 Bottom        //7 BottomRight
            double addHorWidth = Width + e.HorizontalChange;
            double subHorWidth = Width - e.HorizontalChange;
            double addVerHeight = Height + e.VerticalChange;
            double subVerHeight = Height - e.VerticalChange;
            double left = Canvas.GetLeft(this) + e.HorizontalChange;
            double top = Canvas.GetTop(this) + e.VerticalChange;

            if (sender == thumbs[6])//Bottom
            {
                Height = Math.Max(1, addVerHeight);
            }
            else if (sender == thumbs[4])//Right
            {
                Width = Math.Max(1, addHorWidth);
            }
            else if (sender == thumbs[7])//BottomRight
            {
                Width = Math.Max(1, addHorWidth);
                Height = Math.Max(1, addVerHeight);
            }
            else if (sender == thumbs[3])//Left
            {
                Width = Math.Max(1, subHorWidth);
                Canvas.SetLeft(this, left);
            }
            else if (sender == thumbs[1])//Top
            {
                Height = Math.Max(1, subVerHeight);
                Canvas.SetTop(this, top);
            }
            else if (sender == thumbs[5])//BottomLeft
            {
                Width = Math.Max(1, subHorWidth);
                Height = Math.Max(1, addVerHeight);
                Canvas.SetLeft(this, left);
            }
            else if (sender == thumbs[2])//TopRight
            {
                Width = Math.Max(1, addHorWidth);
                Height = Math.Max(1, subVerHeight);
                Canvas.SetTop(this, top);
            }
            else if (sender == thumbs[0])//TopLeft
            {
                Width = Math.Max(1, subHorWidth);
                Height = Math.Max(1, subVerHeight);
                Canvas.SetLeft(this, left);
                Canvas.SetTop(this, top);
            }

            else if (sender == this)//CanvasThumb
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
                for (int i = 0; i < thumbs.Count; i++)
                {
                    panel.Children.Add(thumbs[i]);
                    if (i == 1 || i == 6)// Top, Bottom
                    {
                        thumbs[i].SetBinding(Canvas.LeftProperty, new Binding(nameof(Width)) { Mode = BindingMode.TwoWay, Converter = new MyConverterHalf() });
                    }
                    if (i == 3 || i == 4)// Left, Right
                    {
                        thumbs[i].SetBinding(Canvas.TopProperty, new Binding(nameof(Height)) { Mode = BindingMode.TwoWay, Converter = new MyConverterHalf() });
                    }
                    if (i == 2 || i == 4 || i == 7) // TopRight, Right, BottomRight
                    {
                        thumbs[i].SetBinding(Canvas.LeftProperty, new Binding(nameof(Width)) { Mode = BindingMode.TwoWay });
                    }
                    if (i == 5 || i == 6 || i == 7) // BottomLeft, Bottom, BottomRight
                    {
                        thumbs[i].SetBinding(Canvas.TopProperty, new Binding(nameof(Height)) { Mode = BindingMode.TwoWay });
                    }
                }
            }
        }
    }

    public class MyConverterHalf : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value / 2.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



}
