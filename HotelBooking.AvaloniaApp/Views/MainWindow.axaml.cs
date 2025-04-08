using Avalonia.Controls;
using HotelBooking.Desktop.ViewModels;

namespace HotelBooking.Desktop.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new LoginViewModel();
    }
}