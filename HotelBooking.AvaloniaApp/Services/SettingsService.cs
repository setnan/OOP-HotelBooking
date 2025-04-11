using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using HotelBooking.Core.Models;

namespace HotelBooking.AvaloniaApp.Services;

public class SettingsService
{
    private const string SettingsFileName = "settings.json";
    private readonly string _settingsPath;

    public SettingsService()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appFolder = Path.Combine(appDataPath, "HotelBooking");
        
        if (!Directory.Exists(appFolder))
        {
            Directory.CreateDirectory(appFolder);
        }

        _settingsPath = Path.Combine(appFolder, SettingsFileName);
    }

    public async Task SaveUserCredentialsAsync(string email, string password)
    {
        var settings = new UserSettings
        {
            Email = email,
            Password = password
        };

        var json = JsonSerializer.Serialize(settings);
        await File.WriteAllTextAsync(_settingsPath, json);
    }

    public async Task<UserSettings?> LoadUserCredentialsAsync()
    {
        if (!File.Exists(_settingsPath))
        {
            return null;
        }

        var json = await File.ReadAllTextAsync(_settingsPath);
        return JsonSerializer.Deserialize<UserSettings>(json);
    }

    public async Task ClearUserCredentialsAsync()
    {
        if (File.Exists(_settingsPath))
        {
            await File.WriteAllTextAsync(_settingsPath, "{}");
        }
    }
}

public class UserSettings
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}
