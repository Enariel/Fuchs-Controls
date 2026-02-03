#region Meta

// FuchsControls
// Created: 02/02/2026
// Modified: 02/02/2026

#endregion

using System.Globalization;
using System.Numerics;

namespace FuchsControls.Fields;

public class FuchsStepper<TNumber> : NumericFieldBase<TNumber>
	where TNumber : struct, IParsable<TNumber>, IComparable<TNumber>, IAdditionOperators<TNumber, TNumber, TNumber>, ISubtractionOperators<TNumber, TNumber, TNumber>
{
	private readonly FuchsNumericEntry<TNumber> _numericEntry;

	public FuchsStepper()
	{
		Margin = new Thickness(2, 5, 2, 5);

		_numericEntry = new FuchsNumericEntry<TNumber>();
		_numericEntry.SetBinding(FuchsNumericEntry<TNumber>.NumericValueProperty, new Binding(nameof(NumericValue), source: this, mode: BindingMode.TwoWay));
		_numericEntry.SetBinding(FuchsNumericEntry<TNumber>.MinimumProperty, new Binding(nameof(Minimum), source: this));
		_numericEntry.SetBinding(FuchsNumericEntry<TNumber>.MaximumProperty, new Binding(nameof(Maximum), source: this));
		_numericEntry.SetBinding(FuchsNumericEntry<TNumber>.LabelProperty, new Binding(nameof(Label), source: this));
		_numericEntry.SetBinding(FuchsNumericEntry<TNumber>.HelpTextProperty, new Binding(nameof(HelpText), source: this));
		_numericEntry.SetBinding(FuchsNumericEntry<TNumber>.OrientationProperty, new Binding(nameof(Orientation), source: this));
		_numericEntry.SetBinding(FuchsNumericEntry<TNumber>.PlaceholderProperty, new Binding(nameof(Placeholder), source: this));

		var minusButton = new Button { Text = "-", WidthRequest = 40 };
		minusButton.Clicked += OnMinusClicked;

		var plusButton = new Button { Text = "+", WidthRequest = 40 };
		plusButton.Clicked += OnPlusClicked;

		var stepperLayout = new HorizontalStackLayout
		{
			Spacing = 5,
			Children = { minusButton, _numericEntry, plusButton }
		};

		Content = stepperLayout;
	}

	private void OnMinusClicked(object? sender, EventArgs e)
	{
		if (TNumber.TryParse(Step.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture, out var stepT))
		{
			var newValue = NumericValue - stepT;
			if (TNumber.TryParse(Minimum.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture, out var minT))
			{
				if (newValue.CompareTo(minT) < 0)
					newValue = minT;
			}
			NumericValue = newValue;
		}
	}

	private void OnPlusClicked(object? sender, EventArgs e)
	{
		if (TNumber.TryParse(Step.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture, out var stepT))
		{
			var newValue = NumericValue + stepT;
			if (TNumber.TryParse(Maximum.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture, out var maxT))
			{
				if (newValue.CompareTo(maxT) > 0)
					newValue = maxT;
			}
			NumericValue = newValue;
		}
	}

	protected override void OnNumberChanged(object? sender, ValueChangedEventArgs e)
	{
		// Handled by binding to _numericEntry
	}

	protected override void OnTextChanged(object sender, TextChangedEventArgs e)
	{
		// Handled by binding to _numericEntry
	}
}