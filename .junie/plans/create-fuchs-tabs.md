---
sessionId: session-260924-012716-shbc
---

# Requirements

### Overview & Goals
Add code-only `FuchsTabs` and `FuchsTab` controls in the `FuchsControls.Controls` namespace, styled after the Flatify tab component and integrated with the existing Fuchs theme system.

### Scope
**In scope**
- `FuchsTabs` container for multiple `FuchsTab` instances.
- `FuchsTab` label/header plus arbitrary MAUI `ContentView` content.
- Bindable tab collection, selected-tab/index state, and selection notification suitable for XAML-host application bindings even though the controls themselves use only C#.
- Persistent mounted content so switching tabs does not recreate controls or lose entered values, scroll positions, or view-model state.
- Flatify-inspired active indicator, inactive opacity, bordered/rounded wrapper, header layout, panel padding, disabled behavior, and optional scrollable header.
- Runtime theme refresh through `FuchsThemeProvider.ThemeChanged` and existing `FuchsTheme` tokens.

**Out of scope**
- JavaScript, HTML, SCSS compilation, or XAML control definitions.
- Platform-specific handlers unless MAUI validation exposes a platform-only requirement.
- Tab virtualization, lazy content creation, or animation APIs beyond what can be safely supported by the shared MAUI visual tree.

### Functional Requirements
- `FuchsTab` exposes a bindable `Header`/label and uses the inherited `Content` property for its panel view.
- `FuchsTabs` exposes an observable tab collection and keeps exactly one valid tab selected when tabs exist; it handles empty collections and collection changes safely.
- `SelectedIndex` and selected tab state remain synchronized, with a selection-changed event/notification for host code.
- Clicking a header changes selection, updates accessibility-selected state and active visual state, and shows only the selected panel without removing inactive panels from the visual tree.
- Disabled tabs cannot be selected and have an appropriately muted visual state.
- The active tab uses the theme accent color for text/indicator; inactive tabs use inherited theme text with Flatify-like reduced opacity.
- Runtime theme changes update wrapper, header, panel, tab text, indicator, borders, spacing, and typography without reconstructing tab content.
- Defaults and resource names follow existing `Fuchs*` naming and `FuchsComponent` conventions.

# Technical Design

### Current Implementation
- `Controls/FuchsComponentBase.cs:5-90` provides shared `Color`, `Variant`, `Size`, and `IsDisabled` bindable properties, subscribes to `FuchsThemeProvider.ThemeChanged`, and supplies size-based font/padding helpers.
- `Controls/Cards/FuchsCardControl.cs:6-157` is the closest composition reference: it builds `Border`, `Grid`, and `ContentPresenter` objects in C#, rebuilds layout on content changes, and applies theme values directly.
- `Controls/Buttons/FuchsButtonControl.cs:8-144` demonstrates tap gestures, disabled handling, and visual updates from theme properties.
- `Theme/FuchsThemeProvider.cs` applies current theme values to application resources and raises `ThemeChanged`; `Theme/FuchsTheme.cs` supplies background/text/accent colors, border width, radii, and typography/padding tokens.
- `Resources/Styles/FuchsStyles.xaml:7-61` defines the core tokens and `:107-145` defines reusable component styles. There are currently no tab resources.
- Flatify reference `Reference/Flatify/scss/components/_tabs.scss:26-201` defines wrapper/header/content structure, bottom active line, inactive opacity, optional top line, scrollable header, and padded panels; `Reference/Flatify/js/components/tabs.js:11-117` keeps panels mounted while changing active state.

