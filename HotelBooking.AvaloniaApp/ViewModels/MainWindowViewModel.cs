using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HotelBooking.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase? _currentPage;

    public MainWindowViewModel()
    {
        // Start with dashboard view
        NavigateToDashboard();
    }

    [RelayCommand]
    private void NavigateToDashboard()
    {
        CurrentPage = new DashboardViewModel();
    }

    [RelayCommand]
    private void NavigateToRooms()
    {
        CurrentPage = new RoomsViewModel();
    }

    [RelayCommand]
    private void NavigateToReservation()
    {
        CurrentPage = new ReservationViewModel();
    }

    [RelayCommand]
    private void NavigateToBookings()
    {
        CurrentPage = new BookingsViewModel();
    }

    [RelayCommand]
    private void NavigateToSettings()
    {
        CurrentPage = new SettingsViewModel();
    }
}
