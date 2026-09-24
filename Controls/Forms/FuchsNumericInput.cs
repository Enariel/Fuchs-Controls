using System.Globalization;
using System.Text;

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
	private bool _updatingText;

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
		if (_updatingText)
			return;

		Type targetType = ValueType ?? GetTypeForNumericType(NumericType);
		string numericText = FilterNumericText(text, targetType);
		if (!string.Equals(text, numericText, StringComparison.Ordinal))
		{
			_updatingText = true;
			try
			{
				Text = numericText;
			}
			finally
			{
				_updatingText = false;
			}

			text = numericText;
		}

		if (_updatingValue || !TryParse(text, targetType, out object? value))
			return;

		_updatingValue = true;
		try
		{
			Value = value;
		}
		finally
		{
			_updatingValue = false;
		}
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
		if (!IsNumericType(targetType))
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

	private static string FilterNumericText(string text, Type type)
	{
		Type targetType = Nullable.GetUnderlyingType(type) ?? type;
		bool allowsDecimal = Type.GetTypeCode(targetType) is TypeCode.Decimal or TypeCode.Double or TypeCode.Single;
		bool hasDecimal = false;
		StringBuilder filtered = new();

		foreach (char character in text)
		{
			if (char.IsDigit(character))
			{
				filtered.Append(character);
				continue;
			}

			if (character is '+' or '-' && filtered.Length == 0)
			{
				filtered.Append(character);
				continue;
			}

			if (allowsDecimal && character == '.' && !hasDecimal)
			{
				filtered.Append(character);
				hasDecimal = true;
			}
		}

		return filtered.ToString();
	}

	private static bool IsNumericType(Type type) => Type.GetTypeCode(type) is TypeCode.Byte or TypeCode.Decimal or TypeCode.Double
		or TypeCode.Int16 or TypeCode.Int32 or TypeCode.Int64 or TypeCode.SByte or TypeCode.Single or TypeCode.UInt16
		or TypeCode.UInt32 or TypeCode.UInt64;

	private static Type GetTypeForNumericType(FuchsNumericType type) => type switch
	{
		FuchsNumericType.Int => typeof(int), FuchsNumericType.Float => typeof(float), _ => typeof(double)
	};
}