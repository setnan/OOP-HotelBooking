using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelBooking.Core.Models;
using HotelBooking.AvaloniaApp.Services;

namespace HotelBooking.AvaloniaApp.ViewModels;

public partial class ClientsViewModel : ViewModelBase
{
    private readonly ClientServiceWrapper clientService;

    public ClientsViewModel(ClientServiceWrapper clientService)
    {
        this.clientService = clientService;
        LoadClientsAsync();
    }

    [ObservableProperty]
    private ObservableCollection<Client> clients = new();

    [ObservableProperty]
    private Client? selectedClient;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private string? successMessage;

    // Felter for ny klient
    [ObservableProperty] private string name = string.Empty;
    [ObservableProperty] private string billingAddress = string.Empty;
    [ObservableProperty] private string contactPerson = string.Empty;
    [ObservableProperty] private string contactNumber = string.Empty;

    private async Task LoadClientsAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;
            SuccessMessage = null;

            var clientList = await clientService.GetAllClientsAsync();
            Clients = new ObservableCollection<Client>(clientList);

            SuccessMessage = "Klienter lastet inn.";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Feil ved lasting: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private Task Refresh() => LoadClientsAsync();

    [RelayCommand]
    private async Task DeleteClient()
    {
        if (SelectedClient == null) return;

        try
        {
            IsLoading = true;
            var result = await clientService.DeleteClientAsync(SelectedClient);
            if (result)
            {
                Clients.Remove(SelectedClient);
                SuccessMessage = "Klient slettet.";
            }
            else
            {
                ErrorMessage = "Klarte ikke slette klient.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Feil ved sletting: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
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

            var result = await clientService.AddClientAsync(client);
            if (result)
            {
                SuccessMessage = "Klient lagt til.";
                await LoadClientsAsync();
                ClearNewClientForm();
            }
            else
            {
                ErrorMessage = "Kunne ikke legge til klient.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Feil: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void ClearNewClientForm()
    {
        Name = string.Empty;
        BillingAddress = string.Empty;
        ContactPerson = string.Empty;
        ContactNumber = string.Empty;
    }
}
