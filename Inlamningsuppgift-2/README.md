# FailSafe WeatherMonitor

En konsolapplikation utvecklad i **C# och .NET 10** för att samla in, lagra och analysera väderdata.

Projektet är en del av utbildningen **Systemutvecklare DevSecOps vid KYH** och utvecklas stegvis med fokus på objektorientering, databashantering, felhantering och tydlig kodstruktur.

## Tekniker

- **C# / .NET 10** – applikationsutveckling
- **Spectre.Console** – interaktivt konsolgränssnitt
- **Entity Framework Core** – databashantering med Code First
- **SQL Server LocalDB** – lokal databas
- **Open-Meteo API** – aktuell väderinformation
- **JSON** – API-data och applikationskonfiguration
- **Async / Await** – asynkron programmering

## Projektstruktur

| Mapp / Fil | Beskrivning |
|---|---|
| `Program.cs` | Applikationens startpunkt |
| `Models/` | API-modeller och databasmodeller |
| `Data/WeatherDataContext.cs` | EF Core DbContext |
| `Migrations/` | Databasens versionshantering |
| `Services/WeatherApiService.cs` | Kommunikation med Open-Meteo |
| `Services/WeatherService.cs` | Väderlogik, mapping och databaslagring |
| `UI/Menu.cs` | Huvudmeny och navigering |
| `UI/WeatherScreen.cs` | Presentation av väderinformation |
| `UI/AppTheme.cs` | Gemensam konsoldesign och färghantering |
| `UI/NoticeType.cs` | Meddelandetyper för gränssnittet |
| `appsettings.json` | Lokal databaskonfiguration |

## Implementerat

- Projektstruktur med separerade ansvarsområden
- Interaktiv konsolmeny
- Anpassad konsoldesign
- API-integration med Open-Meteo
- JSON-deserialisering
- EF Core Code First
- SQL Server LocalDB
- InitialCreate-migration
- Mapping mellan API-modell och databasmodell
- Implementerad kod för asynkron databaslagring
- Dubblettkontroll baserad på station och tidstämpel
- Statusmeddelanden för information, framgång och fel


## Återstående arbete

- Verifiera att API-data sparas korrekt i SQL Server
- Förbättra felhantering och tidszonshantering
- Implementera import av historisk JSON-data
- Validera och hantera korrupta mätvärden
- Implementera felloggning till `bad_data.log`
- Visa väderstatistik med LINQ
- Testa och konfigurera molndatabas
- Slutföra dokumentation och tester

## Köra projektet lokalt

Projektet använder .NET 10 och SQL Server LocalDB.

Databasanslutningen konfigureras i `appsettings.json`.

EF Core-migrationer används för att skapa och uppdatera databasstrukturen.

Projektet är fortfarande under utveckling och alla funktioner är ännu inte färdigtestade.

## Dokumentation

Applikationen utvecklas med fokus på **Separation of Concerns**, där presentation, programlogik, externa API-anrop och databashantering har olika ansvarsområden.

Vissa gränssnittskomponenter har vidareutvecklats från ett tidigare privat C#-övningsprojekt och anpassats till WeatherMonitor.

## Källor och studiematerial

Följande resurser används som studiematerial och tekniska referenser under utvecklingen.

| Område | Källa |
|---|---|
| Entity Framework Core | [EF Core for Beginners – Microsoft](https://www.youtube.com/playlist?list=PLdo4fOcmZ0oXCPdC3fTFA3Z79-eVH3K-s) |
| EF Core Migrations | [Entity Framework Migrations Explained – Microsoft](https://learn.microsoft.com/en-us/shows/on-dotnet/entity-framework-migrations-explained) |
| C# och LINQ | [C# Advanced – Microsoft Learn](https://learn.microsoft.com/en-us/shows/C-Advanced/) |
| LINQ | [LINQ for Lists – IAmTimCorey](https://www.youtube.com/watch?v=yClSNQdVD7g) |
| Async / Await | [C# Async / Await – IAmTimCorey](https://www.youtube.com/watch?v=2moh18sh5p4) |
| HTTP och API | [HTTP requests in .NET – Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/csharp/tutorials/console-webapiclient) |
| API-anrop i C# | [How To Call An API in C# – IAmTimCorey](https://www.youtube.com/watch?v=aWePkE2ReGw) |

---

**Status:** Under utveckling.