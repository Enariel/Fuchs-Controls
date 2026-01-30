#region Meta

// FuchsControls
// Created: 22/01/2026
// Modified: 29/01/2026

#endregion

using Microsoft.Maui.Controls.Shapes;
using FuchsControls.Utility;

namespace FuchsControls;

public class FuchsDivider : Border
{
	public static readonly BindableProperty ThicknessProperty =
		BindableProperty.Create(
			nameof(Thickness),
			typeof(double),
			typeof(FuchsDivider),
			1d,
			propertyChanged: (b, _, __) => ((FuchsDivider)b).Update());

	public static readonly BindableProperty LineColorProperty =
		BindableProperty.Create(
			nameof(LineColor),
			typeof(FuchsColor),
			typeof(FuchsDivider),
			FuchsColor.TextColor,
			propertyChanged: (b, _, __) => ((FuchsDivider)b).Update());

	public FuchsDivider()
	{
		StrokeShape = new Rectangle();
		HorizontalOptions = LayoutOptions.Fill;
		VerticalOptions = LayoutOptions.Center;
		BackgroundColor = Colors.Transparent;
		Update();
	}

	/// <summary>Line thickness in device-independent units.</summary>
	public double Thickness
	{
		get => (double)GetValue(ThicknessProperty);
		set => SetValue(ThicknessProperty, value);
	}

	/// <summary>Line color selection from the FuchsColor enum.</summary>
	public FuchsColor LineColor
	{
		get => (FuchsColor)GetValue(LineColorProperty);
		set => SetValue(LineColorProperty, value);
	}

	private void Update()
	{
		Stroke = FuchsColorHelper.GetColor(LineColor);
		StrokeThickness = Thickness;
		HeightRequest = Thickness;
	}
}