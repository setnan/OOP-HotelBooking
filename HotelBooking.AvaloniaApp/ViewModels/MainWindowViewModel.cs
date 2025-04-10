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
    private ViewModelBase currentView;

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

        DashboardViewModel = dashboardViewModel;
        BookingsViewModel = bookingsViewModel;
        RoomManagementViewModel = roomManagementViewModel;
        GuestViewModel = guestViewModel;
        ClientViewModel = clientViewModel;

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
        if (!isAdmin && (viewName == "rooms" || viewName == "backup"))
        {
            return;
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
        try
        {
            isAdmin = UserSession.Instance.IsAdmin;
        }
        catch (Exception)
        {
            isAdmin = false;
        }
    }

    public void OnUserLoggedIn(User user)
    {
        CurrentUser = user;
        isAdmin = UserSession.Instance.IsAdmin;
    }

    public void Cleanup()
    {
        
    }
}
