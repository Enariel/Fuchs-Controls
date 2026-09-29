namespace FuchsControls;

public sealed partial class FuchsTypo : Label
{
	public static readonly BindableProperty ThemeColorProperty = BindableProperty.Create(
		nameof(Color), typeof(FuchsThemeColor), typeof(FuchsTypo), FuchsThemeColor.Text, BindingMode.TwoWay, propertyChanged: OnThemeColorChanged);

	public static readonly BindableProperty TypeProperty = BindableProperty.Create(
		nameof(Typo), typeof(FuchsTypoType), typeof(FuchsTypo), FuchsTypoType.Body, BindingMode.TwoWay, propertyChanged: OnTypeChanged);

	public static readonly BindableProperty AccessibilityLabelProperty = BindableProperty.Create(
		nameof(AccessibilityLabel), typeof(string), typeof(FuchsTypo), string.Empty, propertyChanged: OnAccessibilityChanged);

	public static readonly BindableProperty AccessibilityHintProperty = BindableProperty.Create(
		nameof(AccessibilityHint), typeof(string), typeof(FuchsTypo), string.Empty, propertyChanged: OnAccessibilityChanged);

	public FuchsTypo()
	{
		ApplyStyle();
		PropertyChanged += OnPropertyChanged;
	}

	public FuchsThemeColor Color
	{
		get => (FuchsThemeColor)GetValue(ThemeColorProperty);
		set => SetValue(ThemeColorProperty, value);
	}

	public FuchsTypoType Typo
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

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
		FuchsThemeManager.ThemeChanged -= OnThemeChanged;
		if (Handler is null)
		{
			return;
		}

		FuchsThemeManager.ThemeChanged += OnThemeChanged;
		ApplyThemeColor();
	}

	private static void OnTypeChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsTypo)bindable).ApplyStyle();
	private static void OnThemeColorChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsTypo)bindable).ApplyThemeColor();
	private static void OnAccessibilityChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsTypo)bindable).UpdateAccessibility();
	private void OnThemeChanged(object? sender, EventArgs e) => ApplyThemeColor();
	private void ApplyThemeColor() => TextColor = FuchsThemeResourceLookup.GetColor(Color);

	private void ApplyStyle()
	{
		SetDynamicResource(VisualElement.StyleProperty, Typo switch
														{
															FuchsTypoType.Caption => "FuchsTypoCaptionStyle"
															, FuchsTypoType.Subtitle => "FuchsTypoSubtitleStyle"
															, FuchsTypoType.H1 => "FuchsTypoH1Style"
															, FuchsTypoType.H2 => "FuchsTypoH2Style"
															, FuchsTypoType.H3 => "FuchsTypoH3Style"
															, FuchsTypoType.H4 => "FuchsTypoH4Style"
															, FuchsTypoType.H5 => "FuchsTypoH5Style"
															, FuchsTypoType.H6 => "FuchsTypoH6Style"
															, _ => "FuchsTypoBodyStyle"
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