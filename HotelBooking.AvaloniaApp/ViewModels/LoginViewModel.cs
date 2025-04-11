using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelBooking.AvaloniaApp.Services;
using HotelBooking.Core.Models;
using HotelBooking.Core.Services;

namespace HotelBooking.AvaloniaApp.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly UserService _userService;
    private readonly SettingsService _settingsService;

    public event EventHandler<User>? LoginSuccessful;

    public LoginViewModel(UserService userService, SettingsService settingsService)
    {
        _userService = userService;
        _settingsService = settingsService;
        LoadSavedCredentials();
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

    private async void LoadSavedCredentials()
    {
        var settings = await _settingsService.LoadUserCredentialsAsync();
        if (settings != null)
        {
            Username = settings.Email ?? string.Empty;
            Password = settings.Password ?? string.Empty;
            RememberMe = true;
        }
    }

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
                await _settingsService.SaveUserCredentialsAsync(Username, Password);
            }
            else
            {
                await _settingsService.ClearUserCredentialsAsync();
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
        try
        {
            IsLoading = true;
            ErrorMessage = null;
            
            // TODO: Implement auto-login logic here when ready
            // For now, just simulate a delay
            await Task.Delay(100);
        }
        finally
        {
            IsLoading = false;
        }
    }
}