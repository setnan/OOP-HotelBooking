using System.Data;
using HotelBooking;
using HotelBooking.Database;
using HotelBooking.Models;

public class CateringService
{
    private readonly DatabaseConnection _db;

    public CateringService(DatabaseConnection db)
    {
        _db = db;
    }

    public void AddCatering(Catering c)
    {
        string query = @"
            INSERT INTO Catering (ClientId, RoomId, Date, StartTime, EndTime, ExpectedAttendees, DietaryNotes)
            VALUES (@ClientId, @RoomId, @Date, @StartTime, @EndTime, @ExpectedAttendees, @DietaryNotes)";
        
        var parameters = new
        {
            ClientId = c.Organiser.ClientId,
            RoomId = c.Room.RoomId,
            c.Date,
            c.StartTime,
            c.EndTime,
            c.ExpectedAttendees,
            c.DietaryNotes
        };

        _db.ExecuteSql(query, parameters);
    }

    public List<Catering> GetAllCaterings()
    {
        string query = "SELECT * FROM Catering";

        return _db.ExecuteQuery(query, reader => new Catering
        {
            CateringId = reader.GetInt32("CateringId"),
            Date = reader.GetDateTime("Date"),
            StartTime = reader.GetTimeSpan("StartTime"),
            EndTime = reader.GetTimeSpan("EndTime"),
            ExpectedAttendees = reader.GetInt32("ExpectedAttendees"),
            DietaryNotes = reader.GetString("DietaryNotes"),

            // Lazy-load objekter med kun ID – hent full info ved behov
            Organiser = new Client { ClientId = reader.GetInt32("ClientId") },
            Room = new Room { RoomId = reader.GetInt32("RoomId") }
        });
    }
}