namespace FuchsControls.Responsive;

public sealed class FuchsBreakpoint : FuchsResponsiveBase
{
	private const double XsMaximum = 575.98;
	private const double SmMinimum = 576;
	private const double MdMinimum = 768;
	private const double LgMinimum = 992;
	private const double XlMinimum = 1200;
	private const double XxlMinimum = 1400;
	private const double MaximumOffset = 0.02;

	public static readonly BindableProperty BreakpointProperty = BindableProperty.Create(
		nameof(Breakpoint), typeof(FuchsBreakpointName), typeof(FuchsBreakpoint), FuchsBreakpointName.None,
		propertyChanged: OnBreakpointPropertyChanged);

	public static readonly BindableProperty MatchModeProperty = BindableProperty.Create(
		nameof(MatchMode), typeof(FuchsBreakpointMatchMode), typeof(FuchsBreakpoint), FuchsBreakpointMatchMode.Exact,
		propertyChanged: OnBreakpointPropertyChanged);

	public FuchsBreakpointName Breakpoint
	{
		get => (FuchsBreakpointName)GetValue(BreakpointProperty);
		set => SetValue(BreakpointProperty, value);
	}

	public FuchsBreakpointMatchMode MatchMode
	{
		get => (FuchsBreakpointMatchMode)GetValue(MatchModeProperty);
		set => SetValue(MatchModeProperty, value);
	}

	protected override bool RequiresResponsiveWidth => true;

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
		range = breakpoint switch
		{
			FuchsBreakpointName.Xs => new BreakpointRange(0, XsMaximum, XsMaximum),
			FuchsBreakpointName.Sm => new BreakpointRange(SmMinimum, MdMinimum - 1, XsMaximum),
			FuchsBreakpointName.Md => new BreakpointRange(MdMinimum, LgMinimum - 1, MdMinimum - MaximumOffset),
			FuchsBreakpointName.Lg => new BreakpointRange(LgMinimum, XlMinimum - 1, LgMinimum - MaximumOffset),
			FuchsBreakpointName.Xl => new BreakpointRange(XlMinimum, XxlMinimum - 1, XlMinimum - MaximumOffset),
			FuchsBreakpointName.Xxl => new BreakpointRange(XxlMinimum, double.PositiveInfinity, XxlMinimum - MaximumOffset),
			_ => default
		};

		return breakpoint is FuchsBreakpointName.Xs or FuchsBreakpointName.Sm or FuchsBreakpointName.Md or FuchsBreakpointName.Lg or FuchsBreakpointName.Xl
			or FuchsBreakpointName.Xxl;
	}

	private readonly record struct BreakpointRange(double Minimum, double Maximum, double MaximumMatch);
}