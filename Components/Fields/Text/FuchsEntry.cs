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
		
		var entry = new Entry();
		entry.SetBinding(Entry.KeyboardProperty, new Binding(nameof(Keyboard), source: this));
		entry.SetBinding(Entry.PlaceholderProperty, new Binding(nameof(Placeholder), source: this));
		entry.SetBinding(Entry.MaxLengthProperty, new Binding(nameof(MaxLength), source: this));
		entry.SetBinding(Entry.TextProperty, new Binding(nameof(Text), source: this, mode: BindingMode.TwoWay));
		entry.SetBinding(InputView.IsReadOnlyProperty, new Binding(nameof(IsReadOnly), source: this));

		var labelView = CreateLabelWithHelpToggle();
		var helpView = CreateHelpText();

		var stack = new StackLayout { Spacing = 5 };
		stack.SetBinding(StackLayout.OrientationProperty, new Binding(nameof(Orientation), source: this));
		
		stack.Children.Add(labelView);
		stack.Children.Add(entry);
		stack.Children.Add(helpView);

		ApplyToolTip(stack);
		ApplyAccessibility(entry);

		Content = stack;
	}
}