using System.Diagnostics;
using System.Reflection.Emit;
using CommunityToolkit.Maui.Markup;

namespace FuchsControls;

public static class FuchsControlExtensions
{
	public static FuchsTypo ApplyFuchsFieldLabelStyle(this FuchsTypo label)
	{
		label.Typo = FuchsTypoType.Caption;
		return label.ApplyFuchsStyle("FuchsFieldLabelStyle");
	}

	public static FuchsTypo ApplyFuchsFieldHelpTextStyle(this FuchsTypo label)
	{
		label.Typo = FuchsTypoType.Caption;
		return label.ApplyFuchsStyle("FuchsFieldHelpTextStyle");
	}

	public static T ApplyFuchsInputStyle<T>(this T input) where T : InputView
	{
		return input.ApplyFuchsStyle("FuchsInputStyle");
	}

	public static Entry ApplyFuchsEntryStyle(this Entry entry)
	{
		return entry.ApplyFuchsStyle("FuchsEntryStyle");
	}

	public static Editor ApplyFuchsEditorStyle(this Editor editor)
	{
		return editor.ApplyFuchsStyle("FuchsEditorStyle");
	}

	public static Picker ApplyFuchsPickerStyle(this Picker picker)
	{
		return picker.ApplyFuchsStyle("FuchsPickerStyle");
	}

	public static DatePicker ApplyFuchsDatePickerStyle(this DatePicker picker)
	{
		return picker.ApplyFuchsStyle("FuchsDatePickerStyle");
	}

	public static TimePicker ApplyFuchsTimePickerStyle(this TimePicker picker)
	{
		return picker.ApplyFuchsStyle("FuchsTimePickerStyle");
	}

	public static Switch ApplyFuchsSwitchStyle(this Switch control)
	{
		return control.ApplyFuchsStyle("FuchsSwitchStyle");
	}

	public static CheckBox ApplyFuchsCheckboxStyle(this CheckBox checkBox)
	{
		return checkBox.ApplyFuchsStyle("FuchsCheckboxStyle");
	}

	internal static FuchsButton ApplyFuchsButtonStyle(this FuchsButton button, FuchsVariant variant)
	{
		var styleKey = FuchsThemeResourceLookup.GetButtonStyleKey(variant);
		if (Application.Current?.Resources.TryGetValue(styleKey, out var resource) == true && resource is Style style)
		{
			button.Style = style;
		}

		return button;
	}

	public static RadioButton ApplyFuchsRadioButtonStyle(this RadioButton button)
	{
		return button.ApplyFuchsStyle("FuchsRadioButtonStyle");
	}

	public static Slider ApplyFuchsRangeStyle(this Slider slider)
	{
		return slider.ApplyFuchsStyle("FuchsRangeStyle");
	}

	public static FuchsTypo ApplyFuchsTabTextStyle(this FuchsTypo text, bool active)
	{
		text.Typo = FuchsTypoType.Body;
		return text.ApplyFuchsStyle(active ? "FuchsTabActiveTextStyle" : "FuchsTabTextStyle");
	}

	internal static T ApplyFuchsStyle<T>(this T control, string styleKey) where T : VisualElement
	{
		control.SetDynamicResource(VisualElement.StyleProperty, styleKey);
		return control;
	}

	internal static uint GetFuchsAnimationDuration(string resourceKey) =>
		(uint)GetFuchsDoubleResource(resourceKey);

	internal static double GetFuchsDoubleResource(string resourceKey) =>
		Application.Current?.Resources.TryGetValue(resourceKey, out var value) == true && value is double number ? number : 0;
}