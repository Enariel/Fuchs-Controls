using System;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Controls;

namespace FuchsControls.Containers.Responsive;

#region Meta

// FuchsControls
// Created: 29/01/2026
// Modified: 29/01/2026

#endregion

/// <summary>
/// Shows different content based on the current <see cref="DeviceIdiom"/>.
/// Define one or more templates (Phone/Tablet/Desktop). Any other idiom will default
/// to the Desktop template. If the selected template is not provided, the control
/// will fall back to <see cref="FallbackTemplate"/>.
/// </summary>
public class FuchsIdiomView : FuchsResponsiveView
{
	public static readonly BindableProperty MobileTemplateProperty = BindableProperty.Create(
		nameof(MobileTemplate), typeof(DataTemplate), typeof(FuchsIdiomView), default(DataTemplate), propertyChanged: OnTemplateChanged);

	public static readonly BindableProperty TabletTemplateProperty = BindableProperty.Create(
		nameof(TabletTemplate), typeof(DataTemplate), typeof(FuchsIdiomView), default(DataTemplate), propertyChanged: OnTemplateChanged);

	public static readonly BindableProperty DesktopTemplateProperty = BindableProperty.Create(
		nameof(DesktopTemplate), typeof(DataTemplate), typeof(FuchsIdiomView), default(DataTemplate), propertyChanged: OnTemplateChanged);

	public static readonly BindableProperty FallbackTemplateProperty = BindableProperty.Create(
		nameof(FallbackTemplate), typeof(DataTemplate), typeof(FuchsIdiomView), default(DataTemplate), propertyChanged: OnTemplateChanged);

	public DataTemplate? MobileTemplate
	{
		get => (DataTemplate?)GetValue(MobileTemplateProperty);
		set => SetValue(MobileTemplateProperty, value);
	}

	public DataTemplate? TabletTemplate
	{
		get => (DataTemplate?)GetValue(TabletTemplateProperty);
		set => SetValue(TabletTemplateProperty, value);
	}

	public DataTemplate? DesktopTemplate
	{
		get => (DataTemplate?)GetValue(DesktopTemplateProperty);
		set => SetValue(DesktopTemplateProperty, value);
	}

	public DataTemplate? FallbackTemplate
	{
		get => (DataTemplate?)GetValue(FallbackTemplateProperty);
		set => SetValue(FallbackTemplateProperty, value);
	}


	protected override void UpdateContent()
	{
		// Ensure we have a current idiom; on some platforms this can be Unknown very early
		var idiom = DeviceInfo.Current?.Idiom ?? DeviceIdiom.Unknown;

		DataTemplate? template = null;

		if (idiom == DeviceIdiom.Phone)
		{
			template = MobileTemplate ?? TabletTemplate ?? DesktopTemplate ?? FallbackTemplate;
		}
		else if (idiom == DeviceIdiom.Tablet)
		{
			template = TabletTemplate ?? DesktopTemplate ?? FallbackTemplate ?? MobileTemplate;
		}
		else if (idiom == DeviceIdiom.Desktop)
		{
			template = DesktopTemplate ?? FallbackTemplate ?? TabletTemplate ?? MobileTemplate;
		}
		else
		{
			// Unknown/TV/Watch/etc. -> prefer Desktop, then fallbacks
			template = DesktopTemplate ?? FallbackTemplate ?? TabletTemplate ?? MobileTemplate;
		}

		SetContentFromTemplate(template);
	}

	private static void OnTemplateChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsIdiomView view)
			view.RequestUpdate(); // coalesced update
	}
}