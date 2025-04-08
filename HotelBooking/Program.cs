using HotelBooking.Core.Database;
using HotelBooking.Core.Services;
using HotelBooking.Core.Models;
using HotelBooking.Core.Services;
using HotelBooking.Core.Models;


var builder = WebApplication.CreateBuilder(args);

// Lytt til port satt av Render (fallback til 5000 lokalt)
// var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
// builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// CORS-policy for Vercel-produksjon og lokal utvikling
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins(
                "https://booking.etnan.dev",
                "http://localhost:3000"
            )
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Registrer databaseforbindelse (singleton)
builder.Services.AddSingleton<DatabaseConnection>(_ =>
{
    var db = DatabaseConnection.Instance;
    db.Open();
    return db;
});

// Registrer applikasjonstjenester
builder.Services.AddSingleton<BookingService>();
builder.Services.AddSingleton<GuestService>();
builder.Services.AddSingleton<RoomService>();
builder.Services.AddSingleton<ClientService>();
builder.Services.AddSingleton<EventService>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Init database ved oppstart hvis nødvendig
DatabaseStartup.InitializeAndConnect();

// Aktiver CORS og ruting
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

app.Run();
