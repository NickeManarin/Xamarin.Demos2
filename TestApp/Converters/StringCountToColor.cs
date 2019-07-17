using System;
using System.Globalization;
using Xamarin.Forms;

namespace TestApp.Converters
{
    public class StringCountToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value as string;
            var param = parameter as string;

            if (string.IsNullOrWhiteSpace(param))
                return Color.Black;

            var maxMin = param.Split('.');

            var length = string.IsNullOrEmpty(text) ? 0 : text.Length;

            if (!int.TryParse(maxMin[0], out var max) || !int.TryParse(maxMin.Length > 1 ? maxMin[1] : "0", out var min))
                return Color.Crimson;

            if (length < min)
                return Color.Crimson;

            if (length <= max)
                return Color.Black;

            return Color.Crimson;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}