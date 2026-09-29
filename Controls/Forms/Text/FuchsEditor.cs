namespace FuchsControls;

public sealed class FuchsEditor : FuchsTextBase
{
	public FuchsEditor()
		: base(new Editor().ApplyFuchsEditorStyle())
	{
		((Editor)Input).Completed += OnCompleted;
	}

	private void OnCompleted(object? sender, EventArgs e)
	{
		if (ReturnCommand?.CanExecute(null) == true)
			ReturnCommand.Execute(null);
	}
}