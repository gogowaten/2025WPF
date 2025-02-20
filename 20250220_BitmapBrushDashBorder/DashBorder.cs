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

namespace _20250220_BitmapBrushDashBorder
{

    public class DashBorder : Control
    {
        static DashBorder()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DashBorder), new FrameworkPropertyMetadata(typeof(DashBorder)));
        }
        public DashBorder()
        {
            DataContext = this;
            Initialized += DashBorder_Initialized;

        }

        private void DashBorder_Initialized(object? sender, EventArgs e)
        {

            var mb = new MultiBinding() { Converter = new MyConverterImageBrush() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyDashThicknessProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyDashColor1Property) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyDashColor2Property) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyMeshProperty) });
            SetBinding(MyImageBrushProperty, mb);
        }



        public int MyMesh
        {
            get { return (int)GetValue(MyMeshProperty); }
            set { SetValue(MyMeshProperty, value); }
        }
        public static readonly DependencyProperty MyMeshProperty =
            DependencyProperty.Register(nameof(MyMesh), typeof(int), typeof(DashBorder), new PropertyMetadata(0));

        public int MyDashThickness
        {
            get { return (int)GetValue(MyDashThicknessProperty); }
            set { SetValue(MyDashThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyDashThicknessProperty =
            DependencyProperty.Register(nameof(MyDashThickness), typeof(int), typeof(DashBorder), new PropertyMetadata(1));

        public Color MyDashColor1
        {
            get { return (Color)GetValue(MyDashColor1Property); }
            set { SetValue(MyDashColor1Property, value); }
        }
        public static readonly DependencyProperty MyDashColor1Property =
            DependencyProperty.Register(nameof(MyDashColor1), typeof(Color), typeof(DashBorder), new PropertyMetadata(Colors.White));


        public Color MyDashColor2
        {
            get { return (Color)GetValue(MyDashColor2Property); }
            set { SetValue(MyDashColor2Property, value); }
        }
        public static readonly DependencyProperty MyDashColor2Property =
            DependencyProperty.Register(nameof(MyDashColor2), typeof(Color), typeof(DashBorder), new PropertyMetadata(Colors.Red));

        public ImageBrush MyImageBrush
        {
            get { return (ImageBrush)GetValue(MyImageBrushProperty); }
            set { SetValue(MyImageBrushProperty, value); }
        }
        public static readonly DependencyProperty MyImageBrushProperty =
            DependencyProperty.Register(nameof(MyImageBrush), typeof(ImageBrush), typeof(DashBorder), new PropertyMetadata(null));


    }


    public class MyConverterImageBrush : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var thick = (int)values[0];
            var iro1 = (Color)values[1];
            var iro2 = (Color)values[2];
            var mesh = (int)values[3];

            if (mesh > 0)
            {
                return BitmapImageBrushMaker.MakeBrush2ColorsDash(mesh, iro1, iro2);
            }
            else if (thick > 0)
            {
                return BitmapImageBrushMaker.MakeBrush2ColorsDash(thick, iro1, iro2);
            }
            else { return new ImageBrush(); }

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class MyConverterStrokeBorderThickness : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //var v = (Thickness)value;
            //return (int)v.Top;

            var v = (int)value;
            return new Thickness(v);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (Thickness)value;
            return (int)v.Top;

            //var v = (int)value;
            //return new Thickness(v);
        }
    }


}
