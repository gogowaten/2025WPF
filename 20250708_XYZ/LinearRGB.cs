using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _20250708_XYZ
{
    public enum CurrentCCC { None = 0, RGB, LinearRGB}
    
    
    // sRGB
    public class LinearRGB : DependencyObject
    {
        public LinearRGB() { }


        public byte R
        {
            get { return (byte)GetValue(RProperty); }
            set { SetValue(RProperty, value); }
        }
        public static readonly DependencyProperty RProperty =
            DependencyProperty.Register(nameof(R), typeof(byte), typeof(LinearRGB), new PropertyMetadata((byte)0));

        public byte G
        {
            get { return (byte)GetValue(GProperty); }
            set { SetValue(GProperty, value); }
        }
        public static readonly DependencyProperty GProperty =
            DependencyProperty.Register(nameof(G), typeof(byte), typeof(LinearRGB), new PropertyMetadata((byte)0));

        public byte B
        {
            get { return (byte)GetValue(BProperty); }
            set { SetValue(BProperty, value); }
        }
        public static readonly DependencyProperty BProperty =
            DependencyProperty.Register(nameof(B), typeof(byte), typeof(LinearRGB), new PropertyMetadata((byte)0));


        public double LR
        {
            get { return (double)GetValue(LRProperty); }
            set { SetValue(LRProperty, value); }
        }
        public static readonly DependencyProperty LRProperty =
            DependencyProperty.Register(nameof(LR), typeof(double), typeof(LinearRGB), new PropertyMetadata(0.0));

        public double LG
        {
            get { return (double)GetValue(LGProperty); }
            set { SetValue(LGProperty, value); }
        }
        public static readonly DependencyProperty LGProperty =
            DependencyProperty.Register(nameof(LG), typeof(double), typeof(LinearRGB), new PropertyMetadata(0.0));

        public double LB
        {
            get { return (double)GetValue(LBProperty); }
            set { SetValue(LBProperty, value); }
        }
        public static readonly DependencyProperty LBProperty =
            DependencyProperty.Register(nameof(LB), typeof(double), typeof(LinearRGB), new PropertyMetadata(0.0));

    }
}
