namespace FuchsControls.Behaviours;

public sealed partial class FocusHighlightBehavior : Behavior<VisualElement>
{
	public static readonly BindableProperty TargetProperty =
		BindableProperty.Create(nameof(Target), typeof(Border), typeof(FocusHighlightBehavior));

	public static readonly BindableProperty FocusColorProperty =
		BindableProperty.Create(nameof(FocusColor), typeof(Color), typeof(FocusHighlightBehavior), Colors.Transparent);

	public static readonly BindableProperty FocusStrokeThicknessProperty =
		BindableProperty.Create(nameof(FocusStrokeThickness), typeof(double), typeof(FocusHighlightBehavior), 2d);

	public static readonly BindableProperty NormalStrokeProperty =
		BindableProperty.Create(nameof(NormalStroke), typeof(Brush), typeof(FocusHighlightBehavior), Brush.Transparent);

	public static readonly BindableProperty NormalStrokeThicknessProperty =
		BindableProperty.Create(nameof(NormalStrokeThickness), typeof(double), typeof(FocusHighlightBehavior), 2d);

	private VisualElement? _view;

	public Border? Target
	{
		get => (Border?)GetValue(TargetProperty);
		set => SetValue(TargetProperty, value);
	}

	public Color FocusColor
	{
		get => (Color)GetValue(FocusColorProperty);
		set => SetValue(FocusColorProperty, value);
	}

	public double FocusStrokeThickness
	{
		get => (double)GetValue(FocusStrokeThicknessProperty);
		set => SetValue(FocusStrokeThicknessProperty, value);
	}

	public Brush NormalStroke
	{
		get => (Brush)GetValue(NormalStrokeProperty);
		set => SetValue(NormalStrokeProperty, value);
	}

	public double NormalStrokeThickness
	{
		get => (double)GetValue(NormalStrokeThicknessProperty);
		set => SetValue(NormalStrokeThicknessProperty, value);
	}

	protected override void OnAttachedTo(VisualElement bindable)
	{
		base.OnAttachedTo(bindable);
		_view = bindable;
		bindable.Focused += OnFocused;
		bindable.Unfocused += OnUnfocused;
		Refresh();
	}

	protected override void OnDetachingFrom(VisualElement bindable)
	{
		bindable.Focused -= OnFocused;
		bindable.Unfocused -= OnUnfocused;
		_view = null;
		base.OnDetachingFrom(bindable);
	}

	public void Refresh() => Apply(_view?.IsFocused == true);

	private void OnFocused(object? sender, FocusEventArgs e) => Apply(true);

	private void OnUnfocused(object? sender, FocusEventArgs e) => Apply(false);

	private void Apply(bool focused)
	{
		if (Target is null)
			return;

		Target.Stroke = focused ? new SolidColorBrush(FocusColor) : NormalStroke;
		Target.StrokeThickness = focused ? FocusStrokeThickness : NormalStrokeThickness;
	}
}