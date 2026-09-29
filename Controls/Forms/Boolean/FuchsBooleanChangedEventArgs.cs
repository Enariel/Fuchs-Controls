namespace FuchsControls;

public sealed class FuchsBooleanChangedEventArgs(bool value) : EventArgs
{
	public bool Value { get; } = value;
}