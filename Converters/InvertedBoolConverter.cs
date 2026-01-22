#region Meta
// FuchsControls
// Created: 22/01/2026
// Modified: 22/01/2026
#endregion

using System.Globalization;

namespace FuchsControls;

/// <summary>
/// Inverts a boolean value.
/// </summary>
public class InvertedBoolConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is bool boolValue)
		{
			return !boolValue;
		}

		return false;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is bool boolValue)
		{
			return !boolValue;
		}

		return false;
	}
}