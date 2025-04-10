namespace HotelBooking.Core.Backup;

public interface IBackupService<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task InsertManyAsync(IEnumerable<T> items);
}