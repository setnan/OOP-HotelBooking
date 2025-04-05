# Dockerfile for å bygge og kjøre ASP.NET Core Web API på Render.com
# Bygger prosjektet, publiserer til /app, og starter API med støtte for Render's PORT

# Byggesteg
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app

# Kjøresteg
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .

# Render krav at appen vår bruker PORT fra env
ENV ASPNETCORE_URLS=http://+:$PORT
ENTRYPOINT ["dotnet", "HotelBooking.dll"]
