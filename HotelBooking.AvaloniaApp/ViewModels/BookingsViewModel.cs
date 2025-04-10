using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelBooking.Core.Models;
using HotelBooking.Core.Services;

namespace HotelBooking.AvaloniaApp.ViewModels;

public partial class BookingsViewModel : ViewModelBase
{
    private readonly BookingService bookingService;
    private readonly RoomService roomService;
    private readonly GuestService guestService;

    public BookingsViewModel(
        BookingService bookingService,
        RoomService roomService,
        GuestService guestService)
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

    [ObservableProperty]
    private bool isNewBookingDialogOpen;
    
    [ObservableProperty]
    private Room? selectedRoom;

    [ObservableProperty]
    private Guest? selectedGuest;

    private async Task LoadDataAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;
            SuccessMessage = null;

            var bookingsTask = bookingService.GetAllAsync();
            var roomsTask = roomService.GetAllAsync();
            var guestsTask = guestService.GetAllAsync();

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
    private void CloseNewBookingDialog()
    {
        IsNewBookingDialogOpen = false;
    }

    [RelayCommand]
    private void ShowNewBookingDialog()
    {
        IsNewBookingDialogOpen = true;
    }
    
    [RelayCommand]
    private async Task CreateBooking()
    {
        if (SelectedGuest == null || SelectedRoom == null)
        {
            ErrorMessage = "Please select both a guest and a room.";
            return;
        }

        try
        {
            IsLoading = true;
            ErrorMessage = null;
            SuccessMessage = null;

            var newBooking = new Booking
            {
                GuestId = SelectedGuest.GuestId,
                RoomId = SelectedRoom.RoomId,
                CheckIn = DateTime.Today,
                CheckOut = DateTime.Today.AddDays(1)
            };

            var success = await bookingService.AddBookingAsync(newBooking);
            if (success)
            {
                SuccessMessage = "Booking created successfully.";
                await LoadDataAsync();
                CloseNewBookingDialog();
            }
            else
            {
                ErrorMessage = "Failed to create booking.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error creating booking: {ex.Message}";
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
                await LoadDataAsync();
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

    // New DeleteBookingAsync method for deleting a booking
    [RelayCommand]
    private async Task DeleteBookingAsync()
    {
        if (SelectedBooking == null)
        {
            ErrorMessage = "No booking selected to delete.";
            return;
        }

        try
        {
            IsLoading = true;
            ErrorMessage = null;
            SuccessMessage = null;

            var success = await bookingService.DeleteBookingAsync(SelectedBooking);
            if (success)
            {
                SuccessMessage = "Booking deleted successfully.";
                await LoadDataAsync();
            }
            else
            {
                ErrorMessage = "Failed to delete booking.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error deleting booking: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    // New methods to update CheckIn and CheckOut dates
    [RelayCommand]
    private async Task UpdateCheckInDateAsync()
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

            SelectedBooking.CheckIn = DateTime.Today;  // You can replace this with a selected date
            var success = await bookingService.UpdateBookingAsync(SelectedBooking);
            if (success)
            {
                SuccessMessage = "Check-in date updated successfully.";
                await LoadDataAsync();
            }
            else
            {
                ErrorMessage = "Failed to update check-in date.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error updating check-in date: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task UpdateCheckOutDateAsync()
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

            SelectedBooking.CheckOut = DateTime.Today.AddDays(1);  // You can replace this with a selected date
            var success = await bookingService.UpdateBookingAsync(SelectedBooking);
            if (success)
            {
                SuccessMessage = "Check-out date updated successfully.";
                await LoadDataAsync();
            }
            else
            {
                ErrorMessage = "Failed to update check-out date.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error updating check-out date: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}
