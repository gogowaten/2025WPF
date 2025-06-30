using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace _20250629_ColorPicker
{
    public class MarkerAdorner : Adorner
    {
        private VisualCollection MyVisuals { get; set; }
        protected override int VisualChildrenCount => MyVisuals.Count;
        protected override Visual GetVisualChild(int index) => MyVisuals[index];


        public MarkerAdorner(UIElement adornedElement) : base(adornedElement)
        {
        }
    }
}
