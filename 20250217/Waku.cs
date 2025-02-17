using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250217
{
    
    public class Waku : Control
    {
        static Waku()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Waku), new FrameworkPropertyMetadata(typeof(Waku)));
        }
        public Waku()
        {
            Initialized += Waku_Initialized;
        }

        private void Waku_Initialized(object? sender, EventArgs e)
        {
            InitialBind();
        }

        private void InitialBind()
        {
           

        }



        public DoubleCollection MyStrokeDashArray
        {
            get { return (DoubleCollection)GetValue(MyStrokeDashArrayProperty); }
            set { SetValue(MyStrokeDashArrayProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeDashArrayProperty =
            DependencyProperty.Register(nameof(MyStrokeDashArray), typeof(DoubleCollection), typeof(Waku), new PropertyMetadata(null));


        public Brush MyStrokeDash
        {
            get { return (Brush)GetValue(MyStrokeDashProperty); }
            set { SetValue(MyStrokeDashProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeDashProperty =
            DependencyProperty.Register(nameof(MyStrokeDash), typeof(Brush), typeof(Waku), new PropertyMetadata(Brushes.Transparent));


    }


    public class MyConverterThickness : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (Thickness)value;
            return v.Top;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
