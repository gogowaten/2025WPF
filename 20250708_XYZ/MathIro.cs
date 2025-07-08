using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace _20250708_XYZ
{
    class MathIro
    {

        // 正規化したRGBをSRGB、スクリーン用のRGBをリニアRGBに変換
        public static double SrgbToLinear(double c)
        {
            return c <= 0.04045 ? c / 12.92 : Math.Pow((c + 0.055) / 1.055, 2.4);
        }


        public static (double lr, double lg, double lb) Rgb2LinearRGB(byte r, byte g, byte b)
        {
            static double AAA(byte c)
            {
                double cc = c / 255.0;
                return cc <= 0.04045 ? cc / 12.92 : Math.Pow((cc + 0.055) / 1.055, 2.4);
            }
            return (AAA(r), AAA(g), AAA(b));
        }

        // sRGB - Wikipedia
        // https://en.wikipedia.org/wiki/SRGB
        // P935_53403.pdf
        // https://www.jstage.jst.go.jp/article/iieej/35/6/35_6_935/_pdf
        public static (byte r, byte g, byte b) LinearRgb2Rgb(double lr, double lg, double lb)
        {
            static byte AAA(double c)
            {
                double cc;
                if (c <= 0.0031308)
                {
                    cc = 12.92 * c;
                }
                else
                {
                    cc = (1.055 * Math.Pow(c, 1 / 2.4)) - 0.055;
                }
                return (byte)(double.Clamp(cc * 255, 0, 255));
            }
            return (AAA(lr), AAA(lg), AAA(lb));
        }





        public static (double X, double Y, double Z) ToXYZ(double lr, double lg, double lb)
        {
            return (
                0.4124564 * lr + 0.3575761 * lg + 0.1804375 * lb,
                0.2126729 * lr + 0.7151522 * lg + 0.0721750 * lb,
                0.0193339 * lr + 0.1191920 * lg + 0.9503041 * lb);
        }

        // sRGB - Wikipedia
        // https://en.wikipedia.org/wiki/SRGB
        public static (double lr, double lg, double lb) Xyz2Rgb(double x, double y, double z)
        {
            return (
                (3.2406255 * x) + (-1.5372080 * y) + (-0.4986286 * z),
                (-0.9689307 * x) + (1.8757561 * y) + (0.0415175 * z),
                (0.0557101 * x) + (-0.2040211 * y) + (1.0569959 * z)
                );
        }

    }
}
