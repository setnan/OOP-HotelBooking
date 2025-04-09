using HotelBooking.Core.Database;
using HotelBooking.Core.Models;
using HotelBooking.Core.Utilities;

namespace HotelBooking.Core.Services;

public class ClientService
{
    private readonly DatabaseConnection _db;
    public ClientService(DatabaseConnection db)
    {
        _db = db;
    }
    
    public async Task<List<Client>> GetAllClientsAsync()
    {
        return await _db.GetAllAsync<Client>();
    }
    
    public async Task<bool> AddClientAsync(Client client)
    {
        return await _db.InsertAsync(client);
    }

    
    public async Task<bool> UpdateClientAsync(Client client, string json)
    {
        if (client.ApplyUpdatesFromJson(json))
        {
            return await _db.UpdateAsync(client);
        }
        return false;
    }

    
    public async Task<bool> UpdateClientAsync(Client client)
    {
        return await _db.UpdateAsync(client);
    }
    
    
    public async Task<bool> DeleteClientAsync(Client client)
    {
        return await _db.DeleteAsync(client);
    }
    
    
    public async Task<Client?> GetClientByIdAsync(int id)
    {
        return await _db.GetOneAsync<Client>("ClientId", id);
    }

    
    public async Task<Client?> GetClientByBillingAddressAsync(string billingAddress)
    {
        return await _db.GetOneAsync<Client>("BillingAddress", billingAddress);
    }

    
    public async Task<Client?> GetClientByNameAsync(string name)
    {
        return await _db.GetOneAsync<Client>("Name", name);
    }
}