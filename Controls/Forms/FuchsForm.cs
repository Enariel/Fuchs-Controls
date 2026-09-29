#region Meta

// FuchsControls
// Created: 24/09/2026
// Modified: 24/09/2026

#endregion

namespace FuchsControls;

[ContentProperty(nameof(Children))]
public sealed class FuchsForm : FuchsCard
{
	private readonly VerticalStackLayout formLayout = new VerticalStackLayout();

	public static readonly BindableProperty SpacingProperty = BindableProperty.Create(
		nameof(Spacing), typeof(double), typeof(FuchsForm), default(double), propertyChanged: OnSpacingChanged);

	public FuchsForm()
	{
		Content = formLayout;
		this.ApplyFuchsStyle("FuchsFormStyle");
	}

	public IList<IView> Children => formLayout.Children;

	public double Spacing
	{
		get => (double)GetValue(SpacingProperty);
		set => SetValue(SpacingProperty, value);
	}

	private static void OnSpacingChanged(BindableObject bindable, object oldValue, object newValue) =>
		((FuchsForm)bindable).formLayout.Spacing = (double)newValue;
}