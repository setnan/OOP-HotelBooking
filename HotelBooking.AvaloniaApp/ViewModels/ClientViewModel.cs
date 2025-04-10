using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelBooking.Core.Models;
using HotelBooking.Core.Services;

namespace HotelBooking.AvaloniaApp.ViewModels;

public partial class ClientViewModel : ViewModelBase
{
    private readonly ClientService _clientService;

    public ClientViewModel(ClientService clientService)
    {
        _clientService = clientService;
        LoadClientsCommand.Execute(null);
    }

    [ObservableProperty] private ObservableCollection<Client> clients = new();
    [ObservableProperty] private Client? selectedClient;

    [ObservableProperty] private string name = "";
    [ObservableProperty] private string billingAddress = "";
    [ObservableProperty] private string contactPerson = "";
    [ObservableProperty] private string contactNumber = "";

    [ObservableProperty] private string? errorMessage;
    [ObservableProperty] private string? successMessage;
    [ObservableProperty] private bool isLoading;

    [RelayCommand]
    private async Task LoadClientsAsync()
    {
        IsLoading = true;
        ErrorMessage = SuccessMessage = null;

        var list = await _clientService.GetAllClientsAsync();
        Clients = new ObservableCollection<Client>(list);

        IsLoading = false;
    }

    [RelayCommand]
    private async Task AddClientAsync()
    {
        IsLoading = true;
        ErrorMessage = SuccessMessage = null;

        var newClient = new Client
        {
            Name = Name,
            BillingAddress = BillingAddress,
            ContactPerson = ContactPerson,
            ContactNumber = ContactNumber
        };

        var success = await _clientService.AddClientAsync(newClient);
        if (success)
        {
            Clients.Add(newClient);
            SuccessMessage = "Klient lagt til!";
            ClearFields();
        }
        else
        {
            ErrorMessage = "Klarte ikke legge til klient.";
        }

        IsLoading = false;
    }

    [RelayCommand]
    private async Task DeleteClientAsync()

    {
        if (SelectedClient is null) return;

        IsLoading = true;
        ErrorMessage = SuccessMessage = null;

        var success = await _clientService.DeleteClientAsync(SelectedClient);
        if (success)
        {
            Clients.Remove(SelectedClient);
            SelectedClient = null;
            SuccessMessage = "Klient slettet.";
        }
        else
        {
            ErrorMessage = "Klarte ikke slette klient.";
        }

        IsLoading = false;
    }

    private void ClearFields()
    {
        Name = BillingAddress = ContactPerson = ContactNumber = "";
    }
}
