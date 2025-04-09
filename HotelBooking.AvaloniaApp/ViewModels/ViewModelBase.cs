using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBooking.AvaloniaApp.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    protected static T GetService<T>()
    {
        if (App.Current is App app)
        {
            return app.GetService<T>();
        }
        throw new InvalidOperationException("Cannot get service. Application not initialized.");
    }
}
