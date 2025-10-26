<!-- Copilot / AI agent instructions for contributors -->
# Blizzard_Controller_Support — AI coding assistance guide

This repo contains a legacy overlay app and a modern Avalonia UI. The notes below highlight the minimal, high-value facts an automated coding agent needs to be productive.

## Big picture
- Two UI/runner targets:
  - `Controller` (legacy): native overlay drawing using Raylib/MonoGame. Main loop and platform interop live under `Controller/Classes` (see `OverlayWindowMonoGame.cs`, `OverlayWindow.cs`).
  - `Controller_v2` (current UI): Avalonia-based settings app. Entry is `Controller_v2/App.axaml.cs` and UI -> VM bindings live in `Controller_v2/ViewModels` and `Controller_v2/Views`.
- Shared domain code (controller inputs, settings, platform Invoke wrappers) lives in `Controller/Classes` and is consumed by both targets.

## Key files (quick reference)
- `Controller/Classes/AppSettings.cs` — singleton settings instance (AppSettings.Instance) used for UI binding and runtime configuration. Changing public property names breaks bindings.
- `Controller/Classes/ControllerInputs.cs` — reads controller state and converts it to mouse events and app actions. Also holds legacy static fields that `AppSettings` mirrors for backwards compatibility.
- `Controller/Classes/GameSettings.cs` — per-game overlay sizes, offsets, and the process names used for game detection.
- `Controller/Classes/Invoke.cs` — platform P/Invoke helpers and platform-specific code guarded by `#if WINDOWS/MACOS/LINUX`.
- `Controller/Classes/OverlayWindowMonoGame.cs` & `OverlayWindow.cs` — overlay draw loop and window placement logic for different OSes.
- `Controller_v2/App.axaml.cs` & `Controller_v2/ViewModels/MainViewModel.cs` — starts background threads (controller check/overlay) and shows how UI binds to `AppSettings`.

## Architecture & data flow notes
- Game detection: `ControllerInputs.CheckGameProc` (Background task) sets `OverlayWindow.SC2Proc/SC1Proc/WC*Proc` using `OverlayWindow.GetProcess(...)`. Overlays read those to size/position themselves.
- Settings sync: UI changes `AppSettings.Instance` which raises `PropertyChanged` events; `ControllerInputs` reads `AppSettings.Instance` at runtime. `AppSettings` still writes some values into `ControllerInputs` static fields for backward compatibility — preserve that behavior when refactoring.
- Platform behavior: many methods are OS-gated with `#if WINDOWS/MACOS/LINUX`. When adding features, follow those guards and prefer adding platform-specific helpers in `Invoke.cs`.

## Build, run, and debug (concrete commands)
- Build the solution: `dotnet build Controller.sln` (there's a workspace task named `build` that runs this).
- Run modern UI (Avalonia desktop):
  - On Windows: `dotnet run --project Controller_v2.Desktop` (preferred for quick dev); or open the solution in Visual Studio to debug with proper platform defines.
- Run legacy overlay: on Windows the `Controller` project targets `net8.0-windows` and loads `MonoGame`/`Raylib`. Run via Visual Studio or `dotnet run --project Controller` on Windows. Note: overlay relies on native windowing and may require running on the matching OS/permissions.
- CI: the repo contains `.github/workflows/build-and-upload.yml` for packaged builds.

## Project conventions and gotchas
- Do not rename public properties in `AppSettings` — UI uses exact property names for bindings and `ControllerInputs` expects the singleton to expose those names.
- Backward-compatibility: `AppSettings` intentionally mirrors values into static variables on `ControllerInputs` (see `AppSettings.cs` comments). When modernizing code, either keep these mirrors or change both consumer sites.
- Resources: legacy overlay images are in `Controller/Resources` and are copied at build-time; `Controller_v2/Assets` contains icons for the Avalonia app.
- Game-specific numbers: overlay size/offsets live in `GameSettings` (tuning here affects overlay placement across supported games). Example: `GameSettings.StarCraft2.overlayWidth`.

## Integration points & external deps
- NuGet packages of note (see `Controller.csproj`): `MonoGame.Framework.WindowsDX`, `Raylib-cs`, `SharpHook`, `System.Configuration.ConfigurationManager`. The Avalonia project uses `CommunityToolkit.Mvvm` and `MsBox.Avalonia`.
- Native interaction: `Invoke.cs` wraps platform APIs (Win32 / macOS CoreGraphics / X11). Changes here affect window detection and mouse injection.

## Debugging tips / common developer workflows
- If the overlay is not positioning correctly: breakpoint `OverlayWindow.GetWindowSize` / `OverlayWindowMonoGame.GetWindowSize` and inspect `GameSettings.*` values and detected `Process.MainWindowHandle`.
- To trace controller input handling: set breakpoints in `ControllerInputs.processButtons()` and `processJoysticks()`; `AppSettings.Instance` is the authoritative source for runtime parameters (deadzone, cursor speed).
- To change runtime defaults (example): edit `Controller/Classes/AppSettings.cs` (default deadzone, cursor speed) or `Controller/Classes/GameSettings.cs` for overlay layout.

## Example edits (copy-paste locations)
- Change default deadzone: `Controller/Classes/AppSettings.cs` (property initializer / ctor).
- Tweak SC2 overlay right-side offset: `Controller/Classes/GameSettings.cs` -> `StarCraft2.sideOffset`.
- Add a new UI binding: modify `Controller_v2/ViewModels/MainViewModel.cs` to expose `AppSettings.Instance` properties and update `Controller_v2/Views/MainView.axaml`.

If anything here is unclear or you want me to expand a specific area (run/debug steps for your OS, or a short patch that documents one of the tricky functions), tell me which section to improve.
