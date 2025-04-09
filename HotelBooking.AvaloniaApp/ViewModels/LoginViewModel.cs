using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelBooking.Core.Models;
using HotelBooking.AvaloniaApp.Services;

namespace HotelBooking.AvaloniaApp.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly UserServiceWrapper userService;

    public LoginViewModel(UserServiceWrapper userService)
    {
        this.userService = userService;
    }

    [ObservableProperty]
    private string username = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private bool rememberMe;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string? errorMessage;

    [RelayCommand]
    private async Task Login()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;

            var user = await userService.LoginAsync(Username, Password);
            if (user == null)
            {
                ErrorMessage = "Invalid username or password";
                return;
            }

            if (RememberMe)
            {
                await userService.SaveCredentialsAsync(Username, Password);
            }

            // Navigate to main view
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Login failed: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}
