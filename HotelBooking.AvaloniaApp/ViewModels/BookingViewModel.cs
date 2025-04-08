using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using HotelBooking.Core.Models;
using HotelBooking.Core.Services;

namespace HotelBooking.AvaloniaApp.ViewModels;

public partial class BookingsViewModel : ViewModelBase
{
    // Dette er en liste som UI kan binde seg til og oppdatere dynamisk
    [ObservableProperty]
    private ObservableCollection<Booking> bookings = new();

    // Kan brukes i UI for å vise "Loading..." .eks. mens bookinger hentes
    [ObservableProperty]
    private bool isLoading;

    public BookingsViewModel()
    {
        // Konstruktør: dette skjer når ViewModelen lages
        LoadBookingsAsync(); // ← viktig at dette kalles med en gang
    }

    private async void LoadBookingsAsync()
    {
        // Setter loading = true så GUI kan vise spinner eller melding
        IsLoading = true;

        // Kjører databasekallet i bakgrunnen (så UI ikke henger)
        var allBookings = await Task.Run(() => BookingService.GetAllBookings());

        // Oppdaterer ObservableCollection – UI vil automatisk vise endringen
        Bookings = new ObservableCollection<Booking>(allBookings);

        // Ferdig – skjul "Loading..."
        IsLoading = false;
    }
}