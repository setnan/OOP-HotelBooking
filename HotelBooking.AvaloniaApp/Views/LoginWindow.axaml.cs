using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using HotelBooking.AvaloniaApp.ViewModels;
using HotelBooking.Core.Models;

namespace HotelBooking.AvaloniaApp.Views;

public partial class LoginWindow : Window
{
    private readonly LoginViewModel _viewModel;

    public LoginWindow()
    {
        InitializeComponent();

        _viewModel = App.Current?.Services?.GetService<LoginViewModel>()
            ?? throw new System.InvalidOperationException("Failed to resolve LoginViewModel");

        DataContext = _viewModel;
        _viewModel.LoginSuccessful += OnLoginSuccessful;

        // Try auto-login if credentials are saved
        _viewModel.TryAutoLoginAsync();
    }

    private void OnLoginSuccessful(object? sender, User user)
    {
        // Create and show the main window
        var mainWindow = new MainWindow();

        if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Set the new window as main and close the login window
            desktop.MainWindow = mainWindow;
            Close();
        }
    }

    protected override void OnClosed(System.EventArgs e)
    {
        base.OnClosed(e);
        _viewModel.LoginSuccessful -= OnLoginSuccessful;
    }
}
