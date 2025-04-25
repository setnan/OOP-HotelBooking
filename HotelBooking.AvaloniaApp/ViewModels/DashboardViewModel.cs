using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelBooking.Core.Services;
using HotelBooking.Core.Models;
using System.Linq;

namespace HotelBooking.AvaloniaApp.ViewModels;

public partial class DashboardViewModel : ViewModelBase
{
    private readonly BookingService _bookingService;
    private readonly RoomService _roomService;
    private readonly GuestService _guestService;
    private readonly EventService _eventService;

    [ObservableProperty]
    private int totalRooms;

    [ObservableProperty]
    private int availableRooms;

    [ObservableProperty]
    private int totalGuests;

    [ObservableProperty]
    private int activeBookings;

    [ObservableProperty]
    private int upcomingEvents;

    [ObservableProperty]
    private ObservableCollection<RecentActivity> recentActivities = new();

    [ObservableProperty]
    private ObservableCollection<BookingDisplay> latestBookings = new();

    [ObservableProperty]
    private ObservableCollection<EventDisplay> upcomingEventsList = new();

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string? errorMessage;

    public DashboardViewModel(BookingService bookingService, RoomService roomService, 
        GuestService guestService, EventService eventService)
    {
        _bookingService = bookingService;
        _roomService = roomService;
        _guestService = guestService;
        _eventService = eventService;
    }

    public async Task InitializeAsync()
    {
        await LoadDashboardDataAsync();
    }

    [RelayCommand]
    private async Task LoadDashboardDataAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;

            // Last statistikk
            var rooms = await _roomService.GetAllAsync();
            TotalRooms = rooms.Count();
            
            var availableRoomsList = await _roomService.GetAvailableRoomsAsync();
            AvailableRooms = availableRoomsList.Count();

            var guests = await _guestService.GetAllAsync();
            TotalGuests = guests.Count();

            var bookings = await _bookingService.GetAllAsync();
            ActiveBookings = bookings.Count(b => b.Status == BookingStatus.CheckedIn);

            var events = await _eventService.GetAllAsync();
            var now = DateTime.Now;
            UpcomingEvents = events.Count(e => e.StartDate > now.Date || (e.StartDate == now.Date && e.StartTime > now.TimeOfDay));

            // Last siste bookinger
            var latestBookingsList = bookings
                .OrderByDescending(b => b.CheckIn)
                .Take(5)
                .Select(b => new BookingDisplay
                {
                    GuestName = b.Guest?.Name ?? "Unknown",
                    RoomNumber = $"Room {b.Room?.RoomNumber}",
                    DateRange = $"{b.CheckIn:d} - {b.CheckOut:d}",
                    Status = b.Status.ToString(),
                    StatusColor = GetStatusColor(b.Status)
                });
            LatestBookings = new ObservableCollection<BookingDisplay>(latestBookingsList);

            // Last kommende events
            var upcomingEventsList = events
                .Where(e => e.StartDate > now.Date || (e.StartDate == now.Date && e.StartTime > now.TimeOfDay))
                .OrderBy(e => e.StartDate).ThenBy(e => e.StartTime)
                .Take(5)
                .Select(e => new EventDisplay
                {
                    Title = e.Name,
                    DateTime = $"{e.StartDate:d} {e.StartTime:hh\\:mm}",
                    Location = "TBD",
                    AttendeeCount = $"{(e.EventClients?.Count ?? 0)} attendees"

                });
            UpcomingEventsList = new ObservableCollection<EventDisplay>(upcomingEventsList);

            // Last nylig aktivitet
            var recentActivitiesList = bookings
                .OrderByDescending(b => b.CheckIn)
                .Take(5)
                .Select(b => new RecentActivity
                {
                    Description = $"{b.Guest?.Name} - {b.Status}",
                    Timestamp = b.CheckIn.ToString("g")
                });
            RecentActivities = new ObservableCollection<RecentActivity>(recentActivitiesList);
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

    private string GetStatusColor(BookingStatus status) => status switch
    {
        BookingStatus.Confirmed => "#4CAF50",
        BookingStatus.CheckedIn => "#2196F3",
        BookingStatus.CheckedOut => "#9E9E9E",
        BookingStatus.Cancelled => "#F44336",
        _ => "#9E9E9E"
    };
}

public class BookingDisplay
{
    public string GuestName { get; set; } = "";
    public string RoomNumber { get; set; } = "";
    public string DateRange { get; set; } = "";
    public string Status { get; set; } = "";
    public string StatusColor { get; set; } = "";
}

public class EventDisplay
{
    public string Title { get; set; } = "";
    public string DateTime { get; set; } = "";
    public string Location { get; set; } = "";
    public string AttendeeCount { get; set; } = "";
}

public class RecentActivity
{
    public string Description { get; set; } = "";
    public string Timestamp { get; set; } = "";
}