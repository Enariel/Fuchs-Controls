#region Meta
// FuchsControls
// Created: 22/01/2026
// Modified: 22/01/2026
#endregion

using System.Globalization;

namespace FuchsControls;

public class BoolToStringConverter : IValueConverter
{
	public string TrueText { get; set; } = "True";
	public string FalseText { get; set; } = "False";

	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is bool boolValue)
		{
			return boolValue ? TrueText : FalseText;
		}

		return FalseText;
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		return value?.ToString() == TrueText;
	}
}