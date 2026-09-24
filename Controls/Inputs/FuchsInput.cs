namespace FuchsControls.Controls;

public class FuchsInput : FuchsFieldControl
{
	protected override InputView CreateInput() => new Entry
	{
		BackgroundColor = Colors.Transparent, ClearButtonVisibility = ClearButtonVisibility.WhileEditing
	};
}