// Dbinitializer er kun for lokaltesting,
// nå som vi allerede har initialisert databasen på Render.com trengs egentlig ikke denne.

using Npgsql;

namespace HotelBooking.Core.Database;

public static class DbInitializer
{
    public static void Run(NpgsqlConnection connection)
    {
        // Cast 'regclass' til 'text' for å unngå Npgsql-feil
        using var checkCmd = new NpgsqlCommand("SELECT to_regclass('\"User\"')::text;", connection);
        var result = checkCmd.ExecuteScalar() as string;

        if (!string.IsNullOrEmpty(result))
        {
            Console.WriteLine("Database already initialized.");
            return;
        }

        Console.WriteLine("Initializing database...");

        var basePath = AppContext.BaseDirectory;
        var sqlPath = Path.Combine(basePath, "init_postgres.sql");

        if (!File.Exists(sqlPath))
        {
            Console.WriteLine($"Fant ikke SQL-fil: {sqlPath}");
            throw new FileNotFoundException("init_postgres.sql ikke funnet", sqlPath);
        }

        var sqlScript = File.ReadAllText(sqlPath);

        using var initCmd = new NpgsqlCommand(sqlScript, connection);
        initCmd.ExecuteNonQuery();

        Console.WriteLine("Database initialized successfully.");
    }
}
