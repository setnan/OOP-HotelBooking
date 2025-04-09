using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelBooking.Core.Models;
using HotelBooking.AvaloniaApp.Services;

namespace HotelBooking.AvaloniaApp.ViewModels;

public partial class BackupViewModel : ViewModelBase
{
    private readonly BackupService backupService;

    public BackupViewModel(BackupService backupService)
    {
        this.backupService = backupService;
        LoadBackupsAsync();
    }

    [ObservableProperty]
    private ObservableCollection<BackupInfo> backups = new();

    [ObservableProperty]
    private BackupInfo? selectedBackup;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private string? successMessage;

    private async Task LoadBackupsAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;
            SuccessMessage = null;

            var backupsList = await backupService.GetBackupsAsync();
            Backups = new ObservableCollection<BackupInfo>(backupsList);

            SuccessMessage = "Backups loaded successfully";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading backups: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private Task RefreshBackups() => LoadBackupsAsync();
}
