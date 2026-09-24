namespace FuchsControls.Controls;

public class FuchsEditor : FuchsFieldControl
{
	public static readonly BindableProperty EditorHeightProperty =
		BindableProperty.Create(nameof(EditorHeight), typeof(double), typeof(FuchsEditor), -1d, propertyChanged: OnEditorVisualPropertyChanged);

	public static readonly BindableProperty AutoExpandProperty =
		BindableProperty.Create(nameof(AutoExpand), typeof(bool), typeof(FuchsEditor), false, propertyChanged: OnEditorVisualPropertyChanged);

	public double EditorHeight
	{
		get => (double)GetValue(EditorHeightProperty);
		set => SetValue(EditorHeightProperty, value);
	}

	public bool AutoExpand
	{
		get => (bool)GetValue(AutoExpandProperty);
		set => SetValue(AutoExpandProperty, value);
	}

	public FuchsEditor()
	{
		InitializeField(new Editor
		{
			BackgroundColor = Colors.Transparent
		});
		ApplyEditorHeight();
	}

	protected override void OnTextChanged(string text)
	{
		if (AutoExpand)
		{
			int lineCount = Math.Max(1, text.Split('\n').Length);
			HeightRequest = Math.Max(EditorHeight, lineCount * 24 + 28);
		}
	}

	protected override void ApplyTheme()
	{
		base.ApplyTheme();
		ApplyEditorHeight();
	}

	private static void OnEditorVisualPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsEditor editor)
		{
			editor.ApplyTheme();
			editor.ApplyEditorHeight();
		}
	}

	private void ApplyEditorHeight()
	{
		if (Input is Editor editor)
			editor.HeightRequest = EditorHeight;

		if (AutoExpand)
			OnTextChanged(Text);
		else
			HeightRequest = EditorHeight;
	}
}