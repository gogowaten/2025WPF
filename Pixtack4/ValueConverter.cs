using System.Windows.Media;
using System.Windows.Data;
using System.Globalization;
using System.Windows;
using System.Collections.ObjectModel;

namespace Pixtack4
{

    /// <summary>
    /// フルパスからファイル名だけにする
    /// </summary>
    public class MyConvPathFileName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = (string)value;
            return System.IO.Path.GetFileName(path);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConvRectToOffsetLeft : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (Rect)value;
            return -r.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConvRectToOffsetTop : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (Rect)value;
            return -r.Top;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConvRenderTransformBounds : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var parentRTF = (Transform)values[0];
            var children = (ObservableCollection<KisoThumb>)values[1];
            var width = (double)values[2];
            var height = (double)values[3];

            if (children.Count == 0) { return new Rect(); }
            Rect unionRect = GetRenderTrasformBounds(parentRTF, children[0]);
            for (int i = 1; i < children.Count; i++)
            {
                unionRect.Union(GetRenderTrasformBounds(parentRTF, children[i]));
            }
            return unionRect;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        private static Rect GetRenderTrasformBounds(Transform parentRTF, KisoThumb thumb)
        {
            Rect rectZero = new(0, 0, thumb.Width, thumb.Height);
            Rect rect = new(thumb.MyItemData.MyLeft, thumb.MyItemData.MyTop, thumb.Width, thumb.Height);

            //位置の取得A、child自身のTransformを使ったBounds            
            Rect boundsZero = thumb.MyInsideElement.RenderTransform.TransformBounds(rectZero);

            //位置の取得B、parentのTransformを使ったBounds            
            Rect parentTFBounds = parentRTF.TransformBounds(rect);

            //最終的な位置の取得は、AとBの合成(offset)
            Point topLeft = parentTFBounds.TopLeft;
            topLeft.Offset(boundsZero.X, boundsZero.Y);

            //サイズ取得は、parentとchildのTransformを合成したTransformでのBounds
            MatrixTransform unionTF = UnionTransform(parentRTF, thumb.MyInsideElement.RenderTransform);
            Rect unionBounds = unionTF.TransformBounds(rect);

            //最終的な位置とサイズを返す
            return new Rect(topLeft, unionBounds.Size);
        }

        /// <summary>
        /// Transform1にTransform2を追加(Append)したTransformを作って返す
        /// </summary>
        /// <param name="transform1"></param>
        /// <param name="transform2"></param>
        /// <returns></returns>
        private static MatrixTransform UnionTransform(Transform transform1, Transform transform2)
        {
            Matrix union = transform1.Value;
            union.Append(transform2.Value);
            return new MatrixTransform(union);
        }

    }


    public class MyConvRenderBounds : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var width = (double)values[0];
            var height = (double)values[1];
            var tf = (Transform)values[2];
            return tf.TransformBounds(new Rect(0, 0, width, height));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class MyWakuBrushConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            List<Brush> brushes = (List<Brush>)values[0];
            bool b1 = (bool)values[1];
            bool b2 = (bool)values[2];
            bool b3 = (bool)values[3];
            bool b4 = (bool)values[4];

            if (b1) { return brushes[1]; }//IsFocus
            else if (b2) { return brushes[2]; }//IsSelected
            else if (b3) { return brushes[3]; }//IsEelectable
            else if (b4) { return brushes[4]; }//IsActiveGroup
            else { return brushes[0]; }//それ以外
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConverterARGBtoSolidBrush : IMultiValueConverter
    {
        public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var a = (byte)values[0];
            var r = (byte)values[1];
            var g = (byte)values[2];
            var b = (byte)values[3];

            //0,255,255,255だけ特別でTransparent(透明色)を返す
            //それ以外のalpha = 0ならnull
            //null背景色ならクリックが無効化される
            if (a == 0 && r == 255 && g == 255 && b == 255)
            {
                return Brushes.Transparent;
            }
            else if (a == 0)
            {
                return null;
            }
            return new SolidColorBrush(Color.FromArgb(a, r, g, b));

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return [(byte)0, (byte)0, (byte)0, (byte)0];
            }
            var br = (SolidColorBrush)value;
            return [br.Color.A, br.Color.R, br.Color.G, br.Color.B];

        }
    }



}