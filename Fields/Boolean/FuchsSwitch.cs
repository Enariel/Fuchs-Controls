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
		typeof(bool),
		typeof(FuchsSwitch),
		false, BindingMode.TwoWay, propertyChanged: OnCheckedChanged);

	private static void OnCheckedChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsSwitch switchControl)
		{
			Debug.WriteLine($"Switch value changed: {switchControl.IsChecked}");
			switchControl.CoerceValue(HelpTextProperty);
		}
	}

	/// <inheritdoc />
	public bool IsChecked
	{
		get => (bool)GetValue(IsCheckedProperty);
		set => SetValue(IsCheckedProperty, value);
	}
	
	public FuchsSwitch()
	{
		Margin = new Thickness(0, 5, 0, 5);

		var label = new Label() { FontSize = 16 };
		var helpText = new Label() { FontSize = 12 };
		var toggle = new Switch();

		label.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, new Binding(nameof(Label), source: this, mode: BindingMode.OneWay));
		helpText.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, new Binding(nameof(HelpText), source: this, mode: BindingMode.TwoWay));
		toggle.SetBinding(Switch.IsToggledProperty, new Binding(nameof(IsChecked), source: this, mode: BindingMode.TwoWay));
		
		var layout = new VerticalStackLayout
		{
			Spacing = 5, Children =
			{
				label,
				toggle,
				helpText
			}
		};

		Content = layout;
	}
}