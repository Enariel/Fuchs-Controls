using FuchsControls.Theme;

namespace FuchsControls.Controls;

public class FuchsLabel : Label
{
	public static readonly BindableProperty ColorProperty =
		BindableProperty.Create(nameof(Color), typeof(FuchsColor), typeof(FuchsLabel), FuchsColor.Default, propertyChanged: OnThemePropertyChanged);

	public static readonly BindableProperty TypoProperty =
		BindableProperty.Create(nameof(Typo), typeof(FuchsTextTypo), typeof(FuchsLabel), FuchsTextTypo.Body, propertyChanged: OnThemePropertyChanged);

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

	public FuchsLabel()
	{
		FuchsThemeProvider.ThemeChanged += OnThemeChanged;
		ApplyTheme();
	}

	protected override void OnHandlerChanging(HandlerChangingEventArgs args)
	{
		base.OnHandlerChanging(args);

		if (args.NewHandler is null)
			FuchsThemeProvider.ThemeChanged -= OnThemeChanged;
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

		FontAttributes = Typo is FuchsTextTypo.H1 or FuchsTextTypo.H2 or FuchsTextTypo.H3 or FuchsTextTypo.H4 or FuchsTextTypo.H5 or FuchsTextTypo.H6
			? FontAttributes.Bold
			: FontAttributes.None;

		LineHeight = Typo.ToString().StartsWith("H", StringComparison.Ordinal) ? 1.2 : 1.5;
	}

	private void OnThemeChanged(object? sender, EventArgs e) => ApplyTheme();

	private static void OnThemePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsLabel label)
			label.ApplyTheme();
	}
}