### Key Decisions
- Use two code-only `ContentView` controls in `FuchsControls.Controls`; `FuchsTabs` owns a collection of `FuchsTab` instances and renders separate header and panel layouts.
- Keep all tab panels mounted and toggle `IsVisible`/layout participation instead of replacing `Content`; this preserves state and matches Flatify’s DOM-preserving behavior.
- Use an observable collection with collection-change synchronization rather than an `ItemsView`, because arbitrary stateful MAUI content should not be virtualized or templated away.
- Use bindable `SelectedIndex` as the primary external selection contract plus a read-only/current selected-tab view and a `SelectedTabChanged`/selection event; coerce invalid indices and avoid feedback loops during synchronization.
- Implement styling through C# `ApplyTheme()` and named `DynamicResource`-compatible style resources in `FuchsStyles.xaml`, reusing existing theme colors/radii/padding rather than introducing duplicate hard-coded design tokens.

### Proposed Changes
- Add `Controls/Tabs/FuchsTabControl.cs` with `FuchsTab : FuchsComponent`.
  - Add bindable `Header` string (and, if needed by the established API shape, a header/content-friendly property without duplicating `ContentView.Content`).
  - Let callers assign arbitrary `View` content through inherited `Content`.
  - Maintain internal selection/visibility state supplied by the parent and expose a safe disabled state through inherited `IsDisabled`.
- Add `Controls/Tabs/FuchsTabsControl.cs` with `FuchsTabs : FuchsComponent`.
  - Add bindable `Tabs` collection, `SelectedIndex`, and Flatify-oriented options such as `IsBordered`, `IsScrollable`, and `LineAtTop` only where they map cleanly to the reference behavior.
  - Build a rounded wrapper containing a header layout and content layout; create one clickable header view and one persistent panel host per `FuchsTab`.
  - Subscribe/unsubscribe to collection changes and tab property changes, rebuild only header/panel composition when the collection changes, and preserve existing `FuchsTab` instances/content.
  - On selection, update all header states, panel visibility, accessibility metadata, and selection notification in one guarded operation.
  - Dispose/unsubscribe event handlers when the control handler is removed to avoid retaining tab/container instances.
- Extend `Resources/Styles/FuchsStyles.xaml` in its organized component-default section with tab wrapper, header, header-button/indicator, content, and panel resources. Use `DynamicResource` references for `FuchsTextColor`, `FuchsBackgroundColor`, `FuchsBackgroundDarkerColor`, `FuchsAccentColor`, `FuchsBorderWidth`, existing radii, and panel/control padding.
- Add only any missing tab-specific theme properties/resource mappings required by the implementation to `Theme/FuchsTheme.cs` and `Theme/FuchsThemeProvider.cs`; otherwise resolve directly from existing tokens so custom themes remain centralized.
- Preserve the project metadata-region convention on new C# files and keep namespaces exactly `FuchsControls.Controls`.

### Public Contract Sketch
```csharp
public sealed class FuchsTab : FuchsComponent
{
    public static readonly BindableProperty HeaderProperty;
    public string Header { get; set; }
}

public sealed class FuchsTabs : FuchsComponent
{
    public static readonly BindableProperty TabsProperty;
    public static readonly BindableProperty SelectedIndexProperty;
    public static readonly BindableProperty IsBorderedProperty;
    public static readonly BindableProperty IsScrollableProperty;
    public static readonly BindableProperty LineAtTopProperty;

    public IList<FuchsTab> Tabs { get; }
    public int SelectedIndex { get; set; }
    public FuchsTab? SelectedTab { get; }
    public event EventHandler<...>? SelectedTabChanged;
}
```
The exact event args type and collection-property implementation should follow MAUI bindable-property conventions while ensuring collection replacement and in-place collection edits both update the visual tree.

### File Structure
- Add `Controls/Tabs/FuchsTabControl.cs`.
- Add `Controls/Tabs/FuchsTabsControl.cs`.
- Modify `Resources/Styles/FuchsStyles.xaml` for tab resources.
- Modify `Theme/FuchsTheme.cs`/`Theme/FuchsThemeProvider.cs` only if new tab tokens are needed; otherwise leave the existing theme contract unchanged.
- No project-file change is expected because the SDK compile glob includes nested C# files; `FuchsControls.csproj:8-19` confirms the MAUI/source-generation configuration and `:34-48` confirms the existing folder conventions.

