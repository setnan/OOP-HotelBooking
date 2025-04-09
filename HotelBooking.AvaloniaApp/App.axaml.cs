using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using HotelBooking.AvaloniaApp.ViewModels;
using HotelBooking.AvaloniaApp.Views;
using Microsoft.Extensions.DependencyInjection;
using HotelBooking.Core.Services;
using HotelBooking.AvaloniaApp.Services;
using HotelBooking.Core.Database;
using Microsoft.Extensions.Configuration;

namespace HotelBooking.AvaloniaApp;

public partial class App : Application
{
    private IServiceProvider? _serviceProvider;

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
            services.AddSingleton<BackupService>();


            // Wrappers
            services.AddScoped<UserServiceWrapper>();
            services.AddScoped<RoomServiceWrapper>();
            services.AddScoped<BookingServiceWrapper>();
            services.AddScoped<ClientServiceWrapper>();
            services.AddScoped<GuestServiceWrapper>();


            // ViewModels
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<BookingsViewModel>();
            services.AddTransient<BookingViewModel>();
            services.AddTransient<RoomManagementViewModel>();
            services.AddTransient<RoomsViewModel>();
            services.AddTransient<GuestViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<BackupViewModel>();


            _serviceProvider = services.BuildServiceProvider();

            desktop.MainWindow = new LoginWindow
            {
                DataContext = _serviceProvider.GetRequiredService<LoginViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    public static T GetService<T>()
    {
        if (Current is App app)
        {
            return app._serviceProvider?.GetRequiredService<T>()
                ?? throw new InvalidOperationException($"Service of type {typeof(T)} not found");
        }
        throw new InvalidOperationException("Application not initialized");
    }
}
