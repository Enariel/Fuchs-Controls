namespace FuchsControls;

public sealed partial class FuchsSpan : Span
{
	public static readonly BindableProperty TypeProperty = BindableProperty.Create(
		nameof(Type), typeof(FuchsTypoType), typeof(FuchsSpan), FuchsTypoType.Body, BindingMode.TwoWay, propertyChanged: OnTypeChanged);

	public static readonly BindableProperty AccessibilityLabelProperty = BindableProperty.Create(
		nameof(AccessibilityLabel), typeof(string), typeof(FuchsSpan), string.Empty, propertyChanged: OnAccessibilityChanged);

	public static readonly BindableProperty AccessibilityHintProperty = BindableProperty.Create(
		nameof(AccessibilityHint), typeof(string), typeof(FuchsSpan), string.Empty, propertyChanged: OnAccessibilityChanged);

	public FuchsSpan()
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

	private static void OnTypeChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsSpan)bindable).ApplyStyle();
	private static void OnAccessibilityChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsSpan)bindable).UpdateAccessibility();

	private void ApplyStyle()
	{
		TextColor = FuchsThemeManager.Current.TextColor;
		SetDynamicResource(Span.TextColorProperty, FuchsThemeResourceKeys.TextColor);
		SetDynamicResource(Span.FontSizeProperty, Type == FuchsTypoType.Caption ? FuchsThemeResourceKeys.CaptionFontSize : FuchsThemeResourceKeys.BodyFontSize);
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