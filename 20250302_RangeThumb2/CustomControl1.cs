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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250302_RangeThumb2
{
    public class RangeThumb2 : Thumb
    {
        static RangeThumb2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeThumb2), new FrameworkPropertyMetadata(typeof(RangeThumb2)));
        }
        public RangeThumb2()
        {
            DragDelta += RangeThumb2_DragDelta;
        }

        private void RangeThumb2_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
            Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
            e.Handled = true;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("handle") is Thumb handle)
            {
                handle.DragDelta += Handle_DragDelta;
            }
            else
            {
                throw new ArgumentNullException("ハンドルが見つからんかった");
            }
        }

        //ハンドルの移動でCanvasのサイズを変更
        private void Handle_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Width = Math.Max(1, Width + e.HorizontalChange);
            Height = Math.Max(1, Height + e.VerticalChange);
            e.Handled = true;
        }
    }
}
