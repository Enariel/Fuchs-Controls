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
		var stack = new StackLayout
		{
			Orientation = GetOrientation(),
			Spacing = 5
		};

		var label = new Label();
		label.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, new Binding(nameof(Label), source: this, mode: BindingMode.OneWay));

		var editor = new Editor();
		editor.SetBinding(InputView.KeyboardProperty, new Binding(nameof(Keyboard), source: this, mode: BindingMode.OneWay));
		editor.SetBinding(Editor.PlaceholderProperty, new Binding(nameof(Placeholder), source: this, mode: BindingMode.OneWay));
		editor.SetBinding(InputView.MaxLengthProperty, new Binding(nameof(MaxLength), source: this, mode: BindingMode.OneWay));
		editor.SetBinding(Editor.AutoSizeProperty, new Binding(nameof(AutoSize), source: this, mode: BindingMode.OneWay));
		editor.SetBinding(Editor.HeightRequestProperty, new Binding(nameof(EditorHeight), source: this, mode: BindingMode.OneWay));
		editor.SetBinding(Editor.TextProperty, new Binding(nameof(Text), source: this, mode: BindingMode.TwoWay));
		

		stack.Children.Add(label);
		
#if WINDOWS
        if (!string.IsNullOrEmpty(HelpText))
            ToolTipProperties.SetText(label, new Binding(nameof(HelpText), source: this));
#else
		var helpText = new Label();
		helpText.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, new Binding(nameof(HelpText), source: this));
		stack.Children.Add(helpText);
#endif
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