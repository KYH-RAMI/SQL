using System;
using System.Collections.Generic;
using System.Text;

namespace FailSafe_WeatherMonitor.Services
{
    using System;

    internal static class Menu
    {
        private static readonly string[] MenuOptions =
        {
        "View Weather Data",
        "Exit"
    };

        public static void Show()
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
                            
                            AppTheme.ShowNotice(
                                "WEATHER DATA",
                                "This feature is under development.");
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