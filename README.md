# 🏨 HotelBooking - Hotellreservasjonssystem

Welcome to our hotelBooking system. Made for OOP GA Arbeidskrav 2.

The project is a backend-application written in C#(.NET), with Avalonia as frontend and MySQL for the database.

## 🗂️ ProjectStructure

- **HotelBooking.Core** — All backend-logic and database-connections
- **HotelBooking.AvaloniaApp** — Frontend with Avalonia UI
- **Docker** — For setting up local database

## ⚙️ Project requirements

- .NET 8.0 SDK
- Docker 
- MySQL Docker-image (Will be setup by docker-compose.yml *steps below.*)

## 🚀 Getting Started.

1. **Cloning the project** - *If we set it public. will also be in ready in the .zip*
```bash


git clone <https://github.com/setnan/OOP-HotelBooking>
cd OOP-HotelBooking 
```
*Move to the project folder*--^

2. **Start the MySQL Database with the docker-compose** - *should cd into hotell-docker-setup folder*

```bash


docker-compose up -d
```

3. **Setup the User Secrets** 

```bash


dotnet user-secrets set --project HotelBooking.Core "ConnectionStrings:DefaultConnection" "server=localhost;port=3306;database=HotelBooking;user=hotelluser;password=hotellpass"
dotnet user-secrets set --project HotelBooking.AvaloniaApp "ConnectionStrings:DefaultConnection" "server=localhost;port=3306;database=HotelBooking;user=hotelluser;password=hotellpass"
```

4. **Build the Project**

```bash

dotnet build
```
dotnet build

5. **Run Avalonia for the frontend application**

```bash


dotnet run --project HotelBooking.AvaloniaApp
```

## 🧩 Techonolgies used
- **C# .NET 8.0**
- **Avalonia UI**
- **Dapper ORM**
- **MySQL**
- **Docker**
- **User Secrets** - for secure handling of the connection string

## 📋 Concepts and usefull skills
**We have focused on Object Oriented Programming(OOP)** 
- *With a focus on Well organized and structured arcitecture.*
- *Use of async / await*
- *Dapper for easy Database integration*
- *Docker for easy setup and environment for development*
- *Serviceclasses for logic away from models*
- *Backup and restore for functionality and datahandling*
- *inti(sql) with premade dummydata*
- *Clean Integreation between DLL and Avalonia application*

### Key skills learned from this project. 
- **Structuring a solution between projects.**
- **Learning to bring databases alive into frontend**
- **Working as a team while not always being in contact**
- **so using github and other tools for an everyday experience**
- **Learning to stay on DRY and SRP concepts and sticking to them**

## 👥 Contributors and reflections
Simon - Contribution  
<< Her fyller Simon inn én paragraf om hva han har bidratt med i prosjektet. >>

Simon - Reflection  
<< Her fyller Simon inn én paragraf om hva han har lært eller kan utvikle videre. >>

Marcus - Bidrag  
<< Her fyller Marcus inn én paragraf om hva han har bidratt med i prosjektet. >>

Marcus - Refleksjon  
<< Her fyller Marcus inn én paragraf om hva han har lært eller kan utvikle videre. >>

🤖 AI Bruk  
I prosjektet har vi brukt AI for konseptuell sparring og hjelp til å tenke gjennom strukturer og arkitekturvalg. Spesielt i forbindelse med:

Planlegging av BackupService og datahåndtering

Strukturering av Meal-entiteten

Oppsummering og sjekklister før levering

Promptene og samtalene ble brukt som inspirasjon og guiding, uten direkte kopiering av kode.

(💡 Hvis dere ønsker, kan dere også legge ved eksakte prompter eller si: "Ingen AI-generert kode ble direkte brukt i prosjektet.")

🛠️ Debug setup / Testdata
Databasen blir automatisk initialisert med init.sql når Docker starter containeren.

Dummy-data er inkludert for Clients, Rooms, Guests, Bookings, Events og Meals.

Ingen ekstra miljøvariabler er nødvendig, alt er håndtert via User Secrets.

💡 Annet
<< Hvis dere har noe ekstra dere vil si, f.eks. "Vi vurderte Discord-integrasjon, men valgte å fokusere på kjernefunksjonalitet grunnet tidsrammen." >>