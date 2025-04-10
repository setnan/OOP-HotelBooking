using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelBooking.Core.Models;
using HotelBooking.Core.Services;

namespace HotelBooking.AvaloniaApp.ViewModels;

public partial class BookingViewModel : ViewModelBase
{
    private readonly BookingService bookingService;
    private readonly RoomService roomService;
    private readonly GuestService guestService;

    public BookingViewModel(
        BookingService bookingService,
        RoomService roomService,
        GuestService guestService)
    {
        this.bookingService = bookingService;
        this.roomService = roomService;
        this.guestService = guestService;
        LoadDataAsync();
    }

    [ObservableProperty] private ObservableCollection<Booking> bookings = new();
    [ObservableProperty] private ObservableCollection<Room> availableRooms = new();
    [ObservableProperty] private ObservableCollection<Guest> guests = new();

    [ObservableProperty] private Booking? selectedBooking;
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private string? errorMessage;
    [ObservableProperty] private string? successMessage;
    [ObservableProperty] private bool isNewBookingDialogOpen;

    [ObservableProperty] private Room? selectedRoom;
    [ObservableProperty] private Guest? selectedGuest;
    [ObservableProperty] private DateTime checkIn = DateTime.Today;
    [ObservableProperty] private DateTime checkOut = DateTime.Today.AddDays(1);

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
    private void ShowNewBookingDialog()
    {
        ResetNewBookingForm();
        IsNewBookingDialogOpen = true;
    }

    [RelayCommand]
    private async Task CreateBooking()
    {
        if (SelectedRoom == null || SelectedGuest == null) return;

        try
        {
            IsLoading = true;
            ErrorMessage = null;
            SuccessMessage = null;

            var booking = new Booking
            {
                RoomId = SelectedRoom.RoomId,
                GuestId = SelectedGuest.GuestId,
                CheckIn = CheckIn,
                CheckOut = CheckOut
            };

            await bookingService.AddBookingAsync(booking);
            await LoadDataAsync();

            ResetNewBookingForm();
            IsNewBookingDialogOpen = false;
            SuccessMessage = "Booking created successfully";
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
    private async Task UpdateBooking()
    {
        if (SelectedBooking == null) return;

        try
        {
            IsLoading = true;
            ErrorMessage = null;
            SuccessMessage = null;

            await bookingService.UpdateBookingAsync(SelectedBooking);
            await LoadDataAsync();

            SuccessMessage = "Booking updated successfully";
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

    [RelayCommand]
    private async Task CancelBooking()
    {
        if (SelectedBooking == null) return;

        try
        {
            IsLoading = true;
            ErrorMessage = null;
            SuccessMessage = null;

            await bookingService.DeleteBookingAsync(SelectedBooking);
            await LoadDataAsync();

            SelectedBooking = null;
            SuccessMessage = "Booking cancelled successfully";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error cancelling booking: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void ResetNewBookingForm()
    {
        SelectedRoom = null;
        SelectedGuest = null;
        CheckIn = DateTime.Today;
        CheckOut = DateTime.Today.AddDays(1);
    }
}
