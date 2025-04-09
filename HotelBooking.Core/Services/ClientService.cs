using HotelBooking.Core.Database;
using HotelBooking.Core.Models;
using HotelBooking.Core.Utilities;

namespace HotelBooking.Core.Services;

public class ClientService(DatabaseConnection instance)
{

    public static Task<List<Client>> GetAllClientsAsync()
    {
        return DatabaseConnection.Instance.GetAllAsync<Client>();
    }
    
    public static async Task<bool> AddClientAsync(Client client)
    {
        return await DatabaseConnection.Instance.InsertAsync(client);
    }

    
    public static async Task<bool> UpdateClientAsync(Client client, string json)
    {
        if (client.ApplyUpdatesFromJson(json))
        {
            return await DatabaseConnection.Instance.UpdateAsync(client);
        }
        return false;
    }

    
    public static async Task<bool> UpdateClientAsync(Client client)
    {
        return await DatabaseConnection.Instance.UpdateAsync(client);
    }
    
    
    public static async Task<bool> DeleteClientAsync(Client client)
    {
        return await DatabaseConnection.Instance.DeleteAsync(client);
    }
    
    
    public static async Task<Client?> GetClientByIdAsync(int id)
    {
        return await DatabaseConnection.Instance.GetOneAsync<Client>("ClientId", id);
    }

    
    public static async Task<Client?> GetClientByBillingAddressAsync(string billingAddress)
    {
        return await DatabaseConnection.Instance.GetOneAsync<Client>("BillingAddress", billingAddress);
    }

    
    public static async Task<Client?> GetClientByNameAsync(string name)
    {
        return await DatabaseConnection.Instance.GetOneAsync<Client>("Name", name);
    }
}