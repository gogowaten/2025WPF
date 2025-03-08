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
        public readonly List<Thumb> MyAnchorThumbsList;
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
                MyAnchorThumbsList.Insert(i, anchor);
                MyCanvas.Children.Insert(i, anchor);

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
