#region Meta

// FuchsControls
// Created: 11/01/2026
// Modified: 11/01/2026

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuchsControls.Fields;

public partial class FuchsToggle : BooleanFieldBase, IBooleanField
{
	/// <inheritdoc />
	public bool? IsChecked
	{
		get => (bool?)GetValue(IsCheckedProperty);
		set => SetValue(IsCheckedProperty, value);
	}

	public static readonly BindableProperty OnColorProperty = BindableProperty.Create(
		nameof(OnColor), typeof(Color), typeof(FuchsToggle), Colors.Green);

	public static readonly BindableProperty OffColorProperty = BindableProperty.Create(
		nameof(OffColor), typeof(Color), typeof(FuchsToggle), Colors.LightGray);

	public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
		nameof(IsChecked),
		typeof(bool?),
		typeof(BooleanFieldBase),
		default(bool?), BindingMode.TwoWay, propertyChanged: OnIsCheckedChanged);

	public Color OnColor
	{
		get => (Color)GetValue(OnColorProperty);
		set => SetValue(OnColorProperty, value);
	}

	public Color OffColor
	{
		get => (Color)GetValue(OffColorProperty);
		set => SetValue(OffColorProperty, value);
	}

	public FuchsToggle()
	{
		InitializeComponent();
	}

	private void OnTapped(object sender, TappedEventArgs e) => IsChecked = !IsChecked;

	private static void OnIsCheckedChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsToggle control)
			control.AnimateToggle((bool)newValue);
	}

	private async void AnimateToggle(bool isOn)
	{
		try
		{
			// Smooth color transition
			Track.BackgroundColor = isOn ? OnColor : OffColor;

			// Smooth thumb movement
			// 50 (track) - 22 (thumb) - 6 (margins) = 22px translation
			double targetTranslation = isOn ? 22 : 0;
			await Thumb.TranslateToAsync(targetTranslation, 0, 150, Easing.CubicOut);
		}
		catch (Exception e)
		{
			Debug.WriteLine($"Toggle animation error: {e.Message}");
		}
	}
}