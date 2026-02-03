using System.Globalization;

namespace FuchsControls;

public sealed class HelpTextVisibilityConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
	{
		if (values.Length < 2)
			return false;

		var isHelpVisible = values[0] as bool? ?? false;
		var helpText = values[1] as string;
		return isHelpVisible && !string.IsNullOrWhiteSpace(helpText);
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object? parameter, CultureInfo culture)
		=> Array.Empty<object>();
}