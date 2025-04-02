using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace _20250401_01
{
    public partial class MainWindow : Window
    {
        //private Rect MyRedBounds;
        public MainWindow()
        {
            InitializeComponent();

            Test1();
            Test2();
            Test3();
        }

        //子要素、孫要素ともに回転しているとき
        //親要素から見た孫要素のBounds取得
        private void Test3()
        {
            //位置の取得A、孫要素自体のTransform後のBounds
            Size redSize = new(MyRed3.Width, MyRed3.Height);
            Rect redZeroBounds = MyRed3.RenderTransform.TransformBounds(new Rect(redSize));
            
            //位置の取得B、子要素のTransformを使った孫要素のBounds
            Rect redRect = new(GetPoint(MyRed3), redSize);
            Rect canvasTFBounds = MyCanvas3.RenderTransform.TransformBounds(redRect);

            //最終的な位置の取得は、AとBの合成(offset)
            Point topLeft = canvasTFBounds.TopLeft;
            topLeft.Offset(redZeroBounds.X, redZeroBounds.Y);

            //サイズ取得は、子要素と孫要素のTransformを合成したTransformでのBounds
            MatrixTransform unionTF = UnionTransform(MyCanvas3.RenderTransform, MyRed3.RenderTransform);
            Rect redUnionBounds = unionTF.TransformBounds(redRect);

            //最終的なBoundsを黒枠に適用
            Rect result = new(topLeft, redUnionBounds.Size);
            MySetBounds(MyBlackWaku3, result);
        }

        /// <summary>
        /// Transform1にTransform2を追加(Append)したTransformを作って返す
        /// </summary>
        /// <param name="transform1"></param>
        /// <param name="transform2"></param>
        /// <returns></returns>
        private static MatrixTransform UnionTransform(Transform transform1, Transform transform2)
        {
            Matrix union = transform1.Value;
            union.Append(transform2.Value);
            return new MatrixTransform(union);
        }

        //子要素の回転は無し、親要素のCanvasが回転しているとき
        //親要素から見た子要素のBounds取得
        private void Test2()
        {
            //親CanvasのRenderTransformのTransformBoundsに、
            //子要素の赤枠Boundsを入れるだけで取得できる
            Rect canvasTFBounds = MyCanvas2.RenderTransform.TransformBounds(new Rect(GetLeft(MyRed2), GetTop(MyRed2), MyRed2.Width, MyRed2.Height));
            MySetBounds(MyBlackWaku2, canvasTFBounds);
        }

        private void Test1()
        {
            Rect r = MyRed.RenderTransform.TransformBounds(new Rect(0, 0, MyRed.Width, MyRed.Height));
            r.Offset(GetLeft(MyRed), GetTop(MyRed));
            MySetBounds(MyBlackWaku, r);
        }




        private Point GetPoint(FrameworkElement element) => new Point(Canvas.GetLeft(element), Canvas.GetTop(element));
        private double GetLeft(FrameworkElement element) => Canvas.GetLeft(element);
        private double GetTop(FrameworkElement element) => Canvas.GetTop(element);
        private void SetLeft(FrameworkElement element, double value) => Canvas.SetLeft(element, value);
        private void SetTop(FrameworkElement element, double value) => Canvas.SetTop(element, value);
        private void MySetTopLeft(FrameworkElement element, Point poi)
        {
            SetLeft(element, poi.X);
            SetTop(element, poi.Y);
        }
        private void MySetBounds(FrameworkElement element, Rect bounds)
        {
            SetLeft(element, bounds.Left);
            SetTop(element, bounds.Top);
            element.Width = bounds.Width;
            element.Height = bounds.Height;
        }
    }
}