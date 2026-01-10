#region Meta
// FuchsControls
// Created: 10/01/2026
// Modified: 10/01/2026
#endregion

namespace FuchsControls.Fields;

public interface IFieldBase
{
	public string Label { get; set; }
	public string HelpText { get; set; }
	public string Placeholder { get; set; }
}