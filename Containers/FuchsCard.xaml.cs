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
	public static readonly BindableProperty BodyProperty = BindableProperty.Create(nameof(Body), typeof(string), typeof(FuchsCard), default(string));
	public static readonly BindableProperty SubtitleProperty = BindableProperty.Create(nameof(Subtitle), typeof(string), typeof(FuchsCard), default(string));
	public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(FuchsCard), default(string));

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
}