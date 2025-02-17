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
using static _20250217.DashBorder;

namespace _20250217
{
    public class DashBorder : Control
    {
        public enum DashColor { A = 0, B, C, D, E }
        

        static DashBorder()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DashBorder), new FrameworkPropertyMetadata(typeof(DashBorder)));
        }
        public DashBorder()
        {
            DataContext = this;
            MyStrokeDashArray = [2, 3];
            //SetBinding(MyBorderThicknessProperty, new Binding(nameof(BorderThickness)));
            SetBinding(BorderThicknessProperty, new Binding(nameof(MyBorderThickness)));
                        
        }


        #region 依存関係プロパティ


        public DoubleCollection MyStrokeDashArray
        {
            get { return (DoubleCollection)GetValue(MyStrokeDashArrayProperty); }
            set { SetValue(MyStrokeDashArrayProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeDashArrayProperty =
            DependencyProperty.Register(nameof(MyStrokeDashArray), typeof(DoubleCollection), typeof(DashBorder), new PropertyMetadata(null));



        public Thickness MyBorderThickness
        {
            get { return (Thickness)GetValue(MyBorderThicknessProperty); }
            set { SetValue(MyBorderThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyBorderThicknessProperty =
            DependencyProperty.Register(nameof(MyBorderThickness), typeof(Thickness), typeof(DashBorder), new PropertyMetadata(new Thickness(1.0)));

        #region Brashes

        public Brush MyDashBrushE
        {
            get { return (Brush)GetValue(MyDashBrushEProperty); }
            set { SetValue(MyDashBrushEProperty, value); }
        }
        public static readonly DependencyProperty MyDashBrushEProperty =
            DependencyProperty.Register(nameof(MyDashBrushE), typeof(Brush), typeof(DashBorder), new PropertyMetadata(Brushes.Orange));
        
        public Brush MyDashBrushD
        {
            get { return (Brush)GetValue(MyDashBrushDProperty); }
            set { SetValue(MyDashBrushDProperty, value); }
        }
        public static readonly DependencyProperty MyDashBrushDProperty =
            DependencyProperty.Register(nameof(MyDashBrushD), typeof(Brush), typeof(DashBorder), new PropertyMetadata(Brushes.Green));
        
        public Brush MyDashBrushC
        {
            get { return (Brush)GetValue(MyDashBrushCProperty); }
            set { SetValue(MyDashBrushCProperty, value); }
        }
        public static readonly DependencyProperty MyDashBrushCProperty =
            DependencyProperty.Register(nameof(MyDashBrushC), typeof(Brush), typeof(DashBorder), new PropertyMetadata(Brushes.Blue));

        public Brush MyDashBrushB
        {
            get { return (Brush)GetValue(MyDashBrushBProperty); }
            set { SetValue(MyDashBrushBProperty, value); }
        }
        public static readonly DependencyProperty MyDashBrushBProperty =
            DependencyProperty.Register(nameof(MyDashBrushB), typeof(Brush), typeof(DashBorder), new PropertyMetadata(Brushes.Red));

        public Brush MyDashBrushA
        {
            get { return (Brush)GetValue(MyDashBrushAProperty); }
            set { SetValue(MyDashBrushAProperty, value); }
        }
        public static readonly DependencyProperty MyDashBrushAProperty =
            DependencyProperty.Register(nameof(MyDashBrushA), typeof(Brush), typeof(DashBorder), new PropertyMetadata(Brushes.Transparent));


        public Brush MyLowerBrush
        {
            get { return (Brush)GetValue(MyLowerBrushProperty); }
            set { SetValue(MyLowerBrushProperty, value); }
        }
        public static readonly DependencyProperty MyLowerBrushProperty =
            DependencyProperty.Register(nameof(MyLowerBrush), typeof(Brush), typeof(DashBorder), new PropertyMetadata(Brushes.WhiteSmoke));
        #endregion Brashes


        public DashColor MyDashColorType
        {
            get { return (DashColor)GetValue(MyDashColorTypeProperty); }
            set { SetValue(MyDashColorTypeProperty, value); }
        }
        public static readonly DependencyProperty MyDashColorTypeProperty =
            DependencyProperty.Register(nameof(MyDashColorType), typeof(DashColor), typeof(DashBorder), new PropertyMetadata(DashColor.A));
        #endregion 依存関係プロパティ

     
    }
    public class MyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, System.Type targetType, object parameter, CultureInfo culture)
            {
            var type = (DashColor)values[0];
            var iro1 = (Brush)values[1];
            var iro2 = (Brush)values[2];
            var iro3 = (Brush)values[3];
            var iro4 = (Brush)values[4];
            var iro5 = (Brush)values[5];
            return type switch
            {
                DashColor.A => iro1,
                DashColor.B => iro2,
                DashColor.C => iro3,
                DashColor.D => iro4,
                DashColor.E => iro5,
                _ => iro1
            };
        }

        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TextThumb : Thumb
    {
        static TextThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextThumb), new FrameworkPropertyMetadata(typeof(TextThumb)));
        }
        public TextThumb()
        {

        }


        public string MyText
        {
            get { return (string)GetValue(MyTextProperty); }
            set { SetValue(MyTextProperty, value); }
        }
        public static readonly DependencyProperty MyTextProperty =
            DependencyProperty.Register(nameof(MyText), typeof(string), typeof(TextThumb), new PropertyMetadata(string.Empty));

    }

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
