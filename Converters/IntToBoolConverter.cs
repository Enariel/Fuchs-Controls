#region Meta

// FuchsControls
// Created: 26/01/2026
// Modified: 26/01/2026

#endregion

using System.Globalization;

namespace FuchsControls;

public class IntToBoolConverter : IValueConverter
{
	/// <inheritdoc />
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is int intValue)
		{
			return intValue != 0;
		}

		return false;
	}

	/// <inheritdoc />
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is bool boolValue)
		{
			return boolValue ? 1 : 0;
		}

		return 0;
	}
}