using System.Globalization;

namespace FuchsControls.Controls;

public enum FuchsNumericType
{
	Int,
	Double,
	Float
}

public sealed class FuchsNumericInput : FuchsInput
{
	private bool _updatingValue;

	public static readonly BindableProperty NumericTypeProperty =
		BindableProperty.Create(nameof(NumericType), typeof(FuchsNumericType), typeof(FuchsNumericInput), FuchsNumericType.Double
			, propertyChanged: OnNumericTypeChanged);

	public static readonly BindableProperty ValueProperty =
		BindableProperty.Create(nameof(Value), typeof(object), typeof(FuchsNumericInput), null, BindingMode.TwoWay, propertyChanged: OnValueChanged);

	public FuchsNumericType NumericType
	{
		get => (FuchsNumericType)GetValue(NumericTypeProperty);
		set => SetValue(NumericTypeProperty, value);
	}

	public object? Value
	{
		get => GetValue(ValueProperty);
		set => SetValue(ValueProperty, value);
	}

	public FuchsNumericInput()
	{
		Keyboard = Keyboard.Numeric;
	}

	protected override void OnTextChanged(string text)
	{
		if (_updatingValue || !TryParse(text, NumericType, out object? value))
			return;

		_updatingValue = true;
		Value = value;
		_updatingValue = false;
	}

	private static void OnNumericTypeChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsNumericInput input)
		{
			input.Keyboard = Keyboard.Numeric;
			input.SetValueFromText(input.Text);
		}
	}

	private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsNumericInput input && !input._updatingValue && newValue is not null)
			input.Text = Convert.ToString(newValue, CultureInfo.InvariantCulture) ?? string.Empty;
	}

	private void SetValueFromText(string text)
	{
		if (TryParse(text, NumericType, out object? value))
			Value = value;
	}

	private static bool TryParse(string text, FuchsNumericType type, out object? value)
	{
		value = type switch
		{
			FuchsNumericType.Int when int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result) => result
			, FuchsNumericType.Float when float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out float result) => result
			, FuchsNumericType.Double when double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double result) => result, _ => null
		};

		return value is not null;
	}
}