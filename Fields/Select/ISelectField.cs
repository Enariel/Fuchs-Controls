#region Meta

// FuchsControls
// Created: 10/01/2026
// Modified: 10/01/2026

#endregion

namespace FuchsControls.Fields;

public interface ISelectField<T> : IFieldBase where T : struct, Enum
{
	public T SelectedValue { get; set; }
}