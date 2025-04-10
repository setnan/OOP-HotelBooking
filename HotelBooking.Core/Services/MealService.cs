using HotelBooking.Core.Backup;
using HotelBooking.Core.Database;
using HotelBooking.Core.Models;

namespace HotelBooking.Core.Services;

public class MealService : IBackupService<Meal>
{
    private DatabaseConnection _db;

    public MealService(DatabaseConnection db)
    {
        _db = db;
    }

    public async Task<bool> AddMealAsync(Meal meal)
    {
        return await _db.InsertAsync(meal);
    }

    public async Task<bool> UpdateMealAsync(Meal meal)
    {
        return await _db.UpdateAsync(meal);
    }

    public async Task<bool> DeleteMealAsync(Meal meal)
    {
        return await _db.DeleteAsync(meal);
    }
    
    public async Task<IEnumerable<Meal>> GetAllForBackupAsync()
    {
        return await _db.GetAllAsync<Meal>();
    }

    public async Task InsertManyAsync(IEnumerable<Meal> items)
    {
        foreach (var item in items)
        {
            await _db.InsertAsync(item);
        }
    }

    public async Task DeleteAllAsync()
    {
        await  _db.DeleteAllAsync<Meal>();
    }

    public async Task<IEnumerable<Meal>> GetAllAsync()
    {
        return await _db.GetAllAsync<Meal>();
    }
}