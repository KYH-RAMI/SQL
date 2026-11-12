# FailSafe WeatherMonitor

En konsolapplikation i C# för att arbeta med väderdata. Projektet utvecklas stegvis med fokus på tydlig struktur, läsbar kod och säker datahantering.

## Tekniker

- **C# / .NET 10** – applikationen
- **Spectre.Console** – menyer och presentation
- **Entity Framework Core / SQL Server** – planerad datalagring
- **JSON** – konfiguration och datautbyte

## Projektfiler

| Fil | Beskrivning |
|---|---|
| [`Program.cs`](Program.cs) | Applikationens startpunkt. |
| [`Services/Menu.cs`](Services/Menu.cs) | Huvudmeny och navigering. |
| [`Services/AppTheme.cs`](Services/AppTheme.cs) | Färger och gemensamma konsolkomponenter. |

Mapparna `Models/` och `Data/` fylls på när datamodeller och databasåtkomst implementeras.

## Status

**Under utveckling.** Grundstrukturen och menyfilerna är på plats. Datahantering och databasfunktioner återstår.

## Dokumentation

**Återanvändning av tidigare arbete:** Menyn ([`Menu.cs`](Services/Menu.cs)) och temahanteringen ([`AppTheme.cs`](Services/AppTheme.cs)) bygger på kod från mitt tidigare projekt **PragueParking V2**. Jag har modifierat och anpassat dessa komponenter för FailSafe WeatherMonitor. 
Eftersom jag lagt betydande tid på den ursprungliga implementationen har jag valt att vidareutveckla den i stället för att skriva om samma funktionalitet från grunden.

Filförteckningen ovan uppdateras när nya filer läggs till.

## Källor och studiematerial

Följande resurser har jag använt för att studera tekniker som är relevanta för projektet. De fungerar som referenser när funktionerna implementeras.

| Område | Källa |
|---|---|
| Entity Framework Core | [Entity Framework Core for Beginners – Microsoft (videokurs)](https://www.youtube.com/playlist?list=PLdo4fOcmZ0oXCPdC3fTFA3Z79-eVH3K-s) |
| EF Core Migrations | [Entity Framework Migrations Explained – Microsoft](https://learn.microsoft.com/en-us/shows/on-dotnet/entity-framework-migrations-explained) |
| C# och LINQ | [C# Advanced – Microsoft Learn](https://learn.microsoft.com/en-us/shows/C-Advanced/) |
| LINQ för listor | [C# Essentials – LINQ for Lists – IAmTimCorey](https://www.youtube.com/watch?v=yClSNQdVD7g) |
| Asynkron programmering | [C# Async / Await – IAmTimCorey](https://www.youtube.com/watch?v=2moh18sh5p4) |
| HTTP och API i .NET | [Make HTTP requests in a .NET console app – Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/csharp/tutorials/console-webapiclient) |
| API-anrop i C# | [How To Call An API in C# – IAmTimCorey](https://www.youtube.com/watch?v=aWePkE2ReGw) |
