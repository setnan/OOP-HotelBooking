using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using HotelBooking.Core.Models;
using HotelBooking.Core.Utilities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace HotelBooking.Core.Database;

public class DatabaseConnection
{
    private readonly string? _connectionString = AppConfiguration.Configuration["ConnectionStrings:DefaultConnection"];

    private readonly MySqlConnection _connection;

    public DatabaseConnection()
    {
        _connection = new MySqlConnection(_connectionString);
    }
    
    public void Open() => _connection.Open();
    public void Close() => _connection.Close();
    
    public MySqlConnection GetConnection() => _connection;

    public async Task<List<T>> GetAllAsync<T>()
    {
        var table = GetTableName<T>();
        var query = $"SELECT * FROM {table}";
        return (await _connection.QueryAsync<T>(query)).ToList();
    }

    public async Task<List<T>> GetAllWhereAsync<T>(string parameterName, object parameterValue)
    {
        var table = GetTableName<T>();
        var parameters = new Dictionary<string, object> { { parameterName, parameterValue } };
        var (key, value) = parameters.First();

        var query = $"SELECT * FROM {table} WHERE {key} = @{key}";
        return (await _connection.QueryAsync<T>(query, parameters)).ToList();
    }

    public async Task<T?> GetOneAsync<T>(string parameterName, object parameterValue)
    {
        var table = GetTableName<T>();
        var parameters = new Dictionary<string, object> { { parameterName, parameterValue } };
        var (key, value) = parameters.First();

        var query = $"SELECT * FROM {table} WHERE {key} = @{key}";
        return await _connection.QuerySingleOrDefaultAsync<T>(query, parameters);
    }

    public async Task<bool> InsertAsync<T>(T entity)
    {
        var table = typeof(T).Name;
        var propertyNameList = typeof(T).GetProperties()
            .Select(p => p.Name)
            .Where(name => name != $"{typeof(T).Name}Id")
            .ToList();

        var columns = string.Join(", ", propertyNameList);
        var parameters = string.Join(", ", propertyNameList.Select(name => "@" + name));

        var insertQuery = $"INSERT INTO {table} ({columns}) VALUES ({parameters})";

        var rowsAffected = await _connection.ExecuteAsync(insertQuery, entity);
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateAsync<T>(T entity)
    {
        var table = GetTableName<T>();
        var propertyNameListFiltered = GetPropertyNames<T>(true);

        var setClause = string.Join(", ", propertyNameListFiltered.Select(name => $"{name} = @{name}"));
        var updateQuery = $"UPDATE {table} SET {setClause} WHERE {table}Id = @{table}Id";

        var rowsAffected = await _connection.ExecuteAsync(updateQuery, entity);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync<T>(T entity)
    {
        var table = GetTableName<T>();
        var deleteQuery = $"DELETE FROM {table} WHERE {table}Id = @{table}Id";
        var rowsAffected = await _connection.ExecuteAsync(deleteQuery, entity);
        return rowsAffected > 0;
    }

    private static string GetTableName<T>()
    {
        return typeof(T).Name;
    }

    private static List<string> GetPropertyNames<T>(bool filtered = false)
    {
        var propertyNameList = typeof(T).GetProperties()
            .Select(p => p.Name)
            .ToList();

        if (filtered)
        {
            return propertyNameList.Where(name => name != $"{typeof(T).Name}Id").ToList();
        }
        return propertyNameList;
    }

    public async Task<List<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut)
    {
        var query = @"SELECT * 
                  FROM Room r
                  LEFT JOIN Booking b ON r.RoomId = b.RoomId
                  AND NOT (b.CheckIn >= @checkOut OR b.CheckOut <= @checkIn)
                  WHERE b.BookingId IS NULL;";

        return (await _connection.QueryAsync<Room>(query, new { checkIn, checkOut })).ToList();
    }
}
