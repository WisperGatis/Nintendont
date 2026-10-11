using Avalonia.Styling;
using Ryujinx.Ava.Common.Locale;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ryujinx.Ava.UI.Helpers
{
    internal static class ThemeRegistry
    {
        public const string AutoId = "Auto";
        public const string MatugenId = "Matugen";

        public sealed record ThemeEntry(string Id, string DisplayName, LocaleKeys? LocaleKey, ThemeVariant Variant)
        {
            public string Label => LocaleKey is { } key ? LocaleManager.Instance[key] : DisplayName;
        }

        // The order here is the order shown in the settings combo box.
        public static readonly IReadOnlyList<ThemeEntry> Themes = new List<ThemeEntry>
        {
            new(AutoId, "Auto", LocaleKeys.SettingsTabGeneralThemeAuto, null),
            new("Light", "Light", LocaleKeys.SettingsTabGeneralThemeLight, ThemeVariant.Light),
            new("Dark", "Dark", LocaleKeys.SettingsTabGeneralThemeDark, ThemeVariant.Dark),
            new("Purple", "Purple", LocaleKeys.SettingsTabGeneralThemePurple, App.PurpleTheme),
            new("CatppuccinLatte", "Catppuccin Latte", null, App.CatppuccinLatteTheme),
            new("CatppuccinFrappe", "Catppuccin Frappé", null, App.CatppuccinFrappeTheme),
            new("CatppuccinMacchiato", "Catppuccin Macchiato", null, App.CatppuccinMacchiatoTheme),
            new("CatppuccinMocha", "Catppuccin Mocha", null, App.CatppuccinMochaTheme),
            new("RosePine", "Rosé Pine", null, App.RosePineTheme),
            new("RosePineMoon", "Rosé Pine Moon", null, App.RosePineMoonTheme),
            new("RosePineDawn", "Rosé Pine Dawn", null, App.RosePineDawnTheme),
            new("Gruvbox", "Gruvbox", null, App.GruvboxTheme),
            new("GruvboxLight", "Gruvbox Light", null, App.GruvboxLightTheme),
            new("TokyoNight", "Tokyo Night", null, App.TokyoNightTheme),
            new("TokyoNightDay", "Tokyo Night Day", null, App.TokyoNightDayTheme),
            new("Nord", "Nord", null, App.NordTheme),
            new("Dracula", "Dracula", null, App.DraculaTheme),
            new("Everforest", "Everforest", null, App.EverforestTheme),
            new("EverforestLight", "Everforest Light", null, App.EverforestLightTheme),
            new("Kanagawa", "Kanagawa", null, App.KanagawaTheme),
            new("OneDark", "One Dark", null, App.OneDarkTheme),
            new("SolarizedDark", "Solarized Dark", null, App.SolarizedDarkTheme),
            new("SolarizedLight", "Solarized Light", null, App.SolarizedLightTheme),
            new(MatugenId, "Matugen", null, App.MatugenTheme),
        };

        public static ThemeEntry Find(string id) =>
            Themes.FirstOrDefault(theme => string.Equals(theme.Id, id, StringComparison.Ordinal));

        public static int IndexOf(string id)
        {
            for (int i = 0; i < Themes.Count; i++)
            {
                if (string.Equals(Themes[i].Id, id, StringComparison.Ordinal))
                {
                    return i;
                }
            }

            return 0;
        }

        public static string IdAt(int index) =>
            index >= 0 && index < Themes.Count ? Themes[index].Id : AutoId;

        public static bool IsDark(string id, ThemeVariant systemTheme)
        {
            ThemeVariant variant = Find(id)?.Variant ?? systemTheme;

            return variant == ThemeVariant.Dark || variant.InheritVariant == ThemeVariant.Dark;
        }
    }
}
