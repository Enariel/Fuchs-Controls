using Microsoft.Maui.Controls.Shapes;

namespace FuchsControls;

[ContentProperty(nameof(Content))]
public sealed partial class FuchsCard : Border
{
	private const double PaddingScale = 0.75;
	private bool isThemeChangeSubscribed;
	private bool hasAppliedTheme;
	private Thickness themePadding;

	public static readonly BindableProperty CardBackgroundColorProperty = BindableProperty.Create(
		nameof(CardBackgroundColor), typeof(Color), typeof(FuchsCard), propertyChanged: OnAppearanceChanged);

	public static readonly BindableProperty CardBorderColorProperty = BindableProperty.Create(
		nameof(CardBorderColor), typeof(Color), typeof(FuchsCard), propertyChanged: OnAppearanceChanged);

	public FuchsCard()
	{
		HorizontalOptions = LayoutOptions.Fill;
		ApplyTheme();
	}

	/// <summary>
	/// Gets or sets an optional background color. When unset, the current theme background color is used.
	/// </summary>
	public Color? CardBackgroundColor
	{
		get => (Color?)GetValue(CardBackgroundColorProperty);
		set => SetValue(CardBackgroundColorProperty, value);
	}

	/// <summary>
	/// Gets or sets an optional border color. When unset, the current theme field border color is used.
	/// </summary>
	public Color? CardBorderColor
	{
		get => (Color?)GetValue(CardBorderColorProperty);
		set => SetValue(CardBorderColorProperty, value);
	}

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
		if (Handler is null)
		{
			UnsubscribeFromThemeChanges();
			return;
		}

		SubscribeToThemeChanges();
		ApplyTheme();
	}

	private static void OnAppearanceChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsCard)bindable).ApplyTheme();

	private void ApplyTheme()
	{
		var theme = FuchsThemeManager.Current;
		var padding = new Thickness(Math.Max(0, theme.BodyFontSize * PaddingScale));
		if (!hasAppliedTheme || Padding.Equals(themePadding))
		{
			Padding = padding;
		}

		themePadding = padding;
		hasAppliedTheme = true;
		Background = new SolidColorBrush(CardBackgroundColor ?? theme.BackgroundColor);
		Stroke = new SolidColorBrush(CardBorderColor ?? theme.FieldBorderColor);
		StrokeThickness = theme.BorderWidth;
		StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(theme.CornerRadius) };
	}

	private void SubscribeToThemeChanges()
	{
		if (isThemeChangeSubscribed)
		{
			return;
		}

		FuchsThemeManager.ThemeChanged += OnThemeChanged;
		isThemeChangeSubscribed = true;
	}

	private void UnsubscribeFromThemeChanges()
	{
		if (!isThemeChangeSubscribed)
		{
			return;
		}

		FuchsThemeManager.ThemeChanged -= OnThemeChanged;
		isThemeChangeSubscribed = false;
	}

	private void OnThemeChanged(object? sender, EventArgs e) => ApplyTheme();
}
