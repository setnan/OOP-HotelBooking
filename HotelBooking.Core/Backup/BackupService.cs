using System.Globalization;
using System.Text.Json.Nodes;
using HotelBooking.Core.Models;
using HotelBooking.Core.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

    public async Task<string> BackupDataAsync(bool preRestore = false)
    {
        await BackupDataAsync(true);
        var json = await GetAllForBackupAsync();
        
        var timestamp = DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");
        var prefix = preRestore ? "PreRestoreBackup" : "Backup";
        var fileName = $"{prefix}_{timestamp}.json";
        var filePath = GetFileForBackup(fileName);
        
        await File.WriteAllTextAsync(filePath, json);
        return filePath;
    }
    
    public async Task<string> RestoreDataAsync(string fileName)
    {
        var filePath = GetFileForBackup(fileName);
        var json = await File.ReadAllTextAsync(filePath);
        var backupData = JsonConvert.DeserializeObject<Dictionary<string, JArray>>(json);

        var insertTasks = new List<Task>();
        var deleteTasks = new List<Task>();

        foreach (var entry in backupData)
        {
            var serviceName = entry.Key;
            var dataArray = entry.Value;

            if (_services.TryGetValue(serviceName, out var service))
            {
                var targetType = service.GetType().GenericTypeArguments[0];
                var deserializedData = dataArray.ToObject(typeof(List<>).MakeGenericType(targetType));
                
                dynamic dynamicService = service;
                var oldData = await dynamicService.GetAllForBackupAsync();
                
                deleteTasks.Add(dynamicService.DeleteAllAsync(oldData));
                insertTasks.Add(dynamicService.InsertManyAsync((dynamic)deserializedData));

            }
        }
        
        await Task.WhenAll(deleteTasks);
        await Task.WhenAll(insertTasks);

        return filePath;
    }


    private string GetFileForBackup(string? fileName = null)
    {
        var backupFolder = Path.Combine(AppContext.BaseDirectory, "Backups");
        if (!Directory.Exists(backupFolder))
        {
            Directory.CreateDirectory(backupFolder);
        }

        if (fileName != null) return Path.Combine(backupFolder, fileName);
        return backupFolder;
    }

    private DateTime FileNameToSortableDateTime(string fileName)
    {
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName); // "Backup_10-04-2025_16-45-20"
        var parts = fileNameWithoutExtension.Split('_');
        var datePart = parts[1]; // "10-04-2025"
        var timePart = parts[2]; // "16-45-20"
        var fullDateTimeString = $"{datePart}_{timePart}"; // "10-04-2025_16-45-20"
        var dateTime = DateTime.ParseExact(fullDateTimeString, "dd-MM-yyyy_HH-mm-ss", CultureInfo.InvariantCulture);
        return dateTime;
    }

    public  IEnumerable<string> GetAllBackupFilesSorted()
    {
        var backupFolder = GetFileForBackup();
        List<(string fileName, DateTime date)> backupFiles = new List<(string fileName, DateTime date)>();
        var files = Directory.GetFiles(backupFolder);
        foreach (var file in files)
        {
            var fileDateTime = FileNameToSortableDateTime(file);
            backupFiles.Add((file, fileDateTime));
        }
        
        var sortedBackupFiles = backupFiles.OrderByDescending(x => x.date);
        return sortedBackupFiles.Select(x => x.fileName);
    }
    
    
}