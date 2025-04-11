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
    private readonly EventService _eventService;

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
    public EventViewModel EventViewModel { get; }

    public MainWindowViewModel(
        UserService userService,
        RoleService roleService,
        BookingService bookingService,
        ClientService clientService,
        GuestService guestService,
        RoomService roomService,
        EventService eventService,
        DashboardViewModel dashboardViewModel,
        BookingsViewModel bookingsViewModel,
        RoomManagementViewModel roomManagementViewModel,
        GuestViewModel guestViewModel,
        ClientViewModel clientViewModel,
        EventViewModel eventViewModel)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
        _bookingService = bookingService ?? throw new ArgumentNullException(nameof(bookingService));
        _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
        _guestService = guestService ?? throw new ArgumentNullException(nameof(guestService));
        _roomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
        _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));

        DashboardViewModel = dashboardViewModel ?? throw new ArgumentNullException(nameof(dashboardViewModel));
        BookingsViewModel = bookingsViewModel ?? throw new ArgumentNullException(nameof(bookingsViewModel));
        RoomManagementViewModel = roomManagementViewModel ?? throw new ArgumentNullException(nameof(roomManagementViewModel));
        GuestViewModel = guestViewModel ?? throw new ArgumentNullException(nameof(guestViewModel));
        ClientViewModel = clientViewModel ?? throw new ArgumentNullException(nameof(clientViewModel));
        EventViewModel = eventViewModel ?? throw new ArgumentNullException(nameof(eventViewModel));

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
    private async Task NavigateTo(string viewName)
    {
        if (string.IsNullOrEmpty(viewName)) return;

        if (CurrentUser == null)
        {
            return; // Don't allow navigation if not logged in
        }

        // Only admin can access guests and clients
        if (!IsAdmin && (viewName == "guests" || viewName == "clients"))
        {
            return; // Don't allow navigation to admin-only views
        }

        ViewModelBase nextView;
        switch (viewName.ToLower())
        {
            case "dashboard":
                nextView = DashboardViewModel;
                break;
            case "bookings":
                nextView = BookingsViewModel;
                break;
            case "rooms":
                nextView = RoomManagementViewModel;
                break;
            case "guests":
                nextView = GuestViewModel;
                break;
            case "clients":
                nextView = ClientViewModel;
                break;
            case "events":
                nextView = EventViewModel;
                break;
            default:
                nextView = DashboardViewModel;
                break;
        }

        // Initialize the view if needed
        switch (nextView)
        {
            case BookingsViewModel bookingsVm:
                await bookingsVm.InitializeAsync();
                break;
            case DashboardViewModel dashboardVm:
                await dashboardVm.InitializeAsync();
                break;
            case RoomManagementViewModel roomsVm:
                await roomsVm.InitializeAsync();
                break;
            case GuestViewModel guestVm:
                await guestVm.InitializeAsync();
                break;
            case ClientViewModel clientVm:
                await clientVm.InitializeAsync();
                break;
            case EventViewModel eventVm:
                await eventVm.InitializeAsync();
                break;
        }

        CurrentView = nextView;
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
                await DashboardViewModel.InitializeAsync();
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
