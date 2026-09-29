namespace FuchsControls;

[ContentProperty(nameof(Content))]
public sealed partial class FuchsContainer : ContentView
{
	private const double HorizontalPaddingScale = 0.64;
	private bool isThemeChangeSubscribed;
	private bool hasAppliedTheme;
	private Thickness themePadding;

	public static readonly BindableProperty ContainerSizeProperty = BindableProperty.Create(
		nameof(ContainerSize), typeof(FuchsContainerSize), typeof(FuchsContainer), FuchsContainerSize.None,
		propertyChanged: OnContainerSizeChanged);

	public FuchsContainer()
	{
		this.ApplyFuchsStyle("FuchsContainerStyle");
		ApplyContainerSize();
		ApplyTheme();
	}

	/// <summary>
	/// Gets or sets the Flatify-inspired maximum width applied to this container.
	/// </summary>
	public FuchsContainerSize ContainerSize
	{
		get => (FuchsContainerSize)GetValue(ContainerSizeProperty);
		set => SetValue(ContainerSizeProperty, value);
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

	private static void OnContainerSizeChanged(BindableObject bindable, object oldValue, object newValue) =>
		((FuchsContainer)bindable).ApplyContainerSize();

	private void ApplyContainerSize()
	{
		MaximumWidthRequest = GetMaximumWidth(ContainerSize);
	}

	internal static double GetMaximumWidth(FuchsContainerSize containerSize) => containerSize switch
																				{
																					FuchsContainerSize.Sm => 576, FuchsContainerSize.Md => 768
																					, FuchsContainerSize.Lg => 992, FuchsContainerSize.Xl => 1200
																					, FuchsContainerSize.Xxl => 1400, _ => double.PositiveInfinity,
																				};

	private void ApplyTheme()
	{
		var horizontalPadding = Math.Max(0, FuchsThemeManager.Current.BodyFontSize * HorizontalPaddingScale);
		var padding = new Thickness(horizontalPadding, 0);
		if (!hasAppliedTheme || Padding.Equals(themePadding))
		{
			Padding = padding;
		}

		themePadding = padding;
		hasAppliedTheme = true;
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