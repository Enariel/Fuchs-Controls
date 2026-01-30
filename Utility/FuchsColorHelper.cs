#region Meta

// FuchsControls
// Created: 29/01/2026
// Modified: 29/01/2026

#endregion

namespace FuchsControls.Utility;

public static class FuchsColorHelper
{
	public static readonly IReadOnlyDictionary<FuchsColor, Color> Colors =
		new Dictionary<FuchsColor, Color>
		{
			// Main background/text
			{ FuchsColor.BgColor, Color.FromArgb("#FFFFFF") }, // FuchsBgColor
			{ FuchsColor.TextColor, Color.FromArgb("#2E4051") }, // FuchsTextColor

			// Main semantic colors (primary only)
			{ FuchsColor.Accent, Color.FromArgb("#1CB0F6") }, // FuchsAccent
			{ FuchsColor.Success, Color.FromArgb("#58CC02") }, // FuchsSuccess
			{ FuchsColor.Info, Color.FromArgb("#1CB0F6") }, // FuchsInfo
			{ FuchsColor.Warning, Color.FromArgb("#FF9600") }, // FuchsWarning
			{ FuchsColor.Danger, Color.FromArgb("#FF4B4B") }, // FuchsDanger

			// Convenience mappings
			{ FuchsColor.White, Color.FromArgb("#FFFFFF") }, // lightest theme color (FuchsLightLight)
			{ FuchsColor.Black, Color.FromArgb("#121A20") } // darkest theme color (FuchsDarkDarker)
		};

	public static Color GetColor(FuchsColor key) => Colors[key];

	public static bool TryGetColor(FuchsColor key, out Color color)
		=> ((Dictionary<FuchsColor, Color>)Colors).TryGetValue(key, out color);
}