using FuchsControls.Theme;

namespace FuchsControls.Controls;

public class FuchsSpan : Span
{
	public static readonly BindableProperty ColorProperty =
		BindableProperty.Create(nameof(Color), typeof(FuchsColor), typeof(FuchsSpan), FuchsColor.Default, propertyChanged: OnThemePropertyChanged);

	public static readonly BindableProperty TypoProperty =
		BindableProperty.Create(nameof(Typo), typeof(FuchsTextTypo), typeof(FuchsSpan), FuchsTextTypo.Body, propertyChanged: OnThemePropertyChanged);

	public FuchsColor Color
	{
		get => (FuchsColor)GetValue(ColorProperty);
		set => SetValue(ColorProperty, value);
	}

	public FuchsTextTypo Typo
	{
		get => (FuchsTextTypo)GetValue(TypoProperty);
		set => SetValue(TypoProperty, value);
	}

	public FuchsSpan()
	{
		FuchsThemeProvider.ThemeChanged += OnThemeChanged;
		ApplyTheme();
	}

	private void ApplyTheme()
	{
		FuchsTheme theme = FuchsThemeProvider.Current;

		TextColor = Color == FuchsColor.Default ? theme.Text : theme.GetBorderColor(Color);

		FontSize = Typo switch
		{
			FuchsTextTypo.Caption => theme.FontSizeSm,
			FuchsTextTypo.Subtitle => theme.FontSizeLg,
			FuchsTextTypo.H1 => theme.FontSize4Xl,
			FuchsTextTypo.H2 => theme.FontSize3Xl,
			FuchsTextTypo.H3 => theme.FontSize2Xl,
			FuchsTextTypo.H4 => theme.FontSizeXl,
			FuchsTextTypo.H5 => theme.FontSizeLg,
			FuchsTextTypo.H6 => theme.FontSizeMd,
			_ => theme.FontSizeMd
		};

		FontAttributes = Typo.ToString().StartsWith("H", StringComparison.Ordinal)
			? FontAttributes.Bold
			: FontAttributes.None;
	}

	private void OnThemeChanged(object? sender, EventArgs e) => ApplyTheme();

	private static void OnThemePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsSpan span)
			span.ApplyTheme();
	}
}
