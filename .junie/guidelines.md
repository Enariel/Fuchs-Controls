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
    * Include a **Meta region** at the top of new files:
      ```csharp
      #region Meta
      // FuchsControls
      // Created: DD/MM/YYYY
      // Modified: DD/MM/YYYY
      #endregion
      ```
    * Follow standard MAUI/C# naming conventions (PascalCase for classes/methods, camelCase for private fields).
* **Structure**:
    * `Fields/`: Contains custom MAUI controls designed for various forms of input.
    * `Converters/`: Contains IValueConverter implementations.
    * `Components/`: Contains components that are containers for other controls.
    * `Components/Buttons/`: Contains components that are or function as buttons.
    * `Extensions`: Contains extension methods and classes.
    * `Resources`: Contains resources for components (e.g., `Resources/Styles/` contains style information)
* **Tooling**:
    * The project uses `FuchsControls.slnx` for solution management in supported IDEs (e.g., JetBrains Rider, Visual Studio).
* **Resources**:
    * Use https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/?view=net-maui-10.0 and related documentation for reference.
    * Explore community forums and GitHub issues for additional insights and troubleshooting.