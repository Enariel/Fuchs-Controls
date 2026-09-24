namespace FuchsControls.Theme;

public sealed class FuchsTheme
{
	public Color Background { get; set; } = Color.FromArgb("#FFFFFF");
	public Color BackgroundDark { get; set; } = Color.FromArgb("#F1F4F7");
	public Color BackgroundDarker { get; set; } = Color.FromArgb("#CED9E3");
	public Color BackgroundDarkest { get; set; } = Color.FromArgb("#809CB6");

	public Color Text { get; set; } = Color.FromArgb("#2E4051");
	public Color TextLight { get; set; } = Color.FromArgb("#758696");
	public Color TextDark { get; set; } = Color.FromArgb("#1E2A35");
	public Color TextInverted { get; set; } = Color.FromArgb("#FFFFFF");

	public Color Primary { get; set; } = Color.FromArgb("#1CB0F6");
	public Color PrimaryLight { get; set; } = Color.FromArgb("#77D0FA");
	public Color PrimaryDark { get; set; } = Color.FromArgb("#1896D1");
	public Color PrimaryDarker { get; set; } = Color.FromArgb("#0E587B");

	public Color Success { get; set; } = Color.FromArgb("#58CC02");
	public Color SuccessLight { get; set; } = Color.FromArgb("#9BEB67");
	public Color SuccessDark { get; set; } = Color.FromArgb("#4BAE02");
	public Color SuccessDarker { get; set; } = Color.FromArgb("#2C6601");

	public Color Info { get; set; } = Color.FromArgb("#1CB0F6");
	public Color Warning { get; set; } = Color.FromArgb("#FF9600");
	public Color Danger { get; set; } = Color.FromArgb("#FF4B4B");

	public double BorderWidth { get; set; } = 2;
	public double CornerRadius { get; set; } = 16;
	public double CornerRadiusSmall { get; set; } = 8;
	public double CornerRadiusMedium { get; set; } = 12;
	public double CornerRadiusLarge { get; set; } = 20;

	public Thickness ButtonPadding { get; set; } = new(14, 12, 14, 8);
	public Thickness ControlPadding { get; set; } = new(14, 10);
	public Thickness CardPadding { get; set; } = new(12);
	public Thickness PanelPadding { get; set; } = new(16);

	public double FontSizeXs { get; set; } = 12;
	public double FontSizeSm { get; set; } = 14;
	public double FontSizeMd { get; set; } = 16;
	public double FontSizeLg { get; set; } = 20;
	public double FontSizeXl { get; set; } = 24;
	public double FontSize2Xl { get; set; } = 32;
	public double FontSize3Xl { get; set; } = 40;
	public double FontSize4Xl { get; set; } = 48;

	public static FuchsTheme Default { get; } = new FuchsTheme();

	public Color GetMainColor(FuchsColor color) =>
		color switch
		{
			FuchsColor.Primary => Primary,
			FuchsColor.Secondary => PrimaryDark,
			FuchsColor.Success => Success,
			FuchsColor.Info => Info,
			FuchsColor.Warning => Warning,
			FuchsColor.Danger => Danger,
			FuchsColor.Light => BackgroundDark,
			FuchsColor.Dark => Text,
			_ => BackgroundDark
		};

	public Color GetBorderColor(FuchsColor color) =>
		color switch
		{
			FuchsColor.Primary => PrimaryDark,
			FuchsColor.Secondary => PrimaryDarker,
			FuchsColor.Success => SuccessDark,
			FuchsColor.Info => PrimaryDark,
			FuchsColor.Warning => Color.FromArgb("#D87F00"),
			FuchsColor.Danger => Color.FromArgb("#D84040"),
			FuchsColor.Light => BackgroundDarker,
			FuchsColor.Dark => TextDark,
			_ => BackgroundDarker
		};

	public Color GetTextColor(FuchsColor color, FuchsVariant variant)
	{
		if (variant is FuchsVariant.Outlined or FuchsVariant.Text)
			return color == FuchsColor.Default ? Text : GetBorderColor(color);

		return color switch
		{
			FuchsColor.Default or FuchsColor.Light => Text,
			_ => TextInverted
		};
	}
}
