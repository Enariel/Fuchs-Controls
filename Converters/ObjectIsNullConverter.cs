#region Meta

// FuchsControls
// Created: 29/01/2026
// Modified: 29/01/2026

#endregion

using System.Globalization;

namespace FuchsControls;

public class ObjectIsNullConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		return value is null;
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotSupportedException($"{nameof(ObjectIsNullConverter)} only supports one-way conversion.");
	}
}