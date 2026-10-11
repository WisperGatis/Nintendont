# Workstream D: UI stack upgrade (Avalonia 12, SkiaSharp 3, FluentAvaloniaUI 3)

Repo: /home/wisp/nintendont (Ryujinx, .NET 10). Workstreams A (NUnit 4) and B (Roslyn 5.9) are merged on main (`0143df0`). This file supersedes the D section of `docs/workflow/major-dependency-upgrade-plan.md`. Copy it there when execution starts.

## Context
The UI stack is held at Avalonia 11.3.22, SkiaSharp 2.88.9 and FluentAvaloniaUI 2.0.5. The user approved the major jumps. Research (NuGet nuspecs, Avalonia 11→12 breaking-changes guide) changed the earlier assumptions:

| Finding | Consequence |
|---|---|
| `Avalonia.Skia 12.1.2` depends on **SkiaSharp 3.119.4** | Target SkiaSharp **3.119.4**, not 4.x. 4.x would conflict with Avalonia. |
| `FluentAvaloniaUI 3.1.0` needs Avalonia **12.1.0+** (net10.0 only) | Fluent and Avalonia must move together in one step. |
| `Avalonia.Svg` / `Avalonia.Svg.Skia` latest is **11.3.0** (needs Avalonia 11.3 and SkiaSharp 3.116.1). There is **no Avalonia 12 release**. | Blocker to solve (see step 3). |
| `Avalonia.Diagnostics` is removed in 12. The replacement is a paid "Avalonia Plus" dev tools package. | It is only a Debug reference in `Ryujinx.csproj:40`, and no code calls `AttachDevTools`. Drop it. |
| `UseSkia()` in Avalonia 12 must be paired with `UseHarfBuzz()` (`Avalonia.HarfBuzz`) | Edit `src/Ryujinx/Program.cs:76`. `Avalonia.Desktop` 12 already pulls in the package. |
| Compiled bindings are the default in 12 | Already set (`AvaloniaUseCompiledBindingsByDefault`), so low impact. |
| Only about 4 source lines hit listed 12 breaking APIs (focus events, `AccessKey`, gestures, clipboard) | The API port is small. Risk is concentrated in FluentAvalonia, Svg and the renderer. |

**Possible existing problem on main.** `Avalonia.Svg.Skia 11.3.0` wants SkiaSharp 3.116.1, while HLE pins SkiaSharp 2.88.9. The build passes, but the GUI has not been launched since the package bump. Step 0 checks this.

## Ground rules
- One agent, one branch `upgrade/ui-stack`, one commit per step below. Do not mix steps. Stop and report if a step is blocked. Do not hack around it.
- Baseline: `Ryujinx.Tests` 30,545 passed / 133 skipped, `Ryujinx.Tests.Memory` 84 passed.
- Git has no identity configured. Commit with `git -c user.name=WisperGatis -c user.email=wesley.david09@gmail.com` and end messages with `Co-Authored-By: Claude Sonnet 5 <noreply@anthropic.com>`. Do not push.
- Central versions live in `Directory.Packages.props`. Follow `CLAUDE.md` and the coding-style docs.
- Do not edit the user's Nix config. Collect any missing native libs into a list for the user.

## Steps

### 0. Baseline smoke test on main (before any change)
- Run `./scripts/run-debug.sh`. The main window must open. Check the log for Skia/native load errors (the Svg.Skia/SkiaSharp 2.88 vs 3.116 mismatch).
- If it crashes, fix by moving to step 1 first (SkiaSharp 3.x) and note it in the report.
- Record the smoke checklist results (see Verification).

### 1. SkiaSharp 2.88.9 → 3.119.4 (with Avalonia still on 11.3)
- Avalonia 11.3 supports SkiaSharp 3.x. Bump `SkiaSharp` and `SkiaSharp.NativeAssets.Linux` in `Directory.Packages.props`. Use 3.119.4, which is what Avalonia 12 will need.
- Consumers: `src/Ryujinx.HLE/HOS/Applets/SoftwareKeyboard/SoftwareKeyboardRendererBase.cs`, `HOS/Services/Caps/CaptureManager.cs`, `src/Ryujinx.UI.Common/Helper/ShortcutHelper.cs`, `src/Ryujinx/UI/Views/User/UserProfileImageSelectorView.axaml.cs`, `src/Ryujinx/AppHost.cs`, `src/Ryujinx.Gtk3/UI/Windows/*`.
- Known API break: `SKFilterQuality` is removed, so `bitmap.Resize(info, SKFilterQuality.High)` (4 call sites) becomes `Resize(info, new SKSamplingOptions(SKCubicResampler.Mitchell))` (or `SKFilterMode.Linear` plus `SKMipmapMode.Linear` to keep the look). Also check `SKPaint.TextSize`/`Typeface` (moved to `SKFont`), `SKBitmap.Decode` overloads and `SKImage.Encode` (in `Ryujinx.HLE`).
- Confirm the on-screen software keyboard and screenshot capture still render the same. Keep a before/after image if a game is available.

