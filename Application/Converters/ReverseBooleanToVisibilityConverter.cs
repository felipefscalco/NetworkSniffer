using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Application.Converters
{
    public class ReverseBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                if (boolValue)
                    return Visibility.Hidden;
                else
                    return Visibility.Visible;
            }

            throw new ArgumentOutOfRangeException(nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}