using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelBooking.Core.Models;
using HotelBooking.Core.Services;
using System.Linq;

namespace HotelBooking.AvaloniaApp.ViewModels;

public partial class RoomViewModel : ViewModelBase
{
    private readonly RoomService _roomService;

    [ObservableProperty]
    private string roomNumber = "";

    [ObservableProperty]
    private string type = "";

    [ObservableProperty]
    private decimal price;

    [ObservableProperty]
    private bool isAvailable = true;

    [ObservableProperty]
    private ObservableCollection<Room> rooms = new();

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string? errorMessage;

    public RoomViewModel(RoomService roomService)
    {
        _roomService = roomService;
    }

    public async Task InitializeAsync()
    {
        await LoadRoomsAsync();
    }

    [RelayCommand]
    private async Task LoadRoomsAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;
            var roomsList = await _roomService.GetAllAsync();
            Rooms = new ObservableCollection<Room>(roomsList.OrderBy(r => r.RoomNumber));
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Feil ved lasting av rom: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task AddRoomAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;

            // Sjekk om romnummeret allerede eksisterer
            if (Rooms.Any(r => r.RoomNumber == RoomNumber))
            {
                ErrorMessage = $"Rom {RoomNumber} eksisterer allerede";
                return;
            }

            var newRoom = new Room
            {
                HotelId = 1, // Default hotel
                RoomNumber = RoomNumber,
                Type = Type,
                Price = Price,
                IsAvailable = IsAvailable
            };

            if (await _roomService.AddRoomAsync(newRoom))
            {
                await LoadRoomsAsync();

                // Reset form
                RoomNumber = "";
                Type = "";
                Price = 0;
                IsAvailable = true;
            }
            else
            {
                ErrorMessage = "Kunne ikke legge til rommet";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Feil ved oppretting av rom: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task UpdateRoom(Room room)
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;

            if (await _roomService.UpdateRoomAsync(room))
            {
                await LoadRoomsAsync();
            }
            else
            {
                ErrorMessage = "Kunne ikke oppdatere rommet";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Feil ved oppdatering av rom: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task DeleteRoom(Room room)
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;

            if (await _roomService.DeleteRoomAsync(room))
            {
                await LoadRoomsAsync();
            }
            else
            {
                ErrorMessage = "Kunne ikke slette rommet";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Feil ved sletting av rom: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}
