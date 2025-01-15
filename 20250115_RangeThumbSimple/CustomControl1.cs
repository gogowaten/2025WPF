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
        private const double MinimumSize = 1;
        private const double ZeroLocate = 0;
        static RangeThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeThumb), new FrameworkPropertyMetadata(typeof(RangeThumb)));
        }
        public RangeThumb()
        {
            MyThumb.DragDelta += MyThumb_DragDelta;
            DragDelta += RangeThumb_DragDelta;
            Canvas.SetLeft(MyThumb, 0);
            Canvas.SetTop(MyThumb, 0);
            Canvas.SetLeft(this, 0);
            Canvas.SetTop(this, 0);
        }


        private void RangeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                //マイナス座標にならないように自身の移動
                Canvas.SetLeft(t, Math.Max(ZeroLocate, Canvas.GetLeft(t) + e.HorizontalChange));
                Canvas.SetTop(t, Math.Max(ZeroLocate, Canvas.GetTop(t) + e.VerticalChange));
                e.Handled = true;
            }
        }

        private void MyThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                //1未満にならないようにThumbの移動
                Canvas.SetLeft(t, Math.Max(MinimumSize, Canvas.GetLeft(t) + e.HorizontalChange));
                Canvas.SetTop(t, Math.Max(MinimumSize, Canvas.GetTop(t) + e.VerticalChange));
                e.Handled = true;
            }
        }

        public override void OnApplyTemplate()
        {
            //Templateの中のCanvasを取得してMyThumbを追加とBinding処理
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_Canvas") is Canvas panel)
            {
                panel.Children.Add(MyThumb);
                //Thumbの位置が自身の右下になるようにBinding
                _ = MyThumb.SetBinding(Canvas.LeftProperty, new Binding()
                {
                    Source = this,
                    Path = new PropertyPath(WidthProperty),
                    Mode = BindingMode.TwoWay
                });

                _ = MyThumb.SetBinding(Canvas.TopProperty, new Binding()
                {
                    Source = this,
                    Path = new PropertyPath(HeightProperty),
                    Mode = BindingMode.TwoWay
                });
            }
        }

    }
}
