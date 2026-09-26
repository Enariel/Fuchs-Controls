namespace FuchsControls;

public sealed partial class FuchsTypo : Label
{
	public static readonly BindableProperty TypeProperty = BindableProperty.Create(
		nameof(Type), typeof(FuchsTypoType), typeof(FuchsTypo), FuchsTypoType.Body, BindingMode.TwoWay, propertyChanged: OnTypeChanged);

	public static readonly BindableProperty AccessibilityLabelProperty = BindableProperty.Create(
		nameof(AccessibilityLabel), typeof(string), typeof(FuchsTypo), string.Empty, propertyChanged: OnAccessibilityChanged);

	public static readonly BindableProperty AccessibilityHintProperty = BindableProperty.Create(
		nameof(AccessibilityHint), typeof(string), typeof(FuchsTypo), string.Empty, propertyChanged: OnAccessibilityChanged);

	public FuchsTypo()
	{
		ApplyStyle();
		FuchsThemeManager.ThemeChanged += OnThemeChanged;
		PropertyChanged += OnPropertyChanged;
	}

	public FuchsTypoType Type
	{
		get => (FuchsTypoType)GetValue(TypeProperty);
		set => SetValue(TypeProperty, value);
	}

	public string AccessibilityLabel
	{
		get => (string)GetValue(AccessibilityLabelProperty);
		set => SetValue(AccessibilityLabelProperty, value);
	}

	public string AccessibilityHint
	{
		get => (string)GetValue(AccessibilityHintProperty);
		set => SetValue(AccessibilityHintProperty, value);
	}

	private static void OnTypeChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsTypo)bindable).ApplyStyle();
	private static void OnAccessibilityChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsTypo)bindable).UpdateAccessibility();

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

	private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(Text))
		{
			UpdateAccessibility();
		}
	}

	private void UpdateAccessibility()
	{
		SemanticProperties.SetDescription(this, string.IsNullOrWhiteSpace(AccessibilityLabel) ? Text : AccessibilityLabel);
		SemanticProperties.SetHint(this, AccessibilityHint);
	}
}