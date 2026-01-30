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
///  XS:   0    - 575
///  SM:  576  - 767
///  MD:  768  - 991
///  LG:  >= 992
///
/// You may define any subset of templates (XS/SM/MD/LG). The control will try the
/// selected breakpoint first, then search upwards to larger breakpoints, then downwards
/// to smaller ones. If no template is provided at all, a <see cref="NullTemplateException"/>
/// is thrown.
/// </summary>
public sealed class FuchsBreakpointView : FuchsResponsiveView
{
	// Thresholds define the MAX width for XS/SM/MD; LG is open-ended
	public static readonly BindableProperty XsMaxProperty = BindableProperty.Create(
		nameof(XsMax), typeof(double), typeof(FuchsBreakpointView), 575.0,
		propertyChanged: OnBreakpointChanged,
		coerceValue: CoerceToWholeNumber);

	public static readonly BindableProperty SmMaxProperty = BindableProperty.Create(
		nameof(SmMax), typeof(double), typeof(FuchsBreakpointView), 767.0,
		propertyChanged: OnBreakpointChanged,
		coerceValue: CoerceToWholeNumber);

	public static readonly BindableProperty MdMaxProperty = BindableProperty.Create(
		nameof(MdMax), typeof(double), typeof(FuchsBreakpointView), 991.0,
		propertyChanged: OnBreakpointChanged,
		coerceValue: CoerceToWholeNumber);

	// Templates for each breakpoint bucket
	public static readonly BindableProperty XsTemplateProperty = BindableProperty.Create(
		nameof(XsTemplate), typeof(DataTemplate), typeof(FuchsBreakpointView), default(DataTemplate), propertyChanged: OnTemplateChanged);

	public static readonly BindableProperty SmTemplateProperty = BindableProperty.Create(
		nameof(SmTemplate), typeof(DataTemplate), typeof(FuchsBreakpointView), default(DataTemplate), propertyChanged: OnTemplateChanged);

	public static readonly BindableProperty MdTemplateProperty = BindableProperty.Create(
		nameof(MdTemplate), typeof(DataTemplate), typeof(FuchsBreakpointView), default(DataTemplate), propertyChanged: OnTemplateChanged);

	public static readonly BindableProperty LgTemplateProperty = BindableProperty.Create(
		nameof(LgTemplate), typeof(DataTemplate), typeof(FuchsBreakpointView), default(DataTemplate), propertyChanged: OnTemplateChanged);

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

	protected override void UpdateContent()
	{
		var width = GetCurrentWidth();

		// Determine the active bucket index (0..3)
		int index = width <= XsMax ? 0 :
			width <= SmMax ? 1 :
			width <= MdMax ? 2 : 3; // LG (open-ended)

		var templates = new DataTemplate?[]
		{
			XsTemplate, SmTemplate, MdTemplate, LgTemplate
		};

		DataTemplate? chosen = null;

		// First try upward from current index to find the next defined template
		for (int i = index; i < templates.Length; i++)
		{
			if (templates[i] != null)
			{
				chosen = templates[i];
				break;
			}
		}

		// If not found upward, try downward
		if (chosen == null)
		{
			for (int i = index - 1; i >= 0; i--)
			{
				if (templates[i] != null)
				{
					chosen = templates[i];
					break;
				}
			}
		}

		if (chosen == null)
			throw new NullTemplateException("No templates were provided for any breakpoint (XS/SM/MD/LG).");

		SetContentFromTemplate(chosen);
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