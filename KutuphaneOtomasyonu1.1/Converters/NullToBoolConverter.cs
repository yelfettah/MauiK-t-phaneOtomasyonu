using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace KutuphaneOtomasyonu.Converters
{
    /// <summary>
   /// </summary>
    public class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = value != null;

            if (parameter is string param && param.ToLower() == "invert")
                result = !result;

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Tek yönlü dönüştürme desteklenir.");
        }
    }
}
