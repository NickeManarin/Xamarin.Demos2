using System;
using System.Globalization;
using Xamarin.Forms;

namespace TestApp.Converters
{
    public class IntToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var source = value as int?;

            if (!source.HasValue || !int.TryParse(parameter as string, out var desired))
                return false;

            return source.Value == desired;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}