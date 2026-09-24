---
sessionId: session-260923-231439-18be
---

# Requirements

### Overview & Goals
Modernize the animation invocations in `PressedScaleBehavior` by replacing obsolete .NET MAUI `ScaleTo` and `TranslateTo` extension methods with their modern .NET 10 async counterparts (`ScaleToAsync` and `TranslateToAsync`), eliminating `CS0618` compiler warnings while preserving identical visual behavior and animation parameters.

### Scope
- **In Scope**:
  - Updating `Behaviours/PressedScaleBehavior.cs` to call `ScaleToAsync` and `TranslateToAsync` in `OnPointerPressed` and `OnPointerReleased`.
  - Verifying that the project compiles cleanly without obsolete API warnings for behaviors.
- **Out of Scope**:
  - Changing animation durations, easing curves, or property names.
  - Modifying converter nullability warnings (`CS8767`) or unrelated components.

### User Stories
- As a developer consuming `FuchsControls`, I want the library to use modern .NET 10 MAUI animation APIs so that builds do not emit deprecation warnings and benefit from up-to-date framework lifecycle handling.

### Functional Requirements
- `PressedScaleBehavior.OnPointerPressed` must trigger scaling to `PressedScale` and translating Y to `PressedTranslationY` over 80ms with `Easing.CubicOut` using `ScaleToAsync` and `TranslateToAsync`.
- `PressedScaleBehavior.OnPointerReleased` must trigger scaling back to `1.0` and translating Y back to `0.0` over 100ms with `Easing.CubicOut` using `ScaleToAsync` and `TranslateToAsync`.
- Both animations in each gesture handler must execute concurrently via `Task.WhenAll`.

### Non-Functional Requirements
- **Compatibility**: Ensure full compatibility with .NET 10 MAUI and source-generated XAML.
- **Code Cleanliness**: Zero `CS0618` obsolete API warnings originating from `PressedScaleBehavior.cs`.

# Technical Design

### Current Implementation
In `Behaviours/PressedScaleBehavior.cs`, pointer events trigger animations using `ViewExtensions.ScaleTo` and `ViewExtensions.TranslateTo`:
```csharp
private async void OnPointerPressed(object? sender, PointerEventArgs e)
{
    if (sender is not VisualElement view || !view.IsEnabled)
        return;

    await Task.WhenAll(
        view.ScaleTo(PressedScale, 80, Easing.CubicOut),
        view.TranslateTo(0, PressedTranslationY, 80, Easing.CubicOut));
}

private async void OnPointerReleased(object? sender, PointerEventArgs e)
{
    if (sender is not VisualElement view)
        return;

    await Task.WhenAll(
        view.ScaleTo(1, 100, Easing.CubicOut),
        view.TranslateTo(0, 0, 100, Easing.CubicOut));
}
```
In .NET 10 MAUI, `ScaleTo` and `TranslateTo` are marked `[Obsolete("Please use ScaleToAsync instead.")]` and `[Obsolete("Please use TranslateToAsync instead.")]`, producing `CS0618` compiler warnings during build.

### Key Decisions
- **Adopt `ScaleToAsync` and `TranslateToAsync`**: Use `Microsoft.Maui.Controls.ViewExtensions.ScaleToAsync` and `TranslateToAsync`, which provide the direct, non-obsolete replacements returning `Task<bool>`.
- **Maintain parameter configuration**: Retain existing easing (`Easing.CubicOut`), durations (80ms for press, 100ms for release), and target dimensions (`PressedScale`, `PressedTranslationY`, `1`, `0`).

### Proposed Changes
Update `Behaviours/PressedScaleBehavior.cs`:
```csharp
private async void OnPointerPressed(object? sender, PointerEventArgs e)
{
    if (sender is not VisualElement view || !view.IsEnabled)
        return;

    await Task.WhenAll(
        view.ScaleToAsync(PressedScale, 80, Easing.CubicOut),
        view.TranslateToAsync(0, PressedTranslationY, 80, Easing.CubicOut));
}

private async void OnPointerReleased(object? sender, PointerEventArgs e)
{
    if (sender is not VisualElement view)
        return;

    await Task.WhenAll(
        view.ScaleToAsync(1, 100, Easing.CubicOut),
        view.TranslateToAsync(0, 0, 100, Easing.CubicOut));
}
```

### Components
- `FuchsControls.Behaviours.PressedScaleBehavior`: MAUI behavior adding smooth pressed state scaling and translation to any `View`.

### File Structure
- `Behaviours/PressedScaleBehavior.cs` (modified)

### Risks & Mitigations
- **Risk**: Potential differences in async task completion behavior.
- **Mitigation**: `ScaleToAsync` and `TranslateToAsync` are direct API replacements returning `Task<bool>`, awaited in parallel by `Task.WhenAll` as before.

# Testing

### Validation Approach
- Perform build validation with `dotnet build` targeting `net10.0-windows10.0.19041.0`.
- Verify the compiler warning log to ensure all `CS0618` obsolete warnings for `ScaleTo` and `TranslateTo` in `PressedScaleBehavior.cs` have been eliminated.

### Key Scenarios
- **Build Cleanliness**: Ensure `PressedScaleBehavior.cs` compiles with 0 errors and 0 CS0618 warnings.
- **API Signature Compatibility**: Ensure `ScaleToAsync` and `TranslateToAsync` accept identical duration, easing, and coordinate parameters.

# Delivery Steps

### ✓ Step 1: Update pressed state animation to modern async methods
`PressedScaleBehavior.OnPointerPressed` uses modern `ScaleToAsync` and `TranslateToAsync` animation methods without compiler warnings.

- Replace `view.ScaleTo(PressedScale, 80, Easing.CubicOut)` with `view.ScaleToAsync(PressedScale, 80, Easing.CubicOut)` in `OnPointerPressed` within `Behaviours/PressedScaleBehavior.cs`.
- Replace `view.TranslateTo(0, PressedTranslationY, 80, Easing.CubicOut)` with `view.TranslateToAsync(0, PressedTranslationY, 80, Easing.CubicOut)` in `OnPointerPressed` within `Behaviours/PressedScaleBehavior.cs`.
- Ensure `Task.WhenAll` concurrently awaits both async animation tasks.

### ✓ Step 2: Update released state animation and validate build
`PressedScaleBehavior.OnPointerReleased` uses modern `ScaleToAsync` and `TranslateToAsync` APIs, and the project builds cleanly without obsolete warnings in behaviors.

- Replace `view.ScaleTo(1, 100, Easing.CubicOut)` with `view.ScaleToAsync(1, 100, Easing.CubicOut)` in `OnPointerReleased` within `Behaviours/PressedScaleBehavior.cs`.
- Replace `view.TranslateTo(0, 0, 100, Easing.CubicOut)` with `view.TranslateToAsync(0, 0, 100, Easing.CubicOut)` in `OnPointerReleased` within `Behaviours/PressedScaleBehavior.cs`.
- Validate the solution build via `dotnet build` targeting `net10.0-windows10.0.19041.0` and confirm CS0618 warnings are eliminated.