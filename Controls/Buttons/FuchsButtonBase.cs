namespace FuchsControls;

public abstract class FuchsButtonBase : Button
{
	public static readonly BindableProperty ColorProperty = BindableProperty.Create(
		nameof(Color), typeof(FuchsThemeColor), typeof(FuchsButtonBase), FuchsThemeColor.Default, BindingMode.TwoWay, propertyChanged: OnThemePropertyChanged);

	private bool isThemeChangeSubscribed;

	protected FuchsButtonBase()
	{
		Padding = new Thickness(14, 12, 14, 8);
		Margin = new Thickness(5, 5, 5, 8);
		FontAttributes = FontAttributes.Bold;
		ApplyTheme();
		ApplyVisualStates();
	}

	public FuchsThemeColor Color
	{
		get => (FuchsThemeColor)GetValue(ColorProperty);
		set => SetValue(ColorProperty, value);
	}

	protected Color ThemeColor => FuchsThemeResourceLookup.GetColor(Color);

	protected Color ForegroundColor => Color is FuchsThemeColor.Light
		? FuchsThemeManager.Current.TextColor
		: FuchsThemeManager.Current.LightColor;

	protected abstract void ApplyThemedStyle();

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

	private static void OnThemePropertyChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsButtonBase)bindable).ApplyTheme();

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

	protected void ApplyTheme()
	{
		CornerRadius = (int)Math.Round(FuchsThemeManager.Current.CornerRadius);
		ApplyThemedStyle();
		ApplyVisualStates();
	}

	protected virtual void ApplyVisualStates()
	{
		var group = new VisualStateGroup { Name = "CommonStates" };
		group.States.Add(CreateVisualState("Normal", 1, 0));
		group.States.Add(CreateVisualState("PointerOver", 0.9, 0));
		group.States.Add(CreateVisualState("Pressed", 0.9, 1.38));
		group.States.Add(CreateVisualState("Disabled", 0.7, 1.38));
		VisualStateManager.SetVisualStateGroups(this, new VisualStateGroupList { group });
	}

	protected static VisualState CreateVisualState(string name, double opacity, double translationY)
	{
		var state = new VisualState { Name = name };
		state.Setters.Add(new Setter { Property = OpacityProperty, Value = opacity });
		state.Setters.Add(new Setter { Property = TranslationYProperty, Value = translationY });
		return state;
	}
}