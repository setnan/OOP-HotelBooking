using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelBooking.Core.Models;
using HotelBooking.Core.Services;

namespace HotelBooking.AvaloniaApp.ViewModels;

public partial class RoomManagementViewModel : ViewModelBase
{
    private readonly RoomService roomService;

    public RoomManagementViewModel(RoomService roomService)
    {
        this.roomService = roomService;
        LoadDataAsync();
    }

    [ObservableProperty] private ObservableCollection<Room> rooms = new();
    [ObservableProperty] private Room? selectedRoom;
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private string? errorMessage;
    [ObservableProperty] private string? successMessage;

    [ObservableProperty] private bool isNewRoomDialogOpen;
    [ObservableProperty] private bool isEditRoomDialogOpen;

    [ObservableProperty] private int roomNumber;
    [ObservableProperty] private int floor;
    [ObservableProperty] private string? type;
    [ObservableProperty] private int numberOfBeds = 1;
    [ObservableProperty] private string? quality;
    [ObservableProperty] private string? description;

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

    [RelayCommand]
    private void ShowNewRoomDialog()
    {
        ResetRoomForm();
        IsNewRoomDialogOpen = true;
    }

    [RelayCommand]
    private async Task CreateRoom()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;
            SuccessMessage = null;

            var room = new Room
            {
                RoomNumber = RoomNumber.ToString(),
                Floor = Floor,
                Type = Type,
                NumberOfBeds = NumberOfBeds,
                Quality = Quality,
                Description = Description,
                IsAvailable = true
            };

            var result = await roomService.AddRoomAsync(room);
            if (result)
            {
                SuccessMessage = "Room created successfully";
                await LoadDataAsync();
                IsNewRoomDialogOpen = false;
            }
            else
            {
                ErrorMessage = "Failed to create room.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error creating room: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void ResetRoomForm()
    {
        RoomNumber = 0;
        Floor = 0;
        Type = "";
        NumberOfBeds = 1;
        Quality = "";
        Description = "";
    }
}
