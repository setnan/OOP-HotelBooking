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
    private readonly Action<User> onLoginSuccess;

    public LoginViewModel(Action<User> onLoginSuccess)
    {
        this.onLoginSuccess = onLoginSuccess;
        this.userService = new UserServiceWrapper(); // Du kan også injecte hvis ønskelig
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

            var user = await userService.AuthenticateAsync(Username, Password);
            if (user == null)
            {
                ErrorMessage = "Ugyldig brukernavn eller passord";
                return;
            }

            if (RememberMe)
            {
                await userService.SaveCredentialsAsync(Username, Password);
            }
            
            onLoginSuccess(user);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Innlogging feilet: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}