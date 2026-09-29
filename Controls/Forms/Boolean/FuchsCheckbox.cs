namespace FuchsControls;

public sealed class FuchsCheckbox : FuchsBooleanBase
{
	private readonly CheckBox _checkBox = new CheckBox().ApplyFuchsCheckboxStyle();
	private bool _isUpdating;

	public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
		nameof(IsChecked), typeof(bool), typeof(FuchsCheckbox), false, BindingMode.TwoWay, propertyChanged: OnIsCheckedChanged);

	[Obsolete("Use IsChecked instead. MAUI CheckBox does not support an indeterminate state.")]
	public static readonly BindableProperty ValueProperty = BindableProperty.Create(
		nameof(Value), typeof(bool?), typeof(FuchsCheckbox), null, BindingMode.TwoWay, propertyChanged: OnValueChanged);

	[Obsolete("MAUI CheckBox does not support an indeterminate state.")]
	public static readonly BindableProperty IsThreeStateProperty = BindableProperty.Create(
		nameof(IsThreeState), typeof(bool), typeof(FuchsCheckbox), false);

	public event EventHandler<FuchsBooleanChangedEventArgs>? CheckedChanged;

	public FuchsCheckbox()
	{
		_checkBox.CheckedChanged += OnCheckedChanged;
		SetBooleanInput(_checkBox, _checkBox);
		UpdateIsChecked();
	}

	public bool IsChecked
	{
		get => (bool)GetValue(IsCheckedProperty);
		set => SetValue(IsCheckedProperty, value);
	}

	[Obsolete("Use IsChecked instead. MAUI CheckBox does not support an indeterminate state.")]
	public bool? Value
	{
		get => (bool?)GetValue(ValueProperty);
		set => SetValue(ValueProperty, value);
	}

	[Obsolete("MAUI CheckBox does not support an indeterminate state.")]
	public bool IsThreeState
	{
		get => (bool)GetValue(IsThreeStateProperty);
		set => SetValue(IsThreeStateProperty, value);
	}

	private static void OnIsCheckedChanged(BindableObject bindable, object oldValue, object newValue) =>
		((FuchsCheckbox)bindable).OnIsCheckedChanged((bool)newValue);

	private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue) =>
		((FuchsCheckbox)bindable).SetIsChecked((bool?)newValue ?? false);

	private void OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
	{
		if (!_isUpdating)
			IsChecked = e.Value;
	}

	private void OnIsCheckedChanged(bool value)
	{
		UpdateIsChecked();
		SetValue(ValueProperty, value);
		RaiseValueChanged(value);
		CheckedChanged?.Invoke(this, new FuchsBooleanChangedEventArgs(value));
	}

	private void SetIsChecked(bool value)
	{
		if (IsChecked != value)
			IsChecked = value;
	}

	private void UpdateIsChecked()
	{
		_isUpdating = true;
		_checkBox.IsChecked = IsChecked;
		_isUpdating = false;
	}

	protected override void ApplyThemeColor(Color color) => _checkBox.Color = color;
}