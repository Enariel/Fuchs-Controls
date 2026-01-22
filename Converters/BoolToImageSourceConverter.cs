#region Meta
// FuchsControls
// Created: 22/01/2026
// Modified: 22/01/2026
#endregion

using System.Globalization;

namespace FuchsControls;

/// <summary>
/// Converts a boolean value to an image source.
/// </summary>
public class BoolToImageSourceConverter : IValueConverter
{
	public ImageSource TrueImage { get; set; }
	public ImageSource FalseImage { get; set; }

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is bool boolValue)
		{
			return boolValue ? TrueImage : FalseImage;
		}

		return FalseImage;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}