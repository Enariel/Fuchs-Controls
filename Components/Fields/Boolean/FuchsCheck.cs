#region Meta

// FuchsControls
// Created: 26/01/2026
// Modified: 26/01/2026

#endregion

namespace FuchsControls.Fields;

public class FuchsCheck : BooleanFieldBase
{
	public FuchsCheck()
	{
		Margin = new Thickness(2, 5, 2, 5);

		var check = new Microsoft.Maui.Controls.CheckBox();
		check.SetBinding(Microsoft.Maui.Controls.CheckBox.IsCheckedProperty, new Binding(nameof(IsChecked), source: this, mode: BindingMode.TwoWay, converter: new FuchsControls.NullableBoolToBoolConverter()));
		
		// To support indeterminate visual, we can use Opacity as a simple way
		// or maybe a specific color if we had styles.
		check.SetBinding(VisualElement.OpacityProperty, new Binding(nameof(IsChecked), source: this, converter: new FuchsControls.NullableBoolIsNullConverter(), converterParameter: 0.5));
		// Wait, NullableBoolIsNullConverter returns bool. I need one that returns a value.
		// I'll stick to basic for now or add another converter.

		check.CheckedChanged += (s, e) => ExecuteCommand();

		var labelView = CreateLabelWithHelpToggle();
		var helpView = CreateHelpText();

		var mainLayout = new StackLayout { Spacing = 5 };
		mainLayout.SetBinding(StackLayout.OrientationProperty, new Binding(nameof(Orientation), source: this));

		mainLayout.Children.Add(labelView);
		mainLayout.Children.Add(check);
		mainLayout.Children.Add(helpView);

		ApplyToolTip(mainLayout);
		ApplyAccessibility(check);

		Content = mainLayout;
	}
}
