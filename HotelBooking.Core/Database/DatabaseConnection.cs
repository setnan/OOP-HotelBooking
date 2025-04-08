using Dapper;
using HotelBooking.Core.Models;
using HotelBooking.Core.Utilities;
using MySql.Data.MySqlClient;


namespace HotelBooking.Core.Database;

public class DatabaseConnection
{
    private readonly string? _connectionString = AppConfiguration.Configuration["ConnectionStrings:DefaultConnection"];

    private static DatabaseConnection? _instance;
    public static DatabaseConnection Instance => _instance ??= new DatabaseConnection();

    private readonly MySqlConnection _connection;

    private DatabaseConnection()
    {
        _connection = new MySqlConnection(_connectionString);
    }

    public MySqlConnection GetConnection()
    {
        return _connection;
    }

    public void Open() => _connection.Open();
    public void Close() => _connection.Close();
    
    
    public List<T> GetAll<T>()
    {
        var table = GetTableName<T>();
        var query = $"SELECT * FROM {table}";
        return _connection.Query<T>(query).ToList();
    }


    public List<T> GetAllWhere<T>(string parameterName, object parameterValue)
    {
        var table = GetTableName<T>();
        var parameters = new  Dictionary<string, object>{ { parameterName, parameterValue }};
        var (key, value) =  parameters.First();
        
        var query = $"SELECT * FROM {table} WHERE {key} = @{key}";
        return _connection.Query<T>(query, parameters).ToList();
    }

    
    public T? GetOne<T>(string parameterName, object parameterValue)
    {
        var table = GetTableName<T>();
        var parameters = new  Dictionary<string, object>{ { parameterName, parameterValue }};
        var (key, value) =  parameters.First();
        
        var query = $"SELECT * FROM {table} WHERE {key} = @{key}";
        return _connection.QuerySingleOrDefault<T>(query, parameters);
    }
    
    
    public bool Insert<T>(T entity)
    {
        var table = typeof(T).Name;
        var propertyNameList = new List<string>();

        foreach (var property in typeof(T).GetProperties())
        {
            propertyNameList.Add(property.Name);
        }

        var propertyNameListFiltered = propertyNameList
            .Where(name => name != $"{typeof(T).Name}Id").ToList();


        var columns = string.Join(", ", propertyNameListFiltered);
        var parameters = string.Join(", ", propertyNameListFiltered.Select(name => "@" + name));

        var insertQuery = $"INSERT INTO {table} ({columns}) VALUES ({parameters})";

        var rowsAffected = _connection.Execute(insertQuery, entity);
        return rowsAffected > 0;
    }
    
    
    public bool Update<T>(T entity)
    {
        var table = GetTableName<T>();

        var propertyNameListFiltered = GetPropertyNames<T>(true);

        var setClause = string.Join(", ", propertyNameListFiltered.Select(name => $"{name} = @{name}"));
        var updateQuery = $"UPDATE {table} SET {setClause} WHERE {table}Id = @{table}Id";

        var rowsAffected = _connection.Execute(updateQuery, entity);
        return rowsAffected > 0;
    }

    
    public bool Delete<T>(T entity)
    {
        var table = GetTableName<T>();
        
        var deleteQuery = $"DELETE FROM {table} WHERE {table}Id = @{table}Id";
        var rowsAffected = _connection.Execute(deleteQuery, entity);
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
    
    public List<Room> GetAvailableRooms(DateTime checkIn, DateTime checkOut)
    {
        var query = @"SELECT * 
                  FROM Room r
                  LEFT JOIN Booking b ON r.RoomId = b.RoomId
                  AND NOT (b.CheckIn >= @checkOut OR b.CheckOut <= @checkIn)
                  WHERE b.BookingId IS NULL;";

        return _connection.Query<Room>(query, new { checkIn, checkOut }).ToList();
    }
}