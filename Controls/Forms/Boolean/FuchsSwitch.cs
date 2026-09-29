namespace FuchsControls;

public sealed class FuchsSwitch : FuchsBooleanBase
{
	private readonly Switch _switch = new Switch().ApplyFuchsSwitchStyle();
	private bool _isUpdating;

	public static readonly BindableProperty IsToggledProperty = BindableProperty.Create(
		nameof(IsToggled), typeof(bool), typeof(FuchsSwitch), false, BindingMode.TwoWay, propertyChanged: OnIsToggledChanged);

	[Obsolete("Use IsToggled instead.")]
	public static readonly BindableProperty ValueProperty = BindableProperty.Create(
		nameof(Value), typeof(bool), typeof(FuchsSwitch), false, BindingMode.TwoWay, propertyChanged: OnValueChanged);

	public event EventHandler<FuchsBooleanChangedEventArgs>? Toggled;

	public FuchsSwitch()
	{
		_switch.Toggled += OnToggled;
		SetBooleanInput(_switch, _switch);
		UpdateIsToggled();
	}

	public bool IsToggled
	{
		get => (bool)GetValue(IsToggledProperty);
		set => SetValue(IsToggledProperty, value);
	}

	[Obsolete("Use IsToggled instead.")]
	public bool Value
	{
		get => (bool)GetValue(ValueProperty);
		set => SetValue(ValueProperty, value);
	}

	private static void OnIsToggledChanged(BindableObject bindable, object oldValue, object newValue) =>
		((FuchsSwitch)bindable).OnIsToggledChanged((bool)newValue);

	private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue) =>
		((FuchsSwitch)bindable).SetIsToggled((bool)newValue);

	private void OnToggled(object? sender, ToggledEventArgs e)
	{
		if (!_isUpdating)
			IsToggled = e.Value;
	}

	private void OnIsToggledChanged(bool value)
	{
		UpdateIsToggled();
		SetValue(ValueProperty, value);
		RaiseValueChanged(value);
		Toggled?.Invoke(this, new FuchsBooleanChangedEventArgs(value));
	}

	private void SetIsToggled(bool value)
	{
		if (IsToggled != value)
			IsToggled = value;
	}

	private void UpdateIsToggled()
	{
		_isUpdating = true;
		_switch.IsToggled = IsToggled;
		_isUpdating = false;
	}

	protected override void ApplyThemeColor(Color color) => _switch.OnColor = color;
}