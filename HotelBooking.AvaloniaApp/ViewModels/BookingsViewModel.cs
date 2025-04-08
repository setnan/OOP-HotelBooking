using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using HotelBooking.Core.Models;
using HotelBooking.Core.Services;

namespace HotelBooking.Desktop.ViewModels;

public partial class BookingsViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService;

    [ObservableProperty]
    private ObservableCollection<Booking> _bookings = new();

    [ObservableProperty]
    private bool _isLoading;

    public BookingsViewModel()
    {
        _databaseService = DatabaseService.Instance;
        LoadBookingsAsync();
    }

    private async void LoadBookingsAsync()
    {
        IsLoading = true;
        // TODO: Replace with actual guest ID
        var bookings = await _databaseService.GetBookingsAsync(1);
        Bookings = new ObservableCollection<Booking>(bookings);
        IsLoading = false;
    }
}
