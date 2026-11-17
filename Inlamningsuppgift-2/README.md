# FailSafe WeatherMonitor

A C# console application built with **.NET 11** for collecting, importing, storing and monitoring weather data.

The project is part of the **Systemutvecklare DevSecOps programme at KYH** and focuses on object-oriented programming, database management, API integration and reliable data handling.

## Technologies

- **C# / .NET 11** – Application development
- **Entity Framework Core 11** – Database management and Code First
- **SQL Server LocalDB** – Local database
- **Microsoft Azure SQL** – Cloud database integration
- **Spectre.Console** – Interactive console interface
- **Open-Meteo API** – Weather data provider
- **System.Text.Json** – JSON parsing and serialization
- **LINQ** – Data querying and filtering

## Features

- Retrieve current weather data through Open-Meteo API.
- Import historical weather data from JSON files.
- Validate and filter incoming weather measurements.
- Detect duplicate records using station name and timestamp.
- Store weather data using Entity Framework Core.
- Log rejected records for further inspection.
- Display weather information through a custom console interface.

## Project Structure

```text
FailSafe WeatherMonitor/
│
├── Data/
│   ├── WeatherDataContext.cs
│   └── weather_data.json
│
├── Migrations/
│
├── Models/
│   ├── CurrentWeather.cs
│   ├── OpenMeteoResponse.cs
│   └── WeatherDataPoint.cs
│
├── Services/
│   ├── ImportService.cs
│   ├── WeatherApiService.cs
│   └── WeatherService.cs
│
├── UI/
│   ├── AppTheme.cs
│   ├── Menu.cs
│   ├── NoticeType.cs
│   └── WeatherScreen.cs
│
├── Program.cs
├── appsettings.json
├── appsettings.Production.json
└── README.md
```

## Getting Started

**Requirements**
- .NET 11 SDK
- SQL Server LocalDB
- Visual Studio 2026 or compatible development environment

Configure the database connection in `appsettings.json`.

Clone the repository and run:

```bash
dotnet restore
dotnet build
dotnet run
```

Database migrations are applied automatically when the application starts.

## Azure SQL Demonstration

Cloud database integration will be demonstrated using Microsoft Azure SQL Database.

The demonstration will document the database configuration, EF Core migrations and successful import of weather data into the cloud database.

<!-- Azure screenshots will be added here after verification. -->

## Project Status

Under development as part of the Systemutvecklare DevSecOps programme at KYH.

**Author:** Rami Issa