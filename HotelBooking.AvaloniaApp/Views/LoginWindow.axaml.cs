using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using HotelBooking.AvaloniaApp.ViewModels;
using HotelBooking.Core.Models;
using System;

namespace HotelBooking.AvaloniaApp.Views;

public partial class LoginWindow : Window
{
    private readonly LoginViewModel _viewModel;

    public LoginWindow()
    {
        InitializeComponent();

        _viewModel = App.Services?.GetService(typeof(LoginViewModel)) as LoginViewModel
                     ?? throw new InvalidOperationException("Failed to resolve LoginViewModel");

        DataContext = _viewModel;
        _viewModel.LoginSuccessful += OnLoginSuccessful;
        
        _viewModel.TryAutoLoginAsync();
    }

    private void OnLoginSuccessful(object? sender, User user)
    {
        var mainWindow = new MainWindow();

        if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = mainWindow;
            mainWindow.Show();
            Close();
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        _viewModel.LoginSuccessful -= OnLoginSuccessful;
    }
}