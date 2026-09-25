namespace FuchsControls;

public sealed class FuchsButton : Button
{
	public static readonly BindableProperty VariantProperty = BindableProperty.Create(
		nameof(Variant), typeof(FuchsVariant), typeof(FuchsButton), FuchsVariant.Filled, propertyChanged: OnStyleChanged);

	public static readonly BindableProperty ColorProperty = BindableProperty.Create(
		nameof(Color), typeof(FuchsThemeColor), typeof(FuchsButton), FuchsThemeColor.Default, propertyChanged: OnStyleChanged);

	public FuchsButton()
	{
		SetDynamicResource(StyleProperty, "FuchsButtonStyle");
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
		var foreground = Color is FuchsThemeColor.Light ? Colors.Black : Colors.White;
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