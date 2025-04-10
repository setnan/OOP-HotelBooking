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
    public static IServiceProvider Services { get; private set; } = default!;

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
            // services.AddSingleton<BackupService>();
            services.AddTransient<BookingsViewModel>();

            // ViewModels
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<BookingsViewModel>();
            services.AddTransient<BookingViewModel>();
            services.AddTransient<RoomManagementViewModel>();
            services.AddTransient<RoomsViewModel>();
            services.AddTransient<GuestViewModel>();
            services.AddTransient<SettingsViewModel>();
            // services.AddTransient<BackupViewModel>();
            services.AddTransient<ClientViewModel>();

            Services = services.BuildServiceProvider();

            // LoginWindow med "onLoginSuccess"-callback
            var loginWindow = new LoginWindow();
            var loginViewModel = new LoginViewModel(user =>
            {
                var mainWindow = new MainWindow
                {
                    DataContext = Services.GetRequiredService<MainWindowViewModel>()
                };

                desktop.MainWindow = mainWindow;
                mainWindow.Show();

                loginWindow.Close();
            });

            loginWindow.DataContext = loginViewModel;
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
