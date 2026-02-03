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
/// Defaults align with the SCSS breakpoints defined in Resources/scss/_config.scss
/// (rounded to whole numbers) and can be customized by consumers.
///
/// Ranges (default, rounded):
///  Small:  0    - 767
///  Medium: 768  - 1199
///  Large:  >= 1200
///
/// You may define any subset of templates (Small/Medium/Large). The control will
/// use only the matching bucket. No fallback search is performed.
/// </summary>
public sealed class FuchsBreakpointView : FuchsResponsiveView
{
	// Thresholds define the MAX width for Small/Medium; Large is open-ended
	public static readonly BindableProperty SmallMaxProperty = BindableProperty.Create(
		nameof(SmallMax), typeof(double), typeof(FuchsBreakpointView), 767.0,
		propertyChanged: OnBreakpointChanged,
		coerceValue: CoerceToWholeNumber);

	public static readonly BindableProperty MediumMaxProperty = BindableProperty.Create(
		nameof(MediumMax), typeof(double), typeof(FuchsBreakpointView), 1199.0,
		propertyChanged: OnBreakpointChanged,
		coerceValue: CoerceToWholeNumber);

	// Templates for each breakpoint bucket
	public static readonly BindableProperty SmallTemplateProperty = BindableProperty.Create(
		nameof(SmallTemplate), typeof(DataTemplate), typeof(FuchsBreakpointView), default(DataTemplate), propertyChanged: OnTemplateChanged);

	public static readonly BindableProperty MediumTemplateProperty = BindableProperty.Create(
		nameof(MediumTemplate), typeof(DataTemplate), typeof(FuchsBreakpointView), default(DataTemplate), propertyChanged: OnTemplateChanged);

	public static readonly BindableProperty LargeTemplateProperty = BindableProperty.Create(
		nameof(LargeTemplate), typeof(DataTemplate), typeof(FuchsBreakpointView), default(DataTemplate), propertyChanged: OnTemplateChanged);

	public double SmallMax
	{
		get => (double)GetValue(SmallMaxProperty);
		set => SetValue(SmallMaxProperty, value);
	}

	public double MediumMax
	{
		get => (double)GetValue(MediumMaxProperty);
		set => SetValue(MediumMaxProperty, value);
	}

	public DataTemplate? SmallTemplate
	{
		get => (DataTemplate?)GetValue(SmallTemplateProperty);
		set => SetValue(SmallTemplateProperty, value);
	}

	public DataTemplate? MediumTemplate
	{
		get => (DataTemplate?)GetValue(MediumTemplateProperty);
		set => SetValue(MediumTemplateProperty, value);
	}

	public DataTemplate? LargeTemplate
	{
		get => (DataTemplate?)GetValue(LargeTemplateProperty);
		set => SetValue(LargeTemplateProperty, value);
	}

	protected override void UpdateContent()
	{
		var width = GetCurrentWidth();

		DataTemplate? chosen =
			width <= SmallMax ? SmallTemplate :
			width <= MediumMax ? MediumTemplate :
			LargeTemplate;

		// No fallback: render only the matching bucket.
		SetContentFromTemplate(chosen);
	}

	private static void OnBreakpointChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (Equals(oldValue, newValue))
			return;

		if (bindable is FuchsBreakpointView view)
			view.RequestUpdate();
	}

	private static void OnTemplateChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (ReferenceEquals(oldValue, newValue))
			return;

		if (bindable is FuchsBreakpointView view)
			view.RequestUpdate();
	}

	private static object CoerceToWholeNumber(BindableObject bindable, object value)
	{
		if (value is double d)
			return Math.Round(d);
		try
		{
			return Math.Round(Convert.ToDouble(value));
		}
		catch
		{
			return value;
		}
	}
}