using CommunityToolkit.Mvvm.ComponentModel;

namespace HotelBooking.AvaloniaApp.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    [ObservableProperty]
    private string title = "Settings";

    [ObservableProperty]
    private bool darkMode;

    [ObservableProperty]
    private string language = "English";

    [ObservableProperty]
    private bool notificationsEnabled = true;
}
