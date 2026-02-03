#region Meta
// FuchsControls
// Created: 10/01/2026
// Modified: 10/01/2026
#endregion

using Microsoft.Maui.Controls;

namespace FuchsControls.Fields;

public interface IFieldBase
{
	public string Label { get; set; }
	public string HelpText { get; set; }
	public string Placeholder { get; set; }
	public StackOrientation Orientation { get; set; }
	public bool IsHelpVisible { get; set; }
}