namespace FuchsControls;

public sealed class FuchsImage : Image
{
	public static readonly BindableProperty AlternativeTextProperty = BindableProperty.Create(
		nameof(AlternativeText), typeof(string), typeof(FuchsImage), string.Empty, propertyChanged: OnAlternativeTextChanged);

	public FuchsImage()
	{
		this.ApplyFuchsStyle("FuchsImageStyle");
		UpdateAccessibility();
	}

	public string AlternativeText
	{
		get => (string)GetValue(AlternativeTextProperty);
		set => SetValue(AlternativeTextProperty, value);
	}

	private static void OnAlternativeTextChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsImage)bindable).UpdateAccessibility();

	private void UpdateAccessibility()
	{
		SemanticProperties.SetDescription(this, AlternativeText);
	}
}