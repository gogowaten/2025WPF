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
            Rect r1 = GetRenderTransformBounds(MyCanvas3, MyRed30);
            Rect r2 = GetRenderTransformBounds(MyCanvas3, MyRed31);
            MySetBounds(MyBlackWaku30, r1);
            MySetBounds(MyBlackWaku31, r2);
            r1.Union(r2);
            MySetBounds(MyBlackWaku32, r1);
        }

        /// <summary>
        /// childの位置とサイズのRectを、parentのRenderTransformのTransformBoundsで返す
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        private static Rect GetRenderTransformBounds(FrameworkElement parent, FrameworkElement child)
        {
            Rect rectZero = new(0, 0, child.Width, child.Height);
            Rect rect = new(Canvas.GetLeft(child), Canvas.GetTop(child), child.Width, child.Height);

            //位置の取得A、child自身のTransformを使ったBounds            
            Rect boundsZero = child.RenderTransform.TransformBounds(rectZero);

            //位置の取得B、parentのTransformを使ったBounds            
            Rect parentTFBounds = parent.RenderTransform.TransformBounds(rect);

            //最終的な位置の取得は、AとBの合成(offset)
            Point topLeft = parentTFBounds.TopLeft;
            topLeft.Offset(boundsZero.X, boundsZero.Y);

            //サイズ取得は、parentとchildのTransformを合成したTransformでのBounds
            MatrixTransform unionTF = UnionTransform(parent.RenderTransform, child.RenderTransform);
            Rect unionBounds = unionTF.TransformBounds(rect);

            //最終的な位置とサイズを返す
            return new Rect(topLeft, unionBounds.Size);
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

            //それぞれのRectを合成したものが目的のBoundsになる
            Rect unionR = canvasTFBounds1;
            unionR.Union(canvasTFBounds2);
            MySetBounds(MyBlackWaku22, unionR);
            //Rect neko = GetRenderTransformBounds(MyCanvas2, MyRed20);
            //Rect inu = GetRenderTransformBounds(MyCanvas2, MyRed21);
        }

        //図形だけ回転の場合
        //零座標のRectで計算
        private void Test11()
        {
            Rect r = MyRed10.RenderTransform.TransformBounds(new Rect(0, 0, MyRed10.Width, MyRed10.Height));
            r.Offset(GetLeft(MyRed10), GetTop(MyRed10));
            MySetBounds(MyBlackWaku10, r);
            Rect r2 = MyRed11.RenderTransform.TransformBounds(new Rect(0, 0, MyRed11.Width, MyRed11.Height));
            r2.Offset(GetLeft(MyRed11), GetTop(MyRed11));
            MySetBounds(MyBlackWaku11, r2);

            //それぞれのRectを合成したものが目的のBoundsになる
            Rect unionR = r;
            unionR.Union(r2);
            MySetBounds(MyBlackWaku12, unionR);
        }



        private double GetLeft(FrameworkElement element) => Canvas.GetLeft(element);

        private double GetTop(FrameworkElement element) => Canvas.GetTop(element);

        private void SetLeft(FrameworkElement element, double value) => Canvas.SetLeft(element, value);

        private void SetTop(FrameworkElement element, double value) => Canvas.SetTop(element, value);

    
        private void MySetBounds(FrameworkElement element, Rect bounds)
        {
            SetLeft(element, bounds.Left);
            SetTop(element, bounds.Top);
            element.Width = bounds.Width;
            element.Height = bounds.Height;
        }

    }
}