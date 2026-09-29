using System.Windows.Input;

namespace FuchsControls;

public abstract class FuchsTextBase : FuchsFieldBase
{
	private readonly InputView _input;
	protected InputView Input => _input;

	public static readonly BindableProperty ValueProperty = BindableProperty.Create(
		nameof(Value), typeof(string), typeof(FuchsTextBase), string.Empty, BindingMode.TwoWay, propertyChanged: OnValueChanged);

	public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
		nameof(Placeholder), typeof(string), typeof(FuchsTextBase), string.Empty, propertyChanged: OnInputPropertyChanged);

	public static readonly BindableProperty KeyboardProperty = BindableProperty.Create(
		nameof(Keyboard), typeof(Keyboard), typeof(FuchsTextBase), Keyboard.Default, propertyChanged: OnInputPropertyChanged);

	public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(
		nameof(MaxLength), typeof(int), typeof(FuchsTextBase), int.MaxValue, propertyChanged: OnInputPropertyChanged);

	public static readonly BindableProperty IsReadOnlyProperty = BindableProperty.Create(
		nameof(IsReadOnly), typeof(bool), typeof(FuchsTextBase), false, propertyChanged: OnInputPropertyChanged);

	public static readonly BindableProperty ReturnCommandProperty = BindableProperty.Create(
		nameof(ReturnCommand), typeof(ICommand), typeof(FuchsTextBase), propertyChanged: OnInputPropertyChanged);

	protected FuchsTextBase(InputView input)
	{
		_input = input;
		_input.TextChanged += OnTextChanged;
		SetInput(_input);
		UpdateInput();
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

	public ICommand? ReturnCommand
	{
		get => (ICommand?)GetValue(ReturnCommandProperty);
		set => SetValue(ReturnCommandProperty, value);
	}

	private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsTextBase)bindable)._input.Text = (string)newValue;
	private static void OnInputPropertyChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsTextBase)bindable).UpdateInput();
	private void OnTextChanged(object? sender, TextChangedEventArgs e) => SetValue(ValueProperty, e.NewTextValue ?? string.Empty);

	private void UpdateInput()
	{
		_input.Text = Value;
		_input.Placeholder = Placeholder;
		_input.Keyboard = Keyboard;
		_input.MaxLength = MaxLength;
		_input.IsReadOnly = IsReadOnly;
		UpdateReturnCommand(ReturnCommand);
	}

	protected virtual void UpdateReturnCommand(ICommand? command) { }
}