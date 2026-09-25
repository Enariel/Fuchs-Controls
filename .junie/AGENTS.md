# FuchsControls development notes

## Project overview

- `FuchsControls` is a .NET 10 MAUI control library inspired by FlatifyCSS. 
  - Its goal is to provide reusable flat-design controls, styles, behaviours, converters, and handlers for MAUI applications.
- The project structure is as follows:
  - `Controls`: Where all controls are defined.
  - `Controls/Buttons`: button and button-group controls.
  - `Controls/Containers`: container and tab controls.
  - `Controls/Forms`: form and input controls.
  - `Controls/Layout`: layout controls. 
  - `Controls/Typography`: typography controls such as spans and text components.
  - `Behaviours`: reusable control behaviours.
  - `Converters`: binding converters.
  - `Handlers`: application and platform handler integrations.
  - `Theme`: application theming support.
  - `Resources/Styles`: shared XAML resources and design tokens; `Resources/Fonts` and `Resources/Images` hold packaged assets.
  - `Platforms/<Platform>`: platform-specific implementations for Android, iOS, Mac Catalyst, and Windows.
  - `Reference/Flatify`: upstream/reference FlatifyCSS JavaScript and SCSS used for design and behaviour comparison; it is not a compiled application source tree. 
- The project uses SDK-style MSBuild with `<UseMaui>true</UseMaui>`, `<SingleProject>true</SingleProject>`, nullable reference types, and implicit usings enabled.
- Prefer C# for UI code over XAML, using `CommunityToolkit.Maui.Markup` for UI code.
- Utilize behaviors, converters, extension methods, and handlers to encapsulate reusable logic and platform integration.
- When creating and designing custom controls/components: 
  - Reuse existing `Fuchs*` design tokens and styles instead of introducing duplicate colors, dimensions, shadows, or control states.
  - Reference the upstream FlatifyCSS design and behaviour for guidance, but do not copy its implementation directly.
  - Utilize behaviors, converters, and handlers to encapsulate reusable logic and platform integration.
  - Avoid adding platform conditionals to shared controls when a platform handler or partial implementation is more appropriate.
  - Build controls with a focus on reusability, maintainability, modularity, and cross-platform compatibility.
  - Follow best practices for accessibility, performance, and responsiveness in MAUI applications.

## C# / .NET

- Use C#/.NET 10, nullable reference types.
- Prefer focused classes and methods.
  - `readonly`, compile-time safety, and `async`/`await`.
    - Never block async code with `.Wait()` or `.Result`.
  - Always await `Tasks` asynchronously.
  - Avoid unrelated refactoring and unnecessary abstractions.
  - Each class should be it's own file unless it is a small private class or nested type.
- Prefer composition to inheritance if possible.
- Wrap asynchronous code in try/catch blocks and handle exceptions appropriately, especially async void methods.
- Do not use obsolete APIs. Use asynchronous APIs when available as an alternative to obsolete ones.

## Build and configuration

### SDK and targets

- The project currently targets `net10.0-android`, `net10.0-ios`, `net10.0-maccatalyst`, and, on Windows, `net10.0-windows10.0.19041.0`. Linux intentionally excludes Apple and Windows targets.
- Minimum platform versions are Android API 21, iOS 15.0, Mac Catalyst 15.0, and Windows 10.0.17763.0. Use a matching MAUI/.NET 10 workload and platform SDK when building a platform target.
- Restore dependencies before the first build if the local NuGet cache is incomplete. If an existing assets file reports a missing target/runtime identifier, run `dotnet restore .\FuchsControls.csproj` before building.
- Build the project from the repository root. On Windows, the most useful local validation target is:

```powershell
dotnet build .\FuchsControls.csproj --configuration Release --framework net10.0-windows10.0.19041.0
```

- A verified Windows validation sequence is:

```powershell
dotnet restore .\FuchsControls.csproj
dotnet build .\FuchsControls.csproj --configuration Release --framework net10.0-windows10.0.19041.0 --no-restore
```

  This completed successfully with the current repository and SDK. The build may report a preview-SDK notice when using a .NET 10 preview SDK; that notice is not a project error.

- The project is a library, not an executable. It does not have an application startup target to run directly; consume it from a MAUI host application when validating UI behavior.
- The default SDK compile glob includes nested `*.cs` files. Do not place temporary C# programs or test sources under the repository tree unless they are intentionally part of the library or explicitly excluded.
- There is no test project in the repository. Validate shared code with the library build; validate UI behaviour from a separate MAUI host application rather than adding ad-hoc executable sources to this repository.

## Source and style conventions

- Match the surrounding C# style: file-scoped namespaces, tabs for indentation in the existing converter files, `PascalCase` for public types and members, and nullable annotations consistent with the project. Components and resource keys use the `Fuchs` prefix and descriptive PascalCase names.
- Converter implementations are small `IValueConverter` classes under `FuchsControls.Converters`. Keep `Convert` and `ConvertBack` behavior explicit and provide safe fallback values for unsupported input where that is the established class behavior.
- Keep XAML resources in `Resources/Styles/FuchsStyles.xaml` organized by section. Reuse existing `Fuchs*` design tokens and styles instead of introducing duplicate colors, dimensions, shadows, or control states.
- Resource keys use the `Fuchs` prefix and descriptive PascalCase names, such as `FuchsAccentColor`, `FuchsCornerRadius`, and `FuchsButtonShadow`. Maintain this naming scheme for new resources.
- Components also use the `Fuchs` prefix and descriptive PascalCase names for their components.
- Keep platform-specific implementations in their corresponding `Platforms/<Platform>` directory. Avoid adding platform conditionals to shared controls when a platform handler or partial implementation is more appropriate.
- Preserve the existing package/resource conventions and avoid introducing duplicate design tokens when an existing `Fuchs*` resource or style can be reused.

## Debugging and review guidance

- When a build fails, first identify the target framework in the MSBuild output; a failure may be platform-workload-specific rather than shared-library code.
- For XAML failures, check source-generation diagnostics, resource-key spelling, target types, and whether a referenced `StaticResource` is defined before use or otherwise available in the merged dictionaries.
- For binding/converter issues, inspect the runtime value type and nullability before changing conversion logic. Add a focused regression test for each newly discovered input shape.