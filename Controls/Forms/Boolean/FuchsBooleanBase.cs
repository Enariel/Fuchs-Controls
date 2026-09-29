namespace FuchsControls;

public abstract class FuchsBooleanBase : FuchsFieldBase
{
	private readonly FuchsTypo _textLabel = new FuchsTypo { Typo = FuchsTypoType.Body }.ApplyFuchsStyle("FuchsFormOptionTextStyle");
	private bool _isThemeChangeSubscribed;

	public static readonly BindableProperty TextProperty = BindableProperty.Create(
		nameof(Text), typeof(string), typeof(FuchsBooleanBase), string.Empty, propertyChanged: OnTextChanged);

	public static readonly BindableProperty ColorProperty = BindableProperty.Create(
		nameof(Color), typeof(FuchsThemeColor), typeof(FuchsBooleanBase), FuchsThemeColor.Primary, BindingMode.TwoWay,
		propertyChanged: OnThemePropertyChanged);

	public event EventHandler<FuchsBooleanChangedEventArgs>? ValueChanged;

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	public FuchsThemeColor Color
	{
		get => (FuchsThemeColor)GetValue(ColorProperty);
		set => SetValue(ColorProperty, value);
	}

	protected void SetBooleanInput(View input, VisualElement focusTarget)
	{
		SetInput(new HorizontalStackLayout { Children = { input, _textLabel } }.ApplyFuchsStyle("FuchsFormOptionLayoutStyle"), focusTarget);
	}

	protected void RaiseValueChanged(bool value) => ValueChanged?.Invoke(this, new FuchsBooleanChangedEventArgs(value));

	protected Color ThemeColor => FuchsThemeResourceLookup.GetColor(Color);

	protected abstract void ApplyThemeColor(Color color);

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
		if (Handler is null)
		{
			UnsubscribeFromThemeChanges();
			return;
		}

		SubscribeToThemeChanges();
		ApplyTheme();
	}

	private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue) =>
		((FuchsBooleanBase)bindable)._textLabel.Text = (string)newValue;

	private static void OnThemePropertyChanged(BindableObject bindable, object oldValue, object newValue) =>
		((FuchsBooleanBase)bindable).ApplyTheme();

	private void SubscribeToThemeChanges()
	{
		if (_isThemeChangeSubscribed)
			return;

		FuchsThemeManager.ThemeChanged += OnThemeChanged;
		_isThemeChangeSubscribed = true;
	}

	private void UnsubscribeFromThemeChanges()
	{
		if (!_isThemeChangeSubscribed)
			return;

		FuchsThemeManager.ThemeChanged -= OnThemeChanged;
		_isThemeChangeSubscribed = false;
	}

	private void OnThemeChanged(object? sender, EventArgs e) => ApplyTheme();

	private void ApplyTheme() => ApplyThemeColor(ThemeColor);
}