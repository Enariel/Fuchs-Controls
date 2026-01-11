#region Meta
// FuchsControls
// Created: 11/01/2026
// Modified: 11/01/2026
#endregion

namespace FuchsControls.Fields;

public abstract class BooleanFieldBase : ContentView, IFieldBase
{
	public static readonly BindableProperty LabelProperty = BindableProperty.Create(
		nameof(Label),
		typeof(string),
		typeof(BooleanFieldBase),
		default(string));

	public static readonly BindableProperty HelpTextProperty = BindableProperty.Create(
		nameof(HelpText),
		typeof(string),
		typeof(BooleanFieldBase),
		default(string));

	public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
		nameof(Placeholder),
		typeof(string),
		typeof(BooleanFieldBase),
		default(string));



	/// <inheritdoc />
	public string Label
	{
		get => (string)GetValue(LabelProperty);
		set => SetValue(LabelProperty, value);
	}

	/// <inheritdoc />
	public string HelpText
	{
		get => (string)GetValue(HelpTextProperty);
		set => SetValue(HelpTextProperty, value);
	}

	/// <inheritdoc />
	public string Placeholder
	{
		get => (string)GetValue(PlaceholderProperty);
		set => SetValue(PlaceholderProperty, value);
	}

	
}