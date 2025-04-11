using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelBooking.Core.Models;
using HotelBooking.Core.Services;

namespace HotelBooking.AvaloniaApp.ViewModels;

public partial class EventViewModel : ViewModelBase
{
    private readonly EventService _eventService;
    private readonly ClientService _clientService;

    [ObservableProperty]
    private ObservableCollection<Event> _events;

    [ObservableProperty]
    private ObservableCollection<Client> _clients;

    [ObservableProperty]
    private Event? _selectedEvent;

    [ObservableProperty]
    private Client? _selectedClient;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private DateTime _startDate = DateTime.Now;

    [ObservableProperty]
    private DateTime _endDate = DateTime.Now;

    [ObservableProperty]
    private TimeSpan _startTime = DateTime.Now.TimeOfDay;

    [ObservableProperty]
    private TimeSpan _endTime = DateTime.Now.TimeOfDay;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _isLoading;

    public EventViewModel(EventService eventService, ClientService clientService)
    {
        _eventService = eventService;
        _clientService = clientService;
        _events = new ObservableCollection<Event>();
        _clients = new ObservableCollection<Client>();
    }

    partial void OnSelectedEventChanged(Event? value)
    {
        if (value != null)
        {
            Name = value.Name;
            StartDate = value.StartDate;
            EndDate = value.EndDate;
            StartTime = value.StartTime;
            EndTime = value.EndTime;
            // Finn og sett valgt klient
            var client = _clients.FirstOrDefault(c => c.ClientId == value.OrganiserId);
            if (client != null)
            {
                SelectedClient = client;
            }
        }
    }

    public async Task InitializeAsync()
    {
        await LoadClients();
        await LoadEvents();
    }

    private async Task LoadClients()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            var clients = await _clientService.GetAllAsync();
            if (clients != null)
            {
                Clients = new ObservableCollection<Client>(clients);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Kunne ikke laste klienter: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task LoadEvents()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            var events = await _eventService.GetAllAsync();
            if (events != null)
            {
                Events = new ObservableCollection<Event>(events);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Kunne ikke laste hendelser: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task AddEventAsync()
    {
        if (SelectedClient == null)
        {
            ErrorMessage = "Vennligst velg en klient som organiserer hendelsen.";
            return;
        }

        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            var newEvent = new Event
            {
                Name = Name,
                StartDate = StartDate,
                EndDate = EndDate,
                StartTime = StartTime,
                EndTime = EndTime,
                HotelId = 1,
                OrganiserId = SelectedClient.ClientId
            };

            var success = await _eventService.CreateEventAsync(newEvent, new List<string>(), new List<string>());
            if (success)
            {
                await LoadEvents();
                ClearForm();
            }
            else
            {
                ErrorMessage = "Kunne ikke opprette hendelsen. Prøv igjen.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Feil ved oppretting av hendelse: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task UpdateEvent()
    {
        if (SelectedEvent == null || SelectedClient == null) return;

        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            SelectedEvent.Name = Name;
            SelectedEvent.StartDate = StartDate;
            SelectedEvent.EndDate = EndDate;
            SelectedEvent.StartTime = StartTime;
            SelectedEvent.EndTime = EndTime;
            SelectedEvent.OrganiserId = SelectedClient.ClientId;

            var success = await _eventService.UpdateEventAsync(SelectedEvent);
            if (success)
            {
                await LoadEvents();
                ClearForm();
            }
            else
            {
                ErrorMessage = "Kunne ikke oppdatere hendelsen. Prøv igjen.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Feil ved oppdatering av hendelse: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task RemoveEvent()
    {
        if (SelectedEvent == null) return;

        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            var success = await _eventService.DeleteEventAsync(SelectedEvent);
            if (success)
            {
                await LoadEvents();
                ClearForm();
            }
            else
            {
                ErrorMessage = "Kunne ikke slette hendelsen. Prøv igjen.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Feil ved sletting av hendelse: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void ClearForm()
    {
        Name = string.Empty;
        StartDate = DateTime.Now;
        EndDate = DateTime.Now;
        StartTime = DateTime.Now.TimeOfDay;
        EndTime = DateTime.Now.TimeOfDay;
        SelectedEvent = null;
        SelectedClient = null;
        ErrorMessage = string.Empty;
    }
}
