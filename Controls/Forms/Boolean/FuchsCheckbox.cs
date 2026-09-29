namespace FuchsControls;

public sealed class FuchsCheckbox : FuchsFieldBase
{
	private readonly Button _button = new Button().ApplyFuchsCheckboxStyle();
	private readonly FuchsTypo _textLabel = new FuchsTypo { Typo = FuchsTypoType.Body }.ApplyFuchsStyle("FuchsFormOptionTextStyle");

	public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(bool?), typeof(FuchsCheckbox), null
		, BindingMode.TwoWay, propertyChanged: OnValueChanged);

	public static readonly BindableProperty TextProperty =
		BindableProperty.Create(nameof(Text), typeof(string), typeof(FuchsCheckbox), string.Empty, propertyChanged: OnTextChanged);

	public static readonly BindableProperty IsThreeStateProperty = BindableProperty.Create(nameof(IsThreeState), typeof(bool), typeof(FuchsCheckbox), false);

	public FuchsCheckbox()
	{
		_button.Clicked += (_, _) => SetValue(ValueProperty, IsThreeState ? Value switch { null => true, true => false, _ => null } : !(Value ?? false));
		SetInput(new HorizontalStackLayout { Children = { _button, _textLabel } }.ApplyFuchsStyle("FuchsFormOptionLayoutStyle"), _button);
		UpdateValue();
	}

	public bool? Value
	{
		get => (bool?)GetValue(ValueProperty);
		set => SetValue(ValueProperty, value);
	}

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	public bool IsThreeState
	{
		get => (bool)GetValue(IsThreeStateProperty);
		set => SetValue(IsThreeStateProperty, value);
	}

	private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsCheckbox)bindable).UpdateValue();

	private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue) =>
		((FuchsCheckbox)bindable)._textLabel.Text = (string)newValue;

	private void UpdateValue()
	{
		_button.Text = Value switch { true => "✓", false => string.Empty, _ => "—" };
		_textLabel.Text = Text;
	}
}