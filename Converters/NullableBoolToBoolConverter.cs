using System.Globalization;

namespace FuchsControls;

public sealed class NullableBoolToBoolConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		return value as bool? ?? false;
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		return value as bool? ?? false;
	}
}