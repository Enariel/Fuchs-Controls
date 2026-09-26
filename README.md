# FuchsControls

FuchsControls is a .NET MAUI control library for reusable flat-design controls, styles, behaviours, converters, themes, and handler integrations. Its visual direction is inspired by [FlatifyCSS](https://github.com/ami-momo/flatifycss), whose assets under `Reference/Flatify/` are reference material only and are not compiled by this project.

This repository contains a library, not a runnable MAUI application. Use it from a separate MAUI host application to view and exercise controls.

## Stack

- Language: C# with nullable reference types enabled.
- Framework: .NET 10 and .NET MAUI.
- Project system and package manager: MSBuild project (`FuchsControls.csproj`) and NuGet.
- UI composition: C# controls, plus shared MAUI XAML resources.
- Key dependencies:
  - `Microsoft.Maui.Controls` 10.0.110
  - `CommunityToolkit.Maui` 15.0.1
  - `CommunityToolkit.Maui.Markup` 8.0.0

## Requirements

- .NET 10 SDK.
- The .NET MAUI workload and any platform workload needed for the target you build.
- A supported platform development environment:
  - Android: Android SDK/toolchain.
  - iOS and Mac Catalyst: macOS with the required Apple toolchain.
  - Windows: Windows development environment targeting Windows 10 version 19041 or later.

The project targets `net10.0-android` on all supported operating systems. It additionally targets `net10.0-ios` and `net10.0-maccatalyst` outside Linux, and `net10.0-windows10.0.19041.0` on Windows.

## Setup and build

Clone the repository, then restore and build the library from the repository root:

```powershell
dotnet restore FuchsControls.csproj
dotnet build FuchsControls.csproj
```

To build a single installed target framework, supply its target framework explicitly:

```powershell
dotnet build FuchsControls.csproj --framework net10.0-android
```

## Use from a MAUI host app

Reference the project from a MAUI application, then register FuchsControls while building the app:

```csharp
using FuchsControls;

var builder = MauiApp.CreateBuilder();

builder
    .UseMauiApp<App>()
    .UseFuchsControls();
```

`UseFuchsControls()` is the library's current public MAUI builder entry point. It applies the registered handler setup, including the form-border configuration.

Controls are organized by purpose under `Controls/`; styles and design tokens are provided by `Resources/Styles/FuchsStyles.xaml`.

TODO: Document the supported resource-dictionary merge pattern and a complete, verified control usage example once a consuming host application is available.

## Run commands

This repository does not contain an executable MAUI host application or solution file, so it has no standalone run command. Build this library, then run the separate consuming MAUI host app with the standard `dotnet run` command for its selected target framework.

TODO: Add the host application's project path and verified platform-specific run commands when it is included in or linked from this repository.

## Scripts

No repository script manifest or custom build/test scripts are present. Use the standard .NET CLI commands:

| Command | Purpose |
| --- | --- |
| `dotnet restore FuchsControls.csproj` | Restore NuGet packages. |
| `dotnet build FuchsControls.csproj` | Build all target frameworks available on the current operating system. |
| `dotnet build FuchsControls.csproj --framework <TFM>` | Build one target framework, such as `net10.0-android`. |

## Environment variables

No repository-specific environment variables are defined in the project files.

Platform tooling may require SDK paths, signing credentials, or other environment configuration. Configure these according to the target platform and your host application's requirements.

TODO: Document any required CI, signing, or platform-specific environment variables if they are introduced.

## Tests

No test projects or test source files are currently present in this repository.

For UI validation, reference the library from a separate MAUI host app and test the target platform there. When automated tests are added, document their project path and execution commands in this section.

## Project structure

```text
Behaviours/              Reusable MAUI behaviours.
Controls/                Reusable controls grouped by buttons, containers, forms, and typography.
Converters/              Binding converters.
Handlers/                MAUI and platform handler integrations.
Platforms/               Platform-specific implementations for Android, iOS, Mac Catalyst, and Windows.
Reference/Flatify/       Reference-only upstream FlatifyCSS assets; not compiled source.
Resources/Styles/        Shared XAML styles and Fuchs design tokens.
Theme/                   Themes, variants, resource keys, and theme lookup.
MauiAppBuilderExtensions.cs
                        MAUI host registration extension (`UseFuchsControls`).
FuchsControls.csproj     Library project configuration and NuGet dependencies.
```

## License

TODO: No license file or license declaration is currently present. Add a license file and update this section before distributing the library.