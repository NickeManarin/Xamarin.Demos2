﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace TestApp.Converters
{
    /// <summary>
    /// Converter that returns true when a given string is not null or empty.
    /// </summary>
    public class StringToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !string.IsNullOrEmpty(value as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}