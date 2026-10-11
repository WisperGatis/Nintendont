# Themes

Built-in themes are registered in `src/Ryujinx/UI/Helpers/ThemeRegistry.cs` and
their colors live in `src/Ryujinx/Assets/Styles/Themes.xaml`.

Available: Auto, Light, Dark, Purple, Catppuccin (Latte, Frappé, Macchiato,
Mocha), Rosé Pine (Main, Moon, Dawn), Gruvbox (Dark, Light), Tokyo Night
(Night, Day), Nord, Dracula, Everforest (Dark, Light), Kanagawa, One Dark,
Solarized (Dark, Light), and Matugen (see [matugen.md](matugen.md)).

## Adding a theme

1. Add a `ThemeVariant` in `App.axaml.cs` (inherit `Light` or `Dark`).
2. Add a `ResourceDictionary` keyed `{x:Static ava:App.<Name>Theme}` to
   `Themes.xaml`, defining the same keys as an existing block.
3. Add an entry to `ThemeRegistry.Themes`. The `Id` is what is saved in the
   config, so never rename it.

## Base16 slot mapping

| Base16 slot | Resource |
| --- | --- |
| base00 | `ThemeBackgroundColor`, `PanelBackgroundColor`, `MenuBarBackgroundColor` |
| base01 | `ThemeContentBackgroundColor`, hover background |
| base02 | `DataGridSelectionColor`, selected list item |
| base03 | `ThemeControlBorderColor`, `ThemeAccentColor4` |
| base04 | `SecondaryTextColor` (with alpha) |
| base05 | `ThemeForegroundColor` |
| base08 | `VsyncDisabled`, pressed button |
| base09 / 0A | hover / normal button |
| base0B | `VsyncEnabled` |
| base0D | `SystemAccentColor`, `HighlightBrush` |
