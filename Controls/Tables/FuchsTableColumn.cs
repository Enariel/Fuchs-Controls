#region Meta

// FuchsControls
// Created: 24/09/2026
// Modified: 24/09/2026

#endregion

namespace FuchsControls.Controls;

public sealed class FuchsTableColumn : BindableObject
{
	public static readonly BindableProperty HeaderProperty =
		BindableProperty.Create(nameof(Header), typeof(string), typeof(FuchsTableColumn), string.Empty);

	public static readonly BindableProperty BindingPathProperty =
		BindableProperty.Create(nameof(BindingPath), typeof(string), typeof(FuchsTableColumn), ".");

	public static readonly BindableProperty StringFormatProperty =
		BindableProperty.Create(nameof(StringFormat), typeof(string), typeof(FuchsTableColumn), null);

	public static readonly BindableProperty WidthProperty =
		BindableProperty.Create(nameof(Width), typeof(GridLength), typeof(FuchsTableColumn), new GridLength(1, GridUnitType.Star));

	public static readonly BindableProperty HorizontalTextAlignmentProperty =
		BindableProperty.Create(nameof(HorizontalTextAlignment), typeof(TextAlignment), typeof(FuchsTableColumn), TextAlignment.Start);

	public static readonly BindableProperty VerticalTextAlignmentProperty =
		BindableProperty.Create(nameof(VerticalTextAlignment), typeof(TextAlignment), typeof(FuchsTableColumn), TextAlignment.Center);

	public static readonly BindableProperty CellTemplateProperty =
		BindableProperty.Create(nameof(CellTemplate), typeof(DataTemplate), typeof(FuchsTableColumn));

	public static readonly BindableProperty HeaderTemplateProperty =
		BindableProperty.Create(nameof(HeaderTemplate), typeof(DataTemplate), typeof(FuchsTableColumn));

	public string Header
	{
		get => (string)GetValue(HeaderProperty);
		set => SetValue(HeaderProperty, value);
	}

	public string BindingPath
	{
		get => (string)GetValue(BindingPathProperty);
		set => SetValue(BindingPathProperty, value);
	}

	public string? StringFormat
	{
		get => (string?)GetValue(StringFormatProperty);
		set => SetValue(StringFormatProperty, value);
	}

	public GridLength Width
	{
		get => (GridLength)GetValue(WidthProperty);
		set => SetValue(WidthProperty, value);
	}

	public TextAlignment HorizontalTextAlignment
	{
		get => (TextAlignment)GetValue(HorizontalTextAlignmentProperty);
		set => SetValue(HorizontalTextAlignmentProperty, value);
	}

	public TextAlignment VerticalTextAlignment
	{
		get => (TextAlignment)GetValue(VerticalTextAlignmentProperty);
		set => SetValue(VerticalTextAlignmentProperty, value);
	}

	public DataTemplate? CellTemplate
	{
		get => (DataTemplate?)GetValue(CellTemplateProperty);
		set => SetValue(CellTemplateProperty, value);
	}

	public DataTemplate? HeaderTemplate
	{
		get => (DataTemplate?)GetValue(HeaderTemplateProperty);
		set => SetValue(HeaderTemplateProperty, value);
	}
}