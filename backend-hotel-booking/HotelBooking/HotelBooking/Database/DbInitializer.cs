// Dbinitializer er kun for lokaltesting,
// nå som vi allerede har initialisert databasen på Render.com trengs egentlig ikke denne.

using Npgsql;

namespace HotelBooking.Database;

public static class DbInitializer
{
    public static void Run(NpgsqlConnection connection)
    {
        // Sjekker om databasen allerede er initialisert (tabellen "User" finnes)
        using var checkCmd = new NpgsqlCommand("SELECT to_regclass('\"User\"');", connection);
        var result = checkCmd.ExecuteScalar()?.ToString();

        if (!string.IsNullOrEmpty(result))
        {
            Console.WriteLine("Database already initialized.");
            return;
        }

        Console.WriteLine("Initializing database...");

        // Bruker base directory fra runtime (Render eller lokal)
        var basePath = AppContext.BaseDirectory;

        // Antas at init_postgres.sql ligger direkte i /app (ved deploy)
        var sqlPath = Path.Combine(basePath, "init_postgres.sql");

        if (!File.Exists(sqlPath))
        {
            Console.WriteLine($"Fant ikke SQL-fil: {sqlPath}");
            throw new FileNotFoundException("init_postgres.sql ikke funnet", sqlPath);
        }

        var sqlScript = File.ReadAllText(sqlPath);

        // Kjører SQL-script
        using var initCmd = new NpgsqlCommand(sqlScript, connection);
        initCmd.ExecuteNonQuery();

        Console.WriteLine("Database initialized successfully.");
    }
}
