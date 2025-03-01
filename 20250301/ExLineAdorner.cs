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
using System.Windows.Data;
using _20250301;

namespace _20250301
{
    public class EzLineAdorner : Adorner
    {
        //readonly Thumb MyThumb;//サイズ変更用つまみ
        private readonly List<AnchorEllipseThumb> MyAnchorList;
        private readonly Canvas MyCanvas;
        readonly VisualCollection MyVisualChildren;//表示したい要素を管理する用？
        readonly EzLine MyTarget;//装飾する対象要素
        public EzLineAdorner(EzLine adornedElement) : base(adornedElement)
        {
            MyCanvas = new();
            MyAnchorList = [];
            MyTarget = adornedElement;
            MyVisualChildren = new VisualCollection(this) { MyCanvas };


            for (int i = 0; i < MyTarget.MyPoints.Count; i++)
            {
                MakeKnobThumb2(i);
            }

            ResetAnchorLocate();
            Loaded += EzLineAdorner_Loaded;
        }

        private void EzLineAdorner_Loaded(object sender, RoutedEventArgs e)
        {
            MyBind();
        }

        private void MyBind()
        {
            Binding bind = new() { Source = this, Path = new PropertyPath(AnchorSizeProperty) };
            foreach (var item in MyAnchorList)
            {
                item.SetBinding(AnchorThumb.MyOutSizeProperty, bind);
                item.SetBinding(AnchorThumb.MyInSizeProperty, bind);
            }
        }
        private void ResetAnchorLocate()
        {
            for (int i = 0; i < MyTarget.MyPoints.Count; i++)
            {
                Point p = MyTarget.MyPoints[i];
                Canvas.SetLeft(MyAnchorList[i], p.X - AnchorSize / 2.0);
                Canvas.SetTop(MyAnchorList[i], p.Y - AnchorSize / 2.0);
            }
        }


        private AnchorEllipseThumb MakeKnobThumb2(int id)
        {
            AnchorEllipseThumb anchor = new()
            {
                Cursor = Cursors.Hand,
                Height = AnchorSize,
                Width = AnchorSize,
                //Opacity = 0.3,
                //Background = Brushes.Red,
                Tag = id
            };
            anchor.DragDelta += MyThumb_DragDelta;
            MyAnchorList.Insert(id, anchor);
            MyCanvas.Children.Insert(id, anchor);
            return anchor;
        }


        private void MyThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is AnchorEllipseThumb t)
            {
                int id = (int)t.Tag;
                Point po = MyTarget.MyPoints[id];
                double left = po.X + e.HorizontalChange;
                double top = po.Y + e.VerticalChange;

                Point npo = new(left, top);
                MyTarget.MyPoints[id] = npo;

                Canvas.SetLeft(t, left - AnchorSize / 2.0);
                Canvas.SetTop(t, top - AnchorSize / 2.0);
                e.Handled = true;
            }
        }


        public double AnchorSize
        {
            get { return (double)GetValue(AnchorSizeProperty); }
            set { SetValue(AnchorSizeProperty, value); }
        }
        public static readonly DependencyProperty AnchorSizeProperty =
            DependencyProperty.Register(nameof(AnchorSize), typeof(double), typeof(EzLineAdorner), new PropertyMetadata(31.0));


        protected override Size ArrangeOverride(Size finalSize)
        {

            MyCanvas.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            return base.ArrangeOverride(finalSize);
        }


        #region VisualCollectionで必要        
        protected override int VisualChildrenCount => MyVisualChildren.Count;

        protected override Visual GetVisualChild(int index) => MyVisualChildren[index];
        #endregion VisualCollectionで必要




    }
}