#region Meta

// FuchsControls
// Created: 29/01/2026
// Modified: 29/01/2026

#endregion

using System.Globalization;

namespace FuchsControls.Fields;

public class FuchsNumericEntry<TNumber> : NumericFieldBase<TNumber>
	where TNumber : struct, IParsable<TNumber>, IComparable<TNumber>
{
	private bool _suppressTextChange;

	public FuchsNumericEntry()
	{
		Margin = new Thickness(2, 5, 2, 5);

		// Create the layout
		var entry = new Entry() { Keyboard = Keyboard.Numeric };
		entry.SetBinding(Entry.TextProperty, new Binding(nameof(NumericValue), source: this, mode: BindingMode.TwoWay));
		entry.TextChanged += OnTextChanged;

		var label = new Label { };
		label.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, new Binding(nameof(Label), source: this, mode: BindingMode.OneWay));

		var helpLabel = new Label
		{
			FontSize = 12,
			IsVisible = DeviceInfo.Current.Platform != DevicePlatform.WinUI
		};
		helpLabel.SetBinding(TextProperty, new Binding(nameof(HelpText), source: this));

		var layout = new StackLayout
		{
			Orientation = StackOrientation.Vertical,
			Spacing = 5,
			Children = { label, entry, helpLabel }
		};

#if WINDOWS
		if (!string.IsNullOrEmpty(HelpText))
			ToolTipProperties.SetText(layout, new Binding(nameof(HelpText), source: this));
#endif

		Content = layout;
	}

	protected override void OnTextChanged(object sender, TextChangedEventArgs e)
	{
		if (_suppressTextChange)
			return;

		var input = sender as InputView;
		var newText = e.NewTextValue ?? string.Empty;

		// Allow empty input to represent default value
		if (string.IsNullOrWhiteSpace(newText))
		{
			NumericValue = default;
			return;
		}

		if (!TNumber.TryParse(newText, CultureInfo.CurrentCulture, out var parsed))
		{
			// Reject non-numeric characters by reverting to previous text
			if (input is not null)
			{
				_suppressTextChange = true;
				input.Text = e.OldTextValue;
				_suppressTextChange = false;
			}

			return;
		}

		// Clamp to bounds when possible
		var coerced = CoerceToBounds(parsed, Minimum, Maximum);
		NumericValue = coerced;

		// Normalize textual representation if needed
		if (input is not null)
		{
			var normalized = ToStringInvariant(coerced, CultureInfo.CurrentCulture);
			if (!string.Equals(input.Text, normalized, StringComparison.Ordinal))
			{
				_suppressTextChange = true;
				input.Text = normalized;
				_suppressTextChange = false;
			}
		}
	}

	protected override void OnNumberChanged(object? sender, ValueChangedEventArgs e)
	{
		// Convert the provided numeric (double) to TNumber in a culture-safe way
		var candidateText = e.NewValue.ToString(CultureInfo.CurrentCulture);
		if (TNumber.TryParse(candidateText, CultureInfo.CurrentCulture, out var parsed))
		{
			NumericValue = CoerceToBounds(parsed, Minimum, Maximum);
		}
	}

	private static TNumber CoerceToBounds(TNumber value, int min, int max)
	{
		var result = value;
		if (TNumber.TryParse(min.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture, out var minT))
		{
			if (result.CompareTo(minT) < 0)
				result = minT;
		}

		if (TNumber.TryParse(max.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture, out var maxT))
		{
			if (result.CompareTo(maxT) > 0)
				result = maxT;
		}

		return result;
	}

	private static string ToStringInvariant(TNumber value, IFormatProvider provider)
	{
		// Prefer IFormattable when available for culture-aware formatting
		if (value is IFormattable formattable)
			return formattable.ToString(null, provider);
		return value.ToString() ?? string.Empty;
	}
}