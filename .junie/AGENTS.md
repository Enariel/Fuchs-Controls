# FuchsControls development notes

## Project overview

- `FuchsControls` is a .NET MAUI control library inspired by FlatifyCSS. The main source areas are `Controls`, `Behaviours`, `Converters`, `Handlers`, `Theme`, `Resources`, and platform-specific code under `Platforms`.
- The project uses SDK-style MSBuild with `<UseMaui>true</UseMaui>`, `<SingleProject>true</SingleProject>`, nullable reference types, and implicit usings enabled.
- XAML uses MAUI namespaces, and the project enables source-generated XAML with `<MauiXamlInflator>SourceGen</MauiXamlInflator>`. Keep XAML compatible with source generation unless a file explicitly needs runtime inflation.

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

## Testing

- There is currently no committed test project in this repository. For reusable automated coverage, add a separate test project outside the library's source glob (normally as a sibling project in the solution), reference the target framework under test, and use the repository's existing .NET/MAUI version.
- A simple converter smoke test was verified on Windows by referencing `FuchsControls.csproj`, targeting `net10.0-windows10.0.19041.0`, and asserting both directions of `IntToBoolConverter`:

```csharp
using System.Globalization;
using FuchsControls.Converters;

var converter = new IntToBoolConverter();
if (!Equals(converter.Convert(1, typeof(bool), null!, CultureInfo.InvariantCulture), true))
    throw new Exception("1 should convert to true.");
if (!Equals(converter.ConvertBack(false, typeof(int), null!, CultureInfo.InvariantCulture), 0))
    throw new Exception("false should convert to 0.");
```

- The temporary executable was run successfully with:

```powershell
dotnet run --project .\tmp-smoke\SmokeTest.csproj --configuration Release
```

  It printed `Smoke test passed.`. The temporary project and source were deleted afterward and must not be committed.
- For converter tests, cover valid input, null/wrong-type input, and `ConvertBack` behavior. Pass `CultureInfo.InvariantCulture` unless culture-specific behavior is the subject of the test.
- For controls and handlers, prefer tests in a MAUI-capable host or platform test environment; verify bindings, visual states, resource lookup, and platform-specific behavior rather than only object construction.
- Existing builds currently report nullable warnings (`CS8767`) because several `IValueConverter` implementations use non-nullable `parameter` parameters while the MAUI interface declares them nullable. Treat these as an existing warning baseline; do not hide new warnings or errors.

## Source and style conventions

- Match the surrounding C# style: file-scoped namespaces, tabs for indentation in the existing converter files, `PascalCase` for public types and members, and nullable annotations consistent with the project.
- Converter implementations are small `IValueConverter` classes under `FuchsControls.Converters`. Keep `Convert` and `ConvertBack` behavior explicit and provide safe fallback values for unsupported input where that is the established class behavior.
- Preserve the metadata region used by the converter files when adding or changing files, including created/modified dates if the project convention requires them.
- Keep XAML resources in `Resources/Styles/FuchsStyles.xaml` organized by section. Reuse existing `Fuchs*` design tokens and styles instead of introducing duplicate colors, dimensions, shadows, or control states.
- Resource keys use the `Fuchs` prefix and descriptive PascalCase names, such as `FuchsAccentColor`, `FuchsCornerRadius`, and `FuchsButtonShadow`. Maintain this naming scheme for new resources.
- Components also use the `Fuchs` prefix and descriptive PascalCase names for their components.
- Keep platform-specific implementations in their corresponding `Platforms/<Platform>` directory. Avoid adding platform conditionals to shared controls when a platform handler or partial implementation is more appropriate.
- Follow the existing project and Rider formatting settings. Reformat changed C# and XAML files before review, and keep changes focused; do not reformat unrelated files.

## Debugging and review guidance

- When a build fails, first identify the target framework in the MSBuild output; a failure may be platform-workload-specific rather than shared-library code.
- For XAML failures, check source-generation diagnostics, resource-key spelling, target types, and whether a referenced `StaticResource` is defined before use or otherwise available in the merged dictionaries.
- For binding/converter issues, inspect the runtime value type and nullability before changing conversion logic. Add a focused regression test for each newly discovered input shape.
- Check `git status` before and after work. Existing changes may be in progress; do not overwrite or revert unrelated modified/staged files. Keep generated `bin`/`obj` output and temporary smoke-test files out of commits.
