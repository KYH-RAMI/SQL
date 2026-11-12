using Spectre.Console;
using Spectre.Console.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace FailSafe_WeatherMonitor.Services
{

    internal static class AppTheme
    {

        public const string Background = "#0B1725";

        public const string SkyLight = "#D6F3FF";
        public const string Sky = "#64B9F4";
        public const string SkyDark = "#2E7CB7";

        public const string Accent = Sky;
        public const string Text = "#E9F4FB";
        public const string Muted = "#95AEC1";
        public const string Border = "#426E8E";
        public const string SelectionText = "#071321";
        public const int FormWidth = 62;
        private const int PanelWidth = FormWidth;

        private const int PanelHeight = 22;
        private const int OptionWidth = 26;
        public static readonly Color BorderColor =
            new(66, 110, 142);
        public static readonly Color SuccessBorderColor = Color.Green;

        public static void PrepareScreen()
        {
            Console.CursorVisible = false;

            Console.Write("\u001b]11;rgb:0b/17/25\u001b\\");
            Console.Write("\u001b[0m");

            AnsiConsole.Clear();
        }

        public static void RestoreScreen()
        {
            Console.Write("\u001b]111\u001b\\");
            Console.Write("\u001b[0m");

            Console.CursorVisible = true;
        }
        public static string GradientMarkup(
            string light,
            string middle,
            string dark)
        {
            return
                $"[bold {SkyLight}]{Markup.Escape(light)}[/]" +
                $"[bold {Sky}]{Markup.Escape(middle)}[/]" +
                $"[bold {SkyDark}]{Markup.Escape(dark)}[/]";
        }
        public static void WriteText(
            int left,
            int top,
            string value,
            string foreground,
            string? background = null,
            bool bold = false)
        {
            Console.SetCursorPosition(left, top);

            string style = bold ? "bold " : "";

            AnsiConsole.Markup(
                $"[{style}{foreground} on {background ?? Background}]" +
                $"{Markup.Escape(value)}[/]");
        }

        public static void WriteCentered(
            int left,
            int top,
            int width,
            string text,
            string color,
            bool bold = false)
        {
            int textLeft =
                left + ((width - text.Length) / 2);

            WriteText(textLeft, top, text, color, bold: bold);
        }

        public static void WriteGradientCentered(
            int left,
            int top,
            int width,
            string text)
        {
            string[] words = text.Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries);

            int position =
                left + ((width - text.Length) / 2);

            for(int i = 0; i < words.Length; i++)
            {
                string color = i switch
                {
                    0 when words.Length == 1 => Sky,

                    0 => SkyLight,

                    _ when i == words.Length - 1 => SkyDark,

                    _ => Sky
                };

                WriteText(
                    position,
                    top,
                    words[i],
                    color,
                    bold: true);

                position += words[i].Length + 1;
            }
        }

        public static void DrawFrame(
            int left,
            int top,
            int width,
            int height)
        {
            WriteText(
                left,
                top,
                "╭" + new string('─', width - 2) + "╮",
                Border);

            for(int row = 1; row < height - 1; row++)
            {
                WriteText(left, top + row, "│", Border);

                WriteText(
                    left + width - 1,
                    top + row,
                    "│",
                    Border);
            }

            WriteText(
                left,
                top + height - 1,
                "╰" + new string('─', width - 2) + "╯",
                Border);
        }

        public static void DrawDivider(
            int left,
            int top,
            int width)
        {
            const string segment = "━━━━━━";

            int position =
                left + ((width - (segment.Length * 3)) / 2);

            WriteText(position, top, segment, SkyLight);

            WriteText(
                position + segment.Length,
                top,
                segment,
                Sky);

            WriteText(
                position + (segment.Length * 2),
                top,
                segment,
                SkyDark);
        }

        private static void DrawMenuOption(
            int left,
            int top,
            string option,
            bool isSelected)
        {
            string text = isSelected
                ? $" ❯ {option}"
                : $"   {option}";

            text = text.PadRight(OptionWidth);

            WriteText(
                left,
                top,
                text,
                isSelected ? SelectionText : Text,
                isSelected ? Accent : Background,
                isSelected);
        }

        public static string ShowMenu(
            string title,
            string subtitle,
            IReadOnlyList<string> options,
            ref int selectedIndex,
            string escapeOption,
            string? description = null,
            string? author = null,
            string? hint = null)
        {
            if(options.Count == 0)
            {
                throw new ArgumentException(
                    "The menu must contain at least one option.",
                    nameof(options));
            }

            selectedIndex = Math.Clamp(
                selectedIndex,
                0,
                options.Count - 1);

            // Anpassa höjden efter antalet menyalternativ.
            int menuHeight = PanelHeight +
                Math.Max(0, options.Count - 7) * 3;

            PrepareScreen();

            if(Console.WindowWidth < PanelWidth + 2 ||
                Console.WindowHeight < menuHeight + 2)
            {
                return AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"[bold {Sky}]{Markup.Escape(title)}[/]")
                        .PageSize(Math.Min(options.Count, 7))
                        .AddChoices(options));
            }

            int left =
                (Console.WindowWidth - PanelWidth) / 2;

            int top =
                (Console.WindowHeight - menuHeight) / 2;

            bool hasAuthor =
                !string.IsNullOrWhiteSpace(author);

            int firstOptionRow = hasAuthor ? 11 : 10;
            int optionSpacing = !hasAuthor && options.Count <= 3 ? 2 : 1;

            DrawFrame(left, top, PanelWidth, menuHeight);

            WriteGradientCentered(
                left,
                top + 2,
                PanelWidth,
                title);

            WriteCentered(
                left,
                top + (hasAuthor ? 3 : 4),
                PanelWidth,
                description ?? "",
                Text);

            DrawDivider(
                left,
                top + (hasAuthor ? 5 : 6),
                PanelWidth);

            if(hasAuthor)
            {
                WriteGradientCentered(
                    left,
                    top + 7,
                    PanelWidth,
                    author!);
            }

            WriteCentered(
                left,
                top + (hasAuthor ? 9 : 8),
                PanelWidth,
                subtitle,
                Sky,
                true);

            if(!hasAuthor && options.Count <= 7 && !string.IsNullOrWhiteSpace(hint))
            {
                WriteCentered(
                    left,
                    top + 17,
                    PanelWidth,
                    hint,
                    Muted);
            }

            WriteCentered(
                left,
                top + menuHeight - 3,
                PanelWidth,
                $"↑ ↓ Navigate   |   Enter Select   |   Esc {escapeOption}",
                Muted);

            int menuLeft =
                left + ((PanelWidth - OptionWidth) / 2);

            for(int i = 0; i < options.Count; i++)
            {
                DrawMenuOption(
                    menuLeft,
                    top + firstOptionRow + (i * optionSpacing),
                    options[i],
                    i == selectedIndex);
            }

            while(true)
            {
                ConsoleKey key =
                    Console.ReadKey(intercept: true).Key;

                if(key == ConsoleKey.Enter)
                {
                    return options[selectedIndex];
                }

                if(key == ConsoleKey.Escape)
                {
                    return escapeOption;
                }

                int nextIndex = key switch
                {
                    ConsoleKey.UpArrow =>
                        (selectedIndex - 1 + options.Count)
                        % options.Count,

                    ConsoleKey.DownArrow =>
                        (selectedIndex + 1)
                        % options.Count,

                    _ => selectedIndex
                };

                if(nextIndex == selectedIndex)
                {
                    continue;
                }

                DrawMenuOption(
                    menuLeft,
                    top + firstOptionRow + (selectedIndex * optionSpacing),
                    options[selectedIndex],
                    false);

                selectedIndex = nextIndex;

                DrawMenuOption(
                    menuLeft,
                    top + firstOptionRow + (selectedIndex * optionSpacing),
                    options[selectedIndex],
                    true);
            }
        }

        public static void ShowNotice(
            string title,
            string message)
        {
            PrepareScreen();

            if(Console.WindowWidth < PanelWidth + 2 ||
                Console.WindowHeight < PanelHeight + 2)
            {
                AnsiConsole.MarkupLine(
                    $"[bold {Sky}]{Markup.Escape(title)}[/]");

                AnsiConsole.MarkupLine(
                    $"\n[{Text}]{Markup.Escape(message)}[/]");

                AnsiConsole.MarkupLine(
                    $"\n[{Muted}]Press any key to continue...[/]");

                _ = Console.ReadKey(intercept: true);
                return;
            }

            int left =
                (Console.WindowWidth - PanelWidth) / 2;

            int top =
                (Console.WindowHeight - PanelHeight) / 2;

            DrawFrame(left, top, PanelWidth, PanelHeight);

            WriteGradientCentered(
                left,
                top + 2,
                PanelWidth,
                title);

            DrawDivider(left, top + 5, PanelWidth);

            WriteCentered(
                left,
                top + 9,
                PanelWidth,
                "SYSTEM MESSAGE",
                Sky,
                true);

            WriteCentered(
                left,
                top + 11,
                PanelWidth,
                message,
                Text);

            WriteCentered(
                left,
                top + 19,
                PanelWidth,
                "Press any key to return to Main Menu",
                Muted);

            _ = Console.ReadKey(intercept: true);
        }
        public static Panel CreatePanel(
            IRenderable content,
            string? title = null,
            int? width = null,
            Color? borderColor = null,
            Padding? padding = null)
        {
            var panel = new Panel(content)
            {
                Border = BoxBorder.Rounded,

                BorderStyle = new Style(
                    foreground: borderColor ?? BorderColor),

                Padding = padding ?? new Padding(1, 0, 1, 0)
            };

            if(!string.IsNullOrWhiteSpace(title))
            {
                panel.Header = new PanelHeader(
                    $"[bold {Sky}] {Markup.Escape(title)} [/]");
            }

            if(width.HasValue)
            {
                panel.Width = width.Value;
            }

            return panel;
        }
        public static void WriteCenteredPanel(Panel panel)
        {
            ArgumentNullException.ThrowIfNull(panel);

            AnsiConsole.Write(
                Align.Center(panel));
        }
    }
}