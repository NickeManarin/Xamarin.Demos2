using System;
using System.Globalization;
using Xamarin.Forms;

namespace TestApp.Converters
{
    public class BoolToInvertedBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(value as bool? ?? false);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(value as bool? ?? false);
        }
    }
}