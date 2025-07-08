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

// RGB色空間の変換
// リニアRGBに変換してから変換するのが基本みたいで
// 直接変換するHSVやHSBは例外？

// sRGBとリニアRGBは同じもの？これがわからん

// リニアRGB？とColorクラスのプロパティのScR、ScG、ScBは同じで
// 元の色より暗くなる、元の強さが0.5だと約0.2になる


// XYZに変換
// 


namespace _20250708_XYZ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            byte r = 200;
            byte g = 100;
            byte b = 50;
            var LiRGB = MathIro.Rgb2LinearRGB(r, g, b);
            Color scrgb = Color.FromRgb(r, g, b);
            var lrgb2rgb = MathIro.LinearRgb2Rgb(LiRGB.lr, LiRGB.lg, LiRGB.lb);
            var rgb2xyz = MathIro.ToXYZ(LiRGB.lr, LiRGB.lg, LiRGB.lb);
            var xyz2rgb = MathIro.Xyz2Rgb(rgb2xyz.X, rgb2xyz.Y, rgb2xyz.Z);
        }

        private void AAA()
        {

        }
    }
}