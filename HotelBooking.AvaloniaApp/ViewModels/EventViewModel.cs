using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelBooking.Core.Models;
using HotelBooking.Core.Services;

namespace HotelBooking.AvaloniaApp.ViewModels;

public partial class EventViewModel : ViewModelBase
{
    private readonly EventService _eventService;

    [ObservableProperty]
    private ObservableCollection<Event> _events;

    [ObservableProperty]
    private Event? _selectedEvent;

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

    public EventViewModel(EventService eventService)
    {
        _eventService = eventService;
        _events = new ObservableCollection<Event>();
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
        }
    }

    public async Task InitializeAsync()
    {
        await LoadEvents();
    }

    [RelayCommand]
    private async Task LoadEvents()
    {
        var events = await _eventService.GetAllAsync();
        if (events != null)
        {
            Events = new ObservableCollection<Event>(events);
        }
    }

    [RelayCommand]
    private async Task AddEvent()
    {
        var newEvent = new Event
        {
            Name = Name,
            StartDate = StartDate,
            EndDate = EndDate,
            StartTime = StartTime,
            EndTime = EndTime
        };

        // For now, we'll create the event without clients and rooms
        // TODO: Add UI for selecting clients and rooms
        await _eventService.CreateEventAsync(newEvent, new List<string>(), new List<string>());
        await LoadEvents();
        ClearForm();
    }

    [RelayCommand]
    private async Task UpdateEvent()
    {
        if (SelectedEvent == null) return;

        SelectedEvent.Name = Name;
        SelectedEvent.StartDate = StartDate;
        SelectedEvent.EndDate = EndDate;
        SelectedEvent.StartTime = StartTime;
        SelectedEvent.EndTime = EndTime;

        await _eventService.UpdateEventAsync(SelectedEvent);
        await LoadEvents();
        ClearForm();
    }

    [RelayCommand]
    private async Task RemoveEvent()
    {
        if (SelectedEvent == null) return;

        await _eventService.DeleteEventAsync(SelectedEvent);
        await LoadEvents();
        ClearForm();
    }

    private void ClearForm()
    {
        Name = string.Empty;
        StartDate = DateTime.Now;
        EndDate = DateTime.Now;
        StartTime = DateTime.Now.TimeOfDay;
        EndTime = DateTime.Now.TimeOfDay;
        SelectedEvent = null;
    }
}
