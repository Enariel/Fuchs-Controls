namespace FuchsControls;

public sealed class FuchsButton : Button
{
	public static readonly BindableProperty VariantProperty = BindableProperty.Create(
		nameof(Variant), typeof(FuchsVariant), typeof(FuchsButton), FuchsVariant.Filled, BindingMode.TwoWay, propertyChanged: OnStyleChanged);

	public static readonly BindableProperty ColorProperty = BindableProperty.Create(
		nameof(Color), typeof(FuchsThemeColor), typeof(FuchsButton), FuchsThemeColor.Default, BindingMode.TwoWay, propertyChanged: OnStyleChanged);

	public FuchsButton()
	{
		Padding = new Thickness(14, 10, 14, 8);
		Margin = new Thickness(5);
		CornerRadius = 4;
		FontAttributes = FontAttributes.Bold;
		FuchsThemeManager.ThemeChanged += (_, _) => ApplyTheme();
		ApplyTheme();
	}

	public FuchsVariant Variant
	{
		get => (FuchsVariant)GetValue(VariantProperty);
		set => SetValue(VariantProperty, value);
	}

	public FuchsThemeColor Color
	{
		get => (FuchsThemeColor)GetValue(ColorProperty);
		set => SetValue(ColorProperty, value);
	}

	private static void OnStyleChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsButton)bindable).ApplyTheme();

	private void ApplyTheme()
	{
		var color = FuchsThemeResourceLookup.GetColor(Color);
		var foreground = Color is FuchsThemeColor.Light ? FuchsThemeManager.Current.TextColor : FuchsThemeManager.Current.LightColor;
		BackgroundColor = Variant == FuchsVariant.Filled ? color : Colors.Transparent;
		TextColor = Variant == FuchsVariant.Filled ? foreground : color;
		BorderColor = color;
		BorderWidth = Variant == FuchsVariant.Outlined ? 1 : 0;
		if (Variant == FuchsVariant.Filled)
		{
			Shadow = new Shadow { Brush = new SolidColorBrush(color), Offset = new Point(0, 2), Radius = 0, Opacity = 0.85f };
		}
		else
		{
			ClearValue(ShadowProperty);
		}
	}
}