#region Meta
// FuchsControls
// Created: 10/01/2026
// Modified: 10/01/2026
#endregion

namespace FuchsControls.Fields;

public abstract class NumericFieldBase<TNumber> : TextFieldBase, INumericField<TNumber> where TNumber : struct
{
	public static readonly BindableProperty NumericValueProperty = BindableProperty.Create
	(
		nameof(NumericValue),
		typeof(TNumber),
		typeof(NumericFieldBase<TNumber>),
		defaultValue: default(TNumber)
	);

	public TNumber NumericValue
	{
		get => (TNumber)GetValue(NumericValueProperty);
		set => SetValue(NumericValueProperty, value);
	}

	public static readonly BindableProperty StepProperty = BindableProperty.Create
	(
		nameof(Step),
		typeof(int),
		typeof(NumericFieldBase<TNumber>),
		defaultValue: 1
	);

	public int Step
	{
		get => (int)GetValue(StepProperty);
		set => SetValue(StepProperty, value);
	}

	public static readonly BindableProperty MinimumProperty = BindableProperty.Create
	(
		nameof(Minimum),
		typeof(int),
		typeof(NumericFieldBase<TNumber>),
		defaultValue: 0
	);

	public int Minimum
	{
		get => (int)GetValue(MinimumProperty);
		set => SetValue(MinimumProperty, value);
	}

	public static readonly BindableProperty MaximumProperty = BindableProperty.Create
	(
		nameof(Maximum),
		typeof(int),
		typeof(NumericFieldBase<TNumber>),
		defaultValue: 100
	);

	public int Maximum
	{
		get => (int)GetValue(MaximumProperty);
		set => SetValue(MaximumProperty, value);
	}

	protected NumericFieldBase()
	{
		Keyboard = Keyboard.Numeric;
	}


	// Optional: Configure the stepper in derived classes
	public void ConfigureStepper(int minimum, int maximum, int increment)
	{
		Minimum = minimum;
		Maximum = maximum;
		Step = increment;
	}


	protected abstract void OnNumberChanged(object? sender, ValueChangedEventArgs e);

	protected abstract void OnTextChanged(object sender, TextChangedEventArgs e);
}