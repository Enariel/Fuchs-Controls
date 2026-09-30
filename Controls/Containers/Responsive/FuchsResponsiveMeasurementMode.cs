namespace FuchsControls.Responsive;

/// <summary>
/// Specifies the measurement source used to evaluate responsive breakpoint conditions.
/// </summary>
public enum FuchsResponsiveMeasurementMode
{
	/// <summary>
	/// Measures the available width within the parent container or visual layout bounds.
	/// </summary>
	AvailableBounds,

	/// <summary>
	/// Measures the physical display screen width in device-independent units via device display metrics.
	/// </summary>
	DisplayMetrics
}