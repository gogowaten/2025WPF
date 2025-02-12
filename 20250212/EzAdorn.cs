using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace _20250212
{
    public class EzAdorn : Adorner
    {
        //readonly Thumb MyThumb;//サイズ変更用つまみ
        private readonly List<Thumb> MyKnobList;
        private readonly Canvas MyCanvas;
        readonly VisualCollection MyVisualChildren;//表示したい要素を管理する用？
        readonly EzLine MyTarget;//装飾する対象要素
        public EzAdorn(EzLine adornedElement) : base(adornedElement)
        {
            MyCanvas = new();
            MyTarget = adornedElement;
            MyVisualChildren = new VisualCollection(this)
            {
                MyCanvas
            };
            MyKnobList = [];

            PointCollection pc = MyTarget.MyPoints;
            for (int i = 0; i < pc.Count; i++)
            {
                Thumb knob = MakeKnobThumb();
                knob.Tag = i;
                knob.DragDelta += MyThumb_DragDelta;
                MyKnobList.Add(knob);
                MyCanvas.Children.Add(knob);
            }

            ResetAnchorLocate();

        }

        private void ResetAnchorLocate()
        {
            for (int i = 0; i < MyTarget.MyPoints.Count; i++)
            {
                Point p = MyTarget.MyPoints[i];
                Canvas.SetLeft(MyKnobList[i], p.X);
                Canvas.SetTop(MyKnobList[i], p.Y);
            }
        }


        private static Thumb MakeKnobThumb()
        {
            Thumb knob = new()
            {
                Cursor = Cursors.SizeNWSE,
                Height = 20,
                Width = 20,
                Opacity = 0.5,
                Background = Brushes.Red,
            };
            return knob;
        }

        private void MyThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                int id = (int)t.Tag;
                Point po = MyTarget.MyPoints[id];
                double left = po.X + e.HorizontalChange;
                double top = po.Y + e.VerticalChange;
                Point npo = new(left, top);
                MyTarget.MyPoints[id] = npo;
                Canvas.SetLeft(t, left);
                Canvas.SetTop(t, top);
                e.Handled = true;
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            //MyCanvas.Arrange(new Rect(0, 0, 1,1));
            MyCanvas.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            return base.ArrangeOverride(finalSize);
        }


        #region VisualCollectionで必要        
        protected override int VisualChildrenCount => MyVisualChildren.Count;

        protected override Visual GetVisualChild(int index) => MyVisualChildren[index];
        #endregion VisualCollectionで必要
    }
}
