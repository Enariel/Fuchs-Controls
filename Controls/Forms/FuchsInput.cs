namespace FuchsControls.Controls;

public class FuchsEntry : FormField
{
	public FuchsEntry()
	{
		InitializeField(new Entry
		{
			BackgroundColor = Colors.Transparent, ClearButtonVisibility = ClearButtonVisibility.WhileEditing
		});
	}
}

public class FuchsInput : FuchsEntry { }