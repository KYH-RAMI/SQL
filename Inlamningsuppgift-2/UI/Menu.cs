using System;
using System.Collections.Generic;
using System.Text;
using FailSafe_WeatherMonitor.Models;
using FailSafe_WeatherMonitor.UI;
namespace FailSafe_WeatherMonitor.UI
{
    internal static class Menu
    {
        private static readonly string[] MenuOptions =
        {
        "View Weather Data",
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
                        case "View Weather Data":
                            await WeatherScreen.ShowAsync();
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
    }
}