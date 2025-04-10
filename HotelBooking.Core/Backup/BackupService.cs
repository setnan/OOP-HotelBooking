using System.Text.Json.Nodes;
using HotelBooking.Core.Models;
using HotelBooking.Core.Services;
using Newtonsoft.Json;

namespace HotelBooking.Core.Backup;

public class BackupService
{
    private readonly ClientService _clientService;
    private readonly GuestService _guestService;
    private readonly RoomService _roomService;
    private readonly BookingService _bookingService;
    private readonly EventService _eventService;
    //private readonly MealService _mealService; -Will be added if time.
    private readonly EventClientService _eventClientService;
    private readonly EventRoomService _eventRoomService;

    private readonly Dictionary<string, IBackupService<object>> _services;
    
    public BackupService(
        ClientService clientService,
        GuestService guestService,
        RoomService roomService,
        BookingService bookingService,
        EventService eventService,
        EventClientService eventClientService,
        EventRoomService eventRoomService)
    {
        _clientService = clientService;
        _guestService = guestService;
        _roomService = roomService;
        _bookingService = bookingService;
        _eventService = eventService;
        _eventClientService = eventClientService;
        _eventRoomService = eventRoomService;

        _services = new Dictionary<string, IBackupService<object>>
        {
            { "Clients", (IBackupService<object>)_clientService },
            { "Guests", (IBackupService<object>)_guestService },
            { "Rooms", (IBackupService<object>)_roomService },
            { "Bookings", (IBackupService<object>)_bookingService },
            { "Events", (IBackupService<object>)_eventService },
            { "EventClients", (IBackupService<object>)_eventClientService },
            { "EventRooms", (IBackupService<object>)_eventRoomService }
        };
    }

    public async Task<string> GetAllForBackupAsync()
    {
        var backupObject = new Dictionary<string, IEnumerable<object>>();
        foreach (var entry in _services)
        {
            var key = entry.Key;
            var service = entry.Value;
            var data = await service.GetAllForBackupAsync();
            backupObject[key] = data;
        }
        var json = JsonConvert.SerializeObject(backupObject, Formatting.Indented);
        return json;
    }

    public async Task<string> BackupDataAsync()
    {
        var json = await GetAllForBackupAsync();
        var backupFolder = Path.Combine(AppContext.BaseDirectory, "Backups");
        if (!Directory.Exists(backupFolder))
        {
            Directory.CreateDirectory(backupFolder);
        }
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        var fileName = $"Backup_{timestamp}.json";
        var filePath = Path.Combine(backupFolder, fileName);
        
        await File.WriteAllTextAsync(filePath, json);
        return filePath;
    }
    
}