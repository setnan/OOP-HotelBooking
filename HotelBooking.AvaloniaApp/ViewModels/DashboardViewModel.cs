using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelBooking.Core.Models;
using HotelBooking.AvaloniaApp.Services;
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

    [ObservableProperty]
    private string welcomeMessage = "Welcome to Hotel Management";

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private string? successMessage;

    private async Task LoadDashboardDataAsync()
    {
        try
        {
            isLoading = true;
            errorMessage = null;
            successMessage = null;

            // TODO: Implement dashboard statistics
            successMessage = "Dashboard loaded successfully";
        }
        catch (Exception ex)
        {
            errorMessage = $"Error loading dashboard data: {ex.Message}";
        }
        finally
        {
            isLoading = false;
        }
    }

    [RelayCommand]
    private Task RefreshDashboard() => LoadDashboardDataAsync();
}
