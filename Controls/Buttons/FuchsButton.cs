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
		this.ApplyFuchsButtonStyle(Variant);

		var color = ThemeColor;
		BorderColor = color;
		TextColor = Variant is FuchsVariant.Filled ? ForegroundColor : color;
		if (Variant is FuchsVariant.Filled)
		{
			BackgroundColor = color;

			return;
		}

		ClearValue(BackgroundColorProperty);
	}

	private static void OnVariantChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var button = (FuchsButton)bindable;
		button.ApplyTheme();
	}
}