using System;
using System.Globalization;
using Avalonia.Data.Converters;
using HotelBooking.Core.Models;

namespace HotelBooking.AvaloniaApp.Converters;

public class RoleTextConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string role)
        {
            return role switch
            {
                "Admin" => "Administrator",
                "User" => "Regular User",
                _ => role
            };
        }
        return value?.ToString();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
