using MySql.Data.MySqlClient;

namespace HotelBooking;

public class DatabaseConnection
{
    private MySqlConnection _connection;
    private string _connectionString = "server=localhost;" +
                                       "port=3306;" +
                                       "database=HotelBooking;" +
                                       "uid=hotelluser;" +
                                       "pwd=hotellpass;";

    public DatabaseConnection()
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
}