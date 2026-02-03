## Junie Guidelines for FuchsControls (.NET MAUI Control Library)

### Overview

FuchsControls is a reusable .NET MAUI control library intended for simple “plug and play” usage across multiple apps. Controls should:

- Be composable, lightweight, and easy to integrate.
- Expose sensible defaults with minimal required configuration.
- Allow optional deep customization via bindable properties, styles, templates, and themes.
- Support all enabled platforms.

Where visual styling is applied, use styles inspired by the SCSS resources under Resources/scss (e.g., rounded radii, accent color usage, subtle shadows,
hover/active brightness shifts, and accessible contrast).

---

### Build and Configuration

- Project type: .NET MAUI Class Library targeting .NET 10.0.
- Prerequisites:
    - .NET 10.0 SDK
    - MAUI Workload (dotnet workload install maui)
- Initialization:
    - Consumers MUST call `builder.UseFuchsControls()` in their `MauiProgram.cs` to enable library features (e.g., borderless form fields).
- Build:
    - dotnet build
    - Target platforms as needed (e.g., -f net10.0-android, -f net10.0-windows10.0.19041.0)
- NuGet:
    - CommunityToolkit.Maui
    - CommunityToolkit.Maui.Markup
    - CommunityToolkit.Mvvm
- Solution:
    - Use FuchsControls.slnx in Rider/Visual Studio.

---

### Code Style

- Tabs for indentation.
- File-scoped namespaces (e.g., namespace FuchsControls.Fields;).
- Naming: PascalCase for public types/members; camelCase for private fields.
- Nullable: annotate public API carefully and handle null defensively.
- Async/await:
    - Prefer async APIs end-to-end; never block with .Result or .Wait().
    - ConfigureAwait(false) in library internals when not interacting with UI context.
    - Consider IAsyncEnumerable/IAsyncDisposable when streaming/holding resources.
- Composition over inheritance for controls. Favor ContentView/Templated controls composed from primitives.
- Avoid obsolete APIs (e.g., Frame); prefer modern containers and borders.

---

### Library Design Principles

- Plug-and-Play Defaults:
    - Each control must work “out of the box” with a single constructor and minimal required properties.
    - Provide clear default text, padding, colors, and states.
- Consistent Bindable Properties:
    - Expose bindable properties for key aspects (Text, Icon, IsBusy, Command, CommandParameter, CornerRadius, Elevation/Shadow, Spacing, AccentColor).
    - Keep property names consistent across controls to reduce cognitive load.
- Templates and Styling:
    - Provide ControlTemplate/DataTemplate hooks where appropriate (HeaderTemplate, ItemTemplate, EmptyView, FooterTemplate).
    - Support StyleClass and Style setters; ship default Styles in ResourceDictionaries.
    - Support dynamic theming via dynamic resources and app-level resource keys (e.g., FuchsAccentColor).
- Accessibility:
    - Set AutomationProperties.Name/HelpText where appropriate.
    - Provide large touch targets, focus visuals, and high-contrast defaults.
    - Respect reduced motion preferences by falling back to fade transitions when animations are disabled.
- Responsiveness:
    - Controls should adapt to different densities/sizes; avoid hard-coded pixel sizes when device-independent units or layout constraints suffice.
- Performance:
    - Avoid creating/destroying views excessively; reuse when possible.
    - Defer heavy work; use OnHandlerChanged for platform specifics.
    - Unsubscribe from events (memory-leak safe). Prefer weak events when applicable.

---

### Structure

- Converters: IValueConverter implementations.
- Components: Reusable primitives and composites.
    - Components/Fields: Input/validation helpers and form fields.
    - Components/Buttons: Actionable controls aligned with design tokens.
    - Components/Containers: Layout/surface components (cards, accordions, tabs).
- Extensions: Extension methods/helpers.
- Resources:
    - Styles: ResourceDictionaries for shared styles, colors, and default templates.
    - SCSS-inspired styling guidance (see “Styling Guidance” below).

---

### Public API Guidelines

- Naming and Consistency:
    - Prefer property names used by MAUI where possible (Text, ItemsSource, SelectedItem, Placeholder, IsEnabled, IsVisible).
    - Commands: Command + CommandParameter; raise CanExecuteChanged appropriately.
- Events:
    - Use EventHandler<TEventArgs>; fire events on main thread when they affect UI.
    - Offer both event and Command when feasible (e.g., Clicked and Command).
- Validation:
    - Where inputs exist, expose IsValid, ValidationMessage, and ValidationState enums as needed, but keep simple by default.

---

### Resource and Styling Guidance

- Visual Language (inspired by Resources/scss):
    - Rounded corners (configurable radius), subtle borders, light inset/outset shadows.
    - Accent-first approach: default accent aligns with “accent primary” tone; provide light/dark variants.
    - Hover/focus/active interactions adjust brightness/contrast rather than color shifts.
    - Provide StyleClass hooks like: style-accent, style-success, style-warning, style-danger, style-light, style-dark.
- Resource Keys:
    - Define reusable keys: FuchsCornerRadius, FuchsBorderWidth, FuchsShadowOpacity, FuchsAccentColor, FuchsWarningColor, FuchsSuccessColor.
    - Provide dynamic resources for colors/brushes used by controls to support theming.
- Animations:
    - Offer subtle animations (fade/slide/scale) that can be disabled via a shared boolean resource (FuchsReduceMotion).
    - Respect platform accessibility settings.

---

### XAML and Markup

- Provide both XAML and C# Markup-friendly APIs:
    - Ensure controls are easy to instantiate in XAML with attributes.
    - Keep constructor overloads minimal; use properties for configuration.
- ResourceDictionaries:
    - Ship default styles in Resources/Styles/*.xaml and merge at library level.
    - Include light/dark variants; prefer DynamicResource.
- No implicit global overrides:
    - Do not change default styles of core MAUI controls globally; scope styles to Fuchs controls.

---

### Testing and Samples

- Add sample pages demonstrating:
    - Default usage with zero configuration.
    - Theming via dynamic resources.
    - Accessibility settings (larger fonts, reduced motion).
- Provide basic unit tests for value converters and non-UI logic when a test project is introduced.

---

### Documentation

- Each control:
    - One-paragraph purpose.
    - Minimal example (XAML and C#).
    - Properties/Events/Commands table (concise).
    - Theming and styling snippet with resource keys.
    - Notes on accessibility and performance considerations.

---

### Platform Notes

- Use platform-appropriate gestures/feedback with MAUI abstractions.
- Keep platform-specific handlers minimal and isolated behind partials or mappers.
- Respect SupportedOSPlatformVersion in project settings; avoid APIs below those baselines.

---

### Avoid

- Obsolete components (e.g., Frame) in new work.
- Heavy inheritance trees; prefer small composable parts.
- Breaking changes to public API without version notes.
- Introducing new dependencies without explicit approval.

---

### Ready-to-Use Defaults Checklist (per control)

- Sensible default padding/margins aligned with SCSS-inspired design.
- Default accent-aware color scheme.
- Bindable properties for content, state, and actions.
- Accessible names and focus visuals.
- StyleClass hooks (style-accent, style-light/dark).
- DynamicResource usage for colors/brushes.
- Reduced-motion fallback.