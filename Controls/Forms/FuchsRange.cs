namespace FuchsControls;

public sealed class FuchsRange : FuchsNumericFieldBase
{
	private readonly Slider _slider = new();
	private bool _isUpdating;

	public FuchsRange()
	{
		_slider.SetDynamicResource(StyleProperty, "FuchsRangeStyle");
		_slider.ValueChanged += OnValueChanged;
		SetInput(_slider);
		UpdateSlider();
	}

	protected override void OnNumericValueChanged() => UpdateSlider();

	private void OnValueChanged(object? sender, ValueChangedEventArgs e)
	{
		if (_isUpdating)
		{
			return;
		}

		TryNormalize(e.NewValue.ToString(System.Globalization.CultureInfo.CurrentCulture), out var value, out _);
		SetValue(ValueProperty, value);
	}

	private void UpdateSlider()
	{
		_isUpdating = true;
		_slider.Minimum = Minimum;
		_slider.Maximum = Maximum;
		_slider.Value = Math.Clamp(CurrentValue, Minimum, Maximum);
		_isUpdating = false;
	}
}