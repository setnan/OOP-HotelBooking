namespace HotelBooking.Core.Backup;

public interface IBackupService<T>
{
    Task<IEnumerable<T>> GetAllForBackupAsync();
    Task InsertManyAsync(IEnumerable<T> items);
}
