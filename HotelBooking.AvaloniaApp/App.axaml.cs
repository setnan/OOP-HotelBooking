using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using HotelBooking.AvaloniaApp.ViewModels;
using HotelBooking.AvaloniaApp.Views;
using Microsoft.Extensions.DependencyInjection;
using HotelBooking.Core.Services;
using HotelBooking.Core.Database;
using Microsoft.Extensions.Configuration;

namespace HotelBooking.AvaloniaApp;

public partial class App : Application
{
    public static IServiceProvider? Services { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            // Database connection
            services.AddSingleton<DatabaseConnection>();

            // Core services
            services.AddSingleton<UserSession>();
            services.AddSingleton<RoomService>();
            services.AddSingleton<BookingService>();
            services.AddSingleton<ClientService>();
            services.AddSingleton<GuestService>();
            services.AddSingleton<EventService>();

            // ViewModels
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<BookingsViewModel>();
            services.AddTransient<BookingViewModel>();
            services.AddTransient<RoomManagementViewModel>();
            services.AddTransient<RoomsViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<ClientViewModel>();
            services.AddTransient<GuestViewModel>(provider =>
                new GuestViewModel(
                    provider.GetRequiredService<GuestService>(),
                    provider.GetRequiredService<RoomService>()
                )
            );

            Services = services.BuildServiceProvider();

            var loginWindow = new LoginWindow();
            desktop.MainWindow = loginWindow;
            loginWindow.Show();
        }

        base.OnFrameworkInitializationCompleted();
    }

    public static T GetService<T>() where T : notnull
    {
        return Services.GetRequiredService<T>();
    }
}
