using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Globalization;
using System.ComponentModel;

namespace _20250320_ResizeHandleAdorner
{
    //ハンドルの表示位置、ターゲットの境界線の上、内側、外側
    public enum HandleLayoutType { Online = 0, Inside, Outside }


    /// <summary>
    /// 要素にリサイズ用のハンドルを装飾表示する
    /// </summary>
    public class ResizeHandleAdorner : Adorner
    {


        #region VisualCollectionで必要        
        protected override int VisualChildrenCount => MyVisualChildren.Count;

        protected override Visual GetVisualChild(int index) => MyVisualChildren[index];
        readonly VisualCollection MyVisualChildren;//表示したい要素を管理する用？
        #endregion VisualCollectionで必要

        private readonly List<HandleThumb> MyHandles = [];//ハンドルリスト、必要なかったかも
        private readonly Canvas MyCanvas = new();// ハンドルを表示する用
        private readonly FrameworkElement MyTarget;// 装飾ターゲット
        private readonly HandleThumb Top = new();// 各ハンドル
        private readonly HandleThumb Left = new();
        private readonly HandleThumb Right = new();
        private readonly HandleThumb Bottom = new();
        private readonly HandleThumb TopLeft = new();
        private readonly HandleThumb TopRight = new();
        private readonly HandleThumb BottomLeft = new();
        private readonly HandleThumb BottomRight = new();

        public ResizeHandleAdorner(FrameworkElement adornedElement) : base(adornedElement)
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

            //通常ではサイズが不確定な要素(TextBlockとかButton)のサイズを決定しておく
            if (double.IsNaN(adornedElement.Width))
            {
                adornedElement.Width = adornedElement.ActualWidth;
                adornedElement.Height = adornedElement.ActualHeight;
            }

            //ハンドルの設定
            foreach (HandleThumb item in MyHandles)
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
            MyBind(Top, HandleThumb.MyLeftProperty, WidthProperty);
            MyBind(Left, HandleThumb.MyTopProperty, HeightProperty);
            MyBind(Right, HandleThumb.MyTopProperty, HeightProperty);
            MyBind(Bottom, HandleThumb.MyLeftProperty, WidthProperty);

            //ハンドルの位置とターゲットの縦横サイズをバインド
            MyBind2(Right, HandleThumb.MyLeftProperty, WidthProperty);
            MyBind2(Bottom, HandleThumb.MyTopProperty, HeightProperty);
            MyBind2(TopRight, HandleThumb.MyLeftProperty, WidthProperty);
            MyBind2(BottomLeft, HandleThumb.MyTopProperty, HeightProperty);
            MyBind2(BottomRight, HandleThumb.MyLeftProperty, WidthProperty);
            MyBind2(BottomRight, HandleThumb.MyTopProperty, HeightProperty);

            //ハンドルの位置とハンドルのサイズをバインド
            MyBindHandleSize(Top, HandleThumb.MyTopProperty);
            MyBindHandleSize(Left, HandleThumb.MyLeftProperty);
            MyBindHandleSize(TopLeft, HandleThumb.MyTopProperty);
            MyBindHandleSize(TopLeft, HandleThumb.MyLeftProperty);
            MyBindHandleSize(TopRight, HandleThumb.MyTopProperty);
            MyBindHandleSize(BottomLeft, HandleThumb.MyLeftProperty);

        }

