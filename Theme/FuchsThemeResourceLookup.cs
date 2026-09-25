namespace FuchsControls;

internal static class FuchsThemeResourceLookup
{
	public static Color GetColor(FuchsThemeColor color, bool light = false)
	{
		var key = $"Fuchs{color}{(light ? "Light" : "Color")}";
		if (Application.Current?.Resources.TryGetValue(key, out var resource) == true && resource is Color resourceColor)
		{
			return resourceColor;
		}

		return color switch
			   {
				   FuchsThemeColor.Primary => Colors.DodgerBlue, FuchsThemeColor.Secondary => Colors.SlateGray, FuchsThemeColor.Success => Colors.SeaGreen
				   , FuchsThemeColor.Info => Colors.DeepSkyBlue, FuchsThemeColor.Warning => Colors.DarkOrange, FuchsThemeColor.Danger => Colors.IndianRed
				   , FuchsThemeColor.Light => Colors.WhiteSmoke, FuchsThemeColor.Dark => Colors.DarkSlateGray, _ => Colors.SlateGray
			   };
	}
}