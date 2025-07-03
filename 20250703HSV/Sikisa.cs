using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20250703HSV
{
    class Sikisa
    {


        /// <summary>
        /// 2つのRGB値のCIEDE2000色差を計算します。
        /// </summary>
        /// <param name="r1">1つ目の赤 (0-255)</param>
        /// <param name="g1">1つ目の緑 (0-255)</param>
        /// <param name="b1">1つ目の青 (0-255)</param>
        /// <param name="r2">2つ目の赤 (0-255)</param>
        /// <param name="g2">2つ目の緑 (0-255)</param>
        /// <param name="b2">2つ目の青 (0-255)</param>
        /// <returns>CIEDE2000色差</returns>
        public static double Ciede2000(byte r1, byte g1, byte b1, byte r2, byte g2, byte b2)
        {
            // RGB→XYZ変換
            static (double X, double Y, double Z) RgbToXyz(byte r, byte g, byte b)
            {
                double R = r / 255.0;
                double G = g / 255.0;
                double B = b / 255.0;

                // sRGBガンマ補正
                R = (R > 0.04045) ? Math.Pow((R + 0.055) / 1.055, 2.4) : R / 12.92;
                G = (G > 0.04045) ? Math.Pow((G + 0.055) / 1.055, 2.4) : G / 12.92;
                B = (B > 0.04045) ? Math.Pow((B + 0.055) / 1.055, 2.4) : B / 12.92;

                R *= 100.0;
                G *= 100.0;
                B *= 100.0;

                // D65基準
                double X = R * 0.4124 + G * 0.3576 + B * 0.1805;
                double Y = R * 0.2126 + G * 0.7152 + B * 0.0722;
                double Z = R * 0.0193 + G * 0.1192 + B * 0.9505;
                return (X, Y, Z);
            }

            // XYZ→Lab変換
            static (double L, double a, double b) XyzToLab(double X, double Y, double Z)
            {
                // D65白色点
                double Xn = 95.047;
                double Yn = 100.000;
                double Zn = 108.883;

                double x = X / Xn;
                double y = Y / Yn;
                double z = Z / Zn;

                static double F(double t) =>
                    (t > 0.008856) ? Math.Pow(t, 1.0 / 3.0) : (7.787 * t) + (16.0 / 116.0);

                double fx = F(x);
                double fy = F(y);
                double fz = F(z);

                double L = (116 * fy) - 16;
                double a = 500 * (fx - fy);
                double b = 200 * (fy - fz);
                return (L, a, b);
            }

            // CIEDE2000色差計算
            static double DeltaE2000((double L, double a, double b) lab1, (double L, double a, double b) lab2)
            {
                // 公式に基づく実装
                double L1 = lab1.L, a1 = lab1.a, b1 = lab1.b;
                double L2 = lab2.L, a2 = lab2.a, b2 = lab2.b;

                double avgLp = (L1 + L2) / 2.0;
                double C1 = Math.Sqrt(a1 * a1 + b1 * b1);
                double C2 = Math.Sqrt(a2 * a2 + b2 * b2);
                double avgC = (C1 + C2) / 2.0;

                double G = 0.5 * (1 - Math.Sqrt(Math.Pow(avgC, 7) / (Math.Pow(avgC, 7) + Math.Pow(25.0, 7))));
                double a1p = (1 + G) * a1;
                double a2p = (1 + G) * a2;
                double C1p = Math.Sqrt(a1p * a1p + b1 * b1);
                double C2p = Math.Sqrt(a2p * a2p + b2 * b2);
                double avgCp = (C1p + C2p) / 2.0;

                double h1p = Math.Atan2(b1, a1p);
                if (h1p < 0) h1p += 2 * Math.PI;
                double h2p = Math.Atan2(b2, a2p);
                if (h2p < 0) h2p += 2 * Math.PI;

                double deltahp;
                if (Math.Abs(h1p - h2p) <= Math.PI)
                    deltahp = h2p - h1p;
                else if (h2p <= h1p)
                    deltahp = h2p - h1p + 2 * Math.PI;
                else
                    deltahp = h2p - h1p - 2 * Math.PI;

                double deltaLp = L2 - L1;
                double deltaCp = C2p - C1p;
                double deltaHp = 2 * Math.Sqrt(C1p * C2p) * Math.Sin(deltahp / 2.0);

                double avgHp;
                if (Math.Abs(h1p - h2p) > Math.PI)
                    avgHp = (h1p + h2p + 2 * Math.PI) / 2.0;
                else
                    avgHp = (h1p + h2p) / 2.0;

                double T = 1
                    - 0.17 * Math.Cos(avgHp - Math.PI / 6)
                    + 0.24 * Math.Cos(2 * avgHp)
                    + 0.32 * Math.Cos(3 * avgHp + Math.PI / 30)
                    - 0.20 * Math.Cos(4 * avgHp - 21 * Math.PI / 60);

                double deltaTheta = (30 * Math.PI / 180) * Math.Exp(-Math.Pow((avgHp * 180 / Math.PI - 275) / 25, 2));
                double Rc = 2 * Math.Sqrt(Math.Pow(avgCp, 7) / (Math.Pow(avgCp, 7) + Math.Pow(25.0, 7)));
                double Sl = 1 + ((0.015 * Math.Pow(avgLp - 50, 2)) / Math.Sqrt(20 + Math.Pow(avgLp - 50, 2)));
                double Sc = 1 + 0.045 * avgCp;
                double Sh = 1 + 0.015 * avgCp * T;
                double Rt = -Math.Sin(2 * deltaTheta) * Rc;

                double deltaE = Math.Sqrt(
                    Math.Pow(deltaLp / Sl, 2) +
                    Math.Pow(deltaCp / Sc, 2) +
                    Math.Pow(deltaHp / Sh, 2) +
                    Rt * (deltaCp / Sc) * (deltaHp / Sh)
                );

                return deltaE;
            }

            var xyz1 = RgbToXyz(r1, g1, b1);
            var xyz2 = RgbToXyz(r2, g2, b2);
            var lab1 = XyzToLab(xyz1.X, xyz1.Y, xyz1.Z);
            var lab2 = XyzToLab(xyz2.X, xyz2.Y, xyz2.Z);
            return DeltaE2000(lab1, lab2);
        }
    }
}
