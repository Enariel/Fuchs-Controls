using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Input;
using FuchsControls.Responsive;
using Microsoft.Maui.Devices;

namespace FuchsControls;

public static class FuchsControlExtensions
{
	public static FuchsTypo Text(this FuchsTypo control, string text)
	{
		control.Text = text;
		return control;
	}

	public static FuchsTypo Typo(this FuchsTypo control, FuchsTypoType typo)
	{
		control.Typo = typo;
		return control;
	}

	public static FuchsTypo AccessibilityLabel(this FuchsTypo control, string label)
	{
		control.AccessibilityLabel = label;
		return control;
	}

	public static FuchsTypo AccessibilityHint(this FuchsTypo control, string hint)
	{
		control.AccessibilityHint = hint;
		return control;
	}

	public static FuchsSpan Text(this FuchsSpan control, string text)
	{
		control.Text = text;
		return control;
	}

	public static FuchsSpan ThemeColor(this FuchsSpan control, FuchsThemeColor color)
	{
		control.ThemeColor = color;
		return control;
	}

	public static FuchsSpan Type(this FuchsSpan control, FuchsTypoType type)
	{
		control.Type = type;
		return control;
	}

	public static FuchsSpan AccessibilityLabel(this FuchsSpan control, string label)
	{
		control.AccessibilityLabel = label;
		return control;
	}

	public static FuchsSpan AccessibilityHint(this FuchsSpan control, string hint)
	{
		control.AccessibilityHint = hint;
		return control;
	}

	public static FuchsIcon PathData(this FuchsIcon control, string pathData)
	{
		control.PathData = pathData;
		return control;
	}

	public static FuchsIcon Icon(this FuchsIcon control, string icon)
	{
		control.Icon = icon;
		return control;
	}

	public static T Color<T>(this T control, FuchsThemeColor color) where T : BindableObject
	{
		switch (control)
		{
			case FuchsTypo typo:
				typo.Color = color;
				break;
			case FuchsIcon icon:
				icon.Color = color;
				break;
			case FuchsButtonBase button:
				button.Color = color;
				break;
			case FuchsBooleanBase boolean:
				boolean.Color = color;
				break;
			default:
				throw new ArgumentException($"{typeof(T).Name} does not expose a semantic theme color.", nameof(control));
		}

		return control;
	}

	public static FuchsIcon Size(this FuchsIcon control, double size)
	{
		control.Size = size;
		return control;
	}

	public static FuchsButton Variant(this FuchsButton control, FuchsVariant variant)
	{
		control.Variant = variant;
		return control;
	}

	public static T Label<T>(this T control, string label) where T : FuchsFieldBase
	{
		control.Label = label;
		return control;
	}

	public static T HelpText<T>(this T control, string helpText) where T : FuchsFieldBase
	{
		control.HelpText = helpText;
		return control;
	}

	public static T InputState<T>(this T control, FuchsInputState inputState) where T : FuchsFieldBase
	{
		control.InputState = inputState;
		return control;
	}

	public static T IsRequired<T>(this T control, bool isRequired = true) where T : FuchsFieldBase
	{
		control.IsRequired = isRequired;
		return control;
	}

	public static T AccessibilityLabel<T>(this T control, string label) where T : FuchsFieldBase
	{
		control.AccessibilityLabel = label;
		return control;
	}

	public static T AccessibilityHint<T>(this T control, string hint) where T : FuchsFieldBase
	{
		control.AccessibilityHint = hint;
		return control;
	}

	public static T Value<T>(this T control, string value) where T : FuchsTextBase
	{
		control.Value = value;
		return control;
	}

	public static T Placeholder<T>(this T control, string placeholder) where T : FuchsTextBase
	{
		control.Placeholder = placeholder;
		return control;
	}

	public static T Keyboard<T>(this T control, Keyboard keyboard) where T : FuchsTextBase
	{
		control.Keyboard = keyboard;
		return control;
	}

	public static T MaxLength<T>(this T control, int maxLength) where T : FuchsTextBase
	{
		control.MaxLength = maxLength;
		return control;
	}

	public static T IsReadOnly<T>(this T control, bool isReadOnly = true) where T : FuchsTextBase
	{
		control.IsReadOnly = isReadOnly;
		return control;
	}

	public static T ReturnCommand<T>(this T control, ICommand? command) where T : FuchsTextBase
	{
		control.ReturnCommand = command;
		return control;
	}

