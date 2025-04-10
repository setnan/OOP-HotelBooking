using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelBooking.Core.Models;
using HotelBooking.Core.Services;

namespace HotelBooking.AvaloniaApp.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly UserService _userService;
    private readonly Action<User> _onLoginSuccess;

    public LoginViewModel(UserService userService, Action<User> onLoginSuccess)
    {
        _userService = userService;
        _onLoginSuccess = onLoginSuccess;
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

            var user = await _userService.GetUserByEmailAsync(Username);
            if (user == null || !await _userService.ValidatePasswordAsync(user, Password))
            {
                ErrorMessage = "Ugyldig brukernavn eller passord";
                return;
            }

            if (RememberMe)
            {
                // Her kan vi implementere en lagringsmetode hvis ønskelig
            }

            UserSession.Instance.Login(user); // sett innlogget bruker
            _onLoginSuccess(user);
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