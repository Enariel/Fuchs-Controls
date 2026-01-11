#region Meta
// FuchsControls
// Created: 10/01/2026
// Modified: 10/01/2026
#endregion

namespace FuchsControls.Fields;

public interface INumericField<TNumber> : ITextField
{
	public TNumber NumericValue { get; set; }

	public int Step { get; set; }

	public int Minimum { get; set; }

	public int Maximum { get; set; }
}