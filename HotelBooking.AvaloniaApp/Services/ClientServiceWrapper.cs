using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Core.Models;
using HotelBooking.Core.Services;

namespace HotelBooking.AvaloniaApp.Services;

public class ClientServiceWrapper
{
    public async Task<IEnumerable<Client>> GetAllClientsAsync()
    {
        return await Task.Run(() => ClientService.GetAllClients());
    }

    public async Task<bool> AddClientAsync(Client client)
    {
        return await Task.Run(() => ClientService.AddClient(client));
    }

    public async Task<bool> UpdateClientAsync(Client client)
    {
        return await Task.Run(() => ClientService.UpdateClient(client));
    }

    public async Task<bool> DeleteClientAsync(int id)
    {
        return await Task.Run(() => ClientService.DeleteClient(id));
    }
}
