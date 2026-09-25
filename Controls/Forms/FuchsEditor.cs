namespace FuchsControls;

public sealed class FuchsEditor : FuchsFieldBase
{
	private readonly Editor _editor = new Editor().ApplyFuchsEditorStyle();

	public static readonly BindableProperty ValueProperty = BindableProperty.Create(
		nameof(Value), typeof(string), typeof(FuchsEditor), string.Empty, BindingMode.TwoWay, propertyChanged: OnValueChanged);

	public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
		nameof(Placeholder), typeof(string), typeof(FuchsEditor), string.Empty, propertyChanged: OnEditorPropertyChanged);

	public static readonly BindableProperty KeyboardProperty = BindableProperty.Create(
		nameof(Keyboard), typeof(Keyboard), typeof(FuchsEditor), Keyboard.Default, propertyChanged: OnEditorPropertyChanged);

	public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(
		nameof(MaxLength), typeof(int), typeof(FuchsEditor), int.MaxValue, propertyChanged: OnEditorPropertyChanged);

	public static readonly BindableProperty IsReadOnlyProperty = BindableProperty.Create(
		nameof(IsReadOnly), typeof(bool), typeof(FuchsEditor), false, propertyChanged: OnEditorPropertyChanged);

	public FuchsEditor()
	{
		_editor.TextChanged += OnTextChanged;
		SetInput(_editor);
		UpdateEditor();
	}

	public string Value
	{
		get => (string)GetValue(ValueProperty);
		set => SetValue(ValueProperty, value);
	}

	public string Placeholder
	{
		get => (string)GetValue(PlaceholderProperty);
		set => SetValue(PlaceholderProperty, value);
	}

	public Keyboard Keyboard
	{
		get => (Keyboard)GetValue(KeyboardProperty);
		set => SetValue(KeyboardProperty, value);
	}

	public int MaxLength
	{
		get => (int)GetValue(MaxLengthProperty);
		set => SetValue(MaxLengthProperty, value);
	}

	public bool IsReadOnly
	{
		get => (bool)GetValue(IsReadOnlyProperty);
		set => SetValue(IsReadOnlyProperty, value);
	}

	private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsEditor)bindable)._editor.Text = (string)newValue;
	private static void OnEditorPropertyChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsEditor)bindable).UpdateEditor();
	private void OnTextChanged(object? sender, TextChangedEventArgs e) => SetValue(ValueProperty, e.NewTextValue ?? string.Empty);

	private void UpdateEditor()
	{
		_editor.Text = Value;
		_editor.Placeholder = Placeholder;
		_editor.Keyboard = Keyboard;
		_editor.MaxLength = MaxLength;
		_editor.IsReadOnly = IsReadOnly;
	}
}