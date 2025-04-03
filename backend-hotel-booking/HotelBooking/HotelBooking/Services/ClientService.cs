using System.Data;
using HotelBooking;
using HotelBooking.Models;

public class ClientService
{
    private readonly DatabaseConnection _db;

    public ClientService(DatabaseConnection db)
    {
        _db = db;
    }

    public List<Client> GetAllClients()
    {
        string query = "SELECT * FROM Client";
        return _db.ExecuteQuery(query, reader => new Client
        {
            ClientId = reader.GetInt32("ClientId"),
            Name = reader.GetString("Name"),
            BillingAddress = reader.GetString("BillingAddress"),
            ContactPerson = reader.GetString("ContactPerson"),
            ContactNumber = reader.GetString("ContactNumber")
        });
    }

    public void AddClient(Client client)
    {
        string query = "INSERT INTO Client (Name, BillingAddress, ContactPerson, ContactNumber) VALUES (@Name, @BillingAddress, @ContactPerson, @ContactNumber)";
        _db.ExecuteSql(query, client);
    }
}