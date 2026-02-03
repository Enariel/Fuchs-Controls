#region Meta

// FuchsControls
// Created: 03/02/2026
// Modified: 03/02/2026

#endregion

using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics.Converters;

namespace FuchsControls;

public class FuchsIcon : GraphicsView, IDrawable
{
	public static readonly BindableProperty FillColorProperty = BindableProperty.Create(
		nameof(FillColor),
		typeof(Color),
		typeof(FuchsIcon),
		null,
		propertyChanged: (bindable, _, _) => ((FuchsIcon)bindable).Invalidate());

	public static readonly BindableProperty StrokeColorProperty = BindableProperty.Create(
		nameof(StrokeColor),
		typeof(Color),
		typeof(FuchsIcon),
		Colors.Transparent,
		propertyChanged: (bindable, _, _) => ((FuchsIcon)bindable).Invalidate());

	public static readonly BindableProperty PathDataProperty = BindableProperty.Create(
		nameof(PathData),
		typeof(string),
		typeof(FuchsIcon),
		string.Empty,
		propertyChanged: (bindable, _, _) => ((FuchsIcon)bindable).Invalidate());

	public static readonly BindableProperty IconSizeProperty = BindableProperty.Create(
		nameof(IconSize),
		typeof(double),
		typeof(FuchsIcon),
		24.0,
		propertyChanged: (bindable, _, _) =>
		{
			var icon = (FuchsIcon)bindable;
			icon.WidthRequest = icon.IconSize;
			icon.HeightRequest = icon.IconSize;
			icon.Invalidate();
		});

	public FuchsIcon()
	{
		Drawable = this;
		WidthRequest = IconSize;
		HeightRequest = IconSize;

		this.SetDynamicResource(FillColorProperty, "FuchsTextColor");
	}

	public Color? FillColor
	{
		get => (Color?)GetValue(FillColorProperty);
		set => SetValue(FillColorProperty, value);
	}

	public Color StrokeColor
	{
		get => (Color)GetValue(StrokeColorProperty);
		set => SetValue(StrokeColorProperty, value);
	}

	public string PathData
	{
		get => (string)GetValue(PathDataProperty);
		set => SetValue(PathDataProperty, value);
	}

	public double IconSize
	{
		get => (double)GetValue(IconSizeProperty);
		set => SetValue(IconSizeProperty, value);
	}

	public void Draw(ICanvas canvas, RectF dirtyRect)
	{
		if (string.IsNullOrEmpty(PathData))
			return;

		try
		{
			var path = PathBuilder.Build(PathData);

			// Scale the path to fit the IconSize
			// Most Material Icons (like in FuchsIcons.cs) are 960x960 or 24x24 or 48x48.
			// We should probably normalize it.

			var bounds = path.Bounds;
			float scale = (float)IconSize / Math.Max(bounds.Width, bounds.Height);

			canvas.SaveState();
			canvas.Scale(scale, scale);
			// Translate to center if needed, but usually paths start at 0,0 or have their own offset.
			// For Material Icons, they often have large coordinates.
			// Let's translate so the top-left of the bounds is at 0,0
			canvas.Translate(-bounds.Left, -bounds.Top);

			canvas.FillColor = FillColor ?? Colors.Grey;
			canvas.StrokeColor = StrokeColor;

			canvas.FillPath(path);
			if (StrokeColor != Colors.Transparent)
			{
				canvas.DrawPath(path);
			}

			canvas.RestoreState();
		}
		catch
		{
			// Handle invalid path data gracefully
		}
	}
}