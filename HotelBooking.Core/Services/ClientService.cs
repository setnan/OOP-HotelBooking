using HotelBooking.Core.Database;
using HotelBooking.Core.Models;
using HotelBooking.Core.Utilities;

namespace HotelBooking.Core.Services;

public class ClientService(DatabaseConnection instance)
{

    public static List<Client> GetAllClients()
    {
        return DatabaseConnection.Instance.GetAll<Client>();
    }
    
    public static bool AddClient(Client client)
    {
        return DatabaseConnection.Instance.Insert(client);
    }

    
    public static bool UpdateClient(Client client, string json)
    {
        if (client.ApplyUpdatesFromJson(json))
        {
            return DatabaseConnection.Instance.Update(client);
        }
        return false;
    }

    
    public static bool UpdateClient(Client client)
    {
        return DatabaseConnection.Instance.Update(client);
    }
    
    
    public static bool DeleteClient(Client client)
    {
        return DatabaseConnection.Instance.Delete(client);
    }
    
    
    public static Client? GetClientById(int id)
    {
        return DatabaseConnection.Instance.GetOne<Client>("ClientId", id);
    }

    
    public static Client? GetClientByBillingAddress(string billingAddress)
    {
        return DatabaseConnection.Instance.GetOne<Client>("BillingAddress", billingAddress);
    }

    
    public static Client? GetClientByName(string name)
    {
        return DatabaseConnection.Instance.GetOne<Client>("Name", name);
    }
}