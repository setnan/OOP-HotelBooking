using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace HotelBooking.AvaloniaApp.Converters;

public class BookingStatusColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string status)
        {
            return status switch
            {
                "Pending" => new SolidColorBrush(Colors.Orange),
                "Confirmed" => new SolidColorBrush(Colors.Green),
                "Cancelled" => new SolidColorBrush(Colors.Red),
                "Completed" => new SolidColorBrush(Colors.Blue),
                _ => new SolidColorBrush(Colors.Gray)
            };
        }
        return new SolidColorBrush(Colors.Gray);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
