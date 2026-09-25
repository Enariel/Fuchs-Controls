namespace FuchsControls;

public sealed class FuchsRadioButton : FuchsFieldBase
{
	private readonly RadioButton _radioButton = new RadioButton().ApplyFuchsRadioButtonStyle();
	private readonly FuchsTypo _textLabel = new FuchsTypo { Type = FuchsTypoType.Body, VerticalOptions = LayoutOptions.Center };

	public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(object), typeof(FuchsRadioButton), null);

	public static readonly BindableProperty TextProperty =
		BindableProperty.Create(nameof(Text), typeof(string), typeof(FuchsRadioButton), string.Empty, propertyChanged: OnTextChanged);

	public static readonly BindableProperty GroupNameProperty = BindableProperty.Create(nameof(GroupName), typeof(string), typeof(FuchsRadioButton)
		, string.Empty, propertyChanged: OnGroupNameChanged);

	public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(FuchsRadioButton), false
		, BindingMode.TwoWay, propertyChanged: OnSelectedChanged);

	public FuchsRadioButton()
	{
		_radioButton.CheckedChanged += (_, e) => SetValue(IsSelectedProperty, e.Value);
		SetInput(new HorizontalStackLayout { Spacing = 8, Children = { _radioButton, _textLabel } });
	}

	public object? Value
	{
		get => GetValue(ValueProperty);
		set => SetValue(ValueProperty, value);
	}

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	public string GroupName
	{
		get => (string)GetValue(GroupNameProperty);
		set => SetValue(GroupNameProperty, value);
	}

	public bool IsSelected
	{
		get => (bool)GetValue(IsSelectedProperty);
		set => SetValue(IsSelectedProperty, value);
	}

	private static void OnTextChanged(BindableObject b, object o, object n) => ((FuchsRadioButton)b)._textLabel.Text = (string)n;
	private static void OnGroupNameChanged(BindableObject b, object o, object n) => ((FuchsRadioButton)b)._radioButton.GroupName = (string)n;
	private static void OnSelectedChanged(BindableObject b, object o, object n) => ((FuchsRadioButton)b)._radioButton.IsChecked = (bool)n;
}