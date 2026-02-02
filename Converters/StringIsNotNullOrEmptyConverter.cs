#region Meta

// FuchsControls
// Created: 23/01/2026
// Modified: 23/01/2026

#endregion

using System.Globalization;

namespace FuchsControls;

/// <summary>
/// Converts a string value to a boolean indicating if it is not null or empty.
/// </summary>
/// <remarks>
/// Returns true if the value is not null or empty, false otherwise.
/// </remarks>
public class StringIsNotNullOrEmptyConverter : IValueConverter
{
	/// <inheritdoc />
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return value is string stringValue && !string.IsNullOrEmpty(stringValue);
	}

	/// <inheritdoc />
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return null;
	}
}