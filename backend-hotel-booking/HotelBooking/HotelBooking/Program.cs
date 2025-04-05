using HotelBooking.Database;
using HotelBooking.Services;
using HotelBooking;
using OOP_HotelBooking.Services;

var builder = WebApplication.CreateBuilder(args);

// Render krever at vi lytter på env-var PORT
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// CORS for frontend (f.eks. hosted på Vercel/Netlify/Render)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("https://booking.etnan.dev") // din frontend URL!
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddSingleton<DatabaseConnection>(_ =>
{
    var db = DatabaseConnection.Instance;
    db.Open();
    return db;
});

builder.Services.AddSingleton<BookingService>();
builder.Services.AddSingleton<GuestService>();
builder.Services.AddSingleton<RoomService>();
builder.Services.AddSingleton<ClientService>();
builder.Services.AddSingleton<CateringService>();
builder.Services.AddSingleton<EventService>();

builder.Services.AddControllers();

var app = builder.Build();

DatabaseStartup.InitializeAndConnect();

app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

app.Run();