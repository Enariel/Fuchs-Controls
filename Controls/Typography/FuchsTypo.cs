namespace FuchsControls;

public sealed partial class FuchsTypo : Label
{
	public static readonly BindableProperty TypeProperty = BindableProperty.Create(
		nameof(Type), typeof(FuchsTypoType), typeof(FuchsTypo), FuchsTypoType.Body, BindingMode.TwoWay, propertyChanged: OnTypeChanged);

	public FuchsTypo()
	{
		ApplyStyle();
		FuchsThemeManager.ThemeChanged += OnThemeChanged;
	}

	public FuchsTypoType Type
	{
		get => (FuchsTypoType)GetValue(TypeProperty);
		set => SetValue(TypeProperty, value);
	}

	private static void OnTypeChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsTypo)bindable).ApplyStyle();

	private void ApplyStyle()
	{
		TextColor = FuchsThemeManager.Current.TextColor;
		SetDynamicResource(Label.TextColorProperty, FuchsThemeResourceKeys.TextColor);
		SetDynamicResource(Label.FontSizeProperty, Type switch
												   {
													   FuchsTypoType.Caption => FuchsThemeResourceKeys.CaptionFontSize
													   , FuchsTypoType.Subtitle => FuchsThemeResourceKeys.SubtitleFontSize
													   , _ => FuchsThemeResourceKeys.BodyFontSize
												   });
	}

	private void OnThemeChanged(object? sender, EventArgs e) => ApplyStyle();
}