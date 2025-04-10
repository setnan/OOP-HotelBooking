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
    }

    public async Task InitializeAsync()
    {
        await LoadClientsAsync();
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
    private async Task LoadClients()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;
            var loadedClients = await _clientService.GetAllAsync();
            Clients = new ObservableCollection<Client>(loadedClients);
        }
        catch (System.Exception ex)
        {
            ErrorMessage = $"Error loading clients: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task LoadClientsAsync()
    {
        await LoadClientsCommand.ExecuteAsync(null);
    }

    [RelayCommand]
    private async Task AddClient()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;
            SuccessMessage = null;

            var client = new Client
            {
                Name = Name,
                BillingAddress = BillingAddress,
                ContactPerson = ContactPerson,
                ContactNumber = ContactNumber
            };

            await _clientService.AddClientAsync(client);
            await LoadClientsAsync();

            // Clear form
            Name = "";
            BillingAddress = "";
            ContactPerson = "";
            ContactNumber = "";

            SuccessMessage = "Client added successfully";
        }
        catch (System.Exception ex)
        {
            ErrorMessage = $"Error adding client: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task UpdateClient()
    {
        if (SelectedClient == null) return;

        try
        {
            IsLoading = true;
            ErrorMessage = null;
            SuccessMessage = null;

            var client = new Client
            {
                ClientId = SelectedClient.ClientId,
                Name = Name,
                BillingAddress = BillingAddress,
                ContactPerson = ContactPerson,
                ContactNumber = ContactNumber
            };

            await _clientService.UpdateClientAsync(client);
            await LoadClientsAsync();

            SuccessMessage = "Client updated successfully";
        }
        catch (System.Exception ex)
        {
            ErrorMessage = $"Error updating client: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task DeleteClient()
    {
        if (SelectedClient == null) return;

        try
        {
            IsLoading = true;
            ErrorMessage = null;
            SuccessMessage = null;

            await _clientService.DeleteClientAsync(SelectedClient);
            await LoadClientsAsync();

            SuccessMessage = "Client deleted successfully";
        }
        catch (System.Exception ex)
        {
            ErrorMessage = $"Error deleting client: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    partial void OnSelectedClientChanged(Client? value)
    {
        if (value != null)
        {
            Name = value.Name;
            BillingAddress = value.BillingAddress;
            ContactPerson = value.ContactPerson;
            ContactNumber = value.ContactNumber;
        }
        else
        {
            Name = "";
            BillingAddress = "";
            ContactPerson = "";
            ContactNumber = "";
        }
    }
}
