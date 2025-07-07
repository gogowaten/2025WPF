using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20250707_OKLCH
{
    public static class Oklch
    {

        /// <summary>
        /// RGB (0-255) から OKLCH (L:0-1, C:0-0.4, H:0-360) へ変換します。
        /// </summary>
        public static (double L, double C, double H) RgbToOklch(byte r, byte g, byte b)
        {
            // RGB → Linear sRGB
            double R = r / 255.0;
            double G = g / 255.0;
            double B = b / 255.0;
            R = (R <= 0.04045) ? R / 12.92 : Math.Pow((R + 0.055) / 1.055, 2.4);
            G = (G <= 0.04045) ? G / 12.92 : Math.Pow((G + 0.055) / 1.055, 2.4);
            B = (B <= 0.04045) ? B / 12.92 : Math.Pow((B + 0.055) / 1.055, 2.4);

            // Linear sRGB → XYZ (D65)
            double X = R * 0.4122214708 + G * 0.5363325363 + B * 0.0514459929;
            double Y = R * 0.2119034982 + G * 0.6806995451 + B * 0.1073969566;
            double Z = R * 0.0883024619 + G * 0.2817188376 + B * 0.6299787005;

            // XYZ → OKLab
            double l = 0.8189330101 * X + 0.3618667424 * Y - 0.1288597137 * Z;
            double m = 0.0329845436 * X + 0.9293118715 * Y + 0.0361456387 * Z;
            double s = 0.0482003018 * X + 0.2643662691 * Y + 0.6338517070 * Z;

            double l_ = Math.Cbrt(l);
            double m_ = Math.Cbrt(m);
            double s_ = Math.Cbrt(s);

            double L_ok = 0.2104542553 * l_ + 0.7936177850 * m_ - 0.0040720468 * s_;
            double a_ok = 1.9779984951 * l_ - 2.4285922050 * m_ + 0.4505937099 * s_;
            double b_ok = 0.0259040371 * l_ + 0.7827717662 * m_ - 0.8086757660 * s_;

            // OKLab → OKLCH
            double C = Math.Sqrt(a_ok * a_ok + b_ok * b_ok);
            double H = Math.Atan2(b_ok, a_ok) * 180.0 / Math.PI;
            if (H < 0) H += 360.0;

            return (L_ok, C, H);
        }

        /// <summary>
        /// OKLCH (L:0-1, C:0-0.4, H:0-360) から RGB (0-255) へ変換します。
        /// </summary>
        public static (byte r, byte g, byte b) OklchToRgb(double L, double C, double H)
        {
            // OKLCH → OKLab
            double hRad = H * Math.PI / 180.0;
            double a = C * Math.Cos(hRad);
            double b = C * Math.Sin(hRad);

            // OKLab → LMS
            double l_ = (L + 0.3963377774 * a + 0.2158037573 * b);
            double m_ = (L - 0.1055613458 * a - 0.0638541728 * b);
            double s_ = (L - 0.0894841775 * a - 1.2914855480 * b);

            double l = l_ * l_ * l_;
            double m = m_ * m_ * m_;
            double s = s_ * s_ * s_;

            // LMS → XYZ
            double X = 1.2270138511 * l - 0.5577999807 * m + 0.2812561490 * s;
            double Y = -0.0405801784 * l + 1.1122568696 * m - 0.0716766787 * s;
            double Z = -0.0763812845 * l - 0.4214819784 * m + 1.5861632204 * s;

            // XYZ → Linear sRGB
            double R = 3.2409699419 * X - 1.5373831776 * Y - 0.4986107603 * Z;
            double G = -0.9692436363 * X + 1.8759675015 * Y + 0.0415550574 * Z;
            double B = 0.0556300797 * X - 0.2039769589 * Y + 1.0569715142 * Z;

            // Linear sRGB → sRGB
            R = (R <= 0.0031308) ? 12.92 * R : 1.055 * Math.Pow(R, 1.0 / 2.4) - 0.055;
            G = (G <= 0.0031308) ? 12.92 * G : 1.055 * Math.Pow(G, 1.0 / 2.4) - 0.055;
            B = (B <= 0.0031308) ? 12.92 * B : 1.055 * Math.Pow(B, 1.0 / 2.4) - 0.055;

            // 範囲クリップ
            byte r = (byte)Math.Round(Math.Clamp(R, 0, 1) * 255);
            byte g = (byte)Math.Round(Math.Clamp(G, 0, 1) * 255);
            byte bb = (byte)Math.Round(Math.Clamp(B, 0, 1) * 255);

            return (r, g, bb);
        }
    }
}
