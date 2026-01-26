#region Meta

// FuchsControls
// Created: 26/01/2026
// Modified: 26/01/2026

#endregion

using System.Diagnostics;
using CheckBox = Microsoft.Maui.Controls.CheckBox;

namespace FuchsControls.Fields;

public class FuchsCheck : BooleanFieldBase, IBooleanField
{
	public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
		nameof(IsChecked),
		typeof(bool),
		typeof(FuchsCheck),
		false, BindingMode.TwoWay, propertyChanged: OnCheckedChanged);

	private static void OnCheckedChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsCheck checkControl)
		{
			Debug.WriteLine($"Check value changed: {checkControl.IsChecked}");
			checkControl.CoerceValue(HelpTextProperty);
		}
	}

	/// <inheritdoc />
	public bool IsChecked
	{
		get => (bool)GetValue(IsCheckedProperty);
		set => SetValue(IsCheckedProperty, value);
	}
	
	public FuchsCheck()
	{
		Margin = new Thickness(2, 5, 2, 5);

		var label = new Label() { FontSize = 16 };
		var check = new CheckBox();

		label.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, new Binding(nameof(Label), source: this, mode: BindingMode.OneWay));
		check.SetBinding(CheckBox.IsCheckedProperty, new Binding(nameof(IsChecked), source: this, mode: BindingMode.TwoWay));
		
		var layout = new VerticalStackLayout
		{
			Spacing = 5,
			Children =
			{
				label,
				check,
			}
		};

#if WINDOWS
		if (!string.IsNullOrEmpty(HelpText))
			ToolTipProperties.SetText(layout, new Binding(nameof(HelpText), source: this));
#else
		var helpText = new Label() { FontSize = 12 };
		helpText.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, new Binding(nameof(HelpText), source: this, mode: BindingMode.TwoWay));
		layout.Children.Add(helpText);
#endif
		Content = layout;
	}
}
