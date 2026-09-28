namespace FuchsControls;

internal static class FuchsThemeResourceLookup
{
	public static Color GetColor(FuchsThemeColor color, bool light = false)
	{
		var key = $"Fuchs{color}{(light ? "Light" : "Color")}";
		if (Application.Current?.Resources.TryGetValue(key, out var resource) == true && resource is Color resourceColor)
			return resourceColor;

		var theme = FuchsThemeManager.Current;
		return color switch
			   {
				   FuchsThemeColor.Primary => light ? theme.PrimaryLight : theme.PrimaryColor, FuchsThemeColor.Secondary => theme.SecondaryColor
				   , FuchsThemeColor.Success => theme.SuccessColor, FuchsThemeColor.Info => theme.InfoColor, FuchsThemeColor.Warning => theme.WarningColor
				   , FuchsThemeColor.Danger => theme.DangerColor, FuchsThemeColor.Light => theme.LightColor, FuchsThemeColor.Dark => theme.DarkColor
				   , _ => theme.DefaultColor
			   };
	}
}