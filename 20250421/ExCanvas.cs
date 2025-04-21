using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;

namespace _20250421
{

    /// <summary>
    /// 子要素全体が収まるようにサイズが自動変化するCanvas
    /// ただし、子要素のマージンとパディングは考慮していないし
    /// ArrangeOverrideを理解していないので不具合があるかも
    /// </summary>
    public class ExCanvas : Canvas
    {
        private bool isAutoResize = true;

        /// <summary>
        /// 自動リサイズの切り替えフラグ
        /// falseからtrueに変更時はInvalidateArrangeを実行してリサイズ
        /// </summary>
        public bool IsAutoResize
        {
            get => isAutoResize;
            set
            {
                if (isAutoResize != value)
                {
                    isAutoResize = value;
                    if (value) { InvalidateArrange(); }
                }
            }
        }

        public ExCanvas()
        {
            Loaded += ExCanvas_Loaded;
        }

        private void ExCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ActualWidthProperty) });
            SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ActualHeightProperty) });
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            //if (double.IsNaN(Width) && double.IsNaN(Height) && IsAutoResize)
            if (IsAutoResize)
            {
                base.ArrangeOverride(arrangeSize);
                Size resultSize = new();
                foreach (var item in Children.OfType<FrameworkElement>())
                {

                    double x = GetLeft(item) + item.ActualWidth;
                    if (double.IsNaN(x)) { x = 0 + item.ActualWidth; }
                    double y = GetTop(item) + item.ActualHeight;
                    if (double.IsNaN(y)) { y = 0 + item.ActualHeight; }
                    if (resultSize.Width < x) resultSize.Width = x;
                    if (resultSize.Height < y) resultSize.Height = y;
                }
                //base.ArrangeOverride(resultSize);
                return resultSize;
            }
            else
            {
                return base.ArrangeOverride(arrangeSize);
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            return base.MeasureOverride(constraint);
        }
    }


}