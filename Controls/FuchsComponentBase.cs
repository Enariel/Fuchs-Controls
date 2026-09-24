using FuchsControls.Theme;

namespace FuchsControls.Controls;

public abstract class FuchsComponent : ContentView
{
	public static readonly BindableProperty ColorProperty =
		BindableProperty.Create(nameof(Color), typeof(FuchsColor), typeof(FuchsComponent), FuchsColor.Default, propertyChanged: OnThemePropertyChanged);

	public static readonly BindableProperty VariantProperty =
		BindableProperty.Create(nameof(Variant), typeof(FuchsVariant), typeof(FuchsComponent), FuchsVariant.Filled, propertyChanged: OnThemePropertyChanged);

	public static readonly BindableProperty SizeProperty =
		BindableProperty.Create(nameof(Size), typeof(FuchsSize), typeof(FuchsComponent), FuchsSize.Medium, propertyChanged: OnThemePropertyChanged);

	public static readonly BindableProperty IsDisabledProperty =
		BindableProperty.Create(nameof(IsDisabled), typeof(bool), typeof(FuchsComponent), false, propertyChanged: OnThemePropertyChanged);

	public FuchsColor Color
	{
		get => (FuchsColor)GetValue(ColorProperty);
		set => SetValue(ColorProperty, value);
	}

	public FuchsVariant Variant
	{
		get => (FuchsVariant)GetValue(VariantProperty);
		set => SetValue(VariantProperty, value);
	}

	public FuchsSize Size
	{
		get => (FuchsSize)GetValue(SizeProperty);
		set => SetValue(SizeProperty, value);
	}

	public bool IsDisabled
	{
		get => (bool)GetValue(IsDisabledProperty);
		set => SetValue(IsDisabledProperty, value);
	}

	protected FuchsComponent()
	{
		FuchsThemeProvider.ThemeChanged += OnThemeChanged;
	}

	protected override void OnHandlerChanging(HandlerChangingEventArgs args)
	{
		base.OnHandlerChanging(args);

		if (args.NewHandler is null)
			FuchsThemeProvider.ThemeChanged -= OnThemeChanged;
	}

	protected virtual void ApplyTheme()
	{
	}

	protected double ResolveFontSize()
	{
		FuchsTheme theme = FuchsThemeProvider.Current;

		return Size switch
		{
			FuchsSize.Small => theme.FontSizeSm,
			FuchsSize.Large => theme.FontSizeLg,
			_ => theme.FontSizeMd
		};
	}

	protected Thickness ResolvePadding()
	{
		FuchsTheme theme = FuchsThemeProvider.Current;

		return Size switch
		{
			FuchsSize.Small => new Thickness(10, 8, 10, 6),
			FuchsSize.Large => new Thickness(18, 14, 18, 10),
			_ => theme.ButtonPadding
		};
	}

	private void OnThemeChanged(object? sender, EventArgs e) => ApplyTheme();

	private static void OnThemePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsComponent component)
			component.ApplyTheme();
	}
}
