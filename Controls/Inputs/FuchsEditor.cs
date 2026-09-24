namespace FuchsControls.Controls;

public class FuchsEditor : FuchsFieldControl
{
	protected override InputView CreateInput() => new Editor
	{
		BackgroundColor = Colors.Transparent, AutoSize = EditorAutoSizeOption.TextChanges
	};
}