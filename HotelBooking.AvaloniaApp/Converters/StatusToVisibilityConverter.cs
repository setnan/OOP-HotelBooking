using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Data;
using HotelBooking.Core.Models;

namespace HotelBooking.AvaloniaApp.Converters;

public class StatusToVisibilityConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool status && targetType.IsAssignableFrom(typeof(bool)))
        {
            return status;
        }

        return BindingOperations.DoNothing;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
