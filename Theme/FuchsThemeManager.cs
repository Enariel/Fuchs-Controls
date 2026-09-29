namespace FuchsControls;

public static class FuchsThemeManager
{
	private static FuchsTheme lightTheme = FuchsTheme.CreateLight();
	private static FuchsTheme darkTheme = FuchsTheme.CreateDark();
	private static FuchsTheme current = FuchsTheme.CreateLight();
	private static Application? observedApplication;
	private static bool followsSystemTheme = true;

	static FuchsThemeManager()
	{
		current.PropertyChanged += OnCurrentThemePropertyChanged;
	}

	public static FuchsTheme Current => current;
	public static FuchsTheme LightTheme => lightTheme;
	public static FuchsTheme DarkTheme => darkTheme;
	public static bool FollowsSystemTheme => followsSystemTheme;

	public static event EventHandler? ThemeChanged;

	public static void Configure(FuchsTheme light, FuchsTheme dark)
	{
		ArgumentNullException.ThrowIfNull(light);
		ArgumentNullException.ThrowIfNull(dark);

		lightTheme = light;
		darkTheme = dark;
		followsSystemTheme = true;
		AttachToApplication(Application.Current);
		ApplySystemTheme();
	}

	public static void SetTheme(FuchsTheme theme)
	{
		ArgumentNullException.ThrowIfNull(theme);
		followsSystemTheme = false;
		Apply(theme);
	}

	public static void UseSystemTheme()
	{
		followsSystemTheme = true;
		AttachToApplication(Application.Current);
		ApplySystemTheme();
	}

	internal static void Initialize(Application application)
	{
		ArgumentNullException.ThrowIfNull(application);
		AttachToApplication(application);

		if (followsSystemTheme)
			ApplySystemTheme();
		else
			ApplyResources(current, application.Resources);
	}

	private static void Apply(FuchsTheme theme)
	{
		if (!ReferenceEquals(current, theme))
		{
			current.PropertyChanged -= OnCurrentThemePropertyChanged;
			current = theme;
			current.PropertyChanged += OnCurrentThemePropertyChanged;
		}

		ApplyResources(current, Application.Current?.Resources);
		ThemeChanged?.Invoke(null, EventArgs.Empty);
	}

	private static void ApplySystemTheme()
	{
		var requestedTheme = observedApplication?.RequestedTheme ?? Application.Current?.RequestedTheme ?? AppTheme.Light;
		Apply(requestedTheme == AppTheme.Dark ? darkTheme : lightTheme);
	}

	private static void AttachToApplication(Application? application)
	{
		if (ReferenceEquals(observedApplication, application))
			return;

		if (observedApplication is not null)
			observedApplication.RequestedThemeChanged -= OnRequestedThemeChanged;

		observedApplication = application;
		if (observedApplication is not null)
			observedApplication.RequestedThemeChanged += OnRequestedThemeChanged;
	}

	private static void OnRequestedThemeChanged(object? sender, AppThemeChangedEventArgs e)
	{
		if (followsSystemTheme)
			Apply(e.RequestedTheme == AppTheme.Dark ? darkTheme : lightTheme);
	}

	private static void OnCurrentThemePropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		ApplyResources(current, Application.Current?.Resources);
		ThemeChanged?.Invoke(null, EventArgs.Empty);
	}

	private static void ApplyResources(FuchsTheme theme, ResourceDictionary? resources)
	{
		if (resources is null)
			return;

		resources[FuchsThemeResourceKeys.BackgroundColor] = theme.BackgroundColor;
		resources[FuchsThemeResourceKeys.FieldBackgroundColor] = theme.FieldBackgroundColor;
		resources[FuchsThemeResourceKeys.TextColor] = theme.TextColor;
		resources[FuchsThemeResourceKeys.MutedTextColor] = theme.MutedTextColor;
		resources[FuchsThemeResourceKeys.FieldBorderColor] = theme.FieldBorderColor;
		resources[FuchsThemeResourceKeys.FieldFocusColor] = theme.FieldFocusColor;
		resources[FuchsThemeResourceKeys.FieldValidColor] = theme.FieldValidColor;
		resources[FuchsThemeResourceKeys.FieldWarningColor] = theme.FieldWarningColor;
		resources[FuchsThemeResourceKeys.FieldInvalidColor] = theme.FieldInvalidColor;
		resources[FuchsThemeResourceKeys.DefaultColor] = theme.DefaultColor;
		resources[FuchsThemeResourceKeys.PrimaryColor] = theme.PrimaryColor;
		resources[FuchsThemeResourceKeys.SecondaryColor] = theme.SecondaryColor;
		resources[FuchsThemeResourceKeys.SuccessColor] = theme.SuccessColor;
		resources[FuchsThemeResourceKeys.InfoColor] = theme.InfoColor;
		resources[FuchsThemeResourceKeys.WarningColor] = theme.WarningColor;
		resources[FuchsThemeResourceKeys.DangerColor] = theme.DangerColor;
		resources[FuchsThemeResourceKeys.LightColor] = theme.LightColor;
		resources[FuchsThemeResourceKeys.DarkColor] = theme.DarkColor;
		resources[FuchsThemeResourceKeys.PrimaryLight] = theme.PrimaryLight;
		resources[FuchsThemeResourceKeys.BorderWidth] = theme.BorderWidth;
		resources[FuchsThemeResourceKeys.CornerRadius] = new CornerRadius(theme.CornerRadius);
		resources[FuchsThemeResourceKeys.ButtonCornerRadius] = (int)Math.Round(theme.CornerRadius);
		resources[FuchsThemeResourceKeys.FieldHeight] = theme.FieldHeight;
		resources[FuchsThemeResourceKeys.MultilineFieldMinimumHeight] = theme.MultilineFieldMinimumHeight;
		resources[FuchsThemeResourceKeys.BodyFontSize] = theme.BodyFontSize;
		resources[FuchsThemeResourceKeys.CaptionFontSize] = theme.CaptionFontSize;
		resources[FuchsThemeResourceKeys.SubtitleFontSize] = theme.SubtitleFontSize;
		resources[FuchsThemeResourceKeys.H1FontSize] = theme.H1FontSize;
		resources[FuchsThemeResourceKeys.H2FontSize] = theme.H2FontSize;
		resources[FuchsThemeResourceKeys.H3FontSize] = theme.H3FontSize;
		resources[FuchsThemeResourceKeys.H4FontSize] = theme.H4FontSize;
		resources[FuchsThemeResourceKeys.H5FontSize] = theme.H5FontSize;
		resources[FuchsThemeResourceKeys.H6FontSize] = theme.H6FontSize;
		resources[FuchsThemeResourceKeys.BodyLineHeight] = theme.BodyLineHeight;
	}
}