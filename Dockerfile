# Dockerfile per l'ambiente di test
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["backend/backend.csproj", "backend/"]
RUN dotnet restore "backend/backend.csproj"
COPY backend/ backend/
WORKDIR "/src/backend"
RUN dotnet build "backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Crea la directory per il database SQLite
RUN mkdir -p /app/data

# Imposta le variabili d'ambiente per il deployment
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5000
ENV ConnectionStrings__DefaultConnection="Data Source=/app/data/monadicpilot.db"

ENTRYPOINT ["dotnet", "backend.dll"]
