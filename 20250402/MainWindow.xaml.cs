using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

//WPF、子要素と孫要素ともに回転時の、孫要素がピッタリ収まるBounds(Rect)の取得 - 午後わてんのブログ
//https://gogowaten.hatenablog.com/entry/2025/04/02/151945

namespace _20250402
{
    public partial class MainWindow : Window
    {
        //private Rect MyRedBounds;
        public MainWindow()
        {
            InitializeComponent();

            Test11();
            Test21();
            Test31();
        }

        //子要素、孫要素ともに回転しているとき
        //親要素から見た孫要素のBounds取得
        private void Test31()
        {
            //位置の取得A、孫要素自体のTransform後のBounds
            Size redSize = new(MyRed30.Width, MyRed30.Height);
            Rect redZeroBounds = MyRed30.RenderTransform.TransformBounds(new Rect(redSize));

            //位置の取得B、子要素のTransformを使った孫要素のBounds
            Rect redRect = new(GetPoint(MyRed30), redSize);
            Rect canvasTFBounds = MyCanvas3.RenderTransform.TransformBounds(redRect);

            //最終的な位置の取得は、AとBの合成(offset)
            Point topLeft = canvasTFBounds.TopLeft;
            topLeft.Offset(redZeroBounds.X, redZeroBounds.Y);

            //サイズ取得は、子要素と孫要素のTransformを合成したTransformでのBounds
            MatrixTransform unionTF = UnionTransform(MyCanvas3.RenderTransform, MyRed30.RenderTransform);
            Rect redUnionBounds = unionTF.TransformBounds(redRect);

            //最終的なBoundsを黒枠に適用
            Rect result = new(topLeft, redUnionBounds.Size);
            MySetBounds(MyBlackWaku30, result);

            //Rect canvasTFBounds1 = MyCanvas3.RenderTransform.TransformBounds(new Rect(GetLeft(MyRed30), GetTop(MyRed30), MyRed30.Width, MyRed30.Height));
            //MySetBounds(MyBlackWaku30, canvasTFBounds1);
            //Rect canvasTFBounds2 = MyCanvas3.RenderTransform.TransformBounds(new Rect(GetLeft(MyRed31), GetTop(MyRed31), MyRed31.Width, MyRed31.Height));
            //MySetBounds(MyBlackWaku31, canvasTFBounds2);
            //Rect unionR = canvasTFBounds1;
            //unionR.Union(canvasTFBounds2);
            //MySetBounds(MyBlackWaku32, unionR);

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
        private void Test21()
        {
            //親CanvasのRenderTransformのTransformBoundsに、
            //子要素の赤枠Boundsを入れるだけで取得できる
            Rect canvasTFBounds1 = MyCanvas2.RenderTransform.TransformBounds(new Rect(GetLeft(MyRed20), GetTop(MyRed20), MyRed20.Width, MyRed20.Height));
            MySetBounds(MyBlackWaku20, canvasTFBounds1);
            Rect canvasTFBounds2 = MyCanvas2.RenderTransform.TransformBounds(new Rect(GetLeft(MyRed21), GetTop(MyRed21), MyRed21.Width, MyRed21.Height));
            MySetBounds(MyBlackWaku21, canvasTFBounds2);
            Rect unionR = canvasTFBounds1;
            unionR.Union(canvasTFBounds2);
            MySetBounds(MyBlackWaku22, unionR);

        }

        private void Test11()
        {
            Rect r = MyRed10.RenderTransform.TransformBounds(new Rect(0, 0, MyRed10.Width, MyRed10.Height));
            r.Offset(GetLeft(MyRed10), GetTop(MyRed10));
            MySetBounds(MyBlackWaku10, r);
            Rect r2 = MyRed11.RenderTransform.TransformBounds(new Rect(0, 0, MyRed11.Width, MyRed11.Height));
            r2.Offset(GetLeft(MyRed11), GetTop(MyRed11));
            MySetBounds(MyBlackWaku11, r2);
            Rect unionR = r;
            unionR.Union(r2);
            MySetBounds(MyBlackWaku12, unionR);
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