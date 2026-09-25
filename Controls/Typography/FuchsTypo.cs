namespace FuchsControls;

public sealed partial class FuchsTypo : Label
{
	public static readonly BindableProperty TypeProperty = BindableProperty.Create(
		nameof(Type), typeof(FuchsTypoType), typeof(FuchsTypo), FuchsTypoType.Body, propertyChanged: OnTypeChanged);

	public FuchsTypo()
	{
		ApplyStyle();
	}

	public FuchsTypoType Type
	{
		get => (FuchsTypoType)GetValue(TypeProperty);
		set => SetValue(TypeProperty, value);
	}

	private static void OnTypeChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsTypo)bindable).ApplyStyle();

	private void ApplyStyle() => SetDynamicResource(StyleProperty, $"FuchsTypo{Type}Style");
}