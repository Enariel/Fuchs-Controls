namespace FuchsControls;

public sealed class FuchsTheme : BindableObject
{
	public static readonly BindableProperty BackgroundColorProperty = CreateColorProperty(nameof(BackgroundColor));
	public static readonly BindableProperty FieldBackgroundColorProperty = CreateColorProperty(nameof(FieldBackgroundColor));
	public static readonly BindableProperty TextColorProperty = CreateColorProperty(nameof(TextColor));
	public static readonly BindableProperty MutedTextColorProperty = CreateColorProperty(nameof(MutedTextColor));
	public static readonly BindableProperty FieldBorderColorProperty = CreateColorProperty(nameof(FieldBorderColor));
	public static readonly BindableProperty FieldFocusColorProperty = CreateColorProperty(nameof(FieldFocusColor));
	public static readonly BindableProperty FieldValidColorProperty = CreateColorProperty(nameof(FieldValidColor));
	public static readonly BindableProperty FieldWarningColorProperty = CreateColorProperty(nameof(FieldWarningColor));
	public static readonly BindableProperty FieldInvalidColorProperty = CreateColorProperty(nameof(FieldInvalidColor));
	public static readonly BindableProperty DefaultColorProperty = CreateColorProperty(nameof(DefaultColor));
	public static readonly BindableProperty PrimaryColorProperty = CreateColorProperty(nameof(PrimaryColor));
	public static readonly BindableProperty SecondaryColorProperty = CreateColorProperty(nameof(SecondaryColor));
	public static readonly BindableProperty SuccessColorProperty = CreateColorProperty(nameof(SuccessColor));
	public static readonly BindableProperty InfoColorProperty = CreateColorProperty(nameof(InfoColor));
	public static readonly BindableProperty WarningColorProperty = CreateColorProperty(nameof(WarningColor));
	public static readonly BindableProperty DangerColorProperty = CreateColorProperty(nameof(DangerColor));
	public static readonly BindableProperty LightColorProperty = CreateColorProperty(nameof(LightColor));
	public static readonly BindableProperty DarkColorProperty = CreateColorProperty(nameof(DarkColor));
	public static readonly BindableProperty PrimaryLightProperty = CreateColorProperty(nameof(PrimaryLight));
	public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create(nameof(BorderWidth), typeof(double), typeof(FuchsTheme), 1d);
	public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(FuchsTheme), 4d);
	public static readonly BindableProperty FieldHeightProperty = BindableProperty.Create(nameof(FieldHeight), typeof(double), typeof(FuchsTheme), 42d);

	public static readonly BindableProperty MultilineFieldMinimumHeightProperty =
		BindableProperty.Create(nameof(MultilineFieldMinimumHeight), typeof(double), typeof(FuchsTheme), 88d);

	public static readonly BindableProperty BodyFontSizeProperty = BindableProperty.Create(nameof(BodyFontSize), typeof(double), typeof(FuchsTheme), 16d);
	public static readonly BindableProperty CaptionFontSizeProperty = BindableProperty.Create(nameof(CaptionFontSize), typeof(double), typeof(FuchsTheme), 12d);

	public static readonly BindableProperty SubtitleFontSizeProperty =
		BindableProperty.Create(nameof(SubtitleFontSize), typeof(double), typeof(FuchsTheme), 20d);

	public static readonly BindableProperty H1FontSizeProperty = BindableProperty.Create(nameof(H1FontSize), typeof(double), typeof(FuchsTheme), 56d);
	public static readonly BindableProperty H2FontSizeProperty = BindableProperty.Create(nameof(H2FontSize), typeof(double), typeof(FuchsTheme), 48d);
	public static readonly BindableProperty H3FontSizeProperty = BindableProperty.Create(nameof(H3FontSize), typeof(double), typeof(FuchsTheme), 37.6d);
	public static readonly BindableProperty H4FontSizeProperty = BindableProperty.Create(nameof(H4FontSize), typeof(double), typeof(FuchsTheme), 32d);
	public static readonly BindableProperty H5FontSizeProperty = BindableProperty.Create(nameof(H5FontSize), typeof(double), typeof(FuchsTheme), 26.4d);
	public static readonly BindableProperty H6FontSizeProperty = BindableProperty.Create(nameof(H6FontSize), typeof(double), typeof(FuchsTheme), 21.6d);

	public static readonly BindableProperty BodyLineHeightProperty = BindableProperty.Create(nameof(BodyLineHeight), typeof(double), typeof(FuchsTheme), 1.5d);

	public static readonly BindableProperty
		SmallLineHeightProperty = BindableProperty.Create(nameof(SmallLineHeight), typeof(double), typeof(FuchsTheme), 1.2d);

	public static readonly BindableProperty BaseLineHeightProperty = BindableProperty.Create(nameof(BaseLineHeight), typeof(double), typeof(FuchsTheme), 1.5d);

	public Color BackgroundColor
	{
		get => (Color)GetValue(BackgroundColorProperty);
		set => SetValue(BackgroundColorProperty, value);
	}

	public Color FieldBackgroundColor
	{
		get => (Color)GetValue(FieldBackgroundColorProperty);
		set => SetValue(FieldBackgroundColorProperty, value);
	}

	public Color TextColor
	{
		get => (Color)GetValue(TextColorProperty);
		set => SetValue(TextColorProperty, value);
	}

	public Color MutedTextColor
	{
		get => (Color)GetValue(MutedTextColorProperty);
		set => SetValue(MutedTextColorProperty, value);
	}

	public Color FieldBorderColor
	{
		get => (Color)GetValue(FieldBorderColorProperty);
		set => SetValue(FieldBorderColorProperty, value);
	}

	public Color FieldFocusColor
	{
		get => (Color)GetValue(FieldFocusColorProperty);
		set => SetValue(FieldFocusColorProperty, value);
	}

	public Color FieldValidColor
	{
		get => (Color)GetValue(FieldValidColorProperty);
		set => SetValue(FieldValidColorProperty, value);
	}

	public Color FieldWarningColor
	{
		get => (Color)GetValue(FieldWarningColorProperty);
		set => SetValue(FieldWarningColorProperty, value);
	}

	public Color FieldInvalidColor
	{
		get => (Color)GetValue(FieldInvalidColorProperty);
		set => SetValue(FieldInvalidColorProperty, value);
	}

	public Color DefaultColor
	{
		get => (Color)GetValue(DefaultColorProperty);
		set => SetValue(DefaultColorProperty, value);
	}

	public Color PrimaryColor
	{
		get => (Color)GetValue(PrimaryColorProperty);
		set => SetValue(PrimaryColorProperty, value);
	}

	public Color SecondaryColor
	{
		get => (Color)GetValue(SecondaryColorProperty);
		set => SetValue(SecondaryColorProperty, value);
	}

	public Color SuccessColor
	{
		get => (Color)GetValue(SuccessColorProperty);
		set => SetValue(SuccessColorProperty, value);
	}

	public Color InfoColor
	{
		get => (Color)GetValue(InfoColorProperty);
		set => SetValue(InfoColorProperty, value);
	}

	public Color WarningColor
	{
		get => (Color)GetValue(WarningColorProperty);
		set => SetValue(WarningColorProperty, value);
	}

	public Color DangerColor
	{
		get => (Color)GetValue(DangerColorProperty);
		set => SetValue(DangerColorProperty, value);
	}

	public Color LightColor
	{
		get => (Color)GetValue(LightColorProperty);
		set => SetValue(LightColorProperty, value);
	}

	public Color DarkColor
	{
		get => (Color)GetValue(DarkColorProperty);
		set => SetValue(DarkColorProperty, value);
	}

	public Color PrimaryLight
	{
		get => (Color)GetValue(PrimaryLightProperty);
		set => SetValue(PrimaryLightProperty, value);
	}

	public double BorderWidth
	{
		get => (double)GetValue(BorderWidthProperty);
		set => SetValue(BorderWidthProperty, value);
	}

	public double CornerRadius
	{
		get => (double)GetValue(CornerRadiusProperty);
		set => SetValue(CornerRadiusProperty, value);
	}

	public double FieldHeight
	{
		get => (double)GetValue(FieldHeightProperty);
		set => SetValue(FieldHeightProperty, value);
	}

	public double MultilineFieldMinimumHeight
	{
		get => (double)GetValue(MultilineFieldMinimumHeightProperty);
		set => SetValue(MultilineFieldMinimumHeightProperty, value);
	}

	public double BodyFontSize
	{
		get => (double)GetValue(BodyFontSizeProperty);
		set => SetValue(BodyFontSizeProperty, value);
	}

	public double CaptionFontSize
	{
		get => (double)GetValue(CaptionFontSizeProperty);
		set => SetValue(CaptionFontSizeProperty, value);
	}

	public double SubtitleFontSize
	{
		get => (double)GetValue(SubtitleFontSizeProperty);
		set => SetValue(SubtitleFontSizeProperty, value);
	}

	public double H1FontSize
	{
		get => (double)GetValue(H1FontSizeProperty);
		set => SetValue(H1FontSizeProperty, value);
	}

	public double H2FontSize
	{
		get => (double)GetValue(H2FontSizeProperty);
		set => SetValue(H2FontSizeProperty, value);
	}

	public double H3FontSize
	{
		get => (double)GetValue(H3FontSizeProperty);
		set => SetValue(H3FontSizeProperty, value);
	}

	public double H4FontSize
	{
		get => (double)GetValue(H4FontSizeProperty);
		set => SetValue(H4FontSizeProperty, value);
	}

	public double H5FontSize
	{
		get => (double)GetValue(H5FontSizeProperty);
		set => SetValue(H5FontSizeProperty, value);
	}

	public double H6FontSize
	{
		get => (double)GetValue(H6FontSizeProperty);
		set => SetValue(H6FontSizeProperty, value);
	}

	public double BodyLineHeight
	{
		get => (double)GetValue(BodyLineHeightProperty);
		set => SetValue(BodyLineHeightProperty, value);
	}

	public double SmallLineHeight
	{
		get => (double)GetValue(SmallLineHeightProperty);
		set => SetValue(SmallLineHeightProperty, value);
	}

	public double BaseLineHeight
	{
		get => (double)GetValue(BaseLineHeightProperty);
		set => SetValue(BaseLineHeightProperty, value);
	}

	public static FuchsTheme CreateLight() =>
		new FuchsTheme
		{
			BackgroundColor = Color.FromArgb("#F1F4F7")
			, FieldBackgroundColor = Color.FromArgb("#E5EBF0")
			, TextColor = Color.FromArgb("#141F23")
			, MutedTextColor = Color.FromArgb("#516773")
			, FieldBorderColor = Color.FromArgb("#9FB1BC")
			, FieldFocusColor = Color.FromArgb("#167CC5")
			, FieldValidColor = Color.FromArgb("#298C55")
			, FieldWarningColor = Color.FromArgb("#D07800")
			, FieldInvalidColor = Color.FromArgb("#C84646")
			, DefaultColor = Color.FromArgb("#385661")
			, PrimaryColor = Color.FromArgb("#167CC5")
			, SecondaryColor = Color.FromArgb("#516773")
			, SuccessColor = Color.FromArgb("#298C55")
			, InfoColor = Color.FromArgb("#189DC6")
			, WarningColor = Color.FromArgb("#D07800")
			, DangerColor = Color.FromArgb("#C84646")
			, LightColor = Color.FromArgb("#E5EBF0")
			, DarkColor = Color.FromArgb("#263B43")
			, PrimaryLight = Color.FromArgb("#D7EAF8")
		};

	public static FuchsTheme CreateDark() =>
		new FuchsTheme
		{
			BackgroundColor = Color.FromArgb("#18252B")
			, FieldBackgroundColor = Color.FromArgb("#263B43")
			, TextColor = Color.FromArgb("#F1F4F7")
			, MutedTextColor = Color.FromArgb("#B4C5CD")
			, FieldBorderColor = Color.FromArgb("#607985")
			, FieldFocusColor = Color.FromArgb("#58B6F2")
			, FieldValidColor = Color.FromArgb("#62C98A")
			, FieldWarningColor = Color.FromArgb("#F0A63A")
			, FieldInvalidColor = Color.FromArgb("#F07B7B")
			, DefaultColor = Color.FromArgb("#B4C5CD")
			, PrimaryColor = Color.FromArgb("#58B6F2")
			, SecondaryColor = Color.FromArgb("#9FB1BC")
			, SuccessColor = Color.FromArgb("#62C98A")
			, InfoColor = Color.FromArgb("#58CBEA")
			, WarningColor = Color.FromArgb("#F0A63A")
			, DangerColor = Color.FromArgb("#F07B7B")
			, LightColor = Color.FromArgb("#E5EBF0")
			, DarkColor = Color.FromArgb("#10191D")
			, PrimaryLight = Color.FromArgb("#244D68")
		};

	private static BindableProperty CreateColorProperty(string name) => BindableProperty.Create(name, typeof(Color), typeof(FuchsTheme), Colors.Transparent);
}