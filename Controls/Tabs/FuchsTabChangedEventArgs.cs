#region Meta

// FuchsControls
// Created: 24/09/2026
// Modified: 24/09/2026

#endregion

namespace FuchsControls.Controls;

public sealed class FuchsTabChangedEventArgs : EventArgs
{
	public FuchsTabChangedEventArgs(int oldIndex, FuchsTab? oldTab, int newIndex, FuchsTab? newTab)
	{
		OldIndex = oldIndex;
		OldTab = oldTab;
		NewIndex = newIndex;
		NewTab = newTab;
	}

	public int OldIndex { get; }
	public FuchsTab? OldTab { get; }
	public int NewIndex { get; }
	public FuchsTab? NewTab { get; }
}