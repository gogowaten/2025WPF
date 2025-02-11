using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;


//WPF、マウスでTextBoxのサイズ変更するのにAdorner(装飾者)を使ってみた - 午後わてんのブログ
//https://gogowaten.hatenablog.com/entry/2023/03/05/142526#%E3%82%B3%E3%83%BC%E3%83%89

namespace _20250211
{
    public class EzAdoner : Adorner
    {
        readonly Thumb MyThumb;//サイズ変更用つまみ
        readonly VisualCollection MyVisualChildren;//表示したい要素を管理する用？
        readonly FrameworkElement MyTarget;//装飾する対象要素
        public EzAdoner(FrameworkElement adornedElement) : base(adornedElement)
        {
            MyTarget = adornedElement;
            MyVisualChildren = new VisualCollection(this);
            MyThumb = new Thumb()
            {
                Cursor = Cursors.SizeNWSE,
                Height = 20,
                Width = 20,
                Opacity = 0.5,
                Background = Brushes.Red,
            };
            MyThumb.DragDelta += MyThumb_DragDelta;
            MyVisualChildren.Add(MyThumb);

            //TextBoxなどWidthの既定値がNaNなのを解除する
            MyTarget.Width = MyTarget.DesiredSize.Width;
            MyTarget.Height = MyTarget.DesiredSize.Height;
        }

        private void MyThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            //対象要素のサイズ変更、Thumbより小さくならないようにする
            MyTarget.Width = Math.Max(MyTarget.Width + e.HorizontalChange, MyThumb.Width);
            MyTarget.Height = Math.Max(MyTarget.Height + e.VerticalChange, MyThumb.Height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            //Thumbの表示位置修正、常に対象要素の右下に表示
            MyThumb.Arrange(new Rect(
                MyTarget.Width / 2, MyTarget.Height / 2,
                MyTarget.Width, MyTarget.Height));
            return base.ArrangeOverride(finalSize);
        }

        #region VisualCollectionで必要        
        protected override int VisualChildrenCount => MyVisualChildren.Count;

        protected override Visual GetVisualChild(int index) => MyVisualChildren[index];
        #endregion VisualCollectionで必要
    }
}
