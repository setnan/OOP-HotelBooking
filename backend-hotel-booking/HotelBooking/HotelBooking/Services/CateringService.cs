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
        _db.ExecuteSql(query, c);
    }

    public List<Catering> GetAllCaterings()
    {
        string query = "SELECT * FROM Catering";
        return _db.ExecuteQuery(query, reader => new Catering
        {
            CateringId = reader.GetInt32("CateringId"),
            ClientId = reader.GetInt32("ClientId"),
            RoomId = reader.GetInt32("RoomId"),
            Date = reader.GetDateTime("Date"),
            StartTime = reader.GetTimeSpan("StartTime"),
            EndTime = reader.GetTimeSpan("EndTime"),
            ExpectedAttendees = reader.GetInt32("ExpectedAttendees"),
            DietaryNotes = reader.GetString("DietaryNotes")
        });
    }
}