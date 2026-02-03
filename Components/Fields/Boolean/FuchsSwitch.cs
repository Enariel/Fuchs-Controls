#region Meta

// FuchsControls
// Created: 11/01/2026
// Modified: 11/01/2026

#endregion

namespace FuchsControls.Fields;

public class FuchsSwitch : BooleanFieldBase
{
	public FuchsSwitch()
	{
		Margin = new Thickness(2, 5, 2, 5);

		var toggle = new Microsoft.Maui.Controls.Switch();
		toggle.SetBinding(Microsoft.Maui.Controls.Switch.IsToggledProperty, new Binding(nameof(IsChecked), source: this, mode: BindingMode.TwoWay, converter: new FuchsControls.NullableBoolToBoolConverter()));
		toggle.Toggled += (s, e) => ExecuteCommand();

		var labelView = CreateLabelWithHelpToggle();
		var helpView = CreateHelpText();

		var mainLayout = new StackLayout { Spacing = 5 };
		mainLayout.SetBinding(StackLayout.OrientationProperty, new Binding(nameof(Orientation), source: this));

		mainLayout.Children.Add(labelView);
		mainLayout.Children.Add(toggle);
		mainLayout.Children.Add(helpView);

		ApplyToolTip(mainLayout);
		ApplyAccessibility(toggle);

		Content = mainLayout;
	}
}