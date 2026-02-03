## Project Guidelines

### Overview

This project is a MAUI control library that should be able to be used across several projects. Controls in this project should be customizable where
appropriate, but also simple enough to 'plug and play' for users. Controls should also be designed with multiple platforms enabled.

#### Build/Configuration Instructions

The project is a .NET MAUI Class Library targeting .NET 10.0.

1. **Prerequisites**:
    * .NET 10.0 SDK.
    * MAUI Workload (`dotnet workload install maui`).
2. **Building**:
    * Use `dotnet build` from the root directory.
    * Target specific platforms if needed: `dotnet build -f net10.0-android`, `dotnet build -f net10.0-windows10.0.19041.0`, etc.
3. **NuGet Packages**:
    * `CommunityToolkit.Maui` and `CommunityToolkit.Mvvm` are primary dependencies.

#### Testing Information

Currently, the project does not have a dedicated test project in the repository.

#### Additional Development Information

* **Code Style**:
    * Use **Tabs** for indentation.
    * Use **File-scoped namespaces** (e.g., `namespace FuchsControls.Fields;`).
    * Follow standard MAUI/C# naming conventions (PascalCase for classes/methods, camelCase for private fields).
    * Respect nullability annotations and handle potential nulls defensively. 

* **Structure**:
    * `Converters`: Contains IValueConverter implementations.
    * `Components`: Contains general components that can be used across the project.
    * `Components/Fields`: Contains custom MAUI controls designed for various forms of input.
    * `Components/Buttons`: Contains components that are or function as buttons.
    * `Components/Containers`: Contains components that layout other controls or provide spacing/responsiveness.
    * `Extensions`: Contains extension methods and classes.
    * `Resources`: Contains resources for components (e.g., `Resources/Styles/` contains style information)

* **Tooling**:
    * The project uses `FuchsControls.slnx` for solution management in supported IDEs (e.g., JetBrains Rider, Visual Studio).

* **Resources**:
    * Use https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/?view=net-maui-10.0 and related documentation for reference.
    * Use https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/ for additional documentation related to the CommunityToolkit.Maui library.
    * Use https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/ for additional documentation related to the CommunityToolkit.Mvvm library.
    * Explore community forums and GitHub issues for additional insights and troubleshooting.

* **How Junie should be used**:
    * Use the `CommunityToolkit.Maui` and `CommunityToolkit.Mvvm` NuGet packages.
    * Use the `FuchsControls.slnx` solution file for development.
    * Use the existing `CommunityToolkit.Mvvm` helpers and extensions where possible.
    * Use the existing `CommunityToolkit.Maui` helpers and extensions where possible.
    * Use the existing `CommunityToolkit.Maui.Markup` helpers and extensions where possible.
    * Use best practices for .NET MAUI development.
    * Use best practices for async/await.
      * This includes using `await` for asynchronous operations, avoiding blocking calls, and following the async/await pattern consistently.
      * See https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/ for more information.
      * Utilize `ConfigureAwait(false)` where appropriate.
      * Utilize IAsyncEnumerable and IAsyncDisposable where appropriate.
    * Prefer composition to inheritance. 
    * Follow the MAUI community guidelines and best practices.
    * Avoid unrelated edits.
    * Avoid potential memory leaks.
    * Avoid unnecessary complexity.
    * Do not change functionality unless explicitly asked.
    * Do not change SDK versions, frameworks, or platform settings unless explicitly asked.
    * Do not use obsolete APIs (e.g. `Frame` components).
    * Do not introduce new dependencies or libraries without explicit approval.
