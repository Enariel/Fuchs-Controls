#region Meta

// FuchsControls
// Created: 03/02/2026
// Modified: 03/02/2026

#endregion

using FuchsControls.Handlers;

namespace FuchsControls.Extensions;

public static class AppBuilderExtensions
{
	/// <summary>
	/// Initializes the FuchsControls library.
	/// </summary>
	/// <param name="builder">The MauiAppBuilder.</param>
	/// <returns>The MauiAppBuilder for chaining.</returns>
	public static MauiAppBuilder UseFuchsControls(this MauiAppBuilder builder)
	{
		builder.ConfigureFonts(fonts =>
		{
			fonts.AddFont("AlanSansVariableFont.ttf", "AlanSans");
			fonts.AddFont("ArchivoVariableFont.ttf", "Archivo");
		});
		
		// Enable borderless controls so Fuchs wrappers can provide their own borders
		FormHandler.RemoveBorders();

		return builder;
	}
}