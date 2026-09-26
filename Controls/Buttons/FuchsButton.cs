namespace FuchsControls;

public sealed class FuchsButton : FuchsButtonBase
{
	public static readonly BindableProperty VariantProperty = BindableProperty.Create(
		nameof(Variant), typeof(FuchsVariant), typeof(FuchsButton), FuchsVariant.Filled, BindingMode.TwoWay, propertyChanged: OnVariantChanged);

	public FuchsVariant Variant
	{
		get => (FuchsVariant)GetValue(VariantProperty);
		set => SetValue(VariantProperty, value);
	}

	protected override void ApplyThemedStyle()
	{
		var color = ThemeColor;
		BackgroundColor = Variant is FuchsVariant.Filled ? color : Colors.Transparent;
		TextColor = Variant is FuchsVariant.Filled ? ForegroundColor : color;
		BorderColor = color;
		BorderWidth = Variant is FuchsVariant.Outlined ? FuchsThemeManager.Current.BorderWidth : 0;
		if (Variant is FuchsVariant.Filled or FuchsVariant.Outlined)
		{
			Shadow = new Shadow { Brush = new SolidColorBrush(color), Offset = new Point(0, 2), Radius = 0, Opacity = 0.85f };
		}
		else
		{
			ClearValue(ShadowProperty);
		}
	}

	protected override void ApplyVisualStates()
	{
		var borderWidth = FuchsThemeManager.Current.BorderWidth;
		var pressedTranslation = Variant is FuchsVariant.Outlined ? borderWidth * 1.2 : borderWidth * 1.38;
		var group = new VisualStateGroup { Name = "CommonStates" };
		group.States.Add(CreateVisualState("Normal", 1, 0));
		group.States.Add(CreateVisualState("PointerOver", 0.9, 0));
		group.States.Add(CreateVisualState("Pressed", 0.9, pressedTranslation));

		var disabled = CreateVisualState("Disabled", 0.7, borderWidth * 1.38);
		disabled.Setters.Add(new Setter { Property = BackgroundColorProperty, Value = ThemeColor });
		disabled.Setters.Add(new Setter { Property = TextColorProperty, Value = ForegroundColor });
		disabled.Setters.Add(new Setter { Property = BorderWidthProperty, Value = 0d });
		disabled.Setters.Add(new Setter { Property = ShadowProperty, Value = null });
		group.States.Add(disabled);

		VisualStateManager.SetVisualStateGroups(this, new VisualStateGroupList { group });
	}

	private static void OnVariantChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsButton)bindable).ApplyTheme();
}