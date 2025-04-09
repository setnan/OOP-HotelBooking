using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace HotelBooking.AvaloniaApp.Converters;

public class BookingStatusCancelableConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string status)
        {
            return status is "Pending" or "Confirmed";
        }
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
