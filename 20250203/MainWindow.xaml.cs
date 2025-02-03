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

namespace _20250203
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MyTest()
        {
           //var neko = nemo3.RenderSize;
           //var geoBounds = nemo3.RenderedGeometry.Bounds;
           // Pen pen=new(nemo3.Stroke,nemo3.StrokeThickness);
           // var penBounds = nemo3.RenderedGeometry.GetRenderBounds(pen);
           //var tf=(RotateTransform) nemo3.RenderTransform;
           // var bounds = tf.TransformBounds(geoBounds);
           // var penTFBounds = tf.TransformBounds(penBounds);
           // Geometry geo = nemo3.Data;
           //var geoTF= geo.Transform;
           // Geometry clone = geo.Clone();
           // clone.Transform = tf;
           // var geoTFBounds = clone.Bounds;
           // var geoTFPenBounds = clone.GetRenderBounds(pen);
        }

        public Rect MyBounds
        {
            get { return (Rect)GetValue(MyBoundsProperty); }
            set { SetValue(MyBoundsProperty, value); }
        }
        public static readonly DependencyProperty MyBoundsProperty =
            DependencyProperty.Register(nameof(MyBounds), typeof(Rect), typeof(MainWindow), new PropertyMetadata(Rect.Empty));


        public Rect MyBounds2
        {
            get { return (Rect)GetValue(MyBounds2Property); }
            set { SetValue(MyBounds2Property, value); }
        }
        public static readonly DependencyProperty MyBounds2Property =
            DependencyProperty.Register(nameof(MyBounds2), typeof(Rect), typeof(MainWindow), new PropertyMetadata(Rect.Empty));

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MyTest();
        }
    }
}