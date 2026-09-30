namespace FuchsControls;

public sealed class FuchsBadge : Border
{
	private const string PulseAnimationName = "FuchsBadgePulse";
	private readonly Label label = new Label();
	private bool isThemeChangeSubscribed;

	public static readonly BindableProperty TextProperty = BindableProperty.Create(
		nameof(Text), typeof(string), typeof(FuchsBadge), string.Empty, propertyChanged: OnTextChanged);

	public static readonly BindableProperty ColorProperty = BindableProperty.Create(
		nameof(Color), typeof(FuchsThemeColor), typeof(FuchsBadge), FuchsThemeColor.Primary, propertyChanged: OnThemePropertyChanged);

	public static readonly BindableProperty IsPulsingProperty = BindableProperty.Create(
		nameof(IsPulsing), typeof(bool), typeof(FuchsBadge), false, propertyChanged: OnIsPulsingChanged);

	public FuchsBadge()
	{
		this.ApplyFuchsStyle("FuchsBadgeStyle");
		label.SetDynamicResource(VisualElement.StyleProperty, "FuchsBadgeTextStyle");
		Content = label;
		UpdateText();
		ApplyTheme();
	}

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

	public bool IsPulsing
	{
		get => (bool)GetValue(IsPulsingProperty);
		set => SetValue(IsPulsingProperty, value);
	}

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
		if (Handler is null)
		{
			UnsubscribeFromThemeChanges();
			StopPulse();
			return;
		}

		SubscribeToThemeChanges();
		ApplyTheme();
		UpdatePulse();
	}

	private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsBadge)bindable).UpdateText();

	private static void OnThemePropertyChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsBadge)bindable).ApplyTheme();

	private static void OnIsPulsingChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsBadge)bindable).UpdatePulse();

	private void UnsubscribeFromThemeChanges()
	{
		if (!isThemeChangeSubscribed)
			return;

		FuchsThemeManager.ThemeChanged -= OnThemeChanged;
		isThemeChangeSubscribed = false;
	}

	private void OnThemeChanged(object? sender, EventArgs e) => ApplyTheme();

	private void UpdateText() => label.Text = Text;

	private void ApplyTheme()
	{
		var color = FuchsThemeResourceLookup.GetColor(Color);
		BackgroundColor = color;
		label.TextColor = color.Red * 0.299 + color.Green * 0.587 + color.Blue * 0.114 > 0.58
			? FuchsThemeManager.Current.DarkColor
			: FuchsThemeManager.Current.LightColor;
	}

	private void UpdatePulse()
	{
		if (!IsPulsing || Handler is null)
		{
			StopPulse();
			return;
		}

		this.AbortAnimation(PulseAnimationName);
		var pulse = new Animation();
		pulse.Add(0, 0.5, new Animation(value => Opacity = value, 1, 0.55));
		pulse.Add(0.5, 1, new Animation(value => Opacity = value, 0.55, 1));
		pulse.Commit(this, PulseAnimationName, 16, 900, Easing.SinInOut, repeat: () => IsPulsing && Handler is not null);
	}

	private void StopPulse()
	{
		this.AbortAnimation(PulseAnimationName);
		Opacity = 1;
	}

	private void SubscribeToThemeChanges()
	{
		if (isThemeChangeSubscribed)
			return;

		FuchsThemeManager.ThemeChanged += OnThemeChanged;
		isThemeChangeSubscribed = true;
	}
}