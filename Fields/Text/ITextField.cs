#region Meta
// FuchsControls
// Created: 10/01/2026
// Modified: 10/01/2026
#endregion

namespace FuchsControls.Fields;

public interface ITextField : IFieldBase
{
	public string Text { get; set; }
	public Keyboard Keyboard { get; set; }
	public int MaxLength { get; set; }
	public bool IsReadOnly { get; set; }
}