	public static T Value<T>(this T control, object? value) where T : FuchsNumericFieldBase
	{
		control.Value = value;
		return control;
	}

	public static T Minimum<T>(this T control, double minimum) where T : FuchsNumericFieldBase
	{
		control.Minimum = minimum;
		return control;
	}

	public static T Maximum<T>(this T control, double maximum) where T : FuchsNumericFieldBase
	{
		control.Maximum = maximum;
		return control;
	}

	public static T Step<T>(this T control, double step) where T : FuchsNumericFieldBase
	{
		control.Step = step;
		return control;
	}

	public static T NumberType<T>(this T control, FuchsNumberType numberType) where T : FuchsNumericFieldBase
	{
		control.NumberType = numberType;
		return control;
	}

	public static FuchsNumericField Placeholder(this FuchsNumericField control, string placeholder)
	{
		control.Placeholder = placeholder;
		return control;
	}

	public static FuchsDateField Value(this FuchsDateField control, DateTime value)
	{
		control.Value = value;
		return control;
	}

	public static FuchsDateField MinimumDate(this FuchsDateField control, DateTime minimumDate)
	{
		control.MinimumDate = minimumDate;
		return control;
	}

	public static FuchsDateField MaximumDate(this FuchsDateField control, DateTime maximumDate)
	{
		control.MaximumDate = maximumDate;
		return control;
	}

	public static FuchsTimeField Value(this FuchsTimeField control, TimeSpan value)
	{
		control.Value = value;
		return control;
	}

	public static FuchsColorPicker Value(this FuchsColorPicker control, Color value)
	{
		control.Value = value;
		return control;
	}

	public static T Text<T>(this T control, string text) where T : FuchsBooleanBase
	{
		control.Text = text;
		return control;
	}

	public static FuchsCheckbox IsChecked(this FuchsCheckbox control, bool isChecked = true)
	{
		control.IsChecked = isChecked;
		return control;
	}

	[Obsolete("Use IsChecked instead. MAUI CheckBox does not support an indeterminate state.")]
	public static FuchsCheckbox Value(this FuchsCheckbox control, bool? value)
	{
		control.Value = value;
		return control;
	}

	[Obsolete("MAUI CheckBox does not support an indeterminate state.")]
	public static FuchsCheckbox IsThreeState(this FuchsCheckbox control, bool isThreeState = true)
	{
		control.IsThreeState = isThreeState;
		return control;
	}

	public static FuchsSwitch IsToggled(this FuchsSwitch control, bool isToggled = true)
	{
		control.IsToggled = isToggled;
		return control;
	}

	[Obsolete("Use IsToggled instead.")]
	public static FuchsSwitch Value(this FuchsSwitch control, bool value)
	{
		control.Value = value;
		return control;
	}

	public static FuchsRadioButton Value(this FuchsRadioButton control, object? value)
	{
		control.Value = value;
		return control;
	}

	public static FuchsRadioButton Text(this FuchsRadioButton control, string text)
	{
		control.Text = text;
		return control;
	}

	public static FuchsRadioButton GroupName(this FuchsRadioButton control, string groupName)
	{
		control.GroupName = groupName;
		return control;
	}

	public static FuchsRadioButton IsSelected(this FuchsRadioButton control, bool isSelected = true)
	{
		control.IsSelected = isSelected;
		return control;
	}

	public static FuchsRadioButtonGroup SelectedValue(this FuchsRadioButtonGroup control, object? selectedValue)
	{
		control.SelectedValue = selectedValue;
		return control;
	}

	public static T ItemsSource<T>(this T control, IEnumerable? itemsSource) where T : FuchsSelectBase
	{
		control.ItemsSource = itemsSource;
		return control;
	}

	public static T SelectedItem<T>(this T control, object? selectedItem) where T : FuchsSelectBase
	{
		control.SelectedItem = selectedItem;
		return control;
	}

	public static T Title<T>(this T control, string title) where T : FuchsSelectBase
	{
		control.Title = title;
		return control;
	}

	public static FuchsEnumSelect EnumType(this FuchsEnumSelect control, Type? enumType)
	{
		control.EnumType = enumType;
		return control;
	}

	public static FuchsContainer ContainerSize(this FuchsContainer control, FuchsContainerSize size)
	{
		control.ContainerSize = size;
		return control;
	}

	public static FuchsForm Spacing(this FuchsForm control, double spacing)
	{
		control.Spacing = spacing;
		return control;
	}

