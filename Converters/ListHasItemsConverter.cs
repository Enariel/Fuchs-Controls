#region Meta

// FuchsControls
// Created: 29/01/2026
// Modified: 29/01/2026

#endregion

using System.Collections;
using System.Globalization;

namespace FuchsControls;

public class ListHasItemsConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is IEnumerable enumerable)
		{
			foreach (var _ in enumerable)
				return true;

			return false;
		}

		return false;
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotSupportedException($"{nameof(ListHasItemsConverter)} only supports one-way conversion.");
	}
}