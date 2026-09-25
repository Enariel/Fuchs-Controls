using System.Globalization;

namespace FuchsControls;

public abstract class FuchsNumericFieldBase : FuchsFieldBase
{
	public static readonly BindableProperty ValueProperty = BindableProperty.Create(
		nameof(Value), typeof(object), typeof(FuchsNumericFieldBase), null, BindingMode.TwoWay, propertyChanged: OnNumericPropertyChanged);

	public static readonly BindableProperty MinimumProperty = BindableProperty.Create(nameof(Minimum), typeof(double), typeof(FuchsNumericFieldBase)
		, double.MinValue, propertyChanged: OnNumericPropertyChanged);

	public static readonly BindableProperty MaximumProperty = BindableProperty.Create(nameof(Maximum), typeof(double), typeof(FuchsNumericFieldBase)
		, double.MaxValue, propertyChanged: OnNumericPropertyChanged);

	public static readonly BindableProperty StepProperty =
		BindableProperty.Create(nameof(Step), typeof(double), typeof(FuchsNumericFieldBase), 1d, propertyChanged: OnNumericPropertyChanged);

	public static readonly BindableProperty NumberTypeProperty = BindableProperty.Create(nameof(NumberType), typeof(FuchsNumberType)
		, typeof(FuchsNumericFieldBase), FuchsNumberType.Double, propertyChanged: OnNumericPropertyChanged);

	public object? Value
	{
		get => GetValue(ValueProperty);
		set => SetValue(ValueProperty, value);
	}

	public double Minimum
	{
		get => (double)GetValue(MinimumProperty);
		set => SetValue(MinimumProperty, value);
	}

	public double Maximum
	{
		get => (double)GetValue(MaximumProperty);
		set => SetValue(MaximumProperty, value);
	}

	public double Step
	{
		get => (double)GetValue(StepProperty);
		set => SetValue(StepProperty, value);
	}

	public FuchsNumberType NumberType
	{
		get => (FuchsNumberType)GetValue(NumberTypeProperty);
		set => SetValue(NumberTypeProperty, value);
	}

	protected static void OnNumericPropertyChanged(BindableObject bindable, object oldValue, object newValue) =>
		((FuchsNumericFieldBase)bindable).OnNumericValueChanged();

	protected virtual void OnNumericValueChanged() { }

	protected bool TryNormalize(string? text, out object? value, out string normalized)
	{
		normalized = text?.Trim() ?? string.Empty;
		value = null;
		if (string.IsNullOrEmpty(normalized))
		{
			return true;
		}

		if (!double.TryParse(normalized, NumberStyles.Number | NumberStyles.AllowLeadingSign, CultureInfo.CurrentCulture, out var number))
		{
			return false;
		}

		number = Math.Clamp(number, Minimum, Maximum);
		value = NumberType switch
				{
					FuchsNumberType.Integer => Convert.ToInt32(number), FuchsNumberType.Long => Convert.ToInt64(number)
					, FuchsNumberType.Single => Convert.ToSingle(number), FuchsNumberType.Decimal => Convert.ToDecimal(number), _ => number
				};
		normalized = Convert.ToString(value, CultureInfo.CurrentCulture) ?? string.Empty;
		return true;
	}

	protected double CurrentValue => Value is null ? 0d : Convert.ToDouble(Value, CultureInfo.InvariantCulture);

	protected void ChangeByStep(double direction)
	{
		var value = Math.Clamp(CurrentValue + (Step * direction), Minimum, Maximum);
		TryNormalize(value.ToString(CultureInfo.CurrentCulture), out var typedValue, out _);
		SetValue(ValueProperty, typedValue);
	}
}