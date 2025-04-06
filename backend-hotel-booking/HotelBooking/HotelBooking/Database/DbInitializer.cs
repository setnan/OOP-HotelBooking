// Dbinitializer er kun for lokaltesting,
// nå som vi allerede har initialisert databasen på Render.com trengs egentlig ikke denne.

using Npgsql;

namespace HotelBooking.Database;

public static class DbInitializer
{
    public static void Run(NpgsqlConnection connection)
    {
        // Sjekker om databasen er allerede initialisert
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
        var sqlPath = Path.Combine(basePath, "init_postgres.sql");
        var sqlScript = File.ReadAllText(sqlPath);

        // Kjører scriptet
        using var initCmd = new NpgsqlCommand(sqlScript, connection);
        initCmd.ExecuteNonQuery();

        Console.WriteLine("Database initialized successfully.");
    }
}