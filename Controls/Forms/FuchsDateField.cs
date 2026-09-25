namespace FuchsControls;

public sealed class FuchsDateField : FuchsFieldBase
{
	private readonly DatePicker _picker = new DatePicker();
	private bool _isUpdating;

	public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(DateTime), typeof(FuchsDateField), DateTime.Today
		, BindingMode.TwoWay, propertyChanged: OnDateChanged);

	public static readonly BindableProperty MinimumDateProperty = BindableProperty.Create(nameof(MinimumDate), typeof(DateTime), typeof(FuchsDateField)
		, DateTime.MinValue, propertyChanged: OnDateChanged);

	public static readonly BindableProperty MaximumDateProperty = BindableProperty.Create(nameof(MaximumDate), typeof(DateTime), typeof(FuchsDateField)
		, DateTime.MaxValue, propertyChanged: OnDateChanged);

	public FuchsDateField()
	{
		_picker.SetDynamicResource(StyleProperty, "FuchsDatePickerStyle");
		_picker.DateSelected += (_, e) =>
		{
			if (!_isUpdating) SetValue(ValueProperty, e.NewDate);
		};
		SetInput(_picker);
		UpdatePicker();
	}

	public DateTime Value
	{
		get => (DateTime)GetValue(ValueProperty);
		set => SetValue(ValueProperty, value);
	}

	public DateTime MinimumDate
	{
		get => (DateTime)GetValue(MinimumDateProperty);
		set => SetValue(MinimumDateProperty, value);
	}

	public DateTime MaximumDate
	{
		get => (DateTime)GetValue(MaximumDateProperty);
		set => SetValue(MaximumDateProperty, value);
	}

	private static void OnDateChanged(BindableObject b, object o, object n) => ((FuchsDateField)b).UpdatePicker();

	private void UpdatePicker()
	{
		_isUpdating = true;
		_picker.MinimumDate = MinimumDate;
		_picker.MaximumDate = MaximumDate;
		_picker.Date = Value;
		_isUpdating = false;
	}
}