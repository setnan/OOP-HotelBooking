using System.Threading.Tasks;
using Avalonia.Controls;
using HotelBooking.AvaloniaApp.ViewModels;

namespace HotelBooking.AvaloniaApp.Views;

public partial class MainWindow : Window
{
    private MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext!;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = App.GetService<MainWindowViewModel>();
        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e)
    {
        await InitializeViewModelsAsync();
    }

    private async Task InitializeViewModelsAsync()
    {
        if (DataContext == null)
        {
            DataContext = App.GetService<MainWindowViewModel>();
        }
        
        await ViewModel.InitializeAsync();
        await ViewModel.DashboardViewModel.InitializeAsync();
        await ViewModel.BookingsViewModel.InitializeAsync();
        await ViewModel.RoomManagementViewModel.InitializeAsync();
        await ViewModel.GuestViewModel.InitializeAsync();
        await ViewModel.ClientViewModel.InitializeAsync();
    }
}