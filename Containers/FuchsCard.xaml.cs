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

	public static readonly BindableProperty FooterItemsProperty = BindableProperty.Create(nameof(FooterItems), typeof(IList<View>), typeof(FuchsCard),
		defaultBindingMode: BindingMode.OneWay, defaultValueCreator: _ => new ObservableCollection<View>());

	public static readonly BindableProperty ActionItemsProperty = BindableProperty.Create(nameof(ActionItems), typeof(IList<View>), typeof(FuchsCard),
		defaultBindingMode: BindingMode.OneWay, defaultValueCreator: _ => new ObservableCollection<View>());

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

	public IList<View> FooterItems
	{
		get => (IList<View>)GetValue(FooterItemsProperty);
		set => SetValue(FooterItemsProperty, value);
	}

	public IList<View> ActionItems
	{
		get => (IList<View>)GetValue(ActionItemsProperty);
		set => SetValue(ActionItemsProperty, value);
	}
}