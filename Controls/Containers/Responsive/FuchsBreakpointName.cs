namespace FuchsControls.Responsive;

/// <summary>
/// Defines standard responsive breakpoint identifiers inspired by FlatifyCSS.
/// </summary>
public enum FuchsBreakpointName
{
	/// <summary>
	/// No breakpoint specified; content is always rendered.
	/// </summary>
	None,

	/// <summary>
	/// Extra small breakpoint for screen or container widths less than 576px (0 to 575.98px).
	/// </summary>
	Xs,

	/// <summary>
	/// Small breakpoint for screen or container widths between 576px and 767px.
	/// </summary>
	Sm,

	/// <summary>
	/// Medium breakpoint for screen or container widths between 768px and 991px.
	/// </summary>
	Md,

	/// <summary>
	/// Large breakpoint for screen or container widths between 992px and 1199px.
	/// </summary>
	Lg,

	/// <summary>
	/// Extra large breakpoint for screen or container widths between 1200px and 1399px.
	/// </summary>
	Xl,

	/// <summary>
	/// Extra extra large breakpoint for screen or container widths of 1400px and above.
	/// </summary>
	Xxl
}