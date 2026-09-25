# FuchsControls development notes

## Project overview

- `FuchsControls` is a .NET MAUI control library inspired by FlatifyCSS. 
  - The main source areas are: 
    - `Controls` : Contains custom controls.
      - `Controls/Forms` : Form and Input related field controls. 
      - `Controls/Buttons` : Button and button group controls.
      - `Controls/Layout` : Layout controls.
      - `Controls/Other` : Other controls.
    - `Behaviours` : Implementation of custom control behaviors.
    - `Converters` : Binding converters.
    - `Handlers` : Application handlers.
    - `Theme` : Application theming.
    - `Resources` : Application resources and styles.
    - `Platforms` : Platform-specific code.
- The project uses SDK-style MSBuild with `<UseMaui>true</UseMaui>`, `<SingleProject>true</SingleProject>`, nullable reference types, and implicit usings enabled.
- XAML uses MAUI namespaces, and the project enables source-generated XAML with `<MauiXamlInflator>SourceGen</MauiXamlInflator>`. Keep XAML compatible with source generation unless a file explicitly needs runtime inflation.

## C# / .NET

- Use C#/.NET 10, nullable reference types, standard naming, and `var` when the type is obvious.
- Prefer focused classes and methods, `readonly`, compile-time safety, and `async`/`await`; never block async code with `.Wait()` or `.Result`.
  - Always wait Tasks asynchronously.
- Prefer composition over inheritance.
- Do not use obsolete APIs. Use asynchronous APIs when available as an alternative to obsolete ones.
- Wrap asynchronous code in try/catch blocks and handle exceptions appropriately, especially async void methods.

## Build and configuration

### SDK and targets

- The project currently targets `net10.0-android`, `net10.0-ios`, `net10.0-maccatalyst`, and, on Windows, `net10.0-windows10.0.19041.0`. Linux intentionally excludes Apple and Windows targets.
- Minimum platform versions are Android API 21, iOS 15.0, Mac Catalyst 15.0, and Windows 10.0.17763.0. Use a matching MAUI/.NET 10 workload and platform SDK when building a platform target.
- The only direct package reference is `Microsoft.Maui.Controls` version `10.0.110`. Restore dependencies before the first build if the local NuGet cache is incomplete.
- Build the project from the repository root. On Windows, the most useful local validation target is:

```powershell
dotnet build .\FuchsControls.csproj --configuration Release --framework net10.0-windows10.0.19041.0
```

- The project is a library, not an executable. It does not have an application startup target to run directly; consume it from a MAUI host application when validating UI behavior.
- The default SDK compile glob includes nested `*.cs` files. Do not place temporary C# programs or test sources under the repository tree unless they are intentionally part of the library or explicitly excluded.

## Source and style conventions

- Match the surrounding C# style: file-scoped namespaces, tabs for indentation in the existing converter files, `PascalCase` for public types and members, and nullable annotations consistent with the project.
- Converter implementations are small `IValueConverter` classes under `FuchsControls.Converters`. Keep `Convert` and `ConvertBack` behavior explicit and provide safe fallback values for unsupported input where that is the established class behavior.
- Keep XAML resources in `Resources/Styles/FuchsStyles.xaml` organized by section. Reuse existing `Fuchs*` design tokens and styles instead of introducing duplicate colors, dimensions, shadows, or control states.
- Resource keys use the `Fuchs` prefix and descriptive PascalCase names, such as `FuchsAccentColor`, `FuchsCornerRadius`, and `FuchsButtonShadow`. Maintain this naming scheme for new resources.
- Components also use the `Fuchs` prefix and descriptive PascalCase names for their components.
- Keep platform-specific implementations in their corresponding `Platforms/<Platform>` directory. Avoid adding platform conditionals to shared controls when a platform handler or partial implementation is more appropriate.

## Debugging and review guidance

- When a build fails, first identify the target framework in the MSBuild output; a failure may be platform-workload-specific rather than shared-library code.
- For XAML failures, check source-generation diagnostics, resource-key spelling, target types, and whether a referenced `StaticResource` is defined before use or otherwise available in the merged dictionaries.
- For binding/converter issues, inspect the runtime value type and nullability before changing conversion logic. Add a focused regression test for each newly discovered input shape.