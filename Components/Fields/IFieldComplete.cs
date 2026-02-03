#region Meta

// FuchsControls
// Created: 10/01/2026
// Modified: 10/01/2026

#endregion

namespace FuchsControls.Fields;

public interface IFieldComplete
{
	public Action Completed { get; set; }

	public void SendCompleted()
	{
		Completed?.Invoke();
	}
}