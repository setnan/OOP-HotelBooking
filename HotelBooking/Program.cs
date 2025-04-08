using Dumpify;
using HotelBooking.Core.Database;
using HotelBooking.Core.Services;
using HotelBooking.Core.Models;

Console.WriteLine("Get BY ID ");
var client = ClientService.GetClientById(8);
Console.WriteLine(client.Dump());

var newClient = new Client
{
    Name = "John Doe & co",
    BillingAddress = "test@gmail.com",
    ContactPerson = "John Doe",
    ContactNumber = "456-123-456"
};
ClientService.AddClient(newClient);
var clients = ClientService.GetAllClients();
Console.WriteLine($"\nAdd Client");
foreach (var clientel in clients)
{
    Console.WriteLine(clientel.Dump());
}

Console.WriteLine("\nCHECKPOINT!");
var getClient = ClientService.GetClientById(11);

if (getClient != null) getClient.Name = "John Doe & co & co";
if (getClient != null) ClientService.UpdateClient(getClient);
var getClients = ClientService.GetAllClients();
Console.WriteLine($"\nUpdate Client");
foreach (var newclient in getClients)
{
    Console.WriteLine(newclient.Dump());
}

ClientService.DeleteClient(getClient);

var gottenClients = ClientService.GetAllClients();
Console.WriteLine($"\nDelete Client");
foreach (var newclientell in gottenClients)
{
    Console.WriteLine(newclientell.Dump());
}

