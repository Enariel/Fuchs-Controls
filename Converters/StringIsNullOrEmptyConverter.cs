#region Meta

// FuchsControls
// Created: 02/02/2026
// Modified: 02/02/2026

#endregion

using System.Globalization;

namespace FuchsControls;

public class StringIsNullOrEmptyConverter : IValueConverter
{
	/// <inheritdoc />
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return value == null || (value is string stringValue && string.IsNullOrEmpty(stringValue));
	}

	/// <inheritdoc />
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return null;
	}
}