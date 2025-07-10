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

// 色空間の系統？
// RGBカラーモデルには
//      sRGB、Adobe RGB、DCI-P3など

// ガンマ
// 普通のRGBはガンマ補正されたもので、ガンマ補正された数値は0と255以外は元より大きくなる
// 画像処理や色空間の変換をするときにはガンマ補正を取り除いて、リニアRGBに変換してからの方が良い
// リニアRGBにするには逆ガンマ補正する

// sRGBとリニアRGBは同じもの？これがわからん
// どうやらsRGBは普通のRGBを同じ意味みたい、なのでリニアRGBとは違う？


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
        public XYZ MyXYZ { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            var neko = Math.Pow(6.0 / 29.0, 3.0);// 0.0088564516790356311
            var Y = 0.5;
            var t = Math.Pow(Y / 1.0, 1.0 / 3.0);
            var f = Math.Pow(t, 1.0 / 3.0);

            var ika = (116 * Math.Pow(t, 1 / 3)) - 16;
            var tako = 116 * f - 16;

            MyXYZ = new XYZ(200, 100, 50);
            DataContext = this;

            byte r = 200;
            byte g = 100;
            byte b = 50;
            var LiRGB = MathIro.Rgb2LinearRGB(r, g, b);
            Color c1 = Color.FromRgb(r, g, b);
            var lrgb2rgb = MathIro.LinearRgb2Rgb(LiRGB.lr, LiRGB.lg, LiRGB.lb);
            var rgb2xyz = MathIro.ToXYZ(LiRGB.lr, LiRGB.lg, LiRGB.lb);
            var xyz2rgb = MathIro.Xyz2Rgb(rgb2xyz.X, rgb2xyz.Y, rgb2xyz.Z);
        }

        private void AAA()
        {

        }
    }
}