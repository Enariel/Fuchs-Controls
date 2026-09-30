namespace FuchsControls.Responsive;

/// <summary>
/// Specifies how a breakpoint condition is matched against the current responsive width.
/// </summary>
public enum FuchsBreakpointMatchMode
{
	/// <summary>
	/// Matches only when the width falls strictly within the specified breakpoint range.
	/// </summary>
	Exact,

	/// <summary>
	/// Matches when the width is greater than or equal to the minimum bound of the specified breakpoint.
	/// </summary>
	Minimum,

	/// <summary>
	/// Matches when the width is less than or equal to the maximum matching bound of the specified breakpoint.
	/// </summary>
	Maximum
}