namespace FuchsControls.Responsive;

[ContentProperty(nameof(Content))]
public abstract class FuchsResponsiveBase : ContentView
{
	public static readonly BindableProperty MeasurementModeProperty = BindableProperty.Create(
		nameof(MeasurementMode), typeof(FuchsResponsiveMeasurementMode), typeof(FuchsResponsiveBase), FuchsResponsiveMeasurementMode.AvailableBounds
		, propertyChanged: OnMeasurementModeChanged);

	private bool isDisplayMetricsSubscribed;
	private bool isRefreshing;
	private bool refreshPending;
	private VisualElement? responsiveParent;

	protected FuchsResponsiveBase()
	{
		SizeChanged += OnSizeChanged;
		ParentChanged += OnParentChanged;
	}

	public FuchsResponsiveMeasurementMode MeasurementMode
	{
		get => (FuchsResponsiveMeasurementMode)GetValue(MeasurementModeProperty);
		set => SetValue(MeasurementModeProperty, value);
	}

	/// <summary>
	/// Gets a value indicating whether the condition needs a responsive width before it can be evaluated.
	/// </summary>
	protected virtual bool RequiresResponsiveWidth => false;

	/// <summary>
	/// Evaluates the condition that controls this responsive host's visibility.
	/// </summary>
	protected abstract bool IsConditionMet();

	/// <summary>
	/// Gets the current responsive width in device-independent units.
	/// </summary>
	protected bool TryGetResponsiveWidth(out double width)
	{
		if (MeasurementMode == FuchsResponsiveMeasurementMode.AvailableBounds)
		{
			width = Width;
			if (IsValidMeasurement(width))
			{
				return true;
			}

			width = responsiveParent?.Width ?? 0;
			return IsValidMeasurement(width);
		}

		var displayInfo = DeviceDisplay.Current.MainDisplayInfo;
		width = displayInfo.Width / displayInfo.Density;
		return IsValidMeasurement(width);
	}

	/// <summary>
	/// Re-evaluates the condition and applies its result immediately to this control.
	/// </summary>
	protected void RefreshVisibility()
	{
		if (isRefreshing)
		{
			refreshPending = true;
			return;
		}

		isRefreshing = true;
		try
		{
			do
			{
				refreshPending = false;
				if (RequiresResponsiveWidth && !TryGetResponsiveWidth(out _))
				{
					return;
				}

				var isConditionMet = IsConditionMet();
				if (refreshPending)
				{
					continue;
				}

				IsVisible = isConditionMet;
			} while (refreshPending);
		}
		finally
		{
			isRefreshing = false;
		}
	}

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
		UpdateResponsiveParentSubscription();
		UpdateDisplayMetricsSubscription();
		RefreshVisibility();
	}

	private static void OnMeasurementModeChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var responsiveControl = (FuchsResponsiveBase)bindable;
		responsiveControl.UpdateResponsiveParentSubscription();
		responsiveControl.UpdateDisplayMetricsSubscription();
		responsiveControl.RefreshVisibility();
	}

	private void OnParentChanged(object? sender, EventArgs e)
	{
		UpdateResponsiveParentSubscription();
		if (MeasurementMode == FuchsResponsiveMeasurementMode.AvailableBounds)
		{
			RefreshVisibility();
		}
	}

	private void OnSizeChanged(object? sender, EventArgs e)
	{
		if (MeasurementMode == FuchsResponsiveMeasurementMode.AvailableBounds)
		{
			RefreshVisibility();
		}
	}

	private void OnResponsiveParentSizeChanged(object? sender, EventArgs e)
	{
		if (MeasurementMode == FuchsResponsiveMeasurementMode.AvailableBounds)
		{
			RefreshVisibility();
		}
	}

	private void UpdateResponsiveParentSubscription()
	{
		var shouldSubscribe = Handler is not null && MeasurementMode == FuchsResponsiveMeasurementMode.AvailableBounds;
		var parent = shouldSubscribe ? Parent as VisualElement : null;
		if (ReferenceEquals(parent, responsiveParent))
		{
			return;
		}

		if (responsiveParent is not null)
		{
			responsiveParent.SizeChanged -= OnResponsiveParentSizeChanged;
		}

		responsiveParent = parent;
		if (responsiveParent is not null)
		{
			responsiveParent.SizeChanged += OnResponsiveParentSizeChanged;
		}
	}

	private void OnMainDisplayInfoChanged(object? sender, DisplayInfoChangedEventArgs e)
	{
		if (MeasurementMode == FuchsResponsiveMeasurementMode.DisplayMetrics)
		{
			RefreshVisibility();
		}
	}

	private void UpdateDisplayMetricsSubscription()
	{
		var shouldSubscribe = Handler is not null && MeasurementMode == FuchsResponsiveMeasurementMode.DisplayMetrics;
		if (shouldSubscribe == isDisplayMetricsSubscribed)
		{
			return;
		}

		if (shouldSubscribe)
		{
			DeviceDisplay.Current.MainDisplayInfoChanged += OnMainDisplayInfoChanged;
		}
		else
		{
			DeviceDisplay.Current.MainDisplayInfoChanged -= OnMainDisplayInfoChanged;
		}

		isDisplayMetricsSubscribed = shouldSubscribe;
	}

	private static bool IsValidMeasurement(double width) => double.IsFinite(width) && width > 0;
}