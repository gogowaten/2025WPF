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
        private readonly Thumb MyThumb;
        private const double MinimumSize = 1;
        private const double MinimumLocate = 0;
        private const double ThumbSize = 20;
        static RangeThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeThumb), new FrameworkPropertyMetadata(typeof(RangeThumb)));
        }
        public RangeThumb()
        {
            MyThumb = new()
            {
                Width = ThumbSize,
                Height = ThumbSize,
                Cursor = Cursors.SizeNWSE
            };
            MyThumb.DragDelta += Thumb_DragDelta;
            DragDelta += Thumb_DragDelta;
            SetInitialPosition();
        }
        private void SetInitialPosition()
        {
            Canvas.SetLeft(MyThumb, MinimumLocate);
            Canvas.SetTop(MyThumb, MinimumLocate);
            Canvas.SetLeft(this, MinimumLocate);
            Canvas.SetTop(this, MinimumLocate);
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                if (t == MyThumb)
                {
                    //最小サイズ未満にならないようにThumbの移動
                    Canvas.SetLeft(t, Math.Max(MinimumSize, Canvas.GetLeft(t) + e.HorizontalChange));
                    Canvas.SetTop(t, Math.Max(MinimumSize, Canvas.GetTop(t) + e.VerticalChange));
                    e.Handled = true;
                }
                else if (t == this)
                {
                    //最小座標未満にならないように自身の移動
                    Canvas.SetLeft(t, Math.Max(MinimumLocate, Canvas.GetLeft(t) + e.HorizontalChange));
                    Canvas.SetTop(t, Math.Max(MinimumLocate, Canvas.GetTop(t) + e.VerticalChange));
                    e.Handled = true;
                }
            }
        }

        public override void OnApplyTemplate()
        {
            //Templateの中のCanvasを取得してMyThumbを追加とBinding処理
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_Canvas") is Canvas panel)
            {
                panel.Children.Add(MyThumb);

                //バインド
                //自身のサイズをソースにMyThumbの座標をバインド
                MyThumb.DataContext = this;
                _ = MyThumb.SetBinding(Canvas.LeftProperty,
                    new Binding(nameof(Width)) { Mode = BindingMode.TwoWay });
                _ = MyThumb.SetBinding(Canvas.TopProperty,
                    new Binding(nameof(Height)) { Mode = BindingMode.TwoWay });
            }
        }

    }
}
