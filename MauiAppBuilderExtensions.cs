#region Meta
// FuchsControls
// Created: 23/09/2026
// Modified: 23/09/2026
#endregion

using FuchsControls.Handlers;
using FuchsControls.Resources.Styles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
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

		var applicationDescriptor = builder.Services.LastOrDefault(descriptor => descriptor.ServiceType == typeof(IApplication));
		if (applicationDescriptor is not null)
		{
			builder.Services.Remove(applicationDescriptor);
			builder.Services.Add(new ServiceDescriptor(typeof(IApplication), serviceProvider =>
			{
				var application = applicationDescriptor.ImplementationInstance
					?? applicationDescriptor.ImplementationFactory?.Invoke(serviceProvider)
					?? (applicationDescriptor.ImplementationType is { } implementationType
						? ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, implementationType)
						: throw new InvalidOperationException("The MAUI application service could not be created."));

				if (application is Application mauiApplication && !mauiApplication.Resources.MergedDictionaries.OfType<FuchsStyles>().Any())
					mauiApplication.Resources.MergedDictionaries.Add(new FuchsStyles());

				if (application is Application initializedApplication)
					FuchsThemeManager.Initialize(initializedApplication);

				return application;
			}, applicationDescriptor.Lifetime));
		}

		return builder;
	}
}