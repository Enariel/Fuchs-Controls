#region Meta

// FuchsControls
// Created: 29/01/2026
// Modified: 29/01/2026

#endregion

using Microsoft.Maui.Controls.Shapes;
using Path = Microsoft.Maui.Controls.Shapes.Path;

namespace FuchsControls;

public class FuchsIcon : ContentView
{
	private readonly Path _path;

	public FuchsIcon()
	{
		_path = new Path
		{
			Stroke = StrokeColor,
			Fill = FillColor,
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.Center,
			StrokeThickness = 1
		};

		_path.SetBinding(Shape.StrokeProperty, new Binding(nameof(StrokeColor), source: this));
		_path.SetBinding(Shape.FillProperty, new Binding(nameof(FillColor), source: this));

		var container = new Grid
		{
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.Center
		};

		container.SetBinding(WidthRequestProperty, new Binding(nameof(IconSize), source: this));
		container.SetBinding(HeightRequestProperty, new Binding(nameof(IconSize), source: this));

		container.Children.Add(_path);
		Content = container;

		UpdateGeometry(PathData);
	}

	public static readonly BindableProperty PathDataProperty =
		BindableProperty.Create(
			nameof(PathData),
			typeof(string),
			typeof(FuchsIcon),
			defaultValue: string.Empty,
			propertyChanged: (bindable, _, newValue) =>
			{
				var control = (FuchsIcon)bindable;
				control.UpdateGeometry((string)newValue);
			});

	public string PathData
	{
		get => (string)GetValue(PathDataProperty);
		set => SetValue(PathDataProperty, value);
	}

	public static readonly BindableProperty StrokeColorProperty =
		BindableProperty.Create(
			nameof(StrokeColor),
			typeof(Color),
			typeof(FuchsIcon),
			defaultValue: Colors.Transparent);

	public Color StrokeColor
	{
		get => (Color)GetValue(StrokeColorProperty);
		set => SetValue(StrokeColorProperty, value);
	}

	public static readonly BindableProperty FillColorProperty =
		BindableProperty.Create(
			nameof(FillColor),
			typeof(Color),
			typeof(FuchsIcon),
			defaultValue: Colors.Gray);

	public Color FillColor
	{
		get => (Color)GetValue(FillColorProperty);
		set => SetValue(FillColorProperty, value);
	}

	public static readonly BindableProperty IconSizeProperty =
		BindableProperty.Create(
			nameof(IconSize),
			typeof(double),
			typeof(FuchsIcon),
			defaultValue: 48d);

	public double IconSize
	{
		get => (double)GetValue(IconSizeProperty);
		set => SetValue(IconSizeProperty, value);
	}

	private void UpdateGeometry(string? data)
	{
		if (string.IsNullOrWhiteSpace(data))
		{
			_path.Data = null;
			return;
		}

		var converter = new PathGeometryConverter();
		_path.Data = (Geometry)converter.ConvertFromInvariantString(data);
	}
}