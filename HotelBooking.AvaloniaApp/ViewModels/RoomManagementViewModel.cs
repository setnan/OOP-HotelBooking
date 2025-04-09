using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelBooking.Core.Models;
using HotelBooking.AvaloniaApp.Services;

namespace HotelBooking.AvaloniaApp.ViewModels;

public partial class RoomManagementViewModel : ViewModelBase
{
    private readonly RoomServiceWrapper roomService;

    public RoomManagementViewModel(RoomServiceWrapper roomService)
    {
        this.roomService = roomService;
        LoadDataAsync();
    }

    [ObservableProperty]
    private ObservableCollection<Room> rooms = new();

    [ObservableProperty]
    private Room? selectedRoom;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private string? successMessage;

    private async Task LoadDataAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;
            SuccessMessage = null;

            var roomsList = await roomService.GetAllRoomsAsync();
            Rooms = new ObservableCollection<Room>(roomsList);

            SuccessMessage = "Rooms loaded successfully";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading rooms: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private Task RefreshData() => LoadDataAsync();
}
