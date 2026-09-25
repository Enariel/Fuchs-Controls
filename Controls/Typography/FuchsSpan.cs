namespace FuchsControls;

public sealed partial class FuchsSpan : Span
{
	public static readonly BindableProperty TypeProperty = BindableProperty.Create(
		nameof(Type), typeof(FuchsTypoType), typeof(FuchsSpan), FuchsTypoType.Body, BindingMode.TwoWay, propertyChanged: OnTypeChanged);

	public FuchsSpan()
	{
		ApplyStyle();
		FuchsThemeManager.ThemeChanged += OnThemeChanged;
	}

	public FuchsTypoType Type
	{
		get => (FuchsTypoType)GetValue(TypeProperty);
		set => SetValue(TypeProperty, value);
	}

	private static void OnTypeChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsSpan)bindable).ApplyStyle();

	private void ApplyStyle()
	{
		TextColor = FuchsThemeManager.Current.TextColor;
		SetDynamicResource(Span.TextColorProperty, FuchsThemeResourceKeys.TextColor);
		SetDynamicResource(Span.FontSizeProperty, Type == FuchsTypoType.Caption ? FuchsThemeResourceKeys.CaptionFontSize : FuchsThemeResourceKeys.BodyFontSize);
	}

	private void OnThemeChanged(object? sender, EventArgs e) => ApplyStyle();
}