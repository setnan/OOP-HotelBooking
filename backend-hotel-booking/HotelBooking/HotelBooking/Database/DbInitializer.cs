using Npgsql;

namespace HotelBooking.Database;

public static class DbInitializer
{
    public static void Run(NpgsqlConnection connection)
    {
        // Sjekk om databasen er allerede initialisert
        using var checkCmd = new NpgsqlCommand("SELECT to_regclass('\"User\"');", connection);
        var result = checkCmd.ExecuteScalar();

        if (result != DBNull.Value && result != null)
        {
            Console.WriteLine("Database already initialized.");
            return;
        }

        Console.WriteLine("Initializing database...");

        // Finner riktig path til init_postgres.sql
        var basePath = AppContext.BaseDirectory;
        var sqlPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "Database", "init_postgres.sql"));
        var sqlScript = File.ReadAllText(sqlPath);

        // Kjører scriptet
        using var initCmd = new NpgsqlCommand(sqlScript, connection);
        initCmd.ExecuteNonQuery();

        Console.WriteLine("Database initialized successfully.");
    }
}