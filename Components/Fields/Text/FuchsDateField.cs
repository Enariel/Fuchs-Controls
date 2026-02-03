#region Meta

// FuchsControls
// Created: 02/02/2026
// Modified: 02/02/2026

#endregion

using System.Globalization;
using Microsoft.Maui.Controls.Shapes;

namespace FuchsControls.Fields;

public class FuchsDateField : FieldBase
{
	public static readonly BindableProperty DateProperty = BindableProperty.Create(
		nameof(Date),
		typeof(DateTime?),
		typeof(FuchsDateField),
		defaultValue: null,
		defaultBindingMode: BindingMode.TwoWay);

	public DateTime? Date
	{
		get => (DateTime?)GetValue(DateProperty);
		set => SetValue(DateProperty, value);
	}

	public static readonly BindableProperty FormatProperty = BindableProperty.Create(
		nameof(Format),
		typeof(string),
		typeof(FuchsDateField),
		defaultValue: "d");

	public string Format
	{
		get => (string)GetValue(FormatProperty);
		set => SetValue(FormatProperty, value);
	}

	public FuchsDateField()
	{
		Margin = new Thickness(2, 5, 2, 5);

		var datePicker = new DatePicker
		{
			BackgroundColor = Colors.Transparent
		};
		datePicker.SetBinding(DatePicker.FormatProperty, new Binding(nameof(Format), source: this));

		// Nullable handling: DatePicker doesn't support null. 
		// We'll use a Label/Button overlay or just a clear button.
		var clearButton = new Button { Text = "X", WidthRequest = 40, VerticalOptions = LayoutOptions.Center };
		clearButton.Clicked += (s, e) => Date = null;
		clearButton.SetBinding(VisualElement.IsVisibleProperty, new Binding(nameof(Date), source: this, converter: new NullToBoolConverter(invert: true)));

		datePicker.DateSelected += (s, e) => Date = e.NewDate;

		// Sync DatePicker with our Date property
		this.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(Date))
			{
				if (Date.HasValue)
				{
					datePicker.Date = Date.Value;
					datePicker.Opacity = 1;
				}
				else
				{
					datePicker.Opacity = 0.5; // Visual cue for null
				}
			}
		};

		var pickerLayout = new HorizontalStackLayout
		{
			Spacing = 5,
			Children = { datePicker, clearButton }
		};

		var roundRect = new RoundRectangle { CornerRadius = 8 };
		roundRect.SetDynamicResource(RoundRectangle.CornerRadiusProperty, "FuchsCornerRadius");

		var border = new Border
		{
			Content = pickerLayout,
			Padding = new Thickness(10, 5),
			StrokeShape = roundRect,
			StrokeThickness = 1
		};
		border.SetDynamicResource(Border.StrokeProperty, "FuchsBgColor3");
		border.SetDynamicResource(Border.StrokeThicknessProperty, "FuchsBorderWidth");
		border.SetDynamicResource(VisualElement.BackgroundColorProperty, "FuchsBgColor");

		datePicker.Focused += (s, e) => border.SetDynamicResource(Border.StrokeProperty, "FuchsAccentColor");
		datePicker.Unfocused += (s, e) => border.SetDynamicResource(Border.StrokeProperty, "FuchsBgColor3");

		var labelView = CreateLabelWithHelpToggle();
		var helpView = CreateHelpText();

		var stack = new StackLayout { Spacing = 5 };
		stack.SetBinding(StackLayout.OrientationProperty, new Binding(nameof(Orientation), source: this));

		stack.Children.Add(labelView);
		stack.Children.Add(border);
		stack.Children.Add(helpView);

		ApplyToolTip(stack);
		ApplyAccessibility(datePicker);

		Content = stack;
	}
}

internal class NullToBoolConverter(bool invert = false) : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		bool isNull = value == null;
		return invert ? !isNull : isNull;
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}