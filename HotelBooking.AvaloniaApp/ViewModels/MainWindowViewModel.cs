using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelBooking.AvaloniaApp.Services;
using HotelBooking.Core.Models;
using HotelBooking.Core.Services;

namespace HotelBooking.AvaloniaApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly UserService _userService;
    private readonly BookingService _bookingService;
    private readonly ClientService _clientService;
    private readonly GuestService _guestService;
    private readonly RoomService _roomService;
    private readonly RoleService _roleService;

    [ObservableProperty]
    private ViewModelBase? currentView;

    [ObservableProperty]
    private User? currentUser;

    [ObservableProperty]
    private bool isAdmin;
    
    [ObservableProperty]
    private bool isPaneOpen;

    public DashboardViewModel DashboardViewModel { get; }
    public BookingsViewModel BookingsViewModel { get; }
    public RoomManagementViewModel RoomManagementViewModel { get; }
    public GuestViewModel GuestViewModel { get; }
    public ClientViewModel ClientViewModel { get; }

    public MainWindowViewModel(
        UserService userService,
        RoleService roleService,
        BookingService bookingService,
        ClientService clientService,
        GuestService guestService,
        RoomService roomService,
        DashboardViewModel dashboardViewModel,
        BookingsViewModel bookingsViewModel,
        RoomManagementViewModel roomManagementViewModel,
        GuestViewModel guestViewModel,
        ClientViewModel clientViewModel)
    {
        _userService = userService;
        _roleService = roleService;
        _bookingService = bookingService;
        _clientService = clientService;
        _guestService = guestService;
        _roomService = roomService;

        DashboardViewModel = dashboardViewModel ?? throw new ArgumentNullException(nameof(dashboardViewModel));
        BookingsViewModel = bookingsViewModel ?? throw new ArgumentNullException(nameof(bookingsViewModel));
        RoomManagementViewModel = roomManagementViewModel ?? throw new ArgumentNullException(nameof(roomManagementViewModel));
        GuestViewModel = guestViewModel ?? throw new ArgumentNullException(nameof(guestViewModel));
        ClientViewModel = clientViewModel ?? throw new ArgumentNullException(nameof(clientViewModel));

        // Initialize current user from session if exists
        var session = UserSession.Instance;
        if (session.IsLoggedIn && session.UserId.HasValue)
        {
            CurrentUser = new User
            {
                UserId = session.UserId.Value,
                Name = session.Name ?? string.Empty,
                Email = session.Email ?? string.Empty,
                Role = session.IsAdmin ? Role.Admin : Role.Receptionist
            };
            IsAdmin = session.IsAdmin;
        }

        // Set initial view
        CurrentView = DashboardViewModel;
    }

    [RelayCommand]
    private void TogglePane()
    {
        IsPaneOpen = !IsPaneOpen;
    }

    [RelayCommand]
    private void NavigateTo(string viewName)
    {
        if (string.IsNullOrEmpty(viewName)) return;

        if (CurrentUser == null)
        {
            return; // Don't allow navigation if not logged in
        }

        if (!IsAdmin && (viewName == "rooms" || viewName == "backup"))
        {
            return; // Don't allow navigation to admin-only views
        }

        CurrentView = viewName.ToLower() switch
        {
            "dashboard" => DashboardViewModel,
            "bookings" => BookingsViewModel,
            "rooms" => RoomManagementViewModel,
            "guests" => GuestViewModel,
            "clients" => ClientViewModel,
            _ => DashboardViewModel
        };
    }

    public async Task InitializeAsync()
    {
        var session = UserSession.Instance;
        if (session.IsLoggedIn && session.UserId.HasValue)
        {
            CurrentUser = new User
            {
                UserId = session.UserId.Value,
                Name = session.Name ?? string.Empty,
                Email = session.Email ?? string.Empty,
                Role = session.IsAdmin ? Role.Admin : Role.Receptionist
            };
            IsAdmin = session.IsAdmin;
            
            // Ensure CurrentView is set to a non-null value
            if (CurrentView == null)
            {
                CurrentView = DashboardViewModel;
            }
        }
    }

    public void OnUserLoggedIn(User user)
    {
        CurrentUser = user;
        IsAdmin = user.Role == Role.Admin;
        CurrentView = DashboardViewModel ?? throw new InvalidOperationException("DashboardViewModel is not initialized");
    }

    public void Cleanup()
    {
        CurrentUser = null;
        IsAdmin = false;
        CurrentView = null;
    }
}
