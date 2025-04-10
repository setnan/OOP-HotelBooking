using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelBooking.Core.Models;
using HotelBooking.Core.Services;

namespace HotelBooking.AvaloniaApp.ViewModels;

public partial class RoomsViewModel : ViewModelBase
{
    private readonly RoomService roomService;

    [ObservableProperty]
    private string title = "Rooms";

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

    public RoomsViewModel(RoomService roomService)
    {
        this.roomService = roomService;
        _ = LoadDataAsync(); // fyrer av i bakgrunnen uten å vente
    }

    private async Task LoadDataAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;
            SuccessMessage = null;

            var roomsList = await roomService.GetAllAsync();
            Rooms = new ObservableCollection<Room>(roomsList);
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
    private async Task UpdateRoom()
    {
        if (SelectedRoom == null) return;

        try
        {
            IsLoading = true;
            ErrorMessage = null;
            SuccessMessage = null;

            await roomService.UpdateRoomAsync(SelectedRoom);
            await LoadDataAsync();

            SuccessMessage = "Room updated successfully";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error updating room: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}