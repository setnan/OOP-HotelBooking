using MySql.Data.MySqlClient;

namespace HotelBooking.Core.Database;

public static class DbInitializer
{
    public static void Run(MySqlConnection connection)
    {
        using var checkCmd = new MySqlCommand("SHOW TABLES LIKE 'User';", connection);
        var result = checkCmd.ExecuteScalar();

        if (result != null)
        {
            Console.WriteLine("Database already initialized.");
            return;
        }

        Console.WriteLine("Initializing database...");

        var basePath = AppContext.BaseDirectory;
        var sqlPath = Path.Combine(basePath, "init.sql");

        if (!File.Exists(sqlPath))
        {
            Console.WriteLine($"SQL file not found: {sqlPath}");
            throw new FileNotFoundException("init.sql not found", sqlPath);
        }

        var sqlScript = File.ReadAllText(sqlPath);

        using var initCmd = new MySqlCommand(sqlScript, connection);
        initCmd.ExecuteNonQuery();

        Console.WriteLine("Database initialized successfully.");
    }
}