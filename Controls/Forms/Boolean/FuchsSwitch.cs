namespace FuchsControls;

public sealed class FuchsSwitch : FuchsFieldBase
{
	private readonly Switch _switch = new Switch().ApplyFuchsSwitchStyle();
	private readonly FuchsTypo _valueLabel = new FuchsTypo { Type = FuchsTypoType.Body }.ApplyFuchsStyle("FuchsFormOptionTextStyle");
	private bool _isUpdating;

	public static readonly BindableProperty ValueProperty =
		BindableProperty.Create(nameof(Value), typeof(bool), typeof(FuchsSwitch), false, BindingMode.TwoWay, propertyChanged: OnValueChanged);

	public static readonly BindableProperty TextProperty =
		BindableProperty.Create(nameof(Text), typeof(string), typeof(FuchsSwitch), string.Empty, propertyChanged: OnTextChanged);

	public FuchsSwitch()
	{
		_switch.Toggled += OnToggled;
		SetInput(new HorizontalStackLayout { Children = { _switch, _valueLabel } }.ApplyFuchsStyle("FuchsFormOptionLayoutStyle"), _switch);
		UpdateValue();
	}

	public bool Value
	{
		get => (bool)GetValue(ValueProperty);
		set => SetValue(ValueProperty, value);
	}

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsSwitch)bindable).UpdateValue();
	private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsSwitch)bindable)._valueLabel.Text = (string)newValue;

	private void OnToggled(object? sender, ToggledEventArgs e)
	{
		if (!_isUpdating) SetValue(ValueProperty, e.Value);
	}

	private void UpdateValue()
	{
		_isUpdating = true;
		_switch.IsToggled = Value;
		_valueLabel.Text = Text;
		_isUpdating = false;
	}
}