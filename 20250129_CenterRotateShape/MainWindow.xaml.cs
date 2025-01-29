using System.Globalization;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250129_CenterRotateShape
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            DataContext = this;
            MyShape3.SizeChanged += MyShape3_SizeChanged;
            MyShape5.SizeChanged += MyShape3_SizeChanged;

            MultiBinding mb = new() { Converter = new MyConverterRenderTF() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyAngleProperty), Mode = BindingMode.OneWay });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyBoundsProperty), Mode = BindingMode.OneWay });
            MyShape3.SetBinding(RenderTransformProperty, mb);

            mb = new() { Converter = new MyConverterRenderTF() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyAngleProperty), Mode = BindingMode.OneWay });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyBounds2Property), Mode = BindingMode.OneWay });
            MyShape5.SetBinding(RenderTransformProperty, mb);
        }

        private void MyShape3_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MyBounds = MyShape3.RenderedGeometry.Bounds;
            MyBounds2 = MyShape5.RenderedGeometry.Bounds;
        }


        public double MyAngle
        {
            get { return (double)GetValue(MyAngleProperty); }
            set { SetValue(MyAngleProperty, value); }
        }
        public static readonly DependencyProperty MyAngleProperty =
            DependencyProperty.Register(nameof(MyAngle), typeof(double), typeof(MainWindow), new PropertyMetadata(0.0));

        public Rect MyBounds
        {
            get { return (Rect)GetValue(MyBoundsProperty); }
            set { SetValue(MyBoundsProperty, value); }
        }
        public static readonly DependencyProperty MyBoundsProperty =
            DependencyProperty.Register(nameof(MyBounds), typeof(Rect), typeof(MainWindow), new PropertyMetadata(new Rect()));

        public Rect MyBounds2
        {
            get { return (Rect)GetValue(MyBounds2Property); }
            set { SetValue(MyBounds2Property, value); }
        }
        public static readonly DependencyProperty MyBounds2Property =
            DependencyProperty.Register(nameof(MyBounds2), typeof(Rect), typeof(MainWindow), new PropertyMetadata(new Rect()));

    }



    //角度とRenderBoundsからRotateTransformに変換
    //見た目場違和感のない回転中心点を計算する
    public class MyConverterRenderTF : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double angle = (double)values[0];
            Rect r = (Rect)values[1];
            return new RotateTransform(angle, r.Width / 2.0, r.Height / 2.0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}