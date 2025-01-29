using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

//WPF、図形の回転、PathGeometryで描画した図形の「中央」を中心に回転させるには - 午後わてんのブログ
//https://gogowaten.hatenablog.com/entry/2025/01/29/190555

namespace _20250129_CenterRotateShape
{
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