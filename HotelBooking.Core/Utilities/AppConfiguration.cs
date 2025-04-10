using Microsoft.Extensions.Configuration;

namespace HotelBooking.Core.Utilities;

public static class AppConfiguration
{
    public static IConfigurationRoot Configuration { get; }

    static AppConfiguration()
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddUserSecrets<UserSecretsAnchor>()
            .Build();
    }
}