using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelBooking.Core.Models;
using HotelBooking.Core.Services;

namespace HotelBooking.AvaloniaApp.ViewModels;

public partial class GuestViewModel : ViewModelBase
{
    private readonly GuestService guestService;
    private readonly RoomService roomService;
    
    public GuestViewModel(GuestService guestService, RoomService roomService)
    {
        this.guestService = guestService;
        this.roomService = roomService;
    }

    public async Task InitializeAsync()
    {
        await LoadDataAsync();
    }

    [ObservableProperty]
    private ObservableCollection<Guest> guests = new();

    [ObservableProperty]
    private Guest? selectedGuest;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private string? successMessage;
    
    [ObservableProperty]
    private string newGuestName = "";
    
    [ObservableProperty]
    private string newGuestContact = "";
    
    [ObservableProperty]
    private ObservableCollection<Room> availableRooms = new();  // Liste av tilgjengelige rom
    
    [ObservableProperty]
    private Room? selectedRoom;

    [ObservableProperty]
    private bool isPaneOpen;

    [RelayCommand]
    private async Task AddGuest()
    {
        if (string.IsNullOrEmpty(NewGuestName) || string.IsNullOrEmpty(NewGuestContact))
        {
            ErrorMessage = "Please provide both a name and contact number.";
            return;
        }
        
        var newGuest = new Guest
        {
            Name = NewGuestName,
            ContactNumber = NewGuestContact
            // Hvis vi ønsker å knytte et rom, kan vi bruke selectedRoom her
        };

        try
        {
            IsLoading = true;
            ErrorMessage = SuccessMessage = null;

            var success = await guestService.AddGuestAsync(newGuest);
            if (success)
            {
                Guests.Add(newGuest);
                SuccessMessage = "Guest added successfully!";
                NewGuestName = NewGuestContact = "";
            }
            else
            {
                ErrorMessage = "Failed to add guest.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error adding guest: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = SuccessMessage = null;
            
            var guestsList = await guestService.GetAllAsync();
            Guests = new ObservableCollection<Guest>(guestsList);
            
            var roomsList = await roomService.GetAvailableRoomsAsync();
            AvailableRooms = new ObservableCollection<Room>(roomsList);

            SuccessMessage = "Guests and rooms loaded successfully";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading guests or rooms: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private Task RefreshData() => LoadDataAsync();
    
    [RelayCommand]
    private void TogglePane()
    {
        IsPaneOpen = !IsPaneOpen;
    }
    
    [RelayCommand]
    private async Task UpdateGuest()
    {
        if (SelectedGuest == null)
        {
            ErrorMessage = "Please select a guest to update.";
            return;
        }

        try
        {
            var success = await guestService.UpdateGuestAsync(SelectedGuest);
            if (success)
            {
                SuccessMessage = "Guest updated successfully!";
                await LoadDataAsync();
            }
            else
            {
                ErrorMessage = "Failed to update guest.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error updating guest: {ex.Message}";
        }
    }
    
    [RelayCommand]
    private async Task DeleteGuest()
    {
        if (SelectedGuest == null)
        {
            ErrorMessage = "Please select a guest to delete.";
            return;
        }

        try
        {
            var success = await guestService.DeleteGuestAsync(SelectedGuest);
            if (success)
            {
                Guests.Remove(SelectedGuest);
                SuccessMessage = "Guest deleted successfully.";
                SelectedGuest = null;
            }
            else
            {
                ErrorMessage = "Failed to delete guest.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error deleting guest: {ex.Message}";
        }
    }
}