	public static FuchsCard CardBackgroundColor(this FuchsCard control, Color? color)
	{
		control.CardBackgroundColor = color;
		return control;
	}

	public static FuchsCard CardBorderColor(this FuchsCard control, Color? color)
	{
		control.CardBorderColor = color;
		return control;
	}

	public static FuchsCard Color(this FuchsCard control, FuchsThemeColor? color)
	{
		control.Color = color;
		return control;
	}

	public static FuchsAccordion IsMultipleExpansionEnabled(this FuchsAccordion control, bool enabled = true)
	{
		control.IsMultipleExpansionEnabled = enabled;
		return control;
	}

	public static FuchsAccordionItem Header(this FuchsAccordionItem control, string header)
	{
		control.Header = header;
		return control;
	}

	public static FuchsAccordionItem HeaderFormattedText(this FuchsAccordionItem control, FormattedString? formattedText)
	{
		control.HeaderFormattedText = formattedText;
		return control;
	}

	public static FuchsAccordionItem HeaderView(this FuchsAccordionItem control, View? view)
	{
		control.HeaderView = view;
		return control;
	}

	public static FuchsAccordionItem Content(this FuchsAccordionItem control, View? content)
	{
		control.Content = content;
		return control;
	}

	public static FuchsAccordionItem IsExpanded(this FuchsAccordionItem control, bool isExpanded = true)
	{
		control.IsExpanded = isExpanded;
		return control;
	}

	public static FuchsAccordionItem IsEnabled(this FuchsAccordionItem control, bool isEnabled = true)
	{
		control.IsEnabled = isEnabled;
		return control;
	}

	public static FuchsTab Header(this FuchsTab control, string header)
	{
		control.Header = header;
		return control;
	}

	public static FuchsTab HeaderFormattedText(this FuchsTab control, FormattedString? formattedText)
	{
		control.HeaderFormattedText = formattedText;
		return control;
	}

	public static FuchsTab HeaderView(this FuchsTab control, View? view)
	{
		control.HeaderView = view;
		return control;
	}

	public static FuchsTab Content(this FuchsTab control, View? content)
	{
		control.Content = content;
		return control;
	}

	public static FuchsTab IsEnabled(this FuchsTab control, bool isEnabled = true)
	{
		control.IsEnabled = isEnabled;
		return control;
	}

	public static FuchsTabs SelectedIndex(this FuchsTabs control, int selectedIndex)
	{
		control.SelectedIndex = selectedIndex;
		return control;
	}

	public static FuchsTabs IsScrollable(this FuchsTabs control, bool isScrollable = true)
	{
		control.IsScrollable = isScrollable;
		return control;
	}

	public static FuchsTabs IsBordered(this FuchsTabs control, bool isBordered = true)
	{
		control.IsBordered = isBordered;
		return control;
	}

	public static FuchsTabs LineAtTop(this FuchsTabs control, bool lineAtTop = true)
	{
		control.LineAtTop = lineAtTop;
		return control;
	}

	public static FuchsTabs TabAnimation(this FuchsTabs control, FuchsTabAnimation animation)
	{
		control.TabAnimation = animation;
		return control;
	}

	public static T MeasurementMode<T>(this T control, FuchsResponsiveMeasurementMode measurementMode) where T : FuchsResponsiveBase
	{
		control.MeasurementMode = measurementMode;
		return control;
	}

	public static FuchsBreakpoint Breakpoint(this FuchsBreakpoint control, FuchsBreakpointName breakpoint)
	{
		control.Breakpoint = breakpoint;
		return control;
	}

	public static FuchsBreakpoint MatchMode(this FuchsBreakpoint control, FuchsBreakpointMatchMode matchMode)
	{
		control.MatchMode = matchMode;
		return control;
	}

	public static FuchsIdiom Idiom(this FuchsIdiom control, DeviceIdiom idiom)
	{
		control.Idiom = idiom;
		return control;
	}

	public static FuchsIdiom Idioms(this FuchsIdiom control, ObservableCollection<DeviceIdiom> idioms)
	{
		control.Idioms = idioms;
		return control;
	}

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

	public static FuchsSwitch ApplyFuchsSwitchStyle(this FuchsSwitch control)
	{
		return control.ApplyFuchsStyle("FuchsSwitchStyle");
	}

	public static FuchsCheckbox ApplyFuchsCheckboxStyle(this FuchsCheckbox checkBox)
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
}