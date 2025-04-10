using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HotelBooking.AvaloniaApp.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    protected static T GetService<T>()
    {
        return App.GetService<T>();
    }
}