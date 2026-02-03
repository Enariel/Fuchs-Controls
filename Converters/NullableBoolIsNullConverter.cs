using System.Globalization;

namespace FuchsControls;

public sealed class NullableBoolIsNullConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> value is null;

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> null;
}