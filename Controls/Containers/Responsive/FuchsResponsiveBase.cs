namespace FuchsControls.Responsive;

/// <summary>
/// Provides an abstract base class for controls that dynamically adjust their visibility
/// according to responsive breakpoints or device environment conditions.
/// </summary>
[ContentProperty(nameof(Content))]
public abstract class FuchsResponsiveBase : ContentView
{
	/// <summary>
	/// Identifies the <see cref="MeasurementMode"/> bindable property.
	/// </summary>
	public static readonly BindableProperty MeasurementModeProperty = BindableProperty.Create(
		nameof(MeasurementMode), typeof(FuchsResponsiveMeasurementMode), typeof(FuchsResponsiveBase), FuchsResponsiveMeasurementMode.AvailableBounds,
		propertyChanged: OnMeasurementModeChanged);

	private bool isDisplayMetricsSubscribed;
	private bool isRefreshing;
	private bool refreshPending;
	private VisualElement? responsiveParent;

	/// <summary>
	/// Initializes a new instance of the <see cref="FuchsResponsiveBase"/> class.
	/// </summary>
	protected FuchsResponsiveBase()
	{
		SizeChanged += OnSizeChanged;
		ParentChanged += OnParentChanged;
	}

	/// <summary>
	/// Gets or sets the measurement mode used to evaluate responsive conditions.
	/// </summary>
	public FuchsResponsiveMeasurementMode MeasurementMode
	{
		get => (FuchsResponsiveMeasurementMode)GetValue(MeasurementModeProperty);
		set => SetValue(MeasurementModeProperty, value);
	}

	/// <summary>
	/// Gets a value indicating whether the condition requires a valid responsive width before it can be evaluated.
	/// </summary>
	protected virtual bool RequiresResponsiveWidth => false;

	/// <summary>
	/// Evaluates the condition that controls this responsive host's visibility.
	/// </summary>
	/// <returns><c>true</c> if the condition is met and the control should be visible; otherwise, <c>false</c>.</returns>
	protected abstract bool IsConditionMet();

	/// <summary>
	/// Attempts to retrieve the current responsive width in device-independent units.
	/// </summary>
	/// <param name="width">When this method returns, contains the current responsive width if available; otherwise, zero.</param>
	/// <returns><c>true</c> if a valid responsive width was successfully obtained; otherwise, <c>false</c>.</returns>
	protected bool TryGetResponsiveWidth(out double width)
	{
		if (MeasurementMode == FuchsResponsiveMeasurementMode.AvailableBounds)
		{
			// Prioritize container / parent width so hidden or collapsed children re-evaluate correctly when resized.
			if (responsiveParent is not null && IsValidMeasurement(responsiveParent.Width))
			{
				width = responsiveParent.Width;
				return true;
			}

			// If parent width is not available yet, fall back to the control's own width when visible.
			if (IsVisible && IsValidMeasurement(Width))
			{
				width = Width;
				return true;
			}

			// Walk up the visual tree to find the nearest ancestor with a valid measured width.
			var ancestor = (responsiveParent?.Parent ?? Parent) as VisualElement;
			while (ancestor is not null)
			{
				if (IsValidMeasurement(ancestor.Width))
				{
					width = ancestor.Width;
					return true;
				}

				ancestor = ancestor.Parent as VisualElement;
			}

			width = 0;
			return false;
		}

		var displayInfo = DeviceDisplay.Current.MainDisplayInfo;
		var density = displayInfo.Density;
		if (density <= 0)
		{
			width = 0;
			return false;
		}

		width = displayInfo.Width / density;
		return IsValidMeasurement(width);
	}

	/// <summary>
	/// Re-evaluates the responsive condition and applies the resulting visibility state.
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

				if (IsVisible != isConditionMet)
				{
					IsVisible = isConditionMet;
				}
			} while (refreshPending);
		}
		finally
		{
			isRefreshing = false;
		}
	}

	/// <inheritdoc />
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
		if (RequiresResponsiveWidth && MeasurementMode == FuchsResponsiveMeasurementMode.AvailableBounds)
		{
			RefreshVisibility();
		}
	}

	private void OnSizeChanged(object? sender, EventArgs e)
	{
		if (RequiresResponsiveWidth && MeasurementMode == FuchsResponsiveMeasurementMode.AvailableBounds)
		{
			RefreshVisibility();
		}
	}

	private void OnResponsiveParentSizeChanged(object? sender, EventArgs e)
	{
		if (RequiresResponsiveWidth && MeasurementMode == FuchsResponsiveMeasurementMode.AvailableBounds)
		{
			RefreshVisibility();
		}
	}

	private void UpdateResponsiveParentSubscription()
	{
		var shouldSubscribe = RequiresResponsiveWidth && Handler is not null && MeasurementMode == FuchsResponsiveMeasurementMode.AvailableBounds;
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
		if (RequiresResponsiveWidth && MeasurementMode == FuchsResponsiveMeasurementMode.DisplayMetrics)
		{
			RefreshVisibility();
		}
	}

	private void UpdateDisplayMetricsSubscription()
	{
		var shouldSubscribe = RequiresResponsiveWidth && Handler is not null && MeasurementMode == FuchsResponsiveMeasurementMode.DisplayMetrics;
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