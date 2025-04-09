using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelBooking.Core.Models;
using HotelBooking.AvaloniaApp.Services;

namespace HotelBooking.AvaloniaApp.ViewModels;

public partial class BookingsViewModel : ViewModelBase
{
    private readonly BookingServiceWrapper bookingService;
    private readonly RoomServiceWrapper roomService;
    private readonly GuestServiceWrapper guestService;

    public BookingsViewModel(
        BookingServiceWrapper bookingService,
        RoomServiceWrapper roomService,
        GuestServiceWrapper guestService)
    {
        this.bookingService = bookingService;
        this.roomService = roomService;
        this.guestService = guestService;
        LoadDataAsync();
    }

    [ObservableProperty]
    private ObservableCollection<Booking> bookings = new();

    [ObservableProperty]
    private ObservableCollection<Room> availableRooms = new();

    [ObservableProperty]
    private ObservableCollection<Guest> guests = new();

    [ObservableProperty]
    private Booking? selectedBooking;

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

            var bookingsTask = bookingService.GetAllBookingsAsync();
            var roomsTask = roomService.GetAllRoomsAsync();
            var guestsTask = guestService.GetAllGuestsAsync();

            await Task.WhenAll(bookingsTask, roomsTask, guestsTask);

            Bookings = new ObservableCollection<Booking>(await bookingsTask);
            AvailableRooms = new ObservableCollection<Room>(await roomsTask);
            Guests = new ObservableCollection<Guest>(await guestsTask);

            SuccessMessage = "Data loaded successfully";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading data: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task UpdateBookingAsync()
    {
        if (SelectedBooking == null)
        {
            ErrorMessage = "No booking selected to update.";
            return;
        }

        try
        {
            IsLoading = true;
            ErrorMessage = null;
            SuccessMessage = null;

            var success = await bookingService.UpdateBookingAsync(SelectedBooking);
            if (success)
            {
                SuccessMessage = "Booking updated successfully.";
                await LoadDataAsync(); // Oppdater listen
            }
            else
            {
                ErrorMessage = "Failed to update booking.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error updating booking: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}
