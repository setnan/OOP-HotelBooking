using Dapper;
using HotelBooking.Models;
using Npgsql;

namespace HotelBooking.Database;

public class DatabaseConnection
{
    private static DatabaseConnection? _instance;
    public static DatabaseConnection Instance => _instance ??= new DatabaseConnection();

    private readonly NpgsqlConnection _connection;

    private DatabaseConnection()
    {
        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                               ?? throw new InvalidOperationException("Missing CONNECTION_STRING");

        _connection = new NpgsqlConnection(connectionString);
    }

    public void Open() => _connection.Open();
    public void Close() => _connection.Close();

    public List<T> ExecuteQuery<T>(string query, Func<NpgsqlDataReader, T> map)
    {
        var result = new List<T>();
        using var command = new NpgsqlCommand(query, _connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            result.Add(map(reader));
        }
        return result;
    }

    public void ExecuteNonQuery(string query)
    {
        using var command = new NpgsqlCommand(query, _connection);
        command.ExecuteNonQuery();
    }

    public void ExecuteSql(string sql, object parameters)
    {
        _connection.Execute(sql, parameters);
    }

    public object? ExecuteQueryRow(string mail)
    {
        const string query = "SELECT * FROM \"User\" WHERE \"Email\" = @email";
        return _connection.QuerySingleOrDefault<User>(query, new { email = mail });
    }

    public List<T> GetAll<T>(string tableName)
    {
        var query = $"SELECT * FROM \"{tableName}\"";
        return _connection.Query<T>(query).ToList();
    }

    public T? GetOne<T>(string sql, object parameters)
    {
        return _connection.QuerySingleOrDefault<T>(sql, parameters);
    }
}