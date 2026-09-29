namespace FuchsControls;

public sealed partial class FuchsSpan : Span
{
	public static readonly BindableProperty ThemeColorProperty = BindableProperty.Create(
		nameof(ThemeColor), typeof(FuchsThemeColor), typeof(FuchsSpan), FuchsThemeColor.Text, BindingMode.TwoWay, propertyChanged: OnThemeColorChanged);

	public static readonly BindableProperty TypeProperty = BindableProperty.Create(
		nameof(Type), typeof(FuchsTypoType), typeof(FuchsSpan), FuchsTypoType.Body, BindingMode.TwoWay, propertyChanged: OnTypeChanged);

	public static readonly BindableProperty AccessibilityLabelProperty = BindableProperty.Create(
		nameof(AccessibilityLabel), typeof(string), typeof(FuchsSpan), string.Empty, propertyChanged: OnAccessibilityChanged);

	public static readonly BindableProperty AccessibilityHintProperty = BindableProperty.Create(
		nameof(AccessibilityHint), typeof(string), typeof(FuchsSpan), string.Empty, propertyChanged: OnAccessibilityChanged);

	public FuchsSpan()
	{
		ApplyStyle();
		PropertyChanged += OnPropertyChanged;
	}

	public FuchsThemeColor ThemeColor
	{
		get => (FuchsThemeColor)GetValue(ThemeColorProperty);
		set => SetValue(ThemeColorProperty, value);
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

	protected override void OnParentSet()
	{
		base.OnParentSet();
		FuchsThemeManager.ThemeChanged -= OnThemeChanged;
		if (Parent is null)
		{
			return;
		}

		FuchsThemeManager.ThemeChanged += OnThemeChanged;
		ApplyThemeColor();
	}

	private static void OnTypeChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsSpan)bindable).ApplyStyle();
	private static void OnThemeColorChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsSpan)bindable).ApplyThemeColor();
	private static void OnAccessibilityChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsSpan)bindable).UpdateAccessibility();
	private void OnThemeChanged(object? sender, EventArgs e) => ApplyThemeColor();
	private void ApplyThemeColor() => TextColor = FuchsThemeResourceLookup.GetColor(ThemeColor);

	private void ApplyStyle()
	{
		SetDynamicResource(Span.StyleProperty, Type switch
											   {
												   FuchsTypoType.Caption => "FuchsSpanCaptionStyle"
												   , FuchsTypoType.Subtitle => "FuchsSpanSubtitleStyle"
												   , FuchsTypoType.H1 => "FuchsSpanH1Style"
												   , FuchsTypoType.H2 => "FuchsSpanH2Style"
												   , FuchsTypoType.H3 => "FuchsSpanH3Style"
												   , FuchsTypoType.H4 => "FuchsSpanH4Style"
												   , FuchsTypoType.H5 => "FuchsSpanH5Style"
												   , FuchsTypoType.H6 => "FuchsSpanH6Style"
												   , _ => "FuchsSpanBodyStyle"
											   });
		ApplyThemeColor();
	}

	private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(Text))
			UpdateAccessibility();
	}

	private void UpdateAccessibility()
	{
		SemanticProperties.SetDescription(this, string.IsNullOrWhiteSpace(AccessibilityLabel) ? Text : AccessibilityLabel);
		SemanticProperties.SetHint(this, AccessibilityHint);
	}
}