using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows;

namespace _20250228
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

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            if (double.IsNaN(Width) && double.IsNaN(Height) && IsAutoResize)
            {
                base.ArrangeOverride(arrangeSize);
                Size resultSize = new();
                foreach (var item in Children.OfType<FrameworkElement>())
                {
                    double x = GetLeft(item) + item.ActualWidth;
                    double y = GetTop(item) + item.ActualHeight;
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
    }


}
