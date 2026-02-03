#region Meta
// FuchsControls
// Created: 10/01/2026
// Modified: 10/01/2026
#endregion

namespace FuchsControls.Fields;

public abstract class TextEditorBase : TextFieldBase, ITextEditor
{
	public static readonly BindableProperty EditorHeightProperty = BindableProperty.Create
	(
		nameof(EditorHeight),
		typeof(double),
		typeof(TextEditorBase),
		150D
	);

	public static readonly BindableProperty AutoSizeProperty = BindableProperty.Create
	(
		nameof(AutoSize),
		typeof(EditorAutoSizeOption),
		typeof(TextEditorBase),
		EditorAutoSizeOption.Disabled
	);

	public EditorAutoSizeOption AutoSize
	{
		get => (EditorAutoSizeOption)GetValue(AutoSizeProperty);
		set => SetValue(AutoSizeProperty, value);
	}

	public double EditorHeight
	{
		get => (double)GetValue(EditorHeightProperty);
		set => SetValue(EditorHeightProperty, value);
	}
}