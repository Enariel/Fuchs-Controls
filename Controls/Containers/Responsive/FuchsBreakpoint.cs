namespace FuchsControls.Responsive;

/// <summary>
/// A responsive container that conditionally displays or hides its content
/// based on screen or container width breakpoints.
/// </summary>
public sealed class FuchsBreakpoint : FuchsResponsiveBase
{
	private const double XsMaximum = 575.98;
	private const double SmMinimum = 576;
	private const double MdMinimum = 768;
	private const double LgMinimum = 992;
	private const double XlMinimum = 1200;
	private const double XxlMinimum = 1400;
	private const double MaximumOffset = 0.02;

	private static readonly BreakpointRange XsRange = new(0, XsMaximum, XsMaximum);
	private static readonly BreakpointRange SmRange = new(SmMinimum, MdMinimum - 1, XsMaximum);
	private static readonly BreakpointRange MdRange = new(MdMinimum, LgMinimum - 1, MdMinimum - MaximumOffset);
	private static readonly BreakpointRange LgRange = new(LgMinimum, XlMinimum - 1, LgMinimum - MaximumOffset);
	private static readonly BreakpointRange XlRange = new(XlMinimum, XxlMinimum - 1, XlMinimum - MaximumOffset);
	private static readonly BreakpointRange XxlRange = new(XxlMinimum, double.PositiveInfinity, XxlMinimum - MaximumOffset);

	/// <summary>
	/// Identifies the <see cref="Breakpoint"/> bindable property.
	/// </summary>
	public static readonly BindableProperty BreakpointProperty = BindableProperty.Create(
		nameof(Breakpoint), typeof(FuchsBreakpointName), typeof(FuchsBreakpoint), FuchsBreakpointName.None,
		propertyChanged: OnBreakpointPropertyChanged);

	/// <summary>
	/// Identifies the <see cref="MatchMode"/> bindable property.
	/// </summary>
	public static readonly BindableProperty MatchModeProperty = BindableProperty.Create(
		nameof(MatchMode), typeof(FuchsBreakpointMatchMode), typeof(FuchsBreakpoint), FuchsBreakpointMatchMode.Exact,
		propertyChanged: OnBreakpointPropertyChanged);

	/// <summary>
	/// Gets or sets the target breakpoint name to evaluate against.
	/// </summary>
	public FuchsBreakpointName Breakpoint
	{
		get => (FuchsBreakpointName)GetValue(BreakpointProperty);
		set => SetValue(BreakpointProperty, value);
	}

	/// <summary>
	/// Gets or sets the breakpoint matching strategy.
	/// </summary>
	public FuchsBreakpointMatchMode MatchMode
	{
		get => (FuchsBreakpointMatchMode)GetValue(MatchModeProperty);
		set => SetValue(MatchModeProperty, value);
	}

	/// <inheritdoc />
	protected override bool RequiresResponsiveWidth => Breakpoint != FuchsBreakpointName.None;

	/// <inheritdoc />
	protected override bool IsConditionMet()
	{
		if (Breakpoint == FuchsBreakpointName.None || !TryGetBreakpointRange(Breakpoint, out var range))
		{
			return true;
		}

		if (!TryGetResponsiveWidth(out var width))
		{
			return true;
		}

		return MatchMode switch
		{
			FuchsBreakpointMatchMode.Minimum => width >= range.Minimum,
			FuchsBreakpointMatchMode.Maximum => width <= range.MaximumMatch,
			_ => width >= range.Minimum && width <= range.Maximum
		};
	}

	private static void OnBreakpointPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		((FuchsBreakpoint)bindable).RefreshVisibility();
	}

	private static bool TryGetBreakpointRange(FuchsBreakpointName breakpoint, out BreakpointRange range)
	{
		switch (breakpoint)
		{
			case FuchsBreakpointName.Xs:
				range = XsRange;
				return true;
			case FuchsBreakpointName.Sm:
				range = SmRange;
				return true;
			case FuchsBreakpointName.Md:
				range = MdRange;
				return true;
			case FuchsBreakpointName.Lg:
				range = LgRange;
				return true;
			case FuchsBreakpointName.Xl:
				range = XlRange;
				return true;
			case FuchsBreakpointName.Xxl:
				range = XxlRange;
				return true;
			default:
				range = default;
				return false;
		}
	}

	private readonly record struct BreakpointRange(double Minimum, double Maximum, double MaximumMatch);
}