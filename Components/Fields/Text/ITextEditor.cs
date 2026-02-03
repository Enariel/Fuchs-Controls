#region Meta

// FuchsControls
// Created: 10/01/2026
// Modified: 10/01/2026

#endregion

namespace FuchsControls.Fields;

public interface ITextEditor
{
	public EditorAutoSizeOption AutoSize { get; set; }

	public double EditorHeight { get; set; }
}