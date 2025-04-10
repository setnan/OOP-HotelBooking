using HotelBooking.Core.Backup;
using HotelBooking.Core.Database;
using HotelBooking.Core.Models;
using HotelBooking.Core.Utilities;

namespace HotelBooking.Core.Services;

public class ClientService : IBackupService<Client>
{
    private readonly DatabaseConnection _db;
    private readonly EventClientService _eventClientService;
    public ClientService(DatabaseConnection db, EventClientService eventClientService)
    {
        _db = db;
        _eventClientService = eventClientService;
    }
    
    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await _db.GetAllAsync<Client>();
    }

    public async Task InsertManyAsync(IEnumerable<Client> items)
    {
        foreach (var item in items)
        {
            await _db.InsertAsync(item);
        }
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

    public async Task<IEnumerable<Event>> GetAllEventsForClientAsync(int clientId)
    {
        var eventClients = await _eventClientService.GetAllByClientIdAsync(clientId);
        List<Event> clientEvents  = new List<Event>();
        foreach (var eventClient in eventClients)
        {
            var currentEvent = await _db.GetOneAsync<Event>("EventId", eventClient.EventId);
            if (currentEvent != null) clientEvents.Add(currentEvent); 
        }
        return clientEvents;
    }
}