namespace FuchsControls;

public sealed partial class FuchsSpan : Span
{
	public static readonly BindableProperty TypeProperty = BindableProperty.Create(
		nameof(Type), typeof(FuchsTypoType), typeof(FuchsSpan), FuchsTypoType.Body, propertyChanged: OnTypeChanged);

	public FuchsSpan()
	{
		ApplyStyle();
	}

	public FuchsTypoType Type
	{
		get => (FuchsTypoType)GetValue(TypeProperty);
		set => SetValue(TypeProperty, value);
	}

	private static void OnTypeChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsSpan)bindable).ApplyStyle();

	private void ApplyStyle() => SetDynamicResource(StyleProperty, $"FuchsSpan{Type}Style");
}