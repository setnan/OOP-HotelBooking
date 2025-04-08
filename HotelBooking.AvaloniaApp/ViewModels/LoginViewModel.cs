using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelBooking.Desktop.Services;
using HotelBooking.Desktop.Views;

namespace HotelBooking.Desktop.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public LoginViewModel()
    {
        _databaseService = DatabaseService.Instance;
    }

    [RelayCommand]
    private async Task Login()
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Please enter both email and password";
            return;
        }

        var user = await _databaseService.ValidateUserAsync(Email, Password);
        if (user != null)
        {
            // Store the logged-in user for the session
            App.CurrentUser = user;
            
            // Create new window with the main view
            var mainWindow = new Window
            {
                Width = 1200,
                Height = 700,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Content = new MainView(),
                DataContext = new MainWindowViewModel()
            };

            // Show the new window
            mainWindow.Show();

            // Close the login window
            if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop &&
                desktop.MainWindow != null)
            {
                var oldWindow = desktop.MainWindow;
                desktop.MainWindow = mainWindow;
                oldWindow.Close();
            }
        }
        else
        {
            ErrorMessage = "Invalid email or password";
        }
    }
}
