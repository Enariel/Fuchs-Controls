#region Meta

// FuchsControls
// Created: 10/01/2026
// Modified: 10/01/2026

#endregion

namespace FuchsControls.Fields;

public abstract class SelectFieldBase<TValue> : FieldBase, ISelectField<TValue> where TValue : struct, Enum
{
	public static readonly BindableProperty SelectedValueProperty =
		BindableProperty.Create(nameof(SelectedValue), typeof(TValue), typeof(SelectFieldBase<TValue>), default(TValue), BindingMode.TwoWay);

	/// <inheritdoc />
	public TValue SelectedValue
	{
		get => (TValue)GetValue(SelectedValueProperty);
		set => SetValue(SelectedValueProperty, value);
	}
}
