using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace HotelBooking.AvaloniaApp.Services;

public class BackupService
{
    private readonly string backupDirectory;

    public BackupService()
    {
        backupDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "HotelBooking",
            "Backups");

        Directory.CreateDirectory(backupDirectory);
    }

    public string GetBackupDirectory()
    {
        return backupDirectory;
    }

    public IEnumerable<string> GetAvailableBackups()
    {
        return Directory.Exists(backupDirectory)
            ? Directory.GetFiles(backupDirectory, "*.sql")
            : Enumerable.Empty<string>();
    }

    public async Task<string> CreateBackupAsync()
    {
        var fileName = $"backup_{DateTime.Now:yyyyMMdd_HHmmss}.sql";
        var filePath = Path.Combine(backupDirectory, fileName);

        var connStr = Environment.GetEnvironmentVariable("CONNECTION_STRING");
        if (string.IsNullOrEmpty(connStr))
            throw new InvalidOperationException("Missing CONNECTION_STRING");

        var builder = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder(connStr);
        var args = $"-u{builder.UserID} -p{builder.Password} -h{builder.Server} -P{builder.Port} {builder.Database}";

        var startInfo = new ProcessStartInfo
        {
            FileName = "mysqldump",
            Arguments = args,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(startInfo);
        if (process == null)
            throw new InvalidOperationException("Failed to start mysqldump");

        var output = await process.StandardOutput.ReadToEndAsync();
        await File.WriteAllTextAsync(filePath, output);

        await process.WaitForExitAsync();
        if (process.ExitCode != 0)
            throw new InvalidOperationException($"mysqldump failed with exit code {process.ExitCode}");

        return filePath;
    }

    public async Task RestoreFromBackupAsync(string backupFile)
    {
        if (!File.Exists(backupFile))
            throw new FileNotFoundException("Backup file not found", backupFile);

        var connStr = Environment.GetEnvironmentVariable("CONNECTION_STRING");
        if (string.IsNullOrEmpty(connStr))
            throw new InvalidOperationException("Missing CONNECTION_STRING");

        var builder = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder(connStr);
        var args = $"-u{builder.UserID} -p{builder.Password} -h{builder.Server} -P{builder.Port} {builder.Database}";

        var startInfo = new ProcessStartInfo
        {
            FileName = "mysql",
            Arguments = args,
            RedirectStandardInput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(startInfo);
        if (process == null)
            throw new InvalidOperationException("Failed to start mysql");

        var backup = await File.ReadAllTextAsync(backupFile);
        await process.StandardInput.WriteAsync(backup);
        process.StandardInput.Close();

        await process.WaitForExitAsync();
        if (process.ExitCode != 0)
            throw new InvalidOperationException($"mysql failed with exit code {process.ExitCode}");
    }

    public async Task DeleteBackupAsync(string backupFile)
    {
        if (!File.Exists(backupFile))
            throw new FileNotFoundException("Backup file not found", backupFile);

        await Task.Run(() => File.Delete(backupFile));
    }
}
