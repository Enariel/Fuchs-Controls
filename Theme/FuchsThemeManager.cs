namespace FuchsControls;

public static class FuchsThemeManager
{
	private static FuchsTheme current = FuchsTheme.CreateLight();

	public static FuchsTheme Current => current;

	public static event EventHandler? ThemeChanged;

	public static void Apply(FuchsTheme theme, ResourceDictionary? targetResources = null)
	{
		ArgumentNullException.ThrowIfNull(theme);
		current = theme;
		ApplyResources(theme, targetResources);
		if (Application.Current is not null) 
			ApplyResources(theme, Application.Current.Resources);

		ThemeChanged?.Invoke(null, EventArgs.Empty);
	}

	public static ResourceDictionary CreateResources(FuchsTheme theme)
	{
		var resources = new ResourceDictionary();
		ApplyResources(theme, resources);
		return resources;
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
		resources[FuchsThemeResourceKeys.SmallLineHeight] = theme.SmallLineHeight;
		resources[FuchsThemeResourceKeys.BaseLineHeight] = theme.BaseLineHeight;
	}
}