# Testing

### Validation Approach
- Build `FuchsControls.csproj` for the repository’s Windows target in Release configuration, checking that the new code compiles across the shared MAUI API surface and introduces no new errors.
- Validate in a MAUI-capable host or focused smoke harness that constructs `FuchsTabs` with multiple `FuchsTab` instances containing stateful controls.

### Key Scenarios
- Initial selection chooses the configured/first valid tab and exposes matching `SelectedIndex`/`SelectedTab`.
- Selecting each header updates exactly one active header and panel, including repeated clicks and selection changes in both directions.
- Text entered, scroll position, and child control state in one panel remain after switching away and back.
- Adding, removing, replacing, and clearing tabs keeps collection subscriptions, selection bounds, and visual children consistent.
- `Header`, selected-index bindings, `IsBordered`, `IsScrollable`, `LineAtTop`, `Color`, `Variant`, `Size`, and `IsDisabled` produce the expected visual updates.
- Applying a new `FuchsTheme` updates colors, border/indicator, radius, padding, and typography without losing panel content.
- Disabled tabs cannot be activated and inactive/disabled opacity follows the intended Flatify-inspired state.

### Edge Cases
- Empty or null-replaced collections, duplicate tab instances, invalid selected indices, and a selected tab being removed.
- Header labels that are empty or long when the header is scrollable versus wrapping.
- Theme changes before handler attachment and handler removal without event-handler leaks.

### Test Changes
There is no committed test project currently; use a MAUI-capable host/smoke validation for visual and stateful behavior and keep any temporary validation project outside the library source tree.

# Delivery Steps

### ✓ Step 1: Implement FuchsTab content contract
`FuchsTab` provides a bindable header and state-preserving content surface in the existing controls namespace.

- Add `Controls/Tabs/FuchsTabControl.cs` as a code-only `FuchsComponent`.
- Expose the header/label bindable property and rely on inherited `ContentView.Content` for arbitrary panel content.
- Add internal parent-coordination hooks for active state and theme refresh without recreating content.
- Follow the existing metadata, nullable, file-scoped namespace, and constructor-built control conventions.

### ✓ Step 2: Build FuchsTabs selection and composition
`FuchsTabs` renders persistent tab panels and selectable headers with synchronized collection and selection state.

- Add `Controls/Tabs/FuchsTabsControl.cs` with observable tab collection, selected index/current tab contract, selection notification, and Flatify-oriented layout options.
- Build the rounded wrapper, header row, and content host in C# using MAUI layouts and presenters.
- Keep every supplied `FuchsTab` panel mounted while toggling visibility and update one active header/panel at a time.
- Handle collection replacement/changes, invalid indices, disabled tabs, duplicate/removal cases, and event unsubscription safely.

### ✓ Step 3: Add Flatify-inspired theme resources
Tab visuals use centralized Fuchs tokens and update when the active theme changes.

- Extend `Resources/Styles/FuchsStyles.xaml` with named tab wrapper, header, button/indicator, content, and panel resources.
- Map styling to existing `FuchsTheme` colors, border width, corner radii, padding, typography, and dynamic resource conventions.
- Add only required tab-specific properties/resource mappings to `Theme/FuchsTheme.cs` and `Theme/FuchsThemeProvider.cs`.
- Implement active accent line, inactive opacity, bordered wrapper, optional scrollable header, top indicator, disabled state, and runtime theme refresh.

### ✓ Step 4: Validate tab behavior and build compatibility
The library builds cleanly for the supported Windows target and tab behavior is validated in a MAUI-capable host.

- Run the documented Release build for `net10.0-windows10.0.19041.0` and distinguish existing nullable baseline warnings from new diagnostics.
- Exercise initial selection, bidirectional switching, collection mutations, disabled tabs, long/scrollable headers, and invalid selection inputs.
- Confirm panel child state persists across switches and theme replacement updates visuals without content reconstruction.
- Verify no new source files, XAML inflation, or platform-specific assumptions are required.