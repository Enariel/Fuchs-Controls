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
		Margin = new Thickness(2, 5, 2, 5);
		
		var root = this;
		var label = new Label() { FontSize = 16 };
		var picker = new Picker()
		{
			ItemsSource = Enum.GetValues<TValue>().Cast<TValue>().ToList(),
		};
		// Bind the Picker's SelectedItem to our SelectedValue property
		label.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, new Binding(nameof(Label), source: root));
		picker.SetBinding(Picker.SelectedItemProperty, new Binding(nameof(SelectedValue), source: root));

		var stack = new StackLayout() { Spacing = 5, Children = { label, picker }, Orientation = StackOrientation.Vertical };
		
		
#if WINDOWS
		if (!string.IsNullOrEmpty(HelpText))
			ToolTipProperties.SetText(stack, new Binding(nameof(HelpText), source: this));
#else
		var helpText = new Label();
		helpText.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, new Binding(nameof(HelpText), source: this));
		stack.Children.Add(helpText);
#endif
		
		Content = stack;
	}
}