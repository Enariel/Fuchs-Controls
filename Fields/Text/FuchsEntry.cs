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
		var stack = new StackLayout() { Spacing = 5, Orientation = GetOrientation() };
		var label = new Label();
		var editor = new Entry();
		
		label.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, new Binding(nameof(Label), source: this, mode: BindingMode.OneWay));
		
		editor.SetBinding(Entry.KeyboardProperty, new Binding(nameof(Keyboard), source: this, mode: BindingMode.OneWay));
		editor.SetBinding(Entry.ReturnTypeProperty, new Binding(nameof(ReturnType), source: this, mode: BindingMode.OneWay));
		editor.SetBinding(PlaceholderProperty, new Binding(nameof(Placeholder), source: this, mode: BindingMode.OneWay));
		editor.SetBinding(MaxLengthProperty, new Binding(nameof(MaxLength), source: this, mode: BindingMode.OneWay));
		editor.SetBinding(Entry.TextProperty, new Binding(nameof(Text), source: this, mode: BindingMode.TwoWay));
		editor.SetBinding(InputView.IsReadOnlyProperty, new Binding(nameof(IsReadOnly), source: this, mode: BindingMode.TwoWay));

		stack.Children.Add(label);
		stack.Children.Add(editor);
		
		Content = stack;
	}

	private StackOrientation GetOrientation()
	{
		switch (Orientation)
		{
			case FieldOrientation.Horizontal:
				return StackOrientation.Horizontal;
			case FieldOrientation.Vertical:
				return StackOrientation.Vertical;
			default:
				return StackOrientation.Vertical;
		}
	}
}