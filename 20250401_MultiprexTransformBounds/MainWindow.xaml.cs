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

namespace _20250401_MultiprexTransformBounds
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
            Test2();

        }
        //RenderTransformの合成

        //MyCanvas ここにRectangleのBoundsを表示したい
        //  MyCanvas1 回転
        //      MyCanvas2 回転
        //          Rectangle 回転

        //RenderTransform同士の合成は、RenderTransformのvalueプロパティ同士でAppendメソッドでできる
        //
        
        //
        private void Test1()
        {
            //MyCanvas1とMyCanvas2のRenderTransformを合成
            Matrix unionMatrix = MyCanvas1.RenderTransform.Value;
            unionMatrix.Append(MyCanvas2.RenderTransform.Value);

            Point redPoi = new(GetLeft(MyRed), GetTop(MyRed));
            redPoi = unionMatrix.Transform(redPoi);

            Matrix redM = MyRed.RenderTransform.Value;
            unionMatrix.Append(redM);
            Point canPoi = new(GetLeft(MyCanvas2), GetTop(MyCanvas2));
            Point canPoi1 = MyCanvas1.RenderTransform.Transform(canPoi);

            MatrixTransform mtf = new(unionMatrix);
            Rect redRect = new(0, 0, MyRed.Width, MyRed.Height);
            redRect = mtf.TransformBounds(redRect);
            MyBlackWaku.Width = redRect.Width;
            MyBlackWaku.Height = redRect.Height;
            SetLeft(MyBlackWaku, redRect.Left + GetLeft(MyCanvas1) + canPoi1.X + redPoi.X);
            SetTop(MyBlackWaku, redRect.Top + GetTop(MyCanvas1) + canPoi1.Y + redPoi.Y);
        }

        private void Test2()
        {
            Rect redRect = new(0, 0, MyRed.Width, MyRed.Height);
            Point redPoi = new(GetLeft(MyRed), GetTop(MyRed));

            Matrix m2 = MyCanvas2.RenderTransform.Value;
            redPoi = m2.Transform(redPoi);

            Matrix redM = MyRed.RenderTransform.Value;
            m2.Append(redM);
            MatrixTransform mtf = new(m2);
            redRect = mtf.TransformBounds(redRect);

            MyBlueWaku.Width = redRect.Width;
            MyBlueWaku.Height = redRect.Height;

            SetLeft(MyBlueWaku, GetLeft(MyCanvas2) + redRect.Left + redPoi.X);
            SetTop(MyBlueWaku, GetTop(MyCanvas2) + redRect.Top + redPoi.Y);

        }

        //private void Test3()
        //{

        //    Matrix unionMatrix = MyCanvas1.RenderTransform.Value;
        //    Matrix m1 = MyCanvas1.RenderTransform.Value;
        //    Matrix m2 = MyCanvas2.RenderTransform.Value;
        //    unionMatrix.Append(m2);

        //    Point redPoi = new(GetLeft(MyRed), GetTop(MyRed));
        //    //redPoi = unionMatrix.Transform(redPoi);

        //    Matrix redM = MyRed.RenderTransform.Value;
        //    unionMatrix.Append(redM);
        //    Point canPoi = new(GetLeft(MyCanvas2), GetTop(MyCanvas2));
        //    Point canPoi1 = MyCanvas1.RenderTransform.Transform(canPoi);

        //    MatrixTransform mtf = new(unionMatrix);
        //    Rect redRect = new(0, 0, MyRed.Width, MyRed.Height);
        //    redRect = mtf.TransformBounds(redRect);
        //    MyCyanWaku.Width = redRect.Width;
        //    MyCyanWaku.Height = redRect.Height;
        //    var mm2 = MyCanvas2.RenderTransform.Value;
        //    mm2.Invert();
        //    redM.Append(mm2);
        //    var mm1 = MyCanvas1.RenderTransform.Value;
        //    mm1.Invert();
        //    redM.Append(mm1);
        //    redPoi=redM.Transform(redPoi);
        //    MyCyanWaku.RenderTransform = new MatrixTransform(redM);
            
        //    SetLeft(MyCyanWaku, redRect.Left - redPoi.X);
        //    SetTop(MyCyanWaku, redRect.Top - GetTop(MyCanvas1) - canPoi1.Y - redPoi.Y);
        //}
        
        //
   

        private void SetLeft(FrameworkElement element, double left) => Canvas.SetLeft(element, left);
        private double GetLeft(FrameworkElement element) => Canvas.GetLeft(element);

        private void SetTop(FrameworkElement element, double top) => Canvas.SetTop(element, top);
        private double GetTop(FrameworkElement element) => Canvas.GetTop(element);
    }
}