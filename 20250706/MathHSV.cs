using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace _20250706
{
    public static class MathHSV
    {

        /// <summary>
        /// HSV値 (h:0-360, s:0-1, v:0-1) をRGB値 (0-255) に変換します。
        /// </summary>
        /// <param name="h">色相 (0-360)</param>
        /// <param name="s">彩度 (0-1)</param>
        /// <param name="v">明度 (0-1)</param>
        /// <returns>r, g, b (各0-255)</returns>
        public static (byte r, byte g, byte b) Hsv2rgb(double h, double s, double v)
        {
            double c = v * s;
            double x = c * (1 - Math.Abs((h / 60.0) % 2 - 1));
            double m = v - c;
            double r1;
            double g1;
            double b1;
            if (h < 0 || h >= 360) { h = 0; }

            if (h < 60)
            {
                r1 = c; g1 = x; b1 = 0;
            }
            else if (h < 120)
            {
                r1 = x; g1 = c; b1 = 0;
            }
            else if (h < 180)
            {
                r1 = 0; g1 = c; b1 = x;
            }
            else if (h < 240)
            {
                r1 = 0; g1 = x; b1 = c;
            }
            else if (h < 300)
            {
                r1 = x; g1 = 0; b1 = c;
            }
            else
            {
                r1 = c; g1 = 0; b1 = x;
            }

            byte r = (byte)Math.Round((r1 + m) * 255);
            byte g = (byte)Math.Round((g1 + m) * 255);
            byte b = (byte)Math.Round((b1 + m) * 255);

            return (r, g, b);
        }




        /// <summary>
        /// RGB値 (0-255) をHSV値 (h:0-360, s:0-1, v:0-1) に変換します。
        /// </summary>
        /// <param name="r">赤 (0-255)</param>
        /// <param name="g">緑 (0-255)</param>
        /// <param name="b">青 (0-255)</param>
        /// <returns>h: 色相(0-360), s: 彩度(0-1), v: 明度(0-1)</returns>
        public static (double h, double s, double v) Rgb2hsv(byte r, byte g, byte b)
        {
            double rNorm = r / 255.0;
            double gNorm = g / 255.0;
            double bNorm = b / 255.0;

            double max = Math.Max(rNorm, Math.Max(gNorm, bNorm));
            double min = Math.Min(rNorm, Math.Min(gNorm, bNorm));
            double delta = max - min;

            double h = 0.0;
            if (delta > 0)
            {
                if (max == rNorm)
                {
                    h = 60 * (((gNorm - bNorm) / delta) % 6);
                }
                else if (max == gNorm)
                {
                    h = 60 * (((bNorm - rNorm) / delta) + 2);
                }
                else // max == bNorm
                {
                    h = 60 * (((rNorm - gNorm) / delta) + 4);
                }
            }
            if (h < 0) h += 360;

            double s = (max == 0) ? 0 : delta / max;
            double v = max;

            return (h, s, v);
        }





        public static Color hsv2Color(double h, double s, double v)
        {
            (byte r, byte g, byte b) = Hsv2rgb(h, s, v);
            return Color.FromRgb(r, g, b);
        }






    }
}
