using System;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Animation;
using Avalonia.Styling;
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

    private bool IsMacOS => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

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
            services.AddSingleton<GuestService>();
            services.AddSingleton<BookingService>();
            services.AddSingleton<EventService>();
            services.AddSingleton<SettingsService>();

            // Views
            services.AddTransient<MainWindow>();
            services.AddTransient<LoginWindow>();
            services.AddTransient<BookingsView>();
            services.AddTransient<DashboardView>();
            services.AddTransient<GuestView>();
            services.AddTransient<ClientView>();
            services.AddTransient<EventView>();
            services.AddTransient<RoomView>();

            // ViewModels
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<BookingsViewModel>();
            services.AddTransient<GuestViewModel>();
            services.AddTransient<ClientViewModel>();
            services.AddTransient<EventViewModel>();
            services.AddTransient<RoomViewModel>();

            Services = services.BuildServiceProvider();

            try
            {
                var loginWindow = App.GetService<LoginWindow>();
                desktop.MainWindow = loginWindow;

                // Legg til macOS-spesifikk animasjon
                if (IsMacOS)
                {
                    loginWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    loginWindow.Show();

                    // Bruk Avalonia's innebygde animasjonssystem
                    var animation = new Animation
                    {
                        Duration = TimeSpan.FromSeconds(0.3),
                        FillMode = FillMode.Forward,
                        Children =
                        {
                            new KeyFrame
                            {
                                Cue = new Cue(0d),
                                Setters = { new Setter(Window.OpacityProperty, 0d) }
                            },
                            new KeyFrame
                            {
                                Cue = new Cue(1d),
                                Setters = { new Setter(Window.OpacityProperty, 1d) }
                            }
                        }
                    };

                    animation.RunAsync(loginWindow);
                }
                else
                {
                    loginWindow.Show();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resolving service: {ex.Message}");
            }
        }

        base.OnFrameworkInitializationCompleted();
    }

    public static T GetService<T>() where T : notnull
    {
        if (Services == null)
        {
            throw new InvalidOperationException("Services have not been initialized.");
        }

        try
        {
            var service = Services.GetService<T>();
            if (service == null)
            {
                throw new InvalidOperationException($"Service of type {typeof(T).Name} not found.");
            }

            return service;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Error resolving service: {ex.Message}");
            throw;
        }
    }
}