using CommunityToolkit.Mvvm.ComponentModel;

namespace HotelBooking.Desktop.ViewModels;

public partial class DashboardViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _welcomeMessage;

    public DashboardViewModel()
    {
        _welcomeMessage = $"Welcome {App.CurrentUser?.Name}!";
    }
}
