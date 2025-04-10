using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelBooking.Core.Models;
using HotelBooking.Core.Services;

namespace HotelBooking.AvaloniaApp.ViewModels;

public partial class DashboardViewModel : ViewModelBase
{
    private readonly BookingService bookingService;
    private readonly RoomService roomService;
    private readonly GuestService guestService;

    public DashboardViewModel(
        BookingService bookingService,
        RoomService roomService,
        GuestService guestService)
    {
        this.bookingService = bookingService;
        this.roomService = roomService;
        this.guestService = guestService;
        LoadDashboardDataAsync();
    }

    [ObservableProperty] private string welcomeMessage = "Welcome to Hotel Management";
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private string? errorMessage;
    [ObservableProperty] private string? successMessage;

    [ObservableProperty] private int todayCheckIns;
    [ObservableProperty] private int todayCheckOuts;
    [ObservableProperty] private int availableRooms;
    [ObservableProperty] private double occupancyRate;

    [ObservableProperty] private ObservableCollection<Booking> todaysBookings = new();
    [ObservableProperty] private ObservableCollection<Booking> upcomingBookings = new();
    [ObservableProperty] private ObservableCollection<Room> roomsNeedingAttention = new();

    [RelayCommand]
    private async Task RefreshDashboard() => await LoadDashboardDataAsync();

    [RelayCommand]
    private async Task CheckInBooking(Booking booking)
    {
        booking.Status = BookingStatus.CheckedIn;
        await bookingService.UpdateBookingAsync(booking);
        await RefreshDashboard();
    }

    [RelayCommand]
    private async Task CheckOutBooking(Booking booking)
    {
        booking.Status = BookingStatus.CheckedOut;
        await bookingService.UpdateBookingAsync(booking);
        await RefreshDashboard();
    }
    
    [RelayCommand]
    private async Task MarkRoomCleaned(Room room)
    {
        await roomService.UpdateRoomAvailabilityAsync(room, true);
        await LoadDashboardDataAsync();
    }

    private async Task LoadDashboardDataAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;
            SuccessMessage = null;

            var allBookings = await bookingService.GetAllBookingsAsync();
            var allRooms = await roomService.GetAllRoomsAsync();
            var available = await roomService.GetAvailableRoomsAsync();

            var today = DateTime.Today;
            TodaysBookings = new ObservableCollection<Booking>(allBookings.Where(b => b.CheckIn.Date == today));
            UpcomingBookings = new ObservableCollection<Booking>(allBookings.Where(b => b.CheckIn > today));

            TodayCheckIns = TodaysBookings.Count;
            TodayCheckOuts = allBookings.Count(b => b.CheckOut.Date == today);
            AvailableRooms = available.Count;
            OccupancyRate = allRooms.Count > 0 ? (double)(allRooms.Count - AvailableRooms) / allRooms.Count : 0;

            RoomsNeedingAttention = new ObservableCollection<Room>(allRooms.Where(r => !r.IsAvailable));

            SuccessMessage = "Dashboard loaded successfully";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading dashboard data: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}