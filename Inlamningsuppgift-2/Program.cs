using FailSafe_WeatherMonitor.Data;
using FailSafe_WeatherMonitor.UI;
using Microsoft.EntityFrameworkCore;
using System.Text;

Console.Title = "FailSafe WeatherMonitor | Rami Issa";
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

// Säkerställ att databasen har aktuella migreringar
await using(var db = new WeatherDataContext())
{
    await db.Database.MigrateAsync();
}

// Starta huvudmenyn
await Menu.ShowAsync();
