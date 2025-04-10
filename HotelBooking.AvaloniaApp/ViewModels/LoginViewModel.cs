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

    public event EventHandler<User>? LoginSuccessful;

    public LoginViewModel(UserService userService)
    {
        _userService = userService;
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
                // Her kan vi vurdere om vi vil implementer lagring etterhvert
            }

            UserSession.Instance.Login(user);
            LoginSuccessful?.Invoke(this, user);
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

    public async Task TryAutoLoginAsync()
    {
        // Her kan vi legge til "husket bruker automatisk login" senere
    }
}