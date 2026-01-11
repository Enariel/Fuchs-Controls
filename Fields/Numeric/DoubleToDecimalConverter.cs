#region Meta

// FuchsControls
// Created: 11/01/2026
// Modified: 11/01/2026

#endregion

using System.Globalization;

namespace FuchsControls.Fields;

public class DoubleToDecimalConverter : IValueConverter
{
	/// <inheritdoc />
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is decimal decimalValue)
		{
			return (double)decimalValue;
		}
		return 0.0;
	}

	/// <inheritdoc />
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is double doubleValue)
		{
			return (decimal)doubleValue;
		}
		return 0m;
	}
}