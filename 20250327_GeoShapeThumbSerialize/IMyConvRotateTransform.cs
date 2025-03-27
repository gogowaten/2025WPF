using System.Globalization;

namespace _20250327_GeoShapeThumbSerialize
{
    public interface IMyConvRotateTransform
    {
        object Convert(object value, Type targetType, object parameter, CultureInfo culture);
        object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
}