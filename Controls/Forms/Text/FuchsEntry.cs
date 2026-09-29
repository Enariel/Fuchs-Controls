using System.Windows.Input;

namespace FuchsControls;

public sealed class FuchsEntry : FuchsTextBase
{
	public FuchsEntry()
		: base(new Entry().ApplyFuchsEntryStyle()) { }

	protected override void UpdateReturnCommand(ICommand? command) => ((Entry)Input).ReturnCommand = command;
}