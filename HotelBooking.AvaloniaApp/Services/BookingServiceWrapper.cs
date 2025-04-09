using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Core.Models;
using HotelBooking.Core.Services;

namespace HotelBooking.AvaloniaApp.Services;

public class ClientServiceWrapper
{
    private readonly ClientService clientService;

    public ClientServiceWrapper(ClientService clientService)
    {
        this.clientService = clientService;
    }

    public async Task<IEnumerable<Client>> GetAllClientsAsync()
    {
        return await clientService.GetAllClientsAsync();
    }

    public async Task<bool> AddClientAsync(Client client)
    {
        return await clientService.AddClientAsync(client);
    }

    public async Task<bool> UpdateClientAsync(Client client)
    {
        return await clientService.UpdateClientAsync(client);
    }

    public async Task<bool> DeleteClientAsync(Client client)
    {
        return await clientService.DeleteClientAsync(client);
    }

    public async Task<Client?> GetClientByIdAsync(int id)
    {
        return await clientService.GetClientByIdAsync(id);
    }
}