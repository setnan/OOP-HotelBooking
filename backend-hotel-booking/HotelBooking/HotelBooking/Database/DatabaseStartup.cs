using Npgsql;

namespace HotelBooking.Database;

public static class DatabaseStartup
{
    public static void InitializeAndConnect()
    {
        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

        if (string.IsNullOrEmpty(connectionString))
        {
            Console.WriteLine("CONNECTION_STRING environment variable is missing.");
            Environment.Exit(1);
        }

        using var connection = new NpgsqlConnection(connectionString);
        connection.Open();

        DbInitializer.Run(connection);
    }
}