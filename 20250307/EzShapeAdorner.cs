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

namespace _20250307
{

    public class EzShapeAdorner : Adorner
    {
        private readonly List<Thumb> MyAnchorThumbsList;
        private readonly Canvas MyCanvas;
        readonly VisualCollection MyVisualCollection;
        readonly EzShape MyTarget;
        public EzShapeAdorner(EzShape adornedElement) : base(adornedElement)
        {
            MyCanvas = new();
            MyAnchorThumbsList = [];
            MyTarget = adornedElement;
            MyVisualCollection = new(this) { MyCanvas };

            //アンカーハンドルの作成追加
            InitAnchorThumbs();

            //アンカーハンドルの位置をMyPointsに合わせる
            ResetAnchorLocate();

            Loaded += EzShapeAdorner_Loaded;
        }

        private void EzShapeAdorner_Loaded(object sender, RoutedEventArgs e)
        {
            MyTarget.AAA(GetChildrenRect());
        }

        private void InitAnchorThumbs()
        {
            for (int i = 0; i < MyTarget.MyPoints.Count; i++)
            {
                Thumb anchor = new()
                {
                    Cursor = Cursors.Hand,
                    Height = AnchorSize,
                    Width = AnchorSize,
                    Opacity = 0.3,
                    Background = Brushes.Red,
                    Tag = i
                };
                anchor.DragDelta += Anchor_DragDelta;
                anchor.DragCompleted += Anchor_DragCompleted;
                MyAnchorThumbsList.Insert(i, anchor);
                MyCanvas.Children.Insert(i, anchor);

            }
        }

        private void Anchor_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            MyTarget.AAA(GetChildrenRect());
        }

        private void Anchor_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
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
            DependencyProperty.Register(nameof(AnchorSize), typeof(double), typeof(EzShapeAdorner), new PropertyMetadata(20.0));


        protected override Size ArrangeOverride(Size finalSize)
        {
            MyCanvas.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            return base.ArrangeOverride(finalSize);
        }

        private Rect GetChildrenRect()
        {
            Rect rr = new();
            foreach (var item in MyAnchorThumbsList)
            {
                //Rect r = new(Canvas.GetLeft(item)-item.Width/2.0, Canvas.GetTop(item)-item.Height/2.0, item.Width, item.Height);
                Rect r = new(Canvas.GetLeft(item), Canvas.GetTop(item), item.Width, item.Height);
                rr.Union(r);
            }
            return rr;
        }

        #region VisualCollectionで必要        
        protected override int VisualChildrenCount => MyVisualCollection.Count;

        protected override Visual GetVisualChild(int index) => MyVisualCollection[index];
        #endregion VisualCollectionで必要



        public void ResetAnchorLocate()
        {
            for (int i = 0; i < MyTarget.MyPoints.Count; i++)
            {
                Point p = MyTarget.MyPoints[i];
                Canvas.SetLeft(MyAnchorThumbsList[i], p.X - AnchorSize / 2.0);
                Canvas.SetTop(MyAnchorThumbsList[i], p.Y - AnchorSize / 2.0);
            }
        }


    }


}
