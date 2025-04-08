# 🏨 HotelBooking - Hotellreservasjonssystem

Velkommen til vårt hotellreservasjonssystem, laget som del av OOP GA Arbeidskrav 2.

Prosjektet er en backend-applikasjon skrevet i C# (.NET), med Avalonia som frontend og MySQL som database.

## 🗂️ Prosjektstruktur

- **HotelBooking.Core** — All backend-logikk og database-tilkobling
- **HotelBooking.ConsoleApp** — Test-applikasjon for backend
- **HotelBooking.AvaloniaApp** — Frontend med Avalonia UI
- **Docker** — For oppsett av lokal database med MySQL

## ⚙️ Krav

- .NET 8.0 SDK
- Docker (for lokal database)
- MySQL Docker-image (følger med `docker-compose.yml`)

## 🚀 Kom i gang

### 1. Klon prosjektet
```bash
git clone <https://github.com/setnan/OOP-HotelBooking>
cd OOP-HotelBooking
```
### 2. Start opp MySQL-databasen med Docker
```bash
docker-compose up -d
```
Dette starter en MySQL-container med databasen for prosjektet.

### 3. Sett opp User Secrets for connection string
```bash
dotnet user-secrets init --project HotelBooking.Core
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "server=localhost;port=3306;database=HotelBooking;user=hotelluser;password=hotellpass"
```
Dette legger inn connection string på en trygg måte, uten å hardkode sensitive data i prosjektet.
### 4. Bygg prosjektet
```bash
dotnet build
```
### 5. Kjør Avalonia frontend
```bash
dotnet run --project HotelBooking.AvaloniaApp
```

🧩 Teknologier brukt
C# .NET 8.0
Avalonia UI
Dapper ORM
MySQL
Docker
User Secrets for sikker håndtering av connection string


