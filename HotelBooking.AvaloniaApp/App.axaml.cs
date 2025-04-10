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
using Dapper;
using HotelBooking.AvaloniaApp.Services;
using HotelBooking.Core.Utilities;
using HotelBooking.Core.Models;

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
        // Register enum handler for Dapper
        SqlMapper.AddTypeHandler(new EnumAsStringTypeHandler<BookingStatus>());

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            IServiceCollection services = new ServiceCollection();

            // Configuration
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddUserSecrets<App>()
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            // Database connection
            services.AddSingleton<DatabaseConnection>();

            // Core services - order matters for dependencies
            services.AddSingleton<UserSession>();
            services.AddSingleton<UserService>();
            services.AddSingleton<RoleService>(_ => RoleService.Instance);
            services.AddSingleton<EventRoomService>();
            services.AddSingleton<EventClientService>();
            services.AddSingleton<ClientService>();
            services.AddSingleton<RoomService>();
            services.AddSingleton<BookingService>();
            services.AddSingleton<GuestService>();
            services.AddSingleton<EventService>();  // Moved to end since it depends on other services

            // Views
            services.AddTransient<MainWindow>();
            services.AddTransient<LoginWindow>();
            services.AddTransient<BookingsView>();
            services.AddTransient<DashboardView>();
            services.AddTransient<RoomManagementView>();
            services.AddTransient<GuestView>();
            services.AddTransient<ClientView>();

            // ViewModels
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<BookingsViewModel>();
            services.AddTransient<RoomManagementViewModel>();
            services.AddTransient<GuestViewModel>();
            services.AddTransient<ClientViewModel>();

            Services = services.BuildServiceProvider();

            var loginWindow = App.GetService<LoginWindow>();
            desktop.MainWindow = loginWindow;
            loginWindow.Show();
        }

        base.OnFrameworkInitializationCompleted();
    }

    public static T GetService<T>() where T : notnull
    {
        if (Services == null)
        {
            throw new InvalidOperationException("Services have not been initialized");
        }
        return Services.GetRequiredService<T>();
    }
}