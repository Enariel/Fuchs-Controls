#region Meta

// FuchsControls
// Created: 10/01/2026
// Modified: 10/01/2026

#endregion

namespace FuchsControls.Fields;

public abstract class SelectFieldBase<TValue> : ContentView, ISelectField<TValue> where TValue : struct, Enum
{
	public static readonly BindableProperty LabelProperty =
		BindableProperty.Create(nameof(Label), typeof(string), typeof(SelectFieldBase<TValue>));

	public static readonly BindableProperty SelectedValueProperty =
		BindableProperty.Create(nameof(SelectedValue), typeof(TValue), typeof(SelectFieldBase<TValue>), default(TValue), BindingMode.TwoWay);

	public static readonly BindableProperty HelpTextProperty =
		BindableProperty.Create(nameof(HelpText), typeof(string), typeof(SelectFieldBase<TValue>), string.Empty);

	public static readonly BindableProperty PlaceholderProperty =
		BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(SelectFieldBase<TValue>), string.Empty);

	/// <inheritdoc />
	public string Label
	{
		get => (string)GetValue(LabelProperty);
		set => SetValue(LabelProperty, value);
	}

	/// <inheritdoc />
	public TValue SelectedValue
	{
		get => (TValue)GetValue(SelectedValueProperty);
		set => SetValue(SelectedValueProperty, value);
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