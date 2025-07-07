using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20250707_OKLCH
{
    public static class MathIro
    {
        public static (double X, double Y, double Z) RgbToXyz(byte r, byte g, byte b)
        {
            // 1. sRGB → 線形RGB
            double sr = SrgbToLinear(r / 255.0);
            double sg = SrgbToLinear(g / 255.0);
            double sb = SrgbToLinear(b / 255.0);

            // 2. 線形RGB → XYZ（D65）
            double X = 0.4124564 * sr + 0.3575761 * sg + 0.1804375 * sb;
            double Y = 0.2126729 * sr + 0.7151522 * sg + 0.0721750 * sb;
            double Z = 0.0193339 * sr + 0.1191920 * sg + 0.9503041 * sb;

            return (X, Y, Z);
        }



        public static (double sr, double sg, double sb) Rgb2Srgb(byte r, byte g, byte b)
        {
            return (SrgbToLinear(r / 255.0), SrgbToLinear(g / 255.0), SrgbToLinear(b / 255.0));
        }
        public static (double X, double Y, double Z) Rgb2Xyz(double sr, double sg, double sb)
        {            
            double l = (0.4122214708 * sr) + (0.5363325363 * sg) + (0.0514459929 * sb);
            double m = (0.2119034982 * sr) + (0.6806995451 * sg) + (0.1073969566 * sb);
            double s = (0.0883024619 * sr) + (0.2817188376 * sg) + (0.6299787005 * sb);
            return (l, m, s);
        }

        // A perceptual color space for image processing
        // https://bottosson.github.io/posts/oklab/
        public static (double L, double a, double b) Rgb2OkLab(byte r, byte g, byte b)
        {
            double sr = SrgbToLinear(r / 255.0);
            double sg = SrgbToLinear(g / 255.0);
            double sb = SrgbToLinear(b / 255.0);

            double l = (0.4122214708 * sr) + (0.5363325363 * sg) + (0.0514459929 * sb);
            double m = (0.2119034982 * sr) + (0.6806995451 * sg) + (0.1073969566 * sb);
            double s = (0.0883024619 * sr) + (0.2817188376 * sg) + (0.6299787005 * sb);

            double l_ = CubeRoot(l);
            double m_ = CubeRoot(m);
            double s_ = CubeRoot(s);

            return (
                (0.2104542553 * l_) + (0.7936177850 * m_) - (0.0040720468 * s_),
                (1.9779984951 * l_) - (2.4285922050 * m_) + (0.4505937099 * s_),
                (0.0259040371 * l_) + (0.7827717662 * m_) - (0.8086757660 * s_)
                );
        }






        // sRGB (0-255) → OKLCH
        public static (double L, double C, double H) RgbToOklch(int r, int g, int b)
        {
            // 1. sRGB → 線形RGB
            double sr = SrgbToLinear(r / 255.0);
            double sg = SrgbToLinear(g / 255.0);
            double sb = SrgbToLinear(b / 255.0);

            // 2. 線形RGB → XYZ (D65)
            double x = (0.4122214708 * sr) + (0.5363325363 * sg) + (0.0514459929 * sb);
            double y = (0.2119034982 * sr) + (0.6806995451 * sg) + (0.1073969566 * sb);
            double z = (0.0883024619 * sr) + (0.2817188376 * sg) + (0.6299787005 * sb);

            // 3. XYZ → OKLab
            double l = 0.2104542553 * CubeRoot(x) + 0.7936177850 * CubeRoot(y) - 0.0040720468 * CubeRoot(z);
            double a = 1.9779984951 * CubeRoot(x) - 2.4285922050 * CubeRoot(y) + 0.4505937099 * CubeRoot(z);
            double b2 = 0.0259040371 * CubeRoot(x) + 0.7827717662 * CubeRoot(y) - 0.8086757660 * CubeRoot(z);

            // 4. OKLab → OKLCH
            double C = Math.Sqrt(a * a + b2 * b2);
            double H = Math.Atan2(b2, a) * 180.0 / Math.PI;
            if (H < 0) H += 360.0;

            return (l, C, H);
        }

        // 正規化したRGBをSRGB、スクリーン用のRGBをリニアRGBに変換
        public static double SrgbToLinear(double c)
        {
            return c <= 0.04045 ? c / 12.92 : Math.Pow((c + 0.055) / 1.055, 2.4);
        }

        // 3乗根を返す。マイナス符号もそのままにするためにMath.Signを使っている
        public static double CubeRoot(double x)
        {
            return Math.Sign(x) * Math.Pow(Math.Abs(x), 1.0 / 3.0);
        }







    }











}
