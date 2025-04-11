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
    private readonly ClientService _clientService;
    private readonly EventService _eventService;
    private readonly EventClientService _eventClientService;

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

    public EventViewModel(EventService eventService, ClientService clientService, EventClientService eventClientService)
    {
        _eventService = eventService;
        _clientService = clientService;
        _eventClientService = eventClientService;
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
            var client = Clients.FirstOrDefault(c => c.ClientId == value.OrganiserId);
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
            var events = await _eventService.GetAllAsync();
            Events.Clear();

            if (events != null)
            {
                var allClients = await _clientService.GetAllAsync();

                foreach (var evt in events)
                {
                    var eventClients = await _eventClientService.GetAllByEventIdAsync(evt.EventId);
                    if (eventClients != null && eventClients.Any())
                    {
                        var hasValidClient = eventClients
                            .Any(ec => allClients.Any(c => c.ClientId == ec.ClientId));

                        if (hasValidClient)
                        {
                            Events.Add(evt);
                        }
                    }
                    else
                    {
                        Events.Add(evt);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Kunne ikke laste events: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task AddEventAsync()
    {
        if (SelectedClient == null)
        {
            ErrorMessage = "Vennligst velg en arrangør.";
            return;
        }

        try
        {
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

            var success = await _eventService.AddEventAsync(newEvent);
            if (!success)
            {
                ErrorMessage = "Kunne ikke opprette hendelsen.";
                return;
            }

            var createdEvent = await _eventService.GetEventByNameAndDateAsync(newEvent.Name, newEvent.StartDate);
            if (createdEvent == null)
            {
                ErrorMessage = "Kunne ikke finne det opprettede eventet.";
                return;
            }

            var eventClient = new EventClient
            {
                EventId = createdEvent.EventId,
                ClientId = SelectedClient.ClientId
            };

            var clientAdded = await _eventClientService.AddAsync(eventClient);
            if (!clientAdded)
            {
                ErrorMessage = "Kunne ikke koble klient til hendelsen.";
                return;
            }

            await LoadEvents();
            ClearForm();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"En feil oppstod: {ex.Message}";
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
        SelectedClient = null;
        ErrorMessage = string.Empty;
    }
}
