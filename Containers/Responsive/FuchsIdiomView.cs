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
/// Define one or more templates (Phone/Tablet/Desktop/TV/Watch). If a specific
/// template is not provided, the control will fall back to <see cref="FallbackTemplate"/>.
/// </summary>
public sealed class FuchsIdiomView : FuchsResponsiveView
{
	public static readonly BindableProperty PhoneTemplateProperty = BindableProperty.Create(
		nameof(PhoneTemplate), typeof(DataTemplate), typeof(FuchsIdiomView), default(DataTemplate), propertyChanged: OnTemplateChanged);

	public static readonly BindableProperty TabletTemplateProperty = BindableProperty.Create(
		nameof(TabletTemplate), typeof(DataTemplate), typeof(FuchsIdiomView), default(DataTemplate), propertyChanged: OnTemplateChanged);

	public static readonly BindableProperty DesktopTemplateProperty = BindableProperty.Create(
		nameof(DesktopTemplate), typeof(DataTemplate), typeof(FuchsIdiomView), default(DataTemplate), propertyChanged: OnTemplateChanged);

	public static readonly BindableProperty TVTemplateProperty = BindableProperty.Create(
		nameof(TVTemplate), typeof(DataTemplate), typeof(FuchsIdiomView), default(DataTemplate), propertyChanged: OnTemplateChanged);

	public static readonly BindableProperty WatchTemplateProperty = BindableProperty.Create(
		nameof(WatchTemplate), typeof(DataTemplate), typeof(FuchsIdiomView), default(DataTemplate), propertyChanged: OnTemplateChanged);

	public static readonly BindableProperty FallbackTemplateProperty = BindableProperty.Create(
		nameof(FallbackTemplate), typeof(DataTemplate), typeof(FuchsIdiomView), default(DataTemplate), propertyChanged: OnTemplateChanged);

	public DataTemplate? PhoneTemplate
	{
		get => (DataTemplate?)GetValue(PhoneTemplateProperty);
		set => SetValue(PhoneTemplateProperty, value);
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

	public DataTemplate? TVTemplate
	{
		get => (DataTemplate?)GetValue(TVTemplateProperty);
		set => SetValue(TVTemplateProperty, value);
	}

	public DataTemplate? WatchTemplate
	{
		get => (DataTemplate?)GetValue(WatchTemplateProperty);
		set => SetValue(WatchTemplateProperty, value);
	}

	public DataTemplate? FallbackTemplate
	{
		get => (DataTemplate?)GetValue(FallbackTemplateProperty);
		set => SetValue(FallbackTemplateProperty, value);
	}

	protected override void UpdateContent()
	{
		var idiom = DeviceInfo.Current.Idiom;
		DataTemplate? template = FallbackTemplate;
		if (idiom == DeviceIdiom.Phone)
			template = PhoneTemplate ?? FallbackTemplate;
		else if (idiom == DeviceIdiom.Tablet)
			template = TabletTemplate ?? FallbackTemplate;
		else if (idiom == DeviceIdiom.Desktop)
			template = DesktopTemplate ?? FallbackTemplate;
		else if (idiom == DeviceIdiom.TV)
			template = TVTemplate ?? FallbackTemplate;
		else if (idiom == DeviceIdiom.Watch)
			template = WatchTemplate ?? FallbackTemplate;

		SetContentFromTemplate(template);
	}

	private static void OnTemplateChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsIdiomView view)
		{
			view.UpdateContent();
		}
	}
}