        //ハンドルの位置とターゲットの縦横サイズをバインド
        private void MyBind2(HandleThumb handle, DependencyProperty handleDp, DependencyProperty targetDp)
        {
            MultiBinding mb = new() { Converter = new MyConvNonZeroMulti(), Mode = BindingMode.OneWay };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyHandleSizeProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyHandleLayoutProperty) });
            mb.Bindings.Add(new Binding() { Source = MyTarget, Path = new PropertyPath(targetDp) });
            handle.SetBinding(handleDp, mb);
        }
        //private void MyBind2(HandleThumb handle, DependencyProperty handleDp, DependencyProperty targetDp)
        //{
        //    handle.SetBinding(handleDp, new Binding()
        //    {
        //        Source = MyTarget,
        //        Path = new PropertyPath(targetDp),
        //        Mode = BindingMode.TwoWay,
        //        Converter = new MyConvNonZero()
        //    });
        //}


        //ハンドルの表示位置、ターゲットの辺の中間
        private void MyBind(HandleThumb handle, DependencyProperty dp, DependencyProperty targetProperty)
        {
            MultiBinding mb;
            mb = new() { Converter = new MyConvHalf() };
            mb.Bindings.Add(new Binding() { Source = MyTarget, Path = new PropertyPath(targetProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyHandleSizeProperty) });
            handle.SetBinding(dp, mb);
        }

        ////ハンドルの位置とハンドルのサイズをバインド
        //private void MyBindHandleSize(HandleThumb target, DependencyProperty dp)
        //{
        //    target.SetBinding(dp, new Binding()
        //    {
        //        Source = this,
        //        Path = new PropertyPath(MyHandleSizeProperty),
        //        Converter = new MyConvReverseSign(),
        //        Mode = BindingMode.TwoWay
        //    });
        //}

        private void MyBindHandleSize(HandleThumb target, DependencyProperty dp)
        {
            var mb = new MultiBinding() { Converter = new MyConvReverseSignMulti() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyHandleSizeProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyHandleLayoutProperty) });
            target.SetBinding(dp, mb);
        }



        #region イベント
        //ハンドルのドラッグ移動により、ターゲットの位置が変更されたときに、その変更量を通知
        public event Action<double>? OnTargetLeftChanged;
        public event Action<double>? OnTargetTopChanged;
        //public event EventHandler<double>? OnTargetLeftChanged;//こっちでもいい
        //public event PropertyChangedEventHandler PropertyChanged;//これは違う感じ
        #endregion イベント

        private bool SetTargetWidth(double horizontalChange)
        {
            if (MyTarget.Width + horizontalChange >= 1.0)
            {
                MyTarget.Width += horizontalChange;
                return true;
            }
            else if (MyTarget.Width > 1.0)
            {
                MyTarget.Width = 1.0;
                return true;
            }
            else { return false; }

        }
        private bool SetTargetHeight(double verticalChange)
        {
            if (MyTarget.Height + verticalChange >= 1.0)
            {
                MyTarget.Height += verticalChange;
                return true;
            }
            else if (MyTarget.Height > 1.0)
            {
                MyTarget.Height = 1.0;
                return true;
            }
            else { return false; }
        }

        //各ハンドルをドラッグ移動したとき
        private void Handle_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender == Left)
            {
                HorizontalChange(MyTarget, e.HorizontalChange);
                e.Handled = true;
            }
            else if (sender == Top)
            {
                VerticalChange(MyTarget, e.VerticalChange);
                e.Handled = true;
            }
            else if (sender == Right)
            {
                SetTargetWidth(e.HorizontalChange);
                e.Handled = true;
            }
            else if (sender == Bottom)
            {
                SetTargetHeight(e.VerticalChange);
                e.Handled = true;
            }
            else if (TopLeft == sender)
            {
                HorizontalChange(MyTarget, e.HorizontalChange);
                VerticalChange(MyTarget, e.VerticalChange);
                e.Handled = true;
            }
            else if (TopRight == sender)
            {
                if (SetTargetWidth(e.HorizontalChange))
                {
                    VerticalChange(MyTarget, e.VerticalChange);
                }
                e.Handled = true;
            }
            else if (BottomLeft == sender)
            {
                if (SetTargetHeight(e.VerticalChange))
                {
                    HorizontalChange(MyTarget, e.HorizontalChange);
                }
                e.Handled = true;
            }
            else if (BottomRight == sender)
            {
                SetTargetWidth(e.HorizontalChange);
                SetTargetHeight(e.VerticalChange);
                e.Handled = true;
            }

        }


        #region 依存関係プロパティ


        public HandleLayoutType MyHandleLayout
        {
            get { return (HandleLayoutType)GetValue(MyHandleLayoutProperty); }
            set { SetValue(MyHandleLayoutProperty, value); }
        }
        public static readonly DependencyProperty MyHandleLayoutProperty =
            DependencyProperty.Register(nameof(MyHandleLayout), typeof(HandleLayoutType), typeof(ResizeHandleAdorner),
                new FrameworkPropertyMetadata(HandleLayoutType.Online,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MyHandleSize
        {
            get { return (double)GetValue(MyHandleSizeProperty); }
            set { SetValue(MyHandleSizeProperty, value); }
        }
        public static readonly DependencyProperty MyHandleSizeProperty =
            DependencyProperty.Register(nameof(MyHandleSize), typeof(double), typeof(ResizeHandleAdorner), new FrameworkPropertyMetadata(20.0));
        #endregion 依存関係プロパティ

        //これがないとCanvasのサイズが0のままになって何も表示されない
        protected override Size ArrangeOverride(Size finalSize)
        {
            MyCanvas.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            return base.ArrangeOverride(finalSize);
        }


        /// <summary>
        /// 移動量で水平移動とWidthを変更
        /// もしwidthが1未満になるような移動量のときは、Widthが1になるように移動量を変換して処理
        /// </summary>
        /// <param name="target">ターゲット</param>
        /// <param name="horizontalChange">水平移動量</param>
        /// <returns></returns>
        private void HorizontalChange(FrameworkElement target, double horizontalChange)
        {
            if (horizontalChange == 0) { return; }
            if (target.Width - horizontalChange > 0)
            {
                target.Width -= horizontalChange;
            }
            else
            {
                horizontalChange = target.Width - 1.0;
                target.Width = 1.0;
            }
            OffsetLeft(target, horizontalChange);
            //リサイズハンドルによりターゲットの位置が変更されたことを知らせる
            OnTargetLeftChanged?.Invoke(horizontalChange);
        }

        private void VerticalChange(FrameworkElement target, double verticalChange)
        {
            if (verticalChange == 0) { return; }
            if (target.Height - verticalChange > 0)
            {
                target.Height -= verticalChange;
            }
            else
            {
                verticalChange = target.Height - 1.0;
                target.Height = 1.0;
            }
            OffsetTop(target, verticalChange);
            OnTargetTopChanged?.Invoke(verticalChange);
        }


        /// <summary>
        /// 要素をオフセット移動する
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

        public void GetHandlesRenderBounds()
        {
            var r = this.RenderSize;
        }


    }



    #region コンバーター
    public class MyConvReverseSignMulti : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var handleSize = (double)values[0];
            var layout = (HandleLayoutType)values[1];
            return layout switch
            {
                HandleLayoutType.Inside => 0.0,
                HandleLayoutType.Outside => -handleSize,
                HandleLayoutType.Online => -handleSize / 2.0,
                _ => (object)0.0,
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
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

    public class MyConvNonZeroMulti : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var handleSize = (double)values[0];
            var layout = (HandleLayoutType)values[1];
            var targetSize = (double)values[2];
            if (targetSize <= 1.0) { targetSize = 1.0; }
            return layout switch
            {
                HandleLayoutType.Inside => targetSize - handleSize,
                HandleLayoutType.Outside => targetSize,
                HandleLayoutType.Online => targetSize - handleSize / 2.0,
                _ => targetSize
            };

            //else
            //{
            //    return layout switch
            //    {
            //        HandleLayoutType.In => 1 - handleSize,
            //        HandleLayoutType.Middle => 1-handleSize/2.0,
            //        HandleLayoutType.Out => 1,
            //        _ => 1
            //    };
            //}

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
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