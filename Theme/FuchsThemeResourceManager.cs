namespace FuchsControls.Theme;

public static class FuchsThemeProvider
{
	private static FuchsTheme _current = FuchsTheme.Default;

	public static event EventHandler? ThemeChanged;

	public static FuchsTheme Current
	{
		get => _current;
		set
		{
			_current = value ?? FuchsTheme.Default;
			ThemeChanged?.Invoke(null, EventArgs.Empty);
		}
	}

	public static void ApplyTo(Application application, FuchsTheme? theme = null)
	{
		Current = theme ?? FuchsTheme.Default;

		ResourceDictionary resources = application.Resources;

		resources["FuchsBackgroundColor"] = Current.Background;
		resources["FuchsBackgroundDarkColor"] = Current.BackgroundDark;
		resources["FuchsBackgroundDarkerColor"] = Current.BackgroundDarker;
		resources["FuchsBackgroundDarkestColor"] = Current.BackgroundDarkest;

		resources["FuchsTextColor"] = Current.Text;
		resources["FuchsTextLightColor"] = Current.TextLight;
		resources["FuchsTextDarkColor"] = Current.TextDark;
		resources["FuchsTextInvertedColor"] = Current.TextInverted;

		resources["FuchsAccentColor"] = Current.Primary;
		resources["FuchsAccentLightColor"] = Current.PrimaryLight;
		resources["FuchsAccentDarkColor"] = Current.PrimaryDark;
		resources["FuchsAccentDarkerColor"] = Current.PrimaryDarker;

		resources["FuchsSuccessColor"] = Current.Success;
		resources["FuchsSuccessLightColor"] = Current.SuccessLight;
		resources["FuchsSuccessDarkColor"] = Current.SuccessDark;
		resources["FuchsSuccessDarkerColor"] = Current.SuccessDarker;

		resources["FuchsInfoColor"] = Current.Info;
		resources["FuchsWarningColor"] = Current.Warning;
		resources["FuchsDangerColor"] = Current.Danger;

		resources["FuchsBorderWidth"] = Current.BorderWidth;
		resources["FuchsCornerRadius"] = Current.CornerRadius;
		resources["FuchsCornerRadiusSmall"] = Current.CornerRadiusSmall;
		resources["FuchsCornerRadiusMedium"] = Current.CornerRadiusMedium;
		resources["FuchsCornerRadiusLarge"] = Current.CornerRadiusLarge;

		resources["FuchsButtonPadding"] = Current.ButtonPadding;
		resources["FuchsControlPadding"] = Current.ControlPadding;
		resources["FuchsCardPadding"] = Current.CardPadding;
		resources["FuchsPanelPadding"] = Current.PanelPadding;
	}
}
