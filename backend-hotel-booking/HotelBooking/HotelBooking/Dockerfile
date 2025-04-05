# Dockerfile for å bygge og kjøre ASP.NET Core Web API på Render.com
# Bygger prosjektet, publiserer til /app, og starter API med støtte for Render's PORT

# Byggesteg
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Kopier hele prosjektet inkludert init_postgres.sql
COPY . .

# Viktig! Sørg for at SQL-fila kommer på riktig sted i runtime-containeren
RUN dotnet publish -c Release -o /app

# Kjøresteg
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Kopier utbygd app og SQL-fil til runtime container
COPY --from=build /app . 
COPY init_postgres.sql /Database/init_postgres.sql

ENV ASPNETCORE_URLS=http://+:$PORT
ENTRYPOINT ["dotnet", "HotelBooking.dll"]

