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
			// Backgrounds
			{ FuchsColor.BgColor, Color.FromArgb("#FFFFFF") },
			{ FuchsColor.BgColor2, Color.FromArgb("#F1F4F7") },
			{ FuchsColor.BgColor3, Color.FromArgb("#CED9E3") },
			{ FuchsColor.BgColor4, Color.FromArgb("#809CB6") },

			// Text
			{ FuchsColor.TextColor, Color.FromArgb("#2E4051") },
			{ FuchsColor.TextColorLight, Color.FromArgb("#77838E") },
			{ FuchsColor.TextColorDark, Color.FromArgb("#1E2A35") },
			{ FuchsColor.TextColorDarker, Color.FromArgb("#121A20") },
			{ FuchsColor.TextColorInverted, Color.FromArgb("#FFFFFF") },

			// Blue
			{ FuchsColor.Blue, Color.FromArgb("#1CB0F6") },
			{ FuchsColor.BlueLight, Color.FromArgb("#77D0FA") },
			{ FuchsColor.BlueDark, Color.FromArgb("#1896D1") },
			{ FuchsColor.BlueDarker, Color.FromArgb("#0E587B") },

			// Green
			{ FuchsColor.Green, Color.FromArgb("#58CC02") },
			{ FuchsColor.GreenLight, Color.FromArgb("#9BE067") },
			{ FuchsColor.GreenDark, Color.FromArgb("#4BAD02") },
			{ FuchsColor.GreenDarker, Color.FromArgb("#2C6601") },

			// Yellow
			{ FuchsColor.Yellow, Color.FromArgb("#FFDE00") },
			{ FuchsColor.YellowLight, Color.FromArgb("#FFEB66") },
			{ FuchsColor.YellowDark, Color.FromArgb("#D9BD00") },
			{ FuchsColor.YellowDarker, Color.FromArgb("#A69000") },

			// Orange
			{ FuchsColor.Orange, Color.FromArgb("#FF9600") },
			{ FuchsColor.OrangeLight, Color.FromArgb("#FFC066") },
			{ FuchsColor.OrangeDark, Color.FromArgb("#D98000") },
			{ FuchsColor.OrangeDarker, Color.FromArgb("#804B00") },

			// Red
			{ FuchsColor.Red, Color.FromArgb("#FF4B4B") },
			{ FuchsColor.RedLight, Color.FromArgb("#FF9393") },
			{ FuchsColor.RedDark, Color.FromArgb("#D94040") },
			{ FuchsColor.RedDarker, Color.FromArgb("#802626") },

			// Pink
			{ FuchsColor.Pink, Color.FromArgb("#FF86D0") },
			{ FuchsColor.PinkLight, Color.FromArgb("#FFB6E3") },
			{ FuchsColor.PinkDark, Color.FromArgb("#D972B1") },
			{ FuchsColor.PinkDarker, Color.FromArgb("#804368") },

			// Purple
			{ FuchsColor.Purple, Color.FromArgb("#C164FF") },
			{ FuchsColor.PurpleLight, Color.FromArgb("#DAA2FF") },
			{ FuchsColor.PurpleDark, Color.FromArgb("#A455D9") },
			{ FuchsColor.PurpleDarker, Color.FromArgb("#603280") },

			// Semantic aliases
			{ FuchsColor.Accent, Color.FromArgb("#1CB0F6") },
			{ FuchsColor.AccentLight, Color.FromArgb("#77D0FA") },
			{ FuchsColor.AccentDark, Color.FromArgb("#1896D1") },
			{ FuchsColor.AccentDarker, Color.FromArgb("#0E587B") },

			{ FuchsColor.Success, Color.FromArgb("#58CC02") },
			{ FuchsColor.SuccessLight, Color.FromArgb("#9BE067") },
			{ FuchsColor.SuccessDark, Color.FromArgb("#4BAD02") },
			{ FuchsColor.SuccessDarker, Color.FromArgb("#2C6601") },

			{ FuchsColor.Info, Color.FromArgb("#1CB0F6") },
			{ FuchsColor.InfoLight, Color.FromArgb("#77D0FA") },
			{ FuchsColor.InfoDark, Color.FromArgb("#1896D1") },
			{ FuchsColor.InfoDarker, Color.FromArgb("#0E587B") },

			{ FuchsColor.Warning, Color.FromArgb("#FF9600") },
			{ FuchsColor.WarningLight, Color.FromArgb("#FFC066") },
			{ FuchsColor.WarningDark, Color.FromArgb("#D98000") },
			{ FuchsColor.WarningDarker, Color.FromArgb("#804B00") },

			{ FuchsColor.Danger, Color.FromArgb("#FF4B4B") },
			{ FuchsColor.DangerLight, Color.FromArgb("#FF9393") },
			{ FuchsColor.DangerDark, Color.FromArgb("#D94040") },
			{ FuchsColor.DangerDarker, Color.FromArgb("#802626") },

			{ FuchsColor.Light, Color.FromArgb("#F1F4F7") },
			{ FuchsColor.LightLight, Color.FromArgb("#FFFFFF") },
			{ FuchsColor.LightDark, Color.FromArgb("#CED9E3") },
			{ FuchsColor.LightDarker, Color.FromArgb("#809CB6") },

			{ FuchsColor.Dark, Color.FromArgb("#2E4051") },
			{ FuchsColor.DarkLight, Color.FromArgb("#77838E") },
			{ FuchsColor.DarkDark, Color.FromArgb("#1E2A35") },
			{ FuchsColor.DarkDarker, Color.FromArgb("#121A20") }
		};

	public static Color GetColor(FuchsColor key) => Colors[key];

	public static bool TryGetColor(FuchsColor key, out Color color)
		=> ((Dictionary<FuchsColor, Color>)Colors).TryGetValue(key, out color);
}
