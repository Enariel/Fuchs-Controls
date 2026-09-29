namespace FuchsControls;

public sealed partial class FuchsSwitch : FuchsBooleanBase
{
	private readonly Border _track = new Border
									 {
										 Padding = 0, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center
									 }.ApplyFuchsStyle("FuchsSwitchTrackStyle");

	private readonly Border _thumb = new Border
									 {
										 Padding = 0, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center
									 }.ApplyFuchsStyle("FuchsSwitchThumbStyle");

	private readonly Grid _switchVisual = new() { WidthRequest = 44, HeightRequest = 44 };

	private readonly Grid _input = new()
								   {
									   BackgroundColor = Colors.Transparent, HorizontalOptions = LayoutOptions.Start
									   , VerticalOptions = LayoutOptions.Center
								   };

	private Color _themeColor = Colors.Transparent;

	public static readonly BindableProperty IsToggledProperty = BindableProperty.Create(
		nameof(IsToggled), typeof(bool), typeof(FuchsSwitch), false, BindingMode.TwoWay, propertyChanged: OnIsToggledChanged);

	[Obsolete("Use IsToggled instead.")] public static readonly BindableProperty ValueProperty = BindableProperty.Create(
		nameof(Value), typeof(bool), typeof(FuchsSwitch), false, BindingMode.TwoWay, propertyChanged: OnValueChanged);

	public event EventHandler<FuchsBooleanChangedEventArgs>? Toggled;

	public FuchsSwitch()
	{
		this.ApplyFuchsSwitchStyle();
		_switchVisual.Children.Add(_track);
		_switchVisual.Children.Add(_thumb);
		_input.Children.Add(_switchVisual);
		_input.SetDynamicResource(VisualElement.WidthRequestProperty, FuchsThemeResourceKeys.FieldHeight);
		_input.SetDynamicResource(VisualElement.HeightRequestProperty, FuchsThemeResourceKeys.FieldHeight);
		_input.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(ToggleSwitch) });
		SetBooleanInput(_input, _input);
		UpdateIsToggled();
		ApplyThemeColor(ThemeColor);
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
		_track.BackgroundColor = IsToggled ? _themeColor : FuchsThemeManager.Current.DefaultColor;
		_track.Stroke = new SolidColorBrush(IsToggled ? _themeColor : FuchsThemeManager.Current.FieldBorderColor);
		_thumb.TranslationX = IsToggled ? 10 : -10;
	}

	private void ToggleSwitch()
	{
		if (IsEnabled)
			IsToggled = !IsToggled;
	}

	protected override void ApplyThemeColor(Color color)
	{
		_themeColor = color;
		UpdateIsToggled();
	}
}