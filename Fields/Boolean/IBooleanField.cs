#region Meta
// FuchsControls
// Created: 11/01/2026
// Modified: 11/01/2026
#endregion

namespace FuchsControls.Fields;

public interface IBooleanField : IFieldBase
{
	public bool? IsChecked { get; set; }
}