### 2. Avalonia 11.3 → 12.1.2 **and** FluentAvaloniaUI 2.0.5 → 3.1.0 (one commit, because Fluent 3 requires Avalonia 12)
- Read the Avalonia 11→12 guide (https://docs.avaloniaui.net/docs/avalonia12-breaking-changes) and the FluentAvalonia 3 release notes before editing.
- `Directory.Packages.props`: `Avalonia`, `.Desktop`, `.Controls.DataGrid`, `.Markup.Xaml.Loader` → 12.1.2 (DataGrid 12.1.0 or newer). `FluentAvaloniaUI` → 3.1.0.
- `src/Ryujinx/Ryujinx.csproj`: remove the `Avalonia.Diagnostics` reference (line 40). Re-check the FluentAvalonia System.Text.Json workaround comment (lines about issue 481) and remove it if Fluent 3 fixes it.
- `src/Ryujinx/Program.cs`: `.UseSkia()` → `.UseSkia().UseHarfBuzz()`. Check that `Avalonia.Desktop` brings `Avalonia.HarfBuzz`.
- FluentAvalonia is used in 45 files (`ContentDialog`, `NavigationView`, `FAIcon`/`IconSource`, `FluentAvaloniaTheme` in `App.axaml:14`). Fix compile errors file by file. Expect renamed types and styles, and check `Assets/Styles/Themes.xaml` and `Styles.xaml` (the custom themes from the recent commit).
- Fix the 12-specific spots: focus event arg types (`GotFocus`/`LostFocus`), `Gestures.*` in XAML, `AccessText.AccessKey` (`char`→`string`), `TopLevel` no longer being the visual root (`TopLevel.GetTopLevel(visual)`), clipboard/drag-drop (`IDataObject` removed) and `ExtendClientAreaChromeHints`/title bar APIs if used.
- Highest-risk file: `src/Ryujinx/UI/Renderer/EmbeddedWindow.cs`, the only `NativeControlHost` user. It hosts the native GL/Vulkan surface via X11/Win32/macOS handles. Verify the X11 path (this machine is Wayland/XWayland) plus focus and input passthrough.
- Svg: because the Svg packages have no 12 release, they must be handled in this same commit (step 3). The build cannot pass with both.

### 3. SVG rendering without Avalonia.Svg.Skia
Used in 4 files: `src/Ryujinx/UI/ViewModels/Input/{ControllerInputViewModel,InputViewModel,KeyboardInputViewModel}.cs` and `UI/Applet/ControllerAppletDialog.axaml.cs` (controller/keyboard SVG artwork). Decide in this order:
1. First check whether a newer `Avalonia.Svg.Skia` or `Svg.Skia` (5.2.3) exposes an Avalonia 12 compatible control. If yes, use it.
2. Otherwise depend on `Svg.Skia` 5.2.3 directly (framework-agnostic, needs SkiaSharp 3) and add a thin local `SvgImage`/`Image` source in `src/Ryujinx/UI/Helpers` that renders `SKPicture` into an Avalonia `DrawingImage` or bitmap.
3. Fallback: pre-rasterize the SVGs to PNG assets.
Report which option was used. Do not leave the input configuration screens without artwork.

### 4. DynamicData
`DynamicData` 9.4.33 is already in place. Verify it works with Avalonia 12. It is used in 5 files (`ReactiveUI`-style collections in view models). Only change it if the build or runtime breaks.

### 5. Tmds.DBus.Protocol (workstream C)
- `src/Ryujinx/Ryujinx.csproj:57-58` pins Avalonia's transitive DBus dependency **to a patched release on purpose**. Do not lower it. Check what `Avalonia.FreeDesktop 12.1.2` requires. Bump to that or newer (0.95.1 is latest). Keep the pin comment accurate.
- Used in 1 file. Build and smoke test the tray/notifications/DBus paths on Linux.

## Verification (after every step, and at the end)
1. `dotnet build Ryujinx.sln -c Release` has 0 errors and no `NU16xx` warnings.
2. `dotnet test Ryujinx.sln -c Release` matches the baseline (30,545 / 133 skipped / 84 passed, 0 failed).
3. `dotnet format --verify-no-changes`: about 625 errors already exist (mostly `Ryujinx.Horizon` import ordering). Report only whether the step adds new ones.
4. `./scripts/run-debug.sh` GUI smoke checklist: main window opens; the game list (grid and list); the Settings window (all tabs); the input/controller configuration (SVG artwork visible); the theme switch (custom themes); a ContentDialog (About, Amiibo); the user profile image selector (SkiaSharp resize); and a clean startup log with no binding/Skia errors.
5. If a game or homebrew is available: boot it once. Check that the rendering surface (`EmbeddedWindow`), the software keyboard and a screenshot capture still work.
6. `dotnet list package --outdated` shows only intentional holds.

## Native library notes for the user (Nix)
SkiaSharp 3 and Avalonia 12 native assets normally need `fontconfig`, `freetype`, `libX11`, `libICE`, `libSM`, `libXcursor`, `libXi`, `libXrandr` and `libGL`. Check `flake.nix` (`dotnet-sdk_10` at line 37) against this list. If the GUI fails to load a library, report the exact name and the user adds it to their Nix config.

## Risks and exit criteria
- Likeliest time sinks: FluentAvalonia 3 API changes (45 files) and the Svg replacement.
- If step 2 cannot be completed, revert to the commit after step 1 (SkiaSharp 3 on Avalonia 11.3, which is still a real upgrade) and report what blocked it.
- Gtk3 frontend: it only shares SkiaSharp. Build it after step 1. If SkiaSharp 3 breaks it, report and ask the user whether Gtk3 stays supported. Do not silently drop it.
- Order of merge: `upgrade/ui-stack` into main only after every step passes verification. Then workstream E (docs/CI/Nix pins).
