using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250629_ColorPicker
{
    
    public class CustomControl1 : Control
    {
        static CustomControl1()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomControl1), new FrameworkPropertyMetadata(typeof(CustomControl1)));
        }
    }

    public class EllipseThumb : Thumb
    {
        static EllipseThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EllipseThumb), new FrameworkPropertyMetadata(typeof(EllipseThumb)));
        }
        public EllipseThumb()
        {

        }

        #region 依存関係プロパティ

        public Brush MyInsideStroke
        {
            get { return (Brush)GetValue(MyInsideStrokeProperty); }
            set { SetValue(MyInsideStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyInsideStrokeProperty =
            DependencyProperty.Register(nameof(MyInsideStroke), typeof(Brush), typeof(EllipseThumb), new PropertyMetadata(Brushes.Black));

        public Brush MyOutsideStroke
        {
            get { return (Brush)GetValue(MyOutsideStrokeProperty); }
            set { SetValue(MyOutsideStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyOutsideStrokeProperty =
            DependencyProperty.Register(nameof(MyOutsideStroke), typeof(Brush), typeof(EllipseThumb), new PropertyMetadata(Brushes.White));


        #endregion 依存関係プロパティ
    }


    public class MyConvEllipseSizeDown : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double size = ((double)value) - 2.0;
            if (size < 0) { size = 0; }
            return size;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
