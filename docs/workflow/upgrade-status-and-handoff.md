# Dependency upgrade: status and handoff

Last updated: 2026-09-21 (after D merged). Repo: `/home/wisp/nintendont` (Ryujinx, C#, .NET 10). Read `CLAUDE.md` first for build and test commands.

Related docs:
- `docs/workflow/major-dependency-upgrade-plan.md`: the original plan (workstreams A to E).
- The UI stack plan (workstream D). It is saved outside the repo at `/home/wisp/.claude/plans/these-are-major-changes-dynamic-lake.md`. The D agent copies it into the repo as `docs/workflow/ui-stack-upgrade-plan.md` on its branch, `upgrade/ui-stack`.

## Where things stand

| Workstream | Status | Where |
|---|---|---|
| Framework | Already on .NET 10 (LTS, SDK 10.0.401). .NET 11 is RC only (GA Nov 2026), so it is out of scope. | `global.json` |
| Minor/patch NuGet bumps | Done | `b513dcf` |
| A. NUnit 3.14 → 4.6.1 | Done and merged | `3958a09` |
| B. Roslyn 4.8 → 5.9.0 | Done and merged | `26bbb3d`, merge `0143df0` |
| D. UI stack (SkiaSharp 3.119.4, Avalonia 12.1.2, FluentAvaloniaUI 3.1.0, Svg.Skia 5.1.1 + local `SvgImage`, Tmds.DBus 0.95.1) | Done and merged (fast-forward, `0aa68ac`). **The GUI was only checked for a clean startup.** | `docs/workflow/ui-stack-upgrade-plan.md` |
| C. Tmds.DBus.Protocol → 0.95.1 | Done inside D, step 5 | `0aa68ac` |
| E. Docs, CI, Nix pins | Not started. Do it after D. | |

Main is at `d72fd55` (formatting fixes on top of D). Nothing has been pushed to any remote.

## What changed so far

- **Package bumps** in `Directory.Packages.props` (centrally managed, so never edit versions in csproj files): Avalonia 11.3.22 (DataGrid 11.3.13), Avalonia.Svg(.Skia) 11.3.0, SkiaSharp 2.88.9, OpenTK 4.9.4, Silk.NET.Vulkan 2.23.0, Concentus, DiscordRichPresence, DynamicData 9.4.33, IdentityModel 8.23.0, MoltenVK 1.2.3, System.Text.Json / System.IO.Hashing / System.Management 10.0.12, Unicorn 2.1.3.
- **Vulkan fix:** `DriverId.MesaAgxv` was renamed to `MesaHoneykrisp` in Silk.NET 2.23 (`src/Ryujinx.Graphics.Vulkan/Vendor.cs`).
- **A:** about 500 classic asserts across 54 test files converted to `Assert.That(...)` constraints.
- **B:** the HLE, Horizon and Horizon.Kernel source generators were ported from `ISourceGenerator` to `IIncrementalGenerator` (6 files). This was beyond the plan's scope. The tests pass, but the generated output was not diffed against the old output.
- **Added files:** `CLAUDE.md`, `AGENTS.md`, `flake.nix`, `flake.lock`, `scripts/run-debug.sh`.

## Verified baseline (main at `0143df0`)

- `dotnet build Ryujinx.sln -c Release`: 0 errors.
- `dotnet test Ryujinx.sln -c Release`: `Ryujinx.Tests` 30,545 passed / 133 skipped, `Ryujinx.Tests.Memory` 84 passed, 0 failed.
- `dotnet format --verify-no-changes` **fails with about 625 errors**, mostly "imports ordering" in `src/Ryujinx.Horizon`, plus whitespace in `src/Ryujinx.HLE/HOS/Kernel/SupervisorCall/SyscallDispatch.cs`. They look pre-existing but that was never checked against `62108ff`. Nothing has been fixed. CI's format check would fail.

## Known gaps and risks

- **The GUI has not been launched since the package bump.** It ran once before the bumps and started cleanly. There is a suspected mismatch: `Avalonia.Svg.Skia` 11.3.0 wants SkiaSharp 3.116.1, while HLE pins 2.88.9. D step 0 checks it.
- **B's generator output was not diffed** before and after the Roslyn 5.9 change.
- **Held-back packages:** Avalonia 12, FluentAvaloniaUI 3.1 and SkiaSharp 3.119.4 (all in D), and Tmds.DBus.Protocol 0.95 (D step 5). It carries a deliberate security pin in `src/Ryujinx/Ryujinx.csproj`. Do not lower it.
- **Avalonia.Svg / Avalonia.Svg.Skia have no Avalonia 12 release.** D step 3 replaces them.
- **`Avalonia.Diagnostics` does not exist for Avalonia 12.** D drops it (Debug-only, unused in code).
- **Gtk3 frontend** shares SkiaSharp with the rest. Confirm it still builds after the SkiaSharp change. If it can't, ask the user before dropping it.

## Git notes

- No git identity is configured. Commit with `git -c user.name=WisperGatis -c user.email=wesley.david09@gmail.com commit ...`. Do not change the git config.
- Commit messages end with `Co-Authored-By: Claude Sonnet 5 <noreply@anthropic.com>`.
- Agent worktrees live under `.claude/worktrees/` (untracked). The branches `upgrade/nunit4` and `upgrade/roslyn5` are merged and can be deleted.
- Worktrees are created from the last **commit**. Uncommitted changes in the main checkout are not visible to them.

## Next steps for the next agent

1. **Wait for or check on D.** Branch `upgrade/ui-stack`. Verify it yourself: build, tests against the baseline above, and the GUI smoke checklist in the UI plan. An agent's report can say "done" when it isn't, and two of the earlier agents finished without sending one.
2. **Merge D into main** only if every step passes. If Avalonia 12 stalls, keep the commits up to SkiaSharp 3 on Avalonia 11.3 and merge only those.
3. **Format errors:** first check whether they exist at `62108ff`. If they do, fix them in a separate commit (`dotnet format`, review the diff).
4. **Workstream E:** update `CLAUDE.md` and `AGENTS.md` if commands changed, and check `.github/workflows/*.yml` and `flake.nix` for pinned versions (SDK, libs).
5. **Nix:** do not edit the user's Nix config. Report any missing native libraries (Skia 3 and Avalonia 12 typically need fontconfig, freetype and the X11 libs) to the user.
6. **Cleanup:** delete the merged worktrees and `upgrade/*` branches.
7. Finish with `dotnet list package --outdated` to confirm only intentional holds remain.

## Rules that apply to every agent

- Follow `CLAUDE.md`, `AGENTS.md` and `docs/coding-guidelines/coding-style.md`.
- Do not weaken or skip tests. Do not hand-edit `Autogenerated/` files.
- Do not push. Ask the user before any outward-facing action.
- If a package has no compatible release, stop and report instead of hacking around it.

## Update after D was merged

- **Format:** `d72fd55` fixed the whitespace/import-ordering errors (66 files). The errors were pre-existing (the Horizon files are unchanged since `62108ff`; the cause is commit `7f8ebbd`). About 229 `dotnet format style` errors remain: `dotnet format style` itself crashes ("Changing document properties is not supported"), so they are unfixed.
- **GUI not verified beyond startup.** Still needs a human pass: input/controller SVG artwork (new `src/Ryujinx/UI/Helpers/SvgImage.cs`), ContentDialogs (About, Amiibo; `FAContentDialog` casts in the user views), the avatar `ColorPicker`, the profile image resize, Settings tabs, the theme switch, game list, and a game boot (rendering surface, software keyboard, screenshot capture).
- **macOS fullscreen title bar** styling was removed because Avalonia 12 dropped `ExtendClientAreaChromeHints`. Unverified.
- **Someone is editing the main checkout** (uncommitted Rose Pine / Gruvbox / Matugen theme work under `src/Ryujinx`, `docs/matugen*`). Do not commit or revert those files.
- **Workstream E (remaining):** `.github/workflows/flatpak.yml:52` has a stale ".NET 8.0.100" comment; `flake.nix` already uses `dotnet-sdk_10` and lists `libGL` and `fontconfig`. Compare the rest of its library list with what Skia 3 needs. Update `CLAUDE.md` only if commands change. Delete the merged worktrees under `.claude/worktrees/` and the `upgrade/*` branches.
