#region Meta
// FuchsControls
// Created: 11/01/2026
// Modified: 11/01/2026
#endregion

using System.Windows.Input;

namespace FuchsControls.Fields;

public interface IBooleanField : IFieldBase
{
	public bool? IsChecked { get; set; }
	public ICommand? Command { get; set; }
	public object? CommandParameter { get; set; }
}