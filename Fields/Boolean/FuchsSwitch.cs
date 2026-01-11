#region Meta

// FuchsControls
// Created: 11/01/2026
// Modified: 11/01/2026

#endregion

namespace FuchsControls.Fields;

public class FuchsSwitch : BooleanFieldBase, IBooleanField
{
	public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
		nameof(IsChecked),
		typeof(bool?),
		typeof(FuchsSwitch),
		default(bool?));

	/// <inheritdoc />
	public bool? IsChecked
	{
		get => (bool?)GetValue(IsCheckedProperty);
		set => SetValue(IsCheckedProperty, value);
	}

	public FuchsSwitch()
	{
		var label = new Label();
		var toggle = new Switch();
		var stack = new StackLayout() { Orientation = StackOrientation.Horizontal, Spacing = 5 };

		label.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, nameof(Label));
		toggle.SetBinding(Switch.IsToggledProperty, nameof(IsChecked), BindingMode.TwoWay);
		
		stack.Children.Add(label);
		stack.Children.Add(toggle);
		Content = stack;
	}
}