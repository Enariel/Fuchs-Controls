# FuchsControls agent guide

Use this guide when modifying `FuchsControls`, a .NET 10 MAUI control library for reusable flat-design controls, styles, behaviors, converters, themes, and handlers.

## Project overview

- `FuchsControls` is a .NET 10 MAUI control library inspired by FlatifyCSS.
- It is a library, not an executable application. Validate UI behavior from a separate MAUI host app.
- Prefer C# UI composition over XAML. Use `CommunityToolkit.Maui.Markup` where it improves clarity.
- Keep controls reusable, maintainable, accessible, responsive, and cross-platform.

## Key folders

- `Controls/`: reusable controls.
  - `Buttons/`: buttons and icons.
  - `Containers/`: tabs and container controls.
  - `Forms/`: inputs, form fields, pickers, toggles, and validation states.
  - `Typography/`: text and span controls.
- `Behaviours/`: reusable MAUI behaviors. Keep the existing folder spelling.
- `Converters/`: binding converters.
- `Handlers/`: MAUI and platform handler integrations.
- `Theme/`: themes, variants, resource keys, and theme lookup.
- `Resources/`: shared styles, fonts, images, and design resources.
- `Resources/Styles/FuchsStyles.xaml`: shared XAML resources and design tokens.
- `Platforms/<Platform>/`: platform-specific implementations.
- `Reference/Flatify/`: reference-only upstream FlatifyCSS assets. Do not treat it as compiled source or copy its implementation directly.

## Design system

- Reuse existing `Fuchs*` design tokens, resource keys, styles, theme values, and extension methods.
- Do not introduce duplicate colors, dimensions, shadows, states, or typography rules when an existing token or helper fits.
- Use descriptive `Fuchs`-prefixed PascalCase names for public controls, components, resource keys, and styles.
- Use FlatifyCSS only as a design and behavior reference. Do not port or copy its implementation.
  - FlatifyCSS documentation: <https://github.com/ami-momo/flatifycss/tree/master/docs/docs>
  - FlatifyCSS repository: <https://github.com/ami-momo/flatifycss/tree/master>
- Keep platform-specific visuals or handler behavior in handlers, partials, or `Platforms/<Platform>/` when that is cleaner than shared-code conditionals.

## C# and MAUI style

- Use C#/.NET 10 with nullable reference types.
- Match the surrounding style before introducing new patterns.
- Use file-scoped namespaces, `PascalCase` public APIs, and one public type per file unless a nested or private type is clearer.
- Prefer focused classes, composition, compile-time safety, and clear bindable properties.
- Use `async`/`await`; never block async work with `.Wait()` or `.Result`.
- Catch and handle exceptions in asynchronous UI paths where failure would otherwise be silent or disruptive.
- Avoid obsolete APIs; use supported async alternatives when available.
- Avoid unrelated refactoring, speculative abstractions, and broad formatting-only changes.
- Keep shared XAML resources organized by purpose.

## Implementation guidance

- Encapsulate reusable UI logic in controls, behaviors, converters, extension methods, or handlers.
- Keep converters small and explicit. Return safe fallback values for unsupported inputs when that matches existing behavior.
- Keep shared controls platform-neutral where possible.
- Use handlers or platform-specific files for platform integration instead of scattering platform conditionals through controls.
- Preserve existing package, resource, and namespace conventions.
- Prefer existing resource keys and theme values over new hard-coded values.

## Build and validation

- Target frameworks:
  - `net10.0-android`
  - `net10.0-ios`
  - `net10.0-maccatalyst`
  - `net10.0-windows10.0.19041.0` on Windows
- Linux intentionally excludes Apple and Windows targets.
- Restore before the first build or when assets/target errors occur:
