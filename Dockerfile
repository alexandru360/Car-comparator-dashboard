FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/CarComparator.Api/CarComparator.Api.csproj", "CarComparator.Api/"]
COPY ["src/CarComparator.Shared/CarComparator.Shared.csproj", "CarComparator.Shared/"]
COPY ["src/Modules/Cars/CarComparator.Modules.Cars.csproj", "Modules/Cars/"]
COPY ["src/Modules/Scraping/CarComparator.Modules.Scraping.csproj", "Modules/Scraping/"]
RUN dotnet restore "CarComparator.Api/CarComparator.Api.csproj"
COPY src/ .
RUN dotnet build "CarComparator.Api/CarComparator.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarComparator.Api/CarComparator.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarComparator.Api.dll"]
