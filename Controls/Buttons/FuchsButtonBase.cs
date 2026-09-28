namespace FuchsControls;

public abstract class FuchsButtonBase : Button
{
	public static readonly BindableProperty ColorProperty = BindableProperty.Create(
		nameof(Color), typeof(FuchsThemeColor), typeof(FuchsButtonBase), FuchsThemeColor.Primary, BindingMode.TwoWay, propertyChanged: OnThemePropertyChanged);

	private bool isThemeChangeSubscribed;

	protected FuchsButtonBase()
	{
		this.ApplyFuchsStyle("FuchsControlButtonStyle");
		ApplyTheme();
	}

	public FuchsThemeColor Color
	{
		get => (FuchsThemeColor)GetValue(ColorProperty);
		set => SetValue(ColorProperty, value);
	}

	protected Color ThemeColor => FuchsThemeResourceLookup.GetColor(Color);

	protected Color ForegroundColor => Color is FuchsThemeColor.Light
		? FuchsThemeManager.Current.TextColor
		: FuchsThemeManager.Current.LightColor;

	protected abstract void ApplyThemedStyle();

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

	private static void OnThemePropertyChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsButtonBase)bindable).ApplyTheme();

	private void SubscribeToThemeChanges()
	{
		if (isThemeChangeSubscribed)
		{
			return;
		}

		FuchsThemeManager.ThemeChanged += OnThemeChanged;
		isThemeChangeSubscribed = true;
	}

	private void UnsubscribeFromThemeChanges()
	{
		if (!isThemeChangeSubscribed)
		{
			return;
		}

		FuchsThemeManager.ThemeChanged -= OnThemeChanged;
		isThemeChangeSubscribed = false;
	}

	private void OnThemeChanged(object? sender, EventArgs e) => ApplyTheme();

	protected void ApplyTheme()
	{
		CornerRadius = (int)Math.Round(FuchsThemeManager.Current.CornerRadius);
		ApplyThemedStyle();
	}
}