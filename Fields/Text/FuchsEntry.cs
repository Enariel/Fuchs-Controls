#region Meta

// FuchsControls
// Created: 10/01/2026
// Modified: 10/01/2026

#endregion

namespace FuchsControls.Fields;

public class FuchsEntry : TextFieldBase, ITextField
{
	public FuchsEntry()
	{
		Margin = new Thickness(2, 5, 2, 5);
		var stack = new StackLayout() { Spacing = 5 };
		var label = new Label() { FontSize = 16 };
		var editor = new Entry();
		label.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, new Binding(nameof(Label), source: this, mode: BindingMode.OneWay));

		editor.SetBinding(Entry.KeyboardProperty, new Binding(nameof(Keyboard), source: this, mode: BindingMode.OneWay));
		editor.SetBinding(Entry.ReturnTypeProperty, new Binding(nameof(ReturnType), source: this, mode: BindingMode.OneWay));
		editor.SetBinding(PlaceholderProperty, new Binding(nameof(Placeholder), source: this, mode: BindingMode.OneWay));
		editor.SetBinding(MaxLengthProperty, new Binding(nameof(MaxLength), source: this, mode: BindingMode.OneWay));
		editor.SetBinding(Entry.TextProperty, new Binding(nameof(Text), source: this, mode: BindingMode.TwoWay));
		editor.SetBinding(InputView.IsReadOnlyProperty, new Binding(nameof(IsReadOnly), source: this, mode: BindingMode.TwoWay));

		stack.SetBinding(StackLayout.OrientationProperty, new Binding(nameof(Orientation), source: this, mode: BindingMode.OneWay));
		stack.Children.Add(label);
		stack.Children.Add(editor);
		
		
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