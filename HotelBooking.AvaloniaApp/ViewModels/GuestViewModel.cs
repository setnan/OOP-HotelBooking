using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelBooking.Core.Models;
using HotelBooking.AvaloniaApp.Services;
using HotelBooking.Core.Services;

namespace HotelBooking.AvaloniaApp.ViewModels;

public partial class GuestViewModel : ViewModelBase
{
    private readonly GuestService guestService;

    public GuestViewModel(GuestService guestService)
    {
        this.guestService = guestService;
        LoadDataAsync();
    }


    [ObservableProperty]
    private ObservableCollection<Guest> guests = new();

    [ObservableProperty]
    private Guest? selectedGuest;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private string? successMessage;

    private async Task LoadDataAsync()
    {
        try
        {
            isLoading = true;
            errorMessage = null;
            successMessage = null;

            var guestsList = await guestService.GetAllGuestsAsync();
            guests = new ObservableCollection<Guest>(guestsList);

            successMessage = "Guests loaded successfully";
        }
        catch (System.Exception ex)
        {
            errorMessage = $"Error loading guests: {ex.Message}";
        }
        finally
        {
            isLoading = false;
        }
    }

    [RelayCommand]
    private Task RefreshData() => LoadDataAsync();
}
