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
<< Simon bidrag avsnitt >>

Simon - Reflection  
<< Simon Hva har man lært og hva kan jobbes med >>

Marcus - Bidrag  
<< Marcus bidrag avsnitt >>

Marcus - Refleksjon  
<< Marcus Hva har man lært og hva kan jobbes med >>  

#### 🤖 AI Bruk  
- **In this Project Ai has been used for conceptual sparring and learing some new features previously uknow.**
- **Checklists and good workplans have been reviewed by and helped with by Ai for some parts.**
- **Prompts and conversations were used for inspiration and guiding, no direct code.**   
-*some code were copied where 1 or two words were changed.*  
-*Ai was used for a good structured and clean looking read.me. Which was kind of a waste,*  
-*since we ended up having to redo the whole thing (**but the emojies are nice**)*
- 
### 🛠️ Debug setup / Testdata  
The database will automaticly initialized with init.sql when docker-compose up -d is ran to start  
the container.   
Dummy-data is included for all tables: Clients, Rooms, Guests, Bookings,  
Events, Meals. Every table is on the other all in singulars (Meal, Room, etc.)

## 💡 Annet
<< We were planning on doing more of a full stack solution with frontend working through and API , but 
because of the time constraints we had to scrap that last minute and spend the last few days 
instead working with Avalonia. This forced some issues and refactoring, but we still feel that what
we are handing in is something we can say us decently happy with. " >>