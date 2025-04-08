using CommunityToolkit.Mvvm.ComponentModel;

namespace HotelBooking.Desktop.ViewModels;

public partial class ReservationViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _title = "Make Reservation";
}
