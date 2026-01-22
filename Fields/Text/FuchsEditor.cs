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
		var stack = new StackLayout
		{
			Spacing = 5
		};
		
		var label = new Label() { FontSize = 16 };
		label.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, new Binding(nameof(Label), source: this, mode: BindingMode.OneWay));

		var editor = new Editor();
		editor.SetBinding(InputView.KeyboardProperty, new Binding(nameof(Keyboard), source: this, mode: BindingMode.OneWay));
		editor.SetBinding(Editor.PlaceholderProperty, new Binding(nameof(Placeholder), source: this, mode: BindingMode.OneWay));
		editor.SetBinding(InputView.MaxLengthProperty, new Binding(nameof(MaxLength), source: this, mode: BindingMode.OneWay));
		editor.SetBinding(Editor.AutoSizeProperty, new Binding(nameof(AutoSize), source: this, mode: BindingMode.OneWay));
		editor.SetBinding(Editor.HeightRequestProperty, new Binding(nameof(EditorHeight), source: this, mode: BindingMode.OneWay));
		editor.SetBinding(Editor.TextProperty, new Binding(nameof(Text), source: this, mode: BindingMode.TwoWay));

		stack.SetBinding(StackLayout.OrientationProperty, new Binding(nameof(Orientation), source: this, mode: BindingMode.OneWay));
		stack.Children.Add(label);
		stack.Children.Add(editor);

#if WINDOWS
		ToolTipProperties.SetText(
			stack,
			new Binding(
				nameof(HelpText),
				source: this,
				converter: new FuchsControls.EmptyStringToNullConverter()));
#else
		var helpText = new Label();
		helpText.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, new Binding(nameof(HelpText), source: this));
		stack.Children.Add(helpText);
#endif
		Content = stack;
	}
}