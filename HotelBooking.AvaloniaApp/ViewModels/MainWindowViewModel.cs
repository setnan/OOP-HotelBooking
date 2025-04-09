using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelBooking.Core.Models;
using HotelBooking.Core.Database;
using HotelBooking.Core.Services;
using HotelBooking.Core.Utilities;
using HotelBooking.AvaloniaApp.Services;

namespace HotelBooking.AvaloniaApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly UserServiceWrapper userService;
    private readonly RoleService roleService;
    private readonly BookingServiceWrapper bookingService;
    private readonly ClientServiceWrapper clientService;
    private readonly GuestServiceWrapper guestService;
    private readonly RoomServiceWrapper roomService;

    [ObservableProperty]
    private ViewModelBase currentView;

    [ObservableProperty]
    private User? currentUser;

    [ObservableProperty]
    private bool isAdmin;

    public DashboardViewModel DashboardViewModel { get; }
    public BookingsViewModel BookingsViewModel { get; }
    public RoomManagementViewModel RoomManagementViewModel { get; }
    public GuestViewModel GuestViewModel { get; }
    public BackupViewModel BackupViewModel { get; }
    public ClientsViewModel ClientsViewModel { get; }

    public MainWindowViewModel()
    {
        userService = new UserServiceWrapper();
        roleService = RoleService.Instance;
        bookingService = new BookingServiceWrapper();
        clientService = new ClientServiceWrapper(
            new ClientService(new DatabaseConnection(AppConfiguration.Configuration))
        );
        guestService = new GuestServiceWrapper();
        roomService = new RoomServiceWrapper();

        DashboardViewModel = new DashboardViewModel(bookingService, roomService);
        BookingsViewModel = new BookingsViewModel(bookingService, clientService, guestService, roomService);
        RoomManagementViewModel = new RoomManagementViewModel(roomService, bookingService);
        GuestViewModel = new GuestViewModel(guestService);
        BackupViewModel = new BackupViewModel();
        ClientsViewModel = new ClientsViewModel(clientService);

        CurrentView = DashboardViewModel;
    }

    [RelayCommand]
    private void NavigateTo(string viewName)
    {
        // Validate access to admin views
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
            "backup" => BackupViewModel,
            "clients" => ClientsViewModel,
            _ => DashboardViewModel
        };
    }

    public async Task InitializeAsync()
    {
        try
        {
            CurrentUser = await userService.GetCurrentUserAsync();
            isAdmin = currentUser != null && userService.IsAdmin(currentUser);
        }
        catch (Exception)
        {
            isAdmin = false;
        }
    }

    public void OnUserLoggedIn(User user)
    {
        CurrentUser = user;
        isAdmin = userService.IsAdmin(user);
    }

    public void Cleanup()
    {
        // Unsubscribe from events
    }
}
