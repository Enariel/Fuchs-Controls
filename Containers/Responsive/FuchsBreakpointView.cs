using System;
using Microsoft.Maui.Controls;

namespace FuchsControls.Containers.Responsive;

#region Meta

// FuchsControls
// Created: 29/01/2026
// Modified: 29/01/2026

#endregion

/// <summary>
/// Displays content based on available width using responsive breakpoints.
/// Defaults roughly follow common web breakpoints but can be customized.
///
/// Ranges (defaults):
///  XS:   0   - 479
///  SM: 480   - 767
///  MD: 768   - 1023
///  LG: 1024  - 1439
///  XL: 1440  - 1919
///  XXL: >= 1920
/// </summary>
public sealed class FuchsBreakpointView : FuchsResponsiveView
{
	// Thresholds define the MAX width for each bucket except XXL
	public static readonly BindableProperty XsMaxProperty = BindableProperty.Create(
		nameof(XsMax), typeof(double), typeof(FuchsBreakpointView), 479.0, propertyChanged: OnBreakpointChanged);

	public static readonly BindableProperty SmMaxProperty = BindableProperty.Create(
		nameof(SmMax), typeof(double), typeof(FuchsBreakpointView), 767.0, propertyChanged: OnBreakpointChanged);

	public static readonly BindableProperty MdMaxProperty = BindableProperty.Create(
		nameof(MdMax), typeof(double), typeof(FuchsBreakpointView), 1023.0, propertyChanged: OnBreakpointChanged);

	public static readonly BindableProperty LgMaxProperty = BindableProperty.Create(
		nameof(LgMax), typeof(double), typeof(FuchsBreakpointView), 1439.0, propertyChanged: OnBreakpointChanged);

	public static readonly BindableProperty XlMaxProperty = BindableProperty.Create(
		nameof(XlMax), typeof(double), typeof(FuchsBreakpointView), 1919.0, propertyChanged: OnBreakpointChanged);

	// Templates for each breakpoint bucket
	public static readonly BindableProperty XsTemplateProperty = BindableProperty.Create(
		nameof(XsTemplate), typeof(DataTemplate), typeof(FuchsBreakpointView), default(DataTemplate), propertyChanged: OnTemplateChanged);

	public static readonly BindableProperty SmTemplateProperty = BindableProperty.Create(
		nameof(SmTemplate), typeof(DataTemplate), typeof(FuchsBreakpointView), default(DataTemplate), propertyChanged: OnTemplateChanged);

	public static readonly BindableProperty MdTemplateProperty = BindableProperty.Create(
		nameof(MdTemplate), typeof(DataTemplate), typeof(FuchsBreakpointView), default(DataTemplate), propertyChanged: OnTemplateChanged);

	public static readonly BindableProperty LgTemplateProperty = BindableProperty.Create(
		nameof(LgTemplate), typeof(DataTemplate), typeof(FuchsBreakpointView), default(DataTemplate), propertyChanged: OnTemplateChanged);

	public static readonly BindableProperty XlTemplateProperty = BindableProperty.Create(
		nameof(XlTemplate), typeof(DataTemplate), typeof(FuchsBreakpointView), default(DataTemplate), propertyChanged: OnTemplateChanged);

	public static readonly BindableProperty XxlTemplateProperty = BindableProperty.Create(
		nameof(XxlTemplate), typeof(DataTemplate), typeof(FuchsBreakpointView), default(DataTemplate), propertyChanged: OnTemplateChanged);

	public static readonly BindableProperty FallbackTemplateProperty = BindableProperty.Create(
		nameof(FallbackTemplate), typeof(DataTemplate), typeof(FuchsBreakpointView), default(DataTemplate), propertyChanged: OnTemplateChanged);

	public double XsMax
	{
		get => (double)GetValue(XsMaxProperty);
		set => SetValue(XsMaxProperty, value);
	}

	public double SmMax
	{
		get => (double)GetValue(SmMaxProperty);
		set => SetValue(SmMaxProperty, value);
	}

	public double MdMax
	{
		get => (double)GetValue(MdMaxProperty);
		set => SetValue(MdMaxProperty, value);
	}

	public double LgMax
	{
		get => (double)GetValue(LgMaxProperty);
		set => SetValue(LgMaxProperty, value);
	}

	public double XlMax
	{
		get => (double)GetValue(XlMaxProperty);
		set => SetValue(XlMaxProperty, value);
	}

	public DataTemplate? XsTemplate
	{
		get => (DataTemplate?)GetValue(XsTemplateProperty);
		set => SetValue(XsTemplateProperty, value);
	}

	public DataTemplate? SmTemplate
	{
		get => (DataTemplate?)GetValue(SmTemplateProperty);
		set => SetValue(SmTemplateProperty, value);
	}

	public DataTemplate? MdTemplate
	{
		get => (DataTemplate?)GetValue(MdTemplateProperty);
		set => SetValue(MdTemplateProperty, value);
	}

	public DataTemplate? LgTemplate
	{
		get => (DataTemplate?)GetValue(LgTemplateProperty);
		set => SetValue(LgTemplateProperty, value);
	}

	public DataTemplate? XlTemplate
	{
		get => (DataTemplate?)GetValue(XlTemplateProperty);
		set => SetValue(XlTemplateProperty, value);
	}

	public DataTemplate? XxlTemplate
	{
		get => (DataTemplate?)GetValue(XxlTemplateProperty);
		set => SetValue(XxlTemplateProperty, value);
	}

	public DataTemplate? FallbackTemplate
	{
		get => (DataTemplate?)GetValue(FallbackTemplateProperty);
		set => SetValue(FallbackTemplateProperty, value);
	}

	protected override void UpdateContent()
	{
		var width = GetCurrentWidth();
		DataTemplate? template = FallbackTemplate;

		if (width <= XsMax)
			template = XsTemplate ?? FallbackTemplate;
		else if (width <= SmMax)
			template = SmTemplate ?? FallbackTemplate;
		else if (width <= MdMax)
			template = MdTemplate ?? FallbackTemplate;
		else if (width <= LgMax)
			template = LgTemplate ?? FallbackTemplate;
		else if (width <= XlMax)
			template = XlTemplate ?? FallbackTemplate;
		else
			template = XxlTemplate ?? FallbackTemplate;

		SetContentFromTemplate(template);
	}

	private static void OnBreakpointChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsBreakpointView view)
			view.UpdateContent();
	}

	private static void OnTemplateChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsBreakpointView view)
			view.UpdateContent();
	}
}