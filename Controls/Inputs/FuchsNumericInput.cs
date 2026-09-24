using System.Globalization;

namespace FuchsControls.Controls;

public enum FuchsNumericType
{
	Int
	, Double
	, Float
}

public sealed class FuchsNumericInput : FuchsInput
{
	private bool _updatingValue;

	public static readonly BindableProperty NumericTypeProperty =
		BindableProperty.Create(nameof(NumericType), typeof(FuchsNumericType), typeof(FuchsNumericInput), FuchsNumericType.Double
			, propertyChanged: OnNumericTypeChanged);

	public static readonly BindableProperty ValueProperty =
		BindableProperty.Create(nameof(Value), typeof(object), typeof(FuchsNumericInput), null, BindingMode.TwoWay, propertyChanged: OnValueChanged);

	public static readonly BindableProperty ValueTypeProperty =
		BindableProperty.Create(nameof(ValueType), typeof(Type), typeof(FuchsNumericInput), null, propertyChanged: OnValueTypeChanged);

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

	public Type? ValueType
	{
		get => (Type?)GetValue(ValueTypeProperty);
		set => SetValue(ValueTypeProperty, value);
	}

	public FuchsNumericInput()
	{
		Keyboard = Keyboard.Numeric;
	}

	protected override void OnTextChanged(string text)
	{
		if (_updatingValue || !TryParse(text, ValueType ?? GetTypeForNumericType(NumericType), out object? value))
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
		if (bindable is FuchsNumericInput input && !_IsUpdating(input) && newValue is not null)
		{
			input.ValueType ??= Nullable.GetUnderlyingType(newValue.GetType()) ?? newValue.GetType();
			input.Text = Convert.ToString(newValue, CultureInfo.InvariantCulture) ?? string.Empty;
		}
	}

	private static bool _IsUpdating(FuchsNumericInput input) => input._updatingValue;

	private static void OnValueTypeChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsNumericInput input)
			input.SetValueFromText(input.Text);
	}

	private void SetValueFromText(string text)
	{
		if (TryParse(text, ValueType ?? GetTypeForNumericType(NumericType), out object? value))
			Value = value;
	}

	private static bool TryParse(string text, Type type, out object? value)
	{
		Type targetType = Nullable.GetUnderlyingType(type) ?? type;
		if (!targetType.IsPrimitive && targetType != typeof(decimal))
		{
			value = null;
			return false;
		}

		try
		{
			value = Convert.ChangeType(text, targetType, CultureInfo.InvariantCulture);
			return value is not null;
		}
		catch (FormatException)
		{
			value = null;
			return false;
		}
		catch (OverflowException)
		{
			value = null;
			return false;
		}
	}

	private static Type GetTypeForNumericType(FuchsNumericType type) => type switch
	{
		FuchsNumericType.Int => typeof(int), FuchsNumericType.Float => typeof(float), _ => typeof(double)
	};
}