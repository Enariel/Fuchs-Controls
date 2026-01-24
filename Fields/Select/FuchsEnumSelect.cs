#region Meta

// FuchsControls
// Created: 10/01/2026
// Modified: 10/01/2026

#endregion

namespace FuchsControls.Fields;

public class FuchsEnumSelect<TValue> : FuchsSelect<TValue> where TValue : struct, Enum
{
	public FuchsEnumSelect()
	{
		ItemsSource = Enum.GetValues<TValue>();
	}
}
