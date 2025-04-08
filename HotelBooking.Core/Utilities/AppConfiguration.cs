using Microsoft.Extensions.Configuration;

namespace HotelBooking.Core.Utilities;

public static class AppConfiguration
{
    public static IConfigurationRoot Configuration { get; }

    static AppConfiguration()
    {
        Configuration = new ConfigurationBuilder()
            .AddUserSecrets<UserSecretsAnchor>()
            .Build();
    }
}