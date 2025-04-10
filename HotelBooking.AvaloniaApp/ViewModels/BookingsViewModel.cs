using System;
using System.Collections.ObjectModel;
using System.Linq;
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
    }

    public async Task InitializeAsync()
    {
        await LoadDataAsync();
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
    private Booking? newBooking;

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
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Feil ved lasting av data: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void CancelNewBooking()
    {
        IsNewBookingDialogOpen = false;
        NewBooking = null;
        SelectedRoom = null;
        SelectedGuest = null;
        ErrorMessage = null;
    }

    [RelayCommand]
    private void ShowNewBookingDialog()
    {
        NewBooking = new Booking
        {
            CheckIn = DateTime.Today,
            CheckOut = DateTime.Today.AddDays(1)
        };
        IsNewBookingDialogOpen = true;
        ErrorMessage = null;
    }

    [RelayCommand]
    private async Task SaveNewBooking()
    {
        if (SelectedGuest == null)
        {
            ErrorMessage = "Vennligst velg en gjest";
            return;
        }

        if (SelectedRoom == null)
        {
            ErrorMessage = "Vennligst velg et rom";
            return;
        }

        if (NewBooking == null)
        {
            ErrorMessage = "Ugyldig booking data";
            return;
        }

        if (NewBooking.CheckIn >= NewBooking.CheckOut)
        {
            ErrorMessage = "Utsjekking må være etter innsjekking";
            return;
        }

        try
        {
            IsLoading = true;
            ErrorMessage = null;

            NewBooking.Guest = SelectedGuest;
            NewBooking.Room = SelectedRoom;
            NewBooking.Status = BookingStatus.Confirmed;

            var success = await bookingService.AddBookingAsync(NewBooking);
            if (success)
            {
                await LoadDataAsync();
                SuccessMessage = "Booking opprettet";
                IsNewBookingDialogOpen = false;
                NewBooking = null;
                SelectedRoom = null;
                SelectedGuest = null;
            }
            else
            {
                ErrorMessage = "Kunne ikke opprette booking";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Feil ved oppretting av booking: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task UpdateBooking()
    {
        if (SelectedBooking == null)
        {
            ErrorMessage = "Ingen booking valgt";
            return;
        }

        try
        {
            IsLoading = true;
            ErrorMessage = null;

            var success = await bookingService.UpdateBookingAsync(SelectedBooking);
            if (success)
            {
                await LoadDataAsync();
                SuccessMessage = "Booking oppdatert";
            }
            else
            {
                ErrorMessage = "Kunne ikke oppdatere booking";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Feil ved oppdatering av booking: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task DeleteBooking()
    {
        if (SelectedBooking == null)
        {
            ErrorMessage = "Ingen booking valgt";
            return;
        }

        try
        {
            IsLoading = true;
            ErrorMessage = null;

            var success = await bookingService.DeleteBookingAsync(SelectedBooking);
            if (success)
            {
                await LoadDataAsync();
                SuccessMessage = "Booking kansellert";
                SelectedBooking = null;
            }
            else
            {
                ErrorMessage = "Kunne ikke kansellere booking";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Feil ved kansellering av booking: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}
