using System;
using System.IO;
using Xamarin.Forms;

namespace TestApp.Converters
{
    public class ByteToImageSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return
                null;

            var imageAsBytes = (byte[])value;

            return ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //var source = value as ImageSource;

            //if (source == null)
            //    return null;

            //using (var ms = new MemoryStream())
            //{
            //    return ms.ToArray();
            //}
            throw new NotImplementedException();
        }
    }
}