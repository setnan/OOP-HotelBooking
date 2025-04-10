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

        _viewModel = App.GetService<LoginViewModel>();
        DataContext = _viewModel;
        _viewModel.LoginSuccessful += OnLoginSuccessful;
        
        Loaded += LoginWindow_Loaded;
    }

    private async void LoginWindow_Loaded(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e)
    {
        await _viewModel.TryAutoLoginAsync();
    }

    private void OnLoginSuccessful(object? sender, User user)
    {
        if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainWindow = App.GetService<MainWindow>();
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