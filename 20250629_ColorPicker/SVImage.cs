using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace _20250629_ColorPicker
{
  




    public class SVImage : Image
    {
        public SVImage()
        {
            Loaded += SVImage_Loaded;

        }

        private void SVImage_Loaded(object sender, RoutedEventArgs e)
        {
            MultiBinding mb;
            mb = new() { Converter = new MyConvSVBitmap() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(WidthProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(HeightProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(HueProperty) });
            _ = SetBinding(SourceProperty, mb);

            //mb = new() { Converter = new MyConvHSVBrush() };
            //mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(HueProperty) });
            //mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(SProperty) });
            //mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(VProperty) });
            //_ = SetBinding(MyBrushProperty, mb);
        }



        public Brush MyBrush
        {
            get { return (Brush)GetValue(MyBrushProperty); }
            set { SetValue(MyBrushProperty, value); }
        }
        public static readonly DependencyProperty MyBrushProperty =
            DependencyProperty.Register(nameof(MyBrush), typeof(Brush), typeof(SVImage), new PropertyMetadata(Brushes.Transparent));

        public double MyHue
        {
            get { return (double)GetValue(HueProperty); }
            set { SetValue(HueProperty, value); }
        }
        public static readonly DependencyProperty HueProperty =
            DependencyProperty.Register(nameof(MyHue), typeof(double), typeof(SVImage), new PropertyMetadata(0.0));


        //public double S
        //{
        //    get { return (double)GetValue(SProperty); }
        //    set { SetValue(SProperty, value); }
        //}
        //public static readonly DependencyProperty SProperty =
        //    DependencyProperty.Register(nameof(S), typeof(double), typeof(SVImage), new PropertyMetadata(0.0));

        //public double V
        //{
        //    get { return (double)GetValue(VProperty); }
        //    set { SetValue(VProperty, value); }
        //}
        //public static readonly DependencyProperty VProperty =
        //    DependencyProperty.Register(nameof(V), typeof(double), typeof(SVImage), new PropertyMetadata(0.0));





        private class MyConvSVBitmap : IMultiValueConverter
        {
            private void MyParallel(int p, int y, int stride, byte[] pixels, double hue, int w, int h)
            {
                double v = y / (h - 1.0);
                double ww = w - 1;

                for (int x = 0; x < w; ++x)
                {
                    p = (y * stride) + (x * 3);
                    (pixels[p + 2], pixels[p + 1], pixels[p]) = MathHSV.Hsv2rgb(hue, x / ww, v);
                }
            }
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                var width = (int)(double)values[0];
                var height = (int)(double)values[1];
                var hue = (double)values[2];

                int stride = width * 3;
                byte[] pixles = new byte[height * stride];

                _ = Parallel.For(0, height, y =>
                {
                    MyParallel(0, y, stride, pixles, hue, width, height);
                });

                var bmp = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr24, null);
                bmp.WritePixels(new Int32Rect(0, 0, width, height), pixles, stride, 0);
                return bmp;
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class MyConvHSVBrush : IMultiValueConverter
        {
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                var h = (double)values[0];
                var s = (double)values[1];
                var v = (double)values[2];
                Color iro = MathHSV.HSV2Color(h, s, v);
                return new SolidColorBrush(iro);
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }




    }







}
