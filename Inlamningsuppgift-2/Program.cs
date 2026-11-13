using FailSafe_WeatherMonitor.UI;
using Spectre.Console;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json;
Console.Title = "FailSafe WeatherMonitor | Rami Issa";
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;



await Menu.ShowAsync();
