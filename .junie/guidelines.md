### Project Guidelines

#### Build/Configuration Instructions
The project is a .NET MAUI Class Library targeting .NET 10.0.

1.  **Prerequisites**:
    *   .NET 10.0 SDK.
    *   MAUI Workload (`dotnet workload install maui`).
2.  **Building**:
    *   Use `dotnet build` from the root directory.
    *   Target specific platforms if needed: `dotnet build -f net10.0-android`, `dotnet build -f net10.0-windows10.0.19041.0`, etc.
3.  **NuGet Packages**:
    *   `CommunityToolkit.Maui` and `CommunityToolkit.Mvvm` are primary dependencies.

#### Testing Information
Currently, the project does not have a dedicated test project in the repository.

#### Additional Development Information
*   **Code Style**:
    *   Use **Tabs** for indentation.
    *   Use **File-scoped namespaces** (e.g., `namespace FuchsControls.Fields;`).
    *   Include a **Meta region** at the top of new files:
        ```csharp
        #region Meta
        // FuchsControls
        // Created: DD/MM/YYYY
        // Modified: DD/MM/YYYY
        #endregion
        ```
    *   Follow standard MAUI/C# naming conventions (PascalCase for classes/methods, camelCase for private fields).
*   **Structure**:
    *   `Fields/`: Contains custom MAUI controls.
    *   `Converters/`: Contains IValueConverter implementations.
    *   `Components/`: Contains smaller UI components.
*   **Tooling**:
    *   The project uses `FuchsControls.slnx` for solution management in supported IDEs (e.g., JetBrains Rider, Visual Studio).
