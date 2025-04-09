using Avalonia.Controls;

namespace HotelBooking.AvaloniaApp.Views;

public partial class ClientView : UserControl
{
    public ClientView()
    {
        InitializeComponent();
        DataContext = App.GetService<HotelBooking.AvaloniaApp.ViewModels.ClientViewModel>();
    }
}