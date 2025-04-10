public class MainView : Window
{
    public MainView()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel(
            new UserService(),
            new RoleService(),
            new BookingService(),
            new ClientService(),
            new GuestService(),
            new RoomService(),
            new DashboardViewModel(),
            new BookingsViewModel(),
            new RoomManagementViewModel(),
            new GuestViewModel(),
            new ClientViewModel()
        );
        ((MainWindowViewModel)DataContext).InitializeAsync();
    }
}