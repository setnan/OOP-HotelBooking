using System.Configuration;
using Dapper;
using HotelBooking.Core.Utilities;
using MySql.Data.MySqlClient;
using Npgsql;

namespace HotelBooking.Core.Database;

public class DatabaseConnection
{
    private string? connection_string = AppConfiguration.Configuration["ConnectionStrings:DefaultConnection"];

    private static DatabaseConnection? _instance;
    public static DatabaseConnection Instance => _instance ??= new DatabaseConnection();

    private readonly MySqlConnection _connection;

    private DatabaseConnection()
    {
        _connection = new MySqlConnection(connection_string);
    }

    public MySqlConnection GetConnection()
    {
        return _connection;
    }

    public void Open() => _connection.Open();
    public void Close() => _connection.Close();

    // public List<T> ExecuteQuery<T>(string query, Func<NpgsqlDataReader, T> map)
    // {
    //     var result = new List<T>();
    //     using var command = new MySqlConnection(query, _connection);
    //     using var reader = command.ExecuteReader();
    //     while (reader.Read())
    //     {
    //         result.Add(map(reader));
    //     }
    //     return result;
    // }
    //
    // public void ExecuteNonQuery(string query)
    // {
    //     using var command = new MySqlConnection(query, _connection);
    //     command.ExecuteNonQuery();
    // }
    
    
    public List<T> GetAll<T>()
    {
        var table = GetTableName<T>();
        var query = $"SELECT * FROM {table}";
        return Enumerable.ToList<T>(_connection.Query<T>(query));
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

    
    public static string GetTableName<T>()
    {
        return typeof(T).Name;
    }

    
    public static List<string> GetPropertyNames<T>(bool filtered = false)
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
}