using _20250713_Lab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace _20250713_Lav
{
    class MathIro
    {

        //// 正規化したRGBをSRGB、スクリーン用のRGBをリニアRGBに変換
        //public static double SrgbToLinear(double c)
        //{
        //    return c <= 0.04045 ? c / 12.92 : Math.Pow((c + 0.055) / 1.055, 2.4);
        //}

        #region リニアRGB

        /// <summary>
        /// RGB カラー値を線形 RGB 表現に変換します。
        /// </summary>
        /// <remarks>この変換では、ガンマ補正を適用して sRGB カラー成分を線形 RGB 表現に変換します。これは、線形色空間を必要とする色計算に役立ちます。</remarks>
        /// <param name="r">色の赤成分。範囲は 0 ～ 255。</param>
        /// <param name="g">色の緑成分。範囲は 0 ～ 255。</param>
        /// <param name="b">色の青成分。範囲は 0 ～ 255。</param>
        /// <returns>色の線形 RGB 成分を含むタプル: <list type="bullet">
        /// <item><description><c>lr</c>: 線形赤成分。範囲は 0.0 ～ 1.0。</description></item>
        /// <item><description><c>lg</c>: 線形緑成分。範囲は 0.0 ～ 1.0.</description></item>
        /// <item><description><c>lb</c>: 線形青成分。範囲は 0.0～1.0 です。</description></item>
        /// </list></returns>
        public static (double lr, double lg, double lb) Rgb2LinearRGB(byte r, byte g, byte b)
        {
            static double F(byte c)
            {
                double cc = c / 255.0;
                return cc <= 0.04045 ? cc / 12.92 : Math.Pow((cc + 0.055) / 1.055, 2.4);
            }
            return (F(r), F(g), F(b));
        }

        public static double Rgb2LinearRGB(byte c)
        {
            return c <= 0.04045 ? c / 12.92 : Math.Pow((c + 0.055) / 1.055, 2.4);
        }



        // sRGB - Wikipedia
        // https://en.wikipedia.org/wiki/SRGB
        // P935_53403.pdf
        // https://www.jstage.jst.go.jp/article/iieej/35/6/35_6_935/_pdf
        public static (byte r, byte g, byte b) LinearRgb2Rgb(double lr, double lg, double lb)
        {
            static byte F(double c)
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
                return (byte)double.Clamp((cc * 255) + 0.5, 0, 255);// 四捨五入で0から255
            }
            return (F(lr), F(lg), F(lb));
        }

        public static (byte r, byte g, byte b) LinearRgb2Rgb((double lr, double lg, double lb) linear)
        {
            return LinearRgb2Rgb(linear.lr, linear.lg, linear.lb);
        }

        #endregion リニアRGB






        #region XYZ

        // リニアRGB(D65)からXYZ(D50)
        // 色空間の変換 (3)
        // https://fujiwaratko.sakura.ne.jp/infosci/colorspace/colorspace3.html 
        public static (double X, double Y, double Z) LinearRGBToXYZD50(double lr, double lg, double lb)
        {
            return (
                0.436041 * lr + 0.385113 * lg + 0.143046 * lb,
                0.222485 * lr + 0.716905 * lg + 0.060610 * lb,
                0.013920 * lr + 0.097067 * lg + 0.713913 * lb
                );
        }

        // リニアRGB(D65)からXYZ(D50)
        // RGB/XYZ Matrices
        // http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html
        public static (double X, double Y, double Z) LinearRGBToXYZD50v2(double lr, double lg, double lb)
        {
            return (
                0.4360747 * lr + 0.3850649 * lg + 0.1430804 * lb,
                0.2225045 * lr + 0.7168786 * lg + 0.0606169 * lb,
                0.0139322 * lr + 0.0971045 * lg + 0.7141733 * lb);
        }






        // リニアRGB(D65)からXYZ(D65)
        // RGB/XYZ Matrices
        // http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html
        public static (double X, double Y, double Z) LinearRGBToXYZD65v0(double lr, double lg, double lb)
        {
            return (
                0.4124564 * lr + 0.3575761 * lg + 0.1804375 * lb,
                0.2126729 * lr + 0.7151522 * lg + 0.0721750 * lb,
                0.0193339 * lr + 0.1191920 * lg + 0.9503041 * lb);
        }

        // これらの式では、X、Y、Zの値は、 D65（「白」）のYが 1.0（ X = 0.9505、Y = 1.0000、Z = 1.0890）になるように調整する必要があります。
        // sRGB - Wikipedia
        // https://en.wikipedia.org/wiki/SRGB
        // リニアRGB(D65)からXYZ(D65)
        public static (double X, double Y, double Z) LinearRGBToXYZD65(double r, double g, double b)
        {
            return (
                0.4124 * r + 0.3576 * g + 0.1805 * b,
                0.2126 * r + 0.7152 * g + 0.0722 * b,
                0.0193 * r + 0.1192 * g + 0.9505 * b);
        }




        // XYZ(D65)からリニアRGB(D65)
        // sRGB - Wikipedia
        // https://en.wikipedia.org/wiki/SRGB
        public static (double lr, double lg, double lb) XyzD65ToLinearRgb(double x, double y, double z)
        {
            return (
                (3.2406255 * x) - (1.5372080 * y) - (0.4986286 * z),
               (-0.9689307 * x) + (1.8757561 * y) + (0.0415175 * z),
                (0.0557101 * x) - (0.2040211 * y) + (1.0569959 * z)
                );
        }

        // XYZ(D65)からリニアRGBはこっちの方が良い？
        // RGB/XYZ Matrices
        // http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html
        public static (double lr, double lg, double lb) XyzD65ToLinearRgbv2(double x, double y, double z)
        {
            return (
                (3.2404542 * x) - (1.5371385 * y) - (0.4985314 * z),
               (-0.9692660 * x) + (1.8760108 * y) + (0.0415560 * z),
                (0.0556434 * x) - (0.2040259 * y) + (1.0572252 * z)
                );
        }

        //// 色空間の変換 (3)
        //// https://fujiwaratko.sakura.ne.jp/infosci/colorspace/colorspace3.html
        //// XYZ(D50)からリニアRGB(D65)
        //public static (double lr, double lg, double lb) XyzD50ToLinearRgb(double x, double y, double z)
        //{
        //    return (
        //         (3.134187 * x) - (1.617209 * x) - (0.490694 * x),
        //        (-0.978749 * y) + (1.916130 * y) + (0.033433 * y),
        //         (0.071964 * z) - (0.228994 * z) + (1.405754 * z)
        //        );
        //}

        //        RGB/XYZ Matrices
        //http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html
        public static (double lr, double lg, double lb) XyzD50ToLinearRgbV2(double x, double y, double z)
        {
            return (
                3.1338561 * x - 1.6168667 * y - 0.4906146 * z,
               -0.9787684 * x + 1.9161415 * y + 0.0334540 * z,
                0.0719453 * x - 0.2289914 * y + 1.4052427 * z
                );
        }



        #endregion XYZ






        #region XYZ --> L*a*b

        // D50のXYZをLabに変換
        public static (double L, double a, double b) XyzD50ToLab(double x, double y, double z)
        {
            x *= 100.0; y *= 100.0; z *= 100.0;

            x /= 96.42;// D50のホワイトポイントで正規化
            y /= 100.0;
            z /= 82.49;

            double f = 1.0 / 3.0 * Math.Pow(29.0 / 6.0, 2.0);// 7.7870370370370354
            double muri = 0.008856;//閾値 Math.Pow(6.0 / 29.0, 3.0) = 0.0088564516790356311
            x = x > muri ? Math.Pow(x, 1.0 / 3.0) : (f * x) + (4.0 / 29.0);
            y = y > muri ? Math.Pow(y, 1.0 / 3.0) : (f * y) + (4.0 / 29.0);
            z = z > muri ? Math.Pow(z, 1.0 / 3.0) : (f * z) + (4.0 / 29.0);

            double L = (116 * y) - 16;
            double a = 500 * (x - y);
            double b = 200 * (y - z);
            return (L, a, b);
        }

        public static (double L, double a, double b) XyzD65ToLab(double x, double y, double z)
        {
            x *= 100.0; y *= 100.0; z *= 100.0;
            //x /= 95.039;// D65のホワイトポイント
            //y /= 100.0;
            //z /= 108.88;

            x /= 95.046;// D65のホワイトポイント、こっちの方が良い？
            y /= 100.0;
            z /= 108.9;
            double f = 1.0 / 3.0 * Math.Pow(29.0 / 6.0, 2.0);// 7.7870370370370354
            double threshold = 0.008856;// Math.Pow(6.0 / 29.0, 3.0) = 0.0088564516790356311
            x = x > threshold ? Math.Pow(x, 1.0 / 3.0) : (f * x) + (4.0 / 29.0);
            y = y > threshold ? Math.Pow(y, 1.0 / 3.0) : (f * y) + (4.0 / 29.0);
            z = z > threshold ? Math.Pow(z, 1.0 / 3.0) : (f * z) + (4.0 / 29.0);

            double L = (116 * y) - 16;
            double a = 500 * (x - y);
            double b = 200 * (y - z);
            return (L, a, b);
        }
        // Labでの彩度は
        // 彩度 = Math.Sqrt(a * a + b * b);
        #endregion XYZ --> L*a*b


        #region L*a*b --> XYZ
        public static (double x, double y, double z) LabToXYZD50(double l, double a, double b)
        {
            double Xw = 96.42;// D50のホワイトポイント
            double Yw = 100.0;
            double Zw = 82.49;

            double y = (l + 16) / 116;
            double x = (a / 500) + y;
            double z = (b / -200) + y;

            double muri = 6.0 / 29.0;
            double xx; double yy; double zz;
            if (y > muri) { yy = Yw * Math.Pow(y, 3.0); }
            else { yy = (y - 16 / 116) * 3 * Math.Pow(muri, 2.0) * Yw; }
            xx = x > muri ? (Xw * Math.Pow(x, 3.0)) : (x - 16.0 / 166.0) * 3.0 * Math.Pow(muri, 2.0) * Xw;
            zz = z > muri ? (Zw * Math.Pow(z, 3.0)) : (z - 16.0 / 166.0) * 3.0 * Math.Pow(muri, 2.0) * Zw;

            return (xx / 100.0, yy / 100.0, zz / 100.0);
        }





        #endregion L*a*b --> XYZ
    }
}