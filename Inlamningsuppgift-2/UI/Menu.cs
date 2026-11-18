using FailSafe_WeatherMonitor.Services;
using System.Text.Json;

namespace FailSafe_WeatherMonitor.UI
{
    internal static class Menu
    {
        private static readonly string[] MenuOptions =
        {
            "Import Weather Data",
            "View Weather Data",
            "Weather Data Statistics",
            "Exit"
    };

        public static async Task ShowAsync()
        {
            int selectedIndex = 0;

            try
            {
                while(true)
                {
                    string selectedOption = AppTheme.ShowMenu(
                        title: "FailSafe WeatherMonitor",
                        subtitle: "MAIN MENU",
                        options: MenuOptions,
                        selectedIndex: ref selectedIndex,
                        escapeOption: "Exit",
                        description: "WEATHER MONITORING SYSTEM",
                        author: "Author: Rami Issa");

                    switch(selectedOption)
                    {
                        case "Import Weather Data":
                            await ImportWeatherDataAsync();
                            break;

                        case "View Weather Data":
                            await WeatherScreen.ShowAsync();
                            break;

                        case "Weather Data Statistics":
                              await WeatherStatisticsScreen.ShowAsync();
                              break;

                        case "Exit":
                            return;
                    }
                }
            }
            finally
            {
                AppTheme.RestoreScreen();
            }
        }
        
        private static async Task ImportWeatherDataAsync()
        {
            string filePath = Path.Combine(
                AppContext.BaseDirectory,
                "Data",
                "weather_data.json");

            try
            {
                ImportService importService = new();

                ImportResult result = await importService.ImportAsync(filePath);

                AppTheme.ShowNotice(
                    "IMPORT COMPLETED",
                    $"Imported: {result.Imported}\n" +
                    $"Rejected: {result.Rejected}\n" +
                    $"Duplicates: {result.Duplicates}",
                    NoticeType.Success);
            }
            catch(JsonException ex)
            {
                string line = ex.LineNumber.HasValue
                    ? (ex.LineNumber.Value + 1).ToString()
                    : "Unknown";

                string position = ex.BytePositionInLine.HasValue
                    ? (ex.BytePositionInLine.Value + 1).ToString()
                    : "Unknown";

                AppTheme.ShowNotice(
                    "JSON SYNTAX ERROR",
                    "The JSON file contains invalid syntax.\n" +
                    $"Line: {line}\n" +
                    $"Byte position: {position}\n" +
                    "Please check and correct the JSON file.",
                    NoticeType.Error);
            }
            catch(FileNotFoundException)
            {
                AppTheme.ShowNotice(
                    "FILE NOT FOUND",
                    "The weather data file could not be found.\n" +
                    "Please check the Data folder.",
                    NoticeType.Error);
            }
            catch(IOException)
            {
                AppTheme.ShowNotice(
                    "FILE ERROR",
                    "The JSON file could not be read.\n" +
                    "Please check file access and try again.",
                    NoticeType.Error);
            }
        }
    }
}