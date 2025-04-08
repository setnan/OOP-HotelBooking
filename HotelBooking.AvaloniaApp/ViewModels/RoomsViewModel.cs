using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using HotelBooking.Desktop.Models;
using HotelBooking.Desktop.Services;

namespace HotelBooking.Desktop.ViewModels;

public partial class RoomsViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService;

    [ObservableProperty]
    private string _title = "Available Rooms";

    [ObservableProperty]
    private ObservableCollection<Room> _rooms = new();

    [ObservableProperty]
    private bool _isLoading;

    public RoomsViewModel()
    {
        _databaseService = DatabaseService.Instance;
        LoadRoomsAsync();
    }

    private async void LoadRoomsAsync()
    {
        IsLoading = true;
        var rooms = await _databaseService.GetAvailableRoomsAsync();
        Rooms = new ObservableCollection<Room>(rooms);
        IsLoading = false;
    }
}
