#region Meta

// FuchsControls
// Created: 10/01/2026
// Modified: 10/01/2026

#endregion

namespace FuchsControls.Fields;

public class FuchsEnumSelect<TValue> : SelectFieldBase<TValue> where TValue : struct, Enum
{
	public FuchsEnumSelect()
	{
		Margin = new Thickness(0, 5, 0, 5);
		
		var root = this;
		var label = new Label() { FontSize = 16 };
		var picker = new Picker()
		{
			ItemsSource = Enum.GetValues<TValue>().Cast<TValue>().ToList(),
		};
		// Bind the Picker's SelectedItem to our SelectedValue property
		label.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, new Binding(nameof(Label), source: root));
		picker.SetBinding(Picker.SelectedItemProperty, new Binding(nameof(SelectedValue), source: root));

		var stack = new StackLayout() { Spacing = 5, Children = { label, picker } };
		Content = stack;
	}
}