using Xunit;

namespace _20250703HSV
{
    public class MathHSVTests
    {
        [Theory]
        [InlineData(255, 0, 0, 0, 1, 1)]      // ê‘
        [InlineData(0, 255, 0, 120, 1, 1)]    // óŒ
        [InlineData(0, 0, 255, 240, 1, 1)]    // ê¬
        [InlineData(255, 255, 255, 0, 0, 1)]  // îí
        [InlineData(0, 0, 0, 0, 0, 0)]        // çï
        public void Rgb2hsv_ExpectedValues(byte r, byte g, byte b, double expectedH, double expectedS, double expectedV)
        {
            var (h, s, v) = MathHSV.Rgb2hsv(r, g, b);
            Assert.Equal(expectedH, h, 1);
            Assert.Equal(expectedS, s, 2);
            Assert.Equal(expectedV, v, 2);
        }

        [Theory]
        [InlineData(0, 1, 1, 255, 0, 0)]      // ê‘
        [InlineData(120, 1, 1, 0, 255, 0)]    // óŒ
        [InlineData(240, 1, 1, 0, 0, 255)]    // ê¬
        [InlineData(0, 0, 1, 255, 255, 255)]  // îí
        [InlineData(0, 0, 0, 0, 0, 0)]        // çï
        public void Hsv2rgb_ExpectedValues(double h, double s, double v, byte expectedR, byte expectedG, byte expectedB)
        {
            var (r, g, b) = MathHSV.Hsv2rgb(h, s, v);
            Assert.Equal(expectedR, r);
            Assert.Equal(expectedG, g);
            Assert.Equal(expectedB, b);
        }
    }
}