namespace FuchsControls.Controls;

public class FuchsEntry : FuchsFieldControl
{
	protected override InputView CreateInput() => new Entry
	{
		BackgroundColor = Colors.Transparent, ClearButtonVisibility = ClearButtonVisibility.WhileEditing
	};
}

public class FuchsInput : FuchsEntry { }