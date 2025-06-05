using System.Globalization;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _20250605
{
    public partial class MainWindow : Window
    {
        public List<Thumb> MyThumbs { get; set; } = [];

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;


            MyPoints = [new Point(100, 100), new Point(300, 100), new Point(100, 300)];

            Polyline line = new()
            {
                Points = MyPoints,
                Stroke = Brushes.DodgerBlue,
                StrokeThickness = 2
            };
            MyCanvas.Children.Add(line);

            // Thumbの初期化
            for (int i = 0; i < MyPoints.Count; i++)
            {
                // どの点座標と連携するかの識別のために、
                // 連携するPointsのインデックスをTagプロパティに入れておく
                Thumb t = new() { Width = 20, Height = 20, Tag = i };
                _ = MyCanvas.Children.Add(t);
                Canvas.SetLeft(t, MyPoints[i].X);
                Canvas.SetTop(t, MyPoints[i].Y);

                // ドラッグ移動
                t.DragDelta += (a, b) =>
                {
                    int ii = (int)t.Tag;// 識別用インデックスを取り出す
                    double x = MyPoints[ii].X + b.HorizontalChange;
                    double y = MyPoints[ii].Y + b.VerticalChange;
                    MyPoints[ii] = new Point(x, y);
                    Canvas.SetLeft(t, x);
                    Canvas.SetTop(t, y);

                    b.Handled = true;
                };
            }

            // 3点座標と角度にバインド、コンバーター付き
            _ = SetBinding(MyAngleProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty), Converter = new MyConvABCAngle() });
        }


        // 3点座標用
        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(MainWindow), new PropertyMetadata(new PointCollection()));

        // 角度表示用
        public double MyAngle
        {
            get { return (double)GetValue(MyAngleProperty); }
            set { SetValue(MyAngleProperty, value); }
        }
        public static readonly DependencyProperty MyAngleProperty =
            DependencyProperty.Register(nameof(MyAngle), typeof(double), typeof(MainWindow), new PropertyMetadata(0.0));




        /// <summary>
        /// 3点（a, b, c）から点bを頂点とする角度（∠abc）を度で返す
        /// </summary>
        /// <param name="a">始点座標</param>
        /// <param name="b">頂点座標</param>
        /// <param name="c">終点座標</param>
        /// <returns>角度（度）</returns>
        public static double CalculateAngleABC(Vector2 a, Vector2 b, Vector2 c)
        {
            // ベクトルba, bcを求める
            Vector2 ba = a - b;
            Vector2 bc = c - b;

            // ベクトルの大きさ
            double lenBA = ba.Length();
            double lenBC = bc.Length();

            if (lenBA == 0 || lenBC == 0) return 0; // 0除算防止

            // 内積
            double dot = Vector2.Dot(ba, bc);

            // 余弦定理で角度（ラジアン）
            double cosTheta = dot / (lenBA * lenBC);
            // 計算誤差対策で範囲を制限
            cosTheta = Math.Clamp(cosTheta, -1.0, 1.0);

            double angleRad = Math.Acos(cosTheta);

            // 度に変換
            return angleRad * 180.0 / Math.PI;
        }

    }



    // 3点座標abcをを角度にするコンバーター
    public class MyConvABCAngle : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var abc = (PointCollection)value;
            Point a = abc[0];
            Point b = abc[1];
            Point c = abc[2];
            Vector2 va = new((float)a.X, (float)a.Y);
            Vector2 vb = new((float)b.X, (float)b.Y);
            Vector2 vc = new((float)c.X, (float)c.Y);
            double angle = MainWindow.CalculateAngleABC(va, vb, vc);
            return angle;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}