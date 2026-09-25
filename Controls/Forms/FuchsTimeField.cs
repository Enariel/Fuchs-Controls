namespace FuchsControls;

public sealed class FuchsTimeField : FuchsFieldBase
{
	private readonly TimePicker _picker = new TimePicker();
	private bool _isUpdating;

	public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(TimeSpan), typeof(FuchsTimeField), TimeSpan.Zero
		, BindingMode.TwoWay, propertyChanged: OnTimeChanged);

	public FuchsTimeField()
	{
		_picker.SetDynamicResource(StyleProperty, "FuchsTimePickerStyle");
		_picker.PropertyChanged += (_, e) =>
		{
			if (!_isUpdating && e.PropertyName == nameof(TimePicker.Time)) SetValue(ValueProperty, _picker.Time);
		};
		SetInput(_picker);
		UpdatePicker();
	}

	public TimeSpan Value
	{
		get => (TimeSpan)GetValue(ValueProperty);
		set => SetValue(ValueProperty, value);
	}

	private static void OnTimeChanged(BindableObject b, object o, object n) => ((FuchsTimeField)b).UpdatePicker();

	private void UpdatePicker()
	{
		_isUpdating = true;
		_picker.Time = Value;
		_isUpdating = false;
	}
}