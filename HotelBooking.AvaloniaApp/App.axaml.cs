using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using HotelBooking.AvaloniaApp.Views;
using HotelBooking.AvaloniaApp.ViewModels;

namespace HotelBooking.AvaloniaApp;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // 🚀 Vis BookingsView direkte i et vindu for testing
            desktop.MainWindow = new Window
            {
                Width = 800,
                Height = 600,
                Content = new BookingsView
                {
                    DataContext = new BookingsViewModel()
                }
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}