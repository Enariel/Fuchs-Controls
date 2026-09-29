namespace FuchsControls;

public sealed partial class FuchsCheckbox : FuchsBooleanBase
{
	private readonly BoxView _shortCheckMark = new BoxView
											   {
												   WidthRequest = 3, HeightRequest = 7, Rotation = 45, TranslationX = -4, TranslationY = 2
												   , HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center
											   }.ApplyFuchsStyle("FuchsCheckboxMarkStyle");

	private readonly BoxView _longCheckMark = new BoxView
											  {
												  WidthRequest = 3, HeightRequest = 13, Rotation = -45, TranslationX = 1.5, TranslationY = -1.5
												  , HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center
											  }.ApplyFuchsStyle("FuchsCheckboxMarkStyle");

	private readonly Grid _checkMarkLayout = new() { WidthRequest = 24, HeightRequest = 24 };

	private readonly Border _checkSurface = new Border
											{
												Padding = 0, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center
											}.ApplyFuchsStyle("FuchsCheckboxSurfaceStyle");

	private readonly Grid _input = new()
								   {
									   BackgroundColor = Colors.Transparent, HorizontalOptions = LayoutOptions.Start
									   , VerticalOptions = LayoutOptions.Center
								   };

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
		this.ApplyFuchsCheckboxStyle();
		_checkMarkLayout.Children.Add(_shortCheckMark);
		_checkMarkLayout.Children.Add(_longCheckMark);
		_checkSurface.Content = _checkMarkLayout;
		_input.Children.Add(_checkSurface);
		_input.SetDynamicResource(VisualElement.WidthRequestProperty, FuchsThemeResourceKeys.FieldHeight);
		_input.SetDynamicResource(VisualElement.HeightRequestProperty, FuchsThemeResourceKeys.FieldHeight);
		_input.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(ToggleChecked) });
		SetBooleanInput(_input, _input);
		UpdateIsChecked();
		ApplyThemeColor(ThemeColor);
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
		((FuchsCheckbox)bindable).SetIsChecked(newValue is true);

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
		_shortCheckMark.IsVisible = IsChecked;
		_longCheckMark.IsVisible = IsChecked;
	}

	private void ToggleChecked()
	{
		if (IsEnabled)
			IsChecked = !IsChecked;
	}

	protected override void ApplyThemeColor(Color color)
	{
		_shortCheckMark.Color = color;
		_longCheckMark.Color = color;
	}
}