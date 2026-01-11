#region Meta

// FuchsControls
// Created: 11/01/2026
// Modified: 11/01/2026

#endregion

using System.Diagnostics;
using Switch = Microsoft.Maui.Controls.Switch;

namespace FuchsControls.Fields;

public class FuchsSwitch : BooleanFieldBase, IBooleanField
{
	public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
		nameof(IsChecked),
		typeof(bool?),
		typeof(FuchsSwitch),
		false, BindingMode.TwoWay, propertyChanged: OnCheckedChanged);

	private static void OnCheckedChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsSwitch switchControl)
		{
			Debug.WriteLine($"Switch value changed: {switchControl.IsChecked}");
		}
	}

	/// <inheritdoc />
	public bool? IsChecked
	{
		get => (bool?)GetValue(IsCheckedProperty);
		set => SetValue(IsCheckedProperty, value);
	}

	public static readonly BindableProperty OnTextProperty = BindableProperty.Create(
		nameof(OnText),
		typeof(string),
		typeof(FuchsSwitch),
		"On");

	public static readonly BindableProperty OffTextProperty = BindableProperty.Create(
		nameof(OffText),
		typeof(string),
		typeof(FuchsSwitch),
		"Off");

	public string OnText
	{
		get => (string)GetValue(OnTextProperty);
		set => SetValue(OnTextProperty, value);
	}

	public string OffText
	{
		get => (string)GetValue(OffTextProperty);
		set => SetValue(OffTextProperty, value);
	}

	public FuchsSwitch()
	{
		Margin = new Thickness(0, 5, 0, 5);

		var label = new Label() { FontSize = 16 };
		var helpText = new Label() { FontSize = 12 };
		var toggle = new Switch();
		var grid = new Grid()
		{
			RowSpacing = 5,
			ColumnSpacing = 5,
			ColumnDefinitions =
			{
				new ColumnDefinition { Width = GridLength.Star },
				new ColumnDefinition { Width = GridLength.Star }
			},
			RowDefinitions =
			[
				new RowDefinition { Height = GridLength.Auto },
				new RowDefinition { Height = GridLength.Auto },
			]
		};

		label.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, new Binding(nameof(Label), source: this, mode: BindingMode.OneWay));
		helpText.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, new Binding(nameof(HelpText), source: this, mode: BindingMode.TwoWay));
		toggle.SetBinding(Switch.IsToggledProperty, new Binding(nameof(IsChecked), source: this, mode: BindingMode.TwoWay));

		grid.Add(label, 0, 0);
		grid.Add(toggle, 1, 0);
		grid.AddWithSpan(helpText, 1, 0, columnSpan: 1);
		Content = grid;
	}
}