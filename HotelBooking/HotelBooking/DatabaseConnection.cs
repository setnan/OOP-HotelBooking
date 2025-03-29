using HotelBooking.Models;
using MySql.Data.MySqlClient;
using Dapper;
using MySqlX.XDevAPI.Common;

namespace HotelBooking;

public class DatabaseConnection
{
    private static DatabaseConnection? _instance;
    public static DatabaseConnection Instance => _instance ??= new DatabaseConnection();
    
    private readonly MySqlConnection _connection;
    private readonly string _connectionString = "server=localhost;" +
                                       "port=3306;" +
                                       "database=HotelBooking;" +
                                       "uid=hotelluser;" +
                                       "pwd=hotellpass;";

    private DatabaseConnection()
    {
        _connection = new MySqlConnection(_connectionString);
    }

    public void Open() => _connection.Open();
    public void Close() => _connection.Close();

    public List<T> ExecuteQuery<T>(string query, Func<MySqlDataReader, T> map)
    {
        var result = new List<T>();
        using var command = new MySqlCommand(query, _connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            result.Add(map(reader));
        }
        return result;
    }

    public void ExecuteNonQuery(string query)
    {
        using var command = new MySqlCommand(query, _connection);
        command.ExecuteNonQuery();
    }
    
    public void ExecuteSql(string sql, object parameters)
    {
        _connection.Execute(sql, parameters);
    }
    
    public object? ExecuteQueryRow(string mail)
    {
        
        var query = "SELECT * FROM User WHERE Email = @email";
        var user = _connection.QuerySingleOrDefault<User>(query, new { email = mail });
        return user;
    }

    public List<T> GetAll<T>(string tableName)
    {
        var query = "SELECT * FROM " + tableName;
        return _connection.Query<T>(query).ToList();
    }
    
    public T? GetOne<T>(string sql, object parameters)
    {
        return _connection.QuerySingleOrDefault<T>(sql, parameters);
    }

    
    
}