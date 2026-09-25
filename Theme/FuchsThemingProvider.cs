namespace FuchsControls;

public sealed class FuchsThemingProvider : ContentView
{
	public static readonly BindableProperty LightThemeProperty = BindableProperty.Create(
		nameof(LightTheme), typeof(FuchsTheme), typeof(FuchsThemingProvider), null, BindingMode.TwoWay, propertyChanged: OnThemePropertyChanged);

	public static readonly BindableProperty DarkThemeProperty = BindableProperty.Create(
		nameof(DarkTheme), typeof(FuchsTheme), typeof(FuchsThemingProvider), null, BindingMode.TwoWay, propertyChanged: OnThemePropertyChanged);

	public static readonly BindableProperty IsDarkModeProperty = BindableProperty.Create(
		nameof(IsDarkMode), typeof(bool), typeof(FuchsThemingProvider), false, BindingMode.TwoWay, propertyChanged: OnThemePropertyChanged);

	public FuchsThemingProvider()
	{
		SetValue(LightThemeProperty, FuchsTheme.CreateLight());
		SetValue(DarkThemeProperty, FuchsTheme.CreateDark());
		FuchsThemeManager.ThemeChanged += OnGlobalThemeChanged;
		ApplyTheme();
	}

	public FuchsTheme LightTheme
	{
		get => (FuchsTheme)GetValue(LightThemeProperty);
		set => SetValue(LightThemeProperty, value);
	}

	public FuchsTheme DarkTheme
	{
		get => (FuchsTheme)GetValue(DarkThemeProperty);
		set => SetValue(DarkThemeProperty, value);
	}

	public bool IsDarkMode
	{
		get => (bool)GetValue(IsDarkModeProperty);
		set => SetValue(IsDarkModeProperty, value);
	}

	public FuchsTheme ActiveTheme => IsDarkMode ? DarkTheme : LightTheme;

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
		ApplyTheme();
	}

	protected override void OnParentSet()
	{
		base.OnParentSet();
		ApplyTheme();
	}

	private static void OnThemePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var provider = (FuchsThemingProvider)bindable;
		if (oldValue is FuchsTheme oldTheme)
		{
			oldTheme.PropertyChanged -= provider.OnThemeChanged;
		}

		if (newValue is FuchsTheme newTheme)
		{
			newTheme.PropertyChanged += provider.OnThemeChanged;
		}

		provider.ApplyTheme();
	}

	private void OnThemeChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e) => ApplyTheme();

	private void OnGlobalThemeChanged(object? sender, EventArgs e)
	{
		if (!ReferenceEquals(FuchsThemeManager.Current, ActiveTheme))
		{
			ApplyTheme();
		}
	}

	private void ApplyTheme()
	{
		var theme = ActiveTheme;
		FuchsThemeManager.Apply(theme, Resources);
		BackgroundColor = theme.BackgroundColor;
	}
}