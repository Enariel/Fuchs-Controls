namespace FuchsControls;

public sealed class FocusHighlightBehavior : Behavior<VisualElement>
{
	private readonly FuchsFieldBase _field;

	public FocusHighlightBehavior(FuchsFieldBase field)
	{
		_field = field;
	}

	protected override void OnAttachedTo(VisualElement bindable)
	{
		base.OnAttachedTo(bindable);
		bindable.Focused += OnFocused;
		bindable.Unfocused += OnUnfocused;
	}

	protected override void OnDetachingFrom(VisualElement bindable)
	{
		bindable.Focused -= OnFocused;
		bindable.Unfocused -= OnUnfocused;
		base.OnDetachingFrom(bindable);
	}

	private void OnFocused(object? sender, FocusEventArgs e) => _field.SetInputFocus(true);

	private void OnUnfocused(object? sender, FocusEventArgs e) => _field.SetInputFocus(false);
}