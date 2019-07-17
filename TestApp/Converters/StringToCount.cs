using System;
using System.Globalization;
using Xamarin.Forms;

namespace TestApp.Converters
{
    public class StringToCount : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value as string;
            var param = parameter as string;

            if (string.IsNullOrWhiteSpace(param))
                return "";

            var maxMin = param.Split('.');

            var length = string.IsNullOrEmpty(text) ? 0 : text.Length;

            if (!int.TryParse(maxMin[0], out var max) || !int.TryParse(maxMin.Length > 1 ? maxMin[1] : "0", out var min))
                return "";

            if (length < min)
            {
                var minDiff = min - length;

                if (minDiff > 1)
                    return minDiff + " caracteres restantes";

                return "1 caractere restante";
            }

            if (length <= max)
                return length + "/" + max;

            return $"Limite de caracteres atingido ({length:n0}/{max:n0})";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}