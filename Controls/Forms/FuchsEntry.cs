namespace FuchsControls;

public sealed class FuchsEntry : FuchsFieldBase
{
	private readonly Entry _entry = new Entry();

	public static readonly BindableProperty ValueProperty = BindableProperty.Create(
		nameof(Value), typeof(string), typeof(FuchsEntry), string.Empty, BindingMode.TwoWay, propertyChanged: OnValueChanged);

	public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
		nameof(Placeholder), typeof(string), typeof(FuchsEntry), string.Empty, propertyChanged: OnEntryPropertyChanged);

	public static readonly BindableProperty KeyboardProperty = BindableProperty.Create(
		nameof(Keyboard), typeof(Keyboard), typeof(FuchsEntry), Keyboard.Default, propertyChanged: OnEntryPropertyChanged);

	public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(
		nameof(MaxLength), typeof(int), typeof(FuchsEntry), int.MaxValue, propertyChanged: OnEntryPropertyChanged);

	public static readonly BindableProperty IsReadOnlyProperty = BindableProperty.Create(
		nameof(IsReadOnly), typeof(bool), typeof(FuchsEntry), false, propertyChanged: OnEntryPropertyChanged);

	public FuchsEntry()
	{
		_entry.SetDynamicResource(StyleProperty, "FuchsEntryStyle");
		_entry.TextChanged += OnTextChanged;
		SetInput(_entry);
		UpdateEntry();
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

	private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsEntry)bindable)._entry.Text = (string)newValue;
	private static void OnEntryPropertyChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsEntry)bindable).UpdateEntry();
	private void OnTextChanged(object? sender, TextChangedEventArgs e) => SetValue(ValueProperty, e.NewTextValue ?? string.Empty);

	private void UpdateEntry()
	{
		_entry.Text = Value;
		_entry.Placeholder = Placeholder;
		_entry.Keyboard = Keyboard;
		_entry.MaxLength = MaxLength;
		_entry.IsReadOnly = IsReadOnly;
	}
}