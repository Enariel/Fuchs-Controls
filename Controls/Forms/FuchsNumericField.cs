using System.Globalization;

namespace FuchsControls;

public class FuchsNumericField : FuchsNumericFieldBase
{
	protected readonly Entry NumericEntry = new Entry { Keyboard = Keyboard.Numeric }.ApplyFuchsEntryStyle();
	private bool _isUpdatingText;

	public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
		nameof(Placeholder), typeof(string), typeof(FuchsNumericField), string.Empty, propertyChanged: OnPlaceholderChanged);

	public FuchsNumericField()
	{
		NumericEntry.TextChanged += OnTextChanged;
		SetInput(NumericEntry);
	}

	public string Placeholder
	{
		get => (string)GetValue(PlaceholderProperty);
		set => SetValue(PlaceholderProperty, value);
	}

	protected override void OnNumericValueChanged()
	{
		if (!_isUpdatingText)
		{
			SetText(Value is null ? string.Empty : Convert.ToString(Value, CultureInfo.CurrentCulture) ?? string.Empty);
		}
	}

	private static void OnPlaceholderChanged(BindableObject bindable, object oldValue, object newValue) =>
		((FuchsNumericField)bindable).NumericEntry.Placeholder = (string)newValue;

	private void OnTextChanged(object? sender, TextChangedEventArgs e)
	{
		if (_isUpdatingText)
		{
			return;
		}

		if (TryNormalize(e.NewTextValue, out var value, out var normalized))
		{
			SetValue(ValueProperty, value);
			InputState = FuchsInputState.Normal;
			if (!string.Equals(e.NewTextValue, normalized, StringComparison.Ordinal))
			{
				SetText(normalized);
			}
		}
		else
		{
			InputState = FuchsInputState.Invalid;
			SetText(e.OldTextValue ?? string.Empty);
		}
	}

	protected void SetText(string text)
	{
		_isUpdatingText = true;
		NumericEntry.Text = text;
		_isUpdatingText = false;
	}
}