#region Meta

// FuchsControls
// Created: 10/01/2026
// Modified: 10/01/2026

#endregion

using Microsoft.Maui.Controls.Shapes;

namespace FuchsControls.Fields;

public class FuchsEntry : TextFieldBase, ITextField
{
	public FuchsEntry()
	{
		Margin = new Thickness(0, 5);

		var entry = new Entry
		{
			BackgroundColor = Colors.Transparent,
			Margin = new Thickness(10, 0)
		};
		entry.SetBinding(Entry.KeyboardProperty, new Binding(nameof(Keyboard), source: this));
		entry.SetBinding(Entry.PlaceholderProperty, new Binding(nameof(Placeholder), source: this));
		entry.SetBinding(Entry.MaxLengthProperty, new Binding(nameof(MaxLength), source: this));
		entry.SetBinding(Entry.TextProperty, new Binding(nameof(Text), source: this, mode: BindingMode.TwoWay));
		entry.SetBinding(InputView.IsReadOnlyProperty, new Binding(nameof(IsReadOnly), source: this));

		var roundRect = new RoundRectangle { CornerRadius = 8 };
		roundRect.SetDynamicResource(RoundRectangle.CornerRadiusProperty, "FuchsCornerRadius");

		var border = new Border
		{
			Content = entry,
			Padding = new Thickness(0, 5),
			StrokeShape = roundRect,
			StrokeThickness = 1
		};
		border.SetDynamicResource(Border.StrokeProperty, "FuchsBgColor3");
		border.SetDynamicResource(Border.StrokeThicknessProperty, "FuchsBorderWidth");
		border.SetDynamicResource(VisualElement.BackgroundColorProperty, "FuchsBgColor");

		entry.Focused += (s, e) => border.SetDynamicResource(Border.StrokeProperty, "FuchsAccentColor");
		entry.Unfocused += (s, e) => border.SetDynamicResource(Border.StrokeProperty, "FuchsBgColor3");

		var labelView = CreateLabelWithHelpToggle();
		var helpView = CreateHelpText();

		var stack = new StackLayout { Spacing = 5 };
		stack.SetBinding(StackLayout.OrientationProperty, new Binding(nameof(Orientation), source: this));

		stack.Children.Add(labelView);
		stack.Children.Add(border);
		stack.Children.Add(helpView);

		ApplyToolTip(stack);
		ApplyAccessibility(entry);

		Content = stack;
	}
}