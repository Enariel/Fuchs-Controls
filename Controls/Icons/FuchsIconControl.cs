#region Meta

// FuchsControls
// Created: 24/09/2026
// Modified: 24/09/2026

#endregion

using FuchsControls.Theme;
using Microsoft.Maui.Controls.Shapes;
using MauiPath = Microsoft.Maui.Controls.Shapes.Path;
using ThemeColor = FuchsControls.Theme.FuchsColor;

namespace FuchsControls.Controls;

public class FuchsIcon : FuchsComponent
{
	private readonly MauiPath _path;

	public static readonly BindableProperty IconProperty =
		BindableProperty.Create(nameof(Icon), typeof(string), typeof(FuchsIcon), string.Empty, propertyChanged: OnVisualPropertyChanged);

	public string Icon
	{
		get => (string)GetValue(IconProperty);
		set => SetValue(IconProperty, value);
	}

	public string Data
	{
		get => Icon;
		set => Icon = value;
	}

	public FuchsIcon()
	{
		_path = new MauiPath
		{
			Aspect = Stretch.Uniform, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, InputTransparent = true
		};

		Content = _path;
		ApplyTheme();
	}

	protected override void ApplyTheme()
	{
		FuchsTheme theme = FuchsThemeProvider.Current;
		Color iconColor = Color == ThemeColor.Default ? theme.Text : theme.GetMainColor(Color);

		_path.Data = ParseIcon(Icon);
		_path.Fill = new SolidColorBrush(iconColor);
		_path.Stroke = Brush.Transparent;
		_path.StrokeThickness = 0;
		_path.WidthRequest = ResolveFontSize();
		_path.HeightRequest = ResolveFontSize();
		_path.IsVisible = _path.Data is not null;
		Opacity = IsDisabled ? 0.65 : 1;
	}

	private static Geometry? ParseIcon(string? icon)
	{
		if (string.IsNullOrWhiteSpace(icon))
			return null;

		try
		{
			return new PathGeometryConverter().ConvertFromInvariantString(icon) as Geometry;
		}
		catch (FormatException)
		{
			return null;
		}
		catch (ArgumentException)
		{
			return null;
		}
	}

	private static void OnVisualPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsIcon icon)
			icon.ApplyTheme();
	}
}