#region Meta

// FuchsControls
// Created: 29/01/2026
// Modified: 29/01/2026

#endregion

using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Maui.Controls;

namespace FuchsControls.Containers;

public partial class FuchsCard : ContentView
{
	public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(FuchsCard), string.Empty);
	public static readonly BindableProperty SubtitleProperty = BindableProperty.Create(nameof(Subtitle), typeof(string), typeof(FuchsCard), string.Empty);
	public static readonly BindableProperty BodyProperty = BindableProperty.Create(nameof(Body), typeof(string), typeof(FuchsCard), string.Empty);

	public static readonly BindableProperty CornerRadiusProperty =
		BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(FuchsCard), new CornerRadius(8));

	public static readonly BindableProperty SpacingProperty = BindableProperty.Create(nameof(Spacing), typeof(double), typeof(FuchsCard), 10.0);
	public static readonly BindableProperty AccentColorProperty = BindableProperty.Create(nameof(AccentColor), typeof(Color), typeof(FuchsCard), null);

	public static readonly BindableProperty FooterItemsProperty =
		BindableProperty.Create(nameof(FooterItems), typeof(View), typeof(FuchsCard), defaultBindingMode: BindingMode.OneWay,
			propertyChanged: OnFooterItemsChanged);

	private static void OnFooterItemsChanged(BindableObject bindable, object oldValue, object newValue)
	{
		// Add content to the FooterItemsGrid
		if (newValue is View newFooterItems && bindable is FuchsCard card)
		{
			card.FooterGrid.Children.Clear();
			card.FooterGrid.Children.Add(newFooterItems);
		}
	}

	public static readonly BindableProperty ActionItemsProperty =
		BindableProperty.Create(nameof(ActionItems), typeof(View), typeof(FuchsCard), defaultBindingMode: BindingMode.OneWay,
			propertyChanged: OnActionItemsChanged);

	private static void OnActionItemsChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (newValue is View newActionItems && bindable is FuchsCard card)
		{
			card.ActionGrid.Children.Clear();
			card.ActionGrid.Children.Add(newActionItems);
		}
	}

	public static readonly BindableProperty ImageSourceProperty =
		BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(FuchsCard), default(ImageSource));

	public FuchsCard()
	{
		InitializeComponent();

		this.SetDynamicResource(CornerRadiusProperty, "FuchsCornerRadius");
		this.SetDynamicResource(SpacingProperty, "FuchsSpacing");
		this.SetDynamicResource(AccentColorProperty, "FuchsAccentColor");
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

	public ImageSource ImageSource
	{
		get => (ImageSource)GetValue(ImageSourceProperty);
		set => SetValue(ImageSourceProperty, value);
	}

	public string Title
	{
		get => (string)GetValue(TitleProperty);
		set => SetValue(TitleProperty, value);
	}

	public static readonly BindableProperty TitleFontSizeProperty = BindableProperty.Create(nameof(TitleFontSize), typeof(double), typeof(FuchsCard), 16.0);

	public static readonly BindableProperty SubTitleFontSizeProperty =
		BindableProperty.Create(nameof(SubTitleFontSize), typeof(double), typeof(FuchsCard), 14.0);

	public static readonly BindableProperty BodyFontSizeProperty = BindableProperty.Create(nameof(BodyFontSize), typeof(double), typeof(FuchsCard), 12.0);

	public double TitleFontSize
	{
		get => (double)GetValue(TitleFontSizeProperty);
		set => SetValue(TitleFontSizeProperty, value);
	}

	public double SubTitleFontSize
	{
		get => (double)GetValue(SubTitleFontSizeProperty);
		set => SetValue(SubTitleFontSizeProperty, value);
	}

	public double BodyFontSize
	{
		get => (double)GetValue(BodyFontSizeProperty);
		set => SetValue(BodyFontSizeProperty, value);
	}

	public string Subtitle
	{
		get => (string)GetValue(SubtitleProperty);
		set => SetValue(SubtitleProperty, value);
	}

	public string Body
	{
		get => (string)GetValue(BodyProperty);
		set => SetValue(BodyProperty, value);
	}

	public View FooterItems
	{
		get => (View)GetValue(FooterItemsProperty);
		set => SetValue(FooterItemsProperty, value);
	}

	public View ActionItems
	{
		get => (View)GetValue(ActionItemsProperty);
		set => SetValue(ActionItemsProperty, value);
	}

	public View TitleActionItems
	{
		get => (View)GetValue(TitleActionItemsProperty);
		set => SetValue(TitleActionItemsProperty, value);
	}

	public static readonly BindableProperty TitleActionItemsProperty =
		BindableProperty.Create(nameof(TitleActionItems), typeof(View), typeof(FuchsCard), defaultBindingMode: BindingMode.OneWay,
			propertyChanged: OnTitleActionItemsChanged);

	private static void OnTitleActionItemsChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (newValue is View newTitleActionItems && bindable is FuchsCard card)
		{
			card.TitleActionGrid.Children.Clear();
			card.TitleActionGrid.Children.Add(newTitleActionItems);
		}
	}
}