using System.ComponentModel;

namespace FuchsControls;

public sealed class FuchsButton : FuchsButtonBase
{
	private bool isPointerOver;
	private bool isPressed;

	public static readonly BindableProperty VariantProperty = BindableProperty.Create(
		nameof(Variant), typeof(FuchsVariant), typeof(FuchsButton), FuchsVariant.Filled, BindingMode.TwoWay, propertyChanged: OnVariantChanged);

	public FuchsButton()
	{
		Pressed += OnPressed;
		Released += OnReleased;
		PropertyChanged += OnButtonPropertyChanged;

		var pointerGesture = new PointerGestureRecognizer();
		pointerGesture.PointerEntered += OnPointerEntered;
		pointerGesture.PointerExited += OnPointerExited;
		GestureRecognizers.Add(pointerGesture);
		VisualStateManager.GoToState(this, "FilledNormal");
	}

	public FuchsVariant Variant
	{
		get => (FuchsVariant)GetValue(VariantProperty);
		set => SetValue(VariantProperty, value);
	}

	protected override void ApplyThemedStyle()
	{
		var color = ThemeColor;
		if (!IsEnabled)
		{
			BackgroundColor = color;
			TextColor = ForegroundColor;
			BorderColor = color;
			BorderWidth = 0;
			return;
		}

		BackgroundColor = Variant is FuchsVariant.Filled ? color : Colors.Transparent;
		TextColor = Variant is FuchsVariant.Filled ? ForegroundColor : color;
		BorderColor = color;
		BorderWidth = Variant is FuchsVariant.Outlined ? FuchsThemeManager.Current.BorderWidth : 0;
		if (Variant is FuchsVariant.Filled)
		{
			if (Shadow is { } shadow)
			{
				Shadow = new Shadow
						 {
							 Brush = new SolidColorBrush(color)
							 , Offset = new Point(0
								 , FuchsThemeManager.Current.BorderWidth * FuchsControlExtensions.GetFuchsDoubleResource("FuchsButtonShadowOffsetFactor"))
							 , Radius = shadow.Radius, Opacity = shadow.Opacity
						 };
			}
		}
	}

	private void OnPointerEntered(object? sender, PointerEventArgs e)
	{
		isPointerOver = true;
		if (IsEnabled)
		{
			AnimateToState(GetCurrentState());
		}
	}

	private void OnPointerExited(object? sender, PointerEventArgs e)
	{
		isPointerOver = false;
		if (IsEnabled)
		{
			AnimateToState(GetCurrentState());
		}
	}

	private void OnPressed(object? sender, EventArgs e)
	{
		isPressed = true;
		AnimateToState(GetCurrentState());
	}

	private void OnReleased(object? sender, EventArgs e)
	{
		isPressed = false;
		AnimateToState(GetCurrentState());
	}

	private void OnButtonPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName != nameof(IsEnabled))
		{
			return;
		}

		ApplyTheme();
		AnimateToState(GetCurrentState());
	}

	private string GetCurrentState() => !IsEnabled
		? "FuchsDisabled"
		: isPressed
			? Variant is FuchsVariant.Outlined ? "OutlinedPressed" : "FilledPressed"
			: isPointerOver
				? Variant is FuchsVariant.Outlined ? "OutlinedPointerOver" : "FilledPointerOver"
				: Variant is FuchsVariant.Outlined
					? "OutlinedNormal"
					: "FilledNormal";

	private void AnimateToState(string stateName)
	{
		var state = VisualStateManager.GetVisualStateGroups(this)
									  .FirstOrDefault(group => group.Name == "FuchsButtonStates")?
									  .States.FirstOrDefault(candidate => candidate.Name == stateName);
		if (state is null)
		{
			return;
		}

		var opacity = GetStateValue(state, OpacityProperty);
		var translationFactor = GetStateValue(state, TranslationYProperty);
		if (opacity is not double targetOpacity || translationFactor is not double factor)
		{
			return;
		}

		var targetTranslation = factor * FuchsThemeManager.Current.BorderWidth;
		var duration = Application.Current?.Resources.TryGetValue("FuchsButtonAnimationDuration", out var durationValue) == true
					   && durationValue is double durationMilliseconds
			? (uint)durationMilliseconds
			: 0;
		if (duration == 0)
		{
			VisualStateManager.GoToState(this, stateName);
			if (stateName is "FilledNormal" or "FilledPointerOver")
			{
				ApplyThemedStyle();
			}

			return;
		}

		var initialOpacity = Opacity;
		var initialTranslation = TranslationY;
		this.Animate(
			"ButtonVisualState",
			progress =>
			{
				Opacity = initialOpacity + ((targetOpacity - initialOpacity) * progress);
				TranslationY = initialTranslation + ((targetTranslation - initialTranslation) * progress);
			},
			0,
			1,
			length: duration,
			easing: Easing.CubicInOut,
			finished: (_, finished) =>
			{
				if (finished)
				{
					VisualStateManager.GoToState(this, stateName);
					if (stateName is "FilledNormal" or "FilledPointerOver")
					{
						ApplyThemedStyle();
					}

					Opacity = targetOpacity;
					TranslationY = targetTranslation;
				}
			});
	}

	private static object? GetStateValue(VisualState state, BindableProperty property) =>
		state.Setters.FirstOrDefault(setter => setter.Property == property)?.Value;

	private static void OnVariantChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var button = (FuchsButton)bindable;
		button.ApplyTheme();
		button.AnimateToState(button.GetCurrentState());
	}
}