using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Globalization;
using Microsoft.VisualBasic;

namespace _20250314
{
    public class ResizeAdorner : Adorner
    {
        #region VisualCollectionで必要        
        protected override int VisualChildrenCount => MyVisualChildren.Count;

        protected override Visual GetVisualChild(int index) => MyVisualChildren[index];
        readonly VisualCollection MyVisualChildren;//表示したい要素を管理する用？
        #endregion VisualCollectionで必要

        private readonly List<Handle> MyHandles = [];
        private readonly Canvas MyCanvas = new();
        private readonly FrameworkElement MyTarget;
        private readonly Handle Top = new();
        private readonly Handle Left = new();
        private readonly Handle Right = new();
        private readonly Handle Bottom = new();
        private readonly Handle TopLeft = new();
        private readonly Handle TopRight = new();
        private readonly Handle BottomLeft = new();
        private readonly Handle BottomRight = new();

        public ResizeAdorner(FrameworkElement adornedElement) : base(adornedElement)
        {
            MyTarget = adornedElement;

            MyVisualChildren = new VisualCollection(this)
            {
                MyCanvas
            };
            MyHandles.Add(Top);
            MyHandles.Add(Left);
            MyHandles.Add(Right);
            MyHandles.Add(Bottom);
            MyHandles.Add(TopLeft);
            MyHandles.Add(TopRight);
            MyHandles.Add(BottomLeft);
            MyHandles.Add(BottomRight);

            if (double.IsNaN(adornedElement.Width))
            {
                adornedElement.Width = adornedElement.ActualWidth;
                adornedElement.Height = adornedElement.ActualHeight;
            }

            //ハンドルの設定
            foreach (Handle item in MyHandles)
            {
                item.Cursor = Cursors.Hand;
                item.DragDelta += Handle_DragDelta;
                MyCanvas.Children.Add(item);
                //装飾ターゲットの親要素がCanvasではない場合は、
                //以下の5個のハンドルは表示しない(外す)
                //これらはターゲットの位置変更に関係するからCanvas以外では不具合の元
                if (MyTarget.Parent.GetType() != typeof(Canvas))
                {
                    MyCanvas.Children.Remove(Top);
                    MyCanvas.Children.Remove(Left);
                    MyCanvas.Children.Remove(TopLeft);
                    MyCanvas.Children.Remove(TopRight);
                    MyCanvas.Children.Remove(BottomLeft);
                }

                item.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(MyHandleSizeProperty) });
                item.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(MyHandleSizeProperty) });
            }

            //ハンドルの表示位置、ターゲットの辺の中間
            MyBind(Top, Handle.MyLeftProperty, WidthProperty);
            MyBind(Left, Handle.MyTopProperty, HeightProperty);
            MyBind(Right, Handle.MyTopProperty, HeightProperty);
            MyBind(Bottom, Handle.MyLeftProperty, WidthProperty);

            //ハンドルの位置とターゲットの縦横サイズをバインド
            MyBind2(Right, Handle.MyLeftProperty, WidthProperty);
            MyBind2(Bottom, Handle.MyTopProperty, HeightProperty);
            MyBind2(TopRight, Handle.MyLeftProperty, WidthProperty);
            MyBind2(BottomLeft, Handle.MyTopProperty, HeightProperty);
            MyBind2(BottomRight, Handle.MyLeftProperty, WidthProperty);
            MyBind2(BottomRight, Handle.MyTopProperty, HeightProperty);

            //ハンドルの位置とハンドルのサイズをバインド
            MyBindHandleSize(Top, Handle.MyTopProperty);
            MyBindHandleSize(Left, Handle.MyLeftProperty);
            MyBindHandleSize(TopLeft, Handle.MyTopProperty);
            MyBindHandleSize(TopLeft, Handle.MyLeftProperty);
            MyBindHandleSize(TopRight, Handle.MyTopProperty);
            MyBindHandleSize(BottomLeft, Handle.MyLeftProperty);

        }

        //ハンドルの位置とターゲットの縦横サイズをバインド
        private void MyBind2(Handle waku, DependencyProperty wakuDp, DependencyProperty targetDp)
        {
            waku.SetBinding(wakuDp, new Binding() { Source = MyTarget, Path = new PropertyPath(targetDp), Mode = BindingMode.TwoWay, Converter = new MyConvNonZero() });

        }
        
        //ハンドルの表示位置、ターゲットの辺の中間
        private void MyBind(Handle waku, DependencyProperty dp, DependencyProperty targetProperty)
        {
            MultiBinding mb;
            mb = new() { Converter = new MyConvHalf() };
            mb.Bindings.Add(new Binding() { Source = MyTarget, Path = new PropertyPath(targetProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyHandleSizeProperty) });
            waku.SetBinding(dp, mb);
        }

        //ハンドルの位置とハンドルのサイズをバインド
        private void MyBindHandleSize(Handle target, DependencyProperty dp)
        {
            target.SetBinding(dp, new Binding() { Source = this, Path = new PropertyPath(MyHandleSizeProperty), Converter = new MyConvReverseSign(), Mode = BindingMode.TwoWay });

        }


        private void Handle_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender == Left)
            {
                e.Handled = HorizontalChange(MyTarget, e.HorizontalChange);
            }
            else if (sender == Top)
            {
                e.Handled = VerticalChange(MyTarget, e.VerticalChange);
            }
            else if (sender == Right)
            {
                Right.MyLeft += e.HorizontalChange;
                e.Handled = true;
            }
            else if (sender == Bottom)
            {
                Bottom.MyTop += e.VerticalChange;
                e.Handled = true;
            }
            else if (TopLeft == sender)
            {
                e.Handled = HorizontalChange(MyTarget, e.HorizontalChange);
                e.Handled = VerticalChange(MyTarget, e.VerticalChange);
            }
            else if (TopRight == sender)
            {
                e.Handled = VerticalChange(MyTarget, e.VerticalChange);
                TopRight.MyLeft += e.HorizontalChange;
            }
            else if (BottomLeft == sender)
            {
                e.Handled = HorizontalChange(MyTarget, e.HorizontalChange);
                BottomLeft.MyTop += e.VerticalChange;
            }
            else if (BottomRight == sender)
            {
                BottomRight.MyLeft += e.HorizontalChange;
                BottomRight.MyTop += e.VerticalChange;
                e.Handled = true;
            }

        }



        public double MyHandleSize
        {
            get { return (double)GetValue(MyHandleSizeProperty); }
            set { SetValue(MyHandleSizeProperty, value); }
        }
        public static readonly DependencyProperty MyHandleSizeProperty =
            DependencyProperty.Register(nameof(MyHandleSize), typeof(double), typeof(ResizeAdorner), new FrameworkPropertyMetadata(20.0));



        protected override Size ArrangeOverride(Size finalSize)
        {
            MyCanvas.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            return base.ArrangeOverride(finalSize);
        }




        /// <summary>
        /// 移動量で水平移動とWidthを変更
        /// ただしWidthが0未満にならないときだけ
        /// </summary>
        /// <param name="target">ターゲット</param>
        /// <param name="horizontalChange">水平移動量</param>
        /// <returns></returns>
        private bool HorizontalChange(FrameworkElement target, double horizontalChange)
        {
            if (target.Width - horizontalChange > 0)
            {
                OffsetLeft(target, horizontalChange);
                target.Width -= horizontalChange;
                return true;
            }
            return false;
        }

        private bool VerticalChange(FrameworkElement target, double verticalChange)
        {
            if (target.Height - verticalChange > 0)
            {
                OffsetTop(target, verticalChange);
                target.Height -= verticalChange;
                return true;
            }
            return false;
        }

        /// <summary>
        /// ターゲットをオフセット移動する
        /// </summary>
        /// <param name="elem">ターゲット</param>
        /// <param name="offset">移動量</param>
        public static void OffsetTop(FrameworkElement elem, double offset)
        {
            Canvas.SetTop(elem, Canvas.GetTop(elem) + offset);
        }
        public static void OffsetLeft(FrameworkElement elem, double offset)
        {
            Canvas.SetLeft(elem, Canvas.GetLeft(elem) + offset);
        }

    }



    #region コンバーター

    /// <summary>
    /// +-を逆にする
    /// </summary>
    public class MyConvReverseSign : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return -(double)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 0未満のときだけ1に変換
    /// </summary>
    public class MyConvNonZero : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double v = (double)value;
            return v < 1 ? 1 : v;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double v = (double)value;
            return v < 1 ? 1 : v;
        }
    }

    /// <summary>
    /// ハンドルの位置を装飾ターゲットの辺の中間にするのに使う
    /// </summary>
    public class MyConvHalf : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var target = (double)values[0];// 装飾ターゲットの辺の長さ
            var handle = (double)values[1];// ハンドルの大きさ
            return (target - handle) / 2.0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion コンバーター



}
