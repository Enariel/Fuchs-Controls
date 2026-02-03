#region Meta

// FuchsControls
// Created: 10/01/2026
// Modified: 10/01/2026

#endregion

namespace FuchsControls.Fields;

public class FuchsEditor : TextEditorBase
{
	public FuchsEditor()
	{
		Margin = new Thickness(2, 5, 2, 5);

		var editor = new Editor();
		editor.SetBinding(InputView.KeyboardProperty, new Binding(nameof(Keyboard), source: this));
		editor.SetBinding(Editor.PlaceholderProperty, new Binding(nameof(Placeholder), source: this));
		editor.SetBinding(InputView.MaxLengthProperty, new Binding(nameof(MaxLength), source: this));
		editor.SetBinding(Editor.AutoSizeProperty, new Binding(nameof(AutoSize), source: this));
		editor.SetBinding(Editor.HeightRequestProperty, new Binding(nameof(EditorHeight), source: this));
		editor.SetBinding(Editor.TextProperty, new Binding(nameof(Text), source: this, mode: BindingMode.TwoWay));
		editor.SetBinding(InputView.IsReadOnlyProperty, new Binding(nameof(IsReadOnly), source: this));

		var labelView = CreateLabelWithHelpToggle();
		var helpView = CreateHelpText();

		var stack = new StackLayout { Spacing = 5 };
		stack.SetBinding(StackLayout.OrientationProperty, new Binding(nameof(Orientation), source: this));

		stack.Children.Add(labelView);
		stack.Children.Add(editor);
		stack.Children.Add(helpView);

		ApplyToolTip(stack);
		ApplyAccessibility(editor);

		Content = stack;
	}
}