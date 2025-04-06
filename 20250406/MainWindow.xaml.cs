using System;
using System.Globalization;
using System.Net.NetworkInformation;
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

namespace _20250406
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Test1();
        }

        private void Test1()
        {
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MyRectGeometryBounds.SetBinding(WidthProperty, new Binding(nameof(Shape.RenderedGeometry.Bounds.Width)) { Source = MyPoly.RenderedGeometry.Bounds });
            MyRectGeometryBounds.SetBinding(HeightProperty, new Binding(nameof(Shape.RenderedGeometry.Bounds.Height)) { Source = MyPoly.RenderedGeometry.Bounds });
            SetBinding(MyGeometryBoundsProperty,new Binding() { Source=MyPoly ,Converter=new MyConvBounds()});


        }


        public Geometry MyGeometry
        {
            get { return (Geometry)GetValue(MyGeometryProperty); }
            set { SetValue(MyGeometryProperty, value); }
        }
        public static readonly DependencyProperty MyGeometryProperty =
            DependencyProperty.Register(nameof(MyGeometry), typeof(Geometry), typeof(MainWindow), new PropertyMetadata(null));

        public Rect MyGeometryBounds
        {
            get { return (Rect)GetValue(MyGeometryBoundsProperty); }
            set { SetValue(MyGeometryBoundsProperty, value); }
        }
        public static readonly DependencyProperty MyGeometryBoundsProperty =
            DependencyProperty.Register(nameof(MyGeometryBounds), typeof(Rect), typeof(MainWindow), new PropertyMetadata(new Rect()));

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var neko = MyGeometryBounds;
            //図形の見た目上での中心を軸に回転するRotateTransformを作成(取得)する
            RotateTransform rotate = GetRotateTransform(MyPoly, 45, 0.5);
            MyPoly.RenderTransform = rotate;


            //変形(回転)後の図形の見た目上でのBoundsを取得
            Rect transformedBounds = GetTransfromedGeometryRenderBounds(MyPoly, rotate);
            //MyRectに反映
            MyRect.Width = transformedBounds.Width;
            MyRect.Height = transformedBounds.Height;
            Canvas.SetLeft(MyRect, transformedBounds.Left);
            Canvas.SetTop(MyRect, transformedBounds.Top);

            Pen myPen = new(Brushes.Transparent, MyPoly.StrokeThickness);
            Rect geometryRenderBounds = MyPoly.RenderedGeometry.GetRenderBounds(myPen);
            Rect これは違う = rotate.TransformBounds(geometryRenderBounds);
        }

        /// <summary>
        /// 図形のRotateTransform作成
        /// </summary>
        /// <param name="target">対象図形</param>
        /// <param name="angle">回転角度</param>
        /// <param name="center">回転軸座標、0から1で指定、0.5で中心、0なら左上</param>
        /// <returns></returns>
        private static RotateTransform GetRotateTransform(Shape target, double angle, double center)
        {
            //Geometry(点座標群)のBoundsから回転軸を求める
            Rect geometryBounds = target.RenderedGeometry.Bounds;
            //0.5と0.5で中心になる。0と0なら左上になる
            double centerX = geometryBounds.Left + (geometryBounds.Width * center);
            double centerY = geometryBounds.Top + (geometryBounds.Height * center);
            return new RotateTransform(angle, centerX, centerY);
        }

        /// <summary>
        /// 変形後の図形の見た目上でのBoundsを取得する
        /// </summary>
        /// <param name="target">対象図形</param>
        /// <param name="henkei">変形に使うTransform</param>
        /// <returns></returns>
        private static Rect GetTransfromedGeometryRenderBounds(Shape target, Transform henkei)
        {
            //元の図形に影響を与えないようにするために、クローンしたGeometryをTransformする
            var clone = target.RenderedGeometry.Clone();
            clone.Transform = henkei;
            return clone.GetRenderBounds(new Pen(Brushes.Transparent, target.StrokeThickness));
        }
    }

    public class MyConvBounds : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var target    =(Shape)value;
            return target.RenderedGeometry.Bounds;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}