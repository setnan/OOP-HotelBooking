using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelBooking.Core.Models;
using HotelBooking.Core.Services;
using HotelBooking.Core.Database;

namespace HotelBooking.AvaloniaApp.ViewModels;

public partial class GuestViewModel : ViewModelBase
{
    private readonly GuestService guestService;
    private readonly RoomService roomService;
    private readonly DatabaseConnection db;
    
    public GuestViewModel(GuestService guestService, RoomService roomService, DatabaseConnection db)
    {
        this.guestService = guestService;
        this.roomService = roomService;
        this.db = db;
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
    private ObservableCollection<Room> availableRooms = new();
    
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

        // Sjekk om gjesten allerede eksisterer
        var existingGuest = Guests.FirstOrDefault(g => 
            g.Name.Equals(NewGuestName, StringComparison.OrdinalIgnoreCase) && 
            g.ContactNumber == NewGuestContact);

        if (existingGuest != null)
        {
            ErrorMessage = "A guest with this name and contact number already exists.";
            return;
        }
        
        var newGuest = new Guest
        {
            Name = NewGuestName,
            ContactNumber = NewGuestContact
        };

        try
        {
            IsLoading = true;
            ErrorMessage = SuccessMessage = null;

            db.Open();
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
            db.Close();
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
            
            db.Open();
            var guestsList = await guestService.GetAllAsync();
            
            // Fjern duplikater basert på navn og kontaktnummer
            var uniqueGuests = guestsList
                .GroupBy(g => new { g.Name, g.ContactNumber })
                .Select(g => g.First())
                .OrderBy(g => g.Name);
            
            Guests = new ObservableCollection<Guest>(uniqueGuests);
            
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
            db.Close();
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task RefreshDataAsync()
    {
        await LoadDataAsync();
    }
    
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
            // Sjekk om oppdateringen vil skape en duplikat
            var potentialDuplicate = Guests.FirstOrDefault(g => 
                g.GuestId != SelectedGuest.GuestId && 
                g.Name.Equals(SelectedGuest.Name, StringComparison.OrdinalIgnoreCase) && 
                g.ContactNumber == SelectedGuest.ContactNumber);

            if (potentialDuplicate != null)
            {
                ErrorMessage = "A guest with this name and contact number already exists.";
                return;
            }

            db.Open();
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
        finally
        {
            db.Close();
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
            IsLoading = true;
            ErrorMessage = SuccessMessage = null;

            db.Open();
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
        finally
        {
            db.Close();
            IsLoading = false;
        }
    }
}
