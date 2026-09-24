#region Meta
// FuchsControls
// Created: 23/09/2026
// Modified: 23/09/2026
#endregion

using FuchsControls.Handlers;
using FuchsControls.Theme;

namespace FuchsControls;

public static class MauiAppBuilderExtensions
{
	public static MauiAppBuilder UseFuchsControls(this MauiAppBuilder builder, FuchsTheme? theme = null)
	{
		FormHandler.RemoveBorders();

		builder.ConfigureMauiHandlers(handlers =>
		{
			FuchsEntryHandler.Register();
		});

		if (Application.Current is not null)
			FuchsThemeProvider.ApplyTo(Application.Current, theme);

		return builder;
	}
}