# Hotel Booking API - Arbeidskrav 2 OOP
<span style="color: teal">Utviklet av: Marcus Brustad og Simon Etnan</span>

Dette prosjektet er en fullstack hotellbookingsløsning utviklet som en del av Arbeidskrav 2 
i Objektorientert programmering ved Gokstadakademiet våren 2025</span>

Systemet består av:

- Backend: ASP.NET Core Web API (C#)
- Database: PostgreSQL (Render.com)
- Frontend: Next.js (React)

## Funksjonalitet

- Brukerinnlogging (Admin og Resepsjonist)
- CRUD-operasjoner for:
    - Gjester
    - Rom
    - Bookinger
- Automatisk databaseinitialisering ved første kjøring
- Frontend med støtte for glemt passordµ
- Klar for JWT-autentisering (kan aktiveres senere)

## Teknologier

| Teknologi     | Brukt til                      |
|---------------|--------------------------------|
| C# / .NET 8   | Backend + OOP-modellering      |
| Dapper        | Datatilgang (lettvekts ORM)    |
| PostgreSQL    | Relasjonsdatabase              |
| Render.com    | Deploy og hosting              |
| Next.js       | Frontend med React             |
| Docker        | Containerbasert deploy         |

## Prosjektstruktur

Her kommer mer info..

Login info:
admin@hotel.com
pw: pass123

reseptionist@hotel.com
pw: pass1234