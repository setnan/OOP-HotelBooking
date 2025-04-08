using CommunityToolkit.Mvvm.ComponentModel;

namespace HotelBooking.Desktop.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _title = "Settings";
}
