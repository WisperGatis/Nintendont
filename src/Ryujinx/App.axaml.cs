using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.Styling;
using Avalonia.Threading;
using Ryujinx.Ava.Common;
using Ryujinx.Ava.Common.Locale;
using Ryujinx.Ava.UI.Helpers;
using Ryujinx.Ava.UI.Windows;
using Ryujinx.Common;
using Ryujinx.Common.Logging;
using Ryujinx.UI.Common.Configuration;
using Ryujinx.UI.Common.Helper;
using System;
using System.Diagnostics;

namespace Ryujinx.Ava
{
    public class App : Application
    {
        public static readonly ThemeVariant PurpleTheme = new("Purple", ThemeVariant.Dark);
        public static readonly ThemeVariant CatppuccinLatteTheme = new("CatppuccinLatte", ThemeVariant.Light);
        public static readonly ThemeVariant CatppuccinFrappeTheme = new("CatppuccinFrappe", ThemeVariant.Dark);
        public static readonly ThemeVariant CatppuccinMacchiatoTheme = new("CatppuccinMacchiato", ThemeVariant.Dark);
        public static readonly ThemeVariant CatppuccinMochaTheme = new("CatppuccinMocha", ThemeVariant.Dark);
        public static readonly ThemeVariant RosePineTheme = new("RosePine", ThemeVariant.Dark);
        public static readonly ThemeVariant RosePineDawnTheme = new("RosePineDawn", ThemeVariant.Light);
        public static readonly ThemeVariant GruvboxTheme = new("Gruvbox", ThemeVariant.Dark);
        public static readonly ThemeVariant GruvboxLightTheme = new("GruvboxLight", ThemeVariant.Light);
        public static readonly ThemeVariant RosePineMoonTheme = new("RosePineMoon", ThemeVariant.Dark);
        public static readonly ThemeVariant TokyoNightTheme = new("TokyoNight", ThemeVariant.Dark);
        public static readonly ThemeVariant TokyoNightDayTheme = new("TokyoNightDay", ThemeVariant.Light);
        public static readonly ThemeVariant NordTheme = new("Nord", ThemeVariant.Dark);
        public static readonly ThemeVariant DraculaTheme = new("Dracula", ThemeVariant.Dark);
        public static readonly ThemeVariant EverforestTheme = new("Everforest", ThemeVariant.Dark);
        public static readonly ThemeVariant EverforestLightTheme = new("EverforestLight", ThemeVariant.Light);
        public static readonly ThemeVariant KanagawaTheme = new("Kanagawa", ThemeVariant.Dark);
        public static readonly ThemeVariant OneDarkTheme = new("OneDark", ThemeVariant.Dark);
        public static readonly ThemeVariant SolarizedDarkTheme = new("SolarizedDark", ThemeVariant.Dark);
        public static readonly ThemeVariant SolarizedLightTheme = new("SolarizedLight", ThemeVariant.Light);
        public static readonly ThemeVariant MatugenTheme = new("Matugen", ThemeVariant.Dark);

        public override void Initialize()
        {
            Name = $"Ryujinx {Program.Version}";

            AvaloniaXamlLoader.Load(this);

            if (OperatingSystem.IsMacOS())
            {
                Process.Start("/usr/bin/defaults", "write org.ryujinx.Ryujinx ApplePressAndHoldEnabled -bool false");
            }
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();

            if (Program.PreviewerDetached)
            {
                ApplyConfiguredTheme();

                ConfigurationState.Instance.UI.BaseStyle.Event += ThemeChanged_Event;
                ConfigurationState.Instance.UI.CustomThemePath.Event += ThemeChanged_Event;
                ConfigurationState.Instance.UI.MatugenThemePath.Event += MatugenThemePathChanged_Event;
                ConfigurationState.Instance.UI.EnableCustomTheme.Event += CustomThemeChanged_Event;
            }
        }

        private void CustomThemeChanged_Event(object sender, ReactiveEventArgs<bool> e)
        {
            ApplyConfiguredTheme();
        }

        private void ShowRestartDialog()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    var result = await ContentDialogHelper.CreateConfirmationDialog(
                        LocaleManager.Instance[LocaleKeys.DialogThemeRestartMessage],
                        LocaleManager.Instance[LocaleKeys.DialogThemeRestartSubMessage],
                        LocaleManager.Instance[LocaleKeys.InputDialogYes],
                        LocaleManager.Instance[LocaleKeys.InputDialogNo],
                        LocaleManager.Instance[LocaleKeys.DialogRestartRequiredMessage]);

                    if (result == UserResult.Yes)
                    {
                        var path = Environment.ProcessPath;
                        var proc = Process.Start(path, CommandLineState.Arguments);
                        desktop.Shutdown();
                        Environment.Exit(0);
                    }
                }
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private void ThemeChanged_Event(object sender, ReactiveEventArgs<string> e)
        {
            ApplyConfiguredTheme();
        }

        private void MatugenThemePathChanged_Event(object sender, ReactiveEventArgs<string> e)
        {
            if (ConfigurationState.Instance.UI.BaseStyle == ThemeRegistry.MatugenId)
            {
                MatugenThemeLoader.Configure(e.NewValue);
            }
        }

        public void ApplyConfiguredTheme()
        {
            try
            {
                string baseStyle = ConfigurationState.Instance.UI.BaseStyle;

                if (string.IsNullOrWhiteSpace(baseStyle))
                {
                    ConfigurationState.Instance.UI.BaseStyle.Value = "Auto";

                    baseStyle = ConfigurationState.Instance.UI.BaseStyle;
                }

                ThemeVariant systemTheme = DetectSystemTheme();

                ThemeManager.OnThemeChanged();

                RequestedThemeVariant = ThemeRegistry.Find(baseStyle) is { } theme
                    ? theme.Variant ?? systemTheme
                    : ThemeVariant.Default;

                if (baseStyle == ThemeRegistry.MatugenId)
                {
                    if (!MatugenThemeLoader.Configure(ConfigurationState.Instance.UI.MatugenThemePath))
                    {
                        RequestedThemeVariant = systemTheme;
                    }
                }
                else
                {
                    MatugenThemeLoader.StopWatching();
                }
            }
            catch (Exception)
            {
                Logger.Warning?.Print(LogClass.Application, "Failed to Apply Theme. A restart is needed to apply the selected theme");

                ShowRestartDialog();
            }
        }

        /// <summary>
        /// Converts a PlatformThemeVariant value to the corresponding ThemeVariant value.
        /// </summary>
        public static ThemeVariant ConvertThemeVariant(PlatformThemeVariant platformThemeVariant) =>
            platformThemeVariant switch
            {
                PlatformThemeVariant.Dark => ThemeVariant.Dark,
                PlatformThemeVariant.Light => ThemeVariant.Light,
                _ => ThemeVariant.Default,
            };

        internal static ThemeVariant DetectSystemTheme()
        {
            if (Application.Current is App app)
            {
                var colorValues = app.PlatformSettings.GetColorValues();

                return ConvertThemeVariant(colorValues.ThemeVariant);
            }

            return ThemeVariant.Default;
        }
    }
}
