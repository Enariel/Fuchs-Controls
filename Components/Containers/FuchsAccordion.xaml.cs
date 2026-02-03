#region Meta

// FuchsControls
// Created: 11/01/2026
// Modified: 11/01/2026

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuchsControls.Containers;

public partial class FuchsAccordion : ContentView
{
	public static readonly BindableProperty TitleProperty = BindableProperty.Create(
		nameof(Title), typeof(string), typeof(FuchsAccordion), string.Empty);

	public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(
		nameof(IsExpanded), typeof(bool), typeof(FuchsAccordion), false, propertyChanged: OnIsExpandedChanged);

	public static readonly BindableProperty AccordionContentProperty = BindableProperty.Create(
		nameof(AccordionContent), typeof(View), typeof(FuchsAccordion));

	public static readonly BindableProperty HeaderBackgroundColorProperty = BindableProperty.Create(
		nameof(HeaderBackgroundColor), typeof(Color), typeof(FuchsAccordion), Colors.Transparent);

	public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
		nameof(CornerRadius), typeof(CornerRadius), typeof(FuchsAccordion), new CornerRadius(8));

	public static readonly BindableProperty SpacingProperty = BindableProperty.Create(
		nameof(Spacing), typeof(double), typeof(FuchsAccordion), 10.0);

	public static readonly BindableProperty AccentColorProperty = BindableProperty.Create(
		nameof(AccentColor), typeof(Color), typeof(FuchsAccordion), null);

	public string Title
	{
		get => (string)GetValue(TitleProperty);
		set => SetValue(TitleProperty, value);
	}

	public bool IsExpanded
	{
		get => (bool)GetValue(IsExpandedProperty);
		set => SetValue(IsExpandedProperty, value);
	}

	public View AccordionContent
	{
		get => (View)GetValue(AccordionContentProperty);
		set => SetValue(AccordionContentProperty, value);
	}

	public Color HeaderBackgroundColor
	{
		get => (Color)GetValue(HeaderBackgroundColorProperty);
		set => SetValue(HeaderBackgroundColorProperty, value);
	}

	public CornerRadius CornerRadius
	{
		get => (CornerRadius)GetValue(CornerRadiusProperty);
		set => SetValue(CornerRadiusProperty, value);
	}

	public double Spacing
	{
		get => (double)GetValue(SpacingProperty);
		set => SetValue(SpacingProperty, value);
	}

	public Color? AccentColor
	{
		get => (Color?)GetValue(AccentColorProperty);
		set => SetValue(AccentColorProperty, value);
	}

	public FuchsAccordion()
	{
		InitializeComponent();

		this.SetDynamicResource(CornerRadiusProperty, "FuchsCornerRadius");
		this.SetDynamicResource(SpacingProperty, "FuchsSpacing");
		this.SetDynamicResource(AccentColorProperty, "FuchsAccentColor");
	}

	private void OnHeaderTapped(object sender, EventArgs e)
	{
		IsExpanded = !IsExpanded;
	}

	private static async void OnIsExpandedChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsAccordion accordion)
		{
			await accordion.AnimateExpansion((bool)newValue).ConfigureAwait(true);
		}
	}

	private async Task AnimateExpansion(bool expanded)
	{
		bool reduceMotion = false;
		if (Application.Current?.Resources.TryGetValue("FuchsReduceMotion", out var val) == true && val is bool b)
			reduceMotion = b;

		uint duration = reduceMotion ? 0u : 250u;

		if (expanded)
		{
			ContentContainer.Opacity = 0;
			ContentContainer.TranslationY = reduceMotion ? 0 : -10;
			await Task.WhenAll(
				ContentContainer.FadeTo(1, duration, Easing.CubicOut),
				ContentContainer.TranslateTo(0, 0, duration, Easing.CubicOut),
				ChevronLabel.RotateTo(180, duration, Easing.CubicInOut)
			).ConfigureAwait(true);
		}
		else
		{
			await Task.WhenAll(
				ContentContainer.FadeTo(0, duration, Easing.CubicIn),
				ChevronLabel.RotateTo(0, duration, Easing.CubicInOut)
			).ConfigureAwait(true);
		}
	}
}