using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using Ryujinx.Ava.Common;
using Ryujinx.Common;
using Ryujinx.Common.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace Ryujinx.Ava.UI.Helpers
{
    internal static class MatugenThemeLoader
    {
        private static readonly string[] RequiredColors =
        {
            "SystemAccentColor",
            "SystemAccentColorDark1",
            "SystemAccentColorDark2",
            "SystemAccentColorDark3",
            "SystemAccentColorLight1",
            "SystemAccentColorLight2",
            "SystemAccentColorLight3",
            "ThemeAccentColor4",
            "DataGridSelectionColor",
            "ThemeContentBackgroundColor",
            "ThemeBackgroundColor",
            "ThemeControlBorderColor",
            "TextOnAccentFillColorPrimary",
            "SystemChromeWhiteColor",
            "ThemeForegroundColor",
            "MenuFlyoutPresenterBorderColor",
            "AppListBackgroundColor",
            "AppListHoverBackgroundColor",
            "SecondaryTextColor",
            "VsyncEnabled",
            "VsyncDisabled",
            "MenuBarBackgroundColor",
            "PanelBackgroundColor",
            "ListBoxItemSelectedBackgroundColor",
            "ListBoxItemHoverBackgroundColor",
            "ButtonBackgroundColor",
            "ButtonHoverBackgroundColor",
            "ButtonPressedBackgroundColor",
            "ButtonBorderColor",
            "ThemeDarkColor",
            "ThemeButtonForegroundColor",
            "ThemeBorderLowColor",
            "SystemListLowColor",
            "SystemBaseMediumLowColor",
            "HighlightBrush",
        };

        private static FileSystemWatcher _watcher;
        private static Timer _reloadTimer;
        private static string _path;

        public static bool Configure(string path)
        {
            StopWatching();
            _path = path;

            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            bool loaded = Load(path);

            string directory = Path.GetDirectoryName(path);

            if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
            {
                return loaded;
            }

            _watcher = new FileSystemWatcher(directory, Path.GetFileName(path))
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName,
                EnableRaisingEvents = true,
            };
            _watcher.Changed += FileChanged;
            _watcher.Created += FileChanged;
            _watcher.Renamed += FileChanged;

            return loaded;
        }

        public static void StopWatching()
        {
            _watcher?.Dispose();
            _watcher = null;
            _reloadTimer?.Dispose();
            _reloadTimer = null;
        }

        private static void FileChanged(object sender, FileSystemEventArgs e)
        {
            _reloadTimer?.Dispose();
            _reloadTimer = new Timer(_ => Dispatcher.UIThread.Post(() => Load(_path)), null, 150, Timeout.Infinite);
        }

        private static bool Load(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException("Matugen palette was not found.", path);
                }

                using FileStream stream = new(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XDocument document = XDocument.Load(stream, LoadOptions.PreserveWhitespace);
                XNamespace x = "http://schemas.microsoft.com/winfx/2006/xaml";
                Dictionary<string, Color> colors = document.Root?
                    .Elements()
                    .Where(element => element.Name.LocalName == "Color" && element.Attribute(x + "Key") != null)
                    .ToDictionary(element => element.Attribute(x + "Key").Value, element => Color.Parse(element.Value.Trim()))
                    ?? throw new InvalidDataException("The Matugen palette has no resource dictionary root.");

                string missing = RequiredColors.FirstOrDefault(key => !colors.ContainsKey(key));

                if (missing != null)
                {
                    throw new InvalidDataException($"The Matugen palette is missing the '{missing}' resource.");
                }

                var resources = new ResourceDictionary();

                foreach ((string key, Color color) in colors)
                {
                    resources[key] = color;
                }

                resources["DataGridSelectionBackgroundBrush"] = new SolidColorBrush(colors["DataGridSelectionColor"]);
                resources["ThemeAccentColorBrush"] = new SolidColorBrush(colors["SystemAccentColor"]);
                resources["ThemeAccentBrush4"] = new SolidColorBrush(colors["ThemeAccentColor4"]);

                Application.Current.Resources.ThemeDictionaries[App.MatugenTheme] = resources;
                ThemeManager.OnThemeChanged();
                return true;
            }
            catch (Exception exception)
            {
                Logger.Warning?.Print(LogClass.Application, $"Failed to load Matugen theme '{path}': {exception.Message}");
                return false;
            }
        }
    }
}
