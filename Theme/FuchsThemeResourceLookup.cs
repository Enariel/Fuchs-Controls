namespace FuchsControls;

internal static class FuchsThemeResourceLookup
{
	public static string GetButtonStyleKey(FuchsVariant variant) => variant switch
																	{
																		FuchsVariant.Outlined => FuchsThemeResourceKeys.OutlinedButtonStyle
																		, FuchsVariant.Text => FuchsThemeResourceKeys.TextButtonStyle
																		, _ => FuchsThemeResourceKeys.FilledButtonStyle
																	};

	public static Color GetColor(FuchsThemeColor color)
	{
		var theme = FuchsThemeManager.Current;
		return color switch
			   {
				   FuchsThemeColor.Text => theme.TextColor, FuchsThemeColor.Muted => theme.MutedTextColor
				   , FuchsThemeColor.Background => theme.BackgroundColor, FuchsThemeColor.Primary => theme.PrimaryColor
				   , FuchsThemeColor.Secondary => theme.SecondaryColor
				   , FuchsThemeColor.Success => theme.SuccessColor, FuchsThemeColor.Info => theme.InfoColor, FuchsThemeColor.Warning => theme.WarningColor
				   , FuchsThemeColor.Danger => theme.DangerColor, FuchsThemeColor.Light => theme.LightColor, FuchsThemeColor.Dark => theme.DarkColor
				   , _ => theme.DefaultColor
			   };
	}
}