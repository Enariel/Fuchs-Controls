namespace FuchsControls;

public sealed class FuchsTicker : FuchsNumericFieldBase
{
	private readonly Entry _entry = new Entry { Keyboard = Keyboard.Numeric }.ApplyFuchsEntryStyle();
	private bool _isUpdatingText;

	public FuchsTicker()
	{
		_entry.TextChanged += OnTextChanged;
		var decrement = new Button { Text = "−" }.ApplyFuchsStyle("FuchsTickerButtonStyle");
		decrement.Clicked += (_, _) => ChangeByStep(-1);
		var increment = new Button { Text = "+" }.ApplyFuchsStyle("FuchsTickerButtonStyle");
		increment.Clicked += (_, _) => ChangeByStep(1);
		var inputGrid = new Grid
						{
							ColumnDefinitions =
								{ new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) }
						};
		inputGrid.ApplyFuchsStyle("FuchsTickerGridStyle");
		inputGrid.Add(decrement, 0);
		inputGrid.Add(_entry, 1);
		inputGrid.Add(increment, 2);
		SetInput(inputGrid, _entry);
	}

	protected override void OnNumericValueChanged() =>
		SetText(Value is null ? string.Empty : Convert.ToString(Value, System.Globalization.CultureInfo.CurrentCulture) ?? string.Empty);

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

	private void SetText(string text)
	{
		_isUpdatingText = true;
		_entry.Text = text;
		_isUpdatingText = false;
	}
}