#region Meta
// FuchsControls
// Created: 23/09/2026
// Modified: 23/09/2026
#endregion

using FuchsControls.Handlers;
using Microsoft.Maui.Hosting;

namespace FuchsControls;

public static class MauiAppBuilderExtensions
{
	public static MauiAppBuilder UseFuchsControls(this MauiAppBuilder builder)
	{
		builder.ConfigureMauiHandlers(handlers =>
		{
			FormHandler.RemoveBorders();
		});

		return builder;
	}
}