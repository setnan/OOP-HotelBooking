using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Data;

namespace HotelBooking.AvaloniaApp.Converters;

public class StatusToVisibilityConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return false;

        var status = value.ToString();
        var expected = parameter.ToString();

        return string.Equals(status, expected, StringComparison.OrdinalIgnoreCase);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}