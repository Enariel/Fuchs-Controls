namespace FuchsControls.Behaviours;

public sealed class PressedScaleBehavior : Behavior<View>
{
	public static readonly BindableProperty PressedScaleProperty =
		BindableProperty.Create(nameof(PressedScale), typeof(double), typeof(PressedScaleBehavior), 0.96d);

	public static readonly BindableProperty PressedTranslationYProperty =
		BindableProperty.Create(nameof(PressedTranslationY), typeof(double), typeof(PressedScaleBehavior), 3d);

	public double PressedScale
	{
		get => (double)GetValue(PressedScaleProperty);
		set => SetValue(PressedScaleProperty, value);
	}

	public double PressedTranslationY
	{
		get => (double)GetValue(PressedTranslationYProperty);
		set => SetValue(PressedTranslationYProperty, value);
	}

	protected override void OnAttachedTo(View bindable)
	{
		base.OnAttachedTo(bindable);

		PointerGestureRecognizer pointer = new();
		pointer.PointerPressed += OnPointerPressed;
		pointer.PointerReleased += OnPointerReleased;
		pointer.PointerExited += OnPointerReleased;

		bindable.GestureRecognizers.Add(pointer);
	}

	private async void OnPointerPressed(object? sender, PointerEventArgs e)
	{
		if (sender is not VisualElement view || !view.IsEnabled)
			return;

		await Task.WhenAll(
			view.ScaleToAsync(PressedScale, 80, Easing.CubicOut),
			view.TranslateToAsync(0, PressedTranslationY, 80, Easing.CubicOut));
	}

	private async void OnPointerReleased(object? sender, PointerEventArgs e)
	{
		if (sender is not VisualElement view)
			return;

		await Task.WhenAll(
			view.ScaleToAsync(1, 100, Easing.CubicOut),
			view.TranslateToAsync(0, 0, 100, Easing.CubicOut));
	}
}