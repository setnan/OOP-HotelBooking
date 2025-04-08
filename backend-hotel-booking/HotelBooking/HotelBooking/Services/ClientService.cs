using System.Data;
using HotelBooking;
using HotelBooking.Database;
using HotelBooking.Models;
using HotelBooking.Utilities;

namespace HotelBooking.Services;

public class ClientService(DatabaseConnection instance)
{

    
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

    
    public static bool DeleteClient(Client client)
    {
        return DatabaseConnection.Instance.Delete(client);
    }

    
    public static List<Client> GetAllClients()
    {
        return DatabaseConnection.Instance.GetAll<Client>();
    }

    
    public static Client? GetClientById(int id)
    {
        return DatabaseConnection.Instance.GetOne<Client>("ClientId", id);
    }

    public static Client? GetClientByEmail(string email)
    {
        return DatabaseConnection.Instance.GetOne<Client>("Email", email);
    }

    public static Client? GetClientByName(string name)
    {
        return DatabaseConnection.Instance.GetOne<Client>("ClientName", name);
